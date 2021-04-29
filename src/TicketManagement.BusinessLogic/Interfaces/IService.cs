using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.BusinessLogic.Interfaces
{
    /// <summary>
    /// Service interface with databese context.
    /// </summary>
    public interface IService
    {
        IDbContext DbContext { get; }
    }
}
