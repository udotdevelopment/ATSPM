using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.Preempt
{
    public class PreemptCycle
    {
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
        public List<DateTime> InputOn;
        public List<DateTime> InputOff;
        public List<DateTime> OtherPreemptStart;

        public PreemptCycle()
        {
            InputOn = new List<DateTime>();
            InputOff = new List<DateTime>();
            OtherPreemptStart = new List<DateTime>();
        }

        public Double Delay
        {
            get
            {
                if(HasDelay && EntryStarted > DateTime.MinValue && CycleStart > DateTime.MinValue && EntryStarted > CycleStart)
                {
                    return (EntryStarted - CycleStart).TotalSeconds;
                }
                                
                else
                {
                    return 0;
                }
            }
        }

      
        public Double TimeToService{
            get{

                if (BeginTrackClearance > DateTime.MinValue && CycleStart > DateTime.MinValue && BeginTrackClearance >= CycleStart)
                {
                    if (HasDelay)
                    {
                        return (BeginTrackClearance - EntryStarted).TotalSeconds;
                    }
                    else
                    {
                          return (BeginTrackClearance - CycleStart).TotalSeconds;
                    }
                }

                else if (BeginDwellService > DateTime.MinValue && CycleStart > DateTime.MinValue && BeginDwellService >= CycleStart)
                {
                      if (HasDelay)
                    {
                        return (BeginDwellService - EntryStarted).TotalSeconds;
                      }
                      else
                      {
                    return (BeginDwellService - CycleStart).TotalSeconds;
                      }
                }

                else
                {
                    return 0;
                }

               }
        }

        public Double DwellTime
        {
            get
            {
                if (CycleEnd > DateTime.MinValue && BeginDwellService > DateTime.MinValue && CycleEnd >= BeginDwellService)
                {
                    return (CycleEnd - BeginDwellService).TotalSeconds;
                }
                else
                {
                    return 0;
                }
            }
        }
        




        public Double TimeToCallMaxOut
        {
            get
            {

                if (CycleStart > DateTime.MinValue && MaxPresenceExceeded > DateTime.MinValue && MaxPresenceExceeded > CycleStart)
                {
                    return (MaxPresenceExceeded - CycleStart).TotalSeconds;
                }
                else
                {
                    return 0;
                }
            }
        }


        public Double TimeToEndOfEntryDelay
        {
            get
            {

                if (CycleStart > DateTime.MinValue && EntryStarted > DateTime.MinValue && EntryStarted > CycleStart)
                {
                    return (EntryStarted - CycleStart).TotalSeconds;
                }
                else
                {
                    return 0;
                }
            }
        }

        public Double TimeToTrackClear
        {
            get
            {

                if (BeginDwellService > DateTime.MinValue && BeginTrackClearance > DateTime.MinValue &&  BeginDwellService > BeginTrackClearance)
                {
                    return (BeginDwellService - BeginTrackClearance).TotalSeconds;
                }
                else
                {
                    return 0;
                }
            }
        }

        public Double TimeToGateDown
        {
            get
            {

                if (CycleStart > DateTime.MinValue && GateDown > DateTime.MinValue && GateDown > CycleStart)
                {
                    return (GateDown - CycleStart).TotalSeconds;
                }
                else
                {
                    return 0;
                }
            }
        }
        
    }
}
