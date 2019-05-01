using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public TimingAndActuationsForPhase(Approach approach, TimingAndActuationsOptions options,
            bool getPermissivePhase)
        {

            GetPermissivePhase = getPermissivePhase;
            Approach = approach;
            Options = options;
            PhaseNumber = GetPermissivePhase ? Approach.PermissivePhaseNumber.Value : Approach.ProtectedPhaseNumber;
            // Filter any events that don't fit in the order given
            //Cycles = CycleFactory.GetTimingAndActuationCycles(Options.StartDate.AddMinutes(-expandTimeLine),
            //    Options.EndDate.AddMinutes(expandTimeLine), approach, getPermissivePhase);
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
                GetPedestrianIntervals();
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

        private void GetAllCycleData(DateTime addMinutes, DateTime dateTime, Approach approach, bool getPermissivePhase)
        {
            CycleDataEventLogs = new List<Controller_Event_Log>();
            CycleAllEvents = new Dictionary<string, List<Controller_Event_Log>>();
            var extendLine = 6;
            var controllerEventLogRepository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            var simpleEndDate = Options.EndDate.AddMinutes(-1);
            string keyLabel = "Cycles Intervals";
            //.TotalMinutes;
            CycleDataEventLogs = controllerEventLogRepository.GetEventsByEventCodesParam(Approach.SignalID,
                Options.StartDate.AddMinutes(-extendLine), simpleEndDate.AddMinutes(extendLine),
                new List<int> {1, 3, 8, 9, 11, 61, 63, 64, 65}, PhaseNumber);
            if (CycleDataEventLogs.Count <= 0)
            {
                extendLine = 60;
                CycleDataEventLogs = controllerEventLogRepository.GetEventsByEventCodesParam(Approach.SignalID,
                    Options.StartDate.AddMinutes(-extendLine), simpleEndDate.AddMinutes(extendLine),
                    new List<int> {1, 3, 8, 9, 11, 61, 63, 64, 65}, PhaseNumber);
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
            var controllerEventLogRepository =
                Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            var localSortedDetectors = Approach.Detectors.OrderByDescending(d => d.MovementType.DisplayOrder)
                .ThenByDescending(l => l.LaneNumber).ToList();
            foreach (var detector in localSortedDetectors)
            {
                if (detector.DetectionTypes.Any(d => d.DetectionTypeID == 4))
                {
                    var laneByLane = controllerEventLogRepository.GetEventsByEventCodesParam(Approach.SignalID,
                        Options.StartDate, Options.EndDate, new List<int> {81, 82}, detector.DetChannel);
                    if (detector.LaneNumber != null && laneByLane.Count > 0)
                    {
                        LaneByLanes.Add("Lane-by-lane Count " + detector.MovementType.Abbreviation + " " +
                                        detector.LaneNumber.Value + ", ch" + detector.DetChannel, laneByLane);
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
                                        detector.LaneNumber.Value, forceEventsForAllLanes);
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
                    if (detector.LaneNumber != null && stopEvents.Count > 0)
                    {
                        StopBarEvents.Add("Stop Bar Presence, " + detector.MovementType.Abbreviation + " " +
                                          detector.LaneNumber.Value + ", ch" + detector.DetChannel, stopEvents);
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
                        StopBarEvents.Add("Stop Bar Presence, " + ", ch" + detector.DetChannel +
                                          detector.MovementType.Abbreviation + " " +
                                          detector.LaneNumber.Value, forceEventsForAllLanes);
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
                    if (detector.LaneNumber != null)
                    {
                        if (advanceEvents.Count > 0)
                        {
                            AdvanceCountEvents.Add("Advance Count (" + detector.DistanceFromStopBar + " ft) " +
                                                   detector.MovementType.Abbreviation + " " +
                                                   detector.LaneNumber.Value +
                                                   ", ch" + detector.DetChannel, advanceEvents);
                        }
                    }

                    if (AdvanceCountEvents.Count == 0 && Options.ShowAllLanesInfo)
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
                        AdvanceCountEvents.Add("Advance Count (" + detector.DistanceFromStopBar + " ft), ch" +
                                               detector.DetChannel + " " + detector.MovementType.Abbreviation + " " +
                                               detector.LaneNumber.Value, forceEventsForAllLanes);
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
                    if (detector.LaneNumber != null && advancePresence.Count > 0)
                    {
                        AdvancePresenceEvents.Add("Advanced Presence, " + detector.MovementType.Abbreviation + " " +
                                                  detector.LaneNumber.Value + ", ch" + detector.DetChannel, advancePresence);
                    }
                }

                if (AdvancePresenceEvents.Count == 0 && Options.ShowAllLanesInfo)
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
                    AdvancePresenceEvents.Add("Advanced Presence, ch" + detector.DetChannel +
                                              detector.MovementType.Abbreviation + " " +
                                              detector.LaneNumber.Value, forceEventsForAllLanes);
                }
            }
        }

        private void GetPedestrianEvents()
        {
            var controllerEventLogRepository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            PedestrianEvents = controllerEventLogRepository.GetEventsByEventCodesParam(Approach.SignalID,
                Options.StartDate, Options.EndDate, new List<int> {89, 90}, PhaseNumber);
        }

        private void GetPedestrianIntervals()
        {
            var expandTimeLine = 6;
            var controllerEventLogRepository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            PedestrianIntervals = controllerEventLogRepository.GetEventsByEventCodesParam(Approach.SignalID,
                Options.StartDate.AddMinutes(- expandTimeLine), Options.EndDate.AddMinutes(expandTimeLine), new List<int> {21, 22, 23},
                PhaseNumber);
            if (PedestrianIntervals.Count <= 0)
            {
                expandTimeLine = 60;
                PedestrianIntervals = controllerEventLogRepository.GetEventsByEventCodesParam(Approach.SignalID,
                    Options.StartDate.AddMinutes(-expandTimeLine), Options.EndDate.AddMinutes(expandTimeLine), new List<int> { 21, 22, 23 },
                    PhaseNumber);
            }
        }
    }
}

