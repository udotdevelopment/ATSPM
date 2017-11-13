using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using MOE.Common.Models;


namespace MOE.Common.Business
{
    public class SignalPhase
    {
        public string Direction { get; }
        public VolumeCollection Volume { get; private set; }
        public List<MOE.Common.Business.Plan> Plans { get; private set; }
        public List<PhaseCycleBase> Cycles { get; set; }
        public List<Models.Controller_Event_Log> DetectorEvents { get; set; }
        public bool IsPermissive { get; }
        public Models.Approach Approach { get; }

        public double AvgDelay
        {
            get
            {
                return TotalDelay / TotalVolume;
            }
        }


        /// <summary>
        /// A calculation to get the percent of activations on green
        /// </summary>
        public double PercentArrivalOnGreen
        {
            get
            {
                if (TotalVolume > 0)
                {
                    return Math.Round(((TotalArrivalOnGreen / TotalVolume) * 100));
                }
                return 0;
            }
        }

        public double PercentGreen
        {
            get
            {
                if (TotalTime > 0)
                {
                    return Math.Round(((TotalGreenTime / TotalTime) * 100));
                }
                return 0;
            }
        }

        public double PlatoonRatio
        {
            get
            {
                if (TotalVolume > 0)
                {
                    return Math.Round((PercentArrivalOnGreen / PercentGreen), 2);
                }
                return 0;
            }
        }

        private double totalArrivalOnGreen = -1;
        public double TotalArrivalOnGreen
        {
            get
            {
                if (totalArrivalOnGreen == -1)
                    totalArrivalOnGreen = Cycles.Sum(d => d.TotalArrivalOnGreen);
                return totalArrivalOnGreen;
            }
        }
        

        private double totalArrivalOnRed = -1;
        public double TotalArrivalOnRed
        {
            get { 
                if(totalArrivalOnRed == -1)
                    totalArrivalOnRed = Cycles.Sum(d => d.TotalArrivalOnRed);
                return totalArrivalOnRed;
            }
        }

        private double totalArrivalOnYellow = -1;
        public double TotalArrivalOnYellow
        {
            get
            {
                if (totalArrivalOnYellow == -1)
                    totalArrivalOnYellow = Cycles.Sum(d => d.TotalArrivalOnYellow);
                return totalArrivalOnYellow;
            }
        }
        public double TotalDelay
        {
            get
            {
                return Cycles.Sum(d => d.TotalDelay);
            }
        }

        private double totalVolume = -1;
        public double TotalVolume
        {
            get
            {
                if(totalVolume == -1)
                    totalVolume = Cycles.Sum(d=> d.TotalVolume);
                return totalVolume;
            }

        }

        private double totalGreenTime = -1;
        public double TotalGreenTime
        {
            get
            {
                if(totalGreenTime == -1)
                {
                    totalGreenTime = Cycles.Sum(d => d.GreenTime);
                }
                return totalGreenTime;
            }
        }

        private double totalYellowTime = -1;
        public double TotalYellowTime
        {
            get
            {
                if (totalYellowTime == -1)
                {
                    totalYellowTime = Cycles.Sum(d => d.YellowTime);
                }
                return totalYellowTime;
            }
        }

        private double totalRedTime = -1;
        public double TotalRedTime
        {
            get
            {
                if (totalRedTime == -1)
                {
                    totalRedTime = Cycles.Sum(d => d.TotalRedTime);
                }
                return totalRedTime;
            }
        }

