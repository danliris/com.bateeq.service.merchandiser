using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class SearchByStyleService
    {
        private CostCalculationRetailService RetailService;
        private IQueryable Query;

        public SearchByStyleService(CostCalculationRetailService retailService)
        {
            this.RetailService = retailService;
        }
        public async Task<Object> ReadModelByStyle(string stylename)
        {
            Query = RetailService
                   .DbContext
                   .CostCalculationRetails
                   .Where(retail => retail.StyleName.Contains(stylename) && retail._IsDeleted == false)
                   .Select(retail => new ArticleStyleViewModel
                   {
                       name= retail.StyleName
                   })
                   .GroupBy(x => x.name)
                   .Select(x => x.First());
            
            var result = await Query.ToDynamicListAsync();
            return await Task.FromResult(result);
        }
     
    }
}
