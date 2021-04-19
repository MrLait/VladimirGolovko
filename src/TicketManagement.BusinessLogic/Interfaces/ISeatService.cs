using System.Collections.Generic;
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
        void Create(SeatDto dto);

        /// <summary>
        /// Delete object in database.
        /// </summary>
        /// <param name="dto">Seat data object transfer.</param>
        void Delete(SeatDto dto);

        /// <summary>
        /// Update object in database.
        /// </summary>
        /// <param name="dto">Seat data object transfer.</param>
        void Update(SeatDto dto);

        /// <summary>
        /// Get object by Id from table in database.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <returns>Returns object by id.</returns>
        SeatDto GetByID(int id);

        /// <summary>
        /// Method to get all objects from database table.
        /// </summary>
        /// <returns>Returns list of objects.</returns>
        IEnumerable<SeatDto> GetAll();
    }
}