using System.ComponentModel.DataAnnotations;

namespace TicketManagement.WebMVC.ViewModels.ProfileViewModels
{
    public class ProfileViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Balance")]
        public decimal Balance { get; set; }

        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "TimeZone")]
        public string TimeZoneOffset { get; set; }

        [Display(Name = "Language")]
        public string Language { get; set; }
    }
}
