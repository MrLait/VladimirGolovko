using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using TicketManagement.WebMVC.Extensions;
using TicketManagement.WebMVC.Infrastructure.Delegates;
using TicketManagement.WebMVC.Infrastructure.Filters;
using TicketManagement.WebMVC.JwtTokenAuth;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.Services;

namespace TicketManagement.WebMVC
{
    /// <summary>
    /// Startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configure services.
        /// </summary>
        /// <param name="services">Service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<RedirectToReactAppAttribute>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
            services.AddOptions().Configure<ApiOptions>(binder => binder.IdentityApiAddress = Configuration[ApiOptions.IdentityApiAddressName]);
            services.AddOptions().Configure<ApiOptions>(binder => binder.EventFlowApiAddress = Configuration[ApiOptions.EventFlowApiAddressName]);
            services.AddOptions().Configure<ApiOptions>(binder => binder.ReactAppAddress = Configuration[ApiOptions.ReactAppAddressName]);
            services.AddScoped<IIdentityParser<ApplicationUser>, IdentityParser>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddAuthentication(JwtAuthenticationConstants.SchemeName)
                .AddScheme<JwtAuthenticationOptions, JwtAuthenticationHandler>(JwtAuthenticationConstants.SchemeName, null);

            services.AddIdentityClients(Configuration);
            services.AddEventFlowClients(Configuration);

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();
            services.AddFeatureManagement();

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

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Web host environment.</param>
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