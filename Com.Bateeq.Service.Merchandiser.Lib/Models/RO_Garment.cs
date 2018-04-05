using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Moonlay.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Bateeq.Service.Merchandiser.Lib.Models
{
    public class RO_Garment : StandardEntity, IValidatableObject
    {
        public int CostCalculationGarmentId { get; set; }
        public virtual CostCalculationGarment CostCalculationGarment { get; set; }
        public string Code { get; set; }
        public ICollection<RO_Garment_SizeBreakdown> RO_Garment_SizeBreakdowns { get; set; }
        public string Instruction { get; set; }
        public int Total { get; set; }
        public List<string> ImagesFile { get; set; }
        public List<string> ImagesType { get; set; }
        public string ImagesPath { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            RO_GarmentService service = validationContext.GetService<RO_GarmentService>();

            if (service.DbSet.Count(ro => ro.Id != this.Id && ro.CostCalculationGarmentId.Equals(this.CostCalculationGarmentId) && ro._IsDeleted.Equals(false)) > 0)
                yield return new ValidationResult("Cost Calculation Garment telah terdaftar di RO", new List<string> { "CostCalculationGarment" });
        }
    }
}
