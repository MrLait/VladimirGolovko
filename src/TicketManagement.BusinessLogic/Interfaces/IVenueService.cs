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
    }
}