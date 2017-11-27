using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Business.SplitFail;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using Approach = MOE.Common.Business.ApproachVolume.Approach;

namespace MOE.Common.Business
{
    public static class CycleFactory
    {
        public static List<RedToRedCycle> GetRedToRedCycles(Models.Approach approach, DateTime startTime, DateTime endTime, bool getPermissivePhase, List<Controller_Event_Log> detectorEvents)
        {
            var cycleEvents = GetCycleEvents(getPermissivePhase, startTime, endTime, approach);
            if (cycleEvents != null && cycleEvents.Count > 0 && GetEventType(cycleEvents.LastOrDefault().EventCode) != RedToRedCycle.EventType.ChangeToRed)
            {
                GetEventsToCompleteCycle(getPermissivePhase, startTime, endTime, approach, cycleEvents);
            }
            List<RedToRedCycle> cycles = new List<RedToRedCycle>();
            for (int i = 0; i < cycleEvents.Count; i++)
            {
                if (i < cycleEvents.Count - 3
                    && GetEventType(cycleEvents[i].EventCode) == RedToRedCycle.EventType.ChangeToRed
                    && GetEventType(cycleEvents[i + 1].EventCode) == RedToRedCycle.EventType.ChangeToGreen
                    && GetEventType(cycleEvents[i + 2].EventCode) == RedToRedCycle.EventType.ChangeToYellow
                    && GetEventType(cycleEvents[i + 3].EventCode) == RedToRedCycle.EventType.ChangeToRed)
                {
                    cycles.Add(new RedToRedCycle(cycleEvents[i].Timestamp, cycleEvents[i + 1].Timestamp, cycleEvents[i + 2].Timestamp, cycleEvents[i + 3].Timestamp));
                    i = i + 3;
                }
            }
            return cycles;
        }

