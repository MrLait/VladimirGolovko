using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketManagement.WebMVC.ViewModels.ProfileViewModels
{
    public class ProfileChangePasswordViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string NewPassword { get; set; }

        public string OldPassword { get; set; }
    }
}
