namespace TicketManagement.WebMVC.Clients.IdentityClient
{
    public static class IdentityApiRequestUries
    {
        public const string Register = "AccountUser/register";
        public const string Login = "AccountUser/login";

        /// <summary>
        /// {0} - token.
        /// </summary>
        public const string ValidateToken = "AccountUser/validate?token='{0}'";
    }
}
