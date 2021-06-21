using System.Security.Principal;

namespace TicketManagement.Services.Identity.Domain.Services
{
    internal interface IIdentityParser<T>
    {
        T Parse(IPrincipal principal);
    }
}
