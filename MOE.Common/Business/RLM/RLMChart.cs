using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;

namespace MOE.Common.Business
{
    public class RLMChart
    {
        public WCFServiceLibrary.YellowAndRedOptions Options { get; set; }
        public RLMChart()
        {

        }

        public Chart GetChart(RLMSignalPhase signalPhase, WCFServiceLibrary.YellowAndRedOptions options)
        {
            Options = options;
            Chart chart = new Chart();
            TimeSpan reportTimespan = Options.EndDate - Options.StartDate;


            //Set the chart properties
            chart.ImageType = ChartImageType.Jpeg;
            chart.Height = 700;
            chart.Width = 1100;
            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
            SetChartTitle(chart, signalPhase);
            
            //Create the chart legend
            Legend chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chart.Legends.Add(chartLegend);


            //Create the chart area
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            if (Options.YAxisMax != null)
            {
                chartArea.AxisY.Maximum = Options.YAxisMax.Value;
            }
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
            {
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
            }
            //chartArea.AxisX.Minimum = 0;

            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisX2.MajorTickMark.Enabled = true;

            chartArea.AxisX2.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX2.LabelAutoFitStyle = LabelAutoFitStyles.None;
            //chartArea.AxisX.Minimum = 0;

            chart.ChartAreas.Add(chartArea);

            Color yelowish = Color.FromArgb(245, 237, 127);
            Color blueish = Color.FromArgb(128, 10, 117, 182);
            Color greenish = Color.FromArgb(64, 177, 14);
            Color tanish = Color.FromArgb(220, 138, 78);
            Color whiteish = Color.FromArgb(243, 240, 235);
            Color redish = Color.FromArgb(128, 255, 0, 0);
            Color darkRedish = Color.FromArgb(196, 222, 2, 2);
            //Color redish = Color.FromArgb(163, 60, 62);

            //Add the red series
            Series redSeries = new Series();
            redSeries.ChartType = SeriesChartType.Area;
            redSeries.Color = redish;
            //redSeries.BackGradientStyle = GradientStyle.VerticalCenter;
            redSeries.Name = "Red";
            redSeries.XValueType = ChartValueType.DateTime;

            chart.Series.Add(redSeries);

            //Add the yellow series
            Series redClearanceSeries = new Series();
            redClearanceSeries.ChartType = SeriesChartType.Area;
            redClearanceSeries.Color = darkRedish;
            redClearanceSeries.Name = "Red Clearance";
            redClearanceSeries.XValueType = ChartValueType.DateTime;
            chart.Series.Add(redClearanceSeries);

            //Add the green series
            Series yellowClearanceSeries = new Series();
            yellowClearanceSeries.ChartType = SeriesChartType.Area;
            yellowClearanceSeries.Color = yelowish;
            //yellowClearanceSeries.BackGradientStyle = GradientStyle.DiagonalLeft;
            yellowClearanceSeries.BackSecondaryColor = yelowish;
            yellowClearanceSeries.Name = "Yellow Clearance";
            yellowClearanceSeries.XValueType = ChartValueType.DateTime;
            chart.Series.Add(yellowClearanceSeries);

            //Add the point series
            Series pointSeries = new Series();
            pointSeries.ChartType = SeriesChartType.Point;
            pointSeries.Color = Color.Black;
            pointSeries.Name = "Detector Activation";
            pointSeries.XValueType = ChartValueType.DateTime;
            pointSeries.MarkerSize = 3;
            chart.Series.Add(pointSeries);

            

            

            

            //Add points at the start and and of the x axis to ensure
            //the graph covers the entire period selected by the user
            //whether there is data or not
            chart.Series["Detector Activation"].Points.AddXY(Options.StartDate, 0);
            chart.Series["Detector Activation"].Points.AddXY(Options.EndDate, 0);

            AddDataToChart(chart, signalPhase);
            return chart;
        }

