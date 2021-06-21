using System;
using System.IO;
using System.Reflection;
using System.Text;
using Identity.API.DataAccess;
using Identity.API.Models;
using Identity.API.Services;
using Identity.API.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TicketManagement.DataAccess.DbContexts;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Services.Identity.API.Extensions;
using TicketManagement.Services.Identity.Domain.Models;

namespace TicketManagement.Services.Identity.API
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
            services.AddHealthChecks().AddCheck(
                "current_api_check",
                () => HealthCheckResult.Healthy("User API is alive"),
                new[] { "live" });

            services.AddScoped<IDbContext, EfDbContext>();
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddCustomDbContext(Configuration);

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationUserDbContext>()
                .AddDefaultTokenProviders();

            services.JwtTokenAuthService(Configuration);

            services.AddControllers();

            services.AddCustomSwagger(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRewriter(new RewriteOptions().AddRedirect("^$", "swagger"));
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "User API v1");
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains("live"),
                }).WithMetadata(new AllowAnonymousAttribute());
            });
        }
    }
}
