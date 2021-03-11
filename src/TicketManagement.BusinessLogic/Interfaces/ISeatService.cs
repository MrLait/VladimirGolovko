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
    }
}