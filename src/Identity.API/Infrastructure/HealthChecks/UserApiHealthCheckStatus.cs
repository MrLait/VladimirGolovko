namespace TicketManagement.Services.Identity.API.Infrastructure.HealthChecks
{
    /// <summary>
    /// User api health check status.
    /// </summary>
    internal static class UserApiHealthCheckStatus
    {
        /// <summary>
        /// User api health check name.
        /// </summary>
        public const string UserApiHealthCheckName = "current_api_check";

        /// <summary>
        /// Description.
        /// </summary>
        public const string Description = "User API is alive";

        /// <summary>
        /// Ready status.
        /// </summary>
        public const string Ready = "ready";

        /// <summary>
        /// Live status.
        /// </summary>
        public const string Live = "live";

        /// <summary>
        /// Pattern.
        /// </summary>
        public const string Pattern = "/health/live";
    }
}
