using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business
{
    class WaitTimeChart
    {
        public Chart Chart;

        private readonly WaitTimeOptions _waitTimeOptions;
        private readonly Signal _signal;
        private readonly Approach _approach;
        private readonly IEnumerable<Controller_Event_Log> _phaseEvents;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;
        private readonly AnalysisPhase _phaseInfo;
        private readonly List<PlanSplitMonitor> _plans;

        public class WaitTimeTracker
        {
            public DateTime Time;
            public double WaitTimeSeconds;
        }

        public WaitTimeChart(WaitTimeOptions waitTimeOptions, Signal signal, Approach approach,
            IEnumerable<Controller_Event_Log> phaseEvents, DateTime startDate, DateTime endDate,
            AnalysisPhase phaseInfo, List<PlanSplitMonitor> plans)
        {
            _waitTimeOptions = waitTimeOptions;
            _signal = signal;
            _approach = approach;
            _phaseEvents = phaseEvents;
            _startDate = startDate;
            _endDate = endDate;
            _phaseInfo = phaseInfo;
            _plans = plans;

            string detectionTypesForApproach;
            var hasAdvanceDetection = approach.GetAllDetectorsOfDetectionType(2).Any();
            var hasStopBarDetection = approach.GetAllDetectorsOfDetectionType(6).Any();
            bool useDroppingAlgorithm;

            if (hasAdvanceDetection && hasStopBarDetection)
            {
                detectionTypesForApproach = "Advance + Stop Bar Detection";
                useDroppingAlgorithm = false;
            }
            else if (hasAdvanceDetection)
            {
                detectionTypesForApproach = "Advance Detection";
                useDroppingAlgorithm = false;
            }
            else if (hasStopBarDetection)
            {
                detectionTypesForApproach = "Stop Bar Detection";
                useDroppingAlgorithm = true;
            }
            else
                return;

            Chart = ChartFactory.CreateDefaultChartNoX2Axis(waitTimeOptions);
            SetChartTitle(Chart, waitTimeOptions, signal, approach, detectionTypesForApproach);
            ChartFactory.SetImageProperties(Chart);
            CreateLegend(Chart);

            Chart.ChartAreas[0].AxisY.Title = "Wait Time (s)";
            Chart.ChartAreas[0].AxisY.IntervalType = DateTimeIntervalType.Auto;
            Chart.ChartAreas[0].AxisY.Interval = 20;

            Chart.ChartAreas[0].AxisY2.Title = "Volume Per Hour";

            AddDataToChart(useDroppingAlgorithm);
        }

        private void SetChartTitle(Chart chart, WaitTimeOptions waitTimeOptions, Signal signal, Approach approach,
            string detectionTypes)
        {
            chart.Titles.Add(ChartTitleFactory.GetChartName(waitTimeOptions.MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(signal.SignalID, waitTimeOptions.StartDate,
                waitTimeOptions.EndDate));
            chart.Titles.Add(ChartTitleFactory.GetPhaseAndPhaseDescriptions(approach, false));
            chart.Titles.Add(detectionTypes);
        }

        private void CreateLegend(Chart chart)
        {
            var chartLegend = new Legend();
            chartLegend.Name = "Main Legend";
            chartLegend.Docking = Docking.Left;
            chart.Legends.Add(chartLegend);
        }

        protected void AddDataToChart(bool useDroppingAlgorithm)
        {
            var waitTimeSeries = _waitTimeOptions.CreateChartSeries(Color.Blue, SeriesChartType.Point,
                ChartValueType.DateTime, AxisType.Primary, "Wait Time (s)");
            _waitTimeOptions.CreateVolumeSeries(Chart);

            var redList = _phaseEvents.Where(x => x.EventCode == WaitTimeOptions.PHASE_END_RED_CLEARANCE)
                .OrderBy(x => x.Timestamp);
            var greenList = _phaseEvents.Where(x => x.EventCode == WaitTimeOptions.PHASE_BEGIN_GREEN)
                .OrderBy(x => x.Timestamp);
            var orderedPhaseRegisterList = _phaseEvents.Where(x =>
                x.EventCode == WaitTimeOptions.PHASE_CALL_REGISTERED ||
                x.EventCode == WaitTimeOptions.PHASE_CALL_DROPPED);

            var waitTimeTrackerList = new List<WaitTimeTracker>();

            Chart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            var GapoutSeries = new Series();
            GapoutSeries.ChartType = SeriesChartType.Point;
            GapoutSeries.Color = Color.OliveDrab;
            GapoutSeries.Name = "Gap Out";
            GapoutSeries.XValueType = ChartValueType.DateTime;
            GapoutSeries.MarkerStyle = MarkerStyle.Circle;
            GapoutSeries.MarkerSize = 4;

            var MaxOutSeries = new Series();
            MaxOutSeries.ChartType = SeriesChartType.Point;
            MaxOutSeries.Color = Color.Red;
            MaxOutSeries.Name = "Max Out";
            MaxOutSeries.XValueType = ChartValueType.DateTime;
            MaxOutSeries.MarkerStyle = MarkerStyle.Circle;
            MaxOutSeries.MarkerSize = 4;

            var ForceOffSeries = new Series();
            ForceOffSeries.ChartType = SeriesChartType.Point;
            ForceOffSeries.Color = Color.MediumBlue;
            ForceOffSeries.Name = "Force Off";
            ForceOffSeries.MarkerStyle = MarkerStyle.Circle;
            ForceOffSeries.MarkerSize = 4;

            var UnknownSeries = new Series();
            UnknownSeries.ChartType = SeriesChartType.Point;
            UnknownSeries.Color = Color.Black;
            UnknownSeries.Name = "Unknown";
            UnknownSeries.MarkerStyle = MarkerStyle.Circle;
            UnknownSeries.MarkerSize = 4;

            var ProgrammedSplitSeries = new Series();
            ProgrammedSplitSeries.ChartType = SeriesChartType.StepLine;
            ProgrammedSplitSeries.Color = Color.OrangeRed;
            ProgrammedSplitSeries.Name = "Programmed Split";
            ProgrammedSplitSeries.BorderWidth = 1;

            var AverageSeries = new Series();
            AverageSeries.ChartType = SeriesChartType.Line;
            AverageSeries.Color = Color.Magenta;
            AverageSeries.Name = "Average Wait";
            AverageSeries.BorderWidth = 2;

            try
            {
                foreach (var red in redList)
                {
                    //Find the corresponding green
                    var green = greenList.Where(x => x.Timestamp > red.Timestamp).OrderBy(x => x.Timestamp)
                        .FirstOrDefault();
                    if (green == null)
                        continue;

                    //Find all events between the red and green
                    var phaseCallList = orderedPhaseRegisterList
                        .Where(x => x.Timestamp >= red.Timestamp && x.Timestamp < green.Timestamp)
                        .OrderBy(x => x.Timestamp).ToList();

                    if (!phaseCallList.Any())
                        continue;

                    var exportList = new List<string>();
                    foreach (var row in phaseCallList)
                    {
                        exportList.Add($"{row.SignalID}, {row.Timestamp}, {row.EventCode}, {row.EventParam}");
                    }

                    WaitTimeTracker waitTimeTrackerToFill = null;
                    if (useDroppingAlgorithm &&
                        phaseCallList.Any(x => x.EventCode == WaitTimeOptions.PHASE_CALL_DROPPED))
                    {
                        var lastDroppedPhaseCall =
                            phaseCallList.LastOrDefault(x => x.EventCode == WaitTimeOptions.PHASE_CALL_DROPPED);
                        if (lastDroppedPhaseCall != null)
                        {
                            var lastIndex = phaseCallList.IndexOf(lastDroppedPhaseCall);
                            if (lastIndex + 1 >= phaseCallList.Count)
                                continue;
                            var nextPhaseCall = phaseCallList[lastIndex + 1];

                            waitTimeTrackerToFill = new WaitTimeTracker
                            {
                                Time = green.Timestamp,
                                WaitTimeSeconds = (green.Timestamp - nextPhaseCall.Timestamp).TotalSeconds
                            };
                        }
                    }
                    else
                    {
                        var firstPhaseCall = phaseCallList.First();
                        //waitTimeTrackerList.Add(new WaitTimeTracker { Time = green.Timestamp, WaitTimeSeconds = (green.Timestamp - firstPhaseCall.Timestamp).TotalSeconds });
                        waitTimeTrackerToFill = new WaitTimeTracker
                        {
                            Time = green.Timestamp,
                            WaitTimeSeconds = (green.Timestamp - firstPhaseCall.Timestamp).TotalSeconds
                        };
                    }

                    //Toss anything longer than 6 minutes - usually a bad value as a result of missing data
                    if (waitTimeTrackerToFill.WaitTimeSeconds > 360)
                        continue;

                    var priorPhase = _phaseInfo.Cycles.Items.FirstOrDefault(x => x.EndTime == red.Timestamp);
                    if (priorPhase != null)
                    {
                        waitTimeTrackerList.Add(waitTimeTrackerToFill);
                        switch (priorPhase.TerminationEvent)
                        {
                            case 4: //Gap Out
                                GapoutSeries.Points.AddXY(waitTimeTrackerToFill.Time,
                                    waitTimeTrackerToFill.WaitTimeSeconds);
                                break;
                            case 5: //Max Out
                                MaxOutSeries.Points.AddXY(waitTimeTrackerToFill.Time,
                                    waitTimeTrackerToFill.WaitTimeSeconds);
                                break;
                            case 6: //Force Off
                                ForceOffSeries.Points.AddXY(waitTimeTrackerToFill.Time,
                                    waitTimeTrackerToFill.WaitTimeSeconds);
                                break;
                            case 0:
                                UnknownSeries.Points.AddXY(waitTimeTrackerToFill.Time,
                                    waitTimeTrackerToFill.WaitTimeSeconds);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            try
            {
                if (!waitTimeTrackerList.Any())
                    return;
                
                for (var lowerTimeLimit = _startDate;
                    lowerTimeLimit < _endDate;
                    lowerTimeLimit = lowerTimeLimit.AddMinutes(15))
                {
                    var upperTimeLimit = lowerTimeLimit.AddMinutes(15);
                    var items = waitTimeTrackerList.Where(x => x.Time > lowerTimeLimit && x.Time < upperTimeLimit);
                    if (items.Any())
                    {
                        var avg = items.Average(x => x.WaitTimeSeconds);
                        AverageSeries.Points.AddXY(upperTimeLimit, avg);
                    }
                }

                //Round to the nearest minute
                Chart.ChartAreas[0].AxisY.Maximum =
                    Math.Ceiling(waitTimeTrackerList.Max(x => x.WaitTimeSeconds) / 60d) * 60;
                Chart.Series.Add(GapoutSeries);
                Chart.Series.Add(MaxOutSeries);
                Chart.Series.Add(ForceOffSeries);
                Chart.Series.Add(UnknownSeries);
                Chart.Series.Add(AverageSeries);

                var maxSplitLength = 0;
                foreach (var plan in _plans)
                {
                    var highestSplit = plan.FindHighestRecordedSplitPhase();
                    plan.FillMissingSplits(highestSplit);
                    try
                    {
                        ProgrammedSplitSeries.Points.AddXY(plan.StartTime, plan.Splits[_phaseInfo.PhaseNumber]);
                        ProgrammedSplitSeries.Points.AddXY(plan.EndTime, plan.Splits[_phaseInfo.PhaseNumber]);
                    }
                    catch
                    {
                    }
                }

                Chart.Series.Add(ProgrammedSplitSeries);

                var signalPhase = new SignalPhase(_startDate, _endDate, _approach, true, 15, 32, false);
                if (_waitTimeOptions.ShowPlanStripes)
                {
                    SetSimplePlanStrips(_plans, Chart, _startDate, waitTimeTrackerList);
                }

                AddVolumeToChart(Chart, signalPhase.Volume);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void SetSimplePlanStrips(List<PlanSplitMonitor> plans, Chart chart, DateTime graphStartDate,
            List<WaitTimeTracker> waitTimeTrackerList)
        {
            var backGroundColor = 1;
            foreach (var plan in plans)
            {
                var stripline = new StripLine();
                //Creates alternating backcolor to distinguish the plans
                if (backGroundColor % 2 == 0)
                    stripline.BackColor = Color.FromArgb(120, Color.LightGray);
                else
                    stripline.BackColor = Color.FromArgb(120, Color.LightBlue);

                //Set the stripline properties
                stripline.IntervalOffsetType = DateTimeIntervalType.Hours;
                stripline.Interval = 1;
                stripline.IntervalOffset = (plan.StartTime - graphStartDate).TotalHours;
                stripline.StripWidth = (plan.EndTime - plan.StartTime).TotalHours;
                stripline.StripWidthType = DateTimeIntervalType.Hours;

                chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

                //Add a corrisponding custom label for each strip
                var Plannumberlabel = new CustomLabel();
                Plannumberlabel.FromPosition = plan.StartTime.ToOADate();
                Plannumberlabel.ToPosition = plan.EndTime.ToOADate();
                switch (plan.PlanNumber)
                {
                    case 254:
                        Plannumberlabel.Text = "Free";
                        break;
                    case 255:
                        Plannumberlabel.Text = "Flash";
                        break;
                    case 0:
                        Plannumberlabel.Text = "Unknown";
                        break;
                    default:
                        Plannumberlabel.Text = "Plan " + plan.PlanNumber;

                        break;
                }

                Plannumberlabel.LabelMark = LabelMarkStyle.LineSideMark;
                Plannumberlabel.ForeColor = Color.Black;
                Plannumberlabel.RowIndex = 6;


                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(Plannumberlabel);

                if (waitTimeTrackerList.Any())
                {
                    var waitTimeSubset =
                        waitTimeTrackerList.Where(x => x.Time > plan.StartTime && x.Time < plan.EndTime);
                    if (waitTimeSubset.Any())
                    {
                        var avgWaitTime = waitTimeSubset.Average(x => x.WaitTimeSeconds);
                        var maxWaitTime = waitTimeSubset.Max(x => x.WaitTimeSeconds);

                        var avgLabel = ChartTitleFactory.GetCustomLabelForTitle(
                            $"Avg Wait: {avgWaitTime.ToString("F1")} s", plan.StartTime.ToOADate(),
                            plan.EndTime.ToOADate(), 5, Color.Black);
                        chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(avgLabel);

                        var maxLabel = ChartTitleFactory.GetCustomLabelForTitle(
                            $"Max Wait: {maxWaitTime.ToString("F1")} s", plan.StartTime.ToOADate(),
                            plan.EndTime.ToOADate(), 4, Color.Black);
                        chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(maxLabel);
                    }
                }

                backGroundColor++;
            }
        }

        void AddVolumeToChart(Chart chart, VolumeCollection volumeCollection)
        {
            foreach (var v in volumeCollection.Items)
                chart.Series["Volume Per Hour"].Points.AddXY(v.XAxis, v.YAxis);
        }
    }
}