using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Bateeq.Service.Merchandiser.Lib.ViewModels
{
    public class SizeRangeViewModel : BasicViewModel, IValidatableObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<RelatedSizeViewModel> RelatedSizes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.Name))
                yield return new ValidationResult("Nama harus diisi", new List<string> { "Name" });
        }
    }
}
