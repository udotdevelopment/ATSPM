using System.Collections.Generic;
using System.Linq;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.SplitFail
{
    public class SplitFailPhase
    {
        private List<SplitFailDetectorActivation> _detectorActivations = new List<SplitFailDetectorActivation>();
        public List<SplitFailBin> Bins { get; } = new List<SplitFailBin>();
        public int TotalFails { get; }
        public Approach Approach { get; }
        public bool GetPermissivePhase { get; }
        public List<CycleSplitFail> Cycles { get; }
        public List<PlanSplitFail> Plans { get; }
        public Dictionary<string, string> Statistics { get; }
        public string PhaseNumberSort { get; set; }

        public SplitFailPhase(Approach approach, SplitFailOptions options, bool getPermissivePhase)
        {
            Approach = approach;
            GetPermissivePhase = getPermissivePhase;
            PhaseNumberSort = getPermissivePhase ? approach.PermissivePhaseNumber.Value.ToString()+"-1": approach.ProtectedPhaseNumber.ToString()+"-2";

            using (var db = new SPM())
            {
                Cycles = CycleFactory.GetSplitFailCycles(options, approach, getPermissivePhase, db);
                SetDetectorActivations(options, db);
                AddDetectorActivationsToCycles();
                Plans = PlanFactory.GetSplitFailPlans(Cycles, options, Approach, db);
            }
            TotalFails = Cycles.Count(c => c.IsSplitFail);
            Statistics = new Dictionary<string, string>();
            Statistics.Add("Total Split Failures", TotalFails.ToString());
            SetBinStatistics(options);
        }

        private void AddDetectorActivationsToCycles()
        {
            foreach (var cycleSplitFail in Cycles)
                cycleSplitFail.SetDetectorActivations(_detectorActivations);
        }

        private void SetBinStatistics(SplitFailOptions options)
        {
            var startTime = options.StartDate;
            do
            {
                var endTime = startTime.AddMinutes(15);
                var cycles = Cycles.Where(c => c.StartTime >= startTime && c.StartTime < endTime).ToList();
                Bins.Add(new SplitFailBin(startTime, endTime, cycles));
                startTime = startTime.AddMinutes(15);
            } while (startTime < options.EndDate);
        }

        private void CombineDetectorActivations()
        {
            var tempDetectorActivations = new List<SplitFailDetectorActivation>();

            //look at every item in the original list
            foreach (var current in _detectorActivations)
                if (!current.ReviewedForOverlap)
                {
                    var overlapingActivations = _detectorActivations.Where(d => d.ReviewedForOverlap == false &&
                        (
                            //   if it starts after current starts  and    starts before current ends      and    end after current ends   
                            d.DetectorOn >=
                            current.DetectorOn &&
                            d.DetectorOn <=
                            current.DetectorOff &&
                            d.DetectorOff >= current.DetectorOff
                            //OR if it starts BEFORE curent starts  and ends AFTER current starts           and ends BEFORE current ends
                            || d.DetectorOn <=
                            current.DetectorOn &&
                            d.DetectorOff >=
                            current.DetectorOn &&
                            d.DetectorOff <= current.DetectorOff
                            //OR if it starts AFTER current starts   and it ends BEFORE current ends
                            || d.DetectorOn >=
                            current.DetectorOn &&
                            d.DetectorOff <= current.DetectorOff
                            //OR if it starts BEFORE current starts  and it ens AFTER current ends 
                            || d.DetectorOn <=
                            current.DetectorOn &&
                            d.DetectorOff >= current.DetectorOff
                        )
                        //then add it to the overlap list
                    ).ToList();

                    //if there are any in the list (and here should be at least one that matches current)
                    if (overlapingActivations.Any())
                    {
                        //Then make a new activation that starts witht eh earliest start and ends with the latest end
                        var tempDetectorActivation = new SplitFailDetectorActivation
                        {
                            DetectorOn = overlapingActivations.Min(o => o.DetectorOn),
                            DetectorOff = overlapingActivations.Max(o => o.DetectorOff)
                        };
                        //and add the new one to a temp list
                        tempDetectorActivations.Add(tempDetectorActivation);

                        //mark everything in the  overlap list as Reviewed
                        foreach (var splitFailDetectorActivation in overlapingActivations)
                            splitFailDetectorActivation.ReviewedForOverlap = true;
                    }
                }

            //since we went through every item in the original list, if there were no overlaps, it shoudl equal the temp list
            if (_detectorActivations.Count != tempDetectorActivations.Count)
            {
                //if the counts do not match, we have to set the original list to the temp list and try the combinaitons again
                _detectorActivations = tempDetectorActivations;
                CombineDetectorActivations();
            }
        }

        private void SetDetectorActivations(SplitFailOptions options, SPM db)
        {
            var controllerEventsRepository = ControllerEventLogRepositoryFactory.Create(db);
            var phaseNumber = GetPermissivePhase ? Approach.PermissivePhaseNumber.Value : Approach.ProtectedPhaseNumber;
            var detectors = Approach.GetAllDetectorsOfDetectionType(6);// .GetDetectorsForMetricType(12);

            foreach (var detector in detectors)
            {
                //var lastCycle = Cycles.OrderBy(c => c.StartTime).LastOrDefault();
                List<Models.Controller_Event_Log> events = controllerEventsRepository.GetEventsByEventCodesParamWithLatencyCorrection(Approach.SignalID,
                    options.StartDate, options.EndDate, new List<int> {81, 82}, detector.DetChannel, detector.LatencyCorrection);
                if (!events.Any())
                {
                    CheckForDetectorOnBeforeStart(options, controllerEventsRepository, detector);
                }
                else
                {
                    AddDetectorOnToBeginningIfNecessary(options, detector, events);
                    AddDetectorOffToEndIfNecessary(options, detector, events);
                    AddDetectorActivationsFromList(events);
                }
            }
            CombineDetectorActivations();
        }

        private void AddDetectorActivationsFromList(List<Controller_Event_Log> events)
        {
            events = events.OrderBy(e => e.Timestamp).ToList();
            for (var i = 0; i < events.Count - 1; i++)
                if (events[i].EventCode == 82 && events[i + 1].EventCode == 81)
                    _detectorActivations.Add(new SplitFailDetectorActivation
                    {
                        DetectorOn = events[i].Timestamp,
                        DetectorOff = events[i + 1].Timestamp
                    });
        }

        private static void AddDetectorOffToEndIfNecessary(SplitFailOptions options, Models.Detector detector,
            List<Controller_Event_Log> events)
        {
            if (events.LastOrDefault()?.EventCode == 82)
                events.Insert(events.Count, new Controller_Event_Log
                {
                    Timestamp = options.EndDate,
                    EventCode = 81,
                    EventParam = detector.DetChannel,
                    SignalID = options.SignalID
                });
        }

        private static void AddDetectorOnToBeginningIfNecessary(SplitFailOptions options, Models.Detector detector,
            List<Controller_Event_Log> events)
        {
            if (events.FirstOrDefault()?.EventCode == 81)
                events.Insert(0, new Controller_Event_Log
                {
                    Timestamp = options.StartDate,
                    EventCode = 82,
                    EventParam = detector.DetChannel,
                    SignalID = options.SignalID
                });
        }

        private void CheckForDetectorOnBeforeStart(SplitFailOptions options,
            IControllerEventLogRepository controllerEventsRepository, Models.Detector detector)
        {
            var eventOnBeforeStart = controllerEventsRepository.GetFirstEventBeforeDateByEventCodeAndParameter(
                options.SignalID,
                detector.DetChannel, 81, options.StartDate);
            var eventOffBeforeStart = controllerEventsRepository.GetFirstEventBeforeDateByEventCodeAndParameter(
                options.SignalID,
                detector.DetChannel, 82, options.StartDate);
            if (eventOnBeforeStart != null && eventOffBeforeStart == null)
                _detectorActivations.Add(new SplitFailDetectorActivation
                {
                    DetectorOn = options.StartDate,
                    DetectorOff = options.EndDate
                });
        }
    }
}