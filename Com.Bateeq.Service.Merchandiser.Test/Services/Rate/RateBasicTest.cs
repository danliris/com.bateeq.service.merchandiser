using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Bateeq.Service.Merchandiser.Test.Helpers;
using System;
using Models = Com.Bateeq.Service.Merchandiser.Lib.Models;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Bateeq.Service.Merchandiser.Test.Services.Rate
{
    [Collection("ServiceProviderFixture collection")]
    public class RateBasicTest : BasicServiceTest<MerchandiserDbContext, RateService, Models.Rate>
    {
        private static readonly string[] createAttrAssertions = { "Name", "Value" };
        private static readonly string[] updateAttrAssertions = { "Name", "Value" };
        private static readonly string[] existAttrCriteria = {};

        public RateBasicTest(ServiceProviderFixture fixture) : base(fixture, createAttrAssertions, updateAttrAssertions, existAttrCriteria)
        {
        }

        public override void EmptyCreateModel(Models.Rate model)
        {
        }

        public override void EmptyUpdateModel(Models.Rate model)
        {
        }

        public override Models.Rate GenerateTestModel()
        {
            string guid = Guid.NewGuid().ToString();
            return new Models.Rate()
            {
                Code = guid,
                Name = string.Format("TEST OTL {0}", guid),
                Value = 100000
            };
        }
    }
}
