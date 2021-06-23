namespace TicketManagement.WebMVC.Clients.IdentityClient
{
    /// <summary>
    /// Identity api request uris.
    /// </summary>
    public static class IdentityApiRequestUris
    {
        /// <summary>
        /// Account user register url.
        /// </summary>
        public const string Register = "AccountUser/register";

        /// <summary>
        /// Account user login url.
        /// </summary>
        public const string Login = "AccountUser/login";

        /// <summary>
        /// Validate token url.
        /// {0} - token.
        /// </summary>
        public const string ValidateToken = "AccountUser/validate?token={0}";

        /// <summary>
        /// Get user balance url.
        /// {0} - user id.
        /// </summary>
        public const string ProfileGetBalance = "Profile/getBalance?userId={0}";

        /// <summary>
        /// Update balance url.
        /// </summary>
        public const string ProfileUpdateBalance = "Profile/updateBalance";

        /// <summary>
        /// Get user profile url.
        /// {0} - user id.
        /// {1} - new balance.
        /// </summary>
        public const string ProfileGetUserProfile = "Profile/getUserProfile?userId={0}";

        /// <summary>
        /// Set language url.
        /// </summary>
        public const string ProfileSetLanguage = "Profile/setLanguage";

        /// <summary>
        /// Deposit url.
        /// </summary>
        public const string ProfileDeposit = "Profile/deposit";

        /// <summary>
        /// Edit email url.
        /// </summary>
        public const string ProfileEditEmail = "Profile/editEmail";

        /// <summary>
        /// Edit first name url.
        /// </summary>
        public const string ProfileEditFirstName = "Profile/editFirstName";

        /// <summary>
        /// Edit password url.
        /// </summary>
        public const string ProfileEditPassword = "Profile/editPassword";

        /// <summary>
        /// Edit surname url.
        /// </summary>
        public const string ProfileEditSurname = "Profile/editSurname";

        /// <summary>
        /// Edit time zone offset url.
        /// </summary>
        public const string ProfileEditTimeZoneOffset = "Profile/editTimeZoneOffset";
    }
}
