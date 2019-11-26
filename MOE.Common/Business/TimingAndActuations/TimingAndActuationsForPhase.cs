using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Text;
using System.Linq;
using CsvHelper.Configuration.Attributes;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.TimingAndActuations
{
    public class TimingAndActuationsForPhase
    {
        public readonly int PhaseNumber;
        public bool PhaseOrOverlap { get; set; }
        public Approach Approach { get; set; }
        public List<Plan> Plans { get; set; }

        public string PhaseNumberSort { get; set; }
        public bool GetPermissivePhase { get; }
        public TimingAndActuationsOptions Options { get; }
        public List<TimingAndActuationCycle> Cycles { get; set; }
        public List<Controller_Event_Log> CycleDataEventLogs { get; set; }
        public List<Controller_Event_Log> PedestrianEvents { get; set; }
        public List<Controller_Event_Log> PedestrianIntervals { get; set; }
        public List<Controller_Event_Log> ForceEventsForAllLanes { get; set; }
        public Dictionary<string, List<Controller_Event_Log>> CycleAllEvents { get; set; }
        public Dictionary<string, List<Controller_Event_Log>> PedestrianAllEvents { get; set; }
        public Dictionary<string, List<Controller_Event_Log>> PedestrianAllIntervals { get; set; }
        public Dictionary<string, List<Controller_Event_Log>> AdvanceCountEvents { get; set; }
        public Dictionary<string, List<Controller_Event_Log>> AdvancePresenceEvents { get; set; }
        public Dictionary<string, List<Controller_Event_Log>> StopBarEvents { get; set; }
        public Dictionary<string, List<Controller_Event_Log>> LaneByLanes { get; set; }
        public Dictionary<string, List<Controller_Event_Log>> PhaseCustomEvents { get; set; }

    public TimingAndActuationsForPhase(int phaseNumber, bool phaseOrOverlap, TimingAndActuationsOptions options)
        {
            PhaseNumber = phaseNumber;
            Options = options;
            PhaseOrOverlap = phaseOrOverlap;
            GetAllRawCycleData(options.StartDate, options.EndDate, PhaseOrOverlap);
            if (Options.ShowPedestrianIntervals)
            {
                GetPedestrianIntervals(PhaseOrOverlap);
            }
            //if (Options.ShowPedestrianActuation && !GetPermissivePhase)
            if (Options.ShowPedestrianActuation)
            {
                    GetPedestrianEvents();
            }
            if (Options.PhaseEventCodesList != null)
            {
                var optionsSignalID = Options.SignalID;
                GetRawCustomEvents( optionsSignalID,  PhaseNumber, options.StartDate, options.EndDate);
            }
        }

       
        public TimingAndActuationsForPhase(Approach approach, TimingAndActuationsOptions options,
            bool getPermissivePhase)
        {
            GetPermissivePhase = getPermissivePhase;
            Approach = approach;
            Options = options;
            PhaseNumber = GetPermissivePhase ? Approach.PermissivePhaseNumber.Value : Approach.ProtectedPhaseNumber;
            if (Options.ShowVehicleSignalDisplay)
            {
                GetAllCycleData(Options.StartDate, Options.EndDate, approach, getPermissivePhase);
            }
            if (Options.ShowStopBarPresence)
            {
                GetStopBarPresenceEvents();
            }
            if (Options.ShowPedestrianActuation && !GetPermissivePhase)
            {
                GetPedestrianEvents();
            }
            if (Options.ShowPedestrianIntervals && !GetPermissivePhase)
            {
                var getPhaseOrOverlapEvents = !(approach.IsProtectedPhaseOverlap || approach.IsPermissivePhaseOverlap);
                GetPedestrianIntervals(getPhaseOrOverlapEvents);
            }
            if (Options.ShowLaneByLaneCount)
            {
                GetLaneByLaneEvents();
            }
            if (Options.ShowAdvancedDilemmaZone)
            {
                GetAdvancePresenceEvents();
            }
            if (Options.ShowAdvancedCount)
            {
                GetAdvanceCountEvents();
            }
            if (Options.PhaseEventCodesList != null)
            {
                GetPhaseCustomEvents();
            }
        }

        private void GetRawCustomEvents(string signalID, int numberPhase, DateTime optionsStartDateTime, DateTime optionsEndDateTime)
        {
            PhaseCustomEvents = new Dictionary<string, List<Controller_Event_Log>>();
            var controllerEventLogRepository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            if (Options.PhaseEventCodesList != null && Options.PhaseEventCodesList.Any() &&
                Options.PhaseEventCodesList.Count > 0)
            {
                foreach (var phaseEventCode in Options.PhaseEventCodesList)
                {
                    var phaseEvents = controllerEventLogRepository.GetEventsByEventCodesParam(signalID,
                        optionsStartDateTime, optionsEndDateTime, new List<int> {phaseEventCode}, numberPhase);
                    if (phaseEvents.Count > 0)
                    {
                        PhaseCustomEvents.Add("Phase Event Code: " + phaseEventCode, phaseEvents);
                    }
                }
            }
        }

        private void GetAllRawCycleData(DateTime optionsStartDate, DateTime optionsEndDate, bool phasedata)
        {
            CycleDataEventLogs = new List<Controller_Event_Log>();
            CycleAllEvents = new Dictionary<string, List<Controller_Event_Log>>();
            var extendLine = 6;
            var controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();
            var simpleEndDate = optionsEndDate;
            var phaseEventCodesForCycles = new List<int> { 1, 3, 8, 9, 11 };
            if (!phasedata)
            {
                phaseEventCodesForCycles = new List<int> { 61, 62, 63, 64, 65 };
            }
            string keyLabel = "Cycles Intervals " + PhaseNumber + " " + phasedata;
            CycleDataEventLogs = controllerEventLogRepository.GetEventsByEventCodesParam(Options.SignalID,
                optionsStartDate.AddMinutes(-extendLine), simpleEndDate.AddMinutes(extendLine),
                phaseEventCodesForCycles, PhaseNumber);
            if (CycleDataEventLogs.Count > 0)
            {
                CycleAllEvents.Add(keyLabel, CycleDataEventLogs);
            }
        }

        private void GetAllCycleData(DateTime addMinutes, DateTime dateTime, Approach approach, bool getPermissivePhase)
        {
            CycleDataEventLogs = new List<Controller_Event_Log>();
            CycleAllEvents = new Dictionary<string, List<Controller_Event_Log>>();
            var extendLine = 6;
            var controllerEventLogRepository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            var simpleEndDate = Options.EndDate;
            var phaseEventCodesForCycles = new List<int>{1, 3, 8, 9, 11};
            if (approach.IsProtectedPhaseOverlap || approach.IsPermissivePhaseOverlap)
            {
                phaseEventCodesForCycles = new List<int> {61, 62, 63, 64, 65};
            }
            int phaseNumber = getPermissivePhase ? approach.PermissivePhaseNumber.Value : approach.ProtectedPhaseNumber;
            string keyLabel = "Cycles Intervals " + phaseNumber;
            CycleDataEventLogs = controllerEventLogRepository.GetEventsByEventCodesParam(Approach.SignalID,
                Options.StartDate.AddMinutes(-extendLine), simpleEndDate.AddMinutes(extendLine),
                phaseEventCodesForCycles, PhaseNumber);
            if (CycleDataEventLogs.Count <= 0)
            {
                extendLine = 60;
                CycleDataEventLogs = controllerEventLogRepository.GetEventsByEventCodesParam(Approach.SignalID,
                    Options.StartDate.AddMinutes(-extendLine), simpleEndDate.AddMinutes(6),
                    phaseEventCodesForCycles, PhaseNumber);
            }
            if (CycleDataEventLogs.Count <= 0)
            {
                extendLine = 24 * 60;
                CycleDataEventLogs = controllerEventLogRepository.GetEventsByEventCodesParam(Approach.SignalID,
                    Options.StartDate.AddMinutes(-extendLine), simpleEndDate.AddMinutes(6),
                    phaseEventCodesForCycles, PhaseNumber);
            }
            CycleAllEvents.Add(keyLabel, CycleDataEventLogs);
        }

        private void GetPhaseCustomEvents()
        {
            PhaseCustomEvents = new Dictionary<string, List<Controller_Event_Log>>();
            var controllerEventLogRepository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            if (Options.PhaseEventCodesList != null && Options.PhaseEventCodesList.Any() &&
                Options.PhaseEventCodesList.Count > 0)
            {
                foreach (var phaseEventCode in Options.PhaseEventCodesList)
                {
                    var phaseEvents = controllerEventLogRepository.GetEventsByEventCodesParam(Approach.SignalID,
                        Options.StartDate, Options.EndDate, new List<int> {phaseEventCode}, PhaseNumber);
                    if (phaseEvents.Count > 0)
                    {
                        PhaseCustomEvents.Add(
                            "Phase Events: " + phaseEventCode,phaseEvents);
                    }
                    if (PhaseCustomEvents.Count == 0 && Options.ShowAllLanesInfo)
                    {
                        var forceEventsForAllLanes = new List<Controller_Event_Log>();
                        var tempEvent1 = new Controller_Event_Log()
                        {
                            SignalID = Options.SignalID,
                            EventCode = phaseEventCode,
                            EventParam = PhaseNumber,
                            Timestamp = Options.StartDate.AddSeconds(-10)
                        };
                        forceEventsForAllLanes.Add(tempEvent1);
                        var tempEvent2 = new Controller_Event_Log()
                        {
                            SignalID = Options.SignalID,
                            EventCode = phaseEventCode,
                            EventParam = PhaseNumber,
                            Timestamp = Options.StartDate.AddSeconds(-9)
                        };
                        forceEventsForAllLanes.Add(tempEvent2);
                        PhaseCustomEvents.Add(
                            "Phase Events: " + phaseEventCode, forceEventsForAllLanes);
                    }
                }
            }
        }

        private void GetLaneByLaneEvents()
        {
            LaneByLanes = new Dictionary<string, List<Controller_Event_Log>>();
            var controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();
            var localSortedDetectors = Approach.Detectors.OrderByDescending(d => d.MovementType.DisplayOrder)
                .ThenByDescending(l => l.LaneNumber).ToList();
            foreach (var detector in localSortedDetectors)
            {
                if (detector.DetectionTypes.Any(d => d.DetectionTypeID == 4))
                {
                    var laneNumber = "";
                    if (detector.LaneNumber != null)
                    {
                        laneNumber = detector.LaneNumber.Value.ToString();
                    }
                    var laneByLane = controllerEventLogRepository.GetEventsByEventCodesParam(Approach.SignalID,
                        Options.StartDate, Options.EndDate, new List<int> {81, 82}, detector.DetChannel);
                    if (laneByLane.Count > 0)
                    {
                        LaneByLanes.Add("Lane-by-lane Count " + detector.MovementType.Abbreviation + " " +
                                        laneNumber + ", ch " + detector.DetChannel, laneByLane);
                    }
                    if (LaneByLanes.Count == 0 && Options.ShowAllLanesInfo)
                    {
                        var forceEventsForAllLanes = new List<Controller_Event_Log>();
                        var tempEvent1 = new Controller_Event_Log()
                        {
                            SignalID = Options.SignalID,
                            EventCode = 82,
                            EventParam = detector.DetChannel,
                            Timestamp = Options.StartDate.AddSeconds(-10)
                        };
                        forceEventsForAllLanes.Add(tempEvent1);
                        var tempEvent2 = new Controller_Event_Log()
                        {
                            SignalID = Options.SignalID,
                            EventCode = 81,
                            EventParam = detector.DetChannel,
                            Timestamp = Options.StartDate.AddSeconds(-9)
                        };
                        forceEventsForAllLanes.Add(tempEvent2);
                        LaneByLanes.Add("Lane-by-Lane Count, ch " + detector.DetChannel + " " +
                                        detector.MovementType.Abbreviation + " " +
                                        laneNumber, forceEventsForAllLanes);
                    }
                }
            }
        }

        private void GetStopBarPresenceEvents()
        {
            StopBarEvents = new Dictionary<string, List<Controller_Event_Log>>();
            var controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();
            var localSortedDetectors = Approach.Detectors.OrderByDescending(d => d.MovementType.DisplayOrder)
                .ThenByDescending(l => l.LaneNumber).ToList();
            foreach (var detector in localSortedDetectors)
            {
                if (detector.DetectionTypes.Any(d => d.DetectionTypeID == 6))
                {
                    var stopEvents = controllerEventLogRepository.GetEventsByEventCodesParam(Approach.SignalID,
                        Options.StartDate, Options.EndDate, new List<int> {81, 82}, detector.DetChannel);
                    var laneNumber = "";
                    if (detector.LaneNumber != null)
                    {
                        laneNumber = detector.LaneNumber.Value.ToString();
                    }
                    if (stopEvents.Count > 0)
                    {
                        StopBarEvents.Add("Stop Bar Presence, " + detector.MovementType.Abbreviation + " " +
                                          laneNumber + ", ch " + detector.DetChannel, stopEvents);
                    }
                    if (stopEvents.Count == 0 && Options.ShowAllLanesInfo)
                    {
                        var forceEventsForAllLanes = new List<Controller_Event_Log>();
                        var event1 = new Controller_Event_Log()
                        {
                            SignalID = Options.SignalID,
                            EventCode = 82,
                            EventParam = detector.DetChannel,
                            Timestamp = Options.StartDate.AddSeconds(-10)
                        };
                        forceEventsForAllLanes.Add(event1);
                        var event2 = new Controller_Event_Log()
                        {
                            SignalID = Options.SignalID,
                            EventParam = detector.DetChannel,
                            EventCode = 81,
                            Timestamp = Options.StartDate.AddSeconds(-9)

                        };
                        forceEventsForAllLanes.Add(event2);
                        StopBarEvents.Add("Stop Bar Presence, ch " + detector.DetChannel + " " +
                                          detector.MovementType.Abbreviation + " " +
                                          laneNumber, forceEventsForAllLanes);
                    }
                }
            }
        }

        private void GetAdvanceCountEvents()
        {
            AdvanceCountEvents = new Dictionary<string, List<Controller_Event_Log>>();
            var controllerEventLogRepository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            var localSortedDetectors = Approach.Detectors.OrderByDescending(d => d.MovementType.DisplayOrder)
                .ThenByDescending(l => l.LaneNumber).ToList();
            foreach (var detector in localSortedDetectors)
            {
                if (detector.DetectionTypes.Any(d => d.DetectionTypeID == 2))
                {
                    var advanceEvents = controllerEventLogRepository.GetEventsByEventCodesParam(Approach.SignalID,
                        Options.StartDate, Options.EndDate, new List<int> {81, 82}, detector.DetChannel);
                    var laneNumber = "";
                    if (detector.LaneNumber != null)
                    {
                        laneNumber = detector.LaneNumber.Value.ToString();
                    }
                    if (advanceEvents.Count > 0)
                    {
                        AdvanceCountEvents.Add("Advanced Count (" + detector.DistanceFromStopBar + " ft) " +
                                                detector.MovementType.Abbreviation + " " +
                                                laneNumber +
                                                ", ch " + detector.DetChannel, advanceEvents);
                    } 
                    else if (AdvanceCountEvents.Count == 0 && Options.ShowAllLanesInfo)
                    {
                        var forceEventsForAllLanes = new List<Controller_Event_Log>();
                        var tempEvent1 = new Controller_Event_Log()
                        {
                            SignalID = Options.SignalID,
                            EventCode = 82,
                            EventParam = detector.DetChannel,
                            Timestamp = Options.StartDate.AddSeconds(-10)
                        };
                        forceEventsForAllLanes.Add(tempEvent1);
                        var tempEvent2 = new Controller_Event_Log()
                        {
                            SignalID = Options.SignalID,
                            EventCode = 81,
                            EventParam = detector.DetChannel,
                            Timestamp = Options.StartDate.AddSeconds(-9)
                        };
                        forceEventsForAllLanes.Add(tempEvent2);
                        AdvanceCountEvents.Add("Advanced Count (" + detector.DistanceFromStopBar + " ft), ch " +
                                               detector.DetChannel + " " + detector.MovementType.Abbreviation + " " +
                                               laneNumber, forceEventsForAllLanes);
                    }
                }
            }
        }

        private void GetAdvancePresenceEvents()
        {
            AdvancePresenceEvents = new Dictionary<string, List<Controller_Event_Log>>();
            var controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();
            var localSortedDetectors = Approach.Detectors.OrderByDescending(d => d.MovementType.DisplayOrder)
                .ThenByDescending(l => l.LaneNumber).ToList();
            foreach (var detector in localSortedDetectors)
            {
                if (detector.DetectionTypes.Any(d => d.DetectionTypeID == 7))
                {
                    var advancePresence = controllerEventLogRepository.GetEventsByEventCodesParam(Approach.SignalID,
                        Options.StartDate, Options.EndDate, new List<int> {81, 82}, detector.DetChannel);
                    var laneNumber = "";
                    if (detector.LaneNumber != null)
                    {
                        laneNumber = detector.LaneNumber.Value.ToString();
                    }
                    if (advancePresence.Count > 0)
                    {
                        AdvancePresenceEvents.Add("Advanced Presence, " + detector.MovementType.Abbreviation + " " +
                                                  laneNumber + ", ch " + detector.DetChannel, advancePresence);
                    }
                    else if (AdvancePresenceEvents.Count == 0 && Options.ShowAllLanesInfo)
                    {
                        var forceEventsForAllLanes = new List<Controller_Event_Log>();
                        var tempEvent1 = new Controller_Event_Log()
                        {
                            SignalID = Options.SignalID,
                            EventCode = 82,
                            EventParam = detector.DetChannel,
                            Timestamp = Options.StartDate.AddSeconds(-10)
                        };
                        forceEventsForAllLanes.Add(tempEvent1);
                        var tempEvent2 = new Controller_Event_Log()
                        {
                            SignalID = Options.SignalID,
                            EventCode = 81,
                            EventParam = detector.DetChannel,
                            Timestamp = Options.StartDate.AddSeconds(-9)
                        };
                        forceEventsForAllLanes.Add(tempEvent2);
                        AdvancePresenceEvents.Add("Advanced Presence, ch " + detector.DetChannel + " " +
                                                  detector.MovementType.Abbreviation + " " +
                                                  laneNumber, forceEventsForAllLanes);
                    }
                }
            }
        }

        private void GetPedestrianEvents()
        {
            var controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();
            PedestrianEvents = controllerEventLogRepository.GetEventsByEventCodesParam(Options.SignalID,
                Options.StartDate, Options.EndDate, new List<int> {89, 90}, PhaseNumber);
        }

        public void GetPedestrianIntervals(bool phaseOrOverlap)
        {
            var overlapCodes = new List<int> { 67, 68, 69 };
            if (phaseOrOverlap)
            {
                overlapCodes = new List<int> { 21, 22, 23 };
            }
            var expandTimeLine = 6;
            var controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();
            PedestrianIntervals = controllerEventLogRepository.GetEventsByEventCodesParam(Options.SignalID,
                Options.StartDate.AddMinutes(- expandTimeLine), Options.EndDate.AddMinutes(expandTimeLine),overlapCodes,
                PhaseNumber);
        }
    }
}

