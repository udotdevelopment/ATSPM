using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
    public enum ArrivalType { ArrivalOnGreen, ArrivalOnYellow, ArrivalOnRed }
    public class DetectorDataPoint
    {
        //Represents a time span from the start of the red to red cycle
        public double YPoint { get; }

        //The actual time of the detector activation
        public DateTime TimeStamp { get; }

        public double Delay { get; }

        public ArrivalType ArrivalType { get; }
        
        public DetectorDataPoint(DateTime startDate, DateTime eventTime, DateTime greenEvent, DateTime yellowEvent)
        {
            TimeStamp = eventTime;
            YPoint = (eventTime - startDate).TotalSeconds;
            if (eventTime < greenEvent)
            {
                Delay = (greenEvent - eventTime).TotalSeconds;
                ArrivalType = ArrivalType.ArrivalOnRed;
            }
            else if (eventTime >= greenEvent && eventTime < yellowEvent)
            {
                Delay = 0;
                ArrivalType = ArrivalType.ArrivalOnGreen;
            }
            else if(eventTime >= yellowEvent)
            {
                Delay = 0;
                ArrivalType = ArrivalType.ArrivalOnYellow;
            }
        }
    }
}
