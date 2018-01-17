using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Models = Com.Bateeq.Service.Merchandiser.Lib.Models;
using System;
using Xunit;
using Com.Bateeq.Service.Merchandiser.Test.Helpers;

namespace Com.Bateeq.Service.Merchandiser.Test.Service.Efficiency
{
    [Collection("ServiceProviderFixture collection")]
    public class EfficiencyBasicTest : BasicServiceTest<MerchandiserDbContext, EfficiencyService, Models.Efficiency>
    {
        private static readonly string[] createAttrAssertions = { "InitialRange", "FinalRange", "Value" };
        private static readonly string[] updateAttrAssertions = { "InitialRange", "FinalRange", "Value" };
        private static readonly string[] existAttrCriteria = { };

        public EfficiencyBasicTest(ServiceProviderFixture fixture) : base(fixture, createAttrAssertions, updateAttrAssertions, existAttrCriteria)
        {
        }

        public override void EmptyCreateModel(Models.Efficiency model)
        {
        }

        public override void EmptyUpdateModel(Models.Efficiency model)
        {
        }

        public override Models.Efficiency GenerateTestModel()
        {
            return new Models.Efficiency()
            {
                InitialRange = 51,
                FinalRange = 100,
                Value = 51
            };
        }
    }
}