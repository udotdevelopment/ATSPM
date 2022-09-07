using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class SpmwatchDogErrorEvent
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string SignalId { get; set; }
        public string DetectorId { get; set; }
        public string Direction { get; set; }
        public int Phase { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
