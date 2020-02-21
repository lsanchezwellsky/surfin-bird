using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Multitenant.Common.Multitenant;
using Repository.Entities;
using Repository.Interfaces;


namespace MultitenantAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ILogger<TenantController> _logger;
        private readonly IMultitenantRepository _multitenantRepository;

        public TenantController(ILogger<TenantController> logger, IMultitenantRepository multitenantRepository)
        {
            _logger = logger;
            _multitenantRepository = multitenantRepository;
            _logger.LogInformation("Accessed Tenant Controller");
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentTenants()
        {
            
            var records = await _multitenantRepository.GetAll();
            _logger.LogWarning(records.Count + " retrieved");
            return new ObjectResult(records);
           
        }

        [HttpPost]
        public async Task<IActionResult> AddTenantRecord([FromBody] MultitenantClient tenant)
        {
            await _multitenantRepository.Add(tenant);
            _logger.LogInformation("record added");
            return new ObjectResult("OK");
        }
    }
}