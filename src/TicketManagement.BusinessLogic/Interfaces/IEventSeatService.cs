using System.Collections.Generic;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Interfaces
{
    /// <summary>
    /// Event seat service interface.
    /// </summary>
    internal interface IEventSeatService : IService
    {
        /// <summary>
        /// Update state object in database.
        /// </summary>
        /// <param name="dto">Event seat data object transfer.</param>
        void UpdateState(EventSeatDto dto);

        /// <summary>
        /// Get object by Id from table in database.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <returns>Returns object by id.</returns>
        EventSeatDto GetByID(int id);

        /// <summary>
        /// Method to get all objects from database table.
        /// </summary>
        /// <returns>Returns list of objects.</returns>
        IEnumerable<EventSeatDto> GetAll();
    }
}
