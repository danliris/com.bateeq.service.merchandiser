using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Models = Com.Bateeq.Service.Merchandiser.Lib.Models;
using System;
using Xunit;
using Com.Bateeq.Service.Merchandiser.Test.Helpers;

namespace Com.Bateeq.Service.Merchandiser.Test.Service.Category
{
    [Collection("ServiceProviderFixture collection")]
    public class CategoryBasicTest : BasicServiceTest<MerchandiserDbContext, CategoryService, Models.Category>
    {
        private static readonly string[] createAttrAssertions = { "Code", "Name" };
        private static readonly string[] updateAttrAssertions = { "Code", "Name" };
        private static readonly string[] existAttrCriteria = { "Code" };

        public CategoryBasicTest(ServiceProviderFixture fixture) : base(fixture, createAttrAssertions, updateAttrAssertions, existAttrCriteria)
        {
        }

        public override void EmptyCreateModel(Models.Category model)
        {
            model.Code = string.Empty;
            model.Name = string.Empty;
            model.Description = string.Empty;
        }

        public override void EmptyUpdateModel(Models.Category model)
        {
            model.Code = string.Empty;
            model.Name = string.Empty;
            model.Description = string.Empty;
        }

        public override Models.Category GenerateTestModel()
        {
            string guid = Guid.NewGuid().ToString();
            return new Models.Category()
            {
                Code = guid,
                Name = string.Format("TEST CATEGORY {0}", guid),
                Description = "TEST CATEGORY DESCRIPTION"
            };
        }
    }
}