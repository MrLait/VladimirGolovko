using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Ado;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.WebMVC.Extencions;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IEventAreaService, EventAreaService>();
            services.AddScoped<IEventSeatService, EventSeatService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IPurchaseHistoryService, PurchaseHistoryService>();
            services.AddScoped<IIdentityParser<ApplicationUser>, IdentityParser>();
            services.AddScoped<IDbContext, EfDbContext>();
            services.AddAutoMapper(typeof(MappingProfile));

            var connectionString = Configuration.GetConnectionString("TestConnection");
            services.AddDbContext<EfDbContext>(options =>
            {
                options.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging();
            });

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();
            services.AddAuthentication();
            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("ru"),
                new CultureInfo("be"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=EventHomePage}/{action=Index}/{id?}");
            });
        }
    }
}
