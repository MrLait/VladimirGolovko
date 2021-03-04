using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.BusinessLogic.Services
{
    internal class AbstractService<T> : IDtoService<T>
    {
        protected AbstractService(IDbContext dbContext) => DbContext = dbContext;

        protected IDbContext DbContext { get; private set; }

        /// <inheritdoc cref="IDtoService{T}"/>
        public virtual void Create(T dto)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="IDtoService{T}"/>
        public virtual void Delete(T dto)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="IDtoService{T}"/>
        public virtual void Update(T dto)
        {
            throw new System.NotImplementedException();
        }
    }
}