        public static List<CyclePcd> GetPcdCycles(DateTime startDate, DateTime endDate, Models.Approach approach, List<Controller_Event_Log> detectorEvents, bool getPermissivePhase)
        {
            var cycleEvents = GetCycleEvents(getPermissivePhase, startDate, endDate, approach);
            List<CyclePcd> cycles = new List<CyclePcd>();
            for (int i = 0; i < cycleEvents.Count; i++)
            {
                if (i < cycleEvents.Count - 3
                    && GetEventType(cycleEvents[i].EventCode) == RedToRedCycle.EventType.ChangeToRed
                    && GetEventType(cycleEvents[i + 1].EventCode) == RedToRedCycle.EventType.ChangeToGreen
                    && GetEventType(cycleEvents[i + 2].EventCode) == RedToRedCycle.EventType.ChangeToYellow
                    && GetEventType(cycleEvents[i + 3].EventCode) == RedToRedCycle.EventType.ChangeToRed)
                {
                    cycles.Add(new CyclePcd(cycleEvents[i].Timestamp, cycleEvents[i + 1].Timestamp, cycleEvents[i + 2].Timestamp, cycleEvents[i + 3].Timestamp));
                    i = i + 3;
                }
            }
            if (cycles.Any())
            {
                foreach (var cycle in cycles)
                {
                    var eventsForCycle = detectorEvents.Where(d => d.Timestamp >= cycle.StartTime && d.Timestamp < cycle.EndTime).ToList();
                    foreach (var controllerEventLog in eventsForCycle)
                    {
                        cycle.AddDetectorData(new DetectorDataPoint(cycle.StartTime, controllerEventLog.Timestamp, cycle.GreenEvent, cycle.YellowEvent));
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
                // overlap green
                case 61:
                    return RedToRedCycle.EventType.ChangeToGreen;
                case 8:
                    return RedToRedCycle.EventType.ChangeToYellow;
                // overlap yellow
                case 63:
                    return RedToRedCycle.EventType.ChangeToYellow;
                case 10:
                    return RedToRedCycle.EventType.ChangeToRed;
                // overlap red
                case 64:
                    return RedToRedCycle.EventType.ChangeToRed;
                default:
                    return RedToRedCycle.EventType.Unknown;
            }
        }

        public static List<CycleSpeed> GetSpeedCycles(DateTime startDate, DateTime endDate, List<Speed_Events> detectorEvents, bool getPermissivePhase, Models.Approach approach)
        {
            var cycleEvents = GetCycleEvents(getPermissivePhase, startDate, endDate, approach);
            List<CycleSpeed> cycles = new List<CycleSpeed>();
            for (int i = 0; i < cycleEvents.Count; i++)
            {
                if (i < cycleEvents.Count - 3
                    && GetEventType(cycleEvents[i].EventCode) == RedToRedCycle.EventType.ChangeToRed
                    && GetEventType(cycleEvents[i + 1].EventCode) == RedToRedCycle.EventType.ChangeToGreen
                    && GetEventType(cycleEvents[i + 2].EventCode) == RedToRedCycle.EventType.ChangeToYellow
                    && GetEventType(cycleEvents[i + 3].EventCode) == RedToRedCycle.EventType.ChangeToRed)
                {
                    cycles.Add(new CycleSpeed(cycleEvents[i].Timestamp, cycleEvents[i + 1].Timestamp, cycleEvents[i + 2].Timestamp, cycleEvents[i + 3].Timestamp, detectorEvents));
                    i = i + 3;
                }
            }
            if (cycles.Any())
            {
                foreach (var cycle in cycles)
                {
                    cycle.SpeedEvents = detectorEvents
                        .Where(d => d.timestamp >= cycle.StartTime && d.timestamp < cycle.EndTime).ToList();
                }
            }
            return cycles;
        }
        private static List<Controller_Event_Log> GetCycleEvents(bool getPermissivePhase, DateTime startDate, DateTime endDate, Models.Approach approach)
        {
            var celRepository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            List<Controller_Event_Log> cycleEvents;
            if (getPermissivePhase)
            {
                cycleEvents = celRepository.GetEventsByEventCodesParam(approach.SignalID, startDate,
                    endDate, new List<int>() { 1, 8, 10 }, approach.PermissivePhaseNumber.Value);
            }
            else
            {
                var cycleEventNumbers = approach.IsProtectedPhaseOverlap ? new List<int> { 61, 63, 64 } : new List<int>() { 1, 8, 10 };
                cycleEvents = celRepository.GetEventsByEventCodesParam(approach.SignalID, startDate,
                    endDate, cycleEventNumbers, approach.ProtectedPhaseNumber);
            }
            return cycleEvents;
        }

        private static void GetEventsToCompleteCycle(bool getPermissivePhase, DateTime startDate, DateTime endDate, Models.Approach approach, List<Controller_Event_Log> cycleEvents)
        {
            var celRepository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            if (getPermissivePhase)
            {
                cycleEvents.AddRange(celRepository.GetTopEventsAfterDateByEventCodesParam(approach.SignalID,
                    endDate, new List<int>() { 1, 8, 10 }, approach.PermissivePhaseNumber.Value, 3));
            }
            else
            {
                var cycleEventNumbers = approach.IsProtectedPhaseOverlap ? new List<int> { 61, 63, 64 } : new List<int>() { 1, 8, 10 };
                cycleEvents.AddRange(celRepository.GetTopEventsAfterDateByEventCodesParam(approach.SignalID, endDate, cycleEventNumbers, approach.ProtectedPhaseNumber, 3));
            }
        }
        public static List<CycleSplitFail> GetSplitFailCycles(SplitFailOptions options, Models.Approach approach, 
            bool getPermissivePhase)
        {
            var cycleEvents = GetCycleEvents(getPermissivePhase, options.StartDate, options.EndDate, approach);
            var terminationEvents = GetTerminationEvents(getPermissivePhase, options.StartDate, options.EndDate, approach);
            List<CycleSplitFail> cycles = new List<CycleSplitFail>();
            for (int i = 0; i < cycleEvents.Count; i++)
            {
                if (i < cycleEvents.Count - 3
                    && GetEventType(cycleEvents[i].EventCode) == RedToRedCycle.EventType.ChangeToGreen
                    && GetEventType(cycleEvents[i + 1].EventCode) == RedToRedCycle.EventType.ChangeToYellow
                    && GetEventType(cycleEvents[i + 2].EventCode) == RedToRedCycle.EventType.ChangeToRed
                    && GetEventType(cycleEvents[i + 3].EventCode) == RedToRedCycle.EventType.ChangeToGreen)
                {
                    var termEvent = GetTerminationEventBetweenStartAndEnd(cycleEvents[i].Timestamp, cycleEvents[i + 3].Timestamp, terminationEvents);
                    cycles.Add(new CycleSplitFail(cycleEvents[i].Timestamp, cycleEvents[i + 2].Timestamp, cycleEvents[i + 1].Timestamp, cycleEvents[i + 3].Timestamp, termEvent, options.FirstSecondsOfRed));
                    i = i + 2;
                }
            }
            return cycles;
        }

        private static CycleSplitFail.TerminationType GetTerminationEventBetweenStartAndEnd(DateTime start, DateTime end, List<Controller_Event_Log> terminationEvents)
        {
            CycleSplitFail.TerminationType terminationType = CycleSplitFail.TerminationType.Unknown;
            var terminationEvent = terminationEvents.FirstOrDefault(t => t.Timestamp > start && t.Timestamp <= end);
            if (terminationEvent != null)
            {
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
            }
            return terminationType;
        }

        private static List<Controller_Event_Log> GetTerminationEvents(bool getPermissivePhase, DateTime startDate, DateTime endDate, 
            Models.Approach approach)
        {
            var celRepository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            List<Controller_Event_Log> cycleEvents = celRepository.GetEventsByEventCodesParam(approach.SignalID, startDate,
                 endDate, new List<int>() { 4,5,6 }, getPermissivePhase? approach.PermissivePhaseNumber.Value: approach.ProtectedPhaseNumber);
            return cycleEvents;
        }
    }
}