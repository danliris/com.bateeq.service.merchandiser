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
using System.Collections;

namespace Com.Bateeq.Service.Merchandiser.Test.Services.Efficiency
{
    [Collection("ServiceProviderFixture collection")]
    public class EfficiencyAdvancedTest : IDisposable
    {
        private IServiceProvider ServiceProvider;

        public EfficiencyAdvancedTest(ServiceProviderFixture fixture)
        {
            this.ServiceProvider = fixture.ServiceProvider;
        }

        protected EfficiencyService Service
        {
            get
            {
                EfficiencyService service = (EfficiencyService)this.ServiceProvider.GetService<EfficiencyService>();
                service.Username = "unit_test";
                return service;
            }
        }

        protected MerchandiserDbContext DbContext
        {
            get { return this.ServiceProvider.GetService<MerchandiserDbContext>(); }
        }


        [Theory]
        [ClassData(typeof(EfficiencyTestData))]
        public async Task TestCreateModel_Validation(Models.Efficiency model)
        {
            EfficiencyService service = this.Service;

            try
            {
                int createdCount = await service.CreateModel(model);
                Assert.True(createdCount == 1);
                Models.Efficiency data = await service.ReadModelById(model.Id);
                Assert.NotNull(data);
            }
            catch (ServiceValidationExeption ex)
            {
                if (model.InitialRange <= 0)
                {
                    ValidationResult initialRangeValidation = ex.ValidationResults.FirstOrDefault(r => r.MemberNames.Contains("InitialRange"));
                    Assert.NotNull(initialRangeValidation);
                }
                if (model.FinalRange <= 0)
                {
                    ValidationResult finalRangeValidation = ex.ValidationResults.FirstOrDefault(r => r.MemberNames.Contains("FinalRange"));
                    Assert.NotNull(finalRangeValidation);
                }
                if (model.Value <= 1)
                {
                    ValidationResult valueValidation = ex.ValidationResults.FirstOrDefault(r => r.MemberNames.Contains("Value"));
                    Assert.NotNull(valueValidation);
                }
            }
        }

        public void Dispose()
        {
            this.ServiceProvider = null;
        }
    }

    public class EfficiencyTestData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[]
            {
                new Models.Efficiency { InitialRange = 0, FinalRange = 0, Value = 1 }
            }
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
