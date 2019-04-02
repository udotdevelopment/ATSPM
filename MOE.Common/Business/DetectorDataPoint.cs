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
            StartOfCycle = startDate;
            YPoint = (TimeStamp - StartOfCycle).TotalSeconds;
            YellowEvent = yellowEvent;
            GreenEvent = greenEvent;
            SetDataPointProperties();
        }

        private void SetDataPointProperties()
        {
            //if the detector hit is before greenEvent
            if (TimeStamp < GreenEvent)
            {
                Delay = (GreenEvent - TimeStamp).TotalSeconds;
                ArrivalType = ArrivalType.ArrivalOnRed;
            }
            //if the detector hit is After green, but before yellow
            else if (TimeStamp >= GreenEvent && TimeStamp < YellowEvent)
            {
                Delay = 0;
                ArrivalType = ArrivalType.ArrivalOnGreen;
            }
            //if the event time is after yellow
            else if (TimeStamp >= YellowEvent)
            {
                Delay = 0;
                ArrivalType = ArrivalType.ArrivalOnYellow;
            }
        }

        //Represents a time span from the start of the red to red cycle
        public double YPoint { get; private set; }

        public DateTime StartOfCycle { get;}

        //The actual time of the detector activation
        public DateTime TimeStamp { get; private set; }

        public DateTime YellowEvent { get;}

        public DateTime GreenEvent { get; }

        public double Delay { get; set; }

        public ArrivalType ArrivalType { get; set; }

        public void AddSeconds(int seconds)
        {
            TimeStamp = TimeStamp.AddSeconds(seconds);
            YPoint = (TimeStamp - StartOfCycle).TotalSeconds;
            SetDataPointProperties();
        }
    }
}