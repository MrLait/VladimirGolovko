using System.Security.Principal;

namespace TicketManagement.WebMVC.Services
{
    /// <summary>
    /// Identity parser.
    /// </summary>
    /// <typeparam name="T">Identity user.</typeparam>
    public interface IIdentityParser<T>
    {
        /// <summary>
        /// Parse.
        /// </summary>
        /// <param name="principal">Principals.</param>
        /// <returns>Identity user.</returns>
        T Parse(IPrincipal principal);
    }
}
