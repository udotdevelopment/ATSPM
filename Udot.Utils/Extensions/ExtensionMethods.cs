using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Udot.Utils.Extensions
{
    public static class DefaultValues
    {
        public const int NO_VALUE = -1;
        public const string EMPTY_STR = "";

    }
    public class SemiNumericComparer : IComparer<object>
    {
        public int Compare(object s1, object s2)
        {
            if (IsNumeric(s1) && IsNumeric(s2))
            {
                if (s1.ToInt() > s2.ToInt()) return 1;
                if (s1.ToInt() < s2.ToInt()) return -1;
                if (s1.ToInt() == s2.ToInt()) return 0;
            }

            if (IsNumeric(s1) && !IsNumeric(s2))
                return -1;

            if (!IsNumeric(s1) && IsNumeric(s2))
                return 1;

            var s1Str = Convert.ToString(s1);
            var s2Str = Convert.ToString(s2);
            return string.Compare(s1Str, s2Str, true);
        }

        public static bool IsNumeric(object value)
        {
            try
            {
                int i = Convert.ToInt32(value.ToString());
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }

    public static class ExtensionMethods
    {
        public static int ToInt(this object input, int defaultInt = DefaultValues.NO_VALUE)
        {
            int resultNum = defaultInt;
            if (input != null)
                resultNum = Convert.ToInt32(input);
            return resultNum;
        }

        /// <summary>
        /// Custom Order by with direction. Will decide to use OrderBy or OrderByDescending based on descending bool passed in
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="descending"></param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> OrderByWithDirection<TSource>
            (this IQueryable<TSource> source,
             Expression<Func<TSource, object>> keySelector,
             bool descending)
        {
            return descending ? source.OrderByDescending(keySelector, new SemiNumericComparer())
                              : source.OrderBy(keySelector, new SemiNumericComparer());
        }
    }
}