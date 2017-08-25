using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caseiro.Mvc.PagedList.Attributes
{
	/// <summary>
	/// Attribute to ignore the decorated property when using the ToQueryString extension method
	/// </summary>
	public class IgnoreQueryStringAttribute : Attribute
	{
	}
}
