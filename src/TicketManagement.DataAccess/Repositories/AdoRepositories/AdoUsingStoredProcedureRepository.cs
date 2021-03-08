namespace TicketManagement.DataAccess.Repositories.AdoRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;
    using TicketManagement.DataAccess.Domain.Interfaces;
    using TicketManagement.DataAccess.Exstension;

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
        /// <param name="сonnectionString">Connection string to database.</param>
        internal AdoUsingStoredProcedureRepository(string сonnectionString)
            : base(сonnectionString)
        {
        }

        /// <inheritdoc/>
        public override void Delete(T entity)
        {
            if (Equals(entity, default(T)))
            {
                throw new ArgumentException($"Can not delete null object: {entity}!");
            }

            var entityId = entity.Id;

            if (entityId <= 0)
            {
                throw new ArgumentException($"There is not this entity in the storage with Id: {entityId}!");
            }

            string tableName = new T().GetType().Name;
            string storedProcedure = $"Delete{tableName}";

            using SqlConnection sqlConnection = new SqlConnection(DbConString);
            using SqlCommand sqlCommand = SqlCommandInstance(storedProcedure, sqlConnection, new SqlParameter[] { new SqlParameter("Id", entityId) });
            try
            {
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occured at database, if error in sql expression: {storedProcedure}, {sqlEx}");
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<T> GetAll()
        {
            string tableName = new T().GetType().Name;
            string storedProcedure = $"GetAll{tableName}";

            using SqlConnection sqlConnection = new SqlConnection(DbConString);
            using SqlCommand sqlCommand = SqlCommandInstance(storedProcedure, sqlConnection);
            SqlDataAdapter adpt = new SqlDataAdapter(sqlCommand);

            try
            {
                DataSet ds = new DataSet();
                adpt.Fill(ds);
                return ds.Tables[0].ToEnumerable<T>();
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occured at database, if error in sql expression: {storedProcedure}, {sqlEx}");
            }
        }

        /// <inheritdoc/>
        public override T GetByID(int byId)
        {
            if (byId <= 0)
            {
                throw new ArgumentException($"byId shouldn't be equal {byId}");
            }

            string tableName = new T().GetType().Name;
            string storedProcedure = $"GetById{tableName}";

            using SqlConnection sqlConnection = new SqlConnection(DbConString);
            using SqlCommand sqlCommand = SqlCommandInstance(storedProcedure, sqlConnection, new SqlParameter[] { new SqlParameter("Id", byId) });
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            try
            {
                DataSet ds = new DataSet();
                sqlDataAdapter.Fill(ds);
                return ds.Tables[0].ToEnumerable<T>().SingleOrDefault();
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occured at database, if error in sql expression: {storedProcedure}, {sqlEx}");
            }
        }

        /// <inheritdoc/>
        public override void Create(T entity)
        {
            if (Equals(entity, default(T)))
            {
                throw new ArgumentException($"Can not add null object: {typeof(T).Name} !");
            }

            var storedProcedure = $"Create{entity.GetType().Name}";

            using SqlConnection sqlConnection = new SqlConnection(DbConString);
            using SqlCommand sqlCommand = SqlCommandInstance(storedProcedure, sqlConnection);
            try
            {
                sqlCommand.Parameters.AddRange(GetAddParameter(entity).ToArray());
                sqlConnection.Open();
                sqlCommand.ExecuteScalar();
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occured at database, if error in sql expression: {storedProcedure}, {sqlEx}");
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

            if (entityId <= 0)
            {
                throw new ArgumentException($"There is not this entity in the storage with Id: {entityId}!");
            }

            string tableName = new T().GetType().Name;
            string storedProcedure = $"Update{tableName}";

            using SqlConnection sqlConnection = new SqlConnection(DbConString);
            using SqlCommand sqlCommand = SqlCommandInstance(storedProcedure, sqlConnection);
            sqlCommand.Parameters.AddRange(GetUpdateParameter(entity).ToArray());

            SqlDataAdapter adpt = new SqlDataAdapter(sqlCommand);
            DataSet ds = new DataSet();

            try
            {
                adpt.Fill(ds);
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Some Error occured at database, if error in sql expression: {storedProcedure}, {sqlEx}");
            }
            catch (Exception ex)
            {
                throw ex;
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
        /// <param name="obj">Object to get propertes.</param>
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
