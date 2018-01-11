using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib.Service;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.Helpers
{
    public abstract class BasicService<TDbContext, TModel> : StandardEntityService<TDbContext, TModel>
        where TDbContext : DbContext
        where TModel : StandardEntity, IValidatableObject
    {
        public BasicService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public virtual async Task<int> CreateModel(TModel Model)
        {
            return await this.CreateAsync(Model);
        }

        public abstract Tuple<List<TModel>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null);

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
    }
}
