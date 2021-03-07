using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TicketManagement.DataAccess.Domain.Interfaces;
using TicketManagement.DataAccess.Exstension;

namespace TicketManagement.DataAccess.Repositories.AdoRepositories
{
    public class AdoUsingParametersRepository<T> : AdoRepository<T>
        where T : IEntity, new()
    {
        public AdoUsingParametersRepository(string сonnectionString)
            : base(сonnectionString)
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
                for (int i = 0; i < parameters.Length; i++)
                {
                    sqlCommand.Parameters.AddWithValue("@" + parameters[i].ParameterName, parameters[i].Value);
                }

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occured at database, if error in sql expression: {sqlExpressionInsert}, {sqlEx}");
            }
        }

        /// <inheritdoc/>
        public override void Delete(T entity)
        {
            if (Equals(entity, default(T)))
            {
                throw new ArgumentException($"Can not delete null object: {entity}!");
            }

            var entityId = entity.Id;

            if (entityId == 0)
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
                throw new ArgumentException($"Some Error occured at database, if error in sql expression: {sqlExpressionInsert}, {sqlEx}");
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<T> GetAll()
        {
            var poroperyNames = typeof(T).GetProperties().Select(x => x.Name).ToList();
            string strCol = string.Join(", ", poroperyNames);
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
                throw new ArgumentException($"Some Error occured at database, if error in sql expression: {sqlExpressionSelect}, {sqlEx}");
            }
        }

        /// <inheritdoc/>
        public override T GetByID(int byId)
        {
            if (byId == 0)
            {
                throw new ArgumentException($"byId shouldn't be equal {byId}");
            }

            var poroperyNames = typeof(T).GetProperties().Select(x => x.Name).ToList();
            string strCol = string.Join(", ", poroperyNames);
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
                throw new ArgumentException($"Some Error occured at database, if error in sql expression: {sqlExpressionSelect}, {sqlEx}");
            }
        }

        /// <inheritdoc/>
        public override void Update(T entity)
        {
            if (Equals(entity, default(T)))
            {
                throw new ArgumentException($"{typeof(T).Name} object shouln't be null when saving to database");
            }

            var entityId = entity.Id;

            if (entityId == 0)
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
                for (int i = 0; i < parameters.Length; i++)
                {
                    sqlCommand.Parameters.AddWithValue("@" + parameters[i].ParameterName, parameters[i].Value);
                }

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occured at database, if error in sql expression: {sqlExpressionInsert}, {sqlEx}");
            }
        }
    }
}
