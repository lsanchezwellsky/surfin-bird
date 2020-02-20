using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Multitenant.Business.Classes;
using Multitenant.Business.Classes.Example;
using Multitenant.Common;
using Repository.Entities;
using Repository.Interfaces;


namespace MultitenantAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TenantController : ControllerBase
    {

        private readonly TenantAccessService<Tenant> _tenantService;
        private readonly OperationIdService _operationIdService;
        private readonly ItestRepository _testRepository;
        private readonly ILogger<TenantController> _logger;

        public TenantController(TenantAccessService<Tenant> tenantService, OperationIdService operationIdService, ItestRepository testRepository, ILogger<TenantController> logger)
        {
            _tenantService = tenantService;
            _operationIdService = operationIdService;
            _testRepository = testRepository;
            _logger = logger;
            _logger.LogInformation("Accessed Tenant Controller");
        }

        [HttpGet]
        public async Task<IActionResult> GetRecordsByTenant()
        {
            
            var records = await _testRepository.GetAll();
            _logger.LogWarning(records.Count + " retrieved");
            return new ObjectResult(records);
           
        }

        [HttpGet]
        public async Task<IActionResult> AddTenantRecord(string id, string name)
        {
            
            var newTest = new Test {Id = int.Parse(id), Name = name};
            await _testRepository.addTest(newTest);
            _logger.LogInformation("record added");
            return new ObjectResult("OK");
        }
    }
}