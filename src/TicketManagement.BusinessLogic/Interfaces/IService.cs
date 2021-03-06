using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.BusinessLogic.Interfaces
{
    internal interface IService
    {
        IDbContext DbContext { get; }
    }
}
