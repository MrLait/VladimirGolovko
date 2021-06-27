using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using TicketManagement.WebMVC.Infrastructure.Settings;

namespace TicketManagement.WebMVC
{
    /// <summary>
    /// Program.
    /// </summary>
    public class Program
    {
        private const string FailedToStart = "Failed to start";
        private const string AspnetcoreEnvironment = "ASPNETCORE_ENVIRONMENT";
        private const string LogFilePath = @"wwwroot\logs\log.txt";

        /// <summary>
        /// Initializes a new instance of the <see cref="Program"/> class.
        /// </summary>
        protected Program()
        {
        }

        /// <summary>
        /// Main.
        /// </summary>
        /// <param name="args">String arguments.</param>
        public static async Task Main(string[] args)
        {
            ConfigureLogging();
            try
            {
                await CreateHostBuilder(args).Build().RunAsync();
            }
            catch (Exception e)
            {
                Log.Fatal($"{FailedToStart} {Assembly.GetExecutingAssembly().GetName().Name}", e);
                throw;
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(JsonSettings.HostsettingsJson, true)
                .AddCommandLine(args)
                .Build();

            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseUrls().UseConfiguration(config);
            });
        }

        private static void ConfigureLogging()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(JsonSettings.AppsettingsJson, false, true)
                .AddJsonFile(string.Format(JsonSettings.AppsettingsEnvironmentJson, Environment.GetEnvironmentVariable(AspnetcoreEnvironment)), true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(LogFilePath, rollingInterval: RollingInterval.Day)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}
