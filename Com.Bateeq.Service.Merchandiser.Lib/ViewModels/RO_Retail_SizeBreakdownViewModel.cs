using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.ViewModels
{
    public class RO_Retail_SizeBreakdownViewModel : BasicViewModel
    {
        public string Code { get; set; }
        public StoreViewModel Store { get; set; }
        public Dictionary<string, int> SizeQuantity { get; set; }
        public int Total { get; set; }
    }
}
