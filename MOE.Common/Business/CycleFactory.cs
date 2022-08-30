using System;
using System.Collections.Generic;
//using System.IO;
using System.Linq;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business
{
    public static class CycleFactory
    {
        public static List<RedToRedCycle> GetRedToRedCycles(Approach approach, DateTime startTime, DateTime endTime,
            bool getPermissivePhase, List<Controller_Event_Log> detectorEvents)
        {
            var cycleEvents = GetCycleEvents(getPermissivePhase, startTime, endTime, approach,null);
            if (cycleEvents != null && cycleEvents.Count > 0 && GetEventType(cycleEvents.LastOrDefault().EventCode) !=
                RedToRedCycle.EventType.ChangeToRed)
                GetEventsToCompleteCycle(getPermissivePhase, endTime, approach, cycleEvents);
            var cycles = new List<RedToRedCycle>();
            for (var i = 0; i < cycleEvents.Count; i++)
                if (i < cycleEvents.Count - 3
                    && GetEventType(cycleEvents[i].EventCode) == RedToRedCycle.EventType.ChangeToRed
                    && GetEventType(cycleEvents[i + 1].EventCode) == RedToRedCycle.EventType.ChangeToGreen
                    && GetEventType(cycleEvents[i + 2].EventCode) == RedToRedCycle.EventType.ChangeToYellow
                    && GetEventType(cycleEvents[i + 3].EventCode) == RedToRedCycle.EventType.ChangeToRed)
                    cycles.Add(new RedToRedCycle(cycleEvents[i].Timestamp, cycleEvents[i + 1].Timestamp,
                        cycleEvents[i + 2].Timestamp, cycleEvents[i + 3].Timestamp));
            return cycles;
        }

        public static List<CyclePcd> GetPcdCycles(DateTime startDate, DateTime endDate, Approach approach,
            List<Controller_Event_Log> detectorEvents, bool getPermissivePhase, int? pcdCycleTime, SPM db)
        {
            double pcdCycleShift = pcdCycleTime ?? 0;
            var cycleEvents = GetCycleEvents(getPermissivePhase, startDate, endDate, approach,db);
            if (cycleEvents != null && cycleEvents.Count > 0 && GetEventType(cycleEvents.LastOrDefault().EventCode) !=
                RedToRedCycle.EventType.ChangeToRed)
                GetEventsToCompleteCycle(getPermissivePhase, endDate, approach, cycleEvents);
            var cycles = new List<CyclePcd>();
            for (var i = 0; i < cycleEvents.Count; i++)
                if (i < cycleEvents.Count - 3
                    && GetEventType(cycleEvents[i].EventCode) == RedToRedCycle.EventType.ChangeToRed
                    && GetEventType(cycleEvents[i + 1].EventCode) == RedToRedCycle.EventType.ChangeToGreen
                    && GetEventType(cycleEvents[i + 2].EventCode) == RedToRedCycle.EventType.ChangeToYellow
                    && GetEventType(cycleEvents[i + 3].EventCode) == RedToRedCycle.EventType.ChangeToRed)
                    cycles.Add(new CyclePcd(cycleEvents[i].Timestamp, cycleEvents[i + 1].Timestamp,
                        cycleEvents[i + 2].Timestamp, cycleEvents[i + 3].Timestamp));
            if (cycles.Any())
                foreach (var cycle in cycles)
                {
                    var eventsForCycle = detectorEvents
                        .Where(d => d.Timestamp >= cycle.StartTime.AddSeconds(-pcdCycleShift) && d.Timestamp < cycle.EndTime.AddSeconds(pcdCycleShift)).ToList();
                    foreach (var controllerEventLog in eventsForCycle)
                        cycle.AddDetectorData(new DetectorDataPoint(cycle.StartTime, controllerEventLog.Timestamp,
                            cycle.GreenEvent, cycle.YellowEvent));
                }
            //var totalSortedEvents = cycles.Sum(d => d.DetectorEvents.Count);
            return cycles;
        }

        public static List<TimingAndActuationCycle> GetTimingAndActuationCycles(DateTime startDate, DateTime endDate, Approach approach, bool getPermissivePhase)
        {
            var cycleEvents = GetDetailedCycleEvents(getPermissivePhase, startDate, endDate, approach);
            if (cycleEvents != null && cycleEvents.Count > 0 && GetEventType(cycleEvents.LastOrDefault().EventCode) != RedToRedCycle.EventType.ChangeToRed)
                GetEventsToCompleteCycle(getPermissivePhase, endDate, approach, cycleEvents);
            var cycles = new List<TimingAndActuationCycle>();
            DateTime dummyTime;
            for (var i = 0; i < cycleEvents.Count; i++)
            {
                dummyTime = new DateTime(1900, 1, 1);
                if (i < cycleEvents.Count - 5
                    && GetEventType(cycleEvents[i].EventCode) == RedToRedCycle.EventType.ChangeToGreen
                    && GetEventType(cycleEvents[i + 1].EventCode) == RedToRedCycle.EventType.ChangeToEndMinGreen
                    && GetEventType(cycleEvents[i + 2].EventCode) == RedToRedCycle.EventType.ChangeToYellow
                    && GetEventType(cycleEvents[i + 3].EventCode) == RedToRedCycle.EventType.ChangeToRed
                    && GetEventType(cycleEvents[i + 4].EventCode) == RedToRedCycle.EventType.ChangeToEndOfRedClearance
                    && GetEventType(cycleEvents[i + 5].EventCode) == RedToRedCycle.EventType.ChangeToGreen
                )
                    cycles.Add(new TimingAndActuationCycle(cycleEvents[i].Timestamp, cycleEvents[i + 1].Timestamp,
                        cycleEvents[i + 2].Timestamp, cycleEvents[i + 3].Timestamp, cycleEvents[i + 4].Timestamp,
                        cycleEvents[i + 5].Timestamp, dummyTime));
            }
            //// If there are no 5 part cycles, Try to get a 3 or 4 part cycle.
            //get 4 part series is 61, 63,64 and maybe 66
            if (cycles.Count != 0) return cycles;
            {
                var endRedEvent = new DateTime();
                dummyTime = new DateTime(1900, 1, 1);
                for (var i = 0; i < cycleEvents.Count; i++)
                {
                    if (i < cycleEvents.Count - 5
                        && GetEventType(cycleEvents[i].EventCode) == RedToRedCycle.EventType.ChangeToGreen
                        && GetEventType(cycleEvents[i + 1].EventCode) == RedToRedCycle.EventType.ChangeToYellow
                        && GetEventType(cycleEvents[i + 2].EventCode) == RedToRedCycle.EventType.ChangeToRed
                    )
                    {
                        var overlapDarkTime = cycleEvents[i + 3].Timestamp;
                        endRedEvent = cycleEvents[i + 4].Timestamp;

                        if (GetEventType(cycleEvents[i + 3].EventCode) != RedToRedCycle.EventType.OverLapDark)
                        {
                            endRedEvent = cycleEvents[i + 3].Timestamp;
                        }
                        cycles.Add(new TimingAndActuationCycle(cycleEvents[i].Timestamp, dummyTime, cycleEvents[i + 1].Timestamp,
                            dummyTime, cycleEvents[i+2].Timestamp, endRedEvent, overlapDarkTime));
                    }
                }
            }
            return cycles;
        }

        private static RedToRedCycle.EventType GetEventType(int eventCode)
        {
            switch (eventCode)
            {
                case 1:
                    return RedToRedCycle.EventType.ChangeToGreen;
                case 3:
                    return RedToRedCycle.EventType.ChangeToEndMinGreen;
                case 61:
                    return RedToRedCycle.EventType.ChangeToGreen;
                case 8:
                    return RedToRedCycle.EventType.ChangeToYellow;
                case 63:
                    return RedToRedCycle.EventType.ChangeToYellow;
                case 9:
                    return RedToRedCycle.EventType.ChangeToRed;
                case 11:
                    return RedToRedCycle.EventType.ChangeToEndOfRedClearance;
                case 64:
                    return RedToRedCycle.EventType.ChangeToRed;
                case 66:
                    return RedToRedCycle.EventType.OverLapDark;
                default:
                    return RedToRedCycle.EventType.Unknown;
            }
        }

        public static List<CycleSpeed> GetSpeedCycles(DateTime startDate, DateTime endDate, bool getPermissivePhase,
            Models.Detector detector)
        {
            var cycleEvents = GetCycleEvents(getPermissivePhase, startDate, endDate, detector.Approach, null);
            if (cycleEvents != null && cycleEvents.Count > 0 && GetEventType(cycleEvents.LastOrDefault().EventCode) !=
                RedToRedCycle.EventType.ChangeToRed)
                GetEventsToCompleteCycle(getPermissivePhase, endDate, detector.Approach, cycleEvents);
            var cycles = new List<CycleSpeed>();
            if (cycleEvents != null)
                for (var i = 0; i < cycleEvents.Count; i++)
                    if (i < cycleEvents.Count - 3
                        && GetEventType(cycleEvents[i].EventCode) == RedToRedCycle.EventType.ChangeToRed
                        && GetEventType(cycleEvents[i + 1].EventCode) == RedToRedCycle.EventType.ChangeToGreen
                        && GetEventType(cycleEvents[i + 2].EventCode) == RedToRedCycle.EventType.ChangeToYellow
                        && GetEventType(cycleEvents[i + 3].EventCode) == RedToRedCycle.EventType.ChangeToRed)
                        cycles.Add(new CycleSpeed(cycleEvents[i].Timestamp, cycleEvents[i + 1].Timestamp,
                            cycleEvents[i + 2].Timestamp, cycleEvents[i + 3].Timestamp));
            if (cycles.Any())
            {
                var speedEventRepository = SpeedEventRepositoryFactory.Create();
                var speedEvents = speedEventRepository.GetSpeedEventsByDetector(startDate,
                    cycles.LastOrDefault().EndTime, detector, detector.MinSpeedFilter ?? 5);
                foreach (var cycle in cycles)
                    cycle.FindSpeedEventsForCycle(speedEvents);
            }
            return cycles;
        }

        private static List<Controller_Event_Log> GetCycleEvents(bool getPermissivePhase, DateTime startDate,
            DateTime endDate, Approach approach, SPM db)
        {
            IControllerEventLogRepository celRepository;
            if (db != null)
                 celRepository = ControllerEventLogRepositoryFactory.Create(db);
            else
                 celRepository = ControllerEventLogRepositoryFactory.Create();
            List<Controller_Event_Log> cycleEvents;
            if (getPermissivePhase)
            {
                var cycleEventNumbers = approach.IsPermissivePhaseOverlap
                    ? new List<int> {61, 63, 64, 66}
                    : new List<int> {1, 8, 9};
                cycleEvents = celRepository.GetEventsByEventCodesParam(approach.SignalID, startDate,
                    endDate, cycleEventNumbers, approach.PermissivePhaseNumber.Value);

                //cycleEvents = celRepository.GetEventsByEventCodesParam(approach.SignalID, startDate,
                //    endDate, new List<int>() {1, 8, 9}, approach.PermissivePhaseNumber.Value);
            }
            else
            {
                var cycleEventNumbers = approach.IsProtectedPhaseOverlap
                    ? new List<int> {61, 63, 64,66}
                    : new List<int> {1, 8, 9};
                cycleEvents = celRepository.GetEventsByEventCodesParam(approach.SignalID, startDate,
                    endDate, cycleEventNumbers, approach.ProtectedPhaseNumber);
            }
            return cycleEvents;
        }

        private static List<Controller_Event_Log> GetDetailedCycleEvents(bool getPermissivePhase, DateTime startDate,
            DateTime endDate, Approach approach)
        {
            var celRepository = ControllerEventLogRepositoryFactory.Create();
            List<Controller_Event_Log> cycleEvents;
            
            
            if (getPermissivePhase)
            {
                var cycleEventNumbers = approach.IsPermissivePhaseOverlap
                    ? new List<int> { 61, 63, 64, 66 }
                    : new List<int> { 1, 3, 8, 9, 11 };
                cycleEvents = celRepository.GetEventsByEventCodesParam(approach.SignalID, startDate,
                    endDate, cycleEventNumbers, approach.PermissivePhaseNumber.Value);
            }
            else
            {
                var cycleEventNumbers = approach.IsProtectedPhaseOverlap
                    ? new List<int> { 61, 63, 64, 66 }
                    : new List<int> { 1, 3, 8, 9, 11 };
                cycleEvents = celRepository.GetEventsByEventCodesParam(approach.SignalID, startDate,
                    endDate, cycleEventNumbers, approach.ProtectedPhaseNumber);
            }
            return cycleEvents;
        }

        private static void GetEventsToCompleteCycle(bool getPermissivePhase, DateTime endDate, Approach approach,
            List<Controller_Event_Log> cycleEvents)
        {
            var celRepository = ControllerEventLogRepositoryFactory.Create();
            if (getPermissivePhase)
            {
                var cycleEventNumbers = approach.IsPermissivePhaseOverlap
                    ? new List<int> {61, 63, 64, 66}
                    : new List<int> {1, 8, 9};
                var eventsAfterEndDate = celRepository.GetTopEventsAfterDateByEventCodesParam(approach.SignalID,
                    endDate, cycleEventNumbers, approach.PermissivePhaseNumber.Value, 3);
                if (eventsAfterEndDate != null)
                    cycleEvents.AddRange(eventsAfterEndDate);
            }
            else
            {
                var cycleEventNumbers = approach.IsProtectedPhaseOverlap
                    ? new List<int> {61, 63, 64, 66}
                    : new List<int> {1, 8, 9};
                var eventsAfterEndDate = celRepository.GetTopEventsAfterDateByEventCodesParam(approach.SignalID,
                    endDate, cycleEventNumbers, approach.ProtectedPhaseNumber, 3);
                if (eventsAfterEndDate != null)
                    cycleEvents.AddRange(eventsAfterEndDate);
            }
        }

        public static List<CycleSplitFail> GetSplitFailCycles(SplitFailOptions options, Approach approach,
            bool getPermissivePhase, SPM db)
        {
            var cycleEvents = GetCycleEvents(getPermissivePhase, options.StartDate, options.EndDate, approach, db);
            if (cycleEvents != null && cycleEvents.Count > 0 && GetEventType(cycleEvents.LastOrDefault().EventCode) !=
                RedToRedCycle.EventType.ChangeToGreen)
                GetEventsToCompleteCycle(getPermissivePhase, options.EndDate, approach, cycleEvents);
            var terminationEvents =
                GetTerminationEvents(getPermissivePhase, options.StartDate, options.EndDate, approach);
            var cycles = new List<CycleSplitFail>();
            for (var i = 0; i < cycleEvents.Count - 3; i++)
                if ( GetEventType(cycleEvents[i].EventCode) == RedToRedCycle.EventType.ChangeToGreen
                    && GetEventType(cycleEvents[i + 1].EventCode) == RedToRedCycle.EventType.ChangeToYellow
                    && GetEventType(cycleEvents[i + 2].EventCode) == RedToRedCycle.EventType.ChangeToRed
                    && (GetEventType(cycleEvents[i + 3].EventCode) == RedToRedCycle.EventType.ChangeToGreen || cycleEvents[i + 3].EventCode == 66))
                {
                    var termEvent = GetTerminationEventBetweenStartAndEnd(cycleEvents[i].Timestamp,
                        cycleEvents[i + 3].Timestamp, terminationEvents);
                    cycles.Add(new CycleSplitFail(cycleEvents[i].Timestamp, cycleEvents[i + 2].Timestamp,
                        cycleEvents[i + 1].Timestamp, cycleEvents[i + 3].Timestamp, termEvent,
                        options.FirstSecondsOfRed));
                    //i = i + 2;
                }
            return cycles;
        }

        private static CycleSplitFail.TerminationType GetTerminationEventBetweenStartAndEnd(DateTime start,
            DateTime end, List<Controller_Event_Log> terminationEvents)
        {
            var terminationType = CycleSplitFail.TerminationType.Unknown;
            var terminationEvent = terminationEvents.FirstOrDefault(t => t.Timestamp > start && t.Timestamp <= end);
            if (terminationEvent != null)
                switch (terminationEvent.EventCode)
                {
                    case 4:
                        terminationType = CycleSplitFail.TerminationType.GapOut;
                        break;
                    case 5:
                        terminationType = CycleSplitFail.TerminationType.MaxOut;
                        break;
                    case 6:
                        terminationType = CycleSplitFail.TerminationType.ForceOff;
                        break;
                    default:
                        terminationType = CycleSplitFail.TerminationType.Unknown;
                        break;
                }
            return terminationType;
        }

        private static List<Controller_Event_Log> GetTerminationEvents(bool getPermissivePhase, DateTime startDate,
            DateTime endDate,
            Approach approach)
        {
            var celRepository = ControllerEventLogRepositoryFactory.Create();
            var cycleEvents = celRepository.GetEventsByEventCodesParam(approach.SignalID, startDate,
                endDate, new List<int> {4, 5, 6},
                getPermissivePhase ? approach.PermissivePhaseNumber.Value : approach.ProtectedPhaseNumber);
            return cycleEvents;
        }
    }
}