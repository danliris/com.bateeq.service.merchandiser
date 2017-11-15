using Microsoft.AspNetCore.Mvc;
using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Com.Bateeq.Service.Merchandiser.WebApi.Helpers;

namespace Com.Bateeq.Service.Merchandiser.WebApi.Controllers.v1.BasicControllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/categories")]
    public class CategoriesController : BasicController<CategoryService, Category, CategoryViewModel, MerchandiserDbContext>
    {
        private static readonly string ApiVersion = "1.0";

        public CategoriesController(CategoryService service) : base(service, ApiVersion)
        {
        }
    }

}