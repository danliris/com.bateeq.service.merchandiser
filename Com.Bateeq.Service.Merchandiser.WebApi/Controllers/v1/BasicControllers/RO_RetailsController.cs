using Microsoft.AspNetCore.Mvc;
using Com.Bateeq.Service.Merchandiser.WebApi.Helpers;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Com.Bateeq.Service.Merchandiser.Lib.PdfTemplates;
using System.IO;

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

        [HttpGet("pdf/{id}")]
        public IActionResult GetPDF([FromRoute]int Id)
        {
            try
            {
                var model = Service.ReadModelById(Id).Result;
                var viewModel = Service.MapToViewModel(model);

                RoRetailPdfTemplate PdfTemplate = new RoRetailPdfTemplate();
                MemoryStream stream = PdfTemplate.GeneratePdfTemplate(viewModel);

                return new FileStreamResult(stream, "application/pdf")
                {
                    FileDownloadName = "RORetail_" + viewModel.CostCalculationRetail.RO + ".pdf"
                };
            }
            catch (System.Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
    }
}