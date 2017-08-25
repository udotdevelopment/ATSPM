using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
    public class RLMDetectorDataPoint
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

        
        
        
        /// <summary>
        /// Constructor for the DetectorDataPoint. Sets the timestamp
        /// and the y coordinate.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="eventTime"></param>
        public RLMDetectorDataPoint(DateTime startDate, DateTime eventTime)
        {
            timeStamp = eventTime;
            yPoint = (eventTime - startDate).TotalSeconds;
            
        }
    }
}
