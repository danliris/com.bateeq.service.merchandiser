using Microsoft.AspNetCore.Mvc;
using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Com.Bateeq.Service.Merchandiser.WebApi.Helpers;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Com.Bateeq.Service.Merchandiser.WebApi.Controllers.v1.BasicControllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/sizes")]
    [Authorize]
    public class SizesController : BasicController<MerchandiserDbContext, SizeService, SizeViewModel, Size>
    {
        private static readonly string ApiVersion = "1.0";
        public SizesController(SizeService service) : base(service, ApiVersion)
        {
        }
    }
}