using System.Collections.Generic;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Interfaces
{
    /// <summary>
    /// Area service interface.
    /// </summary>
    internal interface IAreaService : IService
    {
        /// <summary>
        /// Create object in database.
        /// </summary>
        /// <param name="dto">Area data object transfer.</param>
        void Create(AreaDto dto);

        /// <summary>
        /// Delete object in database.
        /// </summary>
        /// <param name="dto">Area data object transfer.</param>
        void Delete(AreaDto dto);

        /// <summary>
        /// Update object in database.
        /// </summary>
        /// <param name="dto">Area data object transfer.</param>
        void Update(AreaDto dto);

        /// <summary>
        /// Get object by Id from table in database.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <returns>Returns object by id.</returns>
        AreaDto GetByID(int id);

        /// <summary>
        /// Method to get all objects from database table.
        /// </summary>
        /// <returns>Returns list of objects.</returns>
        IEnumerable<AreaDto> GetAll();
    }
}