using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Com.Moonlay.NetCore.Lib.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System.Linq.Dynamic.Core;
using System.Reflection;
using Com.Moonlay.NetCore.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Interfaces;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class MaterialService : BasicService<MerchandiserDbContext, Material>
    {
        public MaterialService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<Material>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null)
        {
            IQueryable<Material> Query = this.DbContext.Materials;
            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);

            // Search With Keyword
            if (Keyword != null)
            {
                List<string> SearchAttributes = new List<string>()
                    {
                        "Code", "Name"
                    };

                Query = Query.Where(General.BuildSearch(SearchAttributes, Keyword), Keyword);
            }

            // Const Select
            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code", "Name", "Description", "CategoryId"
                };

            Query = Query
                .Select(b => new Material
                {
                    Id = b.Id,
                    Code = b.Code,
                    Name = b.Name,
                    Description = b.Description,
                    CategoryId = b.CategoryId
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
            Pageable<Material> pageable = new Pageable<Material>(Query, Page - 1, Size);
            List<Material> Data = pageable.Data.ToList<Material>();

            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }
    }
}
