
namespace Caseiro.Mvc.PagedList
{
	/// <summary>
	/// Options for configuring the output of the pager
	/// </summary>
	public class PagerOptions
	{
		/// <summary>
		/// Css classes to add to the container (ul) of the pager
		/// </summary>
		public string PagerContainerCssClass { get; set; }

		/// <summary>
		/// Css classes to add to the previous page container (li) of the pager
		/// </summary>
		public string PreviousPageContainerCssClass { get; set; }

		/// <summary>
		/// Css classes to add to the previous page link (a) of the pager
		/// </summary>
		public string PreviousPageLinkCssClass { get; set; }

		/// <summary>
		/// The html content of the previous page link (a)
		/// </summary>
		public string PreviousPageContent { get; set; }

		/// <summary>
		/// Css classes to add to the previous page container (li) of the pager when there is no previous page
		/// </summary>
		public string NoPreviousPageContainerCssClass { get; set; }

		/// <summary>
		/// Css classes to add to the previous page link (a) of the pager when there is no previous page
		/// </summary>
		public string NoPreviousPageLinkCssClass { get; set; }

		/// <summary>
		/// The html content of the previous page link (a) when there is no previous page
		/// </summary>
		public string NoPreviousPageContent { get; set; }

		/// <summary>
		/// Css classes to add to the next page container (li) of the pager
		/// </summary>
		public string NextPageContainerCssClass { get; set; }

		/// <summary>
		/// Css classes to add to the next page link (a) of the pager
		/// </summary>
		public string NextPageLinkCssClass { get; set; }

		/// <summary>
		/// The html content of the next page link (a)
		/// </summary>
		public string NextPageContent { get; set; }

		/// <summary>
		/// Css classes to add to the next page container (li) of the pager when there is no next page
		/// </summary>
		public string NoNextPageContainerCssClass { get; set; }

		/// <summary>
		/// Css classes to add to the next page link (a) of the pager when there is no next page
		/// </summary>
		public string NoNextPageLinkCssClass { get; set; }

		/// <summary>
		/// The html content of the next page link (a) when there is no next page
		/// </summary>
		public string NoNextPageContent { get; set; }

		/// <summary>
		/// Css classes to add to the page container (li) of the pager
		/// </summary>
		public string PageContainerCssClass { get; set; }

		/// <summary>
		/// Css classes to add to the page link (a) of the pager
		/// </summary>
		public string PageLinkCssClass { get; set; }

		/// <summary>
		/// Css classes to add to the page container (li) of the pager selected page
		/// </summary>
		public string ActualPageContainerCssClass { get; set; }

		/// <summary>
		/// Css classes to add to the page link (a) of the pager selected page
		/// </summary>
		public string ActualPageLinkCssClass { get; set; }
	}
}
