using System.ComponentModel.DataAnnotations;

namespace TicketManagement.WebMVC.ViewModels.AccountViewModels
{
    /// <summary>
    /// Register view model.
    /// </summary>
    public record RegisterViewModel
    {
        /// <summary>
        /// Gets or sets user name.
        /// </summary>
        [Required(ErrorMessage = "UserNameRequired")]
        [Display(Name = "UserName")]
        public string UserName { get; init; }

        /// <summary>
        /// Gets or sets FirstName.
        /// </summary>
        [Display(Name = "FirstName")]
        public string FirstName { get; init; }

        /// <summary>
        /// Gets or sets surname.
        /// </summary>
        [Display(Name = "Surname")]
        public string Surname { get; init; }

        /// <summary>
        /// Gets or sets language.
        /// </summary>
        public string Language { get; init; }

        /// <summary>
        /// Gets or sets TimeZoneOffset.
        /// </summary>
        public string TimeZoneOffset { get; init; }

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "Invalid address")]
        [Display(Name = "Email")]
        public string Email { get; init; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        [Required(ErrorMessage = "PasswordRequired")]
        [Display(Name = "Password")]
        public string Password { get; init; }

        /// <summary>
        /// Gets or sets confirm password.
        /// </summary>
        [Required(ErrorMessage = "PasswordConfirmRequired")]
        [Compare("Password", ErrorMessage = "PasswordMismatch")]
        [Display(Name = "ConfirmPassword")]
        public string PasswordConfirm { get; init; }
    }
}
