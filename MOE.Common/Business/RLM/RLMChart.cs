using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.WCFServiceLibrary;

namespace MOE.Common.Business
{
    public class RLMChart
    {
        public YellowAndRedOptions Options { get; set; }

        public Chart GetChart(RLMSignalPhase signalPhase, YellowAndRedOptions options)
        {
            Options = options;
            var chart = new Chart();
            var reportTimespan = Options.EndDate - Options.StartDate;


            //Set the chart properties
            ChartFactory.SetImageProperties(chart);
            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
            SetChartTitle(chart, signalPhase);

            //Create the chart legend
            var chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chart.Legends.Add(chartLegend);


            //Create the chart area
            var chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            if (Options.YAxisMax != null)
                chartArea.AxisY.Maximum = Options.YAxisMax.Value;
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Title = "Yellow Red Time (Seconds) ";
            chartArea.AxisY.MinorTickMark.Enabled = true;
            chartArea.AxisY.MajorTickMark.Enabled = true;
            chartArea.AxisY.MajorGrid.Interval = 5;
            chartArea.AxisY.MinorGrid.Interval = 1;
            chartArea.AxisX.Title = "Time (Hour of Day)";
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.LabelStyle.Format = "HH";
            chartArea.AxisX2.LabelStyle.Format = "HH";
            chartArea.AxisX.MajorGrid.Enabled = true;
            if (reportTimespan.Days < 1)
                if (reportTimespan.Hours > 1)
                {
                    chartArea.AxisX2.Interval = 1;
                    chartArea.AxisX.Interval = 1;
                }
                else
                {
                    chartArea.AxisX.LabelStyle.Format = "HH:mm";
                    chartArea.AxisX2.LabelStyle.Format = "HH:mm";
                }
            //chartArea.AxisX.Minimum = 0;

            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisX2.MajorTickMark.Enabled = true;

            chartArea.AxisX2.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX2.LabelAutoFitStyle = LabelAutoFitStyles.None;
            //chartArea.AxisX.Minimum = 0;

            chart.ChartAreas.Add(chartArea);

            var yelowish = Color.FromArgb(245, 237, 127);
            var blueish = Color.FromArgb(128, 10, 117, 182);
            var greenish = Color.FromArgb(64, 177, 14);
            var tanish = Color.FromArgb(220, 138, 78);
            var whiteish = Color.FromArgb(243, 240, 235);
            var redish = Color.FromArgb(128, 255, 0, 0);
            var darkRedish = Color.FromArgb(196, 222, 2, 2);
            //Color redish = Color.FromArgb(163, 60, 62);

            //Add the red series
            var redSeries = new Series();
            redSeries.ChartType = SeriesChartType.Area;
            redSeries.Color = redish;
            redSeries.Name = "Red";
            redSeries.XValueType = ChartValueType.DateTime;

            chart.Series.Add(redSeries);

            //Add the red series
            var redClearanceSeries = new Series();
            redClearanceSeries.ChartType = SeriesChartType.Area;
            redClearanceSeries.Color = darkRedish;
            redClearanceSeries.Name = "Red Clearance";
            redClearanceSeries.XValueType = ChartValueType.DateTime;
            chart.Series.Add(redClearanceSeries);

            //Add the yellow series
            var yellowClearanceSeries = new Series();
            yellowClearanceSeries.ChartType = SeriesChartType.Area;
            yellowClearanceSeries.Color = yelowish;
            yellowClearanceSeries.BackSecondaryColor = yelowish;
            yellowClearanceSeries.Name = "Yellow Change";
            yellowClearanceSeries.XValueType = ChartValueType.DateTime;
            chart.Series.Add(yellowClearanceSeries);

            //Add the point series;
            var pointSeries = new Series();
            pointSeries.ChartType = SeriesChartType.Point;
            pointSeries.Color = Color.Black;
            pointSeries.Name = "Detector Activation";
            pointSeries.XValueType = ChartValueType.DateTime;
            pointSeries.MarkerSize = 3;
            chart.Series.Add(pointSeries);


            //Add points at the start and and of the x axis to ensure
            //the graph covers the entire period selected by the user
            //whether there is data or not
            chart.Series["Detector Activation"].Points.AddXY(signalPhase.Cycles.Any() ? signalPhase.Cycles.First().StartTime : Options.StartDate, 0);
            chart.Series["Detector Activation"].Points.AddXY(signalPhase.Cycles.Any()?signalPhase.Cycles.Last().EndTime: Options.EndDate, 0);

            AddDataToChart(chart, signalPhase);
            return chart;
        }

