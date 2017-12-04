using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Models = Com.Bateeq.Service.Merchandiser.Lib.Models;
using System;
using Xunit;
using Com.Bateeq.Service.Merchandiser.Test.DataUtils;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Com.Bateeq.Service.Merchandiser.Test.Helpers;

namespace Com.Bateeq.Service.Merchandiser.Test.Service.Material
{
    [Collection("ServiceProviderFixture collection")]
    public class MaterialBasicTest : BasicServiceTest<MerchandiserDbContext, MaterialService, Models.Material>
    {
        private static readonly string[] createAttrAssertions = { "Code", "Name" };
        private static readonly string[] updateAttrAssertions = { "Code", "Name" };
        private static readonly string[] existAttrCriteria = { "Code" };

        public MaterialBasicTest(ServiceProviderFixture fixture) : base(fixture, createAttrAssertions, updateAttrAssertions, existAttrCriteria)
        {
        }

        protected CategoryServiceDataUtil CategoryDataUtil
        {
            get { return this.ServiceProvider.GetService<CategoryServiceDataUtil>(); }
        }

        public override void EmptyCreateModel(Models.Material model)
        {
            model.Code = string.Empty;
            model.Name = string.Empty;
            model.Description = string.Empty;
        }

        public override void EmptyUpdateModel(Models.Material model)
        {
            model.Code = string.Empty;
            model.Name = string.Empty;
            model.Description = string.Empty;
        }

        public override Models.Material GenerateTestModel()
        {
            Task<Models.Category> testCategory = Task.Run(() => this.CategoryDataUtil.GetTestCategory());
            testCategory.Wait();
            string guid = Guid.NewGuid().ToString();
            return new Models.Material()
            {
                CategoryId = testCategory.Result.Id,
                Code = guid,
                Name = string.Format("TEST CATEGORY {0}", guid),
                Description = "TEST CATEGORY DESCRIPTION"
            };
        }
    }
}