using System.Collections.Generic;
using System.Linq;
using CsvHelper.Configuration.Attributes;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

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
        public List<Controller_Event_Log> PedestrianEvents { get; set; }
        public List<Controller_Event_Log> ForceEventsForAllLanes { get; set; }
        public List<Controller_Event_Log> PedestrianIntervals { get; set; }
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
            //Plans = plans;
            Options = options;
            if (Approach.PermissivePhaseNumber != null)
                PhaseNumber = getPermissivePhase ? Approach.PermissivePhaseNumber.Value : Approach.ProtectedPhaseNumber;
            var expandTimeLine = 30;
            //var reportTimespan = Options.EndDate - Options.StartDate;
            //var totalMinutesRounded = Math.Round(reportTimespan.TotalMinutes);
            //if (totalMinutesRounded <= 5.0)
            //{
            //    expandTimeLine = 15;
            //}
            //else if (totalMinutesRounded <= 10.0)
            //{
            //    expandTimeLine = 6;
            //}
            //else if (totalMinutesRounded <= 20.0)
            //{
            //    expandTimeLine = 8;
            //}
            //else if (totalMinutesRounded <= 60.0)
            //{
            //    expandTimeLine = 12;
            //}
            //else
            //{
            //    expandTimeLine = 20;
            //}
            Cycles = CycleFactory.GetTimingAndActuationCycles(Options.StartDate.AddMinutes(-expandTimeLine),
                        Options.EndDate.AddMinutes( expandTimeLine), approach, getPermissivePhase);
            GetPedestrianEvents();
            GetStopBarPresenceEvents();
            GetLaneByLaneEvents();
            GetAdvancePresenceEvents();
            GetAdvanceCountEvents();
            GetPedestrianIntervals();
            GetPhaseCustomEvents();
        }

        private void GetPhaseCustomEvents()
        {
            PhaseCustomEvents = new Dictionary<string, List<Controller_Event_Log>>();
            var phaseCustomCode1 = 0;
            if (Options.PhaseCustomCode1.HasValue)
            {
                phaseCustomCode1 = Options.PhaseCustomCode1.Value;
                GetPhaseCustomAllEvents(phaseCustomCode1);
            }
            if (Options.PhaseCustomCode2.HasValue)
            {
                var phaseCustomCode2 = Options.PhaseCustomCode2.Value;
                if (Options.PhaseCustomCode1.HasValue && phaseCustomCode1 != phaseCustomCode2)
                {
                    GetPhaseCustomAllEvents(phaseCustomCode2);
                }
            }
        }
    
        private void GetPhaseCustomAllEvents(int phaseCode)
        {
            var controllerEventLogRepository =
                Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            //Approach.Detectors.LaneNumber.OrderBy()
            foreach (var detector in Approach.Detectors)
            {
                var phaseEvents = controllerEventLogRepository.GetEventsByEventCodesParam(Approach.SignalID,
                    Options.StartDate, Options.EndDate, new List<int> { phaseCode }, PhaseNumber);
                if (phaseEvents.Count > 0)
                {
                    if (detector.LaneNumber != null)
                        PhaseCustomEvents.Add("Phase Custom Events: " + phaseCode + " " +
                                              detector.MovementType.Abbreviation +
                                              " " + detector.LaneNumber.Value + ", Ch " + detector.DetChannel + " Ph " +
                                              PhaseNumber, phaseEvents);
                }
            }
        }

        private void GetLaneByLaneEvents()
        {
            LaneByLanes = new Dictionary<string, List<Controller_Event_Log>>();
            var controllerEventLogRepository =
                Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            foreach (var detector in Approach.Detectors)
            {

                if (detector != null && detector.DetectionTypes.All(d => d.DetectionTypeID != 5)) continue;
                if (detector != null)
                {
                    var laneByLane = controllerEventLogRepository.GetEventsByEventCodesParam(Approach.SignalID,
                        Options.StartDate, Options.EndDate, new List<int> {81, 82}, detector.DetChannel);
                    if (detector.LaneNumber != null)
                        LaneByLanes.Add("Lane-by-Lane Count " + detector.MovementType.Abbreviation + " " +
                                        detector.LaneNumber.Value + ", ch" + detector.DetChannel, laneByLane);
                }
            }
        }

        private void GetStopBarPresenceEvents()
        {
            StopBarEvents = new Dictionary<string, List<Controller_Event_Log>>();
            var controllerEventLogRepository =
                Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            foreach (var detector in Approach.Detectors)
            {
                if (detector != null && detector.DetectionTypes.All(d => d.DetectionTypeID != 6)) continue;
                if (detector != null)
                {
                    var stopEvents = controllerEventLogRepository.GetEventsByEventCodesParam(Approach.SignalID,
                        Options.StartDate, Options.EndDate, new List<int> {81, 82}, detector.DetChannel);
                    if (detector.LaneNumber != null)
                    {
                        StopBarEvents.Add("Stop Bar Presence, " + detector.MovementType.Abbreviation + " " +
                                          detector.LaneNumber.Value + ", ch" + detector.DetChannel, stopEvents);
                    }
                    if (stopEvents.Count != 0) continue;
                    //if (!Options.ShowAllLanes) continue;
                    ForceEventsForAllLanes[0].SignalID = Options.SignalID;
                    ForceEventsForAllLanes[0].EventCode = 82;
                    ForceEventsForAllLanes[0].EventParam = -1;
                    ForceEventsForAllLanes[0].Timestamp = Options.StartDate.AddMinutes(-10);
                    ForceEventsForAllLanes[1].SignalID = Options.SignalID;
                    ForceEventsForAllLanes[1].EventCode = 82;
                    ForceEventsForAllLanes[1].EventParam = -1;
                    ForceEventsForAllLanes[1].Timestamp = Options.StartDate.AddMinutes(-9);

                    StopBarEvents.Add("Stop Bar Presence, for lane Number" + detector.LaneNumber.Value, ForceEventsForAllLanes);
                }
            }
        }
        
        private void GetAdvanceCountEvents()
        {
            AdvanceCountEvents = new Dictionary<string, List<Controller_Event_Log>>();
            var controllerEventLogRepository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            foreach(var detector in Approach.Detectors)
            {
                if (detector != null && detector.DetectionTypes.All(d => d.DetectionTypeID != 2)) continue;
                if (detector != null)
                {
                    var advanceEvents = controllerEventLogRepository.GetEventsByEventCodesParam(Approach.SignalID,
                        Options.StartDate, Options.EndDate, new List<int> { 81, 82 }, detector.DetChannel);
                    if (detector.LaneNumber != null)
                        AdvanceCountEvents.Add("Advance Count (" + detector.DistanceFromStopBar + " ft) " +
                                               detector.MovementType.Abbreviation + " " +
                                               detector.LaneNumber.Value + ", ch" + detector.DetChannel, advanceEvents);
                }
            }
        }

        private void GetAdvancePresenceEvents()
        {
            
            AdvancePresenceEvents = new Dictionary<string, List<Controller_Event_Log>>();
            var controllerEventLogRepository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            foreach (var detector in Approach.Detectors)
            {
                if (detector != null && detector.DetectionTypes.All(d => d.DetectionTypeID != 7)) continue;
                if (detector != null)
                {
                    var advancedEvents = controllerEventLogRepository.GetEventsByEventCodesParam(Approach.SignalID,
                        Options.StartDate, Options.EndDate, new List<int> {81, 82}, detector.DetChannel);
                    if (detector.LaneNumber != null)
                        AdvancePresenceEvents.Add("Advance Presence, " + detector.MovementType.Abbreviation + " " +
                                                  detector.LaneNumber.Value + ", ch" + detector.DetChannel,
                            advancedEvents);
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
            var controllerEventLogRepository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            PedestrianIntervals = controllerEventLogRepository.GetEventsByEventCodesParam(Approach.SignalID,
                Options.StartDate.AddMinutes(-40), Options.EndDate.AddMinutes(40), new List<int> {21, 22, 23}, PhaseNumber);
        }

        internal class GlobalCustomEvents
        {
        }
    }
}
