using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using System.Linq;
using MOE.Common.Business.WCFServiceLibrary;

namespace MOE.Common.Business.PEDDelay
{
    public class PEDDelayChart
    {
        public Chart Chart;
        private readonly PedPhase PedPhase;
        private readonly List<RedToRedCycle> RedToRedCycles;
        private Dictionary<DateTime, double> PedDelayCycles;
        private readonly PedDelayOptions Options;

        public PEDDelayChart(PedDelayOptions options,
            PedPhase pedPhase, List<RedToRedCycle> redToRedCycles)
        {
            if (options.ShowPercentDelay)
            {
                Chart = ChartFactory.CreateDefaultChartNoX2Axis(options);
            }
            else
            {
                Chart = ChartFactory.CreateDefaultChartNoX2AxisNoY2Axis(options);
            }
            PedPhase = pedPhase;
            Options = options;
            RedToRedCycles = redToRedCycles;

            //Set the chart properties
            ChartFactory.SetImageProperties(Chart);


            SetChartTitle(Chart, pedPhase, options);

            //Create the chart legend
            var chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            Chart.Legends.Add(chartLegend);


            //Create the chart area
            //var chartArea = new ChartArea();
            Chart.ChartAreas[0].AxisY.Title = "Pedestrian Delay\nby Actuation(minutes)";
            Chart.ChartAreas[0].AxisY.IntervalType = DateTimeIntervalType.Minutes;
            Chart.ChartAreas[0].AxisY.Minimum = DateTime.Today.ToOADate();
            Chart.ChartAreas[0].AxisY.LabelStyle.Format = "mm:ss";
            Chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            Chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            if (options.YAxisMax != null)
                Chart.ChartAreas[0].AxisY.Maximum = DateTime.Today.AddMinutes(options.YAxisMax.Value).ToOADate();

            Chart.ChartAreas[0].AxisY2.Title = "% Delay by Cycle Length";
            Chart.ChartAreas[0].AxisY2.IntervalType = (int)IntervalType.Number;
            Chart.ChartAreas[0].AxisY2.Interval = 20;
            Chart.ChartAreas[0].AxisY2.Maximum = 100;

            //Add the point series
            var PedestrianDelaySeries = new Series();
            PedestrianDelaySeries.ChartType = SeriesChartType.Column;
            PedestrianDelaySeries.Color = Color.Blue;
            PedestrianDelaySeries.Name = "Pedestrian Delay\nby Actuation";
            PedestrianDelaySeries.XValueType = ChartValueType.DateTime;
            Chart.Series.Add(PedestrianDelaySeries);
            Chart.Series["Pedestrian Delay\nby Actuation"]["PixelPointWidth"] = "2";


            var PedWalkSeries = new Series();
            PedWalkSeries.ChartType = SeriesChartType.Point;
            PedWalkSeries.MarkerStyle = MarkerStyle.Circle;
            PedWalkSeries.MarkerColor = Color.Orange;
            PedWalkSeries.Name = "Start of Begin Walk";
            PedWalkSeries.XValueType = ChartValueType.DateTime;
            PedWalkSeries.MarkerSize = 5;
            Chart.Series.Add(PedWalkSeries);

            var CycleDelay = new Series();
            CycleDelay.ChartType = SeriesChartType.Line;
            CycleDelay.Color = Color.Red;
            CycleDelay.Name = "Cycle Length";
            CycleDelay.XValueType = ChartValueType.DateTime;
            Chart.Series.Add(CycleDelay);

            var DelayByCycleLength = new Series();
            DelayByCycleLength.ChartType = SeriesChartType.StepLine;
            DelayByCycleLength.BorderDashStyle = ChartDashStyle.Dash;
            DelayByCycleLength.Color = Color.FromArgb(51, 153, 255);
            DelayByCycleLength.BorderWidth = 1;
            DelayByCycleLength.Name = "% Delay By Cycle Length";
            DelayByCycleLength.YAxisType = AxisType.Secondary;
            Chart.Series.Add(DelayByCycleLength);

            AddDataToChart();
            SetPlanStrips();
        }

