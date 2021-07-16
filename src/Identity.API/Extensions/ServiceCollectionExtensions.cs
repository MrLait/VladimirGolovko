using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TicketManagement.DataAccess.DbContexts;
using TicketManagement.Services.Identity.API.DataAccess;
using TicketManagement.Services.Identity.API.Infrastructure.Filters;
using TicketManagement.Services.Identity.API.Infrastructure.JwtTokenAuth;
using TicketManagement.Services.Identity.API.Infrastructure.Services;
using TicketManagement.Services.Identity.API.Infrastructure.Swagger;
using TicketManagement.Services.Identity.API.Settings;

namespace TicketManagement.Services.Identity.API.Extensions
{
    /// <summary>
    /// Service collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add custom db context.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <param name="configuration">Configuration.</param>
        public static void AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            const string sectionName = "DefaultConnection";
            var connectionString = configuration.GetConnectionString(sectionName);
            services.AddDbContext<EfDbContext>(options =>
            {
                options.UseSqlServer(connectionString).EnableSensitiveDataLogging();
            });
            services.AddDbContext<ApplicationUserDbContext>(options => options.UseSqlServer(connectionString));
        }

        /// <summary>
        /// Add custom swagger.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <param name="configuration">Configuration.</param>
        public static void AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(SwaggerConstants.Version, new OpenApiInfo
                {
                    Title = SwaggerConstants.Title,
                    Version = SwaggerConstants.Version,
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition(JwtAuthenticationConstants.Bearer, new OpenApiSecurityScheme
                {
                    Description = JwtAuthenticationConstants.Description,
                    In = ParameterLocation.Header,
                    Name = JwtAuthenticationConstants.Name,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtAuthenticationConstants.Scheme,
                    BearerFormat = JwtAuthenticationConstants.BearerFormat,
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme,
                    },
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });
        }

        /// <summary>
        /// Jwt token auth service.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <param name="configuration">Configuration.</param>
        public static void JwtTokenAuthService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            var tokenSettings = configuration.GetSection(nameof(JwtTokenSettings));
            services.Configure<JwtTokenSettings>(tokenSettings);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = tokenSettings[nameof(JwtTokenSettings.JwtIssuer)],
                        ValidateAudience = true,
                        ValidAudience = tokenSettings[nameof(JwtTokenSettings.JwtAudience)],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings[nameof(JwtTokenSettings.JwtSecretKey)])),
                        ValidateLifetime = false,
                    };
                });
        }
    }
}
