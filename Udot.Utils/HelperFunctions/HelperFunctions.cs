using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Udot.Utils.HelperFunctions
{
    public static class HelperFunctions
    {
        /// <summary>
        /// Will copy T values, only one level deep. It will not recurse
        /// Returns a dictionary of values it set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public static Dictionary<string, string> CopyValues<T>(T target, T source)
        {
            Type t = typeof(T);
            Dictionary<string, string> propertyDifferences = new Dictionary<string, string>();

            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(source, null);
                if (value != null)
                {
                    var targetValue = prop.GetValue(target, null);
                    if (targetValue == null || Equals(value, targetValue) == false)
                    {
                        //Only set value if the target is null or value is different
                        propertyDifferences.Add(prop.Name, value.ToString());
                        prop.SetValue(target, value, null);
                    }
                }
            }
            return propertyDifferences;
        }

        /// <summary>
        /// Reflection helper
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Expression<Func<TSource, object>> GetExpression<TSource>(string propertyName)
        {
            var param = Expression.Parameter(typeof(TSource), "x");
            Expression conversion = Expression.Convert(Expression.Property
            (param, propertyName), typeof(object));   //important to use the Expression.Convert
            return Expression.Lambda<Func<TSource, object>>(conversion, param);
        }

        /// <summary>
        /// Helper for enums. Easier to loop over them using this.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }

}