        private void SetChartTitle(Chart chart, PedPhase pedPhase, PedDelayOptions options)
        {
            chart.Titles.Add(ChartTitleFactory.GetChartName(options.MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(
                options.SignalID, options.StartDate, options.EndDate));
            chart.Titles.Add(ChartTitleFactory.GetPhase(pedPhase.PhaseNumber));
            var statistics = new Dictionary<string, string>();
            statistics.Add("Ped Presses(PP)", pedPhase.Plans.Sum(p => p.PedPresses).ToString());
            statistics.Add("Time Buffered " + pedPhase.TimeBuffer + "s Presses", pedPhase.UniquePedDetections.ToString());
            statistics.Add("Min Delay", DateTime.Today.AddMinutes(pedPhase.MinDelay / 60).ToString("mm:ss"));
            statistics.Add("Max Delay", DateTime.Today.AddMinutes(pedPhase.MaxDelay / 60).ToString("mm:ss"));
            statistics.Add("Average Delay(AD)", Math.Round(pedPhase.AverageDelay, 2).ToString());
            chart.Titles.Add(ChartTitleFactory.GetStatistics(statistics));
        }


        protected void AddDataToChart()
        {
            int i = 0;
            var stepChart = new Dictionary<DateTime, double>();

            foreach (var pedPlan in PedPhase.Plans)
            {
                foreach (var pedCycle in pedPlan.Cycles)
                {
                    Chart.Series["Pedestrian Delay\nby Actuation"].Points
                    .AddXY(pedCycle.BeginWalk, DateTime.Today.AddMinutes(pedCycle.Delay / 60));
                    if (Options.ShowPedBeginWalk)
                    {
                        Chart.Series["Start of Begin Walk"].Points
                            .AddXY(pedCycle.BeginWalk, DateTime.Today.AddMinutes(pedCycle.Delay / 60).AddSeconds(3)); //add ped walk to top of delay
                    }

                    if (Options.ShowPercentDelay)
                    {
                        AddDataPointToStepChart(pedCycle, i, stepChart);
                    }
                }
            }

            if (Options.ShowPedBeginWalk)
            {
                foreach (var e in PedPhase.PedBeginWalkEvents)
                {
                    Chart.Series["Start of Begin Walk"].Points
                            .AddXY(e.Timestamp, DateTime.Today.AddSeconds(2));
                }
            }

            if (Options.ShowPercentDelay)
            {
                CreatePedDelayList(stepChart);
                foreach (var cycle in PedDelayCycles)
                {
                    Chart.Series["% Delay By Cycle Length"].Points
                                    .AddXY(cycle.Key, cycle.Value);
                }
            }

            if (Options.ShowCycleLength)
            {
                foreach (var cycle in RedToRedCycles)
                {
                    Chart.Series["Cycle Length"].Points.AddXY(cycle.EndTime, DateTime.Today.AddSeconds(cycle.RedLineY));
                }
            }
        }

        protected void AddDataPointToStepChart(PedCycle pc, int i, Dictionary<DateTime, double> stepChart)
        {
            while (i < RedToRedCycles.Count)
            {
                if (RedToRedCycles[i].EndTime > pc.BeginWalk)
                {
                    double cycle1;
                    if (i > 0)
                    {
                        cycle1 = RedToRedCycles[i - 1].RedLineY;
                    }
                    else
                    {
                        cycle1 = RedToRedCycles[i].RedLineY;
                    }
                    var cycle2 = RedToRedCycles[i].RedLineY;
                    var average = (cycle1 + cycle2) / 2;
                    var cycleLength = pc.Delay;

                    if (pc.Delay / 60 > 60)
                    {
                        cycleLength += 60;
                    }
                    stepChart.Add(pc.BeginWalk, cycleLength / average * 100);
                    break;
                }
                i++;
            }
        }

        protected void CreatePedDelayList(Dictionary<DateTime, double> stepChart)
        {
            var bins = new Dictionary<DateTime, double>();
            var startTime = PedPhase.StartDate;
            while (startTime < PedPhase.EndDate)
            {
                var endTime = startTime.AddMinutes(30);
                var cycles = stepChart.Where(c => c.Key >= startTime && c.Key < endTime).ToList();
                double average = 0;
                if (cycles.Count > 0)
                {
                    average = cycles.Average(c => c.Value);
                }
                bins.Add(startTime, average);
                startTime = startTime.AddMinutes(30);
            }
            PedDelayCycles = bins;
        }


        protected void SetPlanStrips()
        {
            var backGroundColor = 1;
            foreach (var plan in PedPhase.Plans)
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
                stripline.IntervalOffset = (plan.StartDate - PedPhase.StartDate).TotalHours;
                stripline.StripWidth = (plan.EndDate - plan.StartDate).TotalHours;
                stripline.StripWidthType = DateTimeIntervalType.Hours;

                Chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

                //Add a corresponding custom label for each strip
                var Plannumberlabel = new CustomLabel();
                Plannumberlabel.FromPosition = plan.StartDate.ToOADate();
                Plannumberlabel.ToPosition = plan.EndDate.ToOADate();
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

                Plannumberlabel.ForeColor = Color.Black;
                Plannumberlabel.RowIndex = 3;

                Chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(Plannumberlabel);

                var pedRecallLabel = new CustomLabel();
                pedRecallLabel.FromPosition = plan.StartDate.ToOADate();
                pedRecallLabel.ToPosition = plan.EndDate.ToOADate();
                string pedRecall = "Ped Recall Off";
                if (plan.PedBeginWalkCount / (plan.PedCallsRegisteredCount + plan.PedBeginWalkCount) * 100 >= 80)
                {
                    pedRecall = "Ped Recall On";
                }
                pedRecallLabel.Text = pedRecall;
                pedRecallLabel.RowIndex = 4;
                Chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(pedRecallLabel);

                var pedPressesLabel = new CustomLabel();
                pedPressesLabel.FromPosition = plan.StartDate.ToOADate();
                pedPressesLabel.ToPosition = plan.EndDate.ToOADate();
                pedPressesLabel.Text = plan.PedPresses + " PP";
                pedPressesLabel.LabelMark = LabelMarkStyle.LineSideMark;
                pedPressesLabel.RowIndex = 2;
                Chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(pedPressesLabel);

                var avgDelayLabel = new CustomLabel();
                avgDelayLabel.FromPosition = plan.StartDate.ToOADate();
                avgDelayLabel.ToPosition = plan.EndDate.ToOADate();
                avgDelayLabel.Text = Math.Round(plan.AvgDelay, 2) + " AD";
                avgDelayLabel.RowIndex = 1;
                Chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(avgDelayLabel);

                //Change the background color counter for alternating color
                backGroundColor++;
            }
        }
    }
}