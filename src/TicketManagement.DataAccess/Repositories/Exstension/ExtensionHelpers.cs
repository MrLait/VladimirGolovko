using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace TicketManagement.DataAccess.Repositories.Exstension
{
    public static class ExtensionHelpers
    {
        public static IEnumerable<T> ToEnumerable<T>(this DataTable table)
            where T : new()
        {
            List<T> list = new List<T>();

            foreach (var row in table.AsEnumerable())
            {
                T obj = new T();

                foreach (var prop in obj.GetType().GetProperties())
                {
                    PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                    propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                }

                list.Add(obj);
            }

            return list;
        }
    }
}
