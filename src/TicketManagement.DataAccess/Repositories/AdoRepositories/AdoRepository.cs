using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.DataAccess.Repositories.AdoRepositories
{
    internal abstract class AdoRepository<T> : IRepository<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdoRepository{T}"/> class.
        /// </summary>
        /// <param name="сonnectionString">Connection string.</param>
        protected AdoRepository(string сonnectionString) => DbConString = сonnectionString;

        /// <summary>
        /// Connection string to database.
        /// </summary>
        protected string DbConString { get; private set; }

        /// <inheritdoc/>
        public abstract void Delete(T entity);

        /// <inheritdoc/>
        public abstract IEnumerable<T> GetAll();

        /// <inheritdoc/>
        public abstract T GetByID(int byId);

        /// <inheritdoc/>
        public abstract void Create(T entity);

        /// <inheritdoc/>
        public abstract void Update(T entity);

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
