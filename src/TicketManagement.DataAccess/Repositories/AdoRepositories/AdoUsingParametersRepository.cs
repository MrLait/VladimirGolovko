using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Domain.Interfaces;
using TicketManagement.DataAccess.Exstension;

namespace TicketManagement.DataAccess.Repositories.AdoRepositories
{
    /// <summary>
    /// Ado using parameters repository class.
    /// </summary>
    /// <typeparam name="T">Table model.</typeparam>
    internal class AdoUsingParametersRepository<T> : AdoRepository<T>
        where T : IEntity, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdoUsingParametersRepository{T}"/> class.
        /// </summary>
        /// <param name="connectionString">Connection string to database.</param>
        internal AdoUsingParametersRepository(string connectionString)
            : base(connectionString)
        {
        }

        /// <inheritdoc/>
        public override async Task CreateAsync(T entity)
        {
            await base.CreateAsync(entity);

            SqlParameter[] parameters = GetAddParameter(entity).ToArray();

            string tableName = entity.GetType().Name;
            string strCol = string.Join(", ", parameters.Select(x => x.ParameterName).ToList());
            var strParam = string.Join(", ", parameters.Select(x => "@" + x.ParameterName).ToList());
            var sqlExpressionInsert = $"INSERT INTO [{tableName}] ({strCol}) VALUES ({strParam})";

            using SqlConnection sqlConnection = new SqlConnection(DbConString);
            using SqlCommand sqlCommand = new SqlCommand(sqlExpressionInsert, sqlConnection);

            try
            {
                foreach (var item in parameters)
                {
                    sqlCommand.Parameters.AddWithValue("@" + item.ParameterName, item.Value);
                }

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occurred at database, if error in sql expression: {sqlExpressionInsert}, {sqlEx}");
            }
        }

        /// <inheritdoc/>
        public override async Task DeleteAsync(T entity)
        {
            await base.DeleteAsync(entity);

            string tableName = new T().GetType().Name;
            string sqlExpressionInsert = $"DELETE FROM {tableName} WHERE Id = {entity.Id}";

            using SqlConnection sqlConnection = new SqlConnection(DbConString);
            using SqlCommand sqlCommand = new SqlCommand(sqlExpressionInsert, sqlConnection);

            try
            {
                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occurred at database, if error in sql expression: {sqlExpressionInsert}, {sqlEx}");
            }
        }

        /// <inheritdoc/>
        public override async Task<IEnumerable<T>> GetAllAsync()
        {
            var poropertyNames = typeof(T).GetProperties().Select(x => x.Name).ToList();
            string strCol = string.Join(", ", poropertyNames);
            string tableName = new T().GetType().Name;
            string sqlExpressionSelect = $"SELECT {strCol} FROM {tableName}";

            using SqlConnection sqlConnection = new SqlConnection(DbConString);
            using SqlCommand sqlCommand = new SqlCommand(sqlExpressionSelect, sqlConnection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            try
            {
                DataSet ds = new DataSet();
                await Task.Run(() => sqlDataAdapter.Fill(ds));
                return ds.Tables[0].ToEnumerable<T>();
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occurred at database, if error in sql expression: {sqlExpressionSelect}, {sqlEx}");
            }
        }

        /// <inheritdoc/>
        public override async Task<T> GetByIDAsync(int byId)
        {
            await base.GetByIDAsync(byId);

            var poropertyNames = typeof(T).GetProperties().Select(x => x.Name).ToList();
            string strCol = string.Join(", ", poropertyNames);
            string tableName = new T().GetType().Name;
            string sqlExpressionSelect = $"SELECT {strCol} FROM {tableName} WHERE Id = {byId}";

            using SqlConnection sqlConnection = new SqlConnection(DbConString);
            using SqlCommand sqlCommand = new SqlCommand(sqlExpressionSelect, sqlConnection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            try
            {
                DataSet ds = new DataSet();
                await Task.Run(() => sqlDataAdapter.Fill(ds));
                return ds.Tables[0].ToEnumerable<T>().SingleOrDefault();
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occurred at database, if error in sql expression: {sqlExpressionSelect}, {sqlEx}");
            }
        }

        /// <inheritdoc/>
        public override async Task UpdateAsync(T entity)
        {
            await base.UpdateAsync(entity);

            SqlParameter[] parameters = GetAddParameter(entity).ToArray();

            string tableName = entity.GetType().Name;
            string setStr = string.Join(", ", parameters.Select(x => $"{x.ParameterName} = @{x.ParameterName}").ToList());

            var sqlExpressionInsert = $"UPDATE [{tableName}] SET {setStr} WHERE Id = {entity.Id}";

            using SqlConnection sqlConnection = new SqlConnection(DbConString);
            using SqlCommand sqlCommand = new SqlCommand(sqlExpressionInsert, sqlConnection);
            try
            {
                foreach (var item in parameters)
                {
                    sqlCommand.Parameters.AddWithValue("@" + item.ParameterName, item.Value);
                }

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occurred at database, if error in sql expression: {sqlExpressionInsert}, {sqlEx}");
            }
        }
    }
}
