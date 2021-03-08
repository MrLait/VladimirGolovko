namespace TicketManagement.BusinessLogic.Interfaces
{
    using TicketManagement.Dto;

    /// <summary>
    /// Event seat service interface.
    /// </summary>
    internal interface IEventSeatService : IService
    {
        /// <summary>
        /// Update state object in database.
        /// </summary>
        /// <param name="dto">Event seat data object transfer.</param>
        void UpdateState(EventSeatDto dto);
    }
}
