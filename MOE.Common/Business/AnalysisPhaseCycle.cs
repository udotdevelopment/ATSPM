using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
    public class AnalysisPhaseCycle
    {
        public enum NextEventResponse{CycleOK, CycleMissingData, CycleComplete};
        public enum TerminationType : int { 
            GapOut = 4,
            MaxOut = 5,
            ForceOff = 6, 
            Unknown = 0};

        private int phaseNumber;
        public int PhaseNumber
        {
            get
            {
                return phaseNumber;
            }
        }

        private string signalId;
        public string SignalId
        {
            get
            {
                return signalId;
            }
        }

        private DateTime startTime;
        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
        }

        private DateTime endTime;
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
        }

        private DateTime pedStartTime;
        public DateTime PedStartTime
        {
            get
            {
                return pedStartTime;
            }
        }

        private DateTime pedEndTime;
        public DateTime PedEndTime
        {
            get
            {
                return pedEndTime;
            }
        }

        private int terminationEvent;
        public int TerminationEvent
        {
            get
            {
                return terminationEvent;
            }
        }

        private TimeSpan duration;
        public TimeSpan Duration
        {
            get
            {
                return duration;
            }
        }

        private double pedDuration;
        public double PedDuration
        {
            get
            {
                if (pedDuration > 0)
                {
                    return pedDuration;
                }
                else
                {
                    return 0;
                }
            }
        }



        public bool HasPed { get; set; }

        public DateTime YellowEvent { get; set; }

        /// <summary>
        /// Phase Objects primarily for the split monitor and terminaiton chart
        /// </summary>
        /// <param name="signalid"></param>
        /// <param name="phasenumber"></param>
        /// <param name="starttime"></param>
        public AnalysisPhaseCycle(string signalid, int phasenumber, DateTime starttime)
        {
            signalId = signalid;
            phaseNumber = phasenumber;
            startTime = starttime;
            HasPed = false;
            terminationEvent = 0;

        }

        public void SetTerminationEvent(int terminatonCode)
        {
            terminationEvent = terminatonCode;
        }

        public void SetEndTime(DateTime endtime)
        {
            endTime = endtime;
            duration = endTime.Subtract(startTime);
        }

        public void SetPedStart(DateTime starttime)
        {
            pedStartTime = starttime;
            HasPed = true;
        }

        public void SetPedEnd(DateTime endtime)
        {
            pedEndTime = endtime;
            pedDuration = pedEndTime.Subtract(pedStartTime).TotalSeconds;
        }

    }
}