using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Business.Bins
{
    public class BinsContainer
    {
        public BinsContainer(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public List<Bin> Bins { get; set; } = new List<Bin>();

        public double SumValue
        {
            get { return Bins.Sum(b => b.Sum); }
        }

        public int AverageValue
        {
            get { return Convert.ToInt32(Math.Round(Bins.Average(b => b.Sum))); }
        }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}