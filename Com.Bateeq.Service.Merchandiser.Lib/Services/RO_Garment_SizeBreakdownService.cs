using Com.Bateeq.Service.Merchandiser.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Dynamic.Core;
using System.Reflection;
using Com.Moonlay.NetCore.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Interfaces;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class RO_Garment_SizeBreakdownService : BasicService<MerchandiserDbContext, RO_Garment_SizeBreakdown>, IMap<RO_Garment_SizeBreakdown, RO_Garment_SizeBreakdownViewModel>
    {
        public RO_Garment_SizeBreakdownService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<RO_Garment_SizeBreakdown>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            IQueryable<RO_Garment_SizeBreakdown> Query = this.DbContext.RO_Garment_SizeBreakdowns;

            List<string> SearchAttributes = new List<string>()
                {
                    "Code"
                };
            Query = ConfigureSearch(Query, SearchAttributes, Keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);

            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code"
                };
            Query = Query
                .Select(b => new RO_Garment_SizeBreakdown
                {
                    Id = b.Id,
                    Code = b.Code
                });

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);

            Pageable<RO_Garment_SizeBreakdown> pageable = new Pageable<RO_Garment_SizeBreakdown>(Query, Page - 1, Size);
            List<RO_Garment_SizeBreakdown> Data = pageable.Data.ToList<RO_Garment_SizeBreakdown>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }
        
        public override void OnCreating(RO_Garment_SizeBreakdown model)
        {
            do
            {
                model.Code = CodeGenerator.GenerateCode();
            }
            while (this.DbSet.Any(d => d.Code.Equals(model.Code)));

            if (model.RO_Garment_SizeBreakdown_Details.Count > 0)
            {
                RO_Garment_SizeBreakdown_DetailService RO_Garment_SizeBreakdown_DetailService = this.ServiceProvider.GetService<RO_Garment_SizeBreakdown_DetailService>();
                foreach (RO_Garment_SizeBreakdown_Detail RO_Garment_SizeBreakdown_Detail in model.RO_Garment_SizeBreakdown_Details)
                {
                    RO_Garment_SizeBreakdown_DetailService.Creating(RO_Garment_SizeBreakdown_Detail);
                }

            }

            base.OnCreating(model);
            model._CreatedAgent = "Service";
            model._CreatedBy = this.Username;
            model._LastModifiedAgent = "Service";
            model._LastModifiedBy = this.Username;
        }
        
        public override void OnUpdating(int id, RO_Garment_SizeBreakdown model)
        {
            RO_Garment_SizeBreakdown_DetailService RO_Garment_SizeBreakdown_DetailService = this.ServiceProvider.GetService<RO_Garment_SizeBreakdown_DetailService>();
            HashSet<int> RO_Garment_SizeBreakdown_Details = new HashSet<int>(RO_Garment_SizeBreakdown_DetailService.DbSet
                .Where(p => p.RO_Garment_SizeBreakdownId.Equals(id))
                .Select(p => p.Id));

            foreach (int RO_Garment_SizeBreakdown_Detail in RO_Garment_SizeBreakdown_Details)
            {
                RO_Garment_SizeBreakdown_Detail childModel = model.RO_Garment_SizeBreakdown_Details.FirstOrDefault(prop => prop.Id.Equals(RO_Garment_SizeBreakdown_Detail));

                if (childModel == null)
                {
                    RO_Garment_SizeBreakdown_DetailService.Deleting(RO_Garment_SizeBreakdown_Detail);
                }
                else
                {
                    RO_Garment_SizeBreakdown_DetailService.Updating(RO_Garment_SizeBreakdown_Detail, childModel);
                }
            }

            foreach (RO_Garment_SizeBreakdown_Detail RO_Garment_SizeBreakdown_Detail in model.RO_Garment_SizeBreakdown_Details)
            {
                if (RO_Garment_SizeBreakdown_Detail.Id.Equals(0))
                {
                    RO_Garment_SizeBreakdown_DetailService.Creating(RO_Garment_SizeBreakdown_Detail);
                }
            }

            base.OnUpdating(id, model);
            model._LastModifiedAgent = "Service";
            model._LastModifiedBy = this.Username;
        }

        public override void OnDeleting(RO_Garment_SizeBreakdown model)
        {
            RO_Garment_SizeBreakdown_DetailService RO_Garment_SizeBreakdown_DetailService = this.ServiceProvider.GetService<RO_Garment_SizeBreakdown_DetailService>();
            HashSet<int> RO_Garment_SizeBreakdown_Details = new HashSet<int>(RO_Garment_SizeBreakdown_DetailService.DbSet
                .Where(p => p.RO_Garment_SizeBreakdownId.Equals(model.Id))
                .Select(p => p.Id));

            foreach (int RO_Garment_SizeBreakdown_Detail in RO_Garment_SizeBreakdown_Details)
            {
                RO_Garment_SizeBreakdown_DetailService.Deleting(RO_Garment_SizeBreakdown_Detail);
            }

            base.OnDeleting(model);
            model._DeletedAgent = "Service";
            model._DeletedBy = this.Username;
        }

        public RO_Garment_SizeBreakdownViewModel MapToViewModel(RO_Garment_SizeBreakdown model)
        {
            RO_Garment_SizeBreakdownViewModel viewModel = new RO_Garment_SizeBreakdownViewModel();
            PropertyCopier<RO_Garment_SizeBreakdown, RO_Garment_SizeBreakdownViewModel>.Copy(model, viewModel);

            viewModel.Color = new ArticleColorViewModel()
            {
                _id = model.ColorId,
                name = model.ColorName
            };

            RO_Garment_SizeBreakdown_DetailService RO_Garment_SizeBreakdown_DetailService = this.ServiceProvider.GetService<RO_Garment_SizeBreakdown_DetailService>();
            viewModel.RO_Garment_SizeBreakdown_Details = new List<RO_Garment_SizeBreakdown_DetailViewModel>();
            if (model.RO_Garment_SizeBreakdown_Details != null)
            {
                foreach (RO_Garment_SizeBreakdown_Detail sizeBreakdownDetail in model.RO_Garment_SizeBreakdown_Details)
                {
                    RO_Garment_SizeBreakdown_DetailViewModel sizeBreakdownDetailVM = RO_Garment_SizeBreakdown_DetailService.MapToViewModel(sizeBreakdownDetail);
                    viewModel.RO_Garment_SizeBreakdown_Details.Add(sizeBreakdownDetailVM);
                }
            }

            return viewModel;
        }

        public RO_Garment_SizeBreakdown MapToModel(RO_Garment_SizeBreakdownViewModel viewModel)
        {
            RO_Garment_SizeBreakdown model = new RO_Garment_SizeBreakdown();
            PropertyCopier<RO_Garment_SizeBreakdownViewModel, RO_Garment_SizeBreakdown>.Copy(viewModel, model);

            model.ColorId = viewModel.Color._id;
            model.ColorName = viewModel.Color.name;

            RO_Garment_SizeBreakdown_DetailService RO_Garment_SizeBreakdown_DetailService = this.ServiceProvider.GetService<RO_Garment_SizeBreakdown_DetailService>();
            model.RO_Garment_SizeBreakdown_Details = new List<RO_Garment_SizeBreakdown_Detail>();
            foreach (RO_Garment_SizeBreakdown_DetailViewModel sizeBreakdownDetailVM in viewModel.RO_Garment_SizeBreakdown_Details)
            {
                RO_Garment_SizeBreakdown_Detail sizeBreakdownDetail = RO_Garment_SizeBreakdown_DetailService.MapToModel(sizeBreakdownDetailVM);
                model.RO_Garment_SizeBreakdown_Details.Add(sizeBreakdownDetail);
            }

            return model;
        }
    }
}
