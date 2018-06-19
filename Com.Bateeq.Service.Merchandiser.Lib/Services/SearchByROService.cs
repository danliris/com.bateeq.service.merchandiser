using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class SearchByROService
    {
        private CostCalculationRetailService RetailService;
        private CostCalculationGarmentService SalesService;
        private IQueryable<Object> Query;

        public SearchByROService(CostCalculationGarmentService salesService, CostCalculationRetailService retailService)
        {
            this.RetailService = retailService;
            this.SalesService = salesService;
        }

        public async Task<Object> ReadModelByRO(string ro)
        {
            Query = RetailService.DbContext.CostCalculationRetails.Where(retail => retail.RO.Contains(ro) && retail._IsDeleted == false).Select(b => new SearchByROViewModel
            {
                RO = b.RO,
                Article = b.Article,
                DeliveryDate = b.DeliveryDate,
                Style = b.StyleName,
                Counter = b.CounterName
                
            });

            var result = await Query.ToDynamicListAsync();

            Query = SalesService.DbContext.CostCalculationGarments.Where(garment => garment.RO.Contains(ro) && garment._IsDeleted == false).Select(b => new SearchByROViewModel
            {
                RO = b.RO,
                Article = b.Article,
                DeliveryDate = b.DeliveryDate,
                Style = "",
                Counter = ""
            });

            result.Add(await Query.ToDynamicListAsync());

            return await Task.FromResult(result);
        }
    }
}
