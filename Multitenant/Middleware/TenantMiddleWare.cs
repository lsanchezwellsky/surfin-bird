using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Multitenant.Business.Classes;
using Multitenant.Common;

namespace Multitenant.MiddleWare
{
    internal class TenantMiddleware<T> where T : Tenant
    {
        private readonly RequestDelegate next;

        public TenantMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Items.ContainsKey("HttpContextTenantKey"))
            {
                var tenantService = context.RequestServices.GetService(typeof(TenantAccessService<T>)) as TenantAccessService<T>;
                context.Items.Add("HttpContextTenantKey", await tenantService.GetTenantAsync());
            }

            //Continue processing
            if (next != null)
                await next(context);
        }
    }
}