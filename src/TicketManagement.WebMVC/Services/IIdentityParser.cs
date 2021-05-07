using System.Security.Principal;

namespace TicketManagement.WebMVC.Services
{
    public interface IIdentityParser<T>
    {
        T Parse(IPrincipal principal);
    }
}
