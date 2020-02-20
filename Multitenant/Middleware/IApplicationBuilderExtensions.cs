using Microsoft.AspNetCore.Builder;
using Multitenant.Common;
using Multitenant.MiddleWare;

namespace MultiTenant.Middleware
{
    /// <summary>
    /// Nice method to register our middleware
    /// </summary>
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Use the Teanant Middleware to process the request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMultiTenancy<T>(this IApplicationBuilder builder) where T : Tenant
            => builder.UseMiddleware<TenantMiddleware<T>>();


        /// <summary>
        /// Use the Teanant Middleware to process the request
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder builder)
            => builder.UseMiddleware<TenantMiddleware<Tenant>>();


        /// <summary>
        /// Use the Teanant Middleware to process the request
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMultiTenantContainer(this IApplicationBuilder builder)
            => builder.UseMiddleware<MultitenantContainerMiddleWare<Tenant>>();

        /// <summary>
        /// Use the Teanant Middleware to process the request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMultiTenantContainer<T>(this IApplicationBuilder builder) where T : Tenant
            => builder.UseMiddleware<MultitenantContainerMiddleWare<T>>();
    }
}