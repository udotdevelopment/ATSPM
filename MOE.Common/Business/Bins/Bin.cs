using System;

namespace MOE.Common.Business.Bins
{
    public class Bin
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public double Sum { get; set; } = 0;
        public double Average { get; set; } = 0;
    }
}