using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Com.Moonlay.NetCore.Lib.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class CategoryService : StandardEntityService<CoreDbContext, Category>
    {
        public CategoryService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
