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
	/// Extension methods for generating table header columns with filter and order information
	/// </summary>
	public static class TableHelper
	{
		/// <summary>
		/// Generates a table header column (th)
		/// </summary>
		/// <typeparam name="TModel">The type of the object that will be used to generate a url querystring</typeparam>
		/// <param name="formAction">The filter form action url</param>
		/// <param name="propertyName">The column property name</param>
		/// <param name="displayName">The column display name</param>
		/// <param name="orderPropertyName">The filter form order property name</param>
		/// <param name="orderPropertyValue">The filter form order property value</param>
		/// <param name="orderDirectionProperty">The filter form order direction property name</param>
		/// <param name="orderDirectionValue">The filter form order direction property value</param>
		/// <param name="filterModel">The object instance that will be used to generate a url querystring</param>
		/// <param name="columnOptions">Formatting options</param>
		/// <returns>Table header column HTML string</returns>
		private static string CreateColumn<TModel>(string formAction, string propertyName, string displayName, string orderPropertyName, string orderPropertyValue, string orderDirectionProperty,
			OrderDirection orderDirectionValue, TModel filterModel, TableHeaderColumnOptions columnOptions)
		{
			TagBuilder th = new TagBuilder("th");
			TagBuilder a = new TagBuilder("a");

			if (propertyName.Equals(orderPropertyValue, StringComparison.OrdinalIgnoreCase))
			{
				a.Attributes.Add("href", formAction + filterModel.ToQueryString(new KeyValuePair<object, object>(orderDirectionProperty,
					orderDirectionValue == OrderDirection.Ascending ? OrderDirection.Descending.ToString() : OrderDirection.Ascending.ToString())));
				a.InnerHtml = displayName;

				if (orderDirectionValue == OrderDirection.Ascending)
				{
					if (!string.IsNullOrWhiteSpace(columnOptions.AscendingContainerCssClass))
					{
						th.AddCssClass(columnOptions.AscendingContainerCssClass);
					}

					if (!string.IsNullOrWhiteSpace(columnOptions.AscendingLinkCssClass))
					{
						a.AddCssClass(columnOptions.AscendingLinkCssClass);
					}

					if (columnOptions.AscendingLinkAdditionalContentPosition == TableHeaderColumnOptions.AdditionalContentPosition.AfterTitle
						&& !string.IsNullOrWhiteSpace(columnOptions.AscendingLinkAdditionalContent))
					{
						a.InnerHtml += columnOptions.AscendingLinkAdditionalContent;
					}
					else if (columnOptions.AscendingLinkAdditionalContentPosition == TableHeaderColumnOptions.AdditionalContentPosition.BeforeTitle
						&& !string.IsNullOrWhiteSpace(columnOptions.AscendingLinkAdditionalContent))
					{
						a.InnerHtml = columnOptions.AscendingLinkAdditionalContent + a.InnerHtml;
					}
				}
				else
				{
					if (!string.IsNullOrWhiteSpace(columnOptions.DescendingContainerCssClass))
					{
						th.AddCssClass(columnOptions.DescendingContainerCssClass);
					}

					if (!string.IsNullOrWhiteSpace(columnOptions.DescendingLinkCssClass))
					{
						a.AddCssClass(columnOptions.DescendingLinkCssClass);
					}

					if (columnOptions.DescendingLinkAdditionalContentPosition == TableHeaderColumnOptions.AdditionalContentPosition.AfterTitle
						&& !string.IsNullOrWhiteSpace(columnOptions.DescendingLinkAdditionalContent))
					{
						a.InnerHtml += columnOptions.DescendingLinkAdditionalContent;
					}
					else if (columnOptions.DescendingLinkAdditionalContentPosition == TableHeaderColumnOptions.AdditionalContentPosition.BeforeTitle
						&& !string.IsNullOrWhiteSpace(columnOptions.DescendingLinkAdditionalContent))
					{
						a.InnerHtml = columnOptions.DescendingLinkAdditionalContent + a.InnerHtml;
					}
				}
			}
			else
			{
				a.Attributes.Add("href", formAction + filterModel.ToQueryString(new KeyValuePair<object, object>(orderPropertyName, propertyName)));
				a.InnerHtml = displayName;

				if (!string.IsNullOrWhiteSpace(columnOptions.ContainerCssClass))
				{
					th.AddCssClass(columnOptions.ContainerCssClass);
				}

				if (!string.IsNullOrWhiteSpace(columnOptions.LinkCssClass))
				{
					a.AddCssClass(columnOptions.LinkCssClass);
				}

				if (columnOptions.LinkAdditionalContentPosition == TableHeaderColumnOptions.AdditionalContentPosition.AfterTitle
					&& !string.IsNullOrWhiteSpace(columnOptions.LinkAdditionalContent))
				{
					a.InnerHtml += columnOptions.LinkAdditionalContent;
				}
				else if (columnOptions.LinkAdditionalContentPosition == TableHeaderColumnOptions.AdditionalContentPosition.BeforeTitle
					&& !string.IsNullOrWhiteSpace(columnOptions.LinkAdditionalContent))
				{
					a.InnerHtml = columnOptions.LinkAdditionalContent + a.InnerHtml;
				}
			}

			th.InnerHtml = a.ToString();

			return th.ToString();
		}

		/// <summary>
		/// Displays an unique table header column with filter and order information
		/// </summary>
		/// <typeparam name="TModel">The type of the filter object</typeparam>
		/// <typeparam name="TProperty">The type of the property in the filter object</typeparam>
		/// <param name="htmlHelper">The HTML helper instance that this method extends</param>
		/// <param name="formAction">The filter form action url</param>
		/// <param name="orderPropertyExpression">An expression that identifies the order property</param>
		/// <param name="orderDirectionExpression">An expression that identifies the order direction property</param>
		/// <param name="expression">An expression that identifies the column property</param>
		/// <returns>A table header column element for the specified property in the filter object</returns>
		public static MvcHtmlString TableHeaderColumnFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string formAction, Expression<Func<TModel, string>> orderPropertyExpression,
				Expression<Func<TModel, OrderDirection>> orderDirectionExpression, Expression<Func<TModel, TProperty>> expression)
		{
			return TableHeaderColumnFor(htmlHelper, formAction, orderPropertyExpression, orderDirectionExpression, expression, null);
		}

		/// <summary>
		/// Displays an unique table header column with filter and order information
		/// </summary>
		/// <typeparam name="TModel">The type of the filter object</typeparam>
		/// <typeparam name="TProperty">The type of the property in the filter object</typeparam>
		/// <param name="htmlHelper">The HTML helper instance that this method extends</param>
		/// <param name="formAction">The filter form action url</param>
		/// <param name="orderPropertyExpression">An expression that identifies the order property</param>
		/// <param name="orderDirectionExpression">An expression that identifies the order direction property</param>
		/// <param name="expression">An expression that identifies the column property</param>
		/// <param name="columnOptions">Formatting options</param>
		/// <returns>A table header column element for the specified property in the filter object</returns>
		public static MvcHtmlString TableHeaderColumnFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string formAction, Expression<Func<TModel, string>> orderPropertyExpression,
				Expression<Func<TModel, OrderDirection>> orderDirectionExpression, Expression<Func<TModel, TProperty>> expression, TableHeaderColumnOptions columnOptions)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			ModelMetadata orderPropertyMetadata = ModelMetadata.FromLambdaExpression(orderPropertyExpression, htmlHelper.ViewData);
			ModelMetadata orderDirectionMetadata = ModelMetadata.FromLambdaExpression(orderDirectionExpression, htmlHelper.ViewData);
			columnOptions = columnOptions ?? new TableHeaderColumnOptions();

			return new MvcHtmlString(CreateColumn(formAction, metadata.PropertyName, metadata.DisplayName ?? metadata.PropertyName, orderPropertyMetadata.PropertyName, orderPropertyMetadata.Model.ToString(),
					orderDirectionMetadata.PropertyName, (OrderDirection)Enum.Parse(typeof(OrderDirection), orderDirectionMetadata.Model.ToString(), true), htmlHelper.ViewData.Model,
					columnOptions));
		}

		/// <summary>
		/// Displays multiple table header columns with filter and order information
		/// </summary>
		/// <typeparam name="TModel">The type of the filter object</typeparam>
		/// <param name="htmlHelper">The HTML helper instance that this method extends</param>
		/// <param name="formAction">The filter form action url</param>
		/// <param name="orderPropertyExpression">An expression that identifies the order property</param>
		/// <param name="orderDirectionExpression">An expression that identifies the order direction property</param>
		/// <param name="expression">An expression that identifies the columns properties</param>
		/// <returns>Multiple table header columns elements for the specified properties in the filter object</returns>
		public static MvcHtmlString TableHeaderColumnsFor<TModel>(this HtmlHelper<TModel> htmlHelper, string formAction, Expression<Func<TModel, string>> orderPropertyExpression,
			Expression<Func<TModel, OrderDirection>> orderDirectionExpression, Expression<Func<TModel, object>> expression)
		{
			return TableHeaderColumnsFor(htmlHelper, formAction, orderPropertyExpression, orderDirectionExpression, expression, null);
		}

		/// <summary>
		/// Displays multiple table header columns with filter and order information
		/// </summary>
		/// <typeparam name="TModel">The type of the filter object</typeparam>
		/// <param name="htmlHelper">The HTML helper instance that this method extends</param>
		/// <param name="formAction">The filter form action url</param>
		/// <param name="orderPropertyExpression">An expression that identifies the order property</param>
		/// <param name="orderDirectionExpression">An expression that identifies the order direction property</param>
		/// <param name="expression">An expression that identifies the columns properties</param>
		/// <param name="columnOptions">Formatting options</param>
		/// <returns>Multiple table header columns elements for the specified properties in the filter object</returns>
		public static MvcHtmlString TableHeaderColumnsFor<TModel>(this HtmlHelper<TModel> htmlHelper, string formAction, Expression<Func<TModel, string>> orderPropertyExpression,
			Expression<Func<TModel, OrderDirection>> orderDirectionExpression, Expression<Func<TModel, object>> expression, TableHeaderColumnOptions columnOptions)
		{
			StringBuilder headerColumns = new StringBuilder();
			columnOptions = columnOptions ?? new TableHeaderColumnOptions();

			ModelMetadata orderPropertyMetadata = ModelMetadata.FromLambdaExpression(orderPropertyExpression, htmlHelper.ViewData);
			ModelMetadata orderDirectionMetadata = ModelMetadata.FromLambdaExpression(orderDirectionExpression, htmlHelper.ViewData);

			foreach (var argument in (expression.Body as NewExpression).Arguments)
			{
				string expressionString = ExpressionHelper.GetExpressionText(Expression.Lambda(argument, expression.Parameters));
				ModelMetadata metadata = ModelMetadata.FromStringExpression(expressionString, htmlHelper.ViewData);

				headerColumns.Append(CreateColumn(formAction, metadata.PropertyName, metadata.DisplayName ?? metadata.PropertyName, orderPropertyMetadata.PropertyName, orderPropertyMetadata.Model.ToString(),
					orderDirectionMetadata.PropertyName, (OrderDirection)Enum.Parse(typeof(OrderDirection), orderDirectionMetadata.Model.ToString(), true), htmlHelper.ViewData.Model,
					columnOptions));
			}

			return new MvcHtmlString(headerColumns.ToString());
		}
	}
}
