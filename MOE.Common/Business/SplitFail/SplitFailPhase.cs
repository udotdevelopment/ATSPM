using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Business.CustomReport;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.SplitFail
{
    public class SplitFailPhase
    {
        public List<DateTime> SplitFails { get; }
        public int TotalFails { get; set; } = 0;
        public int PhaseNumber { get; }
        public Approach Approach { get; }
        public bool GetPermissivePhase { get; }
        public List<CycleSplitFail> Cycles { get; }
        public List<PlanSplitFail> Plans { get; }
        public List<Tuple<DateTime, double>> GorGapOut { get; }
        public List<Tuple<DateTime, double>> RorGapOut { get; }
        public List<Tuple<DateTime, double>> GorForceOff { get; }
        public List<Tuple<DateTime, double>> RorForceOff { get; }
        public List<Tuple<DateTime, double>> PercentFails { get; }
        public List<Tuple<DateTime, double>> AverageRors { get; }
        public List<Tuple<DateTime, double>> AverageGors { get; }
        public Dictionary<string, string> Statistics { get; }
        private List<Tuple<int, List<Controller_Event_Log>>> _controllerEventLogs { get; }
        private List<SplitFailDetectorActivation> _detectorActivations = new List<SplitFailDetectorActivation>();

        public SplitFailPhase(Approach approach, SplitFailOptions options, bool getPermissivePhase)
        {
            Approach = approach;
            GetPermissivePhase = getPermissivePhase;
            //SetControllerEventLogs(phaseNumber, approach, options);
            SetDetectorActivations(options);
            CombineDetectorActivations();
            Cycles = CycleFactory.GetSplitFailCycles(options, approach, getPermissivePhase, _detectorActivations);
            Plans = PlanFactory.GetSplitFailPlans(Cycles, options, Approach);
            _controllerEventLogs = new List<Tuple<int, List<Controller_Event_Log>>>();
            RorGapOut = new List<Tuple<DateTime, double>>();
            GorGapOut = new List<Tuple<DateTime, double>>();
            GorForceOff = new List<Tuple<DateTime, double>>();
            RorForceOff = new List<Tuple<DateTime, double>>();
            PercentFails = new List<Tuple<DateTime, double>>();
            AverageGors = new List<Tuple<DateTime, double>>();
            AverageRors = new List<Tuple<DateTime, double>>();
            Statistics = new Dictionary<string, string>();
            SplitFails = new List<DateTime>();
            ProcessCycles(options);
        }

        public SplitFailPhase()
        {
            _controllerEventLogs = new List<Tuple<int, List<Controller_Event_Log>>>();
            RorGapOut = new List<Tuple<DateTime, double>>();
            GorGapOut = new List<Tuple<DateTime, double>>();
            GorForceOff = new List<Tuple<DateTime, double>>();
            RorForceOff = new List<Tuple<DateTime, double>>();
            PercentFails = new List<Tuple<DateTime, double>>();
            AverageGors = new List<Tuple<DateTime, double>>();
            AverageRors = new List<Tuple<DateTime, double>>();
            Statistics = new Dictionary<string, string>();
            SplitFails = new List<DateTime>();
        }

        private void ProcessCycles(SplitFailOptions options)
        {
            if (_controllerEventLogs.Count > 0)
            {
                foreach (CycleSplitFail cycle in Cycles)
                {
                    //ProcessDetectors(cycle);
                    //CombineDetectorActivations(cycle);
                    SetRorGor(options, cycle);
                }
                Statistics.Add("Total Split Failures ", TotalFails.ToString());
                SetBinStatistics(options);
            }
        }

        private void SetBinStatistics(SplitFailOptions options)
        {
            DateTime counterTime = options.StartDate;
            do
            {
                double binTotalGor = 0;
                double binTotalRor = 0;
                List<CycleSplitFail> cycleBin = GetCycleBin(counterTime);
                var binFails = SplitFails.Count(s => s >= counterTime && s <= counterTime.AddMinutes(15));
                SetBinTotalRorGor(options, ref binTotalGor, ref binTotalRor, cycleBin);
                SetPercentFails(counterTime, cycleBin, binFails);
                SetAverageRorGorForCycleBin(counterTime, binTotalGor, binTotalRor, cycleBin);
                counterTime = counterTime.AddMinutes(15);
            } while (counterTime < options.EndDate.AddMinutes(15));
        }

        private void SetAverageRorGorForCycleBin(DateTime counterTime, double binTotalGor, double binTotalRor, List<CycleSplitFail> cycleBin)
        {
            if (cycleBin.Any())
            {
                double avggor = binTotalGor / cycleBin.Count();
                double avgror = binTotalRor / cycleBin.Count();
                AverageGors.Add(new Tuple<DateTime, double>(counterTime, avggor));
                AverageRors.Add(new Tuple<DateTime, double>(counterTime, avgror));
            }
            else
            {
                AverageGors.Add(new Tuple<DateTime, double>(counterTime, 0));
                AverageRors.Add(new Tuple<DateTime, double>(counterTime, 0));
            }
        }

        private void SetPercentFails(DateTime counterTime, List<CycleSplitFail> cycleBin, int binFails)
        {
            if (binFails > 0 && cycleBin.Any())
            {
                double binFailPercent = (binFails / Convert.ToDouble(cycleBin.Count()));
                PercentFails.Add(new Tuple<DateTime, double>(counterTime, Convert.ToInt32(binFailPercent * 100)));
            }
            else
            {
                PercentFails.Add(new Tuple<DateTime, double>(counterTime, 0));
            }
        }

        private static void SetBinTotalRorGor(SplitFailOptions options, ref double binTotalGor, ref double binTotalRor, List<CycleSplitFail> cycleBin)
        {
            foreach (CycleSplitFail cycle in cycleBin)
            {
                binTotalGor += cycle.GreenOccupancy * 100;
                binTotalRor += cycle.RedOccupancy * 100;
            }
        }

        private void SetRorGor(SplitFailOptions options, CycleSplitFail cycle)
        {
            if (cycle.StartTime > options.StartDate && cycle.StartTime < options.EndDate)
            {
                double gor = cycle.GreenOccupancy * 100;
                double ror = cycle.RedOccupancy * 100;
                if (cycle.TerminationEvent == CycleSplitFail.TerminationType.GapOut)
                {
                    GorGapOut.Add(new Tuple<DateTime, double>(cycle.StartTime, gor));
                    RorGapOut.Add(new Tuple<DateTime, double>(cycle.StartTime, ror));
                }
                else
                {
                    GorForceOff.Add(new Tuple<DateTime, double>(cycle.StartTime, gor));
                    RorForceOff.Add(new Tuple<DateTime, double>(cycle.StartTime, ror));
                }
                if ((gor > 79 && ror > 79))
                {
                    SplitFails.Add(cycle.StartTime);
                    TotalFails++;
                }
            }
        }

        private List<CycleSplitFail> GetCycleBin(DateTime counterTime)
        {
            return Cycles.Where(cur => cur.StartTime >= counterTime && cur.StartTime <= counterTime.AddMinutes(15))
                .OrderBy(cur => cur.StartTime)
                .ToList();
        }

        

        private void CombineDetectorActivations()
        {
            for (int i = 0; i < _detectorActivations.Count - 1;)
            {
                SplitFailDetectorActivation current = _detectorActivations[i];
                SplitFailDetectorActivation next = _detectorActivations[i+1];
                //if the next activaiton is between the previos one, remove the nextone and start again.
                if (next.DetectorOn >= current.DetectorOn && next.DetectorOff <= current.DetectorOff)
                {
                    _detectorActivations.RemoveAt(i + 1);
                }
                //if the next activaiton starts during the current, but ends later, atler current end time, and remove next, and start over. 
                else if (next.DetectorOn >= current.DetectorOn && next.DetectorOn < current.DetectorOff &&
                         next.DetectorOff > current.DetectorOff)
                {
                    current.DetectorOff = next.DetectorOff;
                    _detectorActivations.RemoveAt(i + 1);
                }
                else
                {
                    i++;
                }
            }
        }

        //private void ProcessDetectors(CycleSplitFail cycle)
        //{
        //    foreach (var controllerEventLog in _controllerEventLogs)
        //    {
        //        var detectorHitsForCycle = controllerEventLog.Item2
        //            .Where(ce => ce.Timestamp >= cycle.StartTime && ce.Timestamp <= cycle.EndTime)
        //            .OrderBy(r => r.Timestamp)
        //            .ToList();
        //        if (detectorHitsForCycle.Count > 0)
        //        {
        //            detectorHitsForCycle = detectorHitsForCycle.OrderBy(r => r.Timestamp).ToList();
        //            if (detectorHitsForCycle.Count() > 1)
        //            {
        //                SetDetectorActivationsForCycleWithDetectorHits(cycle, detectorHitsForCycle);
        //            }
        //            else
        //            {
        //                SetDetectorActivationsForCycleWithLessThanOneDetectorHit(cycle, detectorHitsForCycle);
        //            }
        //        }
        //        //if there are no hits in the cycle, we need to determine if the a previous detector activaition lasts the entire cycle
        //        else if (detectorHitsForCycle.Count <= 0)
        //        {
        //            SetDetectorActivationsForCycleWithoutDetectorHits(cycle, controllerEventLog.Item1);
        //        }
        //    }
        //}

        //private void SetDetectorActivationsForCycleWithoutDetectorHits(CycleSplitFail cycle, int channel)
        //{
        //    SplitFailDetectorActivation splitFailDetectorActivation = new SplitFailDetectorActivation();
        //    DateTime earlierTime = cycle.StartTime.AddMinutes(-30);
        //    var controllerLogsRepository = ControllerEventLogRepositoryFactory.Create();
        //    var controllerEvents = controllerLogsRepository.GetSignalEventsByEventCodes(Approach.SignalID, earlierTime,
        //        cycle.StartTime, new List<int> {81, 82});
        //    //if the last detecotr eventCodes was ON, and there is no matching off event, assume the detector was on for the whole cycle
        //    if (cs.Events != null && cs.Events.Count > 0 && cs.Events.LastOrDefault().EventCode == 82)
        //    {
        //        splitFailDetectorActivation.DetectorOn = cycle.StartTime;
        //        splitFailDetectorActivation.DetectorOff = cycle.EndTime;
        //        cycle.Activations.AddActivation(splitFailDetectorActivation);
        //    }
        //}

        //private static void SetDetectorActivationsForCycleWithLessThanOneDetectorHit(CycleSplitFail cycle, List<Controller_Event_Log> detectorHitsForCycle)
        //{
        //    SplitFailDetectorActivation da = new SplitFailDetectorActivation();
        //    Controller_Event_Log current = detectorHitsForCycle.First();
        //    switch (current.EventCode)
        //    {
        //        //if the only event is off
        //        case 81:
        //            da.DetectorOn = cycle.StartTime;
        //            da.DetectorOff = current.Timestamp;
        //            cycle.Activations.AddActivation(da);
        //            break;
        //        //if the only event is on
        //        case 82:
        //            da.DetectorOn = current.Timestamp;
        //            da.DetectorOff = cycle.EndTime;
        //            cycle.Activations.AddActivation(da);
        //            break;
        //    }
        //}

        //private static void SetDetectorActivationsForCycleWithDetectorHits(CycleSplitFail cycle, List<Controller_Event_Log> detectorHitsForCycle)
        //{
        //    for (int i = 0; i < detectorHitsForCycle.Count() - 1; i++)
        //    {
        //        Controller_Event_Log current = detectorHitsForCycle.ElementAt(i);
        //        Controller_Event_Log next = detectorHitsForCycle.ElementAt(i + 1);
        //        if (current.Timestamp.Ticks == next.Timestamp.Ticks)
        //        {
        //            continue;
        //        }
        //        //If the first event is 'Off', then set 'On' to cyclestart
        //        if (i == 0 && current.EventCode == 81)
        //        {
        //            SplitFailDetectorActivation da =
        //                new SplitFailDetectorActivation
        //                {
        //                    DetectorOn = cycle.StartTime,
        //                    DetectorOff = current.Timestamp
        //                };
        //            cycle.Activations.AddActivation(da);
        //        }
        //        //This is the prefered sequence; an 'On'  followed by an 'off'
        //        if (current.EventCode == 82 && next.EventCode == 81)
        //        {
        //            SplitFailDetectorActivation da =
        //                new SplitFailDetectorActivation
        //                {
        //                    DetectorOn = current.Timestamp,
        //                    DetectorOff = next.Timestamp
        //                };
        //            cycle.Activations.AddActivation(da);
        //            continue;
        //        }
        //        //if we are at the penultimate event, and the last event is 'on', set 'off' as CycleEnd.
        //        if (i + 2 == detectorHitsForCycle.Count() && next.EventCode == 82)
        //        {
        //            SplitFailDetectorActivation da =
        //                new SplitFailDetectorActivation
        //                {
        //                    DetectorOn = next.Timestamp,
        //                    DetectorOff = cycle.EndTime
        //                };
        //            cycle.Activations.AddActivation(da);
        //            continue;
        //        }
        //    }
        //}

        private void SetDetectorActivations(SplitFailOptions options)
        {
            var controllerEventsRepository = ControllerEventLogRepositoryFactory.Create();
            int phaseNumber = GetPermissivePhase ? Approach.PermissivePhaseNumber.Value : Approach.ProtectedPhaseNumber;
            var detectors = Approach.Signal.GetDetectorsForSignalThatSupportAMetricByPhaseNumber(12, phaseNumber);
            foreach (var detector in detectors)
            {
                //var channelAndEventCodes = new Tuple<int, List<Controller_Event_Log>>(detector.DetChannel, 
                var events = controllerEventsRepository.GetEventsByEventCodesParam(Approach.SignalID,
                    options.StartDate, options.EndDate, new List<int>() {81, 82}, detector.DetChannel);
                for (int i = 0; i < events.Count - 2; i++)
                {
                    if (events[i].EventCode == 81 && events[i + 1].EventCode == 82)
                    {
                        _detectorActivations.Add(new SplitFailDetectorActivation{ DetectorOn = events[i].Timestamp, DetectorOff = events[i+1].Timestamp });
                    }
                }
                //_controllerEventLogs.Add(channelAndEventCodes);
            }
            
        }
    }
}

        
    
