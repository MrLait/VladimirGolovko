using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Interfaces
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
        void Create(LayoutDto dto);

        /// <summary>
        /// Delete object in database.
        /// </summary>
        /// <param name="dto">Layout data object transfer.</param>
        void Delete(LayoutDto dto);

        /// <summary>
        /// Update object in database.
        /// </summary>
        /// <param name="dto">Layout data object transfer.</param>
        void Update(LayoutDto dto);
    }
}