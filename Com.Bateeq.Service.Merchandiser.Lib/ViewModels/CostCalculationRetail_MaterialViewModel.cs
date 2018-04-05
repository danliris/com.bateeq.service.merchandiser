using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.ViewModels
{
    public class CostCalculationRetail_MaterialViewModel : BasicViewModel
    {
        public string Code { get; set; }
        public int CostCalculationRetailId { get; set; }
        public CategoryViewModel Category { get; set; }
        public MaterialViewModel Material { get; set; }
        public string Description { get; set; }
        public double? Quantity { get; set; }
        public UOMViewModel UOMQuantity { get; set; }
        public double? Price { get; set; }
        public UOMViewModel UOMPrice { get; set; }
        public double? Conversion { get; set; }
        public double Total { get; set; }
        public string Information { get; set; }
    }
}
