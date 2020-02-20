using System.Collections.Generic;
using Multitenant.Business.Interfaces;

namespace Multitenant.Business.Classes
{
    /// <inheritdoc />
    public class TenantHandler : ITenantHandler
    {
        private Dictionary<string,string> _tenantConnections;
        private ItestRepository _testRepository;

        public TenantHandler(ItestRepository testRepository)
        {
            _testRepository = testRepository;
            _tenantConnections = new Dictionary<string, string>(); 
            _tenantConnections.Add("tenant1", "connection1");
            _tenantConnections.Add("tenant2", "connection2");
            _tenantConnections.Add("tenant3", "connection3");
        }

        public string GetTenantConnectionString(string tenantId)
        {
            var list = _testRepository.GetAll();
            return "a";
        }

        public void AddTenantConnectionString(string tenantId, string connection)
        {
           _tenantConnections.Add(tenantId,connection);
         
        }
    }
}