using System.ComponentModel.DataAnnotations;

namespace TicketManagement.WebMVC.ViewModels.AccountViewModels
{
    public record RegisterViewModel
    {
        [Required(ErrorMessage = "UserNameRequired")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "Surname")]
        public string Surname { get; set; }

        public string Language { get; set; }

        public string TimeZoneOffset { get; set; }

        [Required(ErrorMessage = "EmailRequired")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "PasswordConfirmRequired")]
        [Compare("Password", ErrorMessage = "PasswordMismatch")]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        public string PasswordConfirm { get; set; }
    }
}
