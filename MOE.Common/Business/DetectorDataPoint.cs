using System;

namespace MOE.Common.Business
{
    public enum ArrivalType
    {
        ArrivalOnGreen,
        ArrivalOnYellow,
        ArrivalOnRed
    }

    public class DetectorDataPoint
    {
        public DetectorDataPoint(DateTime startDate, DateTime eventTime, DateTime greenEvent, DateTime yellowEvent)
        {
            TimeStamp = eventTime;
            YPoint = (eventTime - startDate).TotalSeconds;
            //if the detector hit is before greenEvent
            if (eventTime < greenEvent)
            {
                Delay = (greenEvent - eventTime).TotalSeconds;
                ArrivalType = ArrivalType.ArrivalOnRed;
            }
            //if the detector hit is After green, but before yellow
            else if (eventTime >= greenEvent && eventTime < yellowEvent)
            {
                Delay = 0;
                ArrivalType = ArrivalType.ArrivalOnGreen;
            }
            //if the event time is after yellow
            else if (eventTime >= yellowEvent)
            {
                Delay = 0;
                ArrivalType = ArrivalType.ArrivalOnYellow;
            }
        }

        //Represents a time span from the start of the red to red cycle
        public double YPoint { get; }

        //The actual time of the detector activation
        public DateTime TimeStamp { get; }

        public double Delay { get; }

        public ArrivalType ArrivalType { get; }
    }
}