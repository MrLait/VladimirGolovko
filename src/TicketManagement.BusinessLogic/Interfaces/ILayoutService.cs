using System.Collections.Generic;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Interfaces
{
    /// <summary>
    /// Layout service interface.
    /// </summary>
    internal interface ILayoutService : IService
    {
        /// <summary>
        /// Create object in database.
        /// </summary>
        /// <param name="dto">Layout data object transfer.</param>
        void Create(LayoutDto dto);

        /// <summary>
        /// Delete object in database.
        /// </summary>
        /// <param name="dto">Layout data object transfer.</param>
        void Delete(LayoutDto dto);

        /// <summary>
        /// Update object in database.
        /// </summary>
        /// <param name="dto">Layout data object transfer.</param>
        void Update(LayoutDto dto);

        /// <summary>
        /// Get object by Id from table in database.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <returns>Returns object by id.</returns>
        LayoutDto GetByID(int id);

        /// <summary>
        /// Method to get all objects from database table.
        /// </summary>
        /// <returns>Returns list of objects.</returns>
        IEnumerable<LayoutDto> GetAll();
    }
}