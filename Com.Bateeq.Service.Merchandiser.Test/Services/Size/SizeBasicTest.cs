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
        private static readonly string[] createAttrAssertions = { "Name" };
        private static readonly string[] updateAttrAssertions = { "Name" };
        private static readonly string[] existAttrCriteria = { };

        public SizeBasicTest(ServiceProviderFixture fixture) : base(fixture, createAttrAssertions, updateAttrAssertions, existAttrCriteria)
        {
        }

        public override void EmptyCreateModel(Models.Size model)
        {
            model.Name = string.Empty;
        }

        public override void EmptyUpdateModel(Models.Size model)
        {
            model.Name = string.Empty;
        }

        public override Models.Size GenerateTestModel()
        {
            Models.Size model = new Models.Size();
            model.Name = string.Format("TEST SIZE {0}", model.Code);
            return model;
        }
    }
}