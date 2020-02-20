using System.Threading.Tasks;

namespace Multitenant.Business.Interfaces
{
    public interface ITenantHandler
    {
        public string GetTenantConnectionString(string tenantId);
        public void AddTenantConnectionString(string tenantId, string connection);
    }
}