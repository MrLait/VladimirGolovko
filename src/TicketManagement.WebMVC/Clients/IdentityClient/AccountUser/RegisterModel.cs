namespace TicketManagement.WebMVC.Clients.IdentityClient
{
    public record RegisterModel
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Language { get; set; }

        public string TimeZoneOffset { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordConfirm { get; set; }
    }
}
