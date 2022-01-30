using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class ApproachSpeedAggregation
    {
        public DateTime BinStartTime { get; set; }
        public string SignalId { get; set; }
        public int ApproachId { get; set; }
        public int SummedSpeed { get; set; }
        public int SpeedVolume { get; set; }
        public int Speed85th { get; set; }
        public int Speed15th { get; set; }
    }
}
