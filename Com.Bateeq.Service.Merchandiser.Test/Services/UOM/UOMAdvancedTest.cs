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

namespace Com.Bateeq.Service.Merchandiser.Test.Services.UOM
{
    [Collection("ServiceProviderFixture collection")]
    public class UOMAdvancedTest : IDisposable
    {
        private IServiceProvider ServiceProvider;

        public UOMAdvancedTest(ServiceProviderFixture fixture)
        {
            this.ServiceProvider = fixture.ServiceProvider;
        }

        protected UOMService Service
        {
            get
            {
                UOMService service = (UOMService)this.ServiceProvider.GetService<UOMService>();
                service.Username = "unit test";
                return service;
            }
        }

        protected MerchandiserDbContext DbContext
        {
            get { return this.ServiceProvider.GetService<MerchandiserDbContext>(); }
        }
        
        //[Fact]
        //public async Task TestCreateModel_NumericName()
        //{
        //    UOMService service = this.Service;
        //    Models.UOM testData = new Models.UOM()
        //    {
        //        Code = "Test Numeric Name",
        //        Name = "123"
        //    };

        //    try
        //    {
        //        await service.CreateModel(testData);
        //    }
        //    catch (ServiceValidationExeption ex)
        //    {
        //        ValidationResult numericNameException = ex.ValidationResults.FirstOrDefault(r => r.MemberNames.Contains("Name"));
        //        Assert.NotNull(numericNameException);
        //    }
        //}

        public void Dispose()
        {
            this.ServiceProvider = null;
        }
    }
}
