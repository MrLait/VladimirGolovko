using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Domain.Interfaces;
using TicketManagement.DataAccess.Exstension;

namespace TicketManagement.DataAccess.Repositories.AdoRepositories
{
    /// <summary>
    /// Ado using stored procedure repository class.
    /// </summary>
    /// <typeparam name="T">Table model.</typeparam>
    internal class AdoUsingStoredProcedureRepository<T> : AdoRepository<T>
        where T : IEntity, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdoUsingStoredProcedureRepository{T}"/> class.
        /// </summary>
        /// <param name="connectionString">Connection string to database.</param>
        internal AdoUsingStoredProcedureRepository(string connectionString)
            : base(connectionString)
        {
        }

        /// <inheritdoc/>
        public override async Task DeleteAsync(T entity)
        {
            await base.DeleteAsync(entity);

            string tableName = new T().GetType().Name;
            string storedProcedure = $"Delete{tableName}";

            using SqlConnection sqlConnection = new SqlConnection(DbConString);
            using SqlCommand sqlCommand = SqlCommandInstance(storedProcedure, sqlConnection, new[] { new SqlParameter("Id", entity.Id) });
            try
            {
                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occurred at database, if error in sql expression: {storedProcedure}, {sqlEx}");
            }
        }

        /// <inheritdoc/>
        public override async Task<IEnumerable<T>> GetAllAsync()
        {
            string tableName = new T().GetType().Name;
            string storedProcedure = $"GetAll{tableName}";

            using SqlConnection sqlConnection = new SqlConnection(DbConString);
            using SqlCommand sqlCommand = SqlCommandInstance(storedProcedure, sqlConnection);
            SqlDataAdapter adpt = new SqlDataAdapter(sqlCommand);

            try
            {
                DataSet ds = new DataSet();
                await Task.Run(() => adpt.Fill(ds));
                return ds.Tables[0].ToEnumerable<T>();
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occurred at database, if error in sql expression: {storedProcedure}, {sqlEx}");
            }
        }

        /// <inheritdoc/>
        public override async Task<T> GetByIDAsync(int byId)
        {
            await base.GetByIDAsync(byId);

            string tableName = new T().GetType().Name;
            string storedProcedure = $"GetById{tableName}";

            using SqlConnection sqlConnection = new SqlConnection(DbConString);
            using SqlCommand sqlCommand = SqlCommandInstance(storedProcedure, sqlConnection, new[] { new SqlParameter("Id", byId) });
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            try
            {
                DataSet ds = new DataSet();
                await Task.Run(() => sqlDataAdapter.Fill(ds));
                return ds.Tables[0].ToEnumerable<T>().SingleOrDefault();
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occurred at database, if error in sql expression: {storedProcedure}, {sqlEx}");
            }
        }

        /// <inheritdoc/>
        public override async Task CreateAsync(T entity)
        {
            await base.CreateAsync(entity);

            var storedProcedure = $"Create{entity.GetType().Name}";

            using SqlConnection sqlConnection = new SqlConnection(DbConString);
            using SqlCommand sqlCommand = SqlCommandInstance(storedProcedure, sqlConnection);
            try
            {
                sqlCommand.Parameters.AddRange(GetAddParameter(entity).ToArray());
                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteScalarAsync();
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occurred at database, if error in sql expression: {storedProcedure}, {sqlEx}");
            }
        }

        /// <inheritdoc/>
        public override async Task UpdateAsync(T entity)
        {
            await base.UpdateAsync(entity);

            string tableName = new T().GetType().Name;
            string storedProcedure = $"Update{tableName}";

            using SqlConnection sqlConnection = new SqlConnection(DbConString);
            using SqlCommand sqlCommand = SqlCommandInstance(storedProcedure, sqlConnection);
            sqlCommand.Parameters.AddRange(GetUpdateParameter(entity).ToArray());

            SqlDataAdapter adpt = new SqlDataAdapter(sqlCommand);
            DataSet ds = new DataSet();

            try
            {
                await Task.Run(() => adpt.Fill(ds));
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occurred at database, if error in sql expression: {storedProcedure}, {sqlEx}");
            }
        }

        /// <summary>
        /// Creating sqlCommand.
        /// </summary>
        /// <param name="storedProcedure">Name of stored procedure.</param>
        /// <param name="sqlConnection"> Sql connection sting.</param>
        /// <returns>return sql command.</returns>
        private SqlCommand SqlCommandInstance(string storedProcedure, SqlConnection sqlConnection)
        {
            SqlCommand sqlCommand = new SqlCommand(storedProcedure, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            return sqlCommand;
        }

        /// <summary>
        /// Creating sqlCommand.
        /// </summary>
        /// <param name="storedProcedure">Name of stored procedure.</param>
        /// <param name="sqlConnection"> Sql connection sting.</param>
        /// <param name="sqlParamArr"> SqlParameters.</param>
        /// <returns>return sql command.</returns>
        private SqlCommand SqlCommandInstance(string storedProcedure, SqlConnection sqlConnection, SqlParameter[] sqlParamArr)
        {
            SqlCommand sqlCommand = SqlCommandInstance(storedProcedure, sqlConnection);
            sqlCommand.Parameters.AddRange(sqlParamArr);
            return sqlCommand;
        }

        /// <summary>
        /// Private method for get property from objects and add their to list for sqlParameters.
        /// </summary>
        /// <param name="obj">Object to get properties.</param>
        /// <returns>returns list of sqlParameters.</returns>
        private List<SqlParameter> GetUpdateParameter(object obj)
        {
            PropertyInfo[] fields = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var sqlParams = new List<SqlParameter>();
            foreach (var f in fields)
            {
                sqlParams.Add(new SqlParameter(f.Name, f.GetValue(obj, null)));
            }

            return sqlParams;
        }
    }
}
