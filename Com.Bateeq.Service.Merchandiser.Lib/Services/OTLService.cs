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
using System.Reflection;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class OTLService : BasicService<MerchandiserDbContext, OTL>
    {
        private CodeGenerator codeGenerator = new CodeGenerator();
        public OTLService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        
        public override Tuple<List<OTL>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null)
        {
            IQueryable<OTL> Query = this.DbContext.OTLs;
            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);

            // Search With Keyword
            if (Keyword != null)
            {
                List<string> SearchAttributes = new List<string>()
                    {
                        "Name", "Rate"
                    };

                Query = Query.Where(General.BuildSearch(SearchAttributes, Keyword), Keyword);
            }

            // Const Select
            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code", "Name", "Rate"
                };

            Query = Query
                .Select(b => new OTL
                {
                    Id = b.Id,
                    Code = b.Code,
                    Name = b.Name,
                    Rate = b.Rate
                });

            // Order
            if (OrderDictionary.Count.Equals(0))
            {
                OrderDictionary.Add("_LastModifiedUtc", General.DESCENDING);

                Query = Query.OrderByDescending(b => b._LastModifiedUtc); // Default Order
            }
            else
            {
                string Key = OrderDictionary.Keys.First();
                string OrderType = OrderDictionary[Key];
                string TransformKey = General.TransformOrderBy(Key);

                BindingFlags IgnoreCase = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

                Query = OrderType.Equals(General.ASCENDING) ?
                    Query.OrderBy(b => b.GetType().GetProperty(TransformKey, IgnoreCase).GetValue(b)) :
                    Query.OrderByDescending(b => b.GetType().GetProperty(TransformKey, IgnoreCase).GetValue(b));
            }

            // Pagination
            Pageable<OTL> pageable = new Pageable<OTL>(Query, Page - 1, Size);
            List<OTL> Data = pageable.Data.ToList<OTL>();

            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public override void OnCreating(OTL model)
        {
            CodeGenerator codeGenerator = new CodeGenerator();

            do
            {
                model.Code = codeGenerator.GenerateCode();
            }
            while (this.DbSet.Any(d => d.Code.Equals(model.Code)));

            base.OnCreating(model);
        }
    }
}
