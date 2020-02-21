using System;
using System.IO;
using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Multitenant.Business;
using Multitenant.Business.Classes.Example;
using Multitenant.Common;
using Multitenant.Common.Multitenant;
using MultiTenant.Middleware;
using Multitenant.Repository;
using Repository;
using Repository.Classes;
using Repository.Classes.MDW.openreferrals.Repository;
using Repository.Interfaces;
using Serilog;
using tenantPOC.Business.Classes;
using tenantPOC.OperationFilter;

namespace tenantPOC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private DbContextOptionsBuilder optionsBuilder;

        public static void ConfigureMultiTenantServices(Tenant t, ContainerBuilder c)
        {
            c.RegisterInstance(new OperationIdService(t)).SingleInstance();
            c.RegisterGeneric(typeof(GenericRepository<>))
                .As(typeof(IGenericRepository<>))
                .InstancePerLifetimeScope();
            c.RegisterType<TestRepository>().As<ItestRepository>();
            
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.ConfigureSwaggerGen(options =>
            {
                options.IncludeXmlComments(Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "xml"));
                options.OperationFilter<AddRequiredHeaderParameter>();

            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Description = "Multitenant Api for Hackaton", Title = "Cobra Kai Multitenant API", Version = "v1" });

            });
            services.AddScoped<IMultitenantRepository, MultitenantRepository>();
            services.AddDbContext<Sp5DbContext>();
            services.AddDbContext<MultitenantDbContext>(options => options.UseInMemoryDatabase(databaseName: "MultitenantDB"));

            services.AddMultiTenancy()
                .WithResolutionStrategy<HeadersTenantIdentificationService>()
                .WithStore<InMemoryTenantStore>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSerilogRequestLogging();
            var context = app.ApplicationServices.GetService<MultitenantDbContext>();
            AddTestData(context);
            app.UseMultiTenancy()
                .UseMultiTenantContainer();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Multitenant API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        private static void AddTestData(MultitenantDbContext context)
        {
            var testClient1 = new MultitenantClient
            {
                ClientId = 1,
                ClientKey = "key1",
                ConnectionString = "Connection1"
            };

            context.MultitenantClients.Add(testClient1);
            var testClient2 = new MultitenantClient
            {
                ClientId = 2,
                ClientKey = "key2",
                ConnectionString = "Connection2"
            };

            context.MultitenantClients.Add(testClient2);

            context.SaveChanges();
        }


    }
}
