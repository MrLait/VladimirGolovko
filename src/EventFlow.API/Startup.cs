using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TicketManagement.DataAccess.DbContexts;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Services.EventFlow.API.Clients.IdentityClient;
using TicketManagement.Services.EventFlow.API.Extensions;
using TicketManagement.Services.EventFlow.API.Infrastructure.HealthChecks;
using TicketManagement.Services.EventFlow.API.Infrastructure.JwtTokenAuth;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces;
using TicketManagement.Services.EventFlow.API.Infrastructure.Swagger;

namespace TicketManagement.Services.EventFlow.API
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
            services.AddHealthChecks().AddCheck<UserApiHealthCheck>(UserApiHealthCheck.UserApiHealthCheckName, tags: new[] { UserApiHealthCheckStatus.Ready });
            services.AddScopesCollection(Configuration);
            services.AddCustomDbContext(Configuration);

            services.AddOptions().Configure<ApiOptions>(binder => binder.IdentityApiAddress = Configuration[ApiOptions.IdentityApiAddressName]);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthentication(JwtAuthenticationConstants.SchemeName)
                .AddScheme<JwtAuthenticationOptions, JwtAuthenticationHandler>(JwtAuthenticationConstants.SchemeName, null);

            services.AddHttpClient<IUserClient, UserClient>((provider, client) =>
            {
                var userApiAddress = provider.GetService<IOptions<ApiOptions>>()?.Value.IdentityApiAddress;
                client.BaseAddress = new Uri(userApiAddress ?? string.Empty);
            });
            services.AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddCustomSwagger(Configuration);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder.</param>
        public void Configure(IApplicationBuilder app)
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

            app.UseAuthentication();
            app.UseRouting();
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