        private void SetChartTitle(Chart chart, RLMSignalPhase signalPhase)
        {
            //Set the chart title
            chart.Titles.Add(ChartTitleFactory.GetChartName(Options.MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(Options.SignalID, Options.StartDate, Options.EndDate));
            chart.Titles.Add(ChartTitleFactory.GetPhaseAndPhaseDescriptions(signalPhase.Approach.ProtectedPhaseNumber, signalPhase.Approach.DirectionType.Description));
            Dictionary<string, string> statistics = new Dictionary<string, string>();
            statistics.Add("Total Violations", signalPhase.Violations.ToString() + " (" + signalPhase.PercentViolations.ToString() + "%)");
            statistics.Add("Severe Violations", signalPhase.Srlv + " (" + signalPhase.PercentSevereViolations.ToString() + "%)");
            statistics.Add("Yellow Light Occurrences", signalPhase.YellowOccurrences + " (" + signalPhase.PercentYellowOccurrences.ToString() + "%)");
            chart.Titles.Add(ChartTitleFactory.GetStatistics(statistics));
        }

        private void AddDataToChart(Chart chart, MOE.Common.Business.RLMSignalPhase signalPhase)
            //, DateTime startDate,
            //DateTime endDate, string signalId, bool showRlv, bool showSrlv,
            //bool showPrlv, bool showPsrlv, bool showAveTrlv, bool showYlo, bool showPylo, 
            //bool showTylo)
        {
            decimal totalDetectorHits = 0;

            foreach (MOE.Common.Business.RLMPlan plan in signalPhase.Plans.PlanList)
            {
                if (plan.RLMCycleCollection.Count > 0)
                {
                    foreach (MOE.Common.Business.RLMCycle rlm in plan.RLMCycleCollection)
                    {
                        chart.Series["Yellow Clearance"].Points.AddXY(
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
                        foreach (MOE.Common.Business.RLMDetectorDataPoint detectorPoint in rlm.DetectorCollection)
                        {
                            chart.Series["Detector Activation"].Points.AddXY(
                                //pcd.StartTime, 
                                detectorPoint.TimeStamp,
                                detectorPoint.YPoint);
                            
                        }
                    }
                }
            }
            SetPlanStrips(signalPhase.Plans.PlanList, chart);
            //, Options.StartDate,
            //    Options.ShowRedLightViolations, Options.showSrlv, showPrlv, showPsrlv, showAveTrlv, showYlo, showPylo, 
            //showTylo);
            

           


        }


        /// <summary>
        /// Adds plan strips to the chart
        /// </summary>
        /// <param name="planCollection"></param>
        /// <param name="chart"></param>
        /// <param name="graphStartDate"></param>
        protected void SetPlanStrips(List<MOE.Common.Business.RLMPlan> planCollection,
            Chart chart)
            //, DateTime graphStartDate, bool showRlv, bool showSrlv,
            //bool showPrlv, bool showPsrlv, bool showAveTrlv, bool showYlo, bool showPylo, 
            //bool showTylo)
        {
            int backGroundColor = 1;
            
            foreach (MOE.Common.Business.RLMPlan plan in planCollection)
            {
                int customLabelIndex = 1;
                StripLine stripline = new StripLine();
                //Creates alternating backcolor to distinguish the plans
                if (backGroundColor % 2 == 0)
                {
                    stripline.BackColor = Color.FromArgb(120, Color.LightGray);
                }
                else
                {
                    stripline.BackColor = Color.FromArgb(120, Color.LightBlue);
                }

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
                    CustomLabel violationLabel = new CustomLabel();
                    violationLabel.FromPosition = plan.StartTime.ToOADate();
                    violationLabel.ToPosition = plan.EndTime.ToOADate();


                    violationLabel.LabelMark = LabelMarkStyle.LineSideMark;
                    violationLabel.ForeColor = Color.Blue;
                    violationLabel.RowIndex = customLabelIndex;
                    customLabelIndex++;
                    violationLabel.Text = "RLV-" + plan.Violations.ToString();
                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(violationLabel);
                }
                if (Options.ShowSevereRedLightViolations)
                {
                    CustomLabel srlvLabel = new CustomLabel();
                    srlvLabel.FromPosition = plan.StartTime.ToOADate();
                    srlvLabel.ToPosition = plan.EndTime.ToOADate();


                    srlvLabel.LabelMark = LabelMarkStyle.LineSideMark;
                    srlvLabel.ForeColor = Color.Maroon;
                    srlvLabel.RowIndex = customLabelIndex;
                    customLabelIndex++; 
                    srlvLabel.Text = "SRLV-" + plan.Srlv.ToString();
                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(srlvLabel);
                }
                if (Options.ShowPercentRedLightViolations)
                {
                    CustomLabel percentViolationsLabel = new CustomLabel();
                    percentViolationsLabel.FromPosition = plan.StartTime.ToOADate();
                    percentViolationsLabel.ToPosition = plan.EndTime.ToOADate();


                    percentViolationsLabel.LabelMark = LabelMarkStyle.LineSideMark;
                    percentViolationsLabel.ForeColor = Color.Maroon;
                    percentViolationsLabel.RowIndex = customLabelIndex;
                    customLabelIndex++;
                    percentViolationsLabel.Text = "%RLV-" + plan.PercentViolations.ToString();
                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(percentViolationsLabel);
                }
                if (Options.ShowPercentSevereRedLightViolations)
                {
                    CustomLabel percentSevereViolationsLabel = new CustomLabel();
                    percentSevereViolationsLabel.FromPosition = plan.StartTime.ToOADate();
                    percentSevereViolationsLabel.ToPosition = plan.EndTime.ToOADate();


                    percentSevereViolationsLabel.LabelMark = LabelMarkStyle.LineSideMark;
                    percentSevereViolationsLabel.ForeColor = Color.Maroon;
                    percentSevereViolationsLabel.RowIndex = customLabelIndex;
                    customLabelIndex++;
                    percentSevereViolationsLabel.Text = "%SRLV-" + plan.PercentSevereViolations.ToString();
                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(percentSevereViolationsLabel);
                }
                if (Options.ShowAverageTimeRedLightViolations)
                {
                    CustomLabel averageTRLV = new CustomLabel();
                    averageTRLV.FromPosition = plan.StartTime.ToOADate();
                    averageTRLV.ToPosition = plan.EndTime.ToOADate();


                    averageTRLV.LabelMark = LabelMarkStyle.LineSideMark;
                    averageTRLV.ForeColor = Color.Maroon;
                    averageTRLV.RowIndex = customLabelIndex;
                    customLabelIndex++;
                    averageTRLV.Text = "Ave TRLV-" + plan.AverageTRLV.ToString();

                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(averageTRLV);
                }

                if (Options.ShowYellowLightOccurrences)
                {
                    CustomLabel YellowOccurences = new CustomLabel();
                    YellowOccurences.FromPosition = plan.StartTime.ToOADate();
                    YellowOccurences.ToPosition = plan.EndTime.ToOADate();


                    YellowOccurences.LabelMark = LabelMarkStyle.LineSideMark;
                    YellowOccurences.ForeColor = Color.Maroon;
                    YellowOccurences.RowIndex = customLabelIndex;
                    customLabelIndex++;
                    YellowOccurences.Text = "#YLO-" + plan.YellowOccurrences.ToString();
                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(YellowOccurences);
                }

                if (Options.ShowPercentYellowLightOccurrences)
                {
                    CustomLabel PercentYellowOccurences = new CustomLabel();
                    PercentYellowOccurences.FromPosition = plan.StartTime.ToOADate();
                    PercentYellowOccurences.ToPosition = plan.EndTime.ToOADate();


                    PercentYellowOccurences.LabelMark = LabelMarkStyle.LineSideMark;
                    PercentYellowOccurences.ForeColor = Color.Maroon;
                    PercentYellowOccurences.RowIndex = customLabelIndex;
                    customLabelIndex++;
                    PercentYellowOccurences.Text = "%YLO-" + plan.PercentYellowOccurrences.ToString();
                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(PercentYellowOccurences);
                }

                if (Options.ShowAverageTimeYellowOccurences)
                {
                    CustomLabel AverageYellowTime = new CustomLabel();
                    AverageYellowTime.FromPosition = plan.StartTime.ToOADate();
                    AverageYellowTime.ToPosition = plan.EndTime.ToOADate();


                    AverageYellowTime.LabelMark = LabelMarkStyle.LineSideMark;
                    AverageYellowTime.ForeColor = Color.Maroon;
                    AverageYellowTime.RowIndex = customLabelIndex;
                    customLabelIndex++;
                    AverageYellowTime.Text = "TYLO-" + plan.AverageTYLO.ToString();

                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(AverageYellowTime);
                }
                CustomLabel Plannumberlabel = new CustomLabel();
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
                        Plannumberlabel.Text = "Plan " + plan.PlanNumber.ToString();

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
