using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Moonlay.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Bateeq.Service.Merchandiser.Lib.Models
{
    public class RO_Retail_SizeBreakdown : StandardEntity, IValidatableObject
    {
        public int RO_RetailId { get; set; }
        public virtual RO_Retail RO_Retail { get; set; }
        public string Code { get; set; }
        public string StoreId { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string SizeQuantity { get; set; }
        public int Total { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
