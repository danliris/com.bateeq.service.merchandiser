using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Bateeq.Service.Merchandiser.Lib.ViewModels
{
    public class SizeRangeViewModel : BasicViewModel, IValidatableObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<RelatedSizeVM> RelatedSizes { get; set; }

        public class RelatedSizeVM
        {
            public int Id { get; set; }
            public SizeVM Size { get; set; }

            public class SizeVM
            {
                public int Id { get; set; }
                public string Code { get; set; }
                public string Name { get; set; }
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.Name))
                yield return new ValidationResult("Nama harus diisi", new List<string> { "Name" });
        }
    }
}
