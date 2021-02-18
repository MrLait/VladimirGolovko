using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TicketManagement.DataAccess.Domain.Interfaces;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Repositories.Exstension;

namespace TicketManagement.DataAccess.Repositories.ADO
{
    public class AdoUsingStoredProcedureRepository<T> : AdoRepository<T>, IUsingStoredProcedureRepository<T>
        where T : IEntity, new()
    {
        public AdoUsingStoredProcedureRepository(string dbConString)
            : base(dbConString)
        {
        }

        /// <inheritdoc/>
        public override void Delete(int byId)
        {
            if (byId == 0)
            {
                throw new ArgumentNullException(paramName: "byId shouldn't be equal" + byId);
            }

            string tableName = new T().GetType().Name;
            string storedProcedure = "Delete" + tableName;

            using (SqlConnection sqlConnection = new SqlConnection(DbConString))
            {
                using (SqlCommand sqlCommand = SqlCommandInstance(storedProcedure, sqlConnection, new SqlParameter[] { new SqlParameter("Id", byId) }))
                {
                    try
                    {
                        sqlConnection.Open();
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch (SqlException sqlEx)
                    {
                        throw new ArgumentException("Some Error occured at database, if error in stored procedure: " + storedProcedure, sqlEx);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<T> GetAll()
        {
            string tableName = new T().GetType().Name;
            string storedProcedure = "GetAll" + tableName;

            using (SqlConnection sqlConnection = new SqlConnection(DbConString))
            {
                using (SqlCommand sqlCommand = SqlCommandInstance(storedProcedure, sqlConnection))
                {
                    SqlDataAdapter adpt = new SqlDataAdapter(sqlCommand);

                    try
                    {
                        DataSet ds = new DataSet();
                        adpt.Fill(ds);
                        return ds.Tables[0].ToEnumerable<T>();
                    }
                    catch (SqlException sqlEx)
                    {
                        throw new ArgumentException("Some Error occured at database, if error in stored procedure. See inner exception for more detail exception." + storedProcedure, sqlEx);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override T GetByID(int byId)
        {
            if (byId == 0)
            {
                throw new ArgumentNullException(paramName: "byId shouldn't be equal" + byId);
            }

            string tableName = new T().GetType().Name;
            string storedProcedure = "GetById" + tableName;

            using (SqlConnection sqlConnection = new SqlConnection(DbConString))
            {
                using (SqlCommand sqlCommand = SqlCommandInstance(storedProcedure, sqlConnection, new SqlParameter[] { new SqlParameter("Id", byId) }))
                {
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                    try
                    {
                        DataSet ds = new DataSet();
                        sqlDataAdapter.Fill(ds);
                        return ds.Tables[0].ToEnumerable<T>().SingleOrDefault();
                    }
                    catch (SqlException sqlEx)
                    {
                        throw new ArgumentException("Some Error occured at database, if error in stored procedure: " + storedProcedure, sqlEx);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override void Create(T entity)
        {
            if (Equals(entity, default(T)))
            {
                throw new ArgumentNullException(typeof(T).Name + "object shouln't be null when saving to database");
            }

            var storedProcedure = "Create" + entity.GetType().Name;

            using (SqlConnection sqlConnection = new SqlConnection(DbConString))
            {
                using (SqlCommand sqlCommand = SqlCommandInstance(storedProcedure, sqlConnection))
                {
                    try
                    {
                        sqlCommand.Parameters.AddRange(GetAddParameter(entity).ToArray());
                        sqlConnection.Open();
                        sqlCommand.ExecuteScalar();
                    }
                    catch (SqlException sqlEx)
                    {
                        throw new ArgumentException("Some Error occured at database, if error in stored procedure: " + storedProcedure, sqlEx);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override void Update(T entity)
        {
            throw new NotImplementedException();
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
    }
}
