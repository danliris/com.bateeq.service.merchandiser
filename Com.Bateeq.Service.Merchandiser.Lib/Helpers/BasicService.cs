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

        public void Validate(TModel model)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext validationContext = new ValidationContext(model, this.ServiceProvider, null);

            if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
                throw new ServiceValidationExeption(validationContext, validationResults);
        }

        public abstract Tuple<List<TModel>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}");

        public virtual async Task<int> CreateModel(TModel model)
        {
            return await this.CreateAsync(model);
        }

        public void Creating(TModel model)
        {
            this.DbSet.Add(model);
            this.OnCreating(model);
            this.Validate(model);
        }

        public virtual async Task<TModel> ReadModelById(int id)
        {
            return await this.GetAsync(id);
        }

        public virtual async Task<int> UpdateModel(int id, TModel model)
        {
            return await this.UpdateAsync(id, model);
        }

        public void Updating(int id, TModel model)
        {
            this.DbContext.Entry(model).State = EntityState.Modified;
            this.OnUpdating(id, model);
            this.Validate(model);
        }

        public virtual async Task<int> DeleteModel(int id)
        {
            return await this.DeleteAsync(id);
        }

        public void Deleting(int id)
        {
            TModel entity = this.Get(id);
            if (entity == null)
            {
                throw new Exception();
            }
            this.OnDeleting(entity);
        }

        public virtual IQueryable<TModel> ConfigureSearch(IQueryable<TModel> Query, List<string> SearchAttributes, string Keyword)
        {
            /* Search with Keyword */
            if (Keyword != null)
            {
                Query = Query.Where(General.BuildSearch(SearchAttributes), Keyword);
            }
            return Query;
        }

        public virtual IQueryable<TModel> ConfigureFilter(IQueryable<TModel> Query, Dictionary<string, object> FilterDictionary)
        {
            if (FilterDictionary != null && !FilterDictionary.Count.Equals(0))
            {
                foreach (var f in FilterDictionary)
                {
                    string Key = f.Key;
                    object Value = f.Value;
                    string filterQuery = string.Concat(string.Empty, Key, " == @0");

                    Query = Query.Where(filterQuery, Value);
                }
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
