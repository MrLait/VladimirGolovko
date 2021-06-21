using System;
using System.IO;
using System.Reflection;
using EventFlow.API.JwtTokenAuth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
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
using TicketManagement.Services.EventFlow.API.Infrastructure.Services;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces;

namespace TicketManagement.Services.EventFlow.API
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
            services.AddHealthChecks().AddCheck<UserApiHealthcheck>("user_api_check", tags: new[] { "ready" });
            services.AddScoped<IEventSeatService, EventSeatService>();
            services.AddScoped<IEventAreaService, EventAreaService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IPurchaseHistoryService, PurchaseHistoryService>();
            services.AddScoped<IDbContext, EfDbContext>();

            services.AddCustomDbContext(Configuration);

            services.AddOptions().Configure<UserApiOptions>(binder => binder.IdentityApiAddress = Configuration["IdentityApiAddress"]);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthentication(JwtAutheticationConstants.SchemeName)
                .AddScheme<JwtAuthenticationOptions, JwtAuthenticationHandler>(JwtAutheticationConstants.SchemeName, null);

            services.AddHttpClient<IUserClient, UserClient>((provider, client) =>
            {
                var userApiAddress = provider.GetService<IOptions<UserApiOptions>>()?.Value.IdentityApiAddress;
                client.BaseAddress = new Uri(userApiAddress ?? string.Empty);
            });
            services.AddControllers();

            services.AddCustomSwagger(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRewriter(new RewriteOptions().AddRedirect("^$", "swagger"));
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "EventFlow API v1");
            });

            app.UseAuthentication();
            app.UseRouting();
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
