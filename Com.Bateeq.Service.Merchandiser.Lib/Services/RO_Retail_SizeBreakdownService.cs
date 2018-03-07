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
    public class RO_Retail_SizeBreakdownService : BasicService<MerchandiserDbContext, RO_Retail_SizeBreakdown>, IMap<RO_Retail_SizeBreakdown, RO_Retail_SizeBreakdownViewModel>
    {
        public RO_Retail_SizeBreakdownService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<RO_Retail_SizeBreakdown>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            IQueryable<RO_Retail_SizeBreakdown> Query = this.DbContext.RO_RetailSizeBreakdowns;

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
                .Select(b => new RO_Retail_SizeBreakdown
                {
                    Id = b.Id,
                    Code = b.Code
                });

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);

            Pageable<RO_Retail_SizeBreakdown> pageable = new Pageable<RO_Retail_SizeBreakdown>(Query, Page - 1, Size);
            List<RO_Retail_SizeBreakdown> Data = pageable.Data.ToList<RO_Retail_SizeBreakdown>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public override void OnCreating(RO_Retail_SizeBreakdown model)
        {
            do
            {
                model.Code = CodeGenerator.GenerateCode();
            }
            while (this.DbSet.Any(d => d.Code.Equals(model.Code)));

            base.OnCreating(model);
        }

        public RO_Retail_SizeBreakdownViewModel MapToViewModel(RO_Retail_SizeBreakdown model)
        {
            RO_Retail_SizeBreakdownViewModel viewModel = new RO_Retail_SizeBreakdownViewModel();
            PropertyCopier<RO_Retail_SizeBreakdown, RO_Retail_SizeBreakdownViewModel>.Copy(model, viewModel);

            viewModel.Store = new StoreViewModel()
            {
                _id = model.StoreId,
                code = model.StoreCode,
                name = model.StoreName
            };
            viewModel.SizeQuantity = JsonConvert.DeserializeObject<Dictionary<string, int>>(model.SizeQuantity);

            return viewModel;
        }

        public RO_Retail_SizeBreakdown MapToModel(RO_Retail_SizeBreakdownViewModel viewModel)
        {
            RO_Retail_SizeBreakdown model = new RO_Retail_SizeBreakdown();
            PropertyCopier<RO_Retail_SizeBreakdownViewModel, RO_Retail_SizeBreakdown>.Copy(viewModel, model);

            model.StoreId = viewModel.Store._id;
            model.StoreCode = viewModel.Store.code;
            model.StoreName = viewModel.Store.name;
            model.SizeQuantity = JsonConvert.SerializeObject(viewModel.SizeQuantity);

            return model;
        }
    }
}
