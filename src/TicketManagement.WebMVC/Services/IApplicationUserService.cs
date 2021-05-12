using System.Threading.Tasks;
using TicketManagement.WebMVC.Models;

namespace TicketManagement.WebMVC.Services
{
    public interface IApplicationUserService
    {
        public Task<decimal> GetBalanceAsync(ApplicationUser applicationUser);

        public Task UpdateBalanceAsync(ApplicationUser applicationUser);
    }
}