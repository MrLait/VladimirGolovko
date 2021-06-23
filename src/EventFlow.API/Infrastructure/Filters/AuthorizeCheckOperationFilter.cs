using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

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
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
            };

            operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new ()
                    {
                        [jwtSecurityScheme] = new[] { "eventFlowApi" },
                    },
                };
        }
    }
}
