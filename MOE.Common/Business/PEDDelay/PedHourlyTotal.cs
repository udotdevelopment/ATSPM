using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.PEDDelay
{
    public class PedHourlyTotal
    {
        private DateTime _Hour;

        public DateTime Hour
        {
            get { return _Hour; }
        }

        private double _DelaySeconds;

        public double Delay
        {
            get { return _DelaySeconds; }
        }
        
        public PedHourlyTotal(DateTime hour, double delaySeconds)
        {
            _Hour = hour;
            _DelaySeconds = delaySeconds;
        }
        
    }
}
