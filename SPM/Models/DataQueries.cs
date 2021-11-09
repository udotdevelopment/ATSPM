using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace SPM.Models
{
    /// <summary>
    /// Class to encapsulate raw events query details. 
    /// </summary>
    public class QueryDetails
    {
        private string _orderBy { get; set; }
        public string OrderBy
        {
            get => _orderBy;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (value[0] == '-')
                    {
                        DecesendingOrder = true;
                        value = value.Substring(1);
                    }
                    _orderBy = CapitalizeFirstLetter(value);
                }
                else
                {
                    _orderBy = "";
                }

            }
        }

        public string Filter { get; set; }

        public bool DecesendingOrder { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public bool IgnorePaging { get; set; }

        public string Id { get; set; }

        public int VersionId { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string FileType { get; set; }

        public int BinSize { get; set; }

        public int TypeId { get; set; }

        public List<DayOfWeek?> DaysOfWeek { get; set; } = new List<DayOfWeek?>();

        private static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return input.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture) +
                   input.Substring(1, input.Length - 1);
        }

        public QueryDetails()
        {
            PageSize = 0;
            PageIndex = 0;
            IgnorePaging = false;
        }
    }

}