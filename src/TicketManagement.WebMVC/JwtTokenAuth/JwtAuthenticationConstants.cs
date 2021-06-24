namespace TicketManagement.WebMVC.JwtTokenAuth
{
    /// <summary>
    /// Jwt authentication constants.
    /// </summary>
    public static class JwtAuthenticationConstants
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
        public const string Bearer = "Bearer ";

        /// <summary>
        /// Secret jwt key.
        /// </summary>
        public const string SecretJwtKey = "secret_jwt_key";
    }
}