using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.SplitFail
{
    public class SplitFailDetectorActivation

    {
        public enum StatusType { Valid, NotValid };

        

        public DateTime DetectorOn
        {
            get;
            set;
        }

        public DateTime DetectorOff
        {
            get;
            set;
        }

        
        public double Duration
        {
            get
            {
                if (DetectorOff != null && DetectorOn != null)
                {
                    return (DetectorOff - DetectorOn).TotalMilliseconds;
                }
                else
                {
                    return 0;
                }
            }
        }


    }
}
