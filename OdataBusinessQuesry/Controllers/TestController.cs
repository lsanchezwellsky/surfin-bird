using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MDW.openreferralsApi.helpers;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Repository.Entities;

namespace OdataBusinessQuery.Controllers
{
    [Route("Tests")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TestController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet]
        [EnableQuery()]
        public async Task<ActionResult<IEnumerable<Test>>> Tests()
        {
            var username = _httpContextAccessor.HttpContext.User.Identity.Name;
            _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Tenant", out StringValues tenantValue);
            var tenantId = tenantValue.FirstOrDefault();

            //var url = $"{_odataHost}{OdataOrganizationUrl}";
            return await HttpHelper.AsyncCall(url, username, tenantId);
        }
    }
}
