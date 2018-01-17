using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib.Service;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace Com.Bateeq.Service.Merchandiser.Lib.Helpers
{
    public abstract class BasicService<TDbContext, TModel> : StandardEntityService<TDbContext, TModel>
        where TDbContext : DbContext
        where TModel : StandardEntity, IValidatableObject
    {
        public BasicService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public abstract Tuple<List<TModel>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null);

        public virtual async Task<int> CreateModel(TModel Model)
        {
            return await this.CreateAsync(Model);
        }

        public virtual async Task<TModel> ReadModelById(int Id)
        {
            return await this.GetAsync(Id);
        }

        public virtual async Task<int> UpdateModel(int Id, TModel Model)
        {
            return await this.UpdateAsync(Id, Model);
        }

        public virtual async Task<int> DeleteModel(int Id)
        {
            return await this.DeleteAsync(Id);
        }

        public virtual IQueryable<TModel> ConfigureSearch(IQueryable<TModel> Query, List<string> SearchAttributes, string Keyword)
        {
            /* Search with Keyword */
            if (Keyword != null)
            {
                Query = Query.Where(General.BuildSearch(SearchAttributes, Keyword), Keyword);
            }
            return Query;
        }

        public virtual IQueryable<TModel> ConfigureOrder(IQueryable<TModel> Query, Dictionary<string, string> OrderDictionary)
        {
            /* Default Order */
            if (OrderDictionary.Count.Equals(0))
            {
                OrderDictionary.Add("_LastModifiedUtc", General.DESCENDING);

                Query = Query.OrderByDescending(b => b._LastModifiedUtc);
            }
            /* Custom Order */
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
            return Query;
        }
    }
}
