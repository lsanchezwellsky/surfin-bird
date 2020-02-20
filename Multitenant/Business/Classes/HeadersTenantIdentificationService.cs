using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Multitenant.Repository.Interfaces;


namespace tenantPOC.Business.Classes
{

    public sealed class HeadersTenantIdentificationService : ITenantResolutionStrategy
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HeadersTenantIdentificationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetTenantIdentifierAsync()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var tenant = _httpContextAccessor.HttpContext.Request.Headers["x-tenant"].ToString();
                //var tenant = context.Request.Query["Tenant"].ToString();
                return string.IsNullOrWhiteSpace(tenant) ? "" : tenant;
            }

            return "";
        }
    }
}
