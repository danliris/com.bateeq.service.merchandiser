using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Moonlay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Bateeq.Service.Merchandiser.Lib.Models
{
    public class Material : StandardEntity, IValidatableObject
    {
        public int CategoryId { get; set; }

        [StringLength(100)]
        public string Code { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        [StringLength(3000)]
        public string Description { get; set; }

        [StringLength(500)]
        public string Composition { get; set; }

        [StringLength(500)]
        public string Construction { get; set; }

        [StringLength(500)]
        public string Width { get; set; }

        [StringLength(500)]
        public string Yarn { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResult = new List<ValidationResult>();
            MaterialService service = (MaterialService)validationContext.GetService(typeof(MaterialService));
            
            if (string.IsNullOrWhiteSpace(this.Code))
                yield return new ValidationResult("Kode harus diisi", new List<string> { "Code" });
            else if (service.DbContext.Set<Material>().Count(r => r._IsDeleted.Equals(false) && r.Id != this.Id && r.Code.Equals(this.Code)) > 0)
                yield return new ValidationResult("Kode sudah terdaftar", new List<string> { "Code" });

            if (string.IsNullOrWhiteSpace(this.Name))
                yield return new ValidationResult("Nama harus diisi", new List<string> { "Name" });
        }
    }
}
