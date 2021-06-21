using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;

namespace TicketManagement.WebMVC
{
    public class Program
    {
        protected Program()
        {
        }

        public static async Task Main(string[] args)
        {
            ConfigureLogging();
            try
            {
                await CreateHostBuilder(args).Build().RunAsync();
            }
            catch (Exception e)
            {
                Log.Fatal($"Failed to start {Assembly.GetExecutingAssembly().GetName().Name}", e);
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hostsettings.json", true)
                .AddCommandLine(args)
                .Build();

            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseUrls("https://*:5000").UseConfiguration(config);
            });
        }

        private static void ConfigureLogging()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configeration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(@"wwwroot\logs\log.txt", rollingInterval: RollingInterval.Day)
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(configeration)
                .CreateLogger();
        }
    }
}
