using System;
using System.Collections.Generic;

namespace MOE.Common.Business.Preempt
{
    public class PreemptCycle
    {
        public List<DateTime> InputOff;
        public List<DateTime> InputOn;
        public List<DateTime> OtherPreemptStart;

        public PreemptCycle()
        {
            InputOn = new List<DateTime>();
            InputOff = new List<DateTime>();
            OtherPreemptStart = new List<DateTime>();
        }
        // public enum CycleState { InputOn, GateDown, InputOff, BeginTrackClearance, EntryStarted  };

        public DateTime WarningInput { get; set; }
        public DateTime StartInputOn { get; set; }
        public DateTime CycleStart { get; set; }
        public DateTime CycleEnd { get; set; }
        public DateTime GateDown { get; set; }
        public DateTime EntryStarted { get; set; }
        public DateTime BeginTrackClearance { get; set; }
        public DateTime BeginDwellService { get; set; }
        public DateTime LinkActive { get; set; }
        public DateTime LinkInactive { get; set; }
        public DateTime MaxPresenceExceeded { get; set; }
        public DateTime BeginExitInterval { get; set; }
        public DateTime TSPCheckIn { get; set; }
        public DateTime TSPAdjustToEarlyGreen { get; set; }
        public DateTime TSPAdjustToExtendGreen { get; set; }
        public DateTime TSPCheckout { get; set; }

        public bool HasDelay { get; set; }

        public double Delay
        {
            get
            {
                if (HasDelay && EntryStarted > DateTime.MinValue && CycleStart > DateTime.MinValue &&
                    EntryStarted > CycleStart)
                    return (EntryStarted - CycleStart).TotalSeconds;

                return 0;
            }
        }


        public double TimeToService
        {
            get
            {
                if (BeginTrackClearance > DateTime.MinValue && CycleStart > DateTime.MinValue &&
                    BeginTrackClearance >= CycleStart)
                {
                    if (HasDelay)
                        return (BeginTrackClearance - EntryStarted).TotalSeconds;
                    return (BeginTrackClearance - CycleStart).TotalSeconds;
                }

                if (BeginDwellService > DateTime.MinValue && CycleStart > DateTime.MinValue &&
                    BeginDwellService >= CycleStart)
                {
                    if (HasDelay)
                        return (BeginDwellService - EntryStarted).TotalSeconds;
                    return (BeginDwellService - CycleStart).TotalSeconds;
                }

                return 0;
            }
        }

        public double DwellTime
        {
            get
            {
                if (CycleEnd > DateTime.MinValue && BeginDwellService > DateTime.MinValue &&
                    CycleEnd >= BeginDwellService)
                    return (CycleEnd - BeginDwellService).TotalSeconds;
                return 0;
            }
        }


        public double TimeToCallMaxOut
        {
            get
            {
                if (CycleStart > DateTime.MinValue && MaxPresenceExceeded > DateTime.MinValue &&
                    MaxPresenceExceeded > CycleStart)
                    return (MaxPresenceExceeded - CycleStart).TotalSeconds;
                return 0;
            }
        }


        public double TimeToEndOfEntryDelay
        {
            get
            {
                if (CycleStart > DateTime.MinValue && EntryStarted > DateTime.MinValue && EntryStarted > CycleStart)
                    return (EntryStarted - CycleStart).TotalSeconds;
                return 0;
            }
        }

        public double TimeToTrackClear
        {
            get
            {
                if (BeginDwellService > DateTime.MinValue && BeginTrackClearance > DateTime.MinValue &&
                    BeginDwellService > BeginTrackClearance)
                    return (BeginDwellService - BeginTrackClearance).TotalSeconds;
                return 0;
            }
        }

        public double TimeToGateDown
        {
            get
            {
                if (CycleStart > DateTime.MinValue && GateDown > DateTime.MinValue && GateDown > CycleStart)
                    return (GateDown - CycleStart).TotalSeconds;
                return 0;
            }
        }
    }
}