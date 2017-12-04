using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Com.Bateeq.Service.Merchandiser.WebApi.Controllers.v1.BasicControllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/values")]
    [Authorize]
    public class ValuesController : Controller
    {
        public ValuesController()
        {
        }

        [HttpGet]
        [Authorize("service.core.read")]
        public IEnumerable<string> Get()
        {
            yield return "hello";
        }
    }
}
