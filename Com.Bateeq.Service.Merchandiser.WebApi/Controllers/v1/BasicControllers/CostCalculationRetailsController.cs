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
    [Route("v{version:apiVersion}/cost-calculation-retails")]
    [Authorize]
    public class CostCalculationRetailsController : BasicController<MerchandiserDbContext, CostCalculationRetailService, CostCalculationRetailViewModel, CostCalculationRetail>
    {
        private static readonly string ApiVersion = "1.0";
        private readonly RateService _rateService;

        public CostCalculationRetailsController(CostCalculationRetailService service, RateService rateService) : base(service, ApiVersion)
        {
            _rateService = rateService;
        }

        [HttpGet("pdf/{id}")]
        public IActionResult GetPDF([FromRoute]int Id)
        {
            try
            {
                var model = Service.ReadModelById(Id).Result;
                var viewModel = Service.MapToViewModel(model);

                CostCalculationRetailPdfTemplate PdfTemplate = new CostCalculationRetailPdfTemplate();
                MemoryStream stream = PdfTemplate.GeneratePdfTemplate(viewModel);

                return new FileStreamResult(stream, "application/pdf")
                {
                    FileDownloadName = "Cost Calculation Retail " + viewModel.RO + ".pdf"
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

        [HttpGet("specol/{id}")]
        public IActionResult GetSpecol([FromRoute]int Id)
        {
            try
            {
                var model = Service.ReadModelById(Id).Result;
                var viewModel = Service.MapToViewModel(model);

                CostCalculationSpecolPdfTemplate PdfTemplate = new CostCalculationSpecolPdfTemplate();
                MemoryStream stream = PdfTemplate.GeneratePdfTemplate(viewModel);

                return new FileStreamResult(stream, "application/pdf")
                {
                    FileDownloadName = "Cost Calculation Retail " + viewModel.RO + ".pdf"
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

        [HttpGet("vvip/{id}")]
        public IActionResult GetVvip([FromRoute]int Id)
        {
            try
            {
                var model = Service.ReadModelById(Id).Result;
                var viewModel = Service.MapToViewModel(model);

                CostCalculationVvipPdfTemplate PdfTemplate = new CostCalculationVvipPdfTemplate();
                MemoryStream stream = PdfTemplate.GeneratePdfTemplate(viewModel);

                return new FileStreamResult(stream, "application/pdf")
                {
                    FileDownloadName = "Cost Calculation Retail " + viewModel.RO + ".pdf"
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
                var rateModel = _rateService.Get().SingleOrDefault(x => x.Name == "THR"); //Get Default THR From Rate (Master Ongkos)
                var viewModel = Service.MapToViewModel(model);
                var rateViewModel = _rateService.MapToViewModel(rateModel);
                viewModel.Thr = rateViewModel;

                CostCalculationRetailBudgetPdfTemplate PdfTemplate = new CostCalculationRetailBudgetPdfTemplate();
                MemoryStream stream = PdfTemplate.GeneratePdfTemplate(viewModel);

                return new FileStreamResult(stream, "application/pdf")
                {
                    FileDownloadName = "CostCalculationRetailBudget_" + viewModel.RO + ".pdf"
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