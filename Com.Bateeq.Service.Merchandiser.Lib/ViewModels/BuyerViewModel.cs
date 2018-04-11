using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.ViewModels
{
    public class BuyerViewModel : BasicViewModel, IValidatableObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.Name))
                yield return new ValidationResult("Nama Pembeli harus diisi", new List<string> { "Name" });

            if (string.IsNullOrWhiteSpace(this.Email))
                yield return new ValidationResult("Email Pembeli harus diisi", new List<string> { "Email" });
            else if (!Helpers.Email.IsValid(this.Email))
                yield return new ValidationResult("Format Email tidak benar", new List<string> { "Email" });
        }
    }
}
