using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business
{
    public class RLMSignalPhase
    {
        private int _detChannel;
        private bool _showVolume;


        public RLMSignalPhase(DateTime startDate, DateTime endDate, int binSize, double severeRedLightViolationsSeconds,
            Approach approach, bool usePermissivePhase)
        {
            SevereRedLightViolationSeconds = severeRedLightViolationsSeconds;
            Approach = approach;
            GetPermissivePhase = usePermissivePhase;
            using (var db = new SPM())
            {
                if (usePermissivePhase)
                {
                    if (Approach.IsPermissivePhaseOverlap)
                    {
                        GetSignalOverlapData(startDate, endDate, _showVolume, binSize, db, usePermissivePhase);
                    }
                    else
                    {
                        GetSignalPhaseData(startDate, endDate, usePermissivePhase, db);
                    }
                }
                else
                {
                    if (Approach.IsProtectedPhaseOverlap)
                    {
                        GetSignalOverlapData(startDate, endDate, _showVolume, binSize, db, usePermissivePhase);
                    }
                    else
                    {
                        GetSignalPhaseData(startDate, endDate, usePermissivePhase, db);
                    }
                    //TotalVolume = controllerRepository.GetTmcVolume(startDate, endDate, Approach.SignalID,
                    //    Approach.ProtectedPhaseNumber);
                }
                
            }
        }

        public RLMSignalPhase()
        {
        }

        public bool GetPermissivePhase { get; set; }

        public VolumeCollection Volume { get; }

        public int PhaseNumber { get; set; }

        public double Violations
        {
            get { return Plans.PlanList.Sum(d => d.Violations); }
        }

        public RLMPlanCollection Plans { get; private set; }

        public List<RLMCycle> Cycles { get; set; }

        public double SevereRedLightViolationSeconds { get; }

        public double SevereRedLightViolations
        {
            get { return Plans.PlanList.Sum(d => d.SevereRedLightViolations); }
        }

        public double TotalVolume { get; private set; }

        public double PercentViolations
        {
            get
            {
                if (TotalVolume > 0)
                    return Math.Round(Violations / TotalVolume * 100, 0);
                return 0;
            }
        }

        public double PercentSevereViolations
        {
            get
            {
                if (TotalVolume > 0)
                    return Math.Round(SevereRedLightViolations / TotalVolume * 100, 2);
                return 0;
            }
        }

        public double YellowOccurrences
        {
            get { return Plans.PlanList.Sum(d => d.YellowOccurrences); }
        }

        public double TotalYellowTime
        {
            get { return Plans.PlanList.Sum(d => d.TotalYellowTime); }
        }

        public double AverageTYLO => Math.Round(TotalYellowTime / YellowOccurrences, 1);

        public double PercentYellowOccurrences
        {
            get
            {
                if (TotalVolume > 0)
                    return Math.Round(YellowOccurrences / TotalVolume * 100, 0);
                return 0;
            }
        }

        public double ViolationTime
        {
            get { return Plans.PlanList.Sum(p => p.ViolationTime); }
        }

        public Approach Approach { get; set; }

        private void GetSignalPhaseData(DateTime startDate, DateTime endDate, bool usePermissivePhase, SPM db)
        {
            if (usePermissivePhase)
                PhaseNumber = Approach.PermissivePhaseNumber ?? 0;
            else
                PhaseNumber = Approach.ProtectedPhaseNumber;
                var controllerRepository =
                    ControllerEventLogRepositoryFactory.Create(db);
                //TotalVolume = controllerRepository.GetTmcVolume(startDate, endDate, Approach.SignalID, PhaseNumber);
            //var cycles = CycleFactory.GetYellowToRedCycles(startDate, endDate, Approach, usePermissivePhase, db);
            List<int> li = new List<int> {1, 8, 9, 10, 11};
                var cycleEvents = controllerRepository.GetEventsByEventCodesParam(Approach.SignalID,
                    startDate.AddSeconds(-900), endDate.AddSeconds(900),li , PhaseNumber);
            GetRedCycle(startDate, endDate, cycleEvents);
            Plans = new RLMPlanCollection(Cycles, Cycles.Any() ? Cycles.First().StartTime : startDate, Cycles.Any()? Cycles.Last().EndTime:endDate, SevereRedLightViolationSeconds, Approach, db);
                if (Plans.PlanList.Count == 0)
                    Plans.AddItem(new RLMPlan(Cycles.Any() ? Cycles.First().StartTime : startDate, Cycles.Any() ? Cycles.Last().EndTime : endDate, 0, Cycles, SevereRedLightViolationSeconds,
                        Approach));
            
        }


        private void GetSignalOverlapData(DateTime startDate, DateTime endDate, bool showVolume, int binSize, SPM db, bool usePermissive)
        {
            var li = new List<int> {62, 63, 64};
            var controllerRepository =
                ControllerEventLogRepositoryFactory.Create(db);
            var cycleEvents = controllerRepository.GetEventsByEventCodesParam(Approach.SignalID,
                startDate.AddSeconds(-900), endDate.AddSeconds(900), li, Approach.ProtectedPhaseNumber);
            //List<Controller_Event_Log> beginningEvents = new List<Controller_Event_Log>();
            //List<Controller_Event_Log> endingEvents = new List<Controller_Event_Log>();
            //Parallel.Invoke(
            //    () =>
            //    {
            //        if (!cycleEvents.Any() || cycleEvents.First().EventCode != 63)
            //        {
            //           beginningEvents = GetEventsToStartCycle(usePermissive, startDate, Approach);
            //        }
            //    },
            //    () =>
            //    {
            //        if (!cycleEvents.Any() || cycleEvents.Last().EventCode != 61)
            //        {
            //            endingEvents = GetEventsToCompleteCycle(usePermissive, endDate, Approach);
            //        }
            //    });
            //if (beginningEvents.Any())
            //    cycleEvents.InsertRange(0, beginningEvents);
            //if (endingEvents.Any())
            //    cycleEvents.AddRange(endingEvents);
            GetRedCycle(startDate, endDate, cycleEvents);
            Plans = new RLMPlanCollection(Cycles, startDate, endDate, SevereRedLightViolationSeconds, Approach, db);
            if (Plans.PlanList.Count == 0)
                Plans.AddItem(new RLMPlan(startDate, endDate, 0, Cycles, SevereRedLightViolationSeconds,
                    Approach));
        }

        public List<Controller_Event_Log> GetEventsToCompleteCycle(bool getPermissivePhase, DateTime endDate, Approach approach)
        {
            var celRepository = ControllerEventLogRepositoryFactory.Create();
            if (getPermissivePhase)
            {
                var cycleEventNumbers = approach.IsPermissivePhaseOverlap
                    ? new List<int> { 61, 63, 64, 65 }
                    : new List<int> { 1, 8, 9 };
                return celRepository.GetTopEventsAfterDateByEventCodesParam(approach.SignalID,
                    endDate, cycleEventNumbers, approach.PermissivePhaseNumber.Value, 3).OrderByDescending(e => e.Timestamp).ToList();
            }
            else
            {
                var cycleEventNumbers = approach.IsProtectedPhaseOverlap
                    ? new List<int> { 61, 63, 64, 65 }
                    : new List<int> { 1, 8, 9 };
                return celRepository.GetTopEventsAfterDateByEventCodesParam(approach.SignalID,
                    endDate, cycleEventNumbers, approach.ProtectedPhaseNumber, 3).OrderByDescending(e => e.Timestamp).ToList();
            }
        }

        public List<Controller_Event_Log> GetEventsToStartCycle(bool getPermissivePhase, DateTime startDate, Approach approach)
        {
            var celRepository = ControllerEventLogRepositoryFactory.Create();
            if (getPermissivePhase)
            {
                var cycleEventNumbers = approach.IsPermissivePhaseOverlap
                    ? new List<int> { 63, 64, 65 }
                    : new List<int> { 8, 9, 11 };
                return  celRepository.GetTopEventsBeforeDateByEventCodesParam(approach.SignalID,
                    startDate, cycleEventNumbers, approach.PermissivePhaseNumber.Value, 3);
            }
            else
            {
                var cycleEventNumbers = approach.IsProtectedPhaseOverlap
                    ? new List<int> { 63, 64, 65 }
                    : new List<int> { 8, 9,11 };
                return celRepository.GetTopEventsBeforeDateByEventCodesParam(approach.SignalID,
                    startDate, cycleEventNumbers, approach.ProtectedPhaseNumber, 3);
            }
        }



        private void GetRedCycle(DateTime startTime, DateTime endTime,
           List<Controller_Event_Log> cycleEvents)
        {
            if(Cycles == null)
                Cycles = new List<RLMCycle>();
            RLMCycle cycle = null;
            //use a counter to help determine when we are on the last row
            var counter = 0;

            foreach (var row in cycleEvents)
            {
                //use a counter to help determine when we are on the last row
                counter++;
                //if (row.Timestamp >= startTime && row.Timestamp <= endTime)
                    if (cycle == null && GetEventType(row.EventCode) == RLMCycle.EventType.BeginYellowClearance)
                    {
                        cycle = new RLMCycle(row.Timestamp, SevereRedLightViolationSeconds);
                        cycle.NextEvent(GetEventType(row.EventCode), row.Timestamp);
                        if (cycle.Status == RLMCycle.NextEventResponse.GroupMissingData)
                            cycle = null;
                    }
                    else if (cycle != null)
                    {
                        cycle.NextEvent(GetEventType(row.EventCode), row.Timestamp);
                        if (cycle.Status == RLMCycle.NextEventResponse.GroupComplete) //&&((cycle.StartTime <= endTime && cycle.StartTime >= startTime)|| (cycle.EndTime >= startTime && cycle.EndTime <= endTime)))
                        {
                            Cycles.Add(cycle);
                            cycle = null;
                        }
                        else if (cycle.Status == RLMCycle.NextEventResponse.GroupMissingData)
                        {
                            cycle = null;
                        }
                    }
            }
            Cycles = Cycles.Where(c => (c.EndTime >= startTime && c.EndTime <= endTime) || (c.StartTime <= endTime && c.StartTime >= startTime)).ToList();
            AddDetectorData(startTime, endTime);
        }

        private RLMCycle.EventType GetEventType(int EventCode)
        {
            switch (EventCode)
            {
                case 8:
                    return RLMCycle.EventType.BeginYellowClearance;
                // overlap yellow
                case 63:
                    return RLMCycle.EventType.BeginYellowClearance;

                case 9:
                    return RLMCycle.EventType.BeginRedClearance;
                // overlap red
                case 64:
                    return RLMCycle.EventType.BeginRedClearance;

                case 65:
                    return RLMCycle.EventType.BeginRed;
                case 11:
                    return RLMCycle.EventType.BeginRed;

                case 1:
                    return RLMCycle.EventType.EndRed;
                // overlap green
                case 61:
                    return RLMCycle.EventType.EndRed;

                default:
                    return RLMCycle.EventType.Unknown;
            }
        }


        private void AddDetectorData(DateTime startTime, DateTime endTime)
        {
            var repository =
                ControllerEventLogRepositoryFactory.Create();
            var detectors = Approach.GetDetectorsForMetricType(11);
            var detectorActivations = new List<Controller_Event_Log>();
            foreach (var d in detectors)
                detectorActivations.AddRange(repository.GetEventsByEventCodesParamWithOffsetAndLatencyCorrection(Approach.SignalID,
                    startTime, endTime,
                    new List<int> { 82 }, d.DetChannel, 0, d.LatencyCorrection));
            TotalVolume = detectorActivations.Count;
            foreach (var cycle in Cycles)
            {
                var events =
                    detectorActivations.Where(d => d.Timestamp >= cycle.StartTime && d.Timestamp < cycle.EndTime);
                foreach (var cve in events)
                {
                    var ddp = new RLMDetectorDataPoint(cycle.StartTime, cve.Timestamp);
                    cycle.AddDetector(ddp);
                }
            }
        }
    }
}