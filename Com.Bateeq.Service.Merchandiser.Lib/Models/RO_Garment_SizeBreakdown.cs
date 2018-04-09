using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Moonlay.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Bateeq.Service.Merchandiser.Lib.Models
{
    public class RO_Garment_SizeBreakdown : StandardEntity, IValidatableObject
    {
        public int RO_GarmentId { get; set; }
        public virtual RO_Garment RO_Garment { get; set; }
        public string Code { get; set; }
        public string ColorId { get; set; }
        public string ColorName { get; set; }
        public ICollection<RO_Garment_SizeBreakdown_Detail> RO_Garment_SizeBreakdown_Details { get; set; }
        public int Total { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
