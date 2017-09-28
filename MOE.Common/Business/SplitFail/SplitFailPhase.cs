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
        public MOE.Common.Business.CustomReport.Phase Phase { get; }
        public List<Tuple<DateTime, double>> GorGapOut { get; }
        public List<Tuple<DateTime, double>> RorGapOut { get; }
        public List<Tuple<DateTime, double>> GorForceOff { get; }
        public List<Tuple<DateTime, double>> RorForceOff { get; }
        public List<Tuple<DateTime, double>> PercentFails { get; }
        public List<Tuple<DateTime, double>> AverageRors { get; }
        public List<Tuple<DateTime, double>> AverageGors { get; }
        public Dictionary<string, string> Statistics { get; }
        List<Tuple<int, List<MOE.Common.Models.Controller_Event_Log>>> _controllerEventLogs;


        public SplitFailPhase(int phaseNumber, MOE.Common.Models.Approach approach, SplitFailOptions options,
            Phase phase)
        {
            PhaseNumber = phaseNumber;
            Phase = phase;
            _controllerEventLogs = new List<Tuple<int, List<MOE.Common.Models.Controller_Event_Log>>>();
            RorGapOut = new List<Tuple<DateTime, double>>();
            GorGapOut = new List<Tuple<DateTime, double>>();
            GorForceOff = new List<Tuple<DateTime, double>>();
            RorForceOff = new List<Tuple<DateTime, double>>();
            PercentFails = new List<Tuple<DateTime, double>>();
            AverageGors = new List<Tuple<DateTime, double>>();
            AverageRors = new List<Tuple<DateTime, double>>();
            Statistics = new Dictionary<string, string>();
            SplitFails = new List<DateTime>();
            SetControllerEventLogs(phaseNumber, approach, options);
            ProcessCycles(options);
        }

        public SplitFailPhase()
        {
            _controllerEventLogs = new List<Tuple<int, List<MOE.Common.Models.Controller_Event_Log>>>();
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
                foreach (MOE.Common.Business.CustomReport.Cycle cycle in Phase.Cycles)
                {
                    ProcessDetectors(cycle);
                    CheckActivations(cycle);
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
                List<CustomReport.Cycle> cycleBin = GetCycleBin(counterTime);
                var binFails = SplitFails.Count(s => s >= counterTime && s <= counterTime.AddMinutes(15));
                SetBinTotalRorGor(options, ref binTotalGor, ref binTotalRor, cycleBin);
                SetPercentFails(counterTime, cycleBin, binFails);
                SetAverageRorGorForCycleBin(counterTime, binTotalGor, binTotalRor, cycleBin);
                counterTime = counterTime.AddMinutes(15);
            } while (counterTime < options.EndDate.AddMinutes(15));
        }

        private void SetAverageRorGorForCycleBin(DateTime counterTime, double binTotalGor, double binTotalRor, List<CustomReport.Cycle> cycleBin)
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

        private void SetPercentFails(DateTime counterTime, List<CustomReport.Cycle> cycleBin, int binFails)
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

        private static void SetBinTotalRorGor(SplitFailOptions options, ref double binTotalGor, ref double binTotalRor, List<CustomReport.Cycle> cycleBin)
        {
            foreach (MOE.Common.Business.CustomReport.Cycle c in cycleBin)
            {
                binTotalGor += c.Activations.GreenOccupancy(c) * 100;
                binTotalRor += c.Activations.StartOfRedOccupancy(c, options.FirstSecondsOfRed) * 100;
            }
        }

        private List<CustomReport.Cycle> GetCycleBin(DateTime counterTime)
        {
            return Phase.Cycles
                .Where(cur => cur.CycleStart >= counterTime && cur.CycleStart <= counterTime.AddMinutes(15))
                .OrderBy(cur => cur.CycleStart)
                .ToList();
        }

        private void SetRorGor(SplitFailOptions options, CustomReport.Cycle cycle)
        {
            if (cycle.CycleStart > options.StartDate && cycle.CycleStart < options.EndDate)
            {
                double gor = cycle.Activations.GreenOccupancy(cycle) * 100;
                double ror = cycle.Activations.StartOfRedOccupancy(cycle, options.FirstSecondsOfRed) * 100;
                if (cycle.TerminationEvent == MOE.Common.Business.CustomReport.Cycle.TerminationCause.GapOut)
                {
                    GorGapOut.Add(new Tuple<DateTime, double>(cycle.CycleStart, gor));
                    RorGapOut.Add(new Tuple<DateTime, double>(cycle.CycleStart, ror));
                }
                else
                {
                    GorForceOff.Add(new Tuple<DateTime, double>(cycle.CycleStart, gor));
                    RorForceOff.Add(new Tuple<DateTime, double>(cycle.CycleStart, ror));
                }
                if ((gor > 79 && ror > 79))
                {
                    SplitFails.Add(cycle.CycleStart);
                    TotalFails++;
                }
            }
        }

        private static void CheckActivations(CustomReport.Cycle cycle)
        {
            for (int i = 0; i < cycle.Activations.Activations.Count - 1;)
            {
                SplitFailDetectorActivation current = cycle.Activations.Activations.ElementAt(i).Value;
                SplitFailDetectorActivation next = cycle.Activations.Activations.ElementAt(i + 1).Value;

                //if the next activaiton is between the previos one, remove the nextone and start again.
                if (next.DetectorOn >= current.DetectorOn && next.DetectorOff <= current.DetectorOff)
                {
                    cycle.Activations.Activations.RemoveAt(i + 1);
                    continue;
                }
                //if the next activaiton starts during the current, but ends later, atler current end time, and remove next, and start over. 
                else if (next.DetectorOn >= current.DetectorOn && next.DetectorOn < current.DetectorOff &&
                         next.DetectorOff > current.DetectorOff)
                {
                    current.DetectorOff = next.DetectorOff;
                    cycle.Activations.Activations.RemoveAt(i + 1);
                    continue;
                }
                else
                {
                    i++;
                }
            }
        }

        private void ProcessDetectors(CustomReport.Cycle cycle)
        {
            foreach (var controllerEventLog in _controllerEventLogs)
            {
                //int channel =  controllerEventLog.First().EventParam;
                var detectorHitsForCycle = controllerEventLog.Item2
                    .Where(ce => ce.Timestamp >= cycle.CycleStart && ce.Timestamp <= cycle.CycleEnd)
                    .OrderBy(r => r.Timestamp)
                    .ToList();
                if (detectorHitsForCycle.Count > 0)
                {
                    detectorHitsForCycle = detectorHitsForCycle.OrderBy(r => r.Timestamp).ToList();
                    if (detectorHitsForCycle.Count() > 1)
                    {
                        SetDetectorActivationsForCycleWithDetectorHits(cycle, detectorHitsForCycle);
                    }
                    else
                    {
                        SetDetectorActivationsForCycleWithLessThanOneDetectorHit(cycle, detectorHitsForCycle);
                    }
                }
                //if there are no hits in the cycle, we need to determine if the a previous detector activaition lasts the entire cycle
                else if (detectorHitsForCycle.Count <= 0)
                {
                    SetDetectorActivationsForCycleWithoutDetectorHits(cycle, controllerEventLog.Item1);
                }
            }
        }

        private void SetDetectorActivationsForCycleWithoutDetectorHits(CustomReport.Cycle cycle, int channel)
        {
            SplitFailDetectorActivation splitFailDetectorActivation = new SplitFailDetectorActivation();
            DateTime earlierTime = cycle.CycleStart.AddMinutes(-30);
            List<int> li = new List<int> { 81, 82 };
            ControllerEventLogs cs = new ControllerEventLogs(Phase.SignalID, earlierTime, cycle.CycleStart,
                channel, li);
            //if the last detecotr eventCodes was ON, and there is no matching off event, assume the detector was on for the whole cycle
            if (cs.Events != null && cs.Events.Count > 0 && cs.Events.LastOrDefault().EventCode == 82)
            {
                splitFailDetectorActivation.DetectorOn = cycle.CycleStart;
                splitFailDetectorActivation.DetectorOff = cycle.CycleEnd;
                cycle.Activations.AddActivation(splitFailDetectorActivation);
            }
        }

        private static void SetDetectorActivationsForCycleWithLessThanOneDetectorHit(CustomReport.Cycle cycle, List<Controller_Event_Log> detectorHitsForCycle)
        {
            SplitFailDetectorActivation da = new SplitFailDetectorActivation();
            MOE.Common.Models.Controller_Event_Log current = detectorHitsForCycle.First();
            switch (current.EventCode)
            {
                //if the only event is off
                case 81:
                    da.DetectorOn = cycle.CycleStart;
                    da.DetectorOff = current.Timestamp;
                    cycle.Activations.AddActivation(da);
                    break;
                //if the only event is on
                case 82:
                    da.DetectorOn = current.Timestamp;
                    da.DetectorOff = cycle.CycleEnd;
                    cycle.Activations.AddActivation(da);
                    break;
            }
        }

        private static void SetDetectorActivationsForCycleWithDetectorHits(CustomReport.Cycle cycle, List<Controller_Event_Log> detectorHitsForCycle)
        {
            for (int i = 0; i < detectorHitsForCycle.Count() - 1; i++)
            {
                MOE.Common.Models.Controller_Event_Log current = detectorHitsForCycle.ElementAt(i);
                MOE.Common.Models.Controller_Event_Log next = detectorHitsForCycle.ElementAt(i + 1);
                if (current.Timestamp.Ticks == next.Timestamp.Ticks)
                {
                    continue;
                }
                //If the first event is 'Off', then set 'On' to cyclestart
                if (i == 0 && current.EventCode == 81)
                {
                    SplitFailDetectorActivation da =
                        new SplitFailDetectorActivation
                        {
                            DetectorOn = cycle.CycleStart,
                            DetectorOff = current.Timestamp
                        };
                    cycle.Activations.AddActivation(da);
                }
                //This is the prefered sequence; an 'On'  followed by an 'off'
                if (current.EventCode == 82 && next.EventCode == 81)
                {
                    SplitFailDetectorActivation da =
                        new SplitFailDetectorActivation
                        {
                            DetectorOn = current.Timestamp,
                            DetectorOff = next.Timestamp
                        };
                    cycle.Activations.AddActivation(da);
                    continue;
                }
                //if we are at the penultimate event, and the last event is 'on', set 'off' as CycleEnd.
                if (i + 2 == detectorHitsForCycle.Count() && next.EventCode == 82)
                {
                    SplitFailDetectorActivation da =
                        new SplitFailDetectorActivation
                        {
                            DetectorOn = next.Timestamp,
                            DetectorOff = cycle.CycleEnd
                        };
                    cycle.Activations.AddActivation(da);
                    continue;
                }
            }
        }

        private void SetControllerEventLogs(int phaseNumber, Approach approach, SplitFailOptions options)
        {
            var controllerEventsRepository = ControllerEventLogRepositoryFactory.Create();
            var detectors = approach.Signal.GetDetectorsForSignalThatSupportAMetricByPhaseNumber(12, phaseNumber);
            foreach (var detector in detectors)
            {
                var channelAndEventCodes = new Tuple<int, List<Controller_Event_Log>>(detector.DetChannel, 
                    controllerEventsRepository.GetEventsByEventCodesParam(approach.SignalID,
                    options.StartDate, options.EndDate, new List<int>() { 81, 82 }, detector.DetChannel));
                _controllerEventLogs.Add(channelAndEventCodes);
            }
            
        }
    }
}

        
    
