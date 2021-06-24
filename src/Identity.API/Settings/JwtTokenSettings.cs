namespace TicketManagement.Services.Identity.API.Settings
{
    /// <summary>
    /// Jwt token settings.
    /// </summary>
    public class JwtTokenSettings
    {
        /// <summary>
        /// Gets or sets JwtIssuer.
        /// </summary>
        public string JwtIssuer { get; set; }

        /// <summary>
        /// Gets or sets JwtAudience.
        /// </summary>
        public string JwtAudience { get; set; }

        /// <summary>
        /// Gets or sets JwtSecretKey.
        /// </summary>
        public string JwtSecretKey { get; set; }
    }
}
