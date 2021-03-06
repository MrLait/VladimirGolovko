using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.BusinessLogic.Interfaces
{
    public interface IService
    {
        IDbContext DbContext { get; }
    }
}
