using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Models = Com.Bateeq.Service.Merchandiser.Lib.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace Com.Bateeq.Service.Merchandiser.Test.Services.Category
{
    [Collection("ServiceProviderFixture collection")]
    public class CategoryAdvancedTest : IDisposable
    {
        private IServiceProvider ServiceProvider;

        public CategoryAdvancedTest(ServiceProviderFixture fixture)
        {
            this.ServiceProvider = fixture.ServiceProvider;
        }

        protected CategoryService Service
        {
            get { return this.ServiceProvider.GetService<CategoryService>(); }
        }

        protected MerchandiserDbContext DbContext
        {
            get { return this.ServiceProvider.GetService<MerchandiserDbContext>(); }
        }

        // Advanced Test Example
        [Fact]
        public void TestAdvancedReadModel()
        {
            CategoryService service = this.Service;

            Tuple<List<Models.Category>, int, Dictionary<string, string>, List<string>> data = service.ReadModel();
            Assert.NotNull(data);
        }

        public void Dispose()
        {
            this.ServiceProvider = null;
        }
    }
}
