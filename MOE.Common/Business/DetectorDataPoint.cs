using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
    public class DetectorDataPoint
    {
        //Represents a time span from the start of the red to red cycle
        private double yPoint;
        public double YPoint
        {
            get
            {
                return yPoint;
            }
        }

        //The actual time of the detector activation
        private DateTime timeStamp;
        public DateTime TimeStamp
        {
            get 
            {
                return timeStamp;
            }
        }

        private double delay;
        public double Delay
        {
            get
            {
                return delay;
            }
            
        }

        private bool arrivalOnGreen;
        public bool ArrivalOnGreen
        {
            get
            {
                return arrivalOnGreen;
            }

        }

        
        
        /// <summary>
        /// Constructor for the DetectorDataPoint. Sets the timestamp
        /// and the y coordinate.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="eventTime"></param>
        public DetectorDataPoint(DateTime startDate, DateTime eventTime, DateTime greenEvent, DateTime redEvent)
        {
            timeStamp = eventTime;
            yPoint = (eventTime - startDate).TotalSeconds;
            if (eventTime < greenEvent)
            {
                delay = (greenEvent - eventTime).TotalSeconds;
                arrivalOnGreen = false;
            }
            else
            {
                delay = 0;
                arrivalOnGreen = true;
            }
        }
    }
}
