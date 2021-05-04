using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.ViewModels.BasketViewModels;

namespace TicketManagement.WebMVC.Services
{
    public interface IBasketService
    {
        Task AddAsync(ApplicationUser user, int productId);

        Task DeleteAsync(Basket basketItem);

        Task<IQueryable<Basket>> GetAllAsync();

        Task<BasketViewModel> GetAllByUserAsync(ApplicationUser user);
    }
}
