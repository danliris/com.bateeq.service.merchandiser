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
            
            /* Required Fields Validation */
            if (string.IsNullOrWhiteSpace(this.Code))
                validationResult.Add(new ValidationResult("Code is required", new List<string> { "Code" }));

            if (string.IsNullOrWhiteSpace(this.Name))
                validationResult.Add(new ValidationResult("Name is required", new List<string> { "Name" }));

            /* Foreign Keys Validation */

            if (validationResult.Count.Equals(0))
            {
                /* Service Validation */
                MaterialService service = (MaterialService)validationContext.GetService(typeof(MaterialService));

                if (service.DbContext.Set<Material>().Count(r => r._IsDeleted.Equals(false) && r.Id != this.Id && r.Code.Equals(this.Code)) > 0) /* Code Unique */
                    validationResult.Add(new ValidationResult("Code already exists", new List<string> { "Code" }));
            }

            return validationResult;
        }
    }
}
