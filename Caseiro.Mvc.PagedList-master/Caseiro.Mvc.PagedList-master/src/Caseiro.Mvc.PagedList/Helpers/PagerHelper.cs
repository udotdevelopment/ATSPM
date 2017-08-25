using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Caseiro.Mvc.PagedList.Extensions;

namespace Caseiro.Mvc.PagedList.Helpers
{
	/// <summary>
	/// Extension methods for generating pager controls for the PagedList
	/// </summary>
	public static class PagerHelper
	{
		/// <summary>
		/// Indicates of the desired page link is the next page, previous page or normal page
		/// </summary>
		private enum PageType
		{
			First,
			Last,
			Page
		}

		/// <summary>
		/// Generates a page link item for the pager
		/// </summary>
		/// <typeparam name="TList">The type of the object the PagedList collection should contain</typeparam>
		/// <param name="pagedList">The PagedList object instance</param>
		/// <param name="page">The page number of the pager item</param>
		/// <param name="pageType">The type of the pager item</param>
		/// <param name="urlFunction">A function that takes the page number and returns the page url</param>
		/// <param name="pagerOptions">Formating options</param>
		/// <returns>Page link item HTML string for the pager</returns>
		private static string GeneratePageLink<TList>(PagedList<TList> pagedList, int page, PageType pageType, Func<int, string> urlFunction, PagerOptions pagerOptions)
		{
			return GeneratePageLink(pagedList, (object)null, page, pageType, urlFunction, pagerOptions);
		}

		/// <summary>
		/// Generates a page link item for the pager with filter and order information
		/// </summary>
		/// <typeparam name="TModel">The type of the object that will be used to generate a url querystring</typeparam>
		/// <typeparam name="TList">The type of the object the PagedList collection should contain</typeparam>
		/// <param name="pagedList">The PagedList object instance</param>
		/// <param name="filterModel">The object instance that will be used to generate a url querystring</param>
		/// <param name="page">The page number of the pager item</param>
		/// <param name="pageType">The type of the pager item</param>
		/// <param name="urlFunction">A function that takes the page number and returns the page url</param>
		/// <param name="pagerOptions">Formating options</param>
		/// <returns>Page link item HTML string for the pager with filter and order information</returns>
		private static string GeneratePageLink<TModel, TList>(PagedList<TList> pagedList, TModel filterModel, int page, PageType pageType, Func<int, string> urlFunction, PagerOptions pagerOptions)
		{
			TagBuilder li = new TagBuilder("li");
			TagBuilder a = new TagBuilder("a");

            a.AddCssClass("pager-action");

			if (filterModel == null)
			{
				a.Attributes.Add("href", urlFunction(page));
			}
			else
			{
				a.Attributes.Add("href", "javascript:GetSignals("+ page +")");
			}

			if (pageType == PageType.First)
			{
				if (pagedList.ActualPage == 1)
				{
					if (!string.IsNullOrWhiteSpace(pagerOptions.NoPreviousPageContainerCssClass))
					{
						li.AddCssClass(pagerOptions.NoPreviousPageContainerCssClass);
					}
					else
					{
						li.AddCssClass("disabled");
					}

					if (!string.IsNullOrWhiteSpace(pagerOptions.NoPreviousPageLinkCssClass))
					{
						a.AddCssClass(pagerOptions.NoPreviousPageLinkCssClass);
					}

					if (!string.IsNullOrWhiteSpace(pagerOptions.NoPreviousPageContent))
					{
						a.InnerHtml = pagerOptions.NoPreviousPageContent;
					}
					else
					{
						a.InnerHtml = "&lt;&lt;";
					}
				}
				else
				{
					if (!string.IsNullOrWhiteSpace(pagerOptions.PreviousPageContainerCssClass))
					{
						li.AddCssClass(pagerOptions.PreviousPageContainerCssClass);
					}

					if (!string.IsNullOrWhiteSpace(pagerOptions.PreviousPageLinkCssClass))
					{
						a.AddCssClass(pagerOptions.PreviousPageLinkCssClass);
					}

					if (!string.IsNullOrWhiteSpace(pagerOptions.PreviousPageContent))
					{
						a.InnerHtml = pagerOptions.PreviousPageContent;
					}
					else
					{
						a.InnerHtml = "&lt;&lt;";
					}
				}
			}
			else if (pageType == PageType.Last)
			{
				if (pagedList.ActualPage == pagedList.PageCount || pagedList.PageCount == 1)
				{
					if (!string.IsNullOrWhiteSpace(pagerOptions.NextPageContainerCssClass))
					{
						li.AddCssClass(pagerOptions.NextPageContainerCssClass);
					}
					else
					{
						li.AddCssClass("disabled");
					}

					if (!string.IsNullOrWhiteSpace(pagerOptions.NoNextPageLinkCssClass))
					{
						a.AddCssClass(pagerOptions.NoNextPageLinkCssClass);
					}

					if (!string.IsNullOrWhiteSpace(pagerOptions.NoNextPageContent))
					{
						a.InnerHtml = pagerOptions.NoNextPageContent;
					}
					else
					{
						a.InnerHtml = "&gt;&gt;";
					}
				}
				else
				{
					if (!string.IsNullOrWhiteSpace(pagerOptions.NextPageContainerCssClass))
					{
						li.AddCssClass(pagerOptions.NextPageContainerCssClass);
					}

					if (!string.IsNullOrWhiteSpace(pagerOptions.NextPageLinkCssClass))
					{
						a.AddCssClass(pagerOptions.NextPageLinkCssClass);
					}

					if (!string.IsNullOrWhiteSpace(pagerOptions.NextPageContent))
					{
						a.InnerHtml = pagerOptions.NextPageContent;
					}
					else
					{
						a.InnerHtml = "&gt;&gt;";
					}
				}
			}
			else
			{
				if (page == pagedList.ActualPage)
				{
					if (!string.IsNullOrWhiteSpace(pagerOptions.ActualPageContainerCssClass))
					{
						li.AddCssClass(pagerOptions.ActualPageContainerCssClass);
					}
					else
					{
						li.AddCssClass("active");
					}

					if (!string.IsNullOrWhiteSpace(pagerOptions.ActualPageLinkCssClass))
					{
						a.AddCssClass(pagerOptions.ActualPageLinkCssClass);
					}
				}
				else
				{
					if (!string.IsNullOrWhiteSpace(pagerOptions.PageContainerCssClass))
					{
						li.AddCssClass(pagerOptions.PageContainerCssClass);
					}

					if (!string.IsNullOrWhiteSpace(pagerOptions.PageLinkCssClass))
					{
						a.AddCssClass(pagerOptions.PageLinkCssClass);
					}
				}
				
				a.SetInnerText(page.ToString());
			}

			li.InnerHtml = a.ToString();

			return li.ToString();
		}

