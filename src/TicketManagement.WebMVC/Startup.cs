using System;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TicketManagement.WebMVC.Clients.EventFlowClient.Basket;
using TicketManagement.WebMVC.Clients.EventFlowClient.Event;
using TicketManagement.WebMVC.Clients.EventFlowClient.EventArea;
using TicketManagement.WebMVC.Clients.EventFlowClient.EventManager;
using TicketManagement.WebMVC.Clients.EventFlowClient.PurchaseHistory;
using TicketManagement.WebMVC.Clients.IdentityClient.AccountUser;
using TicketManagement.WebMVC.Clients.IdentityClient.Profile;
using TicketManagement.WebMVC.Extensions;
using TicketManagement.WebMVC.JwtTokenAuth;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.Services;

namespace TicketManagement.WebMVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions().Configure<ApiOptions>(binder => binder.UserApiAddress = Configuration["UserApiAddress"]);
            services.AddOptions().Configure<ApiOptions>(binder => binder.EventFlowApiAddress = Configuration["EventFlowApiAddress"]);
            services.AddScoped<IIdentityParser<ApplicationUser>, IdentityParser>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddAuthentication(JwtAutheticationConstants.SchemeName)
                .AddScheme<JwtAuthenticationOptions, JwtAuthenticationHandler>(JwtAutheticationConstants.SchemeName, null);

            services.AddIdentityClients(Configuration);
            services.AddEventFlowClients(Configuration);

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru"),
                    new CultureInfo("be"),
                };

                options.DefaultRequestCulture = new RequestCulture("ru");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=EventHomePage}/{action=Index}/{id?}");
            });
        }
    }
}