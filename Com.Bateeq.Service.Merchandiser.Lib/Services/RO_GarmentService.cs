using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using Com.Bateeq.Service.Merchandiser.Lib.Interfaces;
using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Com.Moonlay.NetCore.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;
using Com.Moonlay.NetCore.Lib.Service;
using Microsoft.EntityFrameworkCore;
using Com.Bateeq.Service.Merchandiser.Lib.Services.AzureStorage;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class RO_GarmentService : BasicService<MerchandiserDbContext, RO_Garment>, IMap<RO_Garment, RO_GarmentViewModel>
    {
        public RO_GarmentService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        private AzureImageService AzureImageService
        {
            get { return this.ServiceProvider.GetService<AzureImageService>(); }
        }

        public override Tuple<List<RO_Garment>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            IQueryable<RO_Garment> Query = this.DbContext.RO_Garments;

            List<string> SearchAttributes = new List<string>()
                {
                    "Code"
                };
            Query = ConfigureSearch(Query, SearchAttributes, Keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);

            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code", "CostCalculationGarment", "Total"
                };
            Query = Query
                .Select(ro => new RO_Garment
                {
                    Id = ro.Id,
                    Code = ro.Code,
                    CostCalculationGarment = new CostCalculationGarment()
                    {
                        Id = ro.CostCalculationGarment.Id,
                        Code = ro.CostCalculationGarment.Code,
                        RO = ro.CostCalculationGarment.RO,
                        Article = ro.CostCalculationGarment.Article,
                        LineId = ro.CostCalculationGarment.LineId,
                        LineName = ro.CostCalculationGarment.LineName
                    },
                    Total = ro.Total
                });

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);

            Pageable<RO_Garment> pageable = new Pageable<RO_Garment>(Query, Page - 1, Size);
            List<RO_Garment> Data = pageable.Data.ToList<RO_Garment>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public override async Task<int> CreateModel(RO_Garment Model)
        {
            CostCalculationGarment costCalculationGarment = Model.CostCalculationGarment;
            Model.CostCalculationGarment = null;

            int created = await this.CreateAsync(Model);

            Model.ImagesPath = await this.AzureImageService.UploadMultipleImage(Model.GetType().Name, Model.Id, Model._CreatedUtc, Model.ImagesFile, Model.ImagesType, Model.ImagesPath);

            await this.UpdateAsync(Model.Id, Model);

            CostCalculationGarmentService costCalculationGarmentService = this.ServiceProvider.GetService<CostCalculationGarmentService>();
            costCalculationGarment.RO_GarmentId = Model.Id;
            await costCalculationGarmentService.UpdateModel(costCalculationGarment.Id, costCalculationGarment);

            return created;
        }

        public override void OnCreating(RO_Garment model)
        {
            do
            {
                model.Code = CodeGenerator.GenerateCode();
            }
            while (this.DbSet.Any(d => d.Code.Equals(model.Code)));

            if (model.RO_Garment_SizeBreakdowns.Count > 0)
            {
                RO_Garment_SizeBreakdownService RO_Garment_SizeBreakdownService = this.ServiceProvider.GetService<RO_Garment_SizeBreakdownService>();
                foreach (RO_Garment_SizeBreakdown RO_Garment_SizeBreakdown in model.RO_Garment_SizeBreakdowns)
                {
                    RO_Garment_SizeBreakdownService.Creating(RO_Garment_SizeBreakdown);
                }
            }

            base.OnCreating(model);
            model._CreatedAgent = "Service";
            model._CreatedBy = this.Username;
            model._LastModifiedAgent = "Service";
            model._LastModifiedBy = this.Username;
        }

        public override async Task<RO_Garment> ReadModelById(int id)
        {
            RO_Garment read = await this.DbSet
                .Where(d => d.Id.Equals(id) && d._IsDeleted.Equals(false))
                .Include(d => d.RO_Garment_SizeBreakdowns)
                    .ThenInclude(sb => sb.RO_Garment_SizeBreakdown_Details)
                .Include(d => d.CostCalculationGarment)
                    .ThenInclude(ccg => ccg.CostCalculationGarment_Materials)
                .FirstOrDefaultAsync();

            read.CostCalculationGarment.ImageFile = await this.AzureImageService.DownloadImage(read.CostCalculationGarment.GetType().Name, read.CostCalculationGarment.ImagePath);
            read.ImagesFile = await this.AzureImageService.DownloadMultipleImages(read.GetType().Name, read.ImagesPath);

            return read;
        }

        public override async Task<int> UpdateModel(int Id, RO_Garment Model)
        {
            CostCalculationGarment costCalculationGarment = Model.CostCalculationGarment;
            Model.CostCalculationGarment = null;
            
            Model.ImagesPath = await this.AzureImageService.UploadMultipleImage(Model.GetType().Name, Model.Id, Model._CreatedUtc, Model.ImagesFile, Model.ImagesType, Model.ImagesPath);

            int updated = await this.UpdateAsync(Id, Model);

            CostCalculationGarmentService costCalculationGarmentService = this.ServiceProvider.GetService<CostCalculationGarmentService>();
            costCalculationGarment.RO_GarmentId = Model.Id;
            await costCalculationGarmentService.UpdateModel(costCalculationGarment.Id, costCalculationGarment);

            return updated;
        }

        public override void OnUpdating(int id, RO_Garment model)
        {
            RO_Garment_SizeBreakdownService RO_Garment_SizeBreakdownService = this.ServiceProvider.GetService<RO_Garment_SizeBreakdownService>();
            HashSet<int> RO_Garment_SizeBreakdowns = new HashSet<int>(RO_Garment_SizeBreakdownService.DbSet
                .Where(p => p.RO_GarmentId.Equals(id))
                .Select(p => p.Id));

            foreach (int RO_Garment_SizeBreakdown in RO_Garment_SizeBreakdowns)
            {
                RO_Garment_SizeBreakdown childModel = model.RO_Garment_SizeBreakdowns.FirstOrDefault(prop => prop.Id.Equals(RO_Garment_SizeBreakdown));

                if (childModel == null)
                {
                    RO_Garment_SizeBreakdownService.Deleting(RO_Garment_SizeBreakdown);
                }
                else
                {
                    RO_Garment_SizeBreakdownService.Updating(RO_Garment_SizeBreakdown, childModel);
                }
            }

            foreach (RO_Garment_SizeBreakdown RO_Garment_SizeBreakdown in model.RO_Garment_SizeBreakdowns)
            {
                if (RO_Garment_SizeBreakdown.Id.Equals(0))
                {
                    RO_Garment_SizeBreakdownService.Creating(RO_Garment_SizeBreakdown);
                }
            }

            base.OnUpdating(id, model);
            model._LastModifiedAgent = "Service";
            model._LastModifiedBy = this.Username;
        }

        public override async Task<int> DeleteModel(int Id)
        {
            RO_Garment deletedImage = await this.GetAsync(Id);
            await this.AzureImageService.RemoveMultipleImage(deletedImage.GetType().Name, deletedImage.ImagesPath);

            int deleted = await this.DeleteAsync(Id);

            CostCalculationGarmentService costCalculationGarmentService = this.ServiceProvider.GetService<CostCalculationGarmentService>();
            CostCalculationGarment costCalculationGarment = costCalculationGarmentService.DbSet
                .FirstOrDefault(p => p.RO_GarmentId.Equals(Id));
            costCalculationGarment.RO_GarmentId = null;
            await costCalculationGarmentService.UpdateModel(costCalculationGarment.Id, costCalculationGarment);

            return deleted;
        }

        public override void OnDeleting(RO_Garment model)
        {
            RO_Garment_SizeBreakdownService RO_Garment_SizeBreakdownService = this.ServiceProvider.GetService<RO_Garment_SizeBreakdownService>();
            HashSet<int> RO_Garment_SizeBreakdowns = new HashSet<int>(RO_Garment_SizeBreakdownService.DbSet
                .Where(p => p.RO_GarmentId.Equals(model.Id))
                .Select(p => p.Id));

            foreach (int RO_Garment_SizeBreakdown in RO_Garment_SizeBreakdowns)
            {
                RO_Garment_SizeBreakdownService.Deleting(RO_Garment_SizeBreakdown);
            }

            base.OnDeleting(model);
            model._DeletedAgent = "Service";
            model._DeletedBy = this.Username;
        }

        public RO_GarmentViewModel MapToViewModel(RO_Garment model)
        {
            RO_GarmentViewModel viewModel = new RO_GarmentViewModel();
            PropertyCopier<RO_Garment, RO_GarmentViewModel>.Copy(model, viewModel);
            viewModel.ImagesPath = model.ImagesPath != null ? JsonConvert.DeserializeObject<List<string>>(model.ImagesPath) : null;

            CostCalculationGarmentService costCalculationGarmentService = this.ServiceProvider.GetService<CostCalculationGarmentService>();
            viewModel.CostCalculationGarment = costCalculationGarmentService.MapToViewModel(model.CostCalculationGarment);

            viewModel.RO_Garment_SizeBreakdowns = new List<RO_Garment_SizeBreakdownViewModel>();
            RO_Garment_SizeBreakdownService RO_GarmentSizeBreakdownService = this.ServiceProvider.GetService<RO_Garment_SizeBreakdownService>();
            if (model.RO_Garment_SizeBreakdowns != null)
            {
                foreach (RO_Garment_SizeBreakdown sizeBreakdown in model.RO_Garment_SizeBreakdowns)
                {
                    RO_Garment_SizeBreakdownViewModel sizeBreakdownVM = RO_GarmentSizeBreakdownService.MapToViewModel(sizeBreakdown);
                    viewModel.RO_Garment_SizeBreakdowns.Add(sizeBreakdownVM);
                }
            }

            return viewModel;
        }

        public RO_Garment MapToModel(RO_GarmentViewModel viewModel)
        {
            RO_Garment model = new RO_Garment();
            PropertyCopier<RO_GarmentViewModel, RO_Garment>.Copy(viewModel, model);
            model.ImagesPath = viewModel.ImagesPath != null ? JsonConvert.SerializeObject(viewModel.ImagesPath) : null;

            CostCalculationGarmentService costCalculationGarmentService = this.ServiceProvider.GetService<CostCalculationGarmentService>();
            model.CostCalculationGarmentId = viewModel.CostCalculationGarment.Id;
            model.CostCalculationGarment = costCalculationGarmentService.MapToModel(viewModel.CostCalculationGarment);

            model.RO_Garment_SizeBreakdowns = new List<RO_Garment_SizeBreakdown>();
            RO_Garment_SizeBreakdownService RO_Garment_SizeBreakdownService = this.ServiceProvider.GetService<RO_Garment_SizeBreakdownService>();
            foreach (RO_Garment_SizeBreakdownViewModel sizeBreakdownVM in viewModel.RO_Garment_SizeBreakdowns)
            {
                RO_Garment_SizeBreakdown sizeBreakdown = RO_Garment_SizeBreakdownService.MapToModel(sizeBreakdownVM);
                model.RO_Garment_SizeBreakdowns.Add(sizeBreakdown);
            }

            return model;
        }
    }
}