		/// <summary>
		/// Displays a pager for a PagedList instance
		/// </summary>
		/// <typeparam name="TList">The type of the object the PagedList collection should contain</typeparam>
		/// <param name="htmlHelper">The HTML helper instance that this method extends</param>
		/// <param name="pagedList">The PagedList object instance</param>
		/// <param name="urlFunction">A function that takes the page number and returns the page url</param>
		/// <returns>A pager for a PagedList instance</returns>
		public static MvcHtmlString Pager<TList>(this HtmlHelper htmlHelper, PagedList<TList> pagedList, Func<int, string> urlFunction)
		{
			return Pager(htmlHelper, pagedList, urlFunction, null);
		}

		/// <summary>
		/// Displays a pager for a PagedList instance
		/// </summary>
		/// <typeparam name="TList">The type of the object the PagedList collection should contain</typeparam>
		/// <param name="htmlHelper">The HTML helper instance that this method extends</param>
		/// <param name="pagedList">The PagedList object instance</param>
		/// <param name="urlFunction">A function that takes the page number and returns the page url</param>
		/// <param name="pagerOptions">Formating options</param>
		/// <returns>A pager for a PagedList instance</returns>
		public static MvcHtmlString Pager<TList>(this HtmlHelper htmlHelper, PagedList<TList> pagedList, Func<int, string> urlFunction, PagerOptions pagerOptions)
		{
			pagerOptions = pagerOptions ?? new PagerOptions();

			TagBuilder ul = new TagBuilder("ul");
			ul.AddCssClass("pagination");

			if (!string.IsNullOrWhiteSpace(pagerOptions.PagerContainerCssClass))
			{
				ul.AddCssClass(pagerOptions.PagerContainerCssClass);
			}

			int previousPage = (pagedList.ActualPage == 1) ? 1 : pagedList.ActualPage - 1;
			int nextPage = (pagedList.ActualPage + 1 > pagedList.PageCount) ? pagedList.PageCount : pagedList.ActualPage + 1;

			ul.InnerHtml += GeneratePageLink(pagedList, previousPage, PageType.First, urlFunction, pagerOptions);

			int pageCountBefore = pagedList.PagerSize / 2;
			int pageCountAfter = pagedList.PagerSize - pageCountBefore;

			for (int page = ((pagedList.ActualPage - pageCountBefore) < 1) ? 1 : pagedList.ActualPage - pageCountBefore;
				page <= pagedList.PageCount && page <= (pageCountBefore + pageCountAfter + pagedList.ActualPage); page++)
			{
				ul.InnerHtml += GeneratePageLink(pagedList, page, PageType.Page, urlFunction, pagerOptions);
			}

			ul.InnerHtml += GeneratePageLink(pagedList, nextPage, PageType.Last, urlFunction, pagerOptions);

			return new MvcHtmlString(ul.ToString());
		}

