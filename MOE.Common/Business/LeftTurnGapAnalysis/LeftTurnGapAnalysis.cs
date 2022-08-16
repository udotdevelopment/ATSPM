using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.LeftTurnGapAnalysis
{
    public class LeftTurnGapAnalysis
    {
        public const int EVENT_GREEN = 1;
        public const int EVENT_RED = 10;
        public const int EVENT_DET = 81;

        public Approach Approach { get; }
        public LeftTurnGapAnalysisOptions LeftTurnGapAnalysisOptions { get; set; }
        public List<KeyValuePair<DateTime, double>> PercentTurnableSeries { get; private set; }
        public List<KeyValuePair<DateTime, int>> Gaps1 { get; private set; }
        public List<KeyValuePair<DateTime, int>> Gaps2 { get; private set; }
        public List<KeyValuePair<DateTime, int>> Gaps3 { get; private set; }
        public List<KeyValuePair<DateTime, int>> Gaps4 { get; private set; }
        public List<KeyValuePair<DateTime, int>> Gaps5 { get; private set; }
        public List<KeyValuePair<DateTime, int>> Gaps6 { get; private set; }
        public List<KeyValuePair<DateTime, int>> Gaps7 { get; private set; }
        public List<KeyValuePair<DateTime, int>> Gaps8 { get; private set; }
        public List<KeyValuePair<DateTime, int>> Gaps9 { get; private set; }
        public List<KeyValuePair<DateTime, int>> Gaps10 { get; private set; }
        public List<KeyValuePair<DateTime, int>> Gaps11 { get; private set; }
        public double? SumDuration1 { get; private set; }
        public double? SumDuration2 { get; private set; }
        public double? SumDuration3 { get; private set; }
        public double SumGreenTime { get; private set; }
        public int HighestTotal { get; set; }
        public string DetectionTypeStr { get; set; }

        public LeftTurnGapAnalysis(Approach approach, ControllerEventLogs approachEvents, LeftTurnGapAnalysisOptions options)
        {
            Approach = approach;
            LeftTurnGapAnalysisOptions = options;
            SumGreenTime = 0;
            if (LeftTurnGapAnalysisOptions.SumDurationGap1.HasValue)
                SumDuration1 = 0;
            if (LeftTurnGapAnalysisOptions.SumDurationGap2.HasValue)
                SumDuration2 = 0;
            if (LeftTurnGapAnalysisOptions.SumDurationGap3.HasValue)
                SumDuration3 = 0;


            Gaps1 = new List<KeyValuePair<DateTime, int>>();
            Gaps2 = new List<KeyValuePair<DateTime, int>>();
            Gaps3 = new List<KeyValuePair<DateTime, int>>();
            Gaps4 = new List<KeyValuePair<DateTime, int>>();
            Gaps5 = new List<KeyValuePair<DateTime, int>>();
            Gaps6 = new List<KeyValuePair<DateTime, int>>();
            Gaps7 = new List<KeyValuePair<DateTime, int>>();
            Gaps8 = new List<KeyValuePair<DateTime, int>>();
            Gaps9 = new List<KeyValuePair<DateTime, int>>();
            Gaps10 = new List<KeyValuePair<DateTime, int>>();
            Gaps11 = new List<KeyValuePair<DateTime, int>>();
            GetLeftTurnData(approach, approachEvents);
        }


        private void GetLeftTurnData(Approach approach, ControllerEventLogs eventLogs)
        {
            var phaseEvents = new List<Controller_Event_Log>();

            phaseEvents.AddRange(eventLogs.Events.Where(x =>
                x.EventParam == approach.ProtectedPhaseNumber &&
                (x.EventCode == EVENT_GREEN || x.EventCode == EVENT_RED)));

            var detectorsToUse = new List<Models.Detector>();
            DetectionTypeStr = "Detector Type: Lane-By-Lane Count";

            //Use only lane-by-lane count detectors if they exists, otherwise check for stop bar
            detectorsToUse = approach.GetAllDetectorsOfDetectionType(4);

            if (!detectorsToUse.Any())
            {
                detectorsToUse = approach.GetAllDetectorsOfDetectionType(6);
                DetectionTypeStr = "Detector Type: Stop Bar Presence";

                //If no detectors of either type for this approach, skip it
                if (!detectorsToUse.Any())
                    return;
            }

            foreach (var detector in detectorsToUse)
            {
                // Check for thru, right, thru-right, and thru-left
                if (!IsThruDetector(detector)) continue;

                phaseEvents.AddRange(eventLogs.Events.Where(x =>
                    x.EventCode == EVENT_DET && x.EventParam == detector.DetChannel));
            }

            if (phaseEvents.Any())
            {
                GetData(phaseEvents);
                //var options = new LeftTurnGapAnalysisOptions(signal.SignalID, startDate, endDate, 1, 3.3, 3.3, 3.7, 3.7,
                //    7.4, 7.4, 7.4);
                //var leftTurnChart = new LeftTurnGapAnalysisChart(options, signal, approach, phaseEvents,
                //    GetOpposingPhase(approach.ProtectedPhaseNumber), startDate, endDate, detectionTypeStr);
            }
        }

        private bool IsThruDetector(Models.Detector detector)
        {
            return detector.MovementTypeID == 1 || detector.MovementTypeID == 2 ||
                   detector.MovementTypeID == 4 || detector.MovementTypeID == 5;
        }

        private int GetOpposingPhase(int phase)
        {
            switch (phase)
            {
                case 2:
                    return 6;
                case 4:
                    return 8;
                case 6:
                    return 2;
                case 8:
                    return 4;
                default:
                    return 0;
            }
        }

        protected void GetData(List<Controller_Event_Log> _events)
        {
            if(PercentTurnableSeries == null)
                PercentTurnableSeries = new List<KeyValuePair<DateTime, double>>();
            var greenList = _events.Where(x => x.EventCode == EVENT_GREEN && x.Timestamp >= LeftTurnGapAnalysisOptions.StartDate && x.Timestamp < LeftTurnGapAnalysisOptions.EndDate)
                .OrderBy(x => x.Timestamp).ToList();
            var redList = _events.Where(x => x.EventCode == EVENT_RED && x.Timestamp >= LeftTurnGapAnalysisOptions.StartDate && x.Timestamp < LeftTurnGapAnalysisOptions.EndDate)
                .OrderBy(x => x.Timestamp).ToList();
            var orderedDetectorCallList = _events.Where(x => x.EventCode == EVENT_DET && x.Timestamp >= LeftTurnGapAnalysisOptions.StartDate && x.Timestamp < LeftTurnGapAnalysisOptions.EndDate)
                .OrderBy(x => x.Timestamp).ToList();

            var eventBeforeStart = _events.Where(e => e.Timestamp < LeftTurnGapAnalysisOptions.StartDate && (e.EventCode == EVENT_GREEN || e.EventCode == EVENT_RED)).OrderByDescending(e => e.Timestamp).FirstOrDefault();
            if (eventBeforeStart != null && eventBeforeStart.EventCode == EVENT_GREEN)
            {
                eventBeforeStart.Timestamp = LeftTurnGapAnalysisOptions.StartDate;
                greenList.Insert(0, eventBeforeStart);
            }
            if (eventBeforeStart != null && eventBeforeStart.EventCode == EVENT_RED)
            {
                eventBeforeStart.Timestamp = LeftTurnGapAnalysisOptions.StartDate;
                redList.Insert(0, eventBeforeStart);
            }

            var eventAfterEnd = _events.Where(e => e.Timestamp > LeftTurnGapAnalysisOptions.EndDate && (e.EventCode == EVENT_GREEN || e.EventCode == EVENT_RED)).OrderBy(e => e.Timestamp).FirstOrDefault();
            if (eventAfterEnd != null && eventAfterEnd.EventCode == EVENT_GREEN)
            {
                eventAfterEnd.Timestamp = LeftTurnGapAnalysisOptions.EndDate;
                greenList.Add(eventAfterEnd);
            }
            if (eventAfterEnd != null && eventAfterEnd.EventCode == EVENT_RED)
            {
                eventAfterEnd.Timestamp = LeftTurnGapAnalysisOptions.EndDate;
                redList.Add(eventAfterEnd);
            }

            var phaseTrackerList = GetGapsFromControllerData(greenList, redList, orderedDetectorCallList);

            HighestTotal = 0;

            for (var lowerTimeLimit = LeftTurnGapAnalysisOptions.StartDate; lowerTimeLimit < LeftTurnGapAnalysisOptions.EndDate; lowerTimeLimit = lowerTimeLimit.AddMinutes(LeftTurnGapAnalysisOptions.BinSize))
            {
                var upperTimeLimit = lowerTimeLimit.AddMinutes(LeftTurnGapAnalysisOptions.BinSize);
                var items = phaseTrackerList.Where(x => x.GreenTime >= lowerTimeLimit && x.GreenTime < upperTimeLimit).ToList();

                if (!items.Any()) continue;
                Gaps1.Add(new KeyValuePair<DateTime, int>(upperTimeLimit, items.Sum(x => x.GapCounter1)));
                Gaps2.Add(new KeyValuePair<DateTime, int>(upperTimeLimit, items.Sum(x => x.GapCounter2)));
                Gaps3.Add(new KeyValuePair<DateTime, int>(upperTimeLimit, items.Sum(x => x.GapCounter3)));
                Gaps4.Add(new KeyValuePair<DateTime, int>(upperTimeLimit, items.Sum(x => x.GapCounter4)));
                var localTotal = items.Sum(x => x.GapCounter1) + items.Sum(x => x.GapCounter2)
                                                               + items.Sum(x => x.GapCounter3) +
                                                               items.Sum(x => x.GapCounter4);
                if (LeftTurnGapAnalysisOptions.Gap5Min.HasValue)
                {
                    int sum = items.Sum(x => x.GapCounter5);
                    Gaps5.Add(new KeyValuePair<DateTime, int>(upperTimeLimit, sum));
                    localTotal += sum;
                }
                if (LeftTurnGapAnalysisOptions.Gap6Min.HasValue)
                {
                    int sum = items.Sum(x => x.GapCounter6);
                    Gaps6.Add(new KeyValuePair<DateTime, int>(upperTimeLimit, sum));
                    localTotal += sum;
                }
                if (LeftTurnGapAnalysisOptions.Gap7Min.HasValue)
                {
                    int sum = items.Sum(x => x.GapCounter7);
                    Gaps7.Add(new KeyValuePair<DateTime, int>(upperTimeLimit, sum));
                    localTotal += sum;
                }
                if (LeftTurnGapAnalysisOptions.Gap8Min.HasValue)
                {
                    int sum = items.Sum(x => x.GapCounter8);
                    Gaps8.Add(new KeyValuePair<DateTime, int>(upperTimeLimit, sum));
                    localTotal += sum;
                }
                if (LeftTurnGapAnalysisOptions.Gap9Min.HasValue)
                {
                    int sum = items.Sum(x => x.GapCounter9);
                    Gaps9.Add(new KeyValuePair<DateTime, int>(upperTimeLimit, sum));
                    localTotal += sum;
                }
                if (LeftTurnGapAnalysisOptions.Gap10Min.HasValue)
                {
                    int sum = items.Sum(x => x.GapCounter10);
                    Gaps10.Add(new KeyValuePair<DateTime, int>(upperTimeLimit, sum));
                    localTotal += sum;
                }
                if (LeftTurnGapAnalysisOptions.Gap11Min.HasValue)
                {
                    int sum = items.Sum(x => x.GapCounter11);
                    Gaps11.Add(new KeyValuePair<DateTime, int>(upperTimeLimit, sum));
                    localTotal += sum;
                }
                PercentTurnableSeries.Add(new KeyValuePair<DateTime, double>(upperTimeLimit, items.Average(x => x.PercentPhaseTurnable) * 100));

                if (localTotal > HighestTotal)
                    HighestTotal = localTotal;
            }

        }

        public void AddGapToCounters(LeftTurnGapAnalysisChart.PhaseLeftTurnGapTracker phaseTracker, double gap)
        {
            if (gap > LeftTurnGapAnalysisOptions.Gap1Min && gap <= LeftTurnGapAnalysisOptions.Gap1Max)
            {
                phaseTracker.GapCounter1++;
            }
            else if (gap > LeftTurnGapAnalysisOptions.Gap2Min && gap <= LeftTurnGapAnalysisOptions.Gap2Max)
            {
                phaseTracker.GapCounter2++;
            }
            else if (gap > LeftTurnGapAnalysisOptions.Gap3Min && gap <= LeftTurnGapAnalysisOptions.Gap3Max)
            {
                phaseTracker.GapCounter3++;
            }

            if (LeftTurnGapAnalysisOptions.Gap4Max == null)
            {
                if (gap > LeftTurnGapAnalysisOptions.Gap4Min)
                {
                    phaseTracker.GapCounter4++;
                }
            }
            else
            {
                if (gap > LeftTurnGapAnalysisOptions.Gap4Min && gap <= LeftTurnGapAnalysisOptions.Gap4Max.Value)
                {
                    phaseTracker.GapCounter4++;
                }
            }

            if (LeftTurnGapAnalysisOptions.Gap5Min != null)
            {
                if (LeftTurnGapAnalysisOptions.Gap5Max != null && gap > LeftTurnGapAnalysisOptions.Gap5Min.Value &&
                    gap > LeftTurnGapAnalysisOptions.Gap5Min && gap <= LeftTurnGapAnalysisOptions.Gap5Max.Value)
                {
                    phaseTracker.GapCounter5++;
                }
            }
            if (LeftTurnGapAnalysisOptions.Gap6Min != null)
            {
                if (LeftTurnGapAnalysisOptions.Gap6Max != null && gap > LeftTurnGapAnalysisOptions.Gap6Min.Value &&
                    gap > LeftTurnGapAnalysisOptions.Gap6Min && gap <= LeftTurnGapAnalysisOptions.Gap6Max.Value)
                {
                    phaseTracker.GapCounter6++;
                }
            }
            if (LeftTurnGapAnalysisOptions.Gap7Min != null)
            {
                if (LeftTurnGapAnalysisOptions.Gap7Max != null && gap > LeftTurnGapAnalysisOptions.Gap7Min.Value &&
                    gap > LeftTurnGapAnalysisOptions.Gap7Min && gap <= LeftTurnGapAnalysisOptions.Gap7Max.Value)
                {
                    phaseTracker.GapCounter7++;
                }
            }
            if (LeftTurnGapAnalysisOptions.Gap8Min != null)
            {
                if (LeftTurnGapAnalysisOptions.Gap8Max != null && gap > LeftTurnGapAnalysisOptions.Gap8Min.Value &&
                    gap > LeftTurnGapAnalysisOptions.Gap8Min && gap <= LeftTurnGapAnalysisOptions.Gap8Max.Value)
                {
                    phaseTracker.GapCounter8++;
                }
            }
            if (LeftTurnGapAnalysisOptions.Gap9Min != null)
            {
                if (LeftTurnGapAnalysisOptions.Gap9Max != null && gap > LeftTurnGapAnalysisOptions.Gap9Min.Value &&
                    gap > LeftTurnGapAnalysisOptions.Gap9Min && gap <= LeftTurnGapAnalysisOptions.Gap9Max.Value)
                {
                    phaseTracker.GapCounter9++;
                }
            }
            if (LeftTurnGapAnalysisOptions.Gap10Min != null)
            {
                if (LeftTurnGapAnalysisOptions.Gap10Max != null && gap > LeftTurnGapAnalysisOptions.Gap10Min.Value &&
                    gap > LeftTurnGapAnalysisOptions.Gap10Min && gap <= LeftTurnGapAnalysisOptions.Gap10Max.Value)
                {
                    phaseTracker.GapCounter10++;
                }
            }
            if (LeftTurnGapAnalysisOptions.Gap10Max != null && gap > LeftTurnGapAnalysisOptions.Gap10Max.Value )
            {
                phaseTracker.GapCounter11++;
            }

        }

        private List<LeftTurnGapAnalysisChart.PhaseLeftTurnGapTracker> GetGapsFromControllerData(List<Controller_Event_Log> greenList,
            List<Controller_Event_Log> redList, List<Controller_Event_Log> orderedDetectorCallList)
        {
            //List<Controller_Event_Log> tempGreenList = greenList.Where(g =>
            //        g.Timestamp >= LeftTurnGapAnalysisOptions.StartDate &&
            //        g.Timestamp < LeftTurnGapAnalysisOptions.EndDate)
            //    .ToList();

            //var firstGreen = greenList.Where(g => g.Timestamp < LeftTurnGapAnalysisOptions.StartDate)
            //    .OrderByDescending(g => g.Timestamp).FirstOrDefault();
            //if(firstGreen != null)
            //    tempGreenList.Insert(0, firstGreen);
            //var lastGreen = greenList.Where(g => g.Timestamp > LeftTurnGapAnalysisOptions.EndDate)
            //    .OrderBy(g => g.Timestamp).FirstOrDefault();
            //if (lastGreen != null)
            //    tempGreenList.Add(lastGreen);
            //greenList = tempGreenList;

            //List<Controller_Event_Log> tempRedList = redList.Where(g =>
            //        g.Timestamp >= LeftTurnGapAnalysisOptions.StartDate &&
            //        g.Timestamp < LeftTurnGapAnalysisOptions.EndDate)
            //    .ToList();
            //var firstRed = redList.Where(g => g.Timestamp < LeftTurnGapAnalysisOptions.StartDate)
            //    .OrderByDescending(g => g.Timestamp).FirstOrDefault();
            //if (firstRed != null)
            //    tempRedList.Insert(0, firstRed);
            //var lastRed = redList.Where(g => g.Timestamp > LeftTurnGapAnalysisOptions.EndDate)
            //    .OrderBy(g => g.Timestamp).FirstOrDefault();
            //if (lastRed != null)
            //    tempRedList.Add(lastRed);
            //redList = tempRedList;

            var phaseTrackerList = new List<LeftTurnGapAnalysisChart.PhaseLeftTurnGapTracker>();
            if (redList.Any() && greenList.Any())
            {
                foreach (var green in greenList)
                {
                    //Find the corresponding red
                    var red = redList.Where(x => x.Timestamp > green.Timestamp).OrderBy(x => x.Timestamp)
                        .FirstOrDefault();
                    if (red == null)
                        continue;

                    double trendLineGapTimeCounter = 0;

                    var phaseTracker = new LeftTurnGapAnalysisChart.PhaseLeftTurnGapTracker
                        {GreenTime = green.Timestamp};

                    var gapsList = new List<Controller_Event_Log>();
                    gapsList.Add(green);
                    gapsList.AddRange(orderedDetectorCallList.Where(x =>
                        x.Timestamp > green.Timestamp && x.Timestamp < red.Timestamp));
                    gapsList.Add(red);

                    for (var i = 1; i < gapsList.Count; i++)
                    {
                        var gap = gapsList[i].Timestamp.TimeOfDay.TotalSeconds -
                                  gapsList[i - 1].Timestamp.TimeOfDay.TotalSeconds;

                        if (gap < 0) continue;

                        AddGapToCounters(phaseTracker, gap);

                        if (gap >= LeftTurnGapAnalysisOptions.TrendLineGapThreshold)
                        {
                            trendLineGapTimeCounter += gap;
                        }

                        if (LeftTurnGapAnalysisOptions.SumDurationGap1.HasValue &&
                            gap >= LeftTurnGapAnalysisOptions.SumDurationGap1.Value)
                        {
                            SumDuration1 += gap;
                        }

                        if (LeftTurnGapAnalysisOptions.SumDurationGap2.HasValue &&
                            gap >= LeftTurnGapAnalysisOptions.SumDurationGap2.Value)
                        {
                            SumDuration2 += gap;
                        }

                        if (LeftTurnGapAnalysisOptions.SumDurationGap3.HasValue &&
                            gap >= LeftTurnGapAnalysisOptions.SumDurationGap3.Value)
                        {
                            SumDuration3 += gap;
                        }
                    }

                    //Decimal rounding errors can cause the number to be > 100
                    var percentTurnable =
                        Math.Min(trendLineGapTimeCounter / (red.Timestamp - green.Timestamp).TotalSeconds, 100);
                    SumGreenTime += (red.Timestamp - green.Timestamp).TotalSeconds;
                    phaseTracker.PercentPhaseTurnable = percentTurnable;

                    phaseTrackerList.Add(phaseTracker);
                }
            }

            return phaseTrackerList;
        }

    }
}
