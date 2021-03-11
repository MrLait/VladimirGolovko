using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
        public override void Create(T entity)
        {
            if (Equals(entity, default(T)))
            {
                throw new ArgumentException($"Can not add null object: {typeof(T).Name} !");
            }

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

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occurred at database, if error in sql expression: {sqlExpressionInsert}, {sqlEx}");
            }
        }

        /// <inheritdoc/>
        public override void Delete(T entity)
        {
            if (Equals(entity, default(T)))
            {
                throw new ArgumentException($"Can not delete null object: {default(T)}!");
            }

            var entityId = entity.Id;

            if (entityId <= 0)
            {
                throw new ArgumentException($"There is not this item in the Item Storage with Id: {entityId}!");
            }

            string tableName = new T().GetType().Name;
            string sqlExpressionInsert = $"DELETE FROM {tableName} WHERE Id = {entityId}";

            using SqlConnection sqlConnection = new SqlConnection(DbConString);
            using SqlCommand sqlCommand = new SqlCommand(sqlExpressionInsert, sqlConnection);

            try
            {
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occurred at database, if error in sql expression: {sqlExpressionInsert}, {sqlEx}");
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<T> GetAll()
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
                sqlDataAdapter.Fill(ds);
                return ds.Tables[0].ToEnumerable<T>();
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occurred at database, if error in sql expression: {sqlExpressionSelect}, {sqlEx}");
            }
        }

        /// <inheritdoc/>
        public override T GetByID(int byId)
        {
            if (byId <= 0)
            {
                throw new ArgumentException($"byId shouldn't be equal {byId}");
            }

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
                sqlDataAdapter.Fill(ds);
                return ds.Tables[0].ToEnumerable<T>().SingleOrDefault();
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occurred at database, if error in sql expression: {sqlExpressionSelect}, {sqlEx}");
            }
        }

        /// <inheritdoc/>
        public override void Update(T entity)
        {
            if (Equals(entity, default(T)))
            {
                throw new ArgumentException($"{typeof(T).Name} object shouldn't be null when saving to database");
            }

            var entityId = entity.Id;

            if (entityId <= 0)
            {
                throw new ArgumentException($"There is not this item in the Item Storage with Id: {entityId}!");
            }

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

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occurred at database, if error in sql expression: {sqlExpressionInsert}, {sqlEx}");
            }
        }
    }
}
