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
using Com.Bateeq.Service.Merchandiser.Lib.Services.AzureStorage;
using Com.Bateeq.Service.Merchandiser.Lib.Exceptions;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class CostCalculationGarmentService : BasicService<MerchandiserDbContext, CostCalculationGarment>, IMap<CostCalculationGarment, CostCalculationGarmentViewModel>
    {
        public CostCalculationGarmentService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        private AzureImageService AzureImageService
        {
            get { return this.ServiceProvider.GetService<AzureImageService>(); }
        }

        private CostCalculationGarment_MaterialService CostCalculationGarment_MaterialService
        {
            get
            {
                CostCalculationGarment_MaterialService service = this.ServiceProvider.GetService<CostCalculationGarment_MaterialService>();
                service.Username = this.Username;
                return service;
            }
        }

        public override Tuple<List<CostCalculationGarment>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            IQueryable<CostCalculationGarment> Query = this.DbContext.CostCalculationGarments;

            List<string> SearchAttributes = new List<string>()
                {
                    "RO", "Article"
                };
            Query = ConfigureSearch(Query, SearchAttributes, Keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);

            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code", "RO", "Article", "Line", "Quantity", "ConfirmPrice"
                };
            Query = Query
                .Select(ccg => new CostCalculationGarment
                {
                    Id = ccg.Id,
                    Code = ccg.Code,
                    RO = ccg.RO,
                    Article = ccg.Article,
                    LineId = ccg.LineId,
                    LineName = ccg.LineName,
                    Quantity = ccg.Quantity,
                    ConfirmPrice = ccg.ConfirmPrice
                });

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);

            Pageable<CostCalculationGarment> pageable = new Pageable<CostCalculationGarment>(Query, Page - 1, Size);
            List<CostCalculationGarment> Data = pageable.Data.ToList<CostCalculationGarment>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public override async Task<int> CreateModel(CostCalculationGarment Model)
        {
            int latestSN = this.DbSet
                .Where(d => d.LineId.Equals(Model.LineId))
                .DefaultIfEmpty()
                .Max(d => d.SerialNumber);
            Model.SerialNumber = latestSN != 0 ? latestSN + 1 : 1;
            Model.RO = String.Format("{0}{1:D4}", Model.LineCode, Model.SerialNumber);
            int created = await this.CreateAsync(Model);
            
            Model.ImagePath = await this.AzureImageService.UploadImage(Model.GetType().Name, Model.Id, Model._CreatedUtc, Model.ImageFile);

            await this.UpdateAsync(Model.Id, Model);

            return created;
        }

        public override async Task<CostCalculationGarment> ReadModelById(int id)
        {
            CostCalculationGarment read = await this.DbSet
                .Where(d => d.Id.Equals(id) && d._IsDeleted.Equals(false))
                .Include(d => d.CostCalculationGarment_Materials)
                .FirstOrDefaultAsync();

            read.ImageFile = await this.AzureImageService.DownloadImage(read.GetType().Name, read.ImagePath);
            
            return read;
        }

        public override async Task<int> UpdateModel(int Id, CostCalculationGarment Model)
        {
            Model.ImagePath = await this.AzureImageService.UploadImage(Model.GetType().Name, Model.Id, Model._CreatedUtc, Model.ImageFile);

            int updated = await this.UpdateAsync(Id, Model);

            if (Model.CostCalculationGarment_Materials != null)
            {
                HashSet<int> CostCalculationGarment_Materials = new HashSet<int>(this.CostCalculationGarment_MaterialService.DbSet
                    .Where(p => p.CostCalculationGarmentId.Equals(Id))
                    .Select(p => p.Id));
                foreach (int CostCalculationGarment_Material in CostCalculationGarment_Materials)
                {
                    CostCalculationGarment_Material model = Model.CostCalculationGarment_Materials.FirstOrDefault(prop => prop.Id.Equals(CostCalculationGarment_Material));

                    if (model == null)
                    {
                        await this.CostCalculationGarment_MaterialService.DeleteModel(CostCalculationGarment_Material);
                    }
                    else
                    {
                        await this.CostCalculationGarment_MaterialService.UpdateModel(CostCalculationGarment_Material, model);
                    }
                }

                foreach (CostCalculationGarment_Material CostCalculationGarment_Material in Model.CostCalculationGarment_Materials)
                {
                    if (CostCalculationGarment_Material.Id.Equals(0))
                    {
                        await this.CostCalculationGarment_MaterialService.CreateModel(CostCalculationGarment_Material);
                    }
                }
            }

            return updated;
        }

        public override async Task<int> DeleteModel(int Id)
        {
            CostCalculationGarment deleted = await this.GetAsync(Id);

            if (deleted.RO_GarmentId != null)
            {
                throw new DbReferenceNotNullException("Cost Calculation Garment ini tidak bisa di delete karena masih terdaftar di RO");
            }
            else
            {
                HashSet<int> CostCalculationGarment_Materials = new HashSet<int>(this.CostCalculationGarment_MaterialService.DbSet
                    .Where(p => p.CostCalculationGarmentId.Equals(Id))
                    .Select(p => p.Id));

                foreach (int CostCalculationGarment_Material in CostCalculationGarment_Materials)
                {
                    await this.CostCalculationGarment_MaterialService.DeleteModel(CostCalculationGarment_Material);
                }
                
                await this.AzureImageService.RemoveImage(deleted.GetType().Name, deleted.ImagePath);
            }
            
            return await this.DeleteAsync(Id);
        }

        public override void OnCreating(CostCalculationGarment model)
        {
            do
            {
                model.Code = Code.Generate();
            }
            while (this.DbSet.Any(sr => sr.Code.Equals(model.Code)));

            if (model.CostCalculationGarment_Materials.Count > 0)
            {
                
                foreach (CostCalculationGarment_Material CostCalculationGarment_Material in model.CostCalculationGarment_Materials)
                {
                    this.CostCalculationGarment_MaterialService.OnCreating(CostCalculationGarment_Material);
                }
            }

            base.OnCreating(model);
        }

        public CostCalculationGarmentViewModel MapToViewModel(CostCalculationGarment model)
        {
            CostCalculationGarmentViewModel viewModel = new CostCalculationGarmentViewModel();
            PropertyCopier<CostCalculationGarment, CostCalculationGarmentViewModel>.Copy(model, viewModel);

            viewModel.Line = new LineViewModel();
            viewModel.Line.Id = model.LineId;
            viewModel.Line.Name = model.LineName;

            viewModel.FabricAllowance = Percentage.ToPercent(model.FabricAllowance);
            viewModel.AccessoriesAllowance = Percentage.ToPercent(model.AccessoriesAllowance);

            viewModel.SizeRange = new SizeRangeViewModel();
            viewModel.SizeRange.Id = model.SizeRangeId;
            viewModel.SizeRange.Name = model.SizeRangeName;

            viewModel.Buyer = new BuyerViewModel();
            viewModel.Buyer.Id = model.BuyerId;
            viewModel.Buyer.Name = model.BuyerName;

            viewModel.Efficiency = new EfficiencyViewModel();
            viewModel.Efficiency.Id = model.EfficiencyId;
            viewModel.Efficiency.Value = Percentage.ToPercent(model.EfficiencyValue);

            viewModel.Wage = new RateViewModel();
            viewModel.Wage.Id = model.WageId;
            viewModel.Wage.Value = model.WageRate;

            viewModel.THR = new RateViewModel();
            viewModel.THR.Id = model.THRId;
            viewModel.THR.Value = model.THRRate;

            viewModel.Rate = new RateViewModel();
            viewModel.Rate.Id = model.RateId;
            viewModel.Rate.Value = model.RateValue;

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

                    CostCalculationGarment_MaterialVM.ShippingFeePortion = Percentage.ToPercent(CostCalculationGarment_Material.ShippingFeePortion);

                    viewModel.CostCalculationGarment_Materials.Add(CostCalculationGarment_MaterialVM);
                }
            }

            viewModel.CommissionPortion = Percentage.ToPercent(model.CommissionPortion);
            viewModel.Risk = Percentage.ToPercent(model.Risk);

            viewModel.OTL1 = new RateCalculatedViewModel();
            viewModel.OTL1.Id = model.OTL1Id;
            viewModel.OTL1.Value = model.OTL1Rate;
            viewModel.OTL1.CalculatedValue = model.OTL1CalculatedRate;

            viewModel.OTL2 = new RateCalculatedViewModel();
            viewModel.OTL2.Id = model.OTL2Id;
            viewModel.OTL2.Value = model.OTL2Rate;
            viewModel.OTL2.CalculatedValue = model.OTL2CalculatedRate;

            viewModel.NETFOBP = Percentage.ToPercent((double)model.NETFOBP);

            return viewModel;
        }

        public CostCalculationGarment MapToModel(CostCalculationGarmentViewModel viewModel)
        {
            CostCalculationGarment model = new CostCalculationGarment();
            PropertyCopier<CostCalculationGarmentViewModel, CostCalculationGarment>.Copy(viewModel, model);

            model.LineId = viewModel.Line.Id;
            model.LineCode = viewModel.Line.Code;
            model.LineName = viewModel.Line.Name;

            model.FabricAllowance = Percentage.ToFraction(viewModel.FabricAllowance);
            model.AccessoriesAllowance = Percentage.ToFraction(viewModel.AccessoriesAllowance);

            model.SizeRangeId = viewModel.SizeRange.Id;
            model.SizeRangeName = viewModel.SizeRange.Name;

            model.BuyerId = viewModel.Buyer.Id;
            model.BuyerName = viewModel.Buyer.Name;

            model.EfficiencyId = viewModel.Efficiency.Id;
            model.EfficiencyValue = Percentage.ToFraction(viewModel.Efficiency.Value);

            model.WageId = viewModel.Wage.Id;
            model.WageRate = viewModel.Wage.Value != null ? (double)viewModel.Wage.Value : 0;

            model.THRId = viewModel.THR.Id;
            model.THRRate = viewModel.THR.Value != null ? (double)viewModel.THR.Value : 0;

            model.RateId = viewModel.Rate.Id;
            model.RateValue = viewModel.Rate.Value != null ? (double)viewModel.Rate.Value : 0;

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
                CostCalculationGarment_Material.ShippingFeePortion = Percentage.ToFraction(CostCalculationGarment_MaterialVM.ShippingFeePortion);

                model.CostCalculationGarment_Materials.Add(CostCalculationGarment_Material);
            }

            model.CommissionPortion = Percentage.ToFraction(viewModel.CommissionPortion);
            model.Risk = Percentage.ToFraction(viewModel.Risk);

            model.OTL1Id = viewModel.OTL1.Id;
            model.OTL1Rate = viewModel.OTL1.Value != null ? (double)viewModel.OTL1.Value : 0;
            model.OTL1CalculatedRate = viewModel.OTL1.CalculatedValue != null ? (double)viewModel.OTL1.CalculatedValue : 0;

            model.OTL2Id = viewModel.OTL2.Id;
            model.OTL2Rate = viewModel.OTL2.Value != null ? (double)viewModel.OTL2.Value : 0;
            model.OTL2CalculatedRate = viewModel.OTL2.CalculatedValue != null ? (double)viewModel.OTL2.CalculatedValue : 0;

            model.NETFOBP = Percentage.ToFraction(viewModel.NETFOBP);

            return model;
        }
    }
}
