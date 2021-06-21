using System.Threading.Tasks;
using TicketManagement.Services.EventFlow.API.Models;

namespace TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces
{
    public interface IPurchaseHistoryService
    {
        /// <summary>
        /// Get all by user id.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>Returns purchase history model.</returns>
        Task<PurchaseHistoryModel> GetAllByUserIdAsync(string userId);

        /// <summary>
        /// Add to purchase history.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="itemId">Item id.</param>
        Task AddAsync(string userId, int itemId);
    }
}
