using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TicketManagement.DataAccess.DbContexts;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Services.Identity.API.DataAccess;
using TicketManagement.Services.Identity.API.Extensions;
using TicketManagement.Services.Identity.API.Infrastructure.HealthChecks;
using TicketManagement.Services.Identity.API.Infrastructure.Services;
using TicketManagement.Services.Identity.API.Infrastructure.Swagger;
using TicketManagement.Services.Identity.Domain.Models;

namespace TicketManagement.Services.Identity.API
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
            services.AddHealthChecks().AddCheck(
                UserApiHealthCheckStatus.UserApiHealthCheckName,
                () => HealthCheckResult.Healthy(UserApiHealthCheckStatus.Description),
                new[] { UserApiHealthCheckStatus.Live });

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

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Web host environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder =>
            builder.WithOrigins(Configuration[ApiOptions.ReactAppAddressName])
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());

            app.UseRewriter(new RewriteOptions().AddRedirect("^$", SwaggerConstants.Replacement));
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(SwaggerConstants.Url, SwaggerConstants.Name);
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHealthChecks(UserApiHealthCheckStatus.Pattern, new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains(UserApiHealthCheckStatus.Live),
                }).WithMetadata(new AllowAnonymousAttribute());
            });
        }
    }
}
