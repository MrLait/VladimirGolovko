using Microsoft.AspNetCore.Identity;

namespace TicketManagement.Services.Identity.Domain.Models
{
    /// <summary>
    /// Application user.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
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
        /// Gets or sets time zone offset.
        /// </summary>
        public string TimeZoneOffset { get; set; }

        /// <summary>
        /// Gets or sets balance.
        /// </summary>
        public decimal Balance { get; set; }
    }
}
