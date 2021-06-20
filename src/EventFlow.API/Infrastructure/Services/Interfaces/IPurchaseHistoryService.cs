using System.Threading.Tasks;
using TicketManagement.Services.EventFlow.API.Models;

namespace TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces
{
    public interface IPurchaseHistoryService
    {
        ////Task AddAsync(ApplicationUser user, int productId);

        ////Task DeleteAsync(PurchaseHistory purchaseHistory);

        ////Task<IQueryable<PurchaseHistory>> GetAllAsync();

        ////Task AddFromBasketAsync(IQueryable<Basket> baskets);

        Task<PurchaseHistoryModel> GetAllByUserIdAsync(string userId);

        Task AddAsync(string userId, int itemId);
    }
}
