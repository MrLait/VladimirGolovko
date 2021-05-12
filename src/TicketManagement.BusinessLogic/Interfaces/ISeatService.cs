using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Interfaces
{
    /// <summary>
    /// Seat service interface.
    /// </summary>
    internal interface ISeatService : IService
    {
        /// <summary>
        /// Create object in database.
        /// </summary>
        /// <param name="dto">Seat data object transfer.</param>
        Task CreateAsync(SeatDto dto);

        /// <summary>
        /// Delete object in database.
        /// </summary>
        /// <param name="dto">Seat data object transfer.</param>
        Task DeleteAsync(SeatDto dto);

        /// <summary>
        /// Update object in database.
        /// </summary>
        /// <param name="dto">Seat data object transfer.</param>
        Task UpdateAsync(SeatDto dto);

        /// <summary>
        /// Get object by Id from table in database.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <returns>Returns object by id.</returns>
        Task<SeatDto> GetByIDAsync(int id);

        /// <summary>
        /// Method to get all objects from database table.
        /// </summary>
        /// <returns>Returns list of objects.</returns>
        IEnumerable<SeatDto> GetAll();
    }
}