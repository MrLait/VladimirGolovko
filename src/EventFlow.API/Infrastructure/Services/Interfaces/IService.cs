using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Service interface with database context.
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Get db context.
        /// </summary>
        IDbContext DbContext { get; }
    }
}
