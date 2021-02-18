using System.Collections.Generic;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.DataAccess.Repositories.ADO
{
    internal abstract class AdoRepository<T> : IRepository<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdoRepository{T}"/> class.
        /// </summary>
        /// <param name="dbConString">Connection string.</param>
        protected AdoRepository(string dbConString)
        {
            DbConString = dbConString;
        }

        /// <summary>
        /// Connection string to database.
        /// </summary>
        protected string DbConString { get; private set; }

        /// <inheritdoc/>
        public abstract void Delete(int byId);

        /// <inheritdoc/>
        public abstract IEnumerable<T> GetAll();

        /// <inheritdoc/>
        public abstract T GetByID(int byId);

        /// <inheritdoc/>
        public abstract void Create(T entity);

        /// <inheritdoc/>
        public abstract void Update(T entity);
    }
}
