using Microsoft.AspNetCore.Identity;

namespace TicketManagement.WebMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Language { get; set; }

        public string TimeZoneOffset { get; set; }

        public decimal Balance { get; set; }
    }
}
