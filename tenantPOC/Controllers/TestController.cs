using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Multitenant.Business.Classes;
using Multitenant.Business.Classes.Example;
using Multitenant.Common;
using Repository.Entities;
using Repository.Interfaces;

namespace MultitenantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ItestRepository _testRepository;
        private readonly ILogger<TenantController> _logger;

        /// <inheritdoc />
        public TestController(TenantAccessService<Tenant> tenantService, OperationIdService operationIdService, ItestRepository testRepository, ILogger<TenantController> logger)
        {
            _testRepository = testRepository;
            _logger = logger;
        }


        // GET: api/Test
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var records = await _testRepository.GetAll();
            _logger.LogWarning(records.Count + " retrieved");
            return new ObjectResult(records);
        }

        // GET: api/Test/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Test
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Test test)
        {
            //var newTest = new Test { Id = int.Parse(id), Name = name };
            await _testRepository.addTest(test);
            _logger.LogInformation("record added");
            return new ObjectResult("OK");
        }

        // PUT: api/Test/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
