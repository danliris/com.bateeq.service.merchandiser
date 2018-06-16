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
            Query = RetailService.DbContext.CostCalculationRetails.Where(retail => retail.RO == ro).Select(b => new SearchByROViewModel
            {
                RO = b.RO,
                Article = b.Article,
                Style = b.StyleName,
                Counter = b.CounterName,
                DeliveryDate = b.DeliveryDate
            });
            
            if (Query.Any() == false)
            {
                Query = SalesService.DbContext.CostCalculationGarments.Where(garment => garment.RO == ro).Select(b => new SearchByROViewModel
                {
                    RO = b.RO,
                    Article = b.Article,
                    DeliveryDate = b.DeliveryDate
                });
            }

            return await Task.FromResult(Query);
        }
    }
}
