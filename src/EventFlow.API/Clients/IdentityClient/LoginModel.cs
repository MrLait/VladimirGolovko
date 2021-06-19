namespace TicketManagement.Services.EventFlow.API.Clients.IdentityClient
{
    public class LoginModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
