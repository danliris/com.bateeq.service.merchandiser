using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;

namespace Com.Bateeq.Service.Merchandiser.Lib.Models
{
    public class CostCalculationGarment : StandardEntity, IValidatableObject
    {
        public string Code { get; set; }
        public int SerialNumber { get; set; }
        public string RO { get; set; }
        public string Article { get; set; }
        public int LineId { get; set; }
        public string LineCode { get; set; }
        public string LineName { get; set; }
        public string Commodity { get; set; }
        public double FabricAllowance { get; set; }
        public double AccessoriesAllowance { get; set; }
        public string Section { get; set; }
        public int Quantity { get; set; }
        public int SizeRangeId { get; set; }
        public string SizeRangeName { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime ConfirmDate { get; set; }
        public int LeadTime { get; set; }
        public double SMV_Cutting { get; set; }
        public double SMV_Sewing { get; set; }
        public double SMV_Finishing { get; set; }
        public double SMV_Total { get; set; }
        public int BuyerId { get; set; }
        public string BuyerName { get; set; }
        public int EfficiencyId { get; set; }
        public double EfficiencyValue { get; set; }
        public double Index { get; set; }
        public int WageId { get; set; }
        public double WageRate { get; set; }
        public int THRId { get; set; }
        public double THRRate { get; set; }
        public double ConfirmPrice { get; set; }
        public int RateId { get; set; }
        public double RateValue { get; set; }
        public ICollection<CostCalculationGarment_Material> CostCalculationGarment_Materials { get; set; }
        public double Freight { get; set; }
        public double Insurance { get; set; }
        public double CommissionPortion { get; set; }
        public double CommissionRate { get; set; }
        public int OTL1Id { get; set; }
        public double OTL1Rate { get; set; }
        public double OTL1CalculatedRate { get; set; }
        public int OTL2Id { get; set; }
        public double OTL2Rate { get; set; }
        public double OTL2CalculatedRate { get; set; }
        public double Risk { get; set; }
        public double ProductionCost { get; set; }
        public double NETFOB { get; set; }
        public double FreightCost { get; set; }
        public double NETFOBP { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }
        public string ImageType { get; set; }
        public string ImagePath { get; set; }
        public int? RO_GarmentId  { get; set; }
        public virtual RO_Garment RO_Garment { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            CostCalculationGarmentService service = validationContext.GetService<CostCalculationGarmentService>();

            if (service.DbSet.Count(r => r.Id != this.Id && r.Article.Equals(this.Article) && r._IsDeleted.Equals(false)) > 0)
                yield return new ValidationResult("Nama Artikel sudah ada", new List<string> { "Article" });
        }
    }
}
