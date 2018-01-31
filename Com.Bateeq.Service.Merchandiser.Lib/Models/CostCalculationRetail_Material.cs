using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.Models
{
    public class CostCalculationRetail_Material : StandardEntity, IValidatableObject

    {
        public int CostCalculationRetailId { get; set; }
        public virtual CostCalculationRetail CostCalculationRetail { get; set; }
        public string Code { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int MaterialId { get; set; }
        public string MaterialName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int UOMQuantityId { get; set; }
        public string UOMQuantityName { get; set; }
        public double Price { get; set; }
        public int UOMPriceId { get; set; }
        public string UOMPriceName { get; set; }
        public double Conversion { get; set; }
        public double Total { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
