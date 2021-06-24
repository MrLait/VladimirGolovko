using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.DbContexts;
using TicketManagement.DataAccess.Domain.Interfaces;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.DataAccess.Repositories.EfRepositories
{
    internal class EfRepositoryUsingStoredProcedure<T> : IRepository<T>
        where T : class, IEntity, new()
    {
        public EfRepositoryUsingStoredProcedure(EfDbContext context)
        {
            Context = context;
        }

        protected EfDbContext Context { get; }

        public async Task CreateAsync(T entity)
        {
            var tableName = new T().GetType().Name;
            var param = GetUpdateParameter(entity, true);
            var parameters = param.ToArray();
            var strParam = string.Join(", ", parameters.Select(x => "@" + x.ParameterName));
            var storedProcedure = $"Create{tableName}";
            var sqlQuery = $"EXECUTE {storedProcedure} {strParam}";

            foreach (var item in param)
            {
                item.ParameterName = $"@{item.ParameterName}";
            }

            await Context.Database.ExecuteSqlRawAsync(sqlQuery, param);
        }

        public async Task DeleteAsync(T entity)
        {
            var tableName = new T().GetType().Name;
            var param = new[] { new SqlParameter("@Id", entity.Id) };
            var storedProcedure = $"Delete{tableName}";
            var sqlQuery = $"EXECUTE {storedProcedure} @Id";
            await Context.Database.ExecuteSqlRawAsync(sqlQuery, param);
        }

        public IQueryable<T> GetAllAsQueryable()
        {
            var tableName = new T().GetType().Name;
            var storedProcedure = $"GetAll{tableName}";
            return Context.Set<T>().FromSqlRaw($"EXECUTE {storedProcedure}");
        }

        public async Task<T> GetByIdAsync(int byId)
        {
            var tableName = new T().GetType().Name;
            var param = new[] { new SqlParameter("@Id", byId) };
            var storedProcedure = $"GetByID{tableName}";
            var sqlQuery = $"EXECUTE {storedProcedure} @Id";

            return (await Context.Set<T>().FromSqlRaw(sqlQuery, param).ToListAsync()).FirstOrDefault();
        }

        public async Task UpdateAsync(T entity)
        {
            var tableName = new T().GetType().Name;
            var param = GetUpdateParameter(entity, false);
            var parameters = param.ToArray();
            var strParam = string.Join(", ", parameters.Select(x => "@" + x.ParameterName));
            var storedProcedure = $"Update{tableName}";
            var sqlQuery = $"EXECUTE {storedProcedure} {strParam}";

            await Context.Database.ExecuteSqlRawAsync(sqlQuery, param);
        }

        /// <summary>
        /// Private method for get property from objects and add their to list for sqlParameters.
        /// </summary>
        /// <param name="obj">Object to get properties.</param>
        /// <param name="removeId">Remove id.</param>
        /// <returns>returns list of sqlParameters.</returns>
        private List<SqlParameter> GetUpdateParameter(object obj, bool removeId)
        {
            var fields = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var sqlParams = new List<SqlParameter>();
            foreach (var f in fields)
            {
                if (removeId)
                {
                    if (f.Name != "Id")
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
