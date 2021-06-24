using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.Dto;

namespace TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces
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
        Task CreateAsync(LayoutDto dto);

        /// <summary>
        /// Delete object in database.
        /// </summary>
        /// <param name="dto">Layout data object transfer.</param>
        Task DeleteAsync(LayoutDto dto);

        /// <summary>
        /// Update object in database.
        /// </summary>
        /// <param name="dto">Layout data object transfer.</param>
        Task UpdateAsync(LayoutDto dto);

        /// <summary>
        /// Get object by Id from table in database.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <returns>Returns object by id.</returns>
        Task<LayoutDto> GetByIdAsync(int id);

        /// <summary>
        /// Method to get all objects from database table.
        /// </summary>
        /// <returns>Returns list of objects.</returns>
        IEnumerable<LayoutDto> GetAll();
    }
}