namespace TicketManagement.Services.EventFlow.API.Clients.IdentityClient
{
    /// <summary>
    /// Login model.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Gets or sets email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets sets init password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets remember me.
        /// </summary>
        public bool RememberMe { get; set; }
    }
}
