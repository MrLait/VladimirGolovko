using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Domain.Interfaces;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.DataAccess.Repositories.AdoRepositories
{
    /// <summary>
    /// Ado repository class.
    /// </summary>
    /// <typeparam name="T">Table model.</typeparam>
    internal abstract class AdoRepository<T> : IRepository<T>
        where T : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdoRepository{T}"/> class.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        protected AdoRepository(string connectionString) => DbConString = connectionString;

        /// <summary>
        /// Gets connection string to database.
        /// </summary>
        protected string DbConString { get; }

        /// <inheritdoc/>
        public virtual Task DeleteAsync(T entity)
        {
            if (Equals(entity, default(T)))
            {
                throw new ArgumentException($"Can not delete null object: {default(T)}!");
            }

            if (entity.Id <= 0)
            {
                throw new ArgumentException($"There is not this item in the Item Storage with Id: {entity.Id}!");
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public abstract Task<IQueryable<T>> GetAllAsync();

        /// <inheritdoc/>
        public virtual Task<T> GetByIDAsync(int byId)
        {
            if (byId <= 0)
            {
                throw new ArgumentException($"byId shouldn't be equal {byId}");
            }

            return Task.FromResult(default(T));
        }

        /// <inheritdoc/>
        public virtual Task CreateAsync(T entity)
        {
            if (Equals(entity, default(T)))
            {
                throw new ArgumentException($"Can not add null object: {typeof(T).Name} !");
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public virtual Task UpdateAsync(T entity)
        {
            if (Equals(entity, default(T)))
            {
                throw new ArgumentException($"{typeof(T).Name} object shouldn't be null when saving to database");
            }

            if (entity.Id <= 0)
            {
                throw new ArgumentException($"There is not this item in the Item Storage with Id: {entity.Id}!");
            }

            return Task.CompletedTask;
        }

        protected List<SqlParameter> GetAddParameter(object obj)
        {
            PropertyInfo[] fields = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var sqlParams = new List<SqlParameter>();

            foreach (var f in fields)
            {
                if (f.GetCustomAttributes(false).Length != 0)
                {
                    if (f.GetCustomAttributesData()[0].AttributeType.Name != "KeyAttribute")
                    {
                        sqlParams.Add(new SqlParameter(f.Name, f.GetValue(obj, null)));
                    }
                }
                else
                {
                    sqlParams.Add(new SqlParameter(f.Name, f.GetValue(obj, null)));
                }
            }

            return sqlParams;
        }
    }
}
