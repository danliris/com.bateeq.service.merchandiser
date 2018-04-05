using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Bateeq.Service.Merchandiser.Lib.ViewModels
{
    public class LineViewModel : BasicViewModel, IValidatableObject
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.Code))
                yield return new ValidationResult("Code harus diisi", new List<string> { "Code" });
            if (string.IsNullOrWhiteSpace(this.Name))
                yield return new ValidationResult("Nama harus diisi", new List<string> { "Name" });
        }
    }
}
