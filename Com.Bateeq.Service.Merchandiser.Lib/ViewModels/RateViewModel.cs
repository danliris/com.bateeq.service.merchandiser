using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.ViewModels
{
    public class RateViewModel : BasicViewModel, IValidatableObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double? Value { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.Name))
                yield return new ValidationResult("Nama ongkos harus diisi", new List<string> { "Name" });

            if (this.Value == null)
                yield return new ValidationResult("Tarif ongkos harus diisi", new List<string> { "Rate" });
            else if (this.Value <= 0)
                yield return new ValidationResult("Tarif ongkos harus lebih besar dari 0", new List<string> { "Rate" });
        }
    }
}
