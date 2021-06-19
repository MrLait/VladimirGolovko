using System.Threading.Tasks;
using TicketManagement.Services.EventFlow.API.Models;

namespace TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces
{
    public interface IBasketService
    {
        Task AddAsync(string userId, int productId);

        ////Task DeleteAsync(Basket basketItem);

        ////Task DeleteAsync(ApplicationUser user);

        ////IQueryable<Basket> GetAll();

        ////Task<BasketViewModel> GetAllByUserAsync(ApplicationUser user);
        Task<BasketModel> GetAllByUserIdAsync(string id);
    }
}
