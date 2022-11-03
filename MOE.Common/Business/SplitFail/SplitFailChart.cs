using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.WCFServiceLibrary;

namespace MOE.Common.Business.SplitFail
{
    public class SplitFailChart
    {
        public Chart Chart;

        public SplitFailChart(SplitFailOptions options, SplitFailPhase splitFailPhase, bool getPermissivePhase)
        {
            Options = options;
            SplitFailPhase = splitFailPhase;
            GetPermissivePhase = getPermissivePhase;
            Chart = ChartFactory.CreateSplitFailureChart(Options);
            Chart.ChartAreas[0].AxisX.Minimum = Options.StartDate.ToOADate();
            Chart.ChartAreas[0].AxisX.Maximum = Options.EndDate.ToOADate();
            AddSeries(Chart);
            AddDataToChart(Chart);
        }

        private SplitFailOptions Options { get; }
        private SplitFailPhase SplitFailPhase { get; }
        private bool GetPermissivePhase { get; }

        private void AddSeries(Chart chart)
        {
            var gorGapSeries = new Series();
            gorGapSeries.MarkerSize = 4;
            gorGapSeries.ChartType = SeriesChartType.Point;
            gorGapSeries.MarkerStyle = MarkerStyle.Triangle;
            gorGapSeries.Color = Color.LimeGreen;
            gorGapSeries.Name = "GOR - GapOut";
            gorGapSeries.XValueType = ChartValueType.DateTime;

            var gorForceSeries = new Series();
            gorForceSeries.MarkerSize = 4;
            gorForceSeries.ChartType = SeriesChartType.Point;
            gorForceSeries.MarkerStyle = MarkerStyle.Square;
            gorForceSeries.Color = Color.ForestGreen;
            gorForceSeries.Name = "GOR - ForceOff";
            gorForceSeries.XValueType = ChartValueType.DateTime;

            var rorGapSeries = new Series();
            rorGapSeries.MarkerSize = 4;
            rorGapSeries.ChartType = SeriesChartType.Point;
            rorGapSeries.MarkerStyle = MarkerStyle.Triangle;
            rorGapSeries.Color = Color.HotPink;
            rorGapSeries.Name = "ROR - GapOut";
            rorGapSeries.XValueType = ChartValueType.DateTime;

            var rorForceSeries = new Series();
            rorForceSeries.MarkerSize = 4;
            rorForceSeries.ChartType = SeriesChartType.Point;
            rorForceSeries.MarkerStyle = MarkerStyle.Square;
            rorForceSeries.Color = Color.Red;
            rorForceSeries.Name = "ROR - ForceOff";
            rorForceSeries.XValueType = ChartValueType.DateTime;

            var splitFailSeries = new Series();
            splitFailSeries.ChartType = SeriesChartType.Column;
            splitFailSeries.Color = Color.Gold;
            splitFailSeries.Name = "SplitFail";
            splitFailSeries.XValueType = ChartValueType.DateTime;

            var gorAvg = new Series();
            gorAvg.ChartType = SeriesChartType.StepLine;
            gorAvg.BorderDashStyle = ChartDashStyle.Solid;
            gorAvg.BorderWidth = 2;
            gorAvg.Color = Color.DarkGreen;
            gorAvg.Name = "Avg. GOR";
            gorAvg.XValueType = ChartValueType.DateTime;

            var rorAvg = new Series();
            rorAvg.ChartType = SeriesChartType.StepLine;
            rorAvg.BorderDashStyle = ChartDashStyle.Solid;
            rorAvg.BorderWidth = 2;
            rorAvg.Color = Color.DarkRed;
            rorAvg.Name = "Avg. ROR";
            rorAvg.XValueType = ChartValueType.DateTime;

            var binSplitFailSeries = new Series();
            binSplitFailSeries.ChartType = SeriesChartType.StepLine;
            binSplitFailSeries.BorderDashStyle = ChartDashStyle.Dash;
            binSplitFailSeries.BorderWidth = 2;
            binSplitFailSeries.Color = Color.Blue;
            binSplitFailSeries.Name = "Percent Fails";
            binSplitFailSeries.XValueType = ChartValueType.DateTime;
            chart.Series.Add(splitFailSeries);
            chart.Series.Add(gorGapSeries);
            chart.Series.Add(gorForceSeries);
            chart.Series.Add(rorGapSeries);
            chart.Series.Add(rorForceSeries);
            chart.Series.Add(rorAvg);
            chart.Series.Add(gorAvg);
            chart.Series.Add(binSplitFailSeries);
            chart.Series["SplitFail"].CustomProperties = "DrawingStyle = Cylinder,PixelPointWidth = 1";
        }

