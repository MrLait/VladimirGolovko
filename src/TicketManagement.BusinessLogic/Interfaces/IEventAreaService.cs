using System.Collections.Generic;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Interfaces
{
    /// <summary>
    /// Event area service interface.
    /// </summary>
    internal interface IEventAreaService : IService
    {
        /// <summary>
        /// Update price object in database.
        /// </summary>
        /// <param name="dto">Event area data object transfer.</param>
        void UpdatePrice(EventAreaDto dto);

        /// <summary>
        /// Get object by Id from table in database.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <returns>Returns object by id.</returns>
        EventAreaDto GetByID(int id);

        /// <summary>
        /// Method to get all objects from database table.
        /// </summary>
        /// <returns>Returns list of objects.</returns>
        IEnumerable<EventAreaDto> GetAll();
    }
}
