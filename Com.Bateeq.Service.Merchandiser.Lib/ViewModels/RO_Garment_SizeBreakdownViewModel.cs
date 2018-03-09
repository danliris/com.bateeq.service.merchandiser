using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.ViewModels
{
    public class RO_Garment_SizeBreakdownViewModel : BasicViewModel
    {
        public string Code { get; set; }
        public ArticleColorViewModel Color { get; set; }
        public List<RO_Garment_SizeBreakdown_DetailViewModel> RO_Garment_SizeBreakdown_Details { get; set; }
        public int Total { get; set; }
    }
}
