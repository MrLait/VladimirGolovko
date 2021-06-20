using System.Security.Principal;

namespace TicketManagement.Services.User.API.Services
{
    public interface IIdentityParser<T>
    {
        T Parse(IPrincipal principal);
    }
}
