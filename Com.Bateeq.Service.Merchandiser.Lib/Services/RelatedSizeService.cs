using Com.Bateeq.Service.Merchandiser.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System.Linq.Dynamic.Core;
using System.Reflection;
using Com.Moonlay.NetCore.Lib;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class RelatedSizeService : BasicService<MerchandiserDbContext, RelatedSize>
    {
        public RelatedSizeService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<RelatedSize>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int RelatedSize = 25, string Order = "{}", List<string> Select = null, string Keyword = null)
        {
            IQueryable<RelatedSize> Query = this.DbContext.RelatedSizes;

            List<string> SearchAttributes = new List<string>()
                {
                    "Code"
                };
            Query = ConfigureSearch(Query, SearchAttributes, Keyword);
            
            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code"
                };
            Query = Query
                .Select(b => new RelatedSize
                {
                    Id = b.Id,
                    Code = b.Code
                });

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);
            
            Pageable<RelatedSize> pageable = new Pageable<RelatedSize>(Query, Page - 1, RelatedSize);
            List<RelatedSize> Data = pageable.Data.ToList<RelatedSize>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public override void OnCreating(RelatedSize model)
        {
            do
            {
                model.Code = CodeGenerator.GenerateCode();
            }
            while (this.DbSet.Any(d => d.Code.Equals(model.Code)));

            base.OnCreating(model);
        }
    }
}
