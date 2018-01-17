using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Moonlay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Bateeq.Service.Merchandiser.Lib.Models
{
    public class RelatedSize : StandardEntity, IValidatableObject
    {
        public string Code { get; set; }
        public int SizeId { get; set; }
        public virtual Size Size { get; set; }
        public int SizeRangeId { get; set; }
        public virtual SizeRange SizeRange { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.SizeId == 0)
                yield return new ValidationResult("Size harus diisi", new List<string> { "Size" });
            if (this.SizeRangeId == 0)
                yield return new ValidationResult("Size Range harus diisi", new List<string> { "SizeRange" });
        }
    }
}
