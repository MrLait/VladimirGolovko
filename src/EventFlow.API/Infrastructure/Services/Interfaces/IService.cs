using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Service interface with databese context.
    /// </summary>
    public interface IService
    {
        IDbContext DbContext { get; }
    }
}
