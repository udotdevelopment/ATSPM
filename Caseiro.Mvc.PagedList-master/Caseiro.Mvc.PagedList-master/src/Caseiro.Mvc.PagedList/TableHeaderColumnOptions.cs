using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Caseiro.Mvc.PagedList
{
	/// <summary>
	/// Options for configuring the output of the table header columns
	/// </summary>
	public class TableHeaderColumnOptions
	{
		/// <summary>
		/// The default settings set all the additional content of the header links to after the title text
		/// </summary>
		public TableHeaderColumnOptions()
		{
			this.LinkAdditionalContentPosition = AdditionalContentPosition.AfterTitle;
			this.AscendingLinkAdditionalContentPosition = AdditionalContentPosition.AfterTitle;
			this.DescendingLinkAdditionalContentPosition = AdditionalContentPosition.AfterTitle;
		}

		/// <summary>
		/// Set where the additional content of the header links, if any, will appear
		/// </summary>
		public enum AdditionalContentPosition
		{
			AfterTitle,
			BeforeTitle
		}

		/// <summary>
		/// Css classes to add to the container (th) of the column
		/// </summary>
		public string ContainerCssClass { get; set; }

		/// <summary>
		/// Css classes to add to the link (a) of the column
		/// </summary>
		public string LinkCssClass { get; set; }

		/// <summary>
		/// The additional html content of the column link (a)
		/// </summary>
		public string LinkAdditionalContent { get; set; }

		/// <summary>
		/// The additional html content position in relation to the column title
		/// </summary>
		public AdditionalContentPosition LinkAdditionalContentPosition { get; set; }

		/// <summary>
		/// Css classes to add to the container (th) of the column when the actual column is ordered by ascending
		/// </summary>
		public string AscendingContainerCssClass { get; set; }

		/// <summary>
		/// Css classes to add to the link (a) of the column when the actual column is ordered by ascending
		/// </summary>
		public string AscendingLinkCssClass { get; set; }

		/// <summary>
		/// The additional html content of the column link (a) when the actual column is ordered by ascending
		/// </summary>
		public string AscendingLinkAdditionalContent { get; set; }

		/// <summary>
		/// The additional html content position in relation to the column title when the actual column is ordered by ascending
		/// </summary>
		public AdditionalContentPosition AscendingLinkAdditionalContentPosition { get; set; }

		/// <summary>
		/// Css classes to add to the container (th) of the column when the actual column is ordered by descending
		/// </summary>
		public string DescendingContainerCssClass { get; set; }

		/// <summary>
		/// Css classes to add to the link (a) of the column when the actual column is ordered by descending
		/// </summary>
		public string DescendingLinkCssClass { get; set; }

		/// <summary>
		/// The additional html content of the column link (a) when the actual column is ordered by descending
		/// </summary>
		public string DescendingLinkAdditionalContent { get; set; }

		/// <summary>
		/// The additional html content position in relation to the column title when the actual column is ordered by descending
		/// </summary>
		public AdditionalContentPosition DescendingLinkAdditionalContentPosition { get; set; }
	}
}
