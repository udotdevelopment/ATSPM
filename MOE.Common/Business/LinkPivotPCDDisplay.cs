using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;

namespace MOE.Common.Business
{
    /// <summary>
    /// Creates PCDs based on a Link Pivot Analysis
    /// </summary>
    public class LinkPivotPCDDisplay
    {

        private int MetricTypeID = 13;

        //File location of the upstream before pcd
        private string upstreamBeforePCDPath;
        public string UpstreamBeforePCDPath
        {
            get { return upstreamBeforePCDPath; }
            set { upstreamBeforePCDPath = value; }
        }

        //File location of the downstream before pcd
        private string downstreamBeforePCDPath;
        public string DownstreamBeforePCDPath
        {
            get { return downstreamBeforePCDPath; }
            set { downstreamBeforePCDPath = value; }
        }

        //Create titles for the charts so that they can later be added for accesability
        public string DownstreamBeforeTitle { get; set; }
        public string UpstreamBeforeTitle { get; set; }
        public string DownstreamAfterTitle { get; set; }
        public string UpstreamAfterTitle { get; set; }

        //File location of the upstream after adjustment pcd
        private string upstreamAfterPCDPath;
        public string UpstreamAfterPCDPath
        {
            get { return upstreamAfterPCDPath; }
            set { upstreamAfterPCDPath = value; }
        }

        //File location of the downstream after adjustment pcd
        private string downstreamAfterPCDPath;
        public string DownstreamAfterPCDPath
        {
            get { return downstreamAfterPCDPath; }
            set { downstreamAfterPCDPath = value; }
        }

        //Total Arrival On Green before adjustments
        private double existingTotalAOG=0;
        public double ExistingTotalAOG
        {
            get { return existingTotalAOG; }
            set { existingTotalAOG = value; }
        }

        //Total Percent Arrival On Green before adjustments
        private double existingTotalPAOG = 0;
        public double ExistingTotalPAOG
        {
            get { return existingTotalPAOG; }
            set { existingTotalPAOG = value; }
        }

        //Total arrival on green after adjustments
        private double predictedTotalAOG = 0;
        public double PredictedTotalAOG
        {
            get { return predictedTotalAOG; }
            set { predictedTotalAOG = value; }
        }

        //Total percent arrival on green after adjustments
        private double predictedTotalPAOG = 0;
        public double PredictedTotalPAOG
        {
            get { return predictedTotalPAOG; }
            set { predictedTotalPAOG = value; }
        }

        //Total volume after adjustments
        private double predictedVolume;
        public double PredictedVolume
        {
            get { return predictedVolume; }
            set { predictedVolume = value; }
        }

        //Total volume before adjustments
        private double existingVolume = 0;
        public double ExistingVolume
        {
            get { return existingVolume; }
            set { existingVolume = value; }
        }

        /// <summary>
        /// Generates PCD charts for upstream and downstream detectors based on 
        /// a Link Pivot Link
        /// </summary>
        /// <param name="upstreamSignalID"></param>
        /// <param name="upstreamDirection"></param>
        /// <param name="downstreamSignalID"></param>
        /// <param name="downstreamDirection"></param>
        /// <param name="delta"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="maxYAxis"></param>
        public LinkPivotPCDDisplay(string upstreamSignalID, string upstreamDirection,
            string downstreamSignalID, string downstreamDirection, int delta, 
            DateTime startDate, DateTime endDate, int maxYAxis)
        {
            MOE.Common.Models.Repositories.ISignalsRepository signalRepository =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            MOE.Common.Models.Signal upstreamSignal = signalRepository.GetSignalBySignalID(upstreamSignalID);
            MOE.Common.Models.Signal downstreamSignal = signalRepository.GetSignalBySignalID(downstreamSignalID);
            MOE.Common.Models.Approach upApproachToAnalyze = GetApproachToAnalyze(upstreamSignal, upstreamDirection);
            MOE.Common.Models.Approach downApproachToAnalyze = GetApproachToAnalyze(downstreamSignal, downstreamDirection);
            
            if (upApproachToAnalyze != null)
            {
                GeneratePCD(upApproachToAnalyze, delta, startDate, endDate, true, maxYAxis);
            }
            if (downApproachToAnalyze != null)
            {
                GeneratePCD(downApproachToAnalyze, delta, startDate, endDate, false, maxYAxis);
            }
        }

