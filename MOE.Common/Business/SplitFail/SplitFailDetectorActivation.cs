using System;

namespace MOE.Common.Business.SplitFail
{
    public class SplitFailDetectorActivation

    {
        public DateTime DetectorOn { get; set; }
        public DateTime DetectorOff { get; set; }
        public bool ReviewedForOverlap { get; set; } = false;

        public double DurationInMilliseconds
        {
            get
            {
                if (DetectorOff != null && DetectorOn != null)
                    return (DetectorOff - DetectorOn).TotalMilliseconds;
                return 0;
            }
        }
    }
}