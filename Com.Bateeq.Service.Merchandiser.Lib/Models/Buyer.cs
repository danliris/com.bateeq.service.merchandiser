using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.Models
{
    public class Buyer : StandardEntity, IValidatableObject
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
                yield return new ValidationResult("Nama Pembeli harus diisi", new List<string> { "Email" });
            else if (!EmailValidation.IsValidEmail(this.Email))
                yield return new ValidationResult("Format Email tidak benar", new List<string> { "Email" });
        }
    }
}
