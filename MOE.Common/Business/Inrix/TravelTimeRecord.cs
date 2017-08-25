using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOE.Common.Business.Inrix
{
    public class TravelTimeRecord
    {
        protected DateTime timeStamp;
        public DateTime TimeStamp
        {
            get
            {
                return timeStamp;
            }
        }



        protected double travelTime;
        public double TravelTime
        {
            get
            {
                return travelTime;
            }
        }

        protected int confidence;
        public int Confidence
        {
            get
            {
                return confidence;
            }
        }

        public TravelTimeRecord(DateTime timestamp, double traveltime, int _confidence)
        {
            timeStamp = timestamp;
            travelTime = traveltime;
            confidence = _confidence;
        }
    }
}


