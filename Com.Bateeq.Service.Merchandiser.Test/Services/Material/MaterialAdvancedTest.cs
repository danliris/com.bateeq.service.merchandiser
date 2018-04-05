using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Models = Com.Bateeq.Service.Merchandiser.Lib.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;
using Com.Bateeq.Service.Merchandiser.Lib.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Com.Moonlay.NetCore.Lib.Service;
using System.Linq;
using Com.Bateeq.Service.Merchandiser.Test.DataUtils;

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
            get
            {
                MaterialService service = (MaterialService)this.ServiceProvider.GetService<MaterialService>();
                service.Username = "unit_test";
                return service;
            }
        }

        protected CategoryServiceDataUtil CategoryDataUtil
        {
            get { return this.ServiceProvider.GetService<CategoryServiceDataUtil>(); }
        }

        protected MerchandiserDbContext DbContext
        {
            get { return this.ServiceProvider.GetService<MerchandiserDbContext>(); }
        }

        [Fact]
        public async Task TestCreateModel_WithFabricCategory()
        {
            MaterialService service = this.Service;

            Task<Models.Category> testCategory = Task.Run(() => this.CategoryDataUtil.GetTestCategory_Fabric());
            testCategory.Wait();

            Models.Material testData =  new Models.Material()
            {
                Category = testCategory.Result,
                Name = "TEST MATERIAL FABRIC",
                Description = "TEST MATERIAL FABRIC DESCRIPTION",
                Composition = string.Empty,
                Construction = string.Empty,
                Width = string.Empty,
                Yarn = string.Empty
            };

            try
            {
                await service.CreateModel(testData);
            }
            catch (ServiceValidationExeption ex)
            {
                ValidationResult compositionValidation = ex.ValidationResults.FirstOrDefault(r => r.MemberNames.Contains("Composition"));
                Assert.NotNull(compositionValidation);
                ValidationResult constructionValidation = ex.ValidationResults.FirstOrDefault(r => r.MemberNames.Contains("Construction"));
                Assert.NotNull(constructionValidation);
                ValidationResult widthValidation = ex.ValidationResults.FirstOrDefault(r => r.MemberNames.Contains("Width"));
                Assert.NotNull(widthValidation);
                ValidationResult yarnValidation = ex.ValidationResults.FirstOrDefault(r => r.MemberNames.Contains("Yarn"));
                Assert.NotNull(yarnValidation);
            }
        }

        public void Dispose()
        {
            this.ServiceProvider = null;
        }
    }
}
