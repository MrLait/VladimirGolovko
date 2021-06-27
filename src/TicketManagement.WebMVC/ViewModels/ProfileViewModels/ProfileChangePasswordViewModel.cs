using System.ComponentModel.DataAnnotations;

namespace TicketManagement.WebMVC.ViewModels.ProfileViewModels
{
    /// <summary>
    /// Profile change password view model.
    /// </summary>
    public class ProfileChangePasswordViewModel
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Gets or sets Email.
        /// </summary>
        [Display(Name = "Email")]
        public string Email { get; init; }

        /// <summary>
        /// Gets or sets NewPassword.
        /// </summary>
        [Display(Name = "NewPassword")]
        public string NewPassword { get; init; }

        /// <summary>
        /// Gets or sets OldPassword.
        /// </summary>
        [Display(Name = "OldPassword")]
        public string OldPassword { get; init; }
    }
}
