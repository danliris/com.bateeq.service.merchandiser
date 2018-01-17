using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Moonlay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Bateeq.Service.Merchandiser.Lib.Models
{
    public class OTL : StandardEntity, IValidatableObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Rate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
