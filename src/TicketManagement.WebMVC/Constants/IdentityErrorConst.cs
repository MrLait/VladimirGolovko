namespace TicketManagement.WebMVC.Constants
{
    /// <summary>
    /// Identity error constants.
    /// </summary>
    public static class IdentityErrorConst
    {
        /// <summary>
        /// Password too short.
        /// </summary>
        public const string PasswordTooShort = "PasswordTooShort";

        /// <summary>
        /// Password requires non alphanumeric.
        /// </summary>
        public const string PasswordRequiresNonAlphanumeric = "PasswordRequiresNonAlphanumeric";

        /// <summary>
        /// Password requires digit.
        /// </summary>
        public const string PasswordRequiresDigit = "PasswordRequiresDigit";

        /// <summary>
        /// Password requires upper.
        /// </summary>
        public const string PasswordRequiresUpper = "PasswordRequiresUpper";

        /// <summary>
        /// Incorrect username and(or) password.
        /// </summary>
        public const string IncorrectUsernameAndOrPassword = "Incorrect username and(or) password";
    }
}
