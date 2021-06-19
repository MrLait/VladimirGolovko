﻿using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Services.User.API.Models
{
    public class ProfileChangePasswordViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "NewPassword")]
        public string NewPassword { get; set; }

        [Display(Name = "OldPassword")]
        public string OldPassword { get; set; }
    }
}
