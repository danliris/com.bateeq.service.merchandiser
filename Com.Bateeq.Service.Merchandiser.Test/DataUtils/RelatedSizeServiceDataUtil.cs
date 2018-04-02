using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Merchandiser.Test.DataUtils
{
    public class RelatedSizeServiceDataUtil
    {
        public MerchandiserDbContext DbContext { get; set; }

        public RelatedSizeService RelatedSizeService { get; set; }

        public RelatedSizeServiceDataUtil(MerchandiserDbContext dbContext, RelatedSizeService relatedSizeService)
        {
            this.DbContext = dbContext;
            this.RelatedSizeService = relatedSizeService;
            this.RelatedSizeService.Username = "unit test";
        }

        public Task<RelatedSize> GetTestRelatedSize(int id)
        {
            RelatedSize testRelatedSize = RelatedSizeService.DbSet.FirstOrDefault(rs => rs.Code == "Test");

            if (testRelatedSize != null)
                return Task.FromResult(testRelatedSize);
            else
            {
                testRelatedSize = new RelatedSize()
                {
                    Code = "Test",
                    SizeId = id
                };
                RelatedSizeService.Create(testRelatedSize);
                return RelatedSizeService.GetAsync(testRelatedSize.Id);
            }
        }
    }
}
