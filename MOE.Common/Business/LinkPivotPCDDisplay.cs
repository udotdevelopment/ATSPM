using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business
{
    /// <summary>
    ///     Creates PCDs based on a Link Pivot Analysis
    /// </summary>
    public class LinkPivotPCDDisplay
    {
        //File location of the downstream after adjustment pcd

        //File location of the downstream before pcd

        //Total Arrival On Green before adjustments

        //Total Percent Arrival On Green before adjustments

        //Total volume before adjustments

        private readonly int MetricTypeID = 13;

        //Total arrival on green after adjustments

        //Total percent arrival on green after adjustments

        //Total volume after adjustments

        //File location of the upstream after adjustment pcd

        //File location of the upstream before pcd

        /// <summary>
        ///     Generates PCD charts for upstream and downstream detectors based on
        ///     a Link Pivot Link
        /// </summary>
        /// <param name="upstreamSignalId"></param>
        /// <param name="upstreamDirection"></param>
        /// <param name="downstreamSignalId"></param>
        /// <param name="downstreamDirection"></param>
        /// <param name="delta"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="maxYAxis"></param>
        public LinkPivotPCDDisplay(string upstreamSignalId, string upstreamDirection,
            string downstreamSignalId, string downstreamDirection, int delta,
            DateTime startDate, DateTime endDate, int maxYAxis)
        {
            var signalRepository = SignalsRepositoryFactory.Create();
            var upstreamSignal = signalRepository.GetVersionOfSignalByDate(upstreamSignalId, startDate);
            var downstreamSignal = signalRepository.GetVersionOfSignalByDate(downstreamSignalId, startDate);
            var upApproachToAnalyze = GetApproachToAnalyze(upstreamSignal, upstreamDirection);
            var downApproachToAnalyze = GetApproachToAnalyze(downstreamSignal, downstreamDirection);
            if (upApproachToAnalyze != null)
                GeneratePcd(upApproachToAnalyze, delta, startDate, endDate, true, maxYAxis);
            if (downApproachToAnalyze != null)
                GeneratePcd(downApproachToAnalyze, delta, startDate, endDate, false, maxYAxis);
        }

        public string UpstreamBeforePCDPath { get; set; }

        public string DownstreamBeforePCDPath { get; set; }

        //Create titles for the charts so that they can later be added for accesability
        public string DownstreamBeforeTitle { get; set; }

        public string UpstreamBeforeTitle { get; set; }
        public string DownstreamAfterTitle { get; set; }
        public string UpstreamAfterTitle { get; set; }

        public string UpstreamAfterPCDPath { get; set; }

        public string DownstreamAfterPCDPath { get; set; }

        public double ExistingTotalAOG { get; set; }

        public double ExistingTotalPAOG { get; set; } = 0;

        public double PredictedTotalAOG { get; set; }

        public double PredictedTotalPAOG { get; set; } = 0;

        public double PredictedVolume { get; set; }

        public double ExistingVolume { get; set; }

        private Approach GetApproachToAnalyze(Models.Signal signal, string direction)
        {
            Approach approachToAnalyze = null;
            var approaches = signal.Approaches.Where(a => a.DirectionType.Description == direction).ToList();
            foreach (var approach in approaches)
                if (approach.GetDetectorsForMetricType(6).Count > 0)
                    approachToAnalyze = approach;
            return approachToAnalyze;
        }

        private void GeneratePcd(Approach approach, int delta, DateTime startDate, DateTime endDate, bool upstream,
            int maxYAxis)
        {
            //Create a location string to show the combined cross strees
            var location = string.Empty;
            if (approach.Signal != null)
                location = approach.Signal.PrimaryName + " " + approach.Signal.SecondaryName;
            var chartName = string.Empty;
            //find the upstream approach
            if (!string.IsNullOrEmpty(approach.DirectionType.Description))
            {
                //Find PCD detector for this appraoch
                var detector = approach.Signal.GetDetectorsForSignalThatSupportAMetricByApproachDirection(
                    6, approach.DirectionType.Description).FirstOrDefault();
                //Check for null value
                if (detector != null)
                {
                    //Instantiate a signal phase object
                    var sp = new SignalPhase(startDate, endDate, approach, false, 15, 13, false);

                    //Check the direction of the Link Pivot
                    if (upstream)
                    {
                        //Create a chart for the upstream detector before adjustments
                        UpstreamBeforePCDPath = CreateChart(sp, startDate, endDate, location, "before",
                            ConfigurationManager.AppSettings["ImageLocation"], maxYAxis, "UpstreamBefore");

                        //Add the total arrival on green before adjustments to the running total
                        ExistingTotalAOG += sp.TotalArrivalOnGreen;

                        //Add the volume from the signal phase to the running total
                        ExistingVolume += sp.TotalVolume;

                        //Re run the signal phase by the optimized delta change to get the adjusted pcd
                        sp.LinkPivotAddSeconds(delta * -1);

                        //Create a chart for the upstream detector after adjustments
                        UpstreamAfterPCDPath = CreateChart(sp, startDate, endDate, location, "after",
                            ConfigurationManager.AppSettings["ImageLocation"], maxYAxis, "UpstreamAfter");

                        //Add the total arrival on green after adjustments to the running total
                        PredictedTotalAOG += sp.TotalArrivalOnGreen;

                        //Add the volume from the signal phase to the running total
                        PredictedVolume += sp.TotalVolume;
                    }
                    else
                    {
                        //Create a chart for downstream detector before adjustments
                        DownstreamBeforePCDPath = CreateChart(sp, startDate, endDate, location, "before",
                            ConfigurationManager.AppSettings["ImageLocation"], maxYAxis, "DownstreamBefore");

                        //Add the arrivals on green to the total arrivals on green running total
                        ExistingTotalAOG += sp.TotalArrivalOnGreen;

                        //Add the volume before adjustments to the running total volume
                        ExistingVolume += sp.TotalVolume;

                        //Re run the signal phase by the optimized delta change to get the adjusted pcd
                        sp.LinkPivotAddSeconds(delta);

                        //Create a pcd chart for downstream after adjustments
                        DownstreamAfterPCDPath = CreateChart(sp, startDate, endDate, location, "after",
                            ConfigurationManager.AppSettings["ImageLocation"], maxYAxis, "DownstreamAfter");

                        //Add the total arrivals on green to the running total
                        PredictedTotalAOG += sp.TotalArrivalOnGreen;

                        //Add the total volume to the running total after adjustments
                        PredictedVolume += sp.TotalVolume;
                    }
                }
            }
        }


        private string CreateChart(SignalPhase signalPhase, DateTime startDate, DateTime endDate, string location,
            string chartNameSuffix, string chartLocation, int maxYAxis, string directionBeforeAfter)
        {
            //Instantiate the chart object
            var chart = new Chart();

            //Add formatting to the chart
            chart = GetNewChart(startDate, endDate, signalPhase.Approach.SignalID,
                signalPhase.Approach.ProtectedPhaseNumber,
                signalPhase.Approach.DirectionType.Description, location, signalPhase.Approach.IsProtectedPhaseOverlap,
                maxYAxis, 2000, false, 2);

            //Add the data to the chart
            AddDataToChart(chart, signalPhase, startDate, false, true);

            //Add info to the based on direction and before or after adjustments
            if (directionBeforeAfter == "DownstreamBefore")
            {
                DownstreamBeforeTitle = chart.Titles[0].Text + " " + chart.Titles[1].Text;
                foreach (var l in chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels)
                    DownstreamBeforeTitle += " " + l.Text;
            }
            else if (directionBeforeAfter == "UpstreamBefore")
            {
                UpstreamBeforeTitle = chart.Titles[0].Text + " " + chart.Titles[1].Text;
                foreach (var l in chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels)
                    UpstreamBeforeTitle += " " + l.Text;
            }
            else if (directionBeforeAfter == "DownstreamAfter")
            {
                DownstreamAfterTitle = chart.Titles[0].Text + " " + chart.Titles[1].Text;
                foreach (var l in chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels)
                    DownstreamAfterTitle += " " + l.Text;
            }
            else if (directionBeforeAfter == "UpstreamAfter")
            {
                UpstreamAfterTitle = chart.Titles[0].Text + " " + chart.Titles[1].Text;
                foreach (var l in chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels)
                    UpstreamAfterTitle += " " + l.Text;
            }

            //Create the File Name
            var chartName = "LinkPivot-" +
                            signalPhase.Approach.SignalID +
                            "-" +
                            signalPhase.Approach.ProtectedPhaseNumber +
                            "-" +
                            startDate.Year +
                            startDate.ToString("MM") +
                            startDate.ToString("dd") +
                            startDate.ToString("HH") +
                            startDate.ToString("mm") +
                            "-" +
                            endDate.Year +
                            endDate.ToString("MM") +
                            endDate.ToString("dd") +
                            endDate.ToString("HH") +
                            endDate.ToString("mm-") +
                            DateTime.Now.Minute +
                            DateTime.Now.Second +
                            chartNameSuffix +
                            ".jpg";

            //Save an image of the chart
            chart.SaveImage(chartLocation + @"LinkPivot\" + chartName, ChartImageFormat.Jpeg);
            return chartName;
        }

        private Chart GetNewChart(DateTime graphStartDate, DateTime graphEndDate, string signalId,
            int phase, string direction, string location, bool isOverlap, double y1AxisMaximum,
            double y2AxisMaximum, bool showVolume, int dotSize)
        {
            //Instantiate the chart object
            var chart = new Chart();

            //used for Overlap
            var extendedDirection = string.Empty;
            var movementType = "Phase";
            if (isOverlap)
                movementType = "Overlap";


            //Gets direction for the title
            switch (direction)
            {
                case "SB":
                    extendedDirection = "Southbound";
                    break;
                case "NB":
                    extendedDirection = "Northbound";
                    break;
                default:
                    extendedDirection = direction;
                    break;
            }

            //Set the chart properties
            chart.ImageType = ChartImageType.Jpeg;
            chart.Height = 650;
            chart.Width = 1100;
            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;

            //Set the chart title
            var title = new Title();
            title.Text = location + " Signal " + signalId + " "
                         + movementType + ": " + phase +
                         " " + extendedDirection + "\n" + graphStartDate.ToString("f") +
                         " - " + graphEndDate.ToString("f");
            title.Font = new Font(FontFamily.GenericSansSerif, 20);
            chart.Titles.Add(title);
            chart.AlternateText = title.Text;

            //Create the chart area
            var chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            chartArea.AxisY.Maximum = y1AxisMaximum;
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Title = "Cycle Time (Seconds) ";
            chartArea.AxisY.TitleFont = new Font(FontFamily.GenericSansSerif, 20);
            chartArea.AxisY.LabelAutoFitStyle = LabelAutoFitStyles.None;
            chartArea.AxisY.LabelStyle.Font = new Font(FontFamily.GenericSansSerif, 20);

            //Adds the volume line if true
            if (showVolume)
            {
                chartArea.AxisY2.Enabled = AxisEnabled.True;
                chartArea.AxisY2.MajorTickMark.Enabled = true;
                chartArea.AxisY2.MajorGrid.Enabled = false;
                chartArea.AxisY2.IntervalType = DateTimeIntervalType.Number;
                chartArea.AxisY2.Interval = 500;
                chartArea.AxisY2.Maximum = y2AxisMaximum;
                chartArea.AxisY2.Title = "Volume Per Hour ";
            }

            //Add the X axis settings
            chartArea.AxisX.Title = "Time (Hour of Day)";
            chartArea.AxisX.TitleFont = new Font(FontFamily.GenericSansSerif, 20);
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.LabelStyle.Format = "HH";
            chartArea.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.None;
            chartArea.AxisX.LabelStyle.Font = new Font(FontFamily.GenericSansSerif, 20);

            //Add the second x axis settings 
            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisX2.MajorTickMark.Enabled = true;
            chartArea.AxisX2.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX2.LabelStyle.Format = "HH";
            chartArea.AxisX2.LabelAutoFitStyle = LabelAutoFitStyles.None;
            chartArea.AxisX2.Interval = 1;
            chartArea.AxisX2.LabelStyle.Font = new Font(FontFamily.GenericSansSerif, 20);

            //add the chart areas to the chart
            chart.ChartAreas.Add(chartArea);

            //Add the point series
            var pointSeries = new Series();
            pointSeries.ChartType = SeriesChartType.Point;
            pointSeries.Color = Color.Black;
            pointSeries.Name = "Detector Activation";
            pointSeries.XValueType = ChartValueType.DateTime;
            pointSeries.MarkerSize = dotSize;
            chart.Series.Add(pointSeries);

            //Add the green series
            var greenSeries = new Series();
            greenSeries.ChartType = SeriesChartType.Line;
            greenSeries.Color = Color.DarkGreen;
            greenSeries.Name = "Change to Green";
            greenSeries.XValueType = ChartValueType.DateTime;
            greenSeries.BorderWidth = 2;
            chart.Series.Add(greenSeries);

            //Add the yellow series
            var yellowSeries = new Series();
            yellowSeries.ChartType = SeriesChartType.Line;
            yellowSeries.Color = Color.Yellow;
            yellowSeries.Name = "Change to Yellow";
            yellowSeries.XValueType = ChartValueType.DateTime;
            yellowSeries.BorderWidth = 1;
            chart.Series.Add(yellowSeries);

            //Add the red series
            var redSeries = new Series();
            redSeries.ChartType = SeriesChartType.Line;
            redSeries.Color = Color.Red;
            redSeries.Name = "Change to Red";
            redSeries.XValueType = ChartValueType.DateTime;
            redSeries.BorderWidth = 3;
            chart.Series.Add(redSeries);

            //Add the red series
            var volumeSeries = new Series();
            volumeSeries.ChartType = SeriesChartType.Line;
            volumeSeries.Color = Color.Black;
            volumeSeries.Name = "Volume Per Hour";
            volumeSeries.XValueType = ChartValueType.DateTime;
            volumeSeries.YAxisType = AxisType.Secondary;
            chart.Series.Add(volumeSeries);


            //Add points at the start and and of the x axis to ensure
            //the graph covers the entire period selected by the user
            //whether there is data or not
            chart.Series["Detector Activation"].Points.AddXY(graphStartDate, 0);
            chart.Series["Detector Activation"].Points.AddXY(graphEndDate, 0);
            return chart;
        }


        private void AddDataToChart(Chart chart, SignalPhase signalPhase, DateTime startDate, bool showVolume,
            bool showArrivalOnGreen)
        {
            decimal totalDetectorHits = 0;
            decimal totalOnGreenArrivals = 0;
            decimal percentArrivalOnGreen = 0;
            foreach (var cycle in signalPhase.Cycles)
            {
                //add the data for the green line
                chart.Series["Change to Green"].Points.AddXY(
                    cycle.GreenEvent,
                    cycle.GreenLineY);

                //add the data for the yellow line
                chart.Series["Change to Yellow"].Points.AddXY(
                    cycle.YellowEvent,
                    cycle.YellowLineY);

                //add the data for the red line
                chart.Series["Change to Red"].Points.AddXY(
                    cycle.EndTime,
                    cycle.RedLineY);

                //add the detector hits to the running total
                totalDetectorHits += cycle.DetectorEvents.Count;

                //add the detector hits to the detector activation series
                foreach (var detectorPoint in cycle.DetectorEvents)
                {
                    chart.Series["Detector Activation"].Points.AddXY(
                        detectorPoint.TimeStamp,
                        detectorPoint.YPoint);

                    //if this is an arrival on green add it to the running total
                    if (detectorPoint.YPoint > cycle.GreenLineY && detectorPoint.YPoint < cycle.RedLineY)
                        totalOnGreenArrivals++;
                }
            }

            //add the volume data to the volume series if true
            if (showVolume)
                foreach (var v in signalPhase.Volume.Items)
                    chart.Series["Volume Per Hour"].Points.AddXY(v.XAxis, v.YAxis);

            //if arrivals on green is selected add the data to the chart
            if (showArrivalOnGreen)
            {
                if (totalDetectorHits > 0)
                    percentArrivalOnGreen = totalOnGreenArrivals / totalDetectorHits * 100;
                else
                    percentArrivalOnGreen = 0;
                var title = new Title();
                title.Text = Math.Round(percentArrivalOnGreen) + "% AoG";
                title.Font = new Font(FontFamily.GenericSansSerif, 20);
                chart.Titles.Add(title);


                SetPlanStrips(signalPhase.Plans, chart, startDate);
            }

            var mcr = MetricCommentRepositoryFactory.Create();

            var comment = mcr.GetLatestCommentForReport(signalPhase.Approach.SignalID, MetricTypeID);

            if (comment != null)
            {
                chart.Titles.Add(comment.CommentText);
                chart.Titles[1].Docking = Docking.Bottom;
                chart.Titles[1].ForeColor = Color.Red;
            }
        }


        /// <summary>
        ///     Adds plan strips to the chart
        /// </summary>
        /// <param name="planCollection"></param>
        /// <param name="chart"></param>
        /// <param name="graphStartDate"></param>
        protected void SetPlanStrips(List<PlanPcd> planCollection, Chart chart, DateTime graphStartDate)
        {
            var backGroundColor = 1;
            foreach (var plan in planCollection)
            {
                var stripline = new StripLine();
                //Creates alternating backcolor to distinguish the plans
                if (backGroundColor % 2 == 0)
                    stripline.BackColor = Color.FromArgb(120, Color.LightGray);
                else
                    stripline.BackColor = Color.FromArgb(120, Color.LightBlue);

                //Set the stripline properties
                stripline.IntervalOffset = (plan.StartTime - graphStartDate).TotalHours;
                stripline.IntervalOffsetType = DateTimeIntervalType.Hours;
                stripline.Interval = 1;
                stripline.IntervalType = DateTimeIntervalType.Days;
                stripline.StripWidth = (plan.EndTime - plan.StartTime).TotalHours;
                stripline.StripWidthType = DateTimeIntervalType.Hours;

                //Add the stripline to the chart
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

                Plannumberlabel.ForeColor = Color.Black;
                Plannumberlabel.RowIndex = 3;
                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(Plannumberlabel);

                //add arrivals on gren to the plan strip
                var aogLabel = new CustomLabel();
                aogLabel.FromPosition = plan.StartTime.ToOADate();
                aogLabel.ToPosition = plan.EndTime.ToOADate();
                aogLabel.Text = plan.PercentArrivalOnGreen + "% AoG\n" +
                                plan.PercentGreenTime + "% GT";
                aogLabel.LabelMark = LabelMarkStyle.LineSideMark;
                aogLabel.ForeColor = Color.Blue;
                aogLabel.RowIndex = 2;
                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(aogLabel);

                //add the platoon ratio to the plan strip
                var statisticlabel = new CustomLabel();
                statisticlabel.FromPosition = plan.StartTime.ToOADate();
                statisticlabel.ToPosition = plan.EndTime.ToOADate();
                statisticlabel.Text =
                    plan.PlatoonRatio + " PR";
                statisticlabel.ForeColor = Color.Maroon;
                statisticlabel.RowIndex = 1;
                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(statisticlabel);

                //Change the background color counter for alternating color
                backGroundColor++;
            }
        }
    }
}