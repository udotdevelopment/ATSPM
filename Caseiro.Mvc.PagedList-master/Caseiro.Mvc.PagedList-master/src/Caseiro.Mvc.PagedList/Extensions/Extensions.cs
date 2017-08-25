using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caseiro.Mvc.PagedList.Extensions
{
	/// <summary>
	/// Extensions methods for generating paged lists
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Creates a new instance of the PagedList class with the items of the informed page
		/// </summary>
		/// <typeparam name="T">The type of the object the collection should contain</typeparam>
		/// <param name="queryable">The collection os objects to be paged</param>
		/// <param name="page">The number of the desired page</param>
		/// <param name="pageSize">The size of each page</param>
		/// <returns>A new instance of the PagedList class with the items of the informed page</returns>
		public static PagedList<T> ToPagedList<T>(this IQueryable<T> queryable, int page, int pageSize)
		{
			return new PagedList<T>(queryable, page, pageSize);
		}

		/// <summary>
		/// Creates a new instance of the PagedList class with the items of the informed page and ordered by the informed property and direction
		/// </summary>
		/// <typeparam name="T">The type of the object the collection should contain</typeparam>
		/// <param name="queryable">The collection os objects to be paged</param>
		/// <param name="page">The number of the desired page</param>
		/// <param name="pageSize">The size of each page</param>
		/// <param name="orderField">The name of property to be ordered by</param>
		/// <param name="orderDirection">The direction of the ordering</param>
		/// <returns>A new instance of the PagedList class with the items of the informed page and ordered by the informed property and direction</returns>
		public static PagedList<T> ToPagedList<T>(this IQueryable<T> queryable, int page, int pageSize, string orderField, OrderDirection orderDirection)
		{
			return new PagedList<T>(queryable, page, pageSize, orderField, orderDirection);
		}

		/// <summary>
		/// Generates a url querystring using the property names and values of the informed class
		/// </summary>
		/// <typeparam name="T">The type of the object to be transformed to a querytring</typeparam>
		/// <param name="model">The object instance to be transformed to a querytring</param>
		/// <returns>A url querystring using the property names and values of the informed class</returns>
		public static string ToQueryString<T>(this T model)
		{
			var keyPairs = typeof(T).GetProperties().Where(p => p.PropertyType.IsSerializable &&
				!p.GetCustomAttributes(false).Any(a => a.GetType().Equals(typeof(Attributes.IgnoreQueryStringAttribute))))
					.Select(p => new KeyValuePair<object, object>(p.Name, p.GetValue(model, null)));

			StringBuilder queryString = new StringBuilder();
			foreach (var item in keyPairs)
			{
				if (queryString.Length > 0)
				{
					queryString.Append("&");
				}
				queryString.Append(String.Format("{0}={1}", item.Key, item.Value));
			}
			return "?" + queryString.ToString();
		}

		/// <summary>
		/// Generates a url querystring using the property names and values of the informed class, changing the values for the informed property names
		/// </summary>
		/// <typeparam name="T">The type of the object to be transformed to a querytring</typeparam>
		/// <param name="model">The object instance to be transformed to a querytring</param>
		/// <param name="keyValue">Pair containing the property name and its new value</param>
		/// <returns>A url querystring using the property names and values of the informed class, changing the values for the informed property names</returns>
		public static string ToQueryString<T>(this T model, KeyValuePair<object, object> keyValue)
		{
			var keyPairs = typeof(T).GetProperties().Where(p => p.PropertyType.IsSerializable &&
				!p.GetCustomAttributes(false).Any(a => a.GetType().Equals(typeof(Attributes.IgnoreQueryStringAttribute))))
					.Select(p => new KeyValuePair<object, object>(p.Name, p.GetValue(model, null)));

			StringBuilder queryString = new StringBuilder();
			foreach (var item in keyPairs)
			{
				if (queryString.Length > 0)
				{
					queryString.Append("&");
				}
				if (item.Key.ToString().Equals(keyValue.Key.ToString(), StringComparison.OrdinalIgnoreCase))
				{
					queryString.Append(String.Format("{0}={1}", item.Key, keyValue.Value));
				}
				else
				{
					queryString.Append(String.Format("{0}={1}", item.Key, item.Value));
				}
			}
			return "?" + queryString.ToString();
		}
	}
}
