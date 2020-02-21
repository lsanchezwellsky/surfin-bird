using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using odataAPI.MiddleWare;
using odataAPI.OperationFilter;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using static System.String;

namespace odataAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.ConfigureSwaggerGen(options =>
            {
                options.IncludeXmlComments(Path.ChangeExtension(Assembly.GetEntryAssembly()?.Location, "xml"));
                options.OperationFilter<ODataParametersSwaggerDefinition>();
            });

            services.AddOData();
            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Odata API",
                    Description = "Cobra Kai Team Odata API",
                    TermsOfService = "Just for use in the Wellsky Hackaton 2020"

                });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            app.UseSerilogRequestLogging();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Odata API V1");
                c.RoutePrefix = Empty;
            });
            var builder = new ODataConventionModelBuilder(app.ApplicationServices);
            app.UseMvc(routebuilder =>
            {
                routebuilder.MapODataServiceRoute("ODataRoute", "odata", builder.GetEdmModel());
                routebuilder.EnableDependencyInjection();
                routebuilder.Expand().Select().OrderBy().Filter().MaxTop(null).Count();
            });
      
        }
    }
}
