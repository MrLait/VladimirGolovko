using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Interfaces
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
        void Create(AreaDto dto);

        /// <summary>
        /// Delete object in database.
        /// </summary>
        /// <param name="dto">Area data object transfer.</param>
        void Delete(AreaDto dto);

        /// <summary>
        /// Update object in database.
        /// </summary>
        /// <param name="dto">Area data object transfer.</param>
        void Update(AreaDto dto);
    }
}