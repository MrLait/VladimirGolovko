using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.ViewModels.PurchaseHistoryViewModels;

namespace TicketManagement.WebMVC.Services
{
    public interface IPurchaseHistoryService
    {
        Task AddAsync(ApplicationUser user, int productId);

        Task DeleteAsync(PurchaseHistory purchaseHistory);

        Task<IQueryable<PurchaseHistory>> GetAllAsync();

        Task AddFromBasketAsync(IQueryable<Basket> baskets);

        Task<PurchaseHistoryViewModel> GetAllByUserAsync(ApplicationUser user);
    }
}
