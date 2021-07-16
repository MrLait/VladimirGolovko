namespace TicketManagement.Services.EventFlow.API.Clients.IdentityClient
{
    internal static class IdentityApiRequestUris
    {
        /// <summary>
        /// Validate token url.
        /// {0} - token.
        /// </summary>
        public const string ValidateToken = "AccountUser?token={0}";
    }
}