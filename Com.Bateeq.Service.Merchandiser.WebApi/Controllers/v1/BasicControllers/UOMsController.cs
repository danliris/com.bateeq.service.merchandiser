using Microsoft.AspNetCore.Mvc;
using Com.Bateeq.Service.Merchandiser.WebApi.Helpers;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;

namespace Com.Bateeq.Service.Merchandiser.WebApi.Controllers.v1.BasicControllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/UOMs")]
    public class UOMsController : BasicController<MerchandiserDbContext, UOMService, UOMViewModel, UOM>
    {
        private static readonly string ApiVersion = "1.0";
        public UOMsController(UOMService service) : base(service, ApiVersion)
        {
        }
    }
}