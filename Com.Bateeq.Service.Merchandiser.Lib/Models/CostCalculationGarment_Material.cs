using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.Models
{
    public class CostCalculationGarment_Material : StandardEntity, IValidatableObject
    {
        public int CostCalculationGarmentId { get; set; }
        public virtual CostCalculationGarment CostCalculationGarment { get; set; }
        public string Code { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int MaterialId { get; set; }
        public string MaterialName { get; set; }
        public string Description { get; set; }
        public double Quantity { get; set; }
        public int UOMQuantityId { get; set; }
        public string UOMQuantityName { get; set; }
        public double Price { get; set; }
        public int UOMPriceId { get; set; }
        public string UOMPriceName { get; set; }
        public double Conversion { get; set; }
        public double Total { get; set; }
        public bool isFabricCMT { get; set; }
        public double? CMT_Price { get; set; }
        public double ShippingFeePortion { get; set; }
        public double TotalShippingFee { get; set; }
        public double BudgetQuantity { get; set; }
        public string Information { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
