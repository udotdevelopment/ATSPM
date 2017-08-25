using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
    public class ControllerEvent : object
    {

        private DateTime timeStamp;
        public DateTime TimeStamp
        {
            get 
            { 
                return timeStamp; 
            }
            set 
            {
                timeStamp = value;
            }
        }
    

        private string signalid;
        public string SignalID
        {
            get { return signalid; }
        }

        private int eventCode;
        public int EventCode
        {
            get { return eventCode; }
        }

        private int eventParam;
        public int EventParam
        {
            get { return eventParam; }
        }

        /// <summary>
        /// Generic Timestamp/eventCode event.
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="eventcode"></param>
        public ControllerEvent(DateTime timestamp, int eventcode)
        {
            eventCode = eventcode;
            timeStamp = timestamp;

        }

        
        /// <summary>
        /// Alternate Constructor that can handle all four peices of a controller event
        /// </summary>
        /// <param name="signalId"></param>
        /// <param name="timeStamp"></param>
        /// <param name="eventCode"></param>
        /// <param name="eventParam"></param>
        public ControllerEvent(string SignalID, DateTime TimeStamp, int EventCode, int EventParam)
        {

            signalid = SignalID;
            timeStamp = TimeStamp;
            eventCode = EventCode;
            eventParam = EventParam;
        }



        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            ControllerEvent y = (ControllerEvent)obj;
            return this != null && y != null && this.SignalID == y.SignalID && this.TimeStamp == y.TimeStamp
                && this.EventCode == y.EventCode && this.EventParam == y.EventParam
                ;
        }



        public override int GetHashCode()
        {
            return this == null ? 0 : (this.SignalID.GetHashCode() ^ this.TimeStamp.GetHashCode() ^ this.EventCode.GetHashCode() ^ this.EventParam.GetHashCode());
        }
    }


    }
