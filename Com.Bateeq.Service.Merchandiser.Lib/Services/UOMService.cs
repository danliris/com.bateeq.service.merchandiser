using Com.Bateeq.Service.Merchandiser.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System.Linq.Dynamic.Core;
using System.Reflection;
using Com.Moonlay.NetCore.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using Com.Bateeq.Service.Merchandiser.Lib.Interfaces;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class UOMService : BasicService<MerchandiserDbContext, UOM>, IMap<UOM, UOMViewModel>
    {
        public UOMService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<UOM>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            IQueryable<UOM> Query = this.DbContext.UOMs;

            List<string> SearchAttributes = new List<string>()
                {
                    "Code", "Name"
                };
            Query = ConfigureSearch(Query, SearchAttributes, Keyword);
            
            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code", "Name"
                };
            Query = Query
                .Select(b => new UOM
                {
                    Id = b.Id,
                    Code = b.Code,
                    Name = b.Name
                });

            Dictionary<string, string> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);
            
            Pageable<UOM> pageable = new Pageable<UOM>(Query, Page - 1, Size);
            List<UOM> Data = pageable.Data.ToList<UOM>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public UOMViewModel MapToViewModel(UOM model)
        {
            UOMViewModel viewModel = new UOMViewModel();
            PropertyCopier<UOM, UOMViewModel>.Copy(model, viewModel);
            return viewModel;
        }

        public UOM MapToModel(UOMViewModel viewModel)
        {
            UOM model = new UOM();
            PropertyCopier<UOMViewModel, UOM>.Copy(viewModel, model);
            return model;
        }
    }
}
