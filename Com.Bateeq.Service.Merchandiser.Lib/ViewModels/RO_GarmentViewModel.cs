using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.ViewModels
{
    public class RO_GarmentViewModel : BasicViewModel, IValidatableObject
    {
        public string Code { get; set; }
        public CostCalculationGarmentViewModel CostCalculationGarment { get; set; }
        public List<RO_Garment_SizeBreakdownViewModel> RO_Garment_SizeBreakdowns { get; set; }
        public string Instruction { get; set; }
        public int Total { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.CostCalculationGarment == null)
                yield return new ValidationResult("Nomor RO harus diisi", new List<string> { "CostCalculationGarment" });
        }
    }
}
