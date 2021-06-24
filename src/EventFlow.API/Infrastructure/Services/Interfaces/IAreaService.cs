using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.Dto;

namespace TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces
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
        Task CreateAsync(AreaDto dto);

        /// <summary>
        /// Delete object in database.
        /// </summary>
        /// <param name="dto">Area data object transfer.</param>
        Task DeleteAsync(AreaDto dto);

        /// <summary>
        /// Update object in database.
        /// </summary>
        /// <param name="dto">Area data object transfer.</param>
        Task UpdateAsync(AreaDto dto);

        /// <summary>
        /// Get object by Id from table in database.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <returns>Returns object by id.</returns>
        Task<AreaDto> GetByIdAsync(int id);

        /// <summary>
        /// Method to get all objects from database table.
        /// </summary>
        /// <returns>Returns list of objects.</returns>
        IEnumerable<AreaDto> GetAll();
    }
}