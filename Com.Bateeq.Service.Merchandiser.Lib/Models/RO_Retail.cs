using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Moonlay.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Bateeq.Service.Merchandiser.Lib.Models
{
    public class RO_Retail : StandardEntity, IValidatableObject
    {
        public int CostCalculationRetailId { get; set; }
        public virtual CostCalculationRetail CostCalculationRetail { get; set; }
        public string Code { get; set; }
        public string ColorId { get; set; }
        public string ColorName { get; set; }
        public ICollection<RO_Retail_SizeBreakdown> RO_Retail_SizeBreakdowns { get; set; }
        public string Instruction { get; set; }
        public string SizeQuantityTotal { get; set; }
        public int Total { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            RO_RetailService service = validationContext.GetService<RO_RetailService>();

            if (service.DbSet.Count(ro => ro.Id != this.Id && ro.CostCalculationRetailId.Equals(this.CostCalculationRetailId) && ro._IsDeleted.Equals(false)) > 0)
                yield return new ValidationResult("Cost Calculation Retail telah terdaftar di RO", new List<string> { "CostCalculationRetail" });
        }
    }
}
