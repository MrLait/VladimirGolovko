using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.WebMVC.Models;

namespace TicketManagement.WebMVC.Services
{
    public class PurchaseHistoryService : IPurchaseHistoryService
    {
        public Task AddAsync(ApplicationUser user, int productId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(PurchaseHistory purchaseHistory)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<PurchaseHistory>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