        private void SetChartTitle(Chart chart, RLMSignalPhase signalPhase)
        {
            //Set the chart title
            chart.Titles.Add(ChartTitleFactory.GetChartName(Options.MetricTypeID));
            chart.Titles.Add(
                ChartTitleFactory.GetSignalLocationAndDateRange(Options.SignalID, Options.StartDate, Options.EndDate));
            chart.Titles.Add(ChartTitleFactory.GetPhaseAndPhaseDescriptions(signalPhase.Approach, signalPhase.GetPermissivePhase));
            var statistics = new Dictionary<string, string>();
            statistics.Add("Total Violations", signalPhase.Violations + " (" + signalPhase.PercentViolations + "%)");
            statistics.Add("Severe Violations",
                signalPhase.SevereRedLightViolations + " (" + signalPhase.PercentSevereViolations + "%)");
            statistics.Add("Yellow Light Occurrences",
                signalPhase.YellowOccurrences + " (" + signalPhase.PercentYellowOccurrences + "%)");
            chart.Titles.Add(ChartTitleFactory.GetStatistics(statistics));
        }

        private void AddDataToChart(Chart chart, RLMSignalPhase signalPhase)
            //, DateTime startDate,
            //DateTime endDate, string signalId, bool showRlv, bool showSrlv,
            //bool showPrlv, bool showPsrlv, bool showAveTrlv, bool showYlo, bool showPylo, 
            //bool showTylo)
        {
            decimal totalDetectorHits = 0;

            foreach (var plan in signalPhase.Plans.PlanList)
                if (plan.RLMCycleCollection.Count > 0)
                    foreach (var rlm in plan.RLMCycleCollection)
                    {
                        chart.Series["Yellow Change"].Points.AddXY(
                            //pcd.StartTime,
                            rlm.RedClearanceEvent,
                            rlm.RedClearanceBeginY);
                        chart.Series["Red Clearance"].Points.AddXY(
                            //pcd.StartTime,
                            rlm.RedEvent,
                            rlm.RedBeginY);
                        chart.Series["Red"].Points.AddXY(
                            //pcd.StartTime, 
                            rlm.RedEndEvent,
                            rlm.RedEndY);
                        totalDetectorHits += rlm.DetectorCollection.Count;
                        foreach (var detectorPoint in rlm.DetectorCollection)
                            chart.Series["Detector Activation"].Points.AddXY(
                                //pcd.StartTime, 
                                detectorPoint.TimeStamp,
                                detectorPoint.YPoint);
                    }
            SetPlanStrips(signalPhase.Plans.PlanList, chart);
            //, Options.StartDate,
            //    Options.ShowRedLightViolations, Options.showSrlv, showPrlv, showPsrlv, showAveTrlv, showYlo, showPylo, 
            //showTylo);
        }


