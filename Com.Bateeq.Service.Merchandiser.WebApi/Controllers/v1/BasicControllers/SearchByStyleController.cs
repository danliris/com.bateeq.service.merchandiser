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
    [Route("v{version:apiVersion}/search-by-style")]
    [Authorize]
    public class SearchByStyleController : Controller
    {
        private static readonly string ApiVersion = "1.0";
        private SearchByStyleService searchByStyle;

        public SearchByStyleController(SearchByStyleService searchByStyle)
        {
            this.searchByStyle = searchByStyle;
        }

        [HttpGet("{stylename}")]
        public async Task<IActionResult> GetByStyle(string stylename)
        {
            try
            {
                if (stylename.Length <= 2)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                        .Fail();
                    return NotFound(Result);
                }
                else
                {
                    var model = await searchByStyle.ReadModelByStyle(stylename);

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