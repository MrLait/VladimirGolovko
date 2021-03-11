using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.BusinessLogic.Interfaces
{
    /// <summary>
    /// Service interface with databese context.
    /// </summary>
    internal interface IService
    {
        IDbContext DbContext { get; }
    }
}
