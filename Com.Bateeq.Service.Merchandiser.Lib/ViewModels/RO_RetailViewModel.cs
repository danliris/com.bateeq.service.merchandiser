using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.ViewModels
{
    public class RO_RetailViewModel : BasicViewModel, IValidatableObject
    {
        public string Code { get; set; }
        public CostCalculationRetailViewModel CostCalculationRetail { get; set; }
        public ArticleColorViewModel Color { get; set; }
        public List<RO_Retail_SizeBreakdownViewModel> RO_Retail_SizeBreakdowns { get; set; }
        public string Instruction { get; set; }
        public Dictionary<string, int> SizeQuantityTotal { get; set; }
        public int Total { get; set; }
        public List<string> ImagesFile { get; set; }
        public List<string> ImagesPath { get; set; }
        public List<string> ImagesName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.CostCalculationRetail == null)
                yield return new ValidationResult("Nomor RO harus diisi", new List<string> { "CostCalculationRetail" });
        }
    }
}
