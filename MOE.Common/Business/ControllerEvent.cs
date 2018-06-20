using System;

namespace MOE.Common.Business
{
    public class ControllerEvent : object
    {
        /// <summary>
        ///     Generic Timestamp/eventCode event.
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="eventcode"></param>
        public ControllerEvent(DateTime timestamp, int eventcode)
        {
            EventCode = eventcode;
            TimeStamp = timestamp;
        }


        /// <summary>
        ///     Alternate Constructor that can handle all four peices of a controller event
        /// </summary>
        /// <param name="signalId"></param>
        /// <param name="timeStamp"></param>
        /// <param name="eventCode"></param>
        /// <param name="eventParam"></param>
        public ControllerEvent(string SignalID, DateTime TimeStamp, int EventCode, int EventParam)
        {
            this.SignalID = SignalID;
            this.TimeStamp = TimeStamp;
            this.EventCode = EventCode;
            this.EventParam = EventParam;
        }

        public DateTime TimeStamp { get; set; }

        public string SignalID { get; }

        public int EventCode { get; }

        public int EventParam { get; }


        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            var y = (ControllerEvent) obj;
            return this != null && y != null && SignalID == y.SignalID && TimeStamp == y.TimeStamp
                   && EventCode == y.EventCode && EventParam == y.EventParam
                ;
        }


        public override int GetHashCode()
        {
            return this == null
                ? 0
                : SignalID.GetHashCode() ^ TimeStamp.GetHashCode() ^ EventCode.GetHashCode() ^ EventParam.GetHashCode();
        }
    }
}