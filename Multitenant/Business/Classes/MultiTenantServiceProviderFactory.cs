using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Multitenant.Common;


namespace Multitenant.Business.Classes
{
    public class MultiTenantServiceProviderFactory<T> : IServiceProviderFactory<ContainerBuilder> where T : Tenant
    {

        public readonly Action<T, ContainerBuilder> TenantServicesConfiguration;

        public MultiTenantServiceProviderFactory(Action<T, ContainerBuilder> tenantServicesConfiguration)
        {
            TenantServicesConfiguration = tenantServicesConfiguration;
        }

        /// <summary>
        /// Create a builder populated with global services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.Populate(services);

            return builder;
        }

        /// <summary>
        /// Create our service provider
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <returns></returns>
        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            MultiTenantContainer<T> container = null;

            Func<MultiTenantContainer<T>> containerAccessor = () =>
            {
                return container;
            };

            containerBuilder
                .RegisterInstance(containerAccessor)
                .SingleInstance();

            container = new MultiTenantContainer<T>(containerBuilder.Build(), TenantServicesConfiguration);

            return new AutofacServiceProvider(containerAccessor());
        }
    }
}