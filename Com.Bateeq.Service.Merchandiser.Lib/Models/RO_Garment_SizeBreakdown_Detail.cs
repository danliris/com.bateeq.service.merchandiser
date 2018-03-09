using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Moonlay.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Bateeq.Service.Merchandiser.Lib.Models
{
    public class RO_Garment_SizeBreakdown_Detail : StandardEntity, IValidatableObject
    {
        public int RO_Garment_SizeBreakdownId { get; set; }
        public virtual RO_Garment_SizeBreakdown RO_Garment_SizeBreakdown { get; set; }
        public string Code { get; set; }
        public string Information { get; set; }
        public int SizeId { get; set; }
        public string SizeName { get; set; }
        public int Quantity { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
