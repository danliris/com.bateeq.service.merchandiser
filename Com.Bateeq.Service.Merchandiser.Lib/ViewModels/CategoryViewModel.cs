using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.ViewModels
{
    public class CategoryViewModel : BasicViewModel, IValidatableObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string SubCategory { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.Name))
                yield return new ValidationResult("Nama Kategori harus diisi", new List<string> { "Name" });

            if (string.IsNullOrWhiteSpace(this.SubCategory))
                yield return new ValidationResult("Sub Kategori harus diisi", new List<string> { "SubCategory" });
        }
    }
}
