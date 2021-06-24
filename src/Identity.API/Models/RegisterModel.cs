using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Services.Identity.API.Models
{
    /// <summary>
    /// Register model.
    /// </summary>
    public record RegisterModel
    {
        /// <summary>
        /// Gets or sets UserName.
        /// </summary>
        [Required(ErrorMessage = "UserNameRequired")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets NewPasFirstNamesword.
        /// </summary>
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets Surname.
        /// </summary>
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets Language.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets TimeZoneOffset.
        /// </summary>
        public string TimeZoneOffset { get; set; }

        /// <summary>
        /// Gets or sets Email.
        /// </summary>
        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "Invalid address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        [Required(ErrorMessage = "PasswordRequired")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets PasswordConfirm.
        /// </summary>
        [Required(ErrorMessage = "PasswordConfirmRequired")]
        [Compare("Password", ErrorMessage = "PasswordMismatch")]
        [Display(Name = "ConfirmPassword")]
        public string PasswordConfirm { get; set; }
    }
}
