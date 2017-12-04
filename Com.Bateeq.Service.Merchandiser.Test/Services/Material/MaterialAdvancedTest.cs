using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Models = Com.Bateeq.Service.Merchandiser.Lib.Models;
using Xunit;

namespace Com.Bateeq.Service.Merchandiser.Test.Services.Material
{
    [Collection("ServiceProviderFixture collection")]
    public class MaterialAdvancedTest : IDisposable
    {
        private IServiceProvider ServiceProvider;

        public MaterialAdvancedTest(ServiceProviderFixture fixture)
        {
            this.ServiceProvider = fixture.ServiceProvider;
        }

        protected MaterialService Service
        {
            get { return this.ServiceProvider.GetService<MaterialService>(); }
        }

        protected MerchandiserDbContext DbContext
        {
            get { return this.ServiceProvider.GetService<MerchandiserDbContext>(); }
        }

        // Advanced Test Example
        [Fact]
        public void TestAdvancedReadModel()
        {
            MaterialService service = this.Service;

            Tuple<List<Models.Material>, int, Dictionary<string, string>, List<string>> data = service.ReadModel();
            Assert.NotNull(data);
        }

        public void Dispose()
        {
            this.ServiceProvider = null;
        }
    }
}
