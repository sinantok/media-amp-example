using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace ClientMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "CmsApi")
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .MinimumLevel.Verbose()
                .CreateLogger();
            }
            try
            {
                Log.Information("Application Start");
                host.Run();
            }
            catch (Exception ex)
            {

                Log.Fatal(ex, "Application Start-up Failed");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
