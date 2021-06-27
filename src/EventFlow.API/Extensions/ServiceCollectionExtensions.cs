using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using TicketManagement.DataAccess.DbContexts;
using TicketManagement.Services.EventFlow.API.Infrastructure.Filters;
using TicketManagement.Services.EventFlow.API.Infrastructure.JwtTokenAuth;
using TicketManagement.Services.EventFlow.API.Infrastructure.Swagger;

namespace TicketManagement.Services.EventFlow.API.Extensions
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
    }
}