        /// <summary>
        ///     Adds plan strips to the chart
        /// </summary>
        /// <param name="planCollection"></param>
        /// <param name="chart"></param>
        /// <param name="graphStartDate"></param>
        protected void SetPlanStrips(List<RLMPlan> planCollection,
                Chart chart)
            //, DateTime graphStartDate, bool showRlv, bool showSrlv,
            //bool showPrlv, bool showPsrlv, bool showAveTrlv, bool showYlo, bool showPylo, 
            //bool showTylo)
        {
            var backGroundColor = 1;

            foreach (var plan in planCollection)
            {
                var customLabelIndex = 1;
                var stripline = new StripLine();
                //Creates alternating backcolor to distinguish the plans
                if (backGroundColor % 2 == 0)
                    stripline.BackColor = Color.FromArgb(120, Color.LightGray);
                else
                    stripline.BackColor = Color.FromArgb(120, Color.LightBlue);

                //Set the stripline properties
                stripline.IntervalOffset = (plan.StartTime - Options.StartDate).TotalHours;
                stripline.IntervalOffsetType = DateTimeIntervalType.Hours;
                stripline.Interval = 1;
                stripline.IntervalType = DateTimeIntervalType.Days;
                stripline.StripWidth = (plan.EndTime - plan.StartTime).TotalHours;
                stripline.StripWidthType = DateTimeIntervalType.Hours;

                chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

                //Add a corrisponding custom label for each strip

                if (Options.ShowRedLightViolations)
                {
                    var violationLabel = new CustomLabel();
                    violationLabel.FromPosition = plan.StartTime.ToOADate();
                    violationLabel.ToPosition = plan.EndTime.ToOADate();


                    violationLabel.LabelMark = LabelMarkStyle.LineSideMark;
                    violationLabel.ForeColor = Color.Blue;
                    violationLabel.RowIndex = customLabelIndex;
                    customLabelIndex++;
                    violationLabel.Text = "RLV-" + plan.Violations;
                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(violationLabel);
                }
                if (Options.ShowSevereRedLightViolations)
                {
                    var srlvLabel = new CustomLabel();
                    srlvLabel.FromPosition = plan.StartTime.ToOADate();
                    srlvLabel.ToPosition = plan.EndTime.ToOADate();


                    srlvLabel.LabelMark = LabelMarkStyle.LineSideMark;
                    srlvLabel.ForeColor = Color.Maroon;
                    srlvLabel.RowIndex = customLabelIndex;
                    customLabelIndex++;
                    srlvLabel.Text = "SRLV-" + plan.SevereRedLightViolations;
                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(srlvLabel);
                }
                if (Options.ShowPercentRedLightViolations)
                {
                    var percentViolationsLabel = new CustomLabel();
                    percentViolationsLabel.FromPosition = plan.StartTime.ToOADate();
                    percentViolationsLabel.ToPosition = plan.EndTime.ToOADate();


                    percentViolationsLabel.LabelMark = LabelMarkStyle.LineSideMark;
                    percentViolationsLabel.ForeColor = Color.Maroon;
                    percentViolationsLabel.RowIndex = customLabelIndex;
                    customLabelIndex++;
                    percentViolationsLabel.Text = "%RLV-" + plan.PercentViolations;
                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(percentViolationsLabel);
                }
                if (Options.ShowPercentSevereRedLightViolations)
                {
                    var percentSevereViolationsLabel = new CustomLabel();
                    percentSevereViolationsLabel.FromPosition = plan.StartTime.ToOADate();
                    percentSevereViolationsLabel.ToPosition = plan.EndTime.ToOADate();


                    percentSevereViolationsLabel.LabelMark = LabelMarkStyle.LineSideMark;
                    percentSevereViolationsLabel.ForeColor = Color.Maroon;
                    percentSevereViolationsLabel.RowIndex = customLabelIndex;
                    customLabelIndex++;
                    percentSevereViolationsLabel.Text = "%SRLV-" + plan.PercentSevereViolations;
                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(percentSevereViolationsLabel);
                }
                if (Options.ShowAverageTimeRedLightViolations)
                {
                    var averageTRLV = new CustomLabel();
                    averageTRLV.FromPosition = plan.StartTime.ToOADate();
                    averageTRLV.ToPosition = plan.EndTime.ToOADate();


                    averageTRLV.LabelMark = LabelMarkStyle.LineSideMark;
                    averageTRLV.ForeColor = Color.Maroon;
                    averageTRLV.RowIndex = customLabelIndex;
                    customLabelIndex++;
                    averageTRLV.Text = "Ave TRLV-" + plan.AverageTRLV;

                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(averageTRLV);
                }

                if (Options.ShowYellowLightOccurrences)
                {
                    var YellowOccurences = new CustomLabel();
                    YellowOccurences.FromPosition = plan.StartTime.ToOADate();
                    YellowOccurences.ToPosition = plan.EndTime.ToOADate();


                    YellowOccurences.LabelMark = LabelMarkStyle.LineSideMark;
                    YellowOccurences.ForeColor = Color.Maroon;
                    YellowOccurences.RowIndex = customLabelIndex;
                    customLabelIndex++;
                    YellowOccurences.Text = "#YLO-" + plan.YellowOccurrences;
                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(YellowOccurences);
                }

                if (Options.ShowPercentYellowLightOccurrences)
                {
                    var PercentYellowOccurences = new CustomLabel();
                    PercentYellowOccurences.FromPosition = plan.StartTime.ToOADate();
                    PercentYellowOccurences.ToPosition = plan.EndTime.ToOADate();


                    PercentYellowOccurences.LabelMark = LabelMarkStyle.LineSideMark;
                    PercentYellowOccurences.ForeColor = Color.Maroon;
                    PercentYellowOccurences.RowIndex = customLabelIndex;
                    customLabelIndex++;
                    PercentYellowOccurences.Text = "%YLO-" + plan.PercentYellowOccurrences;
                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(PercentYellowOccurences);
                }

                if (Options.ShowAverageTimeYellowOccurences)
                {
                    var AverageYellowTime = new CustomLabel();
                    AverageYellowTime.FromPosition = plan.StartTime.ToOADate();
                    AverageYellowTime.ToPosition = plan.EndTime.ToOADate();


                    AverageYellowTime.LabelMark = LabelMarkStyle.LineSideMark;
                    AverageYellowTime.ForeColor = Color.Maroon;
                    AverageYellowTime.RowIndex = customLabelIndex;
                    customLabelIndex++;
                    AverageYellowTime.Text = "TYLO-" + plan.AverageTYLO;

                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(AverageYellowTime);
                }
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

                Plannumberlabel.ForeColor = Color.Black;
                Plannumberlabel.RowIndex = customLabelIndex;
                customLabelIndex++;

                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(Plannumberlabel);

                //Change the background color counter for alternating color
                backGroundColor++;
            }
        }
    }
}