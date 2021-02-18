using System;
using System.Collections.Generic;
using TicketManagement.DataAccess.Domain.Interfaces;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.DataAccess.Repositories.ADO
{
    internal class AdoUsingStoredProcedureRepository<T> : AdoRepository<T>, IUsingStoredProcedureRepository<T>
        where T : IEntity, new()
    {
        protected AdoUsingStoredProcedureRepository(string dbConString)
            : base(dbConString)
        {
        }

        /// <inheritdoc/>
        public override void Delete(int byId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override T GetByID(int byId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Insert(T entity)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
