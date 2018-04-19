using Microsoft.AspNetCore.Mvc;
using Com.Bateeq.Service.Merchandiser.WebApi.Helpers;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Com.Bateeq.Service.Merchandiser.Lib.PdfTemplates;
using System.IO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Com.Bateeq.Service.Merchandiser.WebApi.Controllers.v1.BasicControllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/cost-calculation-garments")]
    [Authorize]
    public class CostCalculationGarmentsController : BasicController<MerchandiserDbContext, CostCalculationGarmentService, CostCalculationGarmentViewModel, CostCalculationGarment>
    {
        private static readonly string ApiVersion = "1.0";
        public CostCalculationGarmentsController(CostCalculationGarmentService service) : base(service, ApiVersion)
        {
        }

        [HttpGet("pdf/{id}")]
        public IActionResult GetPDF([FromRoute]int Id)
        {
            try
            {
                var model = Service.ReadModelById(Id).Result;
                var viewModel = Service.MapToViewModel(model);

                CostCalculationGarmentPdfTemplate PdfTemplate = new CostCalculationGarmentPdfTemplate();
                MemoryStream stream = PdfTemplate.GeneratePdfTemplate(viewModel);

                return new FileStreamResult(stream, "application/pdf")
                {
                    FileDownloadName = "CostCalculationGarment_" + viewModel.RO + ".pdf"
                };
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("budget/{id}")]
        public async Task<IActionResult> GetBudget([FromRoute]int Id)
        {
            try
            {
                Service.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;
                Service.Token = Request.Headers["Authorization"].First().Replace("Bearer ", "");

                await Service.GeneratePO(Id);
                var model = Service.ReadModelById(Id).Result;
                var viewModel = Service.MapToViewModel(model);

                CostCalculationGarmentBudgetPdfTemplate PdfTemplate = new CostCalculationGarmentBudgetPdfTemplate();
                MemoryStream stream = PdfTemplate.GeneratePdfTemplate(viewModel);

                return new FileStreamResult(stream, "application/pdf")
                {
                    FileDownloadName = "CostCalculationGarmentBudget_" + viewModel.RO + ".pdf"
                };
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
    }
}