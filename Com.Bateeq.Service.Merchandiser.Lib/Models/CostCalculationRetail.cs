using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using System.Linq;

namespace Com.Bateeq.Service.Merchandiser.Lib.Models
{
    public class CostCalculationRetail : StandardEntity, IValidatableObject
    {
        public string Code { get; set; }
        public int SerialNumber { get; set; }
        public string RO { get; set; }
        public string Article { get; set; }
        public string StyleId { get; set; }
        public string StyleName { get; set; }
        public string SeasonId { get; set; }
        public string SeasonCode { get; set; }
        public string SeasonName { get; set; }
        public string CounterId { get; set; }
        public string CounterName { get; set; }
        public int BuyerId { get; set; }
        public string BuyerName { get; set; }
        public int SizeRangeId { get; set; }
        public string SizeRangeName { get; set; }
        public double SH_Cutting { get; set; }
        public double SH_Sewing { get; set; }
        public double SH_Finishing { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int Quantity { get; set; }
        public int EfficiencyId { get; set; }
        public double EfficiencyValue { get; set; }
        public double Risk { get; set; }
        public string Description { get; set; }
        public int OLId { get; set; }
        public double OLRate { get; set; }
        public double OLCalculatedRate { get; set; }
        public int OTL1Id { get; set; }
        public double OTL1Rate { get; set; }
        public double OTL1CalculatedRate { get; set; }
        public int OTL2Id { get; set; }
        public double OTL2Rate { get; set; }
        public double OTL2CalculatedRate { get; set; }
        public int OTL3Id { get; set; }
        public double OTL3Rate { get; set; }
        public double OTL3CalculatedRate { get; set; }

        public ICollection<CostCalculationRetail_Material> CostCalculationRetail_Materials { get; set; }
        public double HPP { get; set; }
        public double WholesalePrice { get; set; }

        public double Proposed20 { get; set; }
        public double Proposed21 { get; set; }
        public double Proposed22 { get; set; }
        public double Proposed23 { get; set; }
        public double Proposed24 { get; set; }
        public double Proposed25 { get; set; }
        public double Proposed26 { get; set; }
        public double Proposed27 { get; set; }
        public double Proposed28 { get; set; }
        public double Proposed29 { get; set; }
        public double Proposed30 { get; set; }
               
        public double Rounding20 { get; set; }
        public double Rounding21 { get; set; }
        public double Rounding22 { get; set; }
        public double Rounding23 { get; set; }
        public double Rounding24 { get; set; }
        public double Rounding25 { get; set; }
        public double Rounding26 { get; set; }
        public double Rounding27 { get; set; }
        public double Rounding28 { get; set; }
        public double Rounding29 { get; set; }
        public double Rounding30 { get; set; }
        public double RoundingOthers { get; set; }
        public string SelectedRounding { get; set; }
        public string ImageFile { get; set; }
        public string ImageType { get; set; }
        public string ImagePath { get; set; }

        public int? RO_RetailId { get; set; }
        public virtual RO_Retail RO_Retail { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            CostCalculationRetailService service = validationContext.GetService<CostCalculationRetailService>();

            if (service.DbSet.Count(r => r.Id != this.Id && r.Article.Equals(this.Article) && r._IsDeleted.Equals(false)) > 0)
                yield return new ValidationResult("Nama Artikel sudah ada", new List<string> { "Article" });
        }
    }
}
