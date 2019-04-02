using System;

namespace MOE.Common.Business.Inrix
{
    public class TravelTimeRecord
    {
        protected int confidence;
        protected DateTime timeStamp;


        protected double travelTime;

        public TravelTimeRecord(DateTime timestamp, double traveltime, int _confidence)
        {
            timeStamp = timestamp;
            travelTime = traveltime;
            confidence = _confidence;
        }

        public DateTime TimeStamp => timeStamp;

        public double TravelTime => travelTime;

        public int Confidence => confidence;
    }
}