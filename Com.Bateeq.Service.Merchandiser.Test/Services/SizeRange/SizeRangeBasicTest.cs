using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Models = Com.Bateeq.Service.Merchandiser.Lib.Models;
using System;
using Xunit;
using Com.Bateeq.Service.Merchandiser.Test.Helpers;
using System.Threading.Tasks;
using Com.Bateeq.Service.Merchandiser.Test.DataUtils;
using Microsoft.Extensions.DependencyInjection;
using Com.Bateeq.Service.Merchandiser.Lib.Models;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Test.Service.Size
{
    [Collection("ServiceProviderFixture collection")]
    public class SizeRangeBasicTest : BasicServiceTest<MerchandiserDbContext, SizeRangeService, Models.SizeRange>
    {
        private static readonly string[] createAttrAssertions = { };
        private static readonly string[] updateAttrAssertions = { };
        private static readonly string[] existAttrCriteria = { };

        public SizeRangeBasicTest(ServiceProviderFixture fixture) : base(fixture, createAttrAssertions, updateAttrAssertions, existAttrCriteria)
        {
        }

        protected SizeServiceDataUtil SizeDataUtil
        {
            get { return this.ServiceProvider.GetService<SizeServiceDataUtil>(); }
        }

        public override void EmptyCreateModel(Models.SizeRange model)
        {
        }

        public override void EmptyUpdateModel(Models.SizeRange model)
        {
        }

        public override Models.SizeRange GenerateTestModel()
        {
            Task<Models.Size> testSize = Task.Run(() => this.SizeDataUtil.GetTestSize());
            testSize.Wait();
            string guid = Guid.NewGuid().ToString();

            Models.SizeRange model = new Models.SizeRange();
            model.Code = guid;
            model.Name = string.Format("TEST SIZE RANGE {0}", guid);
            RelatedSize relatedSize = new RelatedSize
            {
                SizeId = testSize.Result.Id
            };
            model.RelatedSizes = new List<RelatedSize>();
            model.RelatedSizes.Add(relatedSize);

            return model;
        }
    }
}