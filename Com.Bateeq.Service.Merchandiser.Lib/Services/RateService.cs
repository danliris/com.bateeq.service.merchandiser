using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using Com.Bateeq.Service.Merchandiser.Lib.Interfaces;
using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using Com.Moonlay.NetCore.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class RateService : BasicService<MerchandiserDbContext, Rate>, IMap<Rate, RateViewModel>
    {
        public RateService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        
        public override Tuple<List<Rate>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            IQueryable<Rate> Query = this.DbContext.Rates;

            List<string> SearchAttributes = new List<string>()
                {
                    "Name"
                };
            Query = ConfigureSearch(Query, SearchAttributes, Keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);

            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code", "Name", "Value"
                };
            Query = Query
                .Select(b => new Rate
                {
                    Id = b.Id,
                    Code = b.Code,
                    Name = b.Name,
                    Value = b.Value
                });

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);

            Pageable<Rate> pageable = new Pageable<Rate>(Query, Page - 1, Size);
            List<Rate> Data = pageable.Data.ToList<Rate>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public override void OnCreating(Rate model)
        {
            do
            {
                model.Code = CodeGenerator.GenerateCode();
            }
            while (this.DbSet.Any(d => d.Code.Equals(model.Code)));

            base.OnCreating(model);
        }

        public RateViewModel MapToViewModel(Rate model)
        {
            RateViewModel viewModel = new RateViewModel();
            PropertyCopier<Rate, RateViewModel>.Copy(model, viewModel);
            viewModel.Value = model.Value;
            return viewModel;
        }

        public Rate MapToModel(RateViewModel viewModel)
        {
            Rate model = new Rate();
            PropertyCopier<RateViewModel, Rate>.Copy(viewModel, model);
            model.Value = (double)viewModel.Value;
            return model;
        }
    }
}
