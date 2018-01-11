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
        private static readonly string[] createAttrAssertions = { "Name", "SubCategory" };
        private static readonly string[] updateAttrAssertions = { "Name", "SubCategory" };
        private static readonly string[] existAttrCriteria = {};

        public CategoryBasicTest(ServiceProviderFixture fixture) : base(fixture, createAttrAssertions, updateAttrAssertions, existAttrCriteria)
        {
        }

        public override void EmptyCreateModel(Models.Category model)
        {
            model.Name = string.Empty;
            model.SubCategory = string.Empty;
        }

        public override void EmptyUpdateModel(Models.Category model)
        {
            model.Name = string.Empty;
            model.SubCategory = string.Empty;
        }

        public override Models.Category GenerateTestModel()
        {
            string guid = Guid.NewGuid().ToString();
            return new Models.Category()
            {
                Code = guid,
                Name = string.Format("Test Category {0}", guid),
                SubCategory = "Test Category Sub Category"
            };
        }
    }
}