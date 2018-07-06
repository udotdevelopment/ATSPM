using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace MOE.Common.Business
{
    public class PhaseCycleBase
    {
        public enum TerminationType : int
        {
            GapOut = 4,
            MaxOut = 5,
            ForceOff = 6,
            Unknown = 0
        };

        public enum CycleStartCause
        {
            GreenToGreen,
            RedToRed,
            YellowToYellow
        }

        public enum NextEventResponse { CycleOk, CycleMissingData, CycleComplete };
       
        public enum EventType { ChangeToRed, ChangeToGreen, ChangeToYellow, GreenTermination, BeginYellowClearance, EndYellowClearance, Unknown };

        public int PhaseNumber { get; set; }
        public string SignalId { get; set; }
        public CycleStartCause CycleStartEvent { get; set; }
        public TerminationType TerminationEvent { get; set; }
        public DateTime CycleStart { get; set; }
        public DateTime CycleEnd { get; set; }
        public DateTime GreenStart { get; set; }
        public DateTime RedStart { get; set; }
        public DateTime RedClearStart { get; set; }
        public DateTime YellowStart { get; set; }
        public DateTime PedStart { get; set; }
        public DateTime PedEnd { get; set; }

        public bool HasPed { get; set; }

        public List<Models.Speed_Events> SpeedsForCycle;
        public List<DetectorDataPoint> DetectorEvents { get; set; }
        public List<DetectorDataPoint> PreemptCollection { get; set; }

        public double PedDuration => GetEventLength(PedStart, PedEnd);

        public double GreenTime => GetEventLength(GreenStart, YellowStart);

        public double TotalTime => GetEventLength(GreenStart, CycleEnd);

        public double TotalArrivalOnRed => DetectorEvents.Count(d => d.ArrivalType == ArrivalType.ArrivalOnRed);

        public double TotalArrivalOnYellow => DetectorEvents.Count(d => d.ArrivalType == ArrivalType.ArrivalOnYellow);

        public double TotalArrivalOnGreen => DetectorEvents.Count(d => d.ArrivalType == ArrivalType.ArrivalOnGreen);

        public SplitFail.SplitFailDetectorActivationCollection Activations = new SplitFail.SplitFailDetectorActivationCollection();


        /// <summary>
        /// Red Clear + Red
        /// </summary>
        public double TotalRedTime => GetEventLength(RedClearStart, CycleEnd); 
        



    public double RedClearTime => GetEventLength(RedClearStart, RedStart);
    public double YellowTime => GetEventLength(YellowStart, RedStart);


        public double RedTime
        {
            get
            {
                if (CycleStartEvent == CycleStartCause.GreenToGreen)
                {
                    return GetEventLength(RedClearStart, CycleEnd);
                }

                    return GetEventLength(RedClearStart, GreenStart);
                

            }

        }

        



        public double GetEventLength(DateTime firstEvent, DateTime secondEvent)
        {
            if((firstEvent != null && secondEvent != null) && (secondEvent > firstEvent))
            {
                return (secondEvent - firstEvent).TotalSeconds;
            }
            return 0;

        }

        public double TotalDelay => DetectorEvents.Sum(d => d.Delay);



        public double TotalVolume => DetectorEvents.Count;

        public void FindSpeedEventsForCycle(List<Models.Speed_Events> speeds)
        {
            SpeedsForCycle = (from r in speeds
                where r.timestamp > this.CycleStart
                      && r.timestamp < this.CycleEnd
                select r).ToList();
        }

        public void SetTerminationEvent(int eventCode)
        {
            if (Enum.IsDefined(typeof(TerminationType), eventCode))
            {
                this.TerminationEvent = (TerminationType) eventCode;
            }
        }


    }
    }
