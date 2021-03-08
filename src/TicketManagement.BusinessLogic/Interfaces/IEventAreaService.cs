namespace TicketManagement.BusinessLogic.Interfaces
{
    using TicketManagement.Dto;

    /// <summary>
    /// Event area service interface.
    /// </summary>
    internal interface IEventAreaService : IService
    {
        /// <summary>
        /// Update price object in database.
        /// </summary>
        /// <param name="dto">Event area data object transfer.</param>
        void UpdatePrice(EventAreaDto dto);
    }
}
