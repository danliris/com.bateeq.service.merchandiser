using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Moonlay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Bateeq.Service.Merchandiser.Lib.Models
{
    public class Category : StandardEntity, IValidatableObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string SubCategory { get; set; }

        public List<Material> Materials { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.Name))
                yield return new ValidationResult("Nama Kategori harus diisi", new List<string> { "Name" });

            if (string.IsNullOrWhiteSpace(this.SubCategory))
                yield return new ValidationResult("Sub Kategori harus diisi", new List<string> { "SubCategory" });
        }
    }
}
