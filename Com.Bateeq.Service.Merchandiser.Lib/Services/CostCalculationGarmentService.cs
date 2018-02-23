using Com.Bateeq.Service.Merchandiser.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System.Linq.Dynamic.Core;
using Microsoft.Extensions.DependencyInjection;
using Com.Moonlay.NetCore.Lib;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Com.Bateeq.Service.Merchandiser.Lib.Interfaces;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using Com.Moonlay.NetCore.Lib.Service;
using System.Reflection;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class CostCalculationGarmentService : BasicService<MerchandiserDbContext, CostCalculationGarment>, IMap<CostCalculationGarment, CostCalculationGarmentViewModel>
    {
        public CostCalculationGarmentService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<CostCalculationGarment>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            IQueryable<CostCalculationGarment> Query = this.DbContext.CostCalculationGarments;

            List<string> SearchAttributes = new List<string>()
                {
                    "RO", "Article", "Convection", "Quantity", "ConfirmPrice"
                };
            Query = ConfigureSearch(Query, SearchAttributes, Keyword);

            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code", "RO", "Article", "Convection", "Quantity", "ConfirmPrice"
                };
            Query = Query
                .Select(ccg => new CostCalculationGarment
                {
                    Id = ccg.Id,
                    Code = ccg.Code,
                    RO = ccg.RO,
                    Article = ccg.Article,
                    ConvectionId = ccg.ConvectionId,
                    ConvectionName = ccg.ConvectionName,
                    Quantity = ccg.Quantity,
                    ConfirmPrice = ccg.ConfirmPrice
                });

            Dictionary<string, string> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);

            Pageable<CostCalculationGarment> pageable = new Pageable<CostCalculationGarment>(Query, Page - 1, Size);
            List<CostCalculationGarment> Data = pageable.Data.ToList<CostCalculationGarment>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public override async Task<int> CreateModel(CostCalculationGarment Model)
        {
            int created = 0;
            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    int latestSN = this.DbSet
                        .Where(d => d.ConvectionId.Equals(Model.ConvectionId))
                        .DefaultIfEmpty()
                        .Max(d => d.SerialNumber);
                    Model.SerialNumber = latestSN != 0 ? ++latestSN : 1;
                    Model.RO = String.Format("{0}{1:D4}", Model.ConvectionCode, Model.SerialNumber);
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

        public override async Task<CostCalculationGarment> ReadModelById(int id)
        {
            return await this.DbSet
                .Where(d => d.Id.Equals(id) && d._IsDeleted.Equals(false))
                .Include(d => d.CostCalculationGarment_Materials)
                .FirstOrDefaultAsync();
        }

        public override async Task<int> UpdateModel(int Id, CostCalculationGarment Model)
        {
            CostCalculationGarment_MaterialService CostCalculationGarment_MaterialService = this.ServiceProvider.GetService<CostCalculationGarment_MaterialService>();

            int updated = 0;
            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    HashSet<int> CostCalculationGarment_Materials = new HashSet<int>(CostCalculationGarment_MaterialService.DbSet
                        .Where(p => p.CostCalculationGarmentId.Equals(Id))
                        .Select(p => p.Id));
                    updated = await this.UpdateAsync(Id, Model);

                    foreach (int CostCalculationGarment_Material in CostCalculationGarment_Materials)
                    {
                        CostCalculationGarment_Material model = Model.CostCalculationGarment_Materials.FirstOrDefault(prop => prop.Id.Equals(CostCalculationGarment_Material));

                        if (model == null)
                        {
                            await CostCalculationGarment_MaterialService.DeleteModel(CostCalculationGarment_Material);
                        }
                    }

                    foreach (CostCalculationGarment_Material CostCalculationGarment_Material in Model.CostCalculationGarment_Materials)
                    {
                        if (CostCalculationGarment_Material.Id.Equals(0))
                        {
                            await CostCalculationGarment_MaterialService.CreateModel(CostCalculationGarment_Material);
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
            CostCalculationGarment_MaterialService CostCalculationGarment_MaterialService = this.ServiceProvider.GetService<CostCalculationGarment_MaterialService>();

            int deleted = 0;
            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    deleted = await this.DeleteAsync(Id);

                    HashSet<int> CostCalculationGarment_Materials = new HashSet<int>(CostCalculationGarment_MaterialService.DbSet
                        .Where(p => p.CostCalculationGarmentId.Equals(Id))
                        .Select(p => p.Id));
                    foreach (int CostCalculationGarment_Material in CostCalculationGarment_Materials)
                    {
                        await CostCalculationGarment_MaterialService.DeleteModel(CostCalculationGarment_Material);
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

        public override void OnCreating(CostCalculationGarment model)
        {
            do
            {
                model.Code = CodeGenerator.GenerateCode();
            }
            while (this.DbSet.Any(sr => sr.Code.Equals(model.Code)));

            if (model.CostCalculationGarment_Materials.Count > 0)
            {
                CostCalculationGarment_MaterialService CostCalculationGarment_MaterialService = this.ServiceProvider.GetService<CostCalculationGarment_MaterialService>();
                foreach (CostCalculationGarment_Material CostCalculationGarment_Material in model.CostCalculationGarment_Materials)
                {
                    CostCalculationGarment_MaterialService.OnCreating(CostCalculationGarment_Material);
                }
            }

            base.OnCreating(model);
        }

        public CostCalculationGarmentViewModel MapToViewModel(CostCalculationGarment model)
        {
            CostCalculationGarmentViewModel viewModel = new CostCalculationGarmentViewModel();
            PropertyCopier<CostCalculationGarment, CostCalculationGarmentViewModel>.Copy(model, viewModel);

            viewModel.Convection = new ArticleSeasonViewModel();
            viewModel.Convection._id = model.ConvectionId;
            viewModel.Convection.name = model.ConvectionName;

            viewModel.FabricAllowance = PercentageConverter.ToPercent(model.FabricAllowance);
            viewModel.AccessoriesAllowance = PercentageConverter.ToPercent(model.AccessoriesAllowance);

            viewModel.SizeRange = new SizeRangeViewModel();
            viewModel.SizeRange.Id = model.SizeRangeId;
            viewModel.SizeRange.Name = model.SizeRangeName;

            viewModel.Buyer = new BuyerViewModel();
            viewModel.Buyer.Id = model.BuyerId;
            viewModel.Buyer.Name = model.BuyerName;

            viewModel.Efficiency = new EfficiencyViewModel();
            viewModel.Efficiency.Id = model.EfficiencyId;
            viewModel.Efficiency.Value = PercentageConverter.ToPercent(model.EfficiencyValue);

            viewModel.Wage = new OTLViewModel();
            viewModel.Wage.Id = model.WageId;
            viewModel.Wage.Rate = model.WageRate;

            viewModel.THR = new OTLViewModel();
            viewModel.THR.Id = model.THRId;
            viewModel.THR.Rate = model.THRRate;

            viewModel.RateDollar = new OTLViewModel();
            viewModel.RateDollar.Id = model.RateDollarId;
            viewModel.RateDollar.Rate = model.RateDollarRate;

            viewModel.CostCalculationGarment_Materials = new List<CostCalculationGarment_MaterialViewModel>();
            if (model.CostCalculationGarment_Materials != null)
            {
                foreach (CostCalculationGarment_Material CostCalculationGarment_Material in model.CostCalculationGarment_Materials)
                {
                    CostCalculationGarment_MaterialViewModel CostCalculationGarment_MaterialVM = new CostCalculationGarment_MaterialViewModel();
                    PropertyCopier<CostCalculationGarment_Material, CostCalculationGarment_MaterialViewModel>.Copy(CostCalculationGarment_Material, CostCalculationGarment_MaterialVM);

                    CategoryViewModel categoryVM = new CategoryViewModel()
                    {
                        Id = CostCalculationGarment_Material.CategoryId
                    };
                    string[] names = CostCalculationGarment_Material.CategoryName.Split(new[] { " - " }, StringSplitOptions.None);
                    categoryVM.Name = names[0];
                    try
                    {
                        categoryVM.SubCategory = names[1];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        categoryVM.SubCategory = null;
                    }
                    CostCalculationGarment_MaterialVM.Category = categoryVM;

                    MaterialViewModel materialVM = new MaterialViewModel()
                    {
                        Id = CostCalculationGarment_Material.MaterialId,
                        Name = CostCalculationGarment_Material.MaterialName
                    };
                    CostCalculationGarment_MaterialVM.Material = materialVM;

                    UOMViewModel uomQuantityVM = new UOMViewModel()
                    {
                        Id = CostCalculationGarment_Material.UOMQuantityId,
                        Name = CostCalculationGarment_Material.UOMQuantityName
                    };
                    CostCalculationGarment_MaterialVM.UOMQuantity = uomQuantityVM;

                    UOMViewModel uomPriceVM = new UOMViewModel()
                    {
                        Id = CostCalculationGarment_Material.UOMPriceId,
                        Name = CostCalculationGarment_Material.UOMPriceName
                    };
                    CostCalculationGarment_MaterialVM.UOMPrice = uomPriceVM;

                    CostCalculationGarment_MaterialVM.ShippingFeePortion = PercentageConverter.ToPercent(CostCalculationGarment_Material.ShippingFeePortion);

                    viewModel.CostCalculationGarment_Materials.Add(CostCalculationGarment_MaterialVM);
                }
            }

            viewModel.CommissionPortion = PercentageConverter.ToPercent(model.CommissionPortion);
            viewModel.Risk = PercentageConverter.ToPercent(model.Risk);

            viewModel.OTL1 = new OTLCalculatedViewModel();
            viewModel.OTL1.Id = model.OTL1Id;
            viewModel.OTL1.Rate = model.OTL1Rate;
            viewModel.OTL1.CalculatedRate = model.OTL1CalculatedRate;

            viewModel.OTL2 = new OTLCalculatedViewModel();
            viewModel.OTL2.Id = model.OTL2Id;
            viewModel.OTL2.Rate = model.OTL2Rate;
            viewModel.OTL2.CalculatedRate = model.OTL2CalculatedRate;

            viewModel.NETFOBP = PercentageConverter.ToPercent((double)model.NETFOBP);

            return viewModel;
        }

        public CostCalculationGarment MapToModel(CostCalculationGarmentViewModel viewModel)
        {
            CostCalculationGarment model = new CostCalculationGarment();
            PropertyCopier<CostCalculationGarmentViewModel, CostCalculationGarment>.Copy(viewModel, model);

            model.ConvectionId = viewModel.Convection._id;
            model.ConvectionCode = viewModel.Convection.code;
            model.ConvectionName = viewModel.Convection.name;

            model.FabricAllowance = viewModel.FabricAllowance != null ? PercentageConverter.ToFraction((double)viewModel.FabricAllowance) : 0;
            model.AccessoriesAllowance = viewModel.AccessoriesAllowance != null ? PercentageConverter.ToFraction((double)viewModel.AccessoriesAllowance) : 0;

            model.SizeRangeId = viewModel.SizeRange.Id;
            model.SizeRangeName = viewModel.SizeRange.Name;

            model.BuyerId = viewModel.Buyer.Id;
            model.BuyerName = viewModel.Buyer.Name;

            model.EfficiencyId = viewModel.Efficiency.Id;
            model.EfficiencyValue = viewModel.Efficiency.Value != null ? PercentageConverter.ToFraction((double)viewModel.Efficiency.Value) : 0;

            model.WageId = viewModel.Wage.Id;
            model.WageRate = viewModel.Wage.Rate != null ? (double)viewModel.Wage.Rate : 0;

            model.THRId = viewModel.THR.Id;
            model.THRRate = viewModel.THR.Rate != null ? (double)viewModel.THR.Rate : 0;

            model.RateDollarId = viewModel.RateDollar.Id;
            model.RateDollarRate = viewModel.RateDollar.Rate != null ? (double)viewModel.RateDollar.Rate : 0;

            model.CostCalculationGarment_Materials = new List<CostCalculationGarment_Material>();

            foreach (CostCalculationGarment_MaterialViewModel CostCalculationGarment_MaterialVM in viewModel.CostCalculationGarment_Materials)
            {
                CostCalculationGarment_Material CostCalculationGarment_Material = new CostCalculationGarment_Material();
                PropertyCopier<CostCalculationGarment_MaterialViewModel, CostCalculationGarment_Material>.Copy(CostCalculationGarment_MaterialVM, CostCalculationGarment_Material);

                CostCalculationGarment_Material.CategoryId = CostCalculationGarment_MaterialVM.Category.Id;
                CostCalculationGarment_Material.CategoryName = CostCalculationGarment_MaterialVM.Category.SubCategory != null ? CostCalculationGarment_MaterialVM.Category.Name + " - " + CostCalculationGarment_MaterialVM.Category.SubCategory : CostCalculationGarment_MaterialVM.Category.Name;
                CostCalculationGarment_Material.MaterialId = CostCalculationGarment_MaterialVM.Material.Id;
                CostCalculationGarment_Material.MaterialName = CostCalculationGarment_MaterialVM.Material.Name;
                CostCalculationGarment_Material.UOMQuantityId = CostCalculationGarment_MaterialVM.UOMQuantity.Id;
                CostCalculationGarment_Material.UOMQuantityName = CostCalculationGarment_MaterialVM.UOMQuantity.Name;
                CostCalculationGarment_Material.UOMPriceId = CostCalculationGarment_MaterialVM.UOMPrice.Id;
                CostCalculationGarment_Material.UOMPriceName = CostCalculationGarment_MaterialVM.UOMPrice.Name;
                CostCalculationGarment_Material.ShippingFeePortion = CostCalculationGarment_MaterialVM.ShippingFeePortion != null ? PercentageConverter.ToFraction((double)CostCalculationGarment_MaterialVM.ShippingFeePortion) : 0;

                model.CostCalculationGarment_Materials.Add(CostCalculationGarment_Material);
            }

            model.CommissionPortion = viewModel.CommissionPortion != null ? PercentageConverter.ToFraction((double)viewModel.CommissionPortion) : 0;
            model.Risk = PercentageConverter.ToFraction(viewModel.Risk);

            model.OTL1Id = viewModel.OTL1.Id;
            model.OTL1Rate = viewModel.OTL1.Rate != null ? (double)viewModel.OTL1.Rate : 0;
            model.OTL1CalculatedRate = viewModel.OTL1.CalculatedRate != null ? (double)viewModel.OTL1.CalculatedRate : 0;

            model.OTL2Id = viewModel.OTL2.Id;
            model.OTL2Rate = viewModel.OTL2.Rate != null ? (double)viewModel.OTL2.Rate : 0;
            model.OTL2CalculatedRate = viewModel.OTL2.CalculatedRate != null ? (double)viewModel.OTL2.CalculatedRate : 0;

            model.NETFOBP = PercentageConverter.ToFraction(viewModel.NETFOBP);

            return model;
        }
    }
}
