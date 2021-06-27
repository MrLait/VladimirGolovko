using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.Dto;

namespace TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Event service interface.
    /// </summary>
    public interface IEventService : IService
    {
        /// <summary>
        /// Create object in database.
        /// </summary>
        /// <param name="dto">Event data object transfer.</param>
        Task CreateAsync(EventDto dto);

        /// <summary>
        /// Create object in database.
        /// </summary>
        /// <param name="dto">Event data object transfer.</param>
        Task DeleteAsync(EventDto dto);

        /// <summary>
        /// Create object in database.
        /// </summary>
        /// <param name="dto">Event data object transfer.</param>
        Task UpdateAsync(EventDto dto);

        /// <summary>
        /// Get object by Id from table in database.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <returns>Returns object by id.</returns>
        Task<EventDto> GetByIdAsync(int id);

        /// <summary>
        /// Get last event.
        /// </summary>
        /// <returns>Event data object transfer.</returns>
        EventDto Last();

        /// <summary>
        /// Method to get all objects from database table.
        /// </summary>
        /// <returns>Returns list of objects.</returns>
        IEnumerable<EventDto> GetAll();
    }
}