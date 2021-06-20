namespace TicketManagement.WebMVC.Clients.IdentityClient
{
    public static class IdentityApiRequestUries
    {
        public const string Register = "AccountUser/register";
        public const string Login = "AccountUser/login";

        /// <summary>
        /// {0} - token.
        /// </summary>
        public const string ValidateToken = "AccountUser/validate?token={0}";

        /// <summary>
        /// {0} - user id.
        /// </summary>
        public const string ProfileGetBalance = "Profile/getBalance?userId={0}";

        /// <summary>
        /// {0} - user id.
        /// {1} - new balance.
        /// </summary>
        public const string ProfileUpdateBalance = "Profile/updateBalance?userId={0}&balance={1}";

        /// <summary>
        /// {0} - user id.
        /// {1} - new balance.
        /// </summary>
        public const string ProfileGetUserProfile = "Profile/getUserProfile?userId={0}";

        /// <summary>
        /// {0} - user id.
        /// {1} - culture.
        /// </summary>
        public const string ProfileSetLanguage = "Profile/setLanguage?userId={0}&culture={1}";

        /// <summary>
        /// {0} - user id.
        /// {1} - balance.
        /// </summary>
        public const string ProfileDeposite = "Profile/deposite?userId={0}&balance={1}";

        /// <summary>
        /// {0} - user id.
        /// {1} - email.
        /// </summary>
        public const string ProfileEditEmail = "Profile/editEmail?userId={0}&email={1}";

        /// <summary>
        /// {0} - user id.
        /// {1} - new firstName.
        /// </summary>
        public const string ProfileEditFirstName = "Profile/editFirstName?userId={0}&firstName={1}";

        /// <summary>
        /// {0} - user id.
        /// {1} - old password.
        /// {2} - new password.
        /// </summary>
        public const string ProfileEditPassword = "Profile/editPassword?userId={0}&oldPassword={1}&newPassword={2}";

        /// <summary>
        /// {0} - user id.
        /// {1} - new surname.
        /// </summary>
        public const string ProfileEditSurname = "Profile/editSurname?userId={0}&surname={1}";

        /// <summary>
        /// {0} - user id.
        /// {1} - new time zone off set.
        /// </summary>
        public const string ProfileEditTimeZoneOffset = "Profile/editTimeZoneOffse?userId={0}&timeZoneOffset={1}";
    }
}
