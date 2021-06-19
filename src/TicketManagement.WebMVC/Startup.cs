using System;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RestEase;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.DbContexts;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.WebMVC.Clients;
using TicketManagement.WebMVC.Clients.EventFlowClient;
using TicketManagement.WebMVC.Clients.EventFlowClient.Event;
using TicketManagement.WebMVC.Clients.IdentityClient;
using TicketManagement.WebMVC.JwtTokenAuth;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.Services;
using TicketManagement.WebMVC.Services.FileServices;

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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthentication(JwtAutheticationConstants.SchemeName)
                .AddScheme<JwtAuthenticationOptions, JwtAuthenticationHandler>(JwtAutheticationConstants.SchemeName, null);

            services.AddHttpClient<IUserClient, UserClient>((provider, client) =>
            {
                var userApiAddress = provider.GetService<IOptions<ApiOptions>>()?.Value.UserApiAddress;
                client.BaseAddress = new Uri(userApiAddress ?? string.Empty);
            });

            services.AddHttpClient<IEventClient, EventClient>((provider, client) =>
            {
                var eventFlowApiAddress = provider.GetService<IOptions<ApiOptions>>()?.Value.EventFlowApiAddress;
                client.BaseAddress = new Uri(eventFlowApiAddress ?? string.Empty);
            });

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
        //////    // This method gets called by the runtime. Use this method to add services to the container.
        //////    public void ConfigureServices(IServiceCollection services)
        //////    {
        //////        services.AddScoped<IEventService, EventService>();
        //////        services.AddScoped<IEventAreaService, EventAreaService>();
        //////        services.AddScoped<IEventSeatService, EventSeatService>();
        //////        services.AddScoped<IBasketService, BasketService>();
        //////        services.AddScoped<IApplicationUserService, ApplicationUserService>();
        //////        services.AddScoped<IPurchaseHistoryService, PurchaseHistoryService>();
        //////        services.AddScoped<IIdentityParser<ApplicationUser>, IdentityParser>();
        //////        services.AddScoped<IJsonSerializerService<ThirdPartyEvent>, JsonSerializerService<ThirdPartyEvent>>();
        //////        services.AddScoped<IDbContext, EfDbContext>();
        //////        services.AddScoped<IFileService, FileService>();
        //////        services.AddAutoMapper(typeof(MappingProfile));
        //////        services.Configure<RequestLocalizationOptions>(options =>
        //////        {
        //////            var supportedCultures = new[]
        //////            {
        //////            new CultureInfo("en"),
        //////            new CultureInfo("ru"),
        //////            new CultureInfo("be"),
        //////            };

//////            options.DefaultRequestCulture = new RequestCulture("ru");
//////            options.SupportedCultures = supportedCultures;
//////            options.SupportedUICultures = supportedCultures;
//////        });

//////        var connectionString = Configuration.GetConnectionString("DefaultConnection");
//////        services.AddDbContext<EfDbContext>(options =>
//////        {
//////            options.UseSqlServer(connectionString)
//////            .EnableSensitiveDataLogging();
//////        });

//////        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
//////        services.AddIdentity<ApplicationUser, IdentityRole>()
//////            .AddEntityFrameworkStores<ApplicationDbContext>()
//////            .AddDefaultTokenProviders();

//////        services.AddLocalization(options => options.ResourcesPath = "Resources");
//////        services.AddControllersWithViews()
//////            .AddDataAnnotationsLocalization()
//////            .AddViewLocalization();
//////        services.AddAuthentication();
//////        services.AddAuthorization();
//////    }

//////    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//////    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//////    {
//////        var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
//////        app.UseRequestLocalization(locOptions.Value);

//////        if (env.IsDevelopment())
//////        {
//////            app.UseDeveloperExceptionPage();
//////        }
//////        else
//////        {
//////            app.UseExceptionHandler("/Home/Error");
//////            //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//////            app.UseHsts();
//////        }

//////        app.UseHttpsRedirection();
//////        app.UseStaticFiles();

//////        app.UseRouting();

//////        app.UseAuthentication();
//////        app.UseAuthorization();

//////        app.UseEndpoints(endpoints =>
//////        {
//////            endpoints.MapControllerRoute(
//////                name: "default",
//////                pattern: "{controller=EventHomePage}/{action=Index}/{id?}");
//////        });
//////    }
//////}
