using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TicketManagement.WebMVC.Models;

namespace TicketManagement.WebMVC
{
    public class Program
    {
        protected Program()
        {
        }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            ////using (var scope = host.Services.CreateScope())
            ////{
            ////    var services = scope.ServiceProvider;
            ////    try
            ////    {
            ////        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            ////        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            ////        await RoleInitializerModel.InitializeAsync(userManager, rolesManager);
            ////    }
            ////    catch (Exception ex)
            ////    {
            ////        var logger = services.GetRequiredService<ILogger<Program>>();
            ////        logger.LogError(ex, "An error occurred while seeding the database.");
            ////    }
            ////}

            //// host();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hostsettings.json", true)
                .AddCommandLine(args)
                .Build();

            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseUrls("https://*:5000").UseConfiguration(config);
            });
        }
    }
}
