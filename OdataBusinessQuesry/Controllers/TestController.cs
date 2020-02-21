//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using MDW.openreferralsApi.helpers;
//using Microsoft.AspNet.OData;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Primitives;
//using Newtonsoft.Json;
//using OdataBusinessQuery.Model;

//namespace OdataBusinessQuery.Controllers
//{

//    [ApiController]
//    public class TestController : ControllerBase
//    {
//        private readonly IHttpContextAccessor _httpContextAccessor;

//        public TestController(IHttpContextAccessor httpContextAccessor)
//        {
//            _httpContextAccessor = httpContextAccessor;
//        }


//        [HttpGet]
//        [Route("odata/tests")]
//        public async Task<IActionResult> Tests()
//        {
//            var url = "https://localhost:32813/api/Test";
//            var username = _httpContextAccessor.HttpContext.User.Identity.Name;
//            _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Tenant", out StringValues tenantValue);
//            var tenantId = tenantValue.FirstOrDefault();

//            //var url = $"{_odataHost}{OdataOrganizationUrl}";
//            var response= await HttpHelper.AsyncCall(url, username, "key1");
//            if (response == null) return null;
//            var result = JsonConvert.DeserializeObject<ICollection<TestModel>>(response.Value.ToString());
//            return new ObjectResult(result);
//        }


      
//    }
//}