		/// <summary>
		/// Displays a pager for a PagedList instance
		/// </summary>
		/// <typeparam name="TModel">The type of object that contains a PagedList property</typeparam>
		/// <typeparam name="TList">The type of the object the PagedList collection should contain</typeparam>
		/// <param name="htmlHelper">The HTML helper instance that this method extends</param>
		/// <param name="expression">An expression that identifies the PagedList property</param>
		/// <param name="urlFunction">A function that takes the page number and returns the page url</param>
		/// <returns>>A pager for a PagedList instance</returns>
		public static MvcHtmlString PagerFor<TModel, TList>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, PagedList<TList>>> expression, Func<int, string> urlFunction)
		{
			return PagerFor(htmlHelper, expression, urlFunction, null);
		}

		/// <summary>
		/// Displays a pager for a PagedList instance
		/// </summary>
		/// <typeparam name="TModel">The type of object that contains a PagedList property</typeparam>
		/// <typeparam name="TList">The type of the object the PagedList collection should contain</typeparam>
		/// <param name="htmlHelper">The HTML helper instance that this method extends</param>
		/// <param name="expression">An expression that identifies the PagedList property</param>
		/// <param name="urlFunction">A function that takes the page number and returns the page url</param>
		/// <param name="pagerOptions">Formating options</param>
		/// <returns>A pager for a PagedList instance</returns>
		public static MvcHtmlString PagerFor<TModel, TList>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, PagedList<TList>>> expression, Func<int, string> urlFunction, PagerOptions pagerOptions)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			var pagedList = metadata.Model as PagedList<TList>;

			pagerOptions = pagerOptions ?? new PagerOptions();

			TagBuilder ul = new TagBuilder("ul");
			ul.AddCssClass("pagination");

			if (!string.IsNullOrWhiteSpace(pagerOptions.PagerContainerCssClass))
			{
				ul.AddCssClass(pagerOptions.PagerContainerCssClass);
			}

			int previousPage = (pagedList.ActualPage == 1) ? 1 : pagedList.ActualPage - 1;
			int nextPage = (pagedList.ActualPage + 1 > pagedList.PageCount) ? pagedList.PageCount : pagedList.ActualPage + 1;

			ul.InnerHtml += GeneratePageLink(pagedList, previousPage, PageType.First, urlFunction, pagerOptions);

			int pageCountBefore = pagedList.PagerSize / 2;
			int pageCountAfter = pagedList.PagerSize - pageCountBefore;

			for (int page = ((pagedList.ActualPage - pageCountBefore) < 1) ? 1 : pagedList.ActualPage - pageCountBefore;
				page <= pagedList.PageCount && page <= (pageCountBefore + pageCountAfter + pagedList.ActualPage); page++)
			{
				ul.InnerHtml += GeneratePageLink(pagedList, page, PageType.Page, urlFunction, pagerOptions);
			}

			ul.InnerHtml += GeneratePageLink(pagedList, nextPage, PageType.Last, urlFunction, pagerOptions);

			return new MvcHtmlString(ul.ToString());
		}

		/// <summary>
		/// Displays a pager for a PagedList instance with filter and order information
		/// </summary>
		/// <typeparam name="TModel">The type of filter object</typeparam>
		/// <typeparam name="TList">The type of the object the PagedList collection should contain</typeparam>
		/// <param name="htmlHelper">The HTML helper instance that this method extends</param>
		/// <param name="pagedList">The PagedList object instance</param>
		/// <param name="filterModel">The filter object instance</param>
		/// <param name="urlFunction">A function that takes the page number and returns the page url</param>
		/// <returns>A pager for a PagedList instance with filter and order information</returns>
		public static MvcHtmlString PagerFilter<TModel, TList>(this HtmlHelper htmlHelper, PagedList<TList> pagedList, TModel filterModel, Func<int, string> urlFunction)
		{
			return PagerFilter(htmlHelper, pagedList, filterModel, urlFunction, null);
		}

		/// <summary>
		/// Displays a pager for a PagedList instance with filter and order information
		/// </summary>
		/// <typeparam name="TModel">The type of filter object</typeparam>
		/// <typeparam name="TList">The type of the object the PagedList collection should contain</typeparam>
		/// <param name="htmlHelper">The HTML helper instance that this method extends</param>
		/// <param name="pagedList">The PagedList object instance</param>
		/// <param name="filterModel">The filter object instance</param>
		/// <param name="urlFunction">A function that takes the page number and returns the page url</param>
		/// <param name="pagerOptions">Formating options</param>
		/// <returns>A pager for a PagedList instance</returns>
		public static MvcHtmlString PagerFilter<TModel, TList>(this HtmlHelper htmlHelper, PagedList<TList> pagedList, TModel filterModel, Func<int, string> urlFunction, PagerOptions pagerOptions)
		{
			pagerOptions = pagerOptions ?? new PagerOptions();

			TagBuilder ul = new TagBuilder("ul");
			ul.AddCssClass("pagination");

			if (!string.IsNullOrWhiteSpace(pagerOptions.PagerContainerCssClass))
			{
				ul.AddCssClass(pagerOptions.PagerContainerCssClass);
			}

			int previousPage = (pagedList.ActualPage == 1) ? 1 : pagedList.ActualPage - 1;
			int nextPage = (pagedList.ActualPage + 1 > pagedList.PageCount) ? pagedList.PageCount : pagedList.ActualPage + 1;

			ul.InnerHtml += GeneratePageLink(pagedList, filterModel, previousPage, PageType.First, urlFunction, pagerOptions);

			int pageCountBefore = pagedList.PagerSize / 2;
			int pageCountAfter = pagedList.PagerSize - pageCountBefore;

			for (int page = ((pagedList.ActualPage - pageCountBefore) < 1) ? 1 : pagedList.ActualPage - pageCountBefore;
				page <= pagedList.PageCount && page <= (pageCountBefore + pageCountAfter + pagedList.ActualPage); page++)
			{
				ul.InnerHtml += GeneratePageLink(pagedList, filterModel, page, PageType.Page, urlFunction, pagerOptions);
			}

			ul.InnerHtml += GeneratePageLink(pagedList, filterModel, nextPage, PageType.Last, urlFunction, pagerOptions);

			return new MvcHtmlString(ul.ToString());
		}

		/// <summary>
		/// Displays a pager for a PagedList instance with filter and order information
		/// </summary>
		/// <typeparam name="TModel">The type of filter object</typeparam>
		/// <typeparam name="TList">The type of the object the PagedList collection should contain</typeparam>
		/// <param name="htmlHelper">The HTML helper instance that this method extends</param>
		/// <param name="expression">An expression that identifies the PagedList property</param>
		/// <param name="urlFunction">A function that takes the page number and returns the page url</param>
		/// <returns>A pager for a PagedList instance in the filter object</returns>
		public static MvcHtmlString PagerFilterFor<TModel, TList>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, PagedList<TList>>> expression, Func<int, string> urlFunction)
		{
			return PagerFilterFor(htmlHelper, expression, urlFunction, null);
		}

		/// <summary>
		/// Displays a pager for a PagedList instance with filter and order information
		/// </summary>
		/// <typeparam name="TModel">The type of filter object</typeparam>
		/// <typeparam name="TList">The type of the object the PagedList collection should contain</typeparam>
		/// <param name="htmlHelper">The HTML helper instance that this method extends</param>
		/// <param name="expression">An expression that identifies the PagedList property</param>
		/// <param name="urlFunction">A function that takes the page number and returns the page url</param>
		/// <param name="pagerOptions">Formating options</param>
		/// <returns>A pager for a PagedList instance in the filter object</returns>
		public static MvcHtmlString PagerFilterFor<TModel, TList>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, PagedList<TList>>> expression, Func<int, string> urlFunction, PagerOptions pagerOptions)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			var pagedList = metadata.Model as PagedList<TList>;
			var filterModel = htmlHelper.ViewData.Model;

			pagerOptions = pagerOptions ?? new PagerOptions();

			TagBuilder ul = new TagBuilder("ul");
			ul.AddCssClass("pagination");

			if (!string.IsNullOrWhiteSpace(pagerOptions.PagerContainerCssClass))
			{
				ul.AddCssClass(pagerOptions.PagerContainerCssClass);
			}

			int previousPage = (pagedList.ActualPage == 1) ? 1 : pagedList.ActualPage - 1;
			int nextPage = (pagedList.ActualPage + 1 > pagedList.PageCount) ? pagedList.PageCount : pagedList.ActualPage + 1;

			ul.InnerHtml += GeneratePageLink(pagedList, filterModel, previousPage, PageType.First, urlFunction, pagerOptions);

			int pageCountBefore = pagedList.PagerSize / 2;
			int pageCountAfter = pagedList.PagerSize - pageCountBefore;

			for (int page = ((pagedList.ActualPage - pageCountBefore) < 1) ? 1 : pagedList.ActualPage - pageCountBefore;
				page <= pagedList.PageCount && page <= (pageCountBefore + pageCountAfter + pagedList.ActualPage); page++)
			{
				ul.InnerHtml += GeneratePageLink(pagedList, filterModel, page, PageType.Page, urlFunction, pagerOptions);
			}

			ul.InnerHtml += GeneratePageLink(pagedList, filterModel, nextPage, PageType.Last, urlFunction, pagerOptions);

			return new MvcHtmlString(ul.ToString());
		}
	}
}
