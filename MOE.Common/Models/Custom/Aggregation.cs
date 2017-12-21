using System;

namespace MOE.Common.Models
{
    public abstract class Aggregation
    {
        public abstract int Id { get; set; }
        public abstract DateTime BinStartTime { get; set; }
    }
}