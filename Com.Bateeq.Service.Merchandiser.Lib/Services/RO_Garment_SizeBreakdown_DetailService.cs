using Com.Bateeq.Service.Merchandiser.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System.Linq.Dynamic.Core;
using System.Reflection;
using Com.Moonlay.NetCore.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Interfaces;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class RO_Garment_SizeBreakdown_DetailService : BasicService<MerchandiserDbContext, RO_Garment_SizeBreakdown_Detail>, IMap<RO_Garment_SizeBreakdown_Detail, RO_Garment_SizeBreakdown_DetailViewModel>
    {
        public RO_Garment_SizeBreakdown_DetailService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<RO_Garment_SizeBreakdown_Detail>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            IQueryable<RO_Garment_SizeBreakdown_Detail> Query = this.DbContext.RO_Garment_SizeBreakdown_Details;

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
                .Select(b => new RO_Garment_SizeBreakdown_Detail
                {
                    Id = b.Id,
                    Code = b.Code
                });

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);

            Pageable<RO_Garment_SizeBreakdown_Detail> pageable = new Pageable<RO_Garment_SizeBreakdown_Detail>(Query, Page - 1, Size);
            List<RO_Garment_SizeBreakdown_Detail> Data = pageable.Data.ToList<RO_Garment_SizeBreakdown_Detail>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public override void OnCreating(RO_Garment_SizeBreakdown_Detail model)
        {
            do
            {
                model.Code = CodeGenerator.GenerateCode();
            }
            while (this.DbSet.Any(d => d.Code.Equals(model.Code)));

            base.OnCreating(model);
        }

        public RO_Garment_SizeBreakdown_DetailViewModel MapToViewModel(RO_Garment_SizeBreakdown_Detail model)
        {
            RO_Garment_SizeBreakdown_DetailViewModel viewModel = new RO_Garment_SizeBreakdown_DetailViewModel();
            PropertyCopier<RO_Garment_SizeBreakdown_Detail, RO_Garment_SizeBreakdown_DetailViewModel>.Copy(model, viewModel);

            viewModel.Size = new SizeViewModel()
            {
                Id = model.SizeId,
                Name = model.SizeName
            };

            return viewModel;
        }

        public RO_Garment_SizeBreakdown_Detail MapToModel(RO_Garment_SizeBreakdown_DetailViewModel viewModel)
        {
            RO_Garment_SizeBreakdown_Detail model = new RO_Garment_SizeBreakdown_Detail();
            PropertyCopier<RO_Garment_SizeBreakdown_DetailViewModel, RO_Garment_SizeBreakdown_Detail>.Copy(viewModel, model);

            model.SizeId = viewModel.Size.Id;
            model.SizeName = viewModel.Size.Name;

            return model;
        }
    }
}
