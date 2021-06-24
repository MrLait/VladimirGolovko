using System.ComponentModel.DataAnnotations;

namespace TicketManagement.WebMVC.ViewModels.AccountViewModels
{
    /// <summary>
    /// Login view model.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Gets or sets email.
        /// </summary>
        [Required(ErrorMessage = "EmailRequired")]
        [Display(Name = "Email")]
        public string Email { get; init; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; init; }

        /// <summary>
        /// Gets or sets remember me.
        /// </summary>
        [Display(Name = "RememberMe")]
        public bool RememberMe { get; init; }
    }
}
