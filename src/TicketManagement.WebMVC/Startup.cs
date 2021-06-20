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
using TicketManagement.WebMVC.JwtTokenAuth;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.Services;

namespace TicketManagement.WebMVC
{
#pragma warning disable S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    public class Startup
#pragma warning restore S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
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

            AddIdentityClients(services);
            AddEventFlowClients(services);

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

#pragma warning disable S1541 // Methods and properties should not be too complex
        private static void AddEventFlowClients(IServiceCollection services)
#pragma warning restore S1541 // Methods and properties should not be too complex
        {
            services.AddHttpClient<IEventClient, EventClient>((provider, client) =>
            {
                var eventFlowApiAddress = provider.GetService<IOptions<ApiOptions>>()?.Value.EventFlowApiAddress;
                client.BaseAddress = new Uri(eventFlowApiAddress ?? string.Empty);
            });
            services.AddHttpClient<IEventAreaClient, EventAreaClient>((provider, client) =>
            {
                var eventFlowApiAddress = provider.GetService<IOptions<ApiOptions>>()?.Value.EventFlowApiAddress;
                client.BaseAddress = new Uri(eventFlowApiAddress ?? string.Empty);
            });
            services.AddHttpClient<IBasketClient, BasketClient>((provider, client) =>
            {
                var eventFlowApiAddress = provider.GetService<IOptions<ApiOptions>>()?.Value.EventFlowApiAddress;
                client.BaseAddress = new Uri(eventFlowApiAddress ?? string.Empty);
            });
            services.AddHttpClient<IPurchaseHistoryClient, PurchaseHistoryClient>((provider, client) =>
            {
                var eventFlowApiAddress = provider.GetService<IOptions<ApiOptions>>()?.Value.EventFlowApiAddress;
                client.BaseAddress = new Uri(eventFlowApiAddress ?? string.Empty);
            });
            services.AddHttpClient<IEventManagerClient, EventManagerClient>((provider, client) =>
            {
                var eventFlowApiAddress = provider.GetService<IOptions<ApiOptions>>()?.Value.EventFlowApiAddress;
                client.BaseAddress = new Uri(eventFlowApiAddress ?? string.Empty);
            });
        }

        private static void AddIdentityClients(IServiceCollection services)
        {
            services.AddHttpClient<IUserClient, UserClient>((provider, client) =>
            {
                var userApiAddress = provider.GetService<IOptions<ApiOptions>>()?.Value.UserApiAddress;
                client.BaseAddress = new Uri(userApiAddress ?? string.Empty);
            });
            services.AddHttpClient<IProfileClient, ProfileClient>((provider, client) =>
            {
                var userApiAddress = provider.GetService<IOptions<ApiOptions>>()?.Value.UserApiAddress;
                client.BaseAddress = new Uri(userApiAddress ?? string.Empty);
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