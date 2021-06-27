namespace TicketManagement.Services.Identity.API.Infrastructure.JwtTokenAuth
{
    /// <summary>
    /// Jwt authentication constants.
    /// </summary>
    internal static class JwtAuthenticationConstants
    {
        /// <summary>
        /// Scheme name.
        /// </summary>
        public const string SchemeName = "CustomJwtAuth";

        /// <summary>
        /// Authorization.
        /// </summary>
        public const string Authorization = "Authorization";

        /// <summary>
        /// Unauthorized.
        /// </summary>
        public const string Unauthorized = "Unauthorized";

        /// <summary>
        /// Bearer.
        /// </summary>
        public const string Bearer = "Bearer";

        /// <summary>
        /// Description.
        /// </summary>
        public const string Description = "Jwt Token is required to access the endpoints";

        /// <summary>
        /// Name.
        /// </summary>
        public const string Name = "JWT Authentication";

        /// <summary>
        /// Scheme.
        /// </summary>
        public const string Scheme = "bearer";

        /// <summary>
        /// BearerFormat.
        /// </summary>
        public const string BearerFormat = "JWT";

        /// <summary>
        /// JwtSecurityScheme.
        /// </summary>
        public const string JwtSecurityScheme = "identityApi";
    }
}
