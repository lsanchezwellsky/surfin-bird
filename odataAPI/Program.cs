using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;


namespace odataAPI
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
                CreateWebHostBuilder(args).Build().Run();
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

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseStartup<Startup>();
    }
}
