using System.Collections.Generic;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Interfaces
{
    /// <summary>
    /// Event service interface.
    /// </summary>
    internal interface IEventService : IService
    {
        /// <summary>
        /// Create object in database.
        /// </summary>
        /// <param name="dto">Event data object transfer.</param>
        void Create(EventDto dto);

        /// <summary>
        /// Create object in database.
        /// </summary>
        /// <param name="dto">Event data object transfer.</param>
        void Delete(EventDto dto);

        /// <summary>
        /// Create object in database.
        /// </summary>
        /// <param name="dto">Event data object transfer.</param>
        void Update(EventDto dto);

        /// <summary>
        /// Get object by Id from table in database.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <returns>Returns object by id.</returns>
        EventDto GetByID(int id);

        /// <summary>
        /// Method to get all objects from database table.
        /// </summary>
        /// <returns>Returns list of objects.</returns>
        IEnumerable<EventDto> GetAll();
    }
}