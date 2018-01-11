using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Models = Com.Bateeq.Service.Merchandiser.Lib.Models;
using System;
using Xunit;
using Com.Bateeq.Service.Merchandiser.Test.Helpers;

namespace Com.Bateeq.Service.Merchandiser.Test.Service.Buyer
{
    [Collection("ServiceProviderFixture collection")]
    public class BuyerBasicTest : BasicServiceTest<MerchandiserDbContext, BuyerService, Models.Buyer>
    {
        private static readonly string[] createAttrAssertions = { "Name", "Email" };
        private static readonly string[] updateAttrAssertions = { "Name", "Email" };
        private static readonly string[] existAttrCriteria = { };

        public BuyerBasicTest(ServiceProviderFixture fixture) : base(fixture, createAttrAssertions, updateAttrAssertions, existAttrCriteria)
        {
        }

        public override void EmptyCreateModel(Models.Buyer model)
        {
            model.Name = string.Empty;
            model.Email = string.Empty;
        }

        public override void EmptyUpdateModel(Models.Buyer model)
        {
            model.Name = string.Empty;
            model.Email = string.Empty;
        }

        public override Models.Buyer GenerateTestModel()
        {
            string guid = Guid.NewGuid().ToString();
            return new Models.Buyer()
            {
                Code = guid,
                Name = string.Format("Test Buyer {0}", guid),
                Email = "testbuyer@email.com"
            };
        }
    }
}