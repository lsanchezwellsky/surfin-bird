
using Multitenant.Common;


namespace Multitenant.Business.Interfaces
{
    public interface ITenantProvider
    {
        Tenant getCurrentTenant();
    }
}
