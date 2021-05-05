using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketManagement.WebMVC.ViewModels.ProfileViewModels
{
    public class ProfileViewModel
    {
        public string Id { get; set; }

        public decimal Balance { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string TimeZone { get; set; }

        public string Language { get; set; }
    }
}
