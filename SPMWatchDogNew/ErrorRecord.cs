using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMWatchDogNew
{
    public class ErrorRecord
    {
        public string SignalID;
        public string DetectorID;
        public string PrimaryName;
        public string SecondaryName;
        public int Phase;
        public int Channel;
        public string Direction;
        public int ErrorCode;
        public string ErrorMessage;
       

        public ErrorRecord(string signalid, string primaryname, string secondayyname, string detectorid, 
            int phase, int channel, string direction, int errorCode, string errorMessage)
        {
            SignalID = signalid;
            PrimaryName = primaryname;
            SecondaryName = secondayyname;
            DetectorID = detectorid;
            Phase = phase;
            Channel = channel;
            Direction = direction;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public ErrorRecord(List<MOE.Common.Business.Signal> signalsNoRecords)
        {
           
            
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            ErrorRecord y = (ErrorRecord)obj;
            return this != null && y != null && this.SignalID == y.SignalID && this.DetectorID == y.DetectorID 
                && this.Phase == y.Phase && this.Channel == y.Channel && this.ErrorCode == y.ErrorCode;
        }

        public override int GetHashCode()
        {
            return this == null ? 0 : (this.SignalID.GetHashCode() ^ this.DetectorID.GetHashCode() ^ this.Phase.GetHashCode() ^ this.Channel.GetHashCode());
        }

    }



}
