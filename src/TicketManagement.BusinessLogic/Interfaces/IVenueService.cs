using System.Collections.Generic;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Interfaces
{
    /// <summary>
    /// Venue service interface.
    /// </summary>
    internal interface IVenueService : IService
    {
        /// <summary>
        /// Create object in database.
        /// </summary>
        /// <param name="dto">Venue data object transfer.</param>
        void Create(VenueDto dto);

        /// <summary>
        /// Delete object in database.
        /// </summary>
        /// <param name="dto">Venue data object transfer.</param>
        void Delete(VenueDto dto);

        /// <summary>
        /// Update object in database.
        /// </summary>
        /// <param name="dto">Venue data object transfer.</param>
        void Update(VenueDto dto);

        /// <summary>
        /// Get object by Id from table in database.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <returns>Returns object by id.</returns>
        VenueDto GetByID(int id);

        /// <summary>
        /// Method to get all objects from database table.
        /// </summary>
        /// <returns>Returns list of objects.</returns>
        IEnumerable<VenueDto> GetAll();
    }
}