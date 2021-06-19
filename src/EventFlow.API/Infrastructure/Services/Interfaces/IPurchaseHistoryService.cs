using System.Threading.Tasks;

namespace TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces
{
    public interface IPurchaseHistoryService
    {
        ////Task AddAsync(ApplicationUser user, int productId);

        ////Task DeleteAsync(PurchaseHistory purchaseHistory);

        ////Task<IQueryable<PurchaseHistory>> GetAllAsync();

        ////Task AddFromBasketAsync(IQueryable<Basket> baskets);

        ////Task<PurchaseHistoryViewModel> GetAllByUserAsync(ApplicationUser user);
        Task AddAsync(string userId, int itemId);
    }
}
