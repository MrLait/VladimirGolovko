using System.Security.Principal;

namespace TicketManagement.Services.Identity.Domain.Services
{
    public interface IIdentityParser<T>
    {
        T Parse(IPrincipal principal);
    }
}
