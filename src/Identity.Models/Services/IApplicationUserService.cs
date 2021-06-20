using System.Threading.Tasks;
using TicketManagement.Services.Identity.Domain.Models;

namespace TicketManagement.Services.Identity.Domain.Services
{
    public interface IApplicationUserService
    {
        public Task<decimal> GetBalanceAsync(ApplicationUser applicationUser);

        public Task UpdateBalanceAsync(ApplicationUser applicationUser);
    }
}