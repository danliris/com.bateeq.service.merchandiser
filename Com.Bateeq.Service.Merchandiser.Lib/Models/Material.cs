using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Merchandiser.Lib.Models
{
    public class Material : StandardEntity, IValidatableObject
    { 
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Composition { get; set; }
        public string Construction { get; set; }
        public string Width { get; set; }
        public string Yarn { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            MaterialService service = (MaterialService)validationContext.GetService(typeof(MaterialService));

            if (string.IsNullOrWhiteSpace(this.Name))
                yield return new ValidationResult("Nama harus diisi", new List<string> { "Name" });
            if (this.CategoryId == 0)
                yield return new ValidationResult("Kategori harus diisi", new List<string> { "Category" });
            else
            {
                CategoryService categoryService = (CategoryService)validationContext.GetService(typeof(CategoryService));
                Task<Category> category = Task.Run(() => categoryService.ReadModelById(this.CategoryId));
                category.Wait();

                if (string.Equals(category.Result.Name, "FABRIC", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrWhiteSpace(this.Composition))
                        yield return new ValidationResult("Komposisi harus diisi", new List<string> { "Composition" });
                    if (string.IsNullOrWhiteSpace(this.Construction))
                        yield return new ValidationResult("Konstruksi harus diisi", new List<string> { "Construction" });
                    if (string.IsNullOrWhiteSpace(this.Width))
                        yield return new ValidationResult("Lebar harus diisi", new List<string> { "Width" });
                    if (string.IsNullOrWhiteSpace(this.Yarn))
                        yield return new ValidationResult("Benang harus diisi", new List<string> { "Yarn" });
                }
            }
        }
    }
}
