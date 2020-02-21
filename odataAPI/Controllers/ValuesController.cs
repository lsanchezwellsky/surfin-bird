using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using odataAPI.Helpers;
using OdataBusinessQuery.Model;

namespace odataAPI.Controllers
{
    [Route("odata/[controller]")]
    //[Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValuesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // GET api/values
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {

            var url = "https://api-server:443/api/Test";
            var username = _httpContextAccessor.HttpContext.User.Identity.Name;
            _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Tenant", out StringValues tenantValue);
            var tenantId = tenantValue.FirstOrDefault();

            //var url = $"{_odataHost}{OdataOrganizationUrl}";
            var response = await HttpHelper.AsyncCall(url, username, "key1");
            if (response == null) return null;
            var result = JsonConvert.DeserializeObject<ICollection<TestModel>>(response.Value.ToString());
            return new ObjectResult(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
