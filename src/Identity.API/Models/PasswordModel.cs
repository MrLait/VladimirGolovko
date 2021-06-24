namespace TicketManagement.Services.Identity.API.Models
{
    /// <summary>
    /// Password model.
    /// </summary>
    public class PasswordModel
    {
        /// <summary>
        /// Gets or sets UserId.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets OldPassword.
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// Gets or sets NewPassword.
        /// </summary>
        public string NewPassword { get; set; }
    }
}
