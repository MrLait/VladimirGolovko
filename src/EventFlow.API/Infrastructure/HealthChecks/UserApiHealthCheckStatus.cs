namespace TicketManagement.Services.EventFlow.API.Infrastructure.HealthChecks
{
    /// <summary>
    /// User api health check status.
    /// </summary>
    internal static class UserApiHealthCheckStatus
    {
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
