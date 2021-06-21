using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Identity.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Exceptions;
using TicketManagement.Services.Identity.Domain.Models;

namespace TicketManagement.Services.Identity.API
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
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                        await RoleInitializerModel.InitializeAsync(userManager, rolesManager);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("An error occurred while seeding the database {error}.", ex);
                    }
                }

                await host.RunAsync();
            }
            catch (Exception e)
            {
                Log.Fatal($"Failed to start {Assembly.GetExecutingAssembly().GetName().Name}", e);
                throw;
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
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
                .WriteTo.File(@"logs\log.txt", rollingInterval: RollingInterval.Day)
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(configeration)
                .CreateLogger();
        }
    }
}
