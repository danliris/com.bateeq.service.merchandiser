using Microsoft.AspNetCore.Mvc;
using Com.Bateeq.Service.Merchandiser.WebApi.Helpers;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.IO;
using Com.Bateeq.Service.Merchandiser.Lib.PdfTemplates;

namespace Com.Bateeq.Service.Merchandiser.WebApi.Controllers.v1.BasicControllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/ro-garments")]
    [Authorize]
    public class RO_GarmentsController : BasicController<MerchandiserDbContext, RO_GarmentService, RO_GarmentViewModel, RO_Garment>
    {
        private static readonly string ApiVersion = "1.0";
        public RO_GarmentsController(RO_GarmentService service) : base(service, ApiVersion)
        {
        }

        [HttpGet("pdf/{id}")]
        public IActionResult GetPDF([FromRoute]int Id)
        {
            try
            {
                var model = Service.ReadModelById(Id).Result;
                var viewModel = Service.MapToViewModel(model);

                ROGarmentPdfTemplate PdfTemplate = new ROGarmentPdfTemplate();
                MemoryStream stream = PdfTemplate.GeneratePdfTemplate(viewModel);

                return new FileStreamResult(stream, "application/pdf")
                {
                    FileDownloadName = "RORetail_" + viewModel.CostCalculationGarment.RO + ".pdf"
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