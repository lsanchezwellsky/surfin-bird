using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Multitenant.Business.Classes;
using Multitenant.Common;
using Serilog;
using Serilog.Events;


namespace tenantPOC
{
    public class Program
    {
        public static int Main(string[] args)
        {

            var logPath = Environment.GetEnvironmentVariable("LOGS_PATH");
            var logFileName = Environment.GetEnvironmentVariable("LOGS_FILE_NAME");


            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .WriteTo.File($"{logPath}{DateTime.Now:yyyy-MM-dd-}{logFileName}.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: null)
                .WriteTo.Console()
                .CreateLogger();

            // Wrap creating and running the host in a try-catch block
            try
            {
                Log.Information("Starting host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }


            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .UseServiceProviderFactory(
                    new MultiTenantServiceProviderFactory<Tenant>(Startup.ConfigureMultiTenantServices));
    }
}