        private Models.Approach GetApproachToAnalyze(Models.Signal signal, string direction)
        {
            Models.Approach approachToAnalyze = null;
            var approaches = signal.Approaches.Where(a => a.DirectionType.Description == direction).ToList();
            foreach (var approach in approaches)
            {
                if (approach.GetDetectorsForMetricType(6).Count > 0)
                {
                    approachToAnalyze = approach;
                }
            }
            return approachToAnalyze;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signalId"></param>
        /// <param name="direction"></param>
        /// <param name="delta"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="upstream"></param>
        /// <param name="maxYAxis"></param>
        private void GeneratePCD(Models.Approach approach, int delta, 
            DateTime startDate, DateTime endDate, bool upstream, int maxYAxis)
        {
            MOE.Common.Models.Repositories.ISignalsRepository repository =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            //var signal = repository.GetSignalBySignalID(approach.SignalId);
            
            //Create a location string to show the combined cross strees
            string location = string.Empty;
            if(approach.Signal != null)
            {
                location = approach.Signal.PrimaryName + " " + approach.Signal.SecondaryName;
            }
            string chartName = string.Empty;
             //MOE.Common.Data.LinkPivotTableAdapters.Graph_DetectorsTableAdapter gdAdapter =
             //  new Data.LinkPivotTableAdapters.Graph_DetectorsTableAdapter(); 
            
            //find the upstream approach
            if (!String.IsNullOrEmpty(approach.DirectionType.Description))
            {
                //Find PCD detector for this appraoch
                 MOE.Common.Models.Detector gd =
                     approach.Signal.GetDetectorsForSignalThatSupportAMetricByApproachDirection(
                     6, approach.DirectionType.Description).FirstOrDefault();
                         
                //Check for null value
                if(gd != null)
                {
                    //Instantiate a signal phase object
                    SignalPhase sp = new MOE.Common.Business.SignalPhase(
                                            startDate, endDate,approach, false,
                                            15,13);                    
                    
                    //Check the direction of the Link Pivot
                    if (upstream)
                    {
                        //Create a chart for the upstream detector before adjustments
                        upstreamBeforePCDPath = CreateChart(sp, startDate, endDate, location, "before",
                            ConfigurationManager.AppSettings["ImageLocation"], maxYAxis, "UpstreamBefore");

                        //Add the total arrival on green before adjustments to the running total
                        existingTotalAOG += sp.TotalArrivalOnGreen;

                        //Add the volume from the signal phase to the running total
                        existingVolume += sp.TotalVolume;

                        //Re run the signal phase by the optimized delta change to get the adjusted pcd
                        sp.LinkPivotAddSeconds(delta*-1);                        

                        //Create a chart for the upstream detector after adjustments
                        upstreamAfterPCDPath = CreateChart(sp, startDate, endDate, location, "after",
                            ConfigurationManager.AppSettings["ImageLocation"], maxYAxis, "UpstreamAfter");

                        //Add the total arrival on green after adjustments to the running total
                        predictedTotalAOG += sp.TotalArrivalOnGreen;

                        //Add the volume from the signal phase to the running total
                        predictedVolume += sp.TotalVolume;
                    }
                    else
                    {
                        //Create a chart for downstream detector before adjustments
                        downstreamBeforePCDPath = CreateChart(sp, startDate, endDate, location, "before",
                            ConfigurationManager.AppSettings["ImageLocation"], maxYAxis, "DownstreamBefore");

                        //Add the arrivals on green to the total arrivals on green running total
                        existingTotalAOG += sp.TotalArrivalOnGreen;

                        //Add the volume before adjustments to the running total volume
                        existingVolume += sp.TotalVolume;

                        //Re run the signal phase by the optimized delta change to get the adjusted pcd
                        sp.LinkPivotAddSeconds(delta);

                        //Create a pcd chart for downstream after adjustments
                        downstreamAfterPCDPath = CreateChart(sp, startDate, endDate, location, "after",
                            ConfigurationManager.AppSettings["ImageLocation"], maxYAxis, "DownstreamAfter");

                        //Add the total arrivals on green to the running total
                        predictedTotalAOG += sp.TotalArrivalOnGreen;

                        //Add the total volume to the running total after adjustments
                        predictedVolume += sp.TotalVolume;
                    }

                    

                }
            }
        }

        /// <summary>
        /// Creates a pcd chart specific to the Link Pivot
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="location"></param>
        /// <param name="chartNameSuffix"></param>
        /// <param name="chartLocation"></param>
        /// <returns>Chart Name</returns>
        private string CreateChart(SignalPhase sp, DateTime startDate, DateTime endDate, string location,
            string chartNameSuffix, string chartLocation, int maxYAxis, string directionBeforeAfter)
        {
            //Instantiate the chart object
            Chart chart = new Chart();

            //Add formatting to the chart
            chart = GetNewChart(startDate, endDate, sp.Approach.SignalID, sp.Approach.ProtectedPhaseNumber,
                sp.Approach.DirectionType.Description, location, sp.Approach.IsProtectedPhaseOverlap, 
                maxYAxis, 2000, false, 2);

            //Add the data to the chart
            AddDataToChart(chart, sp, startDate, endDate, false, true);
            
            //Add info to the based on direction and before or after adjustments
            if(directionBeforeAfter == "DownstreamBefore")
            {
                DownstreamBeforeTitle = chart.Titles[0].Text + " " + chart.Titles[1].Text;
                foreach(var l in chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels)
                {
                    DownstreamBeforeTitle += " " + l.Text;
                }
            }
            else if (directionBeforeAfter == "UpstreamBefore")
            {
                UpstreamBeforeTitle = chart.Titles[0].Text + " " + chart.Titles[1].Text;
                foreach (var l in chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels)
                {
                    UpstreamBeforeTitle += " " + l.Text;
                }
            }
            else if (directionBeforeAfter == "DownstreamAfter")
            {
                DownstreamAfterTitle = chart.Titles[0].Text + " " + chart.Titles[1].Text;
                foreach (var l in chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels)
                {
                    DownstreamAfterTitle += " " + l.Text;
                }
            }
            else if (directionBeforeAfter == "UpstreamAfter")
            {
                UpstreamAfterTitle = chart.Titles[0].Text + " " + chart.Titles[1].Text;
                foreach (var l in chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels)
                {
                    UpstreamAfterTitle += " " + l.Text;
                }
            }

            //Create the File Name
            string chartName = "LinkPivot-" +
                sp.Approach.SignalID +
                "-" +
                sp.Approach.ProtectedPhaseNumber.ToString() +
                "-" +
                startDate.Year.ToString() +
                startDate.ToString("MM") +
                startDate.ToString("dd") +
                startDate.ToString("HH") +
                startDate.ToString("mm") +
                "-" +
                endDate.Year.ToString() +
                endDate.ToString("MM") +
                endDate.ToString("dd") +
                endDate.ToString("HH") +
                endDate.ToString("mm-") +
                DateTime.Now.Minute.ToString()+
                DateTime.Now.Second.ToString()+
                chartNameSuffix +
                ".jpg";

            //Save an image of the chart
            chart.SaveImage(chartLocation + @"LinkPivot\" + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);
            return chartName;
        }

        /// <summary>
        /// Gets a new chart for the pcd Diagram
        /// </summary>
        /// <param name="graphStartDate"></param>
        /// <param name="graphEndDate"></param>
        /// <param name="signalId"></param>
        /// <param name="phase"></param>
        /// <param name="direction"></param>
        /// <param name="location"></param>
        /// <returns>Chart object</returns>
        private Chart GetNewChart(DateTime graphStartDate, DateTime graphEndDate, string signalId,
            int phase, string direction, string location, bool isOverlap, double y1AxisMaximum,
            double y2AxisMaximum, bool showVolume, int dotSize)
        {
            //Instantiate the chart object
            Chart chart = new Chart();

            //used for Overlap
            string extendedDirection = string.Empty;
            string movementType = "Phase";
            if (isOverlap)
            {
                movementType = "Overlap";
            }


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
            Title title = new Title();
            title.Text = location + " Signal " + signalId.ToString() + " "
                + movementType + ": " + phase.ToString() +
                " " + extendedDirection + "\n" + graphStartDate.ToString("f") +
                " - " + graphEndDate.ToString("f");
            title.Font = new Font(FontFamily.GenericSansSerif, 20);
            chart.Titles.Add(title);
            chart.AlternateText = title.Text;

            //Create the chart area
            ChartArea chartArea = new ChartArea();
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
            Series pointSeries = new Series();
            pointSeries.ChartType = SeriesChartType.Point;
            pointSeries.Color = Color.Black;
            pointSeries.Name = "Detector Activation";
            pointSeries.XValueType = ChartValueType.DateTime;
            pointSeries.MarkerSize = dotSize;
            chart.Series.Add(pointSeries);

            //Add the green series
            Series greenSeries = new Series();
            greenSeries.ChartType = SeriesChartType.Line;
            greenSeries.Color = Color.DarkGreen;
            greenSeries.Name = "Change to Green";
            greenSeries.XValueType = ChartValueType.DateTime;
            greenSeries.BorderWidth = 2;
            chart.Series.Add(greenSeries);

            //Add the yellow series
            Series yellowSeries = new Series();
            yellowSeries.ChartType = SeriesChartType.Line;
            yellowSeries.Color = Color.Yellow;
            yellowSeries.Name = "Change to Yellow";
            yellowSeries.XValueType = ChartValueType.DateTime;
            yellowSeries.BorderWidth = 1;
            chart.Series.Add(yellowSeries);

            //Add the red series
            Series redSeries = new Series();
            redSeries.ChartType = SeriesChartType.Line;
            redSeries.Color = Color.Red;
            redSeries.Name = "Change to Red";
            redSeries.XValueType = ChartValueType.DateTime;
            redSeries.BorderWidth = 3;
            chart.Series.Add(redSeries);

            //Add the red series
            Series volumeSeries = new Series();
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

        /// <summary>
        /// Adds data points to a graph with the series GreenLine, YellowLine, Redline
        /// and Points already added.
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="signalPhase"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="signalId"></param>
        private void AddDataToChart(Chart chart, MOE.Common.Business.SignalPhase signalPhase, DateTime startDate,
            DateTime endDate, bool showVolume, bool showArrivalOnGreen)
        {
          
            decimal totalDetectorHits = 0;
            decimal totalOnGreenArrivals = 0;
            decimal percentArrivalOnGreen = 0;

            //Add the data by plan
            foreach (MOE.Common.Business.Plan plan in signalPhase.Plans.PlanList)
            {
                //check for empty pcd collection
                if (plan.CycleCollection.Count > 0)
                {
                    foreach (MOE.Common.Business.Cycle pcd in plan.CycleCollection)
                    {
                        //add the data for the green line
                        chart.Series["Change to Green"].Points.AddXY(
                            pcd.GreenEvent,
                            pcd.GreenLineY);

                        //add the data for the yellow line
                        chart.Series["Change to Yellow"].Points.AddXY(
                            pcd.YellowEvent,
                            pcd.YellowLineY);

                        //add the data for the red line
                        chart.Series["Change to Red"].Points.AddXY(
                            pcd.EndTime,
                            pcd.RedLineY);

                        //add the detector hits to the running total
                        totalDetectorHits += pcd.DetectorCollection.Count;

                        //add the detector hits to the detector activation series
                        foreach (MOE.Common.Business.DetectorDataPoint detectorPoint in pcd.DetectorCollection)
                        {
                            chart.Series["Detector Activation"].Points.AddXY(
                                detectorPoint.TimeStamp,
                                detectorPoint.YPoint);

                            //if this is an arrival on green add it to the running total
                            if (detectorPoint.YPoint > pcd.GreenLineY && detectorPoint.YPoint < pcd.RedLineY)
                            {
                                totalOnGreenArrivals++;
                            }
                        }
                    }
                }
            }

            //add the volume data to the volume series if true
            if (showVolume)
            {
                foreach (MOE.Common.Business.Volume v in signalPhase.Volume.Items)
                {
                    chart.Series["Volume Per Hour"].Points.AddXY(v.XAxis, v.YAxis);
                }
            }

            //if arrivals on green is selected add the data to the chart
            if (showArrivalOnGreen)
            {
                if (totalDetectorHits > 0)
                {
                    percentArrivalOnGreen = (totalOnGreenArrivals / totalDetectorHits) * 100;
                }
                else
                {
                    percentArrivalOnGreen = 0;
                }
                Title title = new Title();
                title.Text = Math.Round(percentArrivalOnGreen).ToString() + "% AoG";
                 title.Font = new Font(FontFamily.GenericSansSerif, 20);
                 chart.Titles.Add(title);


                SetPlanStrips(signalPhase.Plans.PlanList, chart, startDate);
            }

            MOE.Common.Models.Repositories.IMetricCommentRepository mcr = MOE.Common.Models.Repositories.MetricCommentRepositoryFactory.Create();

            MOE.Common.Models.MetricComment comment = mcr.GetLatestCommentForReport(signalPhase.Approach.SignalID, MetricTypeID);

            if (comment != null)
            {

                chart.Titles.Add(comment.CommentText);
                chart.Titles[1].Docking = Docking.Bottom;
                chart.Titles[1].ForeColor = Color.Red;
            }



            
        }


        /// <summary>
        /// Adds plan strips to the chart
        /// </summary>
        /// <param name="planCollection"></param>
        /// <param name="chart"></param>
        /// <param name="graphStartDate"></param>
        protected void SetPlanStrips(List<MOE.Common.Business.Plan> planCollection, Chart chart, DateTime graphStartDate)
        {
            int backGroundColor = 1;
            foreach (MOE.Common.Business.Plan plan in planCollection)
            {
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
                stripline.IntervalOffset = (plan.StartTime - graphStartDate).TotalHours;
                stripline.IntervalOffsetType = DateTimeIntervalType.Hours;
                stripline.Interval = 1;
                stripline.IntervalType = DateTimeIntervalType.Days;               
                stripline.StripWidth = (plan.EndTime - plan.StartTime).TotalHours;
                stripline.StripWidthType = DateTimeIntervalType.Hours;

                //Add the stripline to the chart
                chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

                //Add a corrisponding custom label for each strip
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
                Plannumberlabel.RowIndex = 3;
                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(Plannumberlabel);

                //add arrivals on gren to the plan strip
                CustomLabel aogLabel = new CustomLabel();
                aogLabel.FromPosition = plan.StartTime.ToOADate();
                aogLabel.ToPosition = plan.EndTime.ToOADate();
                aogLabel.Text = plan.PercentArrivalOnGreen.ToString() + "% AoG\n" +
                    plan.PercentGreen.ToString() + "% GT";
                aogLabel.LabelMark = LabelMarkStyle.LineSideMark;
                aogLabel.ForeColor = Color.Blue;
                aogLabel.RowIndex = 2;
                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(aogLabel);

                //add the platoon ratio to the plan strip
                CustomLabel statisticlabel = new CustomLabel();
                statisticlabel.FromPosition = plan.StartTime.ToOADate();
                statisticlabel.ToPosition = plan.EndTime.ToOADate();
                statisticlabel.Text =
                    plan.PlatoonRatio.ToString() + " PR";
                statisticlabel.ForeColor = Color.Maroon;
                statisticlabel.RowIndex = 1;
                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(statisticlabel);

                //Change the background color counter for alternating color
                backGroundColor++;

            }
        }

        
    }
}
