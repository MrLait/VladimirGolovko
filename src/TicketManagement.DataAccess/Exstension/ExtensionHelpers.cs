using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using TicketManagement.DataAccess.Enums;

namespace TicketManagement.DataAccess.Exstension
{
    /// <summary>
    /// Helpers to convert models from DataTable to IEnumerable object using reflection.
    /// </summary>
    internal static class ExtensionHelpers
    {
        /// <summary>
        /// Method to convert models from DataTable to IEnumerable object using reflection.
        /// </summary>
        /// <typeparam name="T">Table model.</typeparam>
        /// <param name="table">Table.</param>
        /// <returns>Returns IEnumerable with entity object.</returns>
        public static IEnumerable<T> ToEnumerable<T>(this DataTable table)
            where T : new()
        {
            var list = new List<T>();
            foreach (var row in table.AsEnumerable())
            {
                T obj = new T();

                foreach (var prop in obj.GetType().GetProperties())
                {
                    try
                    {
                        PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                        propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                    }
                    catch
                    {
#pragma warning disable S3626 // Jump statements should not be redundant
                        continue;
#pragma warning restore S3626 // Jump statements should not be redundant
                    }
                }

                list.Add(obj);
            }

            return list;
        }
    }
}
