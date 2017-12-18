using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Models = Com.Bateeq.Service.Merchandiser.Lib.Models;
using System;
using Xunit;
using Com.Bateeq.Service.Merchandiser.Test.Helpers;

namespace Com.Bateeq.Service.Merchandiser.Test.Service.Size
{
    [Collection("ServiceProviderFixture collection")]
    public class SizeBasicTest : BasicServiceTest<MerchandiserDbContext, SizeService, Models.Size>
    {
        private static readonly string[] createAttrAssertions = { "Code", "Name" };
        private static readonly string[] updateAttrAssertions = { "Code", "Name" };
        private static readonly string[] existAttrCriteria = { "Code" };

        public SizeBasicTest(ServiceProviderFixture fixture) : base(fixture, createAttrAssertions, updateAttrAssertions, existAttrCriteria)
        {
        }

        public override void EmptyCreateModel(Models.Size model)
        {
            model.Code = string.Empty;
            model.Name = string.Empty;
        }

        public override void EmptyUpdateModel(Models.Size model)
        {
            model.Code = string.Empty;
            model.Name = string.Empty;
        }

        public override Models.Size GenerateTestModel()
        {
            string guid = Guid.NewGuid().ToString();
            return new Models.Size()
            {
                Code = guid,
                Name = string.Format("TEST SIZE {0}", guid)
            };
        }
    }
}