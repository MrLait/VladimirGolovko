namespace TicketManagement.BusinessLogic.Interfaces
{
    using TicketManagement.Dto;

    /// <summary>
    /// Event service interface.
    /// </summary>
    internal interface IEventService : IService
    {
        /// <summary>
        /// Create object in database.
        /// </summary>
        /// <param name="dto">Event data object transfer.</param>
        void Create(EventDto dto);

        /// <summary>
        /// Create object in database.
        /// </summary>
        /// <param name="dto">Event data object transfer.</param>
        void Delete(EventDto dto);

        /// <summary>
        /// Create object in database.
        /// </summary>
        /// <param name="dto">Event data object transfer.</param>
        void Update(EventDto dto);
    }
}