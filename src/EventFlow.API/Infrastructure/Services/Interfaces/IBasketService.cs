using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.Services.EventFlow.API.Models;

namespace TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces
{
    public interface IBasketService
    {
        /// <summary>
        /// Add item to basket.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="productId">Product id.</param>
        Task AddAsync(string userId, int productId);

        /// <summary>
        /// Delete item to basket.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="productId">Product id.</param>
        Task DeleteAsync(string userId, int productId);

        /// <summary>
        /// Get all items from basket.
        /// </summary>
        /// <returns>Returns basket items.</returns>
        IEnumerable<Basket> GetAll();

        /// <summary>
        /// Get all items from basket for a user.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>Returns basket items.</returns>
        Task<BasketModel> GetAllByUserIdAsync(string id);

        /// <summary>
        /// Delete all items from basket by user id.
        /// </summary>
        /// <param name="id">User id.</param>
        Task DeleteAllByUserIdAsync(string id);
    }
}