        protected void AddDataToChart(Chart chart)
        {
            foreach (var cycle in SplitFailPhase.Cycles)
            {
                if (Options.ShowFailLines)
                    if (cycle.IsSplitFail)
                        chart.Series["SplitFail"].Points.AddXY(cycle.StartTime, 100);
                switch (cycle.TerminationEvent)
                {
                    case CycleSplitFail.TerminationType.GapOut:
                        chart.Series["GOR - GapOut"].Points.AddXY(cycle.StartTime, cycle.GreenOccupancyPercent);
                        chart.Series["ROR - GapOut"].Points.AddXY(cycle.StartTime, cycle.RedOccupancyPercent);
                        break;
                    default:
                        chart.Series["GOR - ForceOff"].Points.AddXY(cycle.StartTime, cycle.GreenOccupancyPercent);
                        chart.Series["ROR - ForceOff"].Points.AddXY(cycle.StartTime, cycle.RedOccupancyPercent);
                        break;
                }
            }
            foreach (var bin in SplitFailPhase.Bins)
            {
                if (Options.ShowAvgLines)
                {
                    chart.Series["Avg. GOR"].Points.AddXY(bin.StartTime, bin.AverageGreenOccupancyPercent);
                    chart.Series["Avg. ROR"].Points.AddXY(bin.StartTime, bin.AverageRedOccupancyPercent);
                }
                if (Options.ShowPercentFailLines)
                    chart.Series["Percent Fails"].Points.AddXY(bin.StartTime, Convert.ToInt32(bin.PercentSplitfails));
            }
            ExtendLinesToTheEndOfTheChart(chart);
            SetChartTitle(SplitFailPhase.Statistics);
            AddPlanStrips(chart, Options.StartDate);
        }

        private void ExtendLinesToTheEndOfTheChart(Chart chart)
        {
            var lastBin = SplitFailPhase.Bins.LastOrDefault();
            if (lastBin != null)
            {
                if (Options.ShowAvgLines)
                {
                    chart.Series["Avg. GOR"].Points.AddXY(lastBin.StartTime.AddMinutes(15),
                        lastBin.AverageGreenOccupancyPercent);
                    chart.Series["Avg. ROR"].Points
                        .AddXY(lastBin.StartTime.AddMinutes(15), lastBin.AverageRedOccupancyPercent);
                }
                if (Options.ShowPercentFailLines)
                    chart.Series["Percent Fails"].Points.AddXY(lastBin.StartTime.AddMinutes(15),
                        Convert.ToInt32(lastBin.PercentSplitfails));
            }
        }

        private void SetChartTitle(Dictionary<string, string> statistics)
        {
            Chart.Titles.Add(ChartTitleFactory.GetChartName(Options.MetricTypeID));
            Chart.Titles.Add(
                ChartTitleFactory.GetSignalLocationAndDateRange(Options.SignalID, Options.StartDate, Options.EndDate));
            Chart.Titles.Add(ChartTitleFactory.GetPhaseAndPhaseDescriptions(SplitFailPhase.Approach, SplitFailPhase.GetPermissivePhase));
            Chart.Titles.Add(ChartTitleFactory.GetStatistics(statistics));
        }

        protected void AddPlanStrips(Chart chart, DateTime startDate)
        {
            var backGroundColor = 1;

            //Parallel.ForEach(planCollection.PlanList, plan =>
            foreach (var plan in SplitFailPhase.Plans)
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
                stripline.IntervalOffset = (plan.StartTime - startDate).TotalHours;
                stripline.StripWidth = (plan.EndTime - plan.StartTime).TotalHours;
                stripline.StripWidthType = DateTimeIntervalType.Hours;

                chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

                //Add a corrisponding custom label for each strip
                var plannumberlabel = new CustomLabel();
                plannumberlabel.FromPosition = plan.StartTime.ToOADate();
                plannumberlabel.ToPosition = plan.EndTime.ToOADate();
                switch (plan.PlanNumber)
                {
                    case 254:
                        plannumberlabel.Text = "Free";
                        break;
                    case 255:
                        plannumberlabel.Text = "Flash";
                        break;
                    case 0:
                        plannumberlabel.Text = "Unknown";
                        break;
                    default:
                        plannumberlabel.Text = "Plan " + plan.PlanNumber;
                        break;
                }

                plannumberlabel.ForeColor = Color.Black;
                plannumberlabel.RowIndex = 2;
                plannumberlabel.LabelMark = LabelMarkStyle.LineSideMark;
                chart.ChartAreas[0].AxisX2.CustomLabels.Add(plannumberlabel);

                var planMetrics = new CustomLabel();
                planMetrics.FromPosition = plan.StartTime.ToOADate();
                planMetrics.ToPosition = plan.EndTime.ToOADate();

                planMetrics.Text += plan.FailsInPlan + " SF";
                planMetrics.Text += "\n" + Convert.ToInt32(plan.PercentFails) + "% SF";

                planMetrics.ForeColor = Color.Black;
                planMetrics.RowIndex = 1;
                chart.ChartAreas[0].AxisX2.CustomLabels.Add(planMetrics);
                //Change the background color counter for alternating color
                backGroundColor++;
            }
        }
    }
}