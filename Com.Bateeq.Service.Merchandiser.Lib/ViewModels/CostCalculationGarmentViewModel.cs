using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.ViewModels
{
    public class CostCalculationGarmentViewModel : BasicViewModel, IValidatableObject
    {
        public string Code { get; set; }
        public int SerialNumber { get; set; }
        public string RO { get; set; }
        public string Article { get; set; }
        public ArticleSeasonViewModel Convection { get; set; }
        public string Commodity { get; set; }
        public double? FabricAllowance { get; set; }
        public double? AccessoriesAllowance { get; set; }
        public string Section { get; set; }
        public int? Quantity { get; set; }
        public SizeRangeViewModel SizeRange { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime ConfirmDate { get; set; }
        public int? LeadTime { get; set; }
        public double? SMV_Cutting { get; set; }
        public double? SMV_Sewing { get; set; }
        public double? SMV_Finishing { get; set; }
        public double SMV_Total { get; set; }
        public BuyerViewModel Buyer { get; set; }
        public EfficiencyViewModel Efficiency { get; set; }
        public double Index { get; set; }
        public RateViewModel Wage { get; set; }
        public RateViewModel THR { get; set; }
        public double? ConfirmPrice { get; set; }
        public RateViewModel Rate { get; set; }
        public List<CostCalculationGarment_MaterialViewModel> CostCalculationGarment_Materials { get; set; }
        public double? Freight { get; set; }
        public double? Insurance { get; set; }
        public double? CommissionPortion { get; set; }
        public double CommissionRate { get; set; }
        public RateCalculatedViewModel OTL1 { get; set; }
        public RateCalculatedViewModel OTL2 { get; set; }
        public double Risk { get; set; }
        public double ProductionCost { get; set; }
        public double NETFOB { get; set; }
        public double FreightCost { get; set; }
        public double NETFOBP { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }
        public string ImageType { get; set; }
        public string ImagePath { get; set; }

        public int? RO_RetailId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.Article))
                yield return new ValidationResult("Nama Artikel harus diisi", new List<string> { "Article" });
            if (this.Convection == null || string.IsNullOrWhiteSpace(this.Convection._id))
                yield return new ValidationResult("Konveksi harus diisi", new List<string> { "Convection" });
            if (this.Quantity == null)
                yield return new ValidationResult("Kuantitas harus diisi", new List<string> { "Quantity" });
            else if (this.Quantity <= 0)
                yield return new ValidationResult("Kuantitas harus lebih besar dari 0", new List<string> { "Quantity" });
            else if (this.Efficiency == null || this.Efficiency.Id == 0)
                yield return new ValidationResult("Tidak ditemukan Efisiensi pada kuantitas ini", new List<string> { "Quantity" });
            if (this.SizeRange == null || this.SizeRange.Id == 0)
                yield return new ValidationResult("Size Range harus diisi", new List<string> { "SizeRange" });
            if (this.DeliveryDate == null || this.DeliveryDate == DateTime.MinValue)
                yield return new ValidationResult("Delivery Date harus diisi", new List<string> { "DeliveryDate" });
            else if (this.DeliveryDate < DateTime.Today)
                yield return new ValidationResult("Delivery Date harus lebih besar dari hari ini", new List<string> { "DeliveryDate" });
            if (this.SMV_Cutting == null)
                yield return new ValidationResult("SMV Cutting harus diisi", new List<string> { "SMV_Cutting" });
            else if (this.SMV_Cutting <= 0)
                yield return new ValidationResult("SMV Cutting harus lebih besar dari 0", new List<string> { "SMV_Cutting" });
            if (this.SMV_Sewing == null)
                yield return new ValidationResult("SMV Sewing harus diisi", new List<string> { "SMV_Sewing" });
            else if (this.SMV_Sewing <= 0)
                yield return new ValidationResult("SMV Sewing harus lebih besar dari 0", new List<string> { "SMV_Sewing" });
            if (this.SMV_Finishing == null)
                yield return new ValidationResult("SMV Finishing harus diisi", new List<string> { "SMV_Finishing" });
            else if (this.SMV_Finishing <= 0)
                yield return new ValidationResult("SMV Finishing harus lebih besar dari 0", new List<string> { "SMV_Finishing" });
            if (this.Buyer == null || this.Buyer.Id == 0)
                yield return new ValidationResult("Buyer harus diisi", new List<string> { "Buyer" });
            if (this.ConfirmPrice == null)
                yield return new ValidationResult("Confirm Price harus diisi", new List<string> { "ConfirmPrice" });
            else if (this.ConfirmPrice <= 0)
                yield return new ValidationResult("Confirm Price harus lebih besar dari 0", new List<string> { "ConfirmPrice" });

            int Count = 0;
            string costCalculationGarment_MaterialsError = "[";

            if (this.CostCalculationGarment_Materials == null || this.CostCalculationGarment_Materials.Count.Equals(0))
                yield return new ValidationResult("Tabel Cost Calculation Garment Material dibawah harus diisi", new List<string> { "CostCalculationGarment_MaterialTable" });
            else
            {
                foreach (CostCalculationGarment_MaterialViewModel costCalculation_Material in this.CostCalculationGarment_Materials)
                {
                    costCalculationGarment_MaterialsError += "{";
                    if (costCalculation_Material.Category == null || costCalculation_Material.Category.Id == 0)
                    {
                        Count++;
                        costCalculationGarment_MaterialsError += "Category: 'Kategori harus diisi', ";
                    }
                    else
                    {
                        if (costCalculation_Material.Material == null || costCalculation_Material.Material.Id == 0)
                        {
                            Count++;
                            costCalculationGarment_MaterialsError += "Material: 'Material harus diisi', ";
                        }

                        if (costCalculation_Material.Quantity == null)
                        {
                            Count++;
                            costCalculationGarment_MaterialsError += "Quantity: 'Kuantitas harus diisi', ";
                        }
                        else if (costCalculation_Material.Quantity <= 0)
                        {
                            Count++;
                            costCalculationGarment_MaterialsError += "Quantity: 'Kuantitas harus lebih besar dari 0', ";
                        }

                        if (costCalculation_Material.Price == null)
                        {
                            Count++;
                            costCalculationGarment_MaterialsError += "Price: 'Harga harus diisi', ";
                        }
                        else if (costCalculation_Material.Price <= 0)
                        {
                            Count++;
                            costCalculationGarment_MaterialsError += "Price: 'Harga harus lebih besar dari 0', ";
                        }

                        if (costCalculation_Material.Conversion == null)
                        {
                            Count++;
                            costCalculationGarment_MaterialsError += "Conversion: 'Konversi harus diisi', ";
                        }
                        else if (costCalculation_Material.Conversion <= 0)
                        {
                            Count++;
                            costCalculationGarment_MaterialsError += "Conversion: 'Konversi harus lebih besar dari 0', ";
                        }

                        if (costCalculation_Material.UOMQuantity == null || costCalculation_Material.UOMQuantity.Id == 0)
                        {
                            Count++;
                            costCalculationGarment_MaterialsError += "UOMQuantity: 'Satuan Kuantitas harus diisi', ";
                        }

                        if (costCalculation_Material.UOMPrice == null || costCalculation_Material.UOMPrice.Id == 0)
                        {
                            Count++;
                            costCalculationGarment_MaterialsError += "UOMPrice: 'Satuan Harga harus diisi', ";
                        }
                    }
                    costCalculationGarment_MaterialsError += "},";
                }
            }

            costCalculationGarment_MaterialsError += "]";

            if (Count > 0)
            {
                yield return new ValidationResult(costCalculationGarment_MaterialsError, new List<string> { "CostCalculationGarment_Materials" });
            }
        }
    }
}
