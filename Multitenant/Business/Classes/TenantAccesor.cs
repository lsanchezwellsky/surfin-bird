
using Microsoft.AspNetCore.Http;
using Multitenant.Business.Classes;
using Multitenant.Business.Interfaces;
using Multitenant.Common;


namespace tenantPOC.Business.Classes
{
    public class TenantAccessor<T> : ITenantAccessor<T> where T : Tenant
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public T Tenant => _httpContextAccessor.HttpContext.GetTenant<T>();
    }
}
