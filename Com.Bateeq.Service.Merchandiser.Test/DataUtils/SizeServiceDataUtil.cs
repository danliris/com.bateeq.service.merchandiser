using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Merchandiser.Test.DataUtils
{
    public class SizeServiceDataUtil
    {
        public MerchandiserDbContext DbContext { get; set; }

        public SizeService SizeService { get; set; }

        public SizeServiceDataUtil(MerchandiserDbContext dbContext, SizeService sizeService)
        {
            this.DbContext = dbContext;
            this.SizeService = sizeService;
        }

        public Task<Size> GetTestSize()
        {
            Size testSize = SizeService.DbSet.FirstOrDefault(size => size.Code == "Test");

            if (testSize != null)
                return Task.FromResult(testSize);
            else
            {
                testSize = new Size()
                {
                    Code = "Test",
                    Name = "Test Category"
                };
                int id = SizeService.Create(testSize);
                return SizeService.GetAsync(id);
            }
        }
    }
}
