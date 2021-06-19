////using System.Linq;
////using System.Threading.Tasks;
////using TicketManagement.DataAccess.Domain.Models;
////using TicketManagement.Services.Basket.API.Models;
////using BasketModel = TicketManagement.DataAccess.Domain.Models.Basket;

////namespace TicketManagement.Services.Basket.API.Services
////{
////    public interface IPurchaseHistoryService
////    {
////        Task AddAsync(ApplicationUser user, int productId);

////        Task DeleteAsync(PurchaseHistory purchaseHistory);

////        Task<IQueryable<PurchaseHistory>> GetAllAsync();

////        Task AddFromBasketAsync(IQueryable<BasketModel> baskets);

////        Task<PurchaseHistoryViewModel> GetAllByUserAsync(ApplicationUser user);
////    }
////}
