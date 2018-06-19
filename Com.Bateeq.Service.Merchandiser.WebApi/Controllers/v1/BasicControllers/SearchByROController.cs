using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using Com.Bateeq.Service.Merchandiser.WebApi.Helpers;
using System;
using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Com.Bateeq.Service.Merchandiser.Lib.Services;

namespace Com.Bateeq.Service.Merchandiser.WebApi.Controllers.v1.BasicControllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/search-by-ro")]
    [Authorize]
    public class SearchByROController : Controller
    {
        private static readonly string ApiVersion = "1.0";
        private SearchByROService SearchByRO;

        public SearchByROController(SearchByROService searchByROService)
        {
            this.SearchByRO = searchByROService;
        }

        [HttpGet("{ro}")]
        public async Task<IActionResult> GetByRO(string ro)
        {
            try
            {
                if (ro.Length <= 2)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                        .Fail();
                    return NotFound(Result);
                }
                else
                {
                    var model = await SearchByRO.ReadModelByRO(ro);

                    Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(model);
                    return Ok(Result);
                }
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