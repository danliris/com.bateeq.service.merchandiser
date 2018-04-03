using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Moonlay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Com.Bateeq.Service.Merchandiser.Lib.Models
{
    public class UOM : StandardEntity, IValidatableObject
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            UOMService service = validationContext.GetService<UOMService>();

            if (service.DbSet.Count(r => r.Id != this.Id && r.Code.Equals(this.Code) && r._IsDeleted.Equals(false)) > 0)
                yield return new ValidationResult("Kode satuan sudah ada", new List<string> { "Code" });
        }
    }
}
