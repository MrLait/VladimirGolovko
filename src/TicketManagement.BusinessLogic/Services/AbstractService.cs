using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.BusinessLogic.Services
{
    internal class AbstractService
    {
        protected AbstractService(IDbContext dbContext) => DbContext = dbContext;

        protected IDbContext DbContext { get; private set; }
    }
}
