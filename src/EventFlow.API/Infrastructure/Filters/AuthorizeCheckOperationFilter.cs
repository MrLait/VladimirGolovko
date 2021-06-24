using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TicketManagement.Services.EventFlow.API.Infrastructure.JwtTokenAuth;

namespace TicketManagement.Services.EventFlow.API.Infrastructure.Filters
{
    /// <summary>
    /// Authorize check operation filter.
    /// </summary>
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Apply.
        /// </summary>
        /// <param name="operation">Open api operation.</param>
        /// <param name="context">Operation filter context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = JwtAuthenticationConstants.Bearer },
            };

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new ()
                {
                    [jwtSecurityScheme] = new[] { JwtAuthenticationConstants.JwtSecurityScheme },
                },
            };
        }
    }
}
