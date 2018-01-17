using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.ViewModels
{
    public class MaterialViewModel : BasicViewModel, IValidatableObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Composition { get; set; }
        public string Construction { get; set; }
        public string Width { get; set; }
        public string Yarn { get; set; }
        public int? CategoryId { get; set; }
        public CategoryVM Category { get; set; }

        public class CategoryVM
        {
            public int Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string SubCategory { get; set; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.Name))
                yield return new ValidationResult("Nama harus diisi", new List<string> { "Name" });
            if (this.Category == null || this.Category.Id == 0)
                yield return new ValidationResult("Kategori harus diisi", new List<string> { "Category" });
        }
    }
}
