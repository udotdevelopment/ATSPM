using System;

namespace MOE.Common.Business
{
    public class AnalysisPhaseCycle
    {
        public enum NextEventResponse
        {
            CycleOK,
            CycleMissingData,
            CycleComplete
        }

        public enum TerminationType
        {
            GapOut = 4,
            MaxOut = 5,
            ForceOff = 6,
            Unknown = 0
        }

        private double pedDuration;

        /// <summary>
        ///     Phase Objects primarily for the split monitor and terminaiton chart
        /// </summary>
        /// <param name="signalid"></param>
        /// <param name="phasenumber"></param>
        /// <param name="starttime"></param>
        public AnalysisPhaseCycle(string signalid, int phasenumber, DateTime starttime)
        {
            SignalId = signalid;
            PhaseNumber = phasenumber;
            StartTime = starttime;
            HasPed = false;
            TerminationEvent = 0;
        }

        public int PhaseNumber { get; }

        public string SignalId { get; }

        public DateTime StartTime { get; }

        public DateTime EndTime { get; private set; }

        public DateTime PedStartTime { get; private set; }

        public DateTime PedEndTime { get; private set; }

        public int TerminationEvent { get; private set; }

        public TimeSpan Duration { get; private set; }

        public double PedDuration
        {
            get
            {
                if (pedDuration > 0)
                    return pedDuration;
                return 0;
            }
        }


        public bool HasPed { get; set; }

        public DateTime YellowEvent { get; set; }

        public void SetTerminationEvent(int terminatonCode)
        {
            TerminationEvent = terminatonCode;
        }

        public void SetEndTime(DateTime endtime)
        {
            EndTime = endtime;
            Duration = EndTime.Subtract(StartTime);
        }

        public void SetPedStart(DateTime starttime)
        {
            PedStartTime = starttime;
            HasPed = true;
        }

        public void SetPedEnd(DateTime endtime)
        {
            PedEndTime = endtime;
            pedDuration = PedEndTime.Subtract(PedStartTime).TotalSeconds;
        }
    }
}