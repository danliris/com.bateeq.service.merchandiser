using Com.Bateeq.Service.Merchandiser.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System.Linq.Dynamic.Core;
using Com.Moonlay.NetCore.Lib;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class CostCalculationRetail_MaterialService : BasicService<MerchandiserDbContext, CostCalculationRetail_Material>
    {
        public CostCalculationRetail_MaterialService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<CostCalculationRetail_Material>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            IQueryable<CostCalculationRetail_Material> Query = this.DbContext.CostCalculationRetail_Materials;

            List<string> SearchAttributes = new List<string>()
                {
                    "Code"
                };
            Query = ConfigureSearch(Query, SearchAttributes, Keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);

            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code"
                };
            Query = Query
                .Select(b => new CostCalculationRetail_Material
                {
                    Id = b.Id,
                    Code = b.Code
                });

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);

            Pageable<CostCalculationRetail_Material> pageable = new Pageable<CostCalculationRetail_Material>(Query, Page - 1, Size);
            List<CostCalculationRetail_Material> Data = pageable.Data.ToList<CostCalculationRetail_Material>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }
        
        public async Task GeneratePO(CostCalculationRetail_Material model)
        {
            string category = model.CategoryName.Substring(0, 3).ToUpper();
            int latestSN_Retail = this.DbSet
                .Where(d => d.CategoryName.Substring(0, 3).ToUpper() == category)
                .DefaultIfEmpty()
                .Max(d => d.PO_SerialNumber)
                .GetValueOrDefault();
            int latestSN_Garment = this.DbContext.CostCalculationGarment_Materials
                .Where(d => d.CategoryName.Substring(0, 3).ToUpper() == category)
                .DefaultIfEmpty()
                .Max(d => d.PO_SerialNumber)
                .GetValueOrDefault();
            int latestSN = Math.Max(latestSN_Retail, latestSN_Garment);
            model.PO_SerialNumber = latestSN != 0 ? latestSN + 1 : 1;
            if (category == "FAB")
                model.PO = String.Format("{0}{1}{2:D5}", "PM", model._CreatedUtc.ToString("yy"), model.PO_SerialNumber);
            else
                model.PO = String.Format("{0}{1}{2:D5}", "PA", model._CreatedUtc.ToString("yy"), model.PO_SerialNumber);
            await this.UpdateModel(model.Id, model);
        }

        public override void OnCreating(CostCalculationRetail_Material model)
        {
            do
            {
                model.Code = Code.Generate();
            }
            while (this.DbSet.Any(d => d.Code.Equals(model.Code)));

            base.OnCreating(model);
        }
    }
}
