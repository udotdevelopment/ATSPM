using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.ViewModel
{
    public class LinkPivotAdjustment
    {
        public int LinkNumber { get; set; }
        public string SignalId { get; set; }
        public string Location { get; set; }
        public int Delta { get; set; }
        public int Adjustment { get; set; }

        public LinkPivotAdjustment(int linkNumber, string signalId, string location, int delta, int adjustment)
        {
            LinkNumber = linkNumber;
            SignalId = signalId;
            Location = location;
            Delta = delta;
            Adjustment = adjustment;
        }
    }
}
