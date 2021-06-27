using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Services.Identity.API.Models
{
    /// <summary>
    /// Login model.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Gets or sets Email.
        /// </summary>
        [Required(ErrorMessage = "EmailRequired")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets RememberMe.
        /// </summary>
        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }
}
