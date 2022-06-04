using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.WCFServiceLibrary;

namespace MOE.Common.Business.PEDDelay
{
    public class PEDDelayChart
    {
        public Chart Chart;
        private readonly PedPhase PedPhase;
        private readonly List<RedToRedCycle> RedToRedCycles;

        public PEDDelayChart(PedDelayOptions options,
            PedPhase pedPhase, List<RedToRedCycle> redToRedCycles)
        {
            Chart = ChartFactory.CreateDefaultChartNoX2AxisNoY2Axis(options);
            PedPhase = pedPhase;
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
            if (options.YAxisMax != null)
                Chart.ChartAreas[0].AxisY.Maximum = DateTime.Today.AddMinutes(options.YAxisMax.Value).ToOADate();



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
            PedWalkSeries.MarkerStyle = MarkerStyle.Square;
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

            AddDataToChart();
            SetPlanStrips();
        }

        private void SetChartTitle(Chart chart, PedPhase pp, PedDelayOptions options)
        {
            chart.Titles.Add(ChartTitleFactory.GetChartName(options.MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(
                options.SignalID, options.StartDate, options.EndDate));
            chart.Titles.Add(ChartTitleFactory.GetPhase(pp.PhaseNumber));
            var statistics = new Dictionary<string, string>();
            statistics.Add("Ped Actuations(PA)", pp.PedActuations.ToString());
            statistics.Add("Time Buffered " + pp.TimeBuffer + "s Presses", pp.UniquePedDetections.ToString());
            statistics.Add("Min Delay", DateTime.Today.AddMinutes(pp.MinDelay / 60).ToString("mm:ss"));
            statistics.Add("Max Delay", DateTime.Today.AddMinutes(pp.MaxDelay / 60).ToString("mm:ss"));
            statistics.Add("Average Delay(AD)", DateTime.Today.AddMinutes(pp.AverageDelay / 60).ToString("mm:ss"));
            chart.Titles.Add(ChartTitleFactory.GetStatistics(statistics));
        }


        protected void AddDataToChart()
        {
            foreach (var pp in PedPhase.Plans)
            {
                foreach (var pc in pp.Cycles)
                {
                    Chart.Series["Pedestrian Delay\nby Actuation"].Points
                    .AddXY(pc.BeginWalk, DateTime.Today.AddMinutes(pc.Delay / 60));

                    Chart.Series["Start of Begin Walk"].Points
                       .AddXY(pc.BeginWalk, DateTime.Today.AddMinutes(pc.Delay / 60).AddSeconds(2));
                }
            }
            
            foreach (var e in PedPhase.PedBeginWalkEvents)
            {
                Chart.Series["Start of Begin Walk"].Points
                        .AddXY(e.Timestamp, DateTime.Today.AddSeconds(2));
            }

            foreach (var cycle in RedToRedCycles)
            {
                Chart.Series["Cycle Length"].Points.AddXY(cycle.EndTime, DateTime.Today.AddSeconds(cycle.RedLineY));
            }
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

                var pedActuationsLabel = new CustomLabel();
                pedActuationsLabel.FromPosition = plan.StartDate.ToOADate();
                pedActuationsLabel.ToPosition = plan.EndDate.ToOADate();
                pedActuationsLabel.Text = plan.PedActuations + " PA";
                pedActuationsLabel.LabelMark = LabelMarkStyle.LineSideMark;
                pedActuationsLabel.RowIndex = 2;
                Chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(pedActuationsLabel);

                var avgDelayLabel = new CustomLabel();
                avgDelayLabel.FromPosition = plan.StartDate.ToOADate();
                avgDelayLabel.ToPosition = plan.EndDate.ToOADate();
                avgDelayLabel.Text = Math.Round(plan.AvgDelay / 60) + " AD";
                avgDelayLabel.RowIndex = 1;
                Chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(avgDelayLabel);

                //Change the background color counter for alternating color
                backGroundColor++;
            }
        }
    }
}