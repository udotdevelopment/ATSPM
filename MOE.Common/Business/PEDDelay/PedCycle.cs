using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.PEDDelay
{
    public class PedCycle
    {
        private DateTime _BeginWalk;

        public DateTime BeginWalk
        {
            get { return _BeginWalk; }
        }

        private DateTime _CallRegistered;

        public DateTime CallRegistered
        {
            get { return _CallRegistered; }
        }

        public double Delay
        {
            get { return Math.Abs((BeginWalk - CallRegistered).TotalSeconds);}
        }

        public PedCycle(DateTime beginWalk, DateTime callRegistered)
        {
            _BeginWalk = beginWalk;
            _CallRegistered = callRegistered;
        }
    }
}
