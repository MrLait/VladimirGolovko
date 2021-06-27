namespace TicketManagement.Services.EventFlow.API.Clients.IdentityClient
{
    /// <summary>
    /// Register model.
    /// </summary>
    public record RegisterModel
    {
        /// <summary>
        /// Gets or sets user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets surname.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets language.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets time zone off set.
        /// </summary>
        public string TimeZoneOffset { get; set; }

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets password confirm.
        /// </summary>
        public string PasswordConfirm { get; set; }
    }
}
