using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtspmApi.Models
{
    public class EventLogFromSignalRequest
    {
        public string SignalId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}