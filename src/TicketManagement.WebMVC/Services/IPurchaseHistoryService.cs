using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.WebMVC.Models;

namespace TicketManagement.WebMVC.Services
{
    public interface IPurchaseHistoryService
    {
        Task AddAsync(ApplicationUser user, int productId);

        Task DeleteAsync(PurchaseHistory purchaseHistory);

        Task<IQueryable<PurchaseHistory>> GetAllAsync();

        ////Task<BasketViewModel> GetAllByUserAsync(ApplicationUser user);
    }
}
