using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public static class ExtensionMethods
    {
        public static IEnumerable<T> OrderDataBy<T>(this List<T> list, string sortExpression)
        {
            sortExpression += "";
            string[] parts = sortExpression.Split(' ');
            bool descending = false;
            string property = "";

            if (parts.Length > 0 && parts[0] != "")
            {
                property = parts[0];

                if (parts.Length > 1)
                {
                    descending = parts[1].ToLower().Contains("desc");
                }

                PropertyInfo prop = typeof(T).GetProperty(property, BindingFlags.SetProperty | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (prop == null)
                {
                    throw new Exception("No property '" + property + "' in + " + typeof(T).Name + "'");
                }

                if (descending)
                    return list.OrderByDescending(x => prop.GetValue(x, null));
                else
                    return list.OrderBy(x => prop.GetValue(x, null));
            }

            return list;
        }
    }
}
