using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Bateeq.Service.Merchandiser.Test.Helpers;
using System;
using Models = Com.Bateeq.Service.Merchandiser.Lib.Models;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Bateeq.Service.Merchandiser.Test.Services.OTL
{
    [Collection("ServiceProviderFixture collection")]
    public class OTLBasicTest : BasicServiceTest<MerchandiserDbContext, OTLService, Models.OTL>
    {
        private static readonly string[] createAttrAssertions = { "Name", "Rate" };
        private static readonly string[] updateAttrAssertions = { "Name", "Rate" };
        private static readonly string[] existAttrCriteria = {};

        public OTLBasicTest(ServiceProviderFixture fixture) : base(fixture, createAttrAssertions, updateAttrAssertions, existAttrCriteria)
        {
        }

        public override void EmptyCreateModel(Models.OTL model)
        {
            model.Name = string.Empty;
        }

        public override void EmptyUpdateModel(Models.OTL model)
        {
            model.Name = string.Empty;
        }

        public override Models.OTL GenerateTestModel()
        {
            string guid = Guid.NewGuid().ToString();
            return new Models.OTL()
            {
                Code = guid,
                Name = string.Format("TEST OTL {0}", guid),
                Rate = 100000
            };
        }
    }
}
