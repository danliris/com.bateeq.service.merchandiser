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

namespace Com.Bateeq.Service.Merchandiser.Test.Services.Buyer
{
    [Collection("ServiceProviderFixture collection")]
    public class BuyerAdvancedTest : IDisposable
    {
        private IServiceProvider ServiceProvider;

        public BuyerAdvancedTest(ServiceProviderFixture fixture)
        {
            this.ServiceProvider = fixture.ServiceProvider;
        }

        protected BuyerService Service
        {
            get
            {
                BuyerService service = (BuyerService)this.ServiceProvider.GetService<BuyerService>();
                service.Username = "unit_test";
                return service;
            }
        }

        protected MerchandiserDbContext DbContext
        {
            get { return this.ServiceProvider.GetService<MerchandiserDbContext>(); }
        }

        [Fact]
        public async Task TestCreateModel_InvalidEmail()
        {
            BuyerService service = this.Service;
            Models.Buyer testData = new Models.Buyer()
            {
                Name = "Test Email",
                Email = "InvalidEmail"
            };

            try
            {
                await service.CreateModel(testData);
            }
            catch (ServiceValidationExeption ex)
            {
                ValidationResult numericNameException = ex.ValidationResults.FirstOrDefault(r => r.MemberNames.Contains("Email"));
                Assert.NotNull(numericNameException);
            }
        }

        public void Dispose()
        {
            this.ServiceProvider = null;
        }
    }
}
