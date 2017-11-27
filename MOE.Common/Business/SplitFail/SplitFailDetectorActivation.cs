using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.SplitFail
{
    public class SplitFailDetectorActivation

    {
        public DateTime DetectorOn{get;set;}
        public DateTime DetectorOff{get;set;}
        public bool ReviewedForOverlap { get; set; } = false;
        
        public double Duration
        {
            get
            {
                if (DetectorOff != null && DetectorOn != null)
                {
                    return (DetectorOff - DetectorOn).TotalMilliseconds;
                }
                return 0;
            }
        }


    }
}
