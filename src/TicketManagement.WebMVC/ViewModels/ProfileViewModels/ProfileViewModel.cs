using System.ComponentModel.DataAnnotations;

namespace TicketManagement.WebMVC.ViewModels.ProfileViewModels
{
    /// <summary>
    /// Profile view model.
    /// </summary>
    public class ProfileViewModel
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets Balance.
        /// </summary>
        [Display(Name = "Balance")]
        public decimal Balance { get; set; }

        /// <summary>
        /// Gets or sets FirstName.
        /// </summary>
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets Surname.
        /// </summary>
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets Email.
        /// </summary>
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets TimeZoneOffset.
        /// </summary>
        [Display(Name = "TimeZone")]
        public string TimeZoneOffset { get; set; }

        /// <summary>
        /// Gets or sets Language.
        /// </summary>
        [Display(Name = "Language")]
        public string Language { get; set; }
    }
}
