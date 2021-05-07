using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Interfaces
{
    /// <summary>
    /// Event seat service interface.
    /// </summary>
    public interface IEventSeatService : IService
    {
        /// <summary>
        /// Update state object in database.
        /// </summary>
        /// <param name="dto">Event seat data object transfer.</param>
        Task UpdateStateAsync(EventSeatDto dto);

        /// <summary>
        /// Get object by Id from table in database.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <returns>Returns object by id.</returns>
        Task<EventSeatDto> GetByIDAsync(int id);

        IEnumerable<EventSeatDto> GetByEventAreaId(EventAreaDto dto);

        /// <summary>
        /// Method to get all objects from database table.
        /// </summary>
        /// <returns>Returns list of objects.</returns>
        IEnumerable<EventSeatDto> GetAll();
    }
}
