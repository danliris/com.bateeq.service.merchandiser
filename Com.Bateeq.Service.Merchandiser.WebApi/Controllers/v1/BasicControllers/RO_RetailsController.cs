using Microsoft.AspNetCore.Mvc;
using Com.Bateeq.Service.Merchandiser.WebApi.Helpers;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Com.Bateeq.Service.Merchandiser.WebApi.Controllers.v1.BasicControllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/ro-retails")]
    [Authorize]
    public class RO_RetailsController : BasicController<MerchandiserDbContext, RO_RetailService, RO_RetailViewModel, RO_Retail>
    {
        private static readonly string ApiVersion = "1.0";
        public RO_RetailsController(RO_RetailService service) : base(service, ApiVersion)
        {
        }
    }
}