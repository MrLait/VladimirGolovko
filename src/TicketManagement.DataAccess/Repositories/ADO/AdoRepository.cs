﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using TicketManagement.DataAccess.Domain.Interfaces;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.DataAccess.Repositories.ADO
{
    public class AdoRepository<T> : IRepository<T>
        where T : IEntity, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdoRepository{T}"/> class.
        /// </summary>
        /// <param name="dbConString">Connection string.</param>
        public AdoRepository(string dbConString)
        {
            DbConString = dbConString;
        }

        /// <summary>
        /// Connection string to database.
        /// </summary>
        protected string DbConString { get; private set; }

        /// <inheritdoc/>
        public void Add(T entity)
        {
            if (object.Equals(entity, default(T)))
            {
                throw new ArgumentNullException(typeof(T).Name + "object shouln't be null when saving to database");
            }

            SqlParameter[] parameters = GetAddParameter(entity).ToArray();

            string tableName = entity.GetType().Name;
            string strCol = string.Join(", ", parameters.Select(x => x.ParameterName).ToList());
            var strParam = string.Join(", ", parameters.Select(x => "@" + x.ParameterName).ToList());
            var sqlExpressionInsert = "INSERT INTO [" + tableName + "] (" + strCol + ") VALUES (" + strParam + ")";

            using (SqlConnection sqlConnection = new SqlConnection(DbConString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sqlExpressionInsert, sqlConnection))
                {
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
                        throw new ArgumentException("Some Error occured at database, if error in sql expression: " + sqlExpressionInsert, sqlEx);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public void Delete(int byId)
        {
            if (byId == 0)
            {
                throw new ArgumentNullException(paramName: "byId shouldn't be equal" + byId);
            }

            string tableName = new T().GetType().Name;
            string sqlExpressionInsert = "DELETE FROM " + tableName + " WHERE Id = " + byId;

            using (SqlConnection sqlConnection = new SqlConnection(DbConString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sqlExpressionInsert, sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch (SqlException sqlEx)
                    {
                        throw new ArgumentException("Some Error occured at database, if error in sql expression: " + sqlExpressionInsert, sqlEx);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public T GetByID(int byId)
        {
            if (byId == 0)
            {
                throw new ArgumentNullException(paramName: "byId shouldn't be equal" + byId);
            }

            string tableName = new T().GetType().Name;
            string storedProcedure = "Get" + tableName + "ById";

            using (SqlConnection sqlConnection = new SqlConnection(DbConString))
            {
                using (SqlCommand sqlCommand = SqlCommandInstance(storedProcedure, sqlConnection, new SqlParameter[] { new SqlParameter("Id", byId) }))
                {
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                    try
                    {
                        DataSet ds = new DataSet();
                        sqlDataAdapter.Fill(ds);
                        return ds.Tables[0].ToEnumerable<T>().ToList().SingleOrDefault();
                    }
                    catch (SqlException sqlEx)
                    {
                        throw new ArgumentException("Some Error occured at database, if error in stored procedure: " + storedProcedure, sqlEx);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public T Update(T entity)
        {
            throw new NotImplementedException();
        }

        private List<SqlParameter> GetAddParameter(object obj)
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
