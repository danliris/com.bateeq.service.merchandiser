using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Moonlay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Bateeq.Service.Merchandiser.Lib.Models
{
    public class UOM : StandardEntity, IValidatableObject
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            UOMService service = (UOMService)validationContext.GetService(typeof(UOMService));

            if (string.IsNullOrWhiteSpace(this.Code))
                yield return new ValidationResult("Kode harus diisi", new List<string> { "Code" });
            else if (service.DbContext.Set<UOM>().Count(r => r._IsDeleted.Equals(false) && r.Id != this.Id && r.Code.Equals(this.Code)) > 0)
                yield return new ValidationResult("Kode satuan sudah ada", new List<string> { "Code" });

            if (string.IsNullOrWhiteSpace(this.Name))
                yield return new ValidationResult("Nama harus diisi", new List<string> { "Name" });
            else if (int.TryParse(this.Name, out int n))
                yield return new ValidationResult("Satuan hanya berupa huruf", new List<string> { "Name" });
        }
    }
}
