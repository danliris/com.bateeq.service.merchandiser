using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.Models
{
    public class Category : StandardEntity, IValidatableObject
    {
        [StringLength(100)]
        public string Code { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        [StringLength(3000)]
        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResult = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(this.Code))
                validationResult.Add(new ValidationResult("Code is required", new List<string> { "Code" }));

            if (string.IsNullOrWhiteSpace(this.Name))
                validationResult.Add(new ValidationResult("Name is required", new List<string> { "Name" }));

            if (validationResult.Count > 0)
            {
                return validationResult;
            }

            else
            {
                /* Service Validation */
                CategoryService service = (CategoryService)validationContext.GetService(typeof(CategoryService));

                if (service.DbContext.Set<Category>().Count(r => r._IsDeleted.Equals(false) && r.Id != this.Id && r.Code.Equals(this.Code)) > 0) /* Code Unique */
                    validationResult.Add(new ValidationResult("Code already exists", new List<string> { "Code" }));

                return validationResult;
            }
        }
    }
}
