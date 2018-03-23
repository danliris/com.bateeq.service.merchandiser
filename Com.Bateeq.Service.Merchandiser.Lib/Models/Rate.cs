using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Moonlay.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Bateeq.Service.Merchandiser.Lib.Models
{
    public class Rate : StandardEntity, IValidatableObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            RateService service = validationContext.GetService<RateService>();

            if (service.DbSet.Count(r => r.Id != this.Id && r.Name.Equals(this.Name) && r._IsDeleted.Equals(false)) > 0)
                yield return new ValidationResult("Nama Rate sudah ada", new List<string> { "Name" });
        }
    }
}
