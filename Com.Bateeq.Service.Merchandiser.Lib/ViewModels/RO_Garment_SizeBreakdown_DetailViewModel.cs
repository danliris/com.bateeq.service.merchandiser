using Com.Bateeq.Service.Merchandiser.Lib.Helpers;

namespace Com.Bateeq.Service.Merchandiser.Lib.ViewModels
{
    public class RO_Garment_SizeBreakdown_DetailViewModel : BasicViewModel
    {
        public string Code { get; set; }
        public string Information { get; set; }
        public SizeViewModel Size { get; set; }
        public int? Quantity { get; set; }
    }
}
