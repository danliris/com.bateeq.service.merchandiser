    using Com.Bateeq.Service.Merchandiser.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System.Linq.Dynamic.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Com.Moonlay.NetCore.Lib;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Com.Bateeq.Service.Merchandiser.Lib.Interfaces;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using Com.Moonlay.NetCore.Lib.Service;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class CostCalculationRetailService : BasicService<MerchandiserDbContext, CostCalculationRetail>, IMap<CostCalculationRetail, CostCalculationRetailViewModel>
    {
        public CostCalculationRetailService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<CostCalculationRetail>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            IQueryable<CostCalculationRetail> Query = this.DbContext.CostCalculationRetails;

            List<string> SearchAttributes = new List<string>()
                {
                    "Article", "RO", "Style", "Counter"
                };
            Query = ConfigureSearch(Query, SearchAttributes, Keyword);

            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code", "RO", "Article", "Style", "Counter"
                };
            Query = Query
                .Select(ccr => new CostCalculationRetail
                {
                    Id = ccr.Id,
                    Code = ccr.Code,
                    RO = ccr.RO,
                    Article = ccr.Article,
                    StyleId = ccr.StyleId,
                    StyleName = ccr.StyleName,
                    CounterId = ccr.StyleId,
                    CounterName = ccr.CounterName
                });

            Dictionary<string, string> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);

            Pageable<CostCalculationRetail> pageable = new Pageable<CostCalculationRetail>(Query, Page - 1, Size);
            List<CostCalculationRetail> Data = pageable.Data.ToList<CostCalculationRetail>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public override async Task<int> CreateModel(CostCalculationRetail Model)
        {
            int created = 0;
            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    int latestSN = this.DbSet
                        .Where(d => d.SeasonId.Equals(Model.SeasonId))
                        .DefaultIfEmpty()
                        .Max(d => d.SerialNumber);
                    Model.SerialNumber = latestSN != 0 ? ++latestSN : 1;
                    Model.RO = String.Format("{0}{1:D4}", Model.SeasonCode, Model.SerialNumber);
                    created = await this.CreateAsync(Model);
                    transaction.Commit();
                }
                catch (ServiceValidationExeption e)
                {
                    throw new ServiceValidationExeption(e.ValidationContext, e.ValidationResults);

                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            return created;
        }

        public override async Task<CostCalculationRetail> ReadModelById(int id)
        {
            return await this.DbSet
                .Where(d => d.Id.Equals(id) && d._IsDeleted.Equals(false))
                .Include(d => d.CostCalculationRetail_Materials)
                .FirstOrDefaultAsync();
        }

        public override async Task<int> UpdateModel(int Id, CostCalculationRetail Model)
        {
            CostCalculationRetail_MaterialService costCalculationRetail_MaterialService = this.ServiceProvider.GetService<CostCalculationRetail_MaterialService>();

            int updated = 0;
            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    HashSet<int> costCalculationRetail_Materials = new HashSet<int>(costCalculationRetail_MaterialService.DbSet
                        .Where(p => p.CostCalculationRetailId.Equals(Id))
                        .Select(p => p.Id));
                    updated = await this.UpdateAsync(Id, Model);

                    foreach (int costCalculationRetail_Material in costCalculationRetail_Materials)
                    {
                        CostCalculationRetail_Material model = Model.CostCalculationRetail_Materials.FirstOrDefault(prop => prop.Id.Equals(costCalculationRetail_Material));

                        if (model == null)
                        {
                            await costCalculationRetail_MaterialService.DeleteModel(costCalculationRetail_Material);
                        }
                    }

                    foreach (CostCalculationRetail_Material costCalculationRetail_Material in Model.CostCalculationRetail_Materials)
                    {
                        if (costCalculationRetail_Material.Id.Equals(0))
                        {
                            await costCalculationRetail_MaterialService.CreateModel(costCalculationRetail_Material);
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }

            return updated;
        }

        public override async Task<int> DeleteModel(int Id)
        {
            CostCalculationRetail_MaterialService costCalculationRetail_MaterialService = this.ServiceProvider.GetService<CostCalculationRetail_MaterialService>();

            int deleted = 0;
            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    deleted = await this.DeleteAsync(Id);

                    HashSet<int> costCalculationRetail_Materials = new HashSet<int>(costCalculationRetail_MaterialService.DbSet
                        .Where(p => p.CostCalculationRetailId.Equals(Id))
                        .Select(p => p.Id));
                    foreach (int costCalculationRetail_Material in costCalculationRetail_Materials)
                    {
                        await costCalculationRetail_MaterialService.DeleteModel(costCalculationRetail_Material);
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }

            return deleted;
        }

        public override void OnCreating(CostCalculationRetail model)
        {
            do
            {
                model.Code = CodeGenerator.GenerateCode();
            }
            while (this.DbSet.Any(sr => sr.Code.Equals(model.Code)));

            if (model.CostCalculationRetail_Materials.Count > 0)
            {
                CostCalculationRetail_MaterialService costCalculationRetail_MaterialService = this.ServiceProvider.GetService<CostCalculationRetail_MaterialService>();
                foreach (CostCalculationRetail_Material costCalculationRetail_Material in model.CostCalculationRetail_Materials)
                {
                    costCalculationRetail_MaterialService.OnCreating(costCalculationRetail_Material);
                }
            }

            base.OnCreating(model);
        }

        public CostCalculationRetailViewModel MapToViewModel(CostCalculationRetail model)
        {
            CostCalculationRetailViewModel viewModel = new CostCalculationRetailViewModel();
            viewModel.CostCalculationRetail_Materials = new List<CostCalculationRetailViewModel.CostCalculationRetail_MaterialVM>();

            PropertyCopier<CostCalculationRetail, CostCalculationRetailViewModel>.Copy(model, viewModel);

            viewModel.Style = new CostCalculationRetailViewModel.StyleVM();
            viewModel.Style._id = model.StyleId;
            viewModel.Style.name = model.StyleName;

            viewModel.Season = new CostCalculationRetailViewModel.SeasonVM();
            viewModel.Season._id = model.SeasonId;
            viewModel.Season.name = model.SeasonName;

            viewModel.Buyer = new CostCalculationRetailViewModel.BuyerVM();
            viewModel.Buyer.Id = model.BuyerId;
            viewModel.Buyer.Name = model.BuyerName;

            viewModel.SizeRange = new CostCalculationRetailViewModel.SizeRangeVM();
            viewModel.SizeRange.Id = model.SizeRangeId;
            viewModel.SizeRange.Name = model.SizeRangeName;

            viewModel.Counter = new CostCalculationRetailViewModel.CounterVM();
            viewModel.Counter._id = model.CounterId;
            viewModel.Counter.name = model.CounterName;

            viewModel.SH_Cutting = model.SH_Cutting;
            viewModel.SH_Sewing = model.SH_Sewing;
            viewModel.SH_Finishing = model.SH_Finishing;
            viewModel.Quantity = model.Quantity;

            viewModel.Efficiency = new CostCalculationRetailViewModel.EfficiencyVM();
            viewModel.Efficiency.Id = model.EfficiencyId;
            viewModel.Efficiency.Value = model.EfficiencyValue;

            viewModel.Risk = model.Risk * 100;

            viewModel.OL = new CostCalculationRetailViewModel.OLVM();
            viewModel.OL.Id = model.OLId;
            viewModel.OL.Rate = model.OLRate;
            viewModel.OL.CalculatedRate = model.OLCalculatedRate;

            viewModel.OTL1 = new CostCalculationRetailViewModel.OTL1VM();
            viewModel.OTL1.Id = model.OTL1Id;
            viewModel.OTL1.Rate = model.OTL1Rate;
            viewModel.OTL1.CalculatedRate = model.OTL1CalculatedRate;

            viewModel.OTL2 = new CostCalculationRetailViewModel.OTL2VM();
            viewModel.OTL2.Id = model.OTL2Id;
            viewModel.OTL2.Rate = model.OTL2Rate;
            viewModel.OTL2.CalculatedRate = model.OTL2CalculatedRate;

            viewModel.OTL3 = new CostCalculationRetailViewModel.OTL3VM();
            viewModel.OTL3.Id = model.OTL3Id;
            viewModel.OTL3.Rate = model.OTL3Rate;
            viewModel.OTL3.CalculatedRate = model.OTL3CalculatedRate;

            viewModel.HPP = model.HPP;
            viewModel.WholesalePrice = model.WholesalePrice;

            viewModel.Proposed20 = model.Proposed20;
            viewModel.Proposed21 = model.Proposed21;
            viewModel.Proposed22 = model.Proposed22;
            viewModel.Proposed23 = model.Proposed23;
            viewModel.Proposed24 = model.Proposed24;
            viewModel.Proposed25 = model.Proposed25;
            viewModel.Proposed26 = model.Proposed26;
            viewModel.Proposed27 = model.Proposed27;
            viewModel.Proposed28 = model.Proposed28;
            viewModel.Proposed29 = model.Proposed29;
            viewModel.Proposed30 = model.Proposed30;

            viewModel.Rounding20 = model.Rounding20;
            viewModel.Rounding21 = model.Rounding21;
            viewModel.Rounding22 = model.Rounding22;
            viewModel.Rounding23 = model.Rounding23;
            viewModel.Rounding24 = model.Rounding24;
            viewModel.Rounding25 = model.Rounding25;
            viewModel.Rounding26 = model.Rounding26;
            viewModel.Rounding27 = model.Rounding27;
            viewModel.Rounding28 = model.Rounding28;
            viewModel.Rounding29 = model.Rounding29;
            viewModel.Rounding30 = model.Rounding30;
            viewModel.RoundingOthers = model.RoundingOthers;

            if (model.CostCalculationRetail_Materials != null)
            {
                foreach (CostCalculationRetail_Material costCalculationRetail_Material in model.CostCalculationRetail_Materials)
                {
                    CostCalculationRetailViewModel.CostCalculationRetail_MaterialVM costCalculationRetail_MaterialVM = new CostCalculationRetailViewModel.CostCalculationRetail_MaterialVM();
                    PropertyCopier<CostCalculationRetail_Material, CostCalculationRetailViewModel.CostCalculationRetail_MaterialVM>.Copy(costCalculationRetail_Material, costCalculationRetail_MaterialVM);

                    costCalculationRetail_MaterialVM.Id = costCalculationRetail_Material.Id;
                    
                    CostCalculationRetailViewModel.CostCalculationRetail_MaterialVM.CategoryVM categoryVM = new CostCalculationRetailViewModel.CostCalculationRetail_MaterialVM.CategoryVM()
                    {
                        Id = costCalculationRetail_Material.CategoryId
                    };
                    string[] names = costCalculationRetail_Material.CategoryName.Split(new[] { " - " }, StringSplitOptions.None);
                    categoryVM.Name = names[0];
                    try
                    {
                        categoryVM.SubCategory = names[1];
                    }
                    catch(IndexOutOfRangeException)
                    {
                        categoryVM.SubCategory = null;
                    }
                    costCalculationRetail_MaterialVM.Category = categoryVM;
                    CostCalculationRetailViewModel.CostCalculationRetail_MaterialVM.MaterialVM materialVM = new CostCalculationRetailViewModel.CostCalculationRetail_MaterialVM.MaterialVM()
                    {
                        Id = costCalculationRetail_Material.MaterialId,
                        Name = costCalculationRetail_Material.MaterialName
                    };
                    costCalculationRetail_MaterialVM.Material = materialVM;

                    costCalculationRetail_MaterialVM.Quantity = costCalculationRetail_Material.Quantity;
                    CostCalculationRetailViewModel.CostCalculationRetail_MaterialVM.UOMQuantityVM uomQuantityVM = new CostCalculationRetailViewModel.CostCalculationRetail_MaterialVM.UOMQuantityVM()
                    {
                        Id = costCalculationRetail_Material.UOMQuantityId,
                        Name = costCalculationRetail_Material.UOMQuantityName
                    };
                    costCalculationRetail_MaterialVM.UOMQuantity = uomQuantityVM;

                    costCalculationRetail_MaterialVM.Price = costCalculationRetail_Material.Price;
                    CostCalculationRetailViewModel.CostCalculationRetail_MaterialVM.UOMPriceVM uomPriceVM = new CostCalculationRetailViewModel.CostCalculationRetail_MaterialVM.UOMPriceVM()
                    {
                        Id = costCalculationRetail_Material.UOMPriceId,
                        Name = costCalculationRetail_Material.UOMPriceName
                    };
                    costCalculationRetail_MaterialVM.UOMPrice = uomPriceVM;

                    costCalculationRetail_MaterialVM.Conversion = costCalculationRetail_Material.Conversion;
                    costCalculationRetail_MaterialVM.Total = costCalculationRetail_Material.Total;

                    viewModel.CostCalculationRetail_Materials.Add(costCalculationRetail_MaterialVM);
                }
            }

            return viewModel;
        }

        public CostCalculationRetail MapToModel(CostCalculationRetailViewModel viewModel)
        {
            CostCalculationRetail model = new CostCalculationRetail();
            model.CostCalculationRetail_Materials = new List<CostCalculationRetail_Material>();

            PropertyCopier<CostCalculationRetailViewModel, CostCalculationRetail>.Copy(viewModel, model);

            model.StyleId = viewModel.Style._id;
            model.StyleName = viewModel.Style.name;
            model.SeasonId = viewModel.Season._id;
            model.SeasonCode = viewModel.Season.code;
            model.SeasonName = viewModel.Season.name;
            model.CounterId = viewModel.Counter._id;
            model.CounterName = viewModel.Counter.name;
            model.BuyerId = viewModel.Buyer.Id != null ? (int)viewModel.Buyer.Id : 0;
            model.BuyerName = viewModel.Buyer.Name;
            model.SizeRangeId = viewModel.SizeRange.Id != null ? (int)viewModel.SizeRange.Id : 0;
            model.SizeRangeName = viewModel.SizeRange.Name;
            model.SH_Cutting = viewModel.SH_Cutting != null ? (double)viewModel.SH_Cutting : 0;
            model.SH_Sewing = viewModel.SH_Sewing != null ? (double)viewModel.SH_Sewing : 0;
            model.SH_Finishing = viewModel.SH_Finishing != null ? (double)viewModel.SH_Finishing : 0;
            model.Quantity = viewModel.Quantity != null ? (int)viewModel.Quantity : 0;
            model.EfficiencyId = viewModel.Efficiency.Id != null ? (int)viewModel.Efficiency.Id : 0;
            model.EfficiencyValue = viewModel.Efficiency.Value != null ? (double)viewModel.Efficiency.Value : 0;
            model.Risk = viewModel.Risk != null ? (double)viewModel.Risk / 100 : 0;

            model.OLId = viewModel.OL.Id != null ? (int)viewModel.OL.Id : 0;
            model.OLRate = viewModel.OL.Rate != null ? (double)viewModel.OL.Rate : 0;
            model.OLCalculatedRate = viewModel.OL.CalculatedRate != null ? (double)viewModel.OL.CalculatedRate : 0;
            model.OTL1Id = viewModel.OTL1.Id != null ? (int)viewModel.OTL1.Id : 0;
            model.OTL1Rate = viewModel.OTL1.Rate != null ? (double)viewModel.OTL1.Rate : 0;
            model.OTL1CalculatedRate = viewModel.OTL1.CalculatedRate != null ? (double)viewModel.OTL1.CalculatedRate : 0;
            model.OTL2Id = viewModel.OTL2.Id != null ? (int)viewModel.OTL2.Id : 0;
            model.OTL2Rate = viewModel.OTL2.Rate != null ? (double)viewModel.OTL2.Rate : 0;
            model.OTL2CalculatedRate = viewModel.OTL2.CalculatedRate != null ? (double)viewModel.OTL2.CalculatedRate : 0;
            model.OTL3Id = viewModel.OTL3.Id != null ? (int)viewModel.OTL3.Id : 0;
            model.OTL3Rate = viewModel.OTL3.Rate != null ? (double)viewModel.OTL3.Rate : 0;
            model.OTL3CalculatedRate = viewModel.OTL3.CalculatedRate != null ? (double)viewModel.OTL3.CalculatedRate : 0;

            model.HPP = viewModel.HPP != null ? (double)viewModel.HPP : 0;
            model.WholesalePrice = viewModel.WholesalePrice != null ? (double)viewModel.WholesalePrice : 0;

            model.Proposed20 = viewModel.Proposed20 != null ? (double)viewModel.Proposed20 : 0;
            model.Proposed21 = viewModel.Proposed21 != null ? (double)viewModel.Proposed21 : 0;
            model.Proposed22 = viewModel.Proposed22 != null ? (double)viewModel.Proposed22 : 0;
            model.Proposed23 = viewModel.Proposed23 != null ? (double)viewModel.Proposed23 : 0;
            model.Proposed24 = viewModel.Proposed24 != null ? (double)viewModel.Proposed24 : 0;
            model.Proposed25 = viewModel.Proposed25 != null ? (double)viewModel.Proposed25 : 0;
            model.Proposed26 = viewModel.Proposed26 != null ? (double)viewModel.Proposed26 : 0;
            model.Proposed27 = viewModel.Proposed27 != null ? (double)viewModel.Proposed27 : 0;
            model.Proposed28 = viewModel.Proposed28 != null ? (double)viewModel.Proposed28 : 0;
            model.Proposed29 = viewModel.Proposed29 != null ? (double)viewModel.Proposed29 : 0;
            model.Proposed30 = viewModel.Proposed30 != null ? (double)viewModel.Proposed30 : 0;

            model.Rounding20 = viewModel.Rounding20 != null ? (double)viewModel.Rounding20 : 0;
            model.Rounding21 = viewModel.Rounding21 != null ? (double)viewModel.Rounding21 : 0;
            model.Rounding22 = viewModel.Rounding22 != null ? (double)viewModel.Rounding22 : 0;
            model.Rounding23 = viewModel.Rounding23 != null ? (double)viewModel.Rounding23 : 0;
            model.Rounding24 = viewModel.Rounding24 != null ? (double)viewModel.Rounding24 : 0;
            model.Rounding25 = viewModel.Rounding25 != null ? (double)viewModel.Rounding25 : 0;
            model.Rounding26 = viewModel.Rounding26 != null ? (double)viewModel.Rounding26 : 0;
            model.Rounding27 = viewModel.Rounding27 != null ? (double)viewModel.Rounding27 : 0;
            model.Rounding28 = viewModel.Rounding28 != null ? (double)viewModel.Rounding28 : 0;
            model.Rounding29 = viewModel.Rounding29 != null ? (double)viewModel.Rounding29 : 0;
            model.Rounding30 = viewModel.Rounding30 != null ? (double)viewModel.Rounding30 : 0;
            model.RoundingOthers = viewModel.RoundingOthers != null ? (double)viewModel.RoundingOthers : 0;

            foreach (CostCalculationRetailViewModel.CostCalculationRetail_MaterialVM costCalculationRetail_MaterialVM in viewModel.CostCalculationRetail_Materials)
            {
                CostCalculationRetail_Material costCalculationRetail_Material = new CostCalculationRetail_Material();
                PropertyCopier<CostCalculationRetailViewModel.CostCalculationRetail_MaterialVM, CostCalculationRetail_Material>.Copy(costCalculationRetail_MaterialVM, costCalculationRetail_Material);

                costCalculationRetail_Material.CategoryId = costCalculationRetail_MaterialVM.Category.Id != null ? (int)costCalculationRetail_MaterialVM.Category.Id : 0;
                costCalculationRetail_Material.CategoryName = costCalculationRetail_MaterialVM.Category.SubCategory != null ?costCalculationRetail_MaterialVM.Category.Name + " - " + costCalculationRetail_MaterialVM.Category.SubCategory : costCalculationRetail_MaterialVM.Category.Name;
                costCalculationRetail_Material.MaterialId = costCalculationRetail_MaterialVM.Material.Id != null ? (int)costCalculationRetail_MaterialVM.Material.Id : 0;
                costCalculationRetail_Material.MaterialName = costCalculationRetail_MaterialVM.Material.Name;
                costCalculationRetail_Material.Quantity = costCalculationRetail_MaterialVM.Quantity != null ? (double)costCalculationRetail_MaterialVM.Quantity : 0;
                costCalculationRetail_Material.UOMQuantityId = costCalculationRetail_MaterialVM.UOMQuantity.Id != null ? (int)costCalculationRetail_MaterialVM.UOMQuantity.Id : 0;
                costCalculationRetail_Material.UOMQuantityName = costCalculationRetail_MaterialVM.UOMQuantity.Name;
                costCalculationRetail_Material.Price = costCalculationRetail_MaterialVM.Price != null ? (double)costCalculationRetail_MaterialVM.Price : 0;
                costCalculationRetail_Material.UOMPriceId = costCalculationRetail_MaterialVM.UOMPrice.Id != null ? (int)costCalculationRetail_MaterialVM.UOMPrice.Id : 0;
                costCalculationRetail_Material.UOMPriceName = costCalculationRetail_MaterialVM.UOMPrice.Name;
                costCalculationRetail_Material.Conversion = costCalculationRetail_MaterialVM.Conversion != null ? (double)costCalculationRetail_MaterialVM.Conversion : 0;
                costCalculationRetail_Material.Total = costCalculationRetail_MaterialVM.Total != null ? (double)costCalculationRetail_MaterialVM.Total : 0;

                model.CostCalculationRetail_Materials.Add(costCalculationRetail_Material);
            }

            return model;
        }
    }
}
