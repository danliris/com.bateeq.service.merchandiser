using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Models = Com.Bateeq.Service.Merchandiser.Lib.Models;
using System;
using Xunit;
using Com.Bateeq.Service.Merchandiser.Test.Helpers;

namespace Com.Bateeq.Service.Merchandiser.Test.Service.UOM
{
    [Collection("ServiceProviderFixture collection")]
    public class UOMBasicTest : BasicServiceTest<MerchandiserDbContext, UOMService, Models.UOM>
    {
        private static readonly string[] createAttrAssertions = { };
        private static readonly string[] updateAttrAssertions = { };
        private static readonly string[] existAttrCriteria = { "Code" };

        public UOMBasicTest(ServiceProviderFixture fixture) : base(fixture, createAttrAssertions, updateAttrAssertions, existAttrCriteria)
        {
        }

        public override void EmptyCreateModel(Models.UOM model)
        {
        }

        public override void EmptyUpdateModel(Models.UOM model)
        {
        }

        public override Models.UOM GenerateTestModel()
        {
            string guid = Guid.NewGuid().ToString();
            return new Models.UOM()
            {
                Code = guid,
                Name = string.Format("Test UOM {0}", guid)
            };
        }
    }
}