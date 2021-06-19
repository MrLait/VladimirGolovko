using System;
using System.Globalization;
using System.Text;
using Identity.API.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

            ConfigureAuthorization(services);

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

            app.UseAuthentication();
            app.UseRouting();
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

#pragma warning disable S1144 // Unused private types or members should be removed
        private void ConfigureAuthorization(IServiceCollection services)
        {
            var tokenSettings = Configuration.GetSection(nameof(JwtTokenSettings));
            services.AddAuthentication(JwtAutheticationConstants.SchemeName)
                .AddScheme<JwtAuthenticationOptions, JwtAuthenticationHandler>(JwtAutheticationConstants.SchemeName, null)
                .AddJwtBearer(options =>
                {
                    var jwtSecurityScheme = new OpenApiSecurityScheme
                    {
                        Description = "Jwt Token is required to access the endpoints",
                        In = ParameterLocation.Header,
                        Name = "JWT Authentication",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Reference = new OpenApiReference
                        {
                            Id = JwtBearerDefaults.AuthenticationScheme,
                            Type = ReferenceType.SecurityScheme,
                        },
                    };

                    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() },
                });
                    ////options.
                    ////options.RequireHttpsMetadata = false;
                    ////options.TokenValidationParameters = new TokenValidationParameters
                    ////{
                    ////    ValidateIssuer = true,
                    ////    ValidIssuer = tokenSettings[nameof(JwtTokenSettings.JwtIssuer)],
                    ////    ValidateAudience = true,
                    ////    ValidAudience = tokenSettings[nameof(JwtTokenSettings.JwtAudience)],
                    ////    ValidateIssuerSigningKey = true,
                    ////    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings[nameof(JwtTokenSettings.JwtSecretKey)])),
                    ////    ValidateLifetime = false,
                    ////};
                });
            services.Configure<JwtTokenSettings>(tokenSettings);

            ////var tokenSettings = Configuration.GetSection(nameof(JwtTokenSettings));
            ////services.AddAuthentication(options =>
            ////{
            ////    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            ////    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            ////    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            ////})
            ////    .AddJwtBearer(options =>
            ////    {
            ////        options.RequireHttpsMetadata = false;
            ////        options.TokenValidationParameters = new TokenValidationParameters
            ////        {
            ////            ValidateIssuer = true,
            ////            ValidIssuer = tokenSettings[nameof(JwtTokenSettings.JwtIssuer)],
            ////            ValidateAudience = true,
            ////            ValidAudience = tokenSettings[nameof(JwtTokenSettings.JwtAudience)],
            ////            ValidateIssuerSigningKey = true,
            ////            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings[nameof(JwtTokenSettings.JwtSecretKey)])),
            ////            ValidateLifetime = false,
            ////        };
            ////    });
            ////services.Configure<JwtTokenSettings>(tokenSettings);
            ////services.AddScoped<JwtTokenService>();
        }
#pragma warning restore S1144 // Unused private types or members should be removed

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
