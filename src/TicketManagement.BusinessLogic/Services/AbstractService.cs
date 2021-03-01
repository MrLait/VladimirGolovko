using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.BusinessLogic.Services
{
    internal abstract class AbstractService<T> : IDtoService<T>
    {
        protected AbstractService(IDbContext dbContext) => DbContext = dbContext;

        protected IDbContext DbContext { get; private set; }

        public abstract void Create(T dto);
    }
}