        public double TotalTime
        {
            get
            {
                return Cycles.Sum(d => d.TotalTime);
            }
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        
        public SignalPhase(DateTime startDate, DateTime endDate, Approach approach,
            bool showVolume, int binSize, int metricTypeId, bool isPermissive)
        {
            StartDate = startDate;
            EndDate = endDate;
            Approach = approach;
            Cycles = new List<PhaseCycleBase>();
            Plans = new List<Plan>();
            if (approach.DirectionType != null) this.Direction = approach.DirectionType.Description;
            IsPermissive = isPermissive;
            GetSignalPhaseData(showVolume, binSize, metricTypeId);
        }

        public void LinkPivotAddSeconds(int seconds)
        {
            totalArrivalOnRed = -1;
            totalVolume = -1;
            totalGreenTime = -1;
            totalArrivalOnGreen = -1;
            this.Volume = null;
            foreach (Controller_Event_Log row in DetectorEvents)
            {
                row.Timestamp = row.Timestamp.AddSeconds(seconds);
            }
            //Todo:Fix for Link Pivot
            //Plans.LinkPivotAddDetectorData(this.DetectorEvents);
        }

        
        private void GetSignalPhaseData(bool showVolume, int binSize, int metricTypeId)
        {
            GetDetectorEvents(metricTypeId);
            GetCycleEvents();
            GetPreemptEvents();
            GetPlans();
            if (showVolume)
            {
                SetVolume(DetectorEvents, binSize);
            }
        }

        private void GetPlans()
        {
            var celRepository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            List<Controller_Event_Log> planEvents = new List<Controller_Event_Log>();
            var firstPlanEvent = celRepository.GetFirstEventBeforeDate(Approach.SignalID, 131, StartDate);
            if (firstPlanEvent != null)
            {
                firstPlanEvent.Timestamp = StartDate;
                planEvents.Add(firstPlanEvent);
            }
            else
            {
                firstPlanEvent = new Controller_Event_Log();
                firstPlanEvent.Timestamp = StartDate;
                planEvents.Add(firstPlanEvent);
            }
            var tempPlanEvents = celRepository.GetSignalEventsByEventCode(Approach.SignalID, StartDate, EndDate, 131);
            if (tempPlanEvents != null)
            {
                planEvents.AddRange(tempPlanEvents.OrderBy(e => e.Timestamp).Distinct());
                AddPlans(planEvents);
            }
        }

        private void AddPlans(List<Controller_Event_Log> planEvents)
        {
            for (int i = 0; i < planEvents.Count; i++)
            {
                if (planEvents.Count - 1 == i)
                {
                    if (planEvents[i].Timestamp != EndDate)
                    {
                        var planCycles = Cycles
                            .Where(c => c.CycleStart >= planEvents[i].Timestamp && c.CycleStart < EndDate).ToList();
                        Plans.Add(new Plan(planEvents[i].Timestamp, EndDate, planEvents[i].EventParam, Approach, planCycles));
                    }
                }
                else
                {
                    if (planEvents[i].Timestamp != planEvents[i + 1].Timestamp)
                    {
                        var planCycles = Cycles
                            .Where(c => c.CycleStart >= planEvents[i].Timestamp && c.CycleStart < planEvents[i + 1].Timestamp).ToList();
                        Plans.Add(new Plan(planEvents[i].Timestamp, planEvents[i + 1].Timestamp, planEvents[i].EventParam, Approach, planCycles));
                    }
                }
            }
        }

        private void GetDetectorEvents(int metricTypeId)
        {
            var celRepository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            DetectorEvents = new List<Models.Controller_Event_Log>();
            var detectorsForMetric = Approach.GetDetectorsForMetricType(metricTypeId);
            foreach (Models.Detector d in detectorsForMetric)
            {
                DetectorEvents.AddRange(celRepository.GetEventsByEventCodesParamWithOffset(Approach.SignalID, StartDate,
                    EndDate, new List<int> { 81 }, d.DetChannel, d.GetOffset()));
            }
        }

        private void GetPreemptEvents()
        {
            var celRepository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            List<Controller_Event_Log> preemptEvents = celRepository.GetSignalEventsByEventCodes(Approach.SignalID, StartDate,
                            EndDate, new List<int>() { 102 });
            foreach (var preemptEvent in preemptEvents)
            {
                var cycle = Cycles.FirstOrDefault(c => c.CycleStart <= preemptEvent.Timestamp && c.CycleEnd > preemptEvent.Timestamp);
                cycle?.PreemptCollection.Add(new DetectorDataPoint(cycle.CycleStart, preemptEvent.Timestamp, cycle.GreenStart, cycle.YellowStart));
            }
        }

        private void GetCycleEvents()
        {
            List<Controller_Event_Log> cycleEvents = new List<Controller_Event_Log>();
            var celRepository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            if (IsPermissive)
            {
                if (Approach.PermissivePhaseNumber != null && Approach.PermissivePhaseNumber > 0)
                {
                    cycleEvents = celRepository.GetEventsByEventCodesParam(Approach.SignalID, StartDate,
                        EndDate, new List<int>() { 1, 8, 10 }, Approach.PermissivePhaseNumber.Value);
                }
            }
            else
            {
                var cycleEventNumbers = Approach.IsProtectedPhaseOverlap ? new List<int> {61, 63, 64} : new List<int>() {1, 8, 10};
                cycleEvents = celRepository.GetEventsByEventCodesParam(Approach.SignalID, StartDate,
                EndDate, cycleEventNumbers, Approach.ProtectedPhaseNumber);
            }
            AddCycles(cycleEvents);
        }

        private void AddCycles(List<Controller_Event_Log> cycleEvents)
        {
            for (int i = 0; i < cycleEvents.Count; i++)
            {
                if (i < cycleEvents.Count - 3
                    && GetEventType(cycleEvents[i].EventCode) == Business.PhaseCycleBase.EventType.ChangeToRed
                    && GetEventType(cycleEvents[i + 1].EventCode) == Business.PhaseCycleBase.EventType.ChangeToGreen
                    && GetEventType(cycleEvents[i + 2].EventCode) == Business.PhaseCycleBase.EventType.ChangeToYellow
                    && GetEventType(cycleEvents[i + 3].EventCode) == Business.PhaseCycleBase.EventType.ChangeToRed)
                {
                    Cycles.Add(new PhaseCycleBase
                    {
                        CycleStart = cycleEvents[i].Timestamp,
                        RedClearStart = cycleEvents[i].Timestamp,
                        GreenStart = cycleEvents[i + 1].Timestamp,
                        YellowStart = cycleEvents[i + 2].Timestamp,
                        CycleEnd = cycleEvents[i + 3].Timestamp
                    });

                    i = i + 3;
                }
            }
            if (Cycles != null)
            {
                foreach (var cycle in Cycles)
                {
                    var detectorEventsForCycle =
                        DetectorEvents.Where(d => d.Timestamp >= cycle.CycleStart && d.Timestamp < cycle.CycleEnd)
                            .ToList();
                    foreach (var controllerEventLog in detectorEventsForCycle)
                    {
                        cycle.DetectorEvents.Add(new DetectorDataPoint(cycle.CycleStart, controllerEventLog.Timestamp,
                            cycle.GreenStart, cycle.YellowStart));
                    }
                }
            }
        }

        private PhaseCycleBase.EventType GetEventType(int eventCode)
        {
            switch (eventCode)
            {
                case 1:
                    return PhaseCycleBase.EventType.ChangeToGreen;
                // overlap green
                case 61:
                    return PhaseCycleBase.EventType.ChangeToGreen;
                case 8:
                    return PhaseCycleBase.EventType.ChangeToYellow;
                // overlap yellow
                case 63:
                    return PhaseCycleBase.EventType.ChangeToYellow;
                case 10:
                    return PhaseCycleBase.EventType.ChangeToRed;
                // overlap red
                case 64:
                    return PhaseCycleBase.EventType.ChangeToRed;
                default:
                    return PhaseCycleBase.EventType.Unknown;
            }
        }

        private void SetVolume(List<Controller_Event_Log> detectorEvents, int binSize)
        {
            Volume = new VolumeCollection(StartDate, EndDate, detectorEvents, binSize);            
        }
    }
}
