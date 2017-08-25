using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common;

namespace MOEWcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PhaseTermination" in both code and config file together.
    public class PhaseTermination : IPhaseTermination
    {
        private int MetricTypeID = 1;

        public string CreateChart(DateTime startDate, 
            DateTime endDate, 
            string signalId, 
            bool showPedWalkStartTime,
            int consecutiveCount, 
            string location, 
            bool showPlanStripes
            )
        {
            string returnString = string.Empty;

            string chartLocation = ConfigurationManager.AppSettings["ImageLocation"] + @"Phase Termination Charts\";


            string chartName = "PPT-" +
                                signalId +
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
                                consecutiveCount.ToString();

                    if (showPedWalkStartTime)
                    {
                        chartName += "-PED";
                    }
                    if (showPlanStripes)
                    {
                        chartName += "-PLN";
                    }
                    Random r = new Random();
                    chartName += r.Next().ToString();
                    chartName += ".jpg";

            //if (endDate <= DateTime.Today)
            //{
            //    if (File.Exists(chartLocation + chartName))
            //    {
            //        returnString = chartName;
            //    }
                
            //}

            if (returnString == string.Empty)
            {
                Chart chart = new Chart();
                MOE.Common.Business.AnalysisPhaseCollection analysisPhaseCollection =
                               new MOE.Common.Business.AnalysisPhaseCollection(signalId, startDate,
                                   endDate, consecutiveCount);

                //If there are phases in the collection add the charts
                if (analysisPhaseCollection.Items.Count > 0)
                {
                    chart = GetNewTermEventChart(startDate, endDate, signalId, location,
                        consecutiveCount, analysisPhaseCollection.MaxPhaseInUse, showPedWalkStartTime);
                    AddTermEventDataToChart(chart, startDate, endDate, analysisPhaseCollection, signalId,
                        showPedWalkStartTime, showPlanStripes);
                }

                chart.SaveImage(chartLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);
                returnString = chartName;
            }

            return returnString;
        }


        /// <summary>
        /// Gets a new chart for the Termination Event Diagrams
        /// </summary>

        /// <returns>Chart</returns>
        protected Chart GetNewTermEventChart(DateTime graphStartDate, DateTime graphEndDate, 
            string signalId, string location, int consecutiveCount,
            int maxPhaseInUse, bool showPedWalkStartTime)
        {            
            Chart chart = new Chart();
            string extendedDirection = string.Empty;




            //Set the chart properties
            chart.ImageType = ChartImageType.Jpeg;
            chart.Height = 450;
            chart.Width = 1100;
            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
            chart.BorderlineDashStyle = ChartDashStyle.Dot;


            //Set the chart title
            chart.Titles.Add(location + "Signal " + signalId.ToString() + " "
                + "\n" + graphStartDate.ToString("f") + " - " + graphEndDate.ToString("f"));


            chart.Titles.Add("Currently showing Force-Offs, Max-Outs and Gap-Outs with a consecutive occurrence of " + 
                consecutiveCount.ToString() + " or more. \n  Pedestrian events are never filtered");



            //MOE.Common.Models.Repositories.IDetectorCommentRepository dcr = MOE.Common.Models.Repositories.DetectorCommentRepositoryFactory.Create();

            //MOE.Common.Models.Repositories.IMetricCommentRepository mcr = MOE.Common.Models.Repositories.MetricCommentRepositoryFactory.Create();

            //MOE.Common.Models.MetricComment comment = mcr.GetLatestCommentForReport(signalId, MetricTypeID);

            //if (comment != null)
            //{

            //    chart.Titles.Add(comment.CommentText);
            //    chart.Titles[1].Docking = Docking.Bottom;
            //    chart.Titles[1].ForeColor = Color.Red;
            //}





            //Create the chart area
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            chartArea.AxisY.Maximum = (maxPhaseInUse + .5);
            chartArea.AxisY.Minimum = -.5;
            


            chartArea.AxisX.Title = "Time (Hour of Day)";
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.LabelStyle.Format = "HH";


            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisX2.MajorTickMark.Enabled = true;
            chartArea.AxisX2.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX2.LabelStyle.Format = "HH";
            chartArea.AxisX2.LabelAutoFitStyle = LabelAutoFitStyles.None;
            chartArea.AxisX2.Interval = 1;

            chartArea.AxisY.Title = "Phase Number";
            //chartArea.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chartArea.AxisY.Interval = 1;
            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea.AxisY.IntervalOffset = .5;

            chart.ChartAreas.Add(chartArea);


            Series GapoutSeries = new Series();
            GapoutSeries.ChartType = SeriesChartType.Point;
            GapoutSeries.Color = Color.OliveDrab;
            GapoutSeries.Name = "GapOut";
            GapoutSeries.XValueType = ChartValueType.DateTime;
            GapoutSeries.MarkerStyle = MarkerStyle.Circle;
            GapoutSeries.MarkerSize = 3;


            Series MaxOutSeries = new Series();
            MaxOutSeries.ChartType = SeriesChartType.Point;
            MaxOutSeries.Color = Color.Red;
            MaxOutSeries.Name = "MaxOut";
            MaxOutSeries.XValueType = ChartValueType.DateTime;
            MaxOutSeries.MarkerStyle = MarkerStyle.Cross;
            MaxOutSeries.MarkerSize = 4;


            Series ForceOffSeries = new Series();
            ForceOffSeries.ChartType = SeriesChartType.Point;
            ForceOffSeries.Color = Color.MediumBlue;
            ForceOffSeries.Name = "ForceOff";
            ForceOffSeries.MarkerStyle = MarkerStyle.Circle;
            ForceOffSeries.MarkerSize = 4;

            Series UnknownTermination = new Series();
            UnknownTermination.ChartType = SeriesChartType.Point;
            UnknownTermination.Color = Color.Gray;
            UnknownTermination.Name = "Unknown";
            UnknownTermination.MarkerStyle = MarkerStyle.Circle;
            UnknownTermination.MarkerSize = 3;

            Series PedSeries = new Series();
            PedSeries.ChartType = SeriesChartType.Point;
            PedSeries.Color = Color.OrangeRed;
            PedSeries.Name = "Ped Walk Begin";
            //PedSeries.MarkerImage = 
            PedSeries.MarkerStyle = MarkerStyle.Triangle;
            PedSeries.MarkerSize = 3;

            if (showPedWalkStartTime)
            {
                PedSeries.IsVisibleInLegend = true;
            }
            else
            {
                PedSeries.IsVisibleInLegend = false;
            }


            //ForceOffSeries.BorderWidth = 2;

            chart.Series.Add(GapoutSeries);
            chart.Series.Add(MaxOutSeries);
            chart.Series.Add(ForceOffSeries);
            chart.Series.Add(PedSeries);
            chart.Series.Add(UnknownTermination);

            //Add the Posts series to ensure the chart is the size of the selected timespan
            Series testSeries = new Series();
            testSeries.IsVisibleInLegend = false;
            testSeries.ChartType = SeriesChartType.Point;
            testSeries.Color = Color.White;
            testSeries.Name = "Posts";
            testSeries.XValueType = ChartValueType.DateTime;
            //GapoutSeries.MarkerSize = 0;
            chart.Series.Add(testSeries);





            //Add points at the start and and of the x axis to ensure
            //the graph covers the entire period selected by the user
            //whether there is data or not
            chart.Series["Posts"].Points.AddXY(graphStartDate, 0);
            chart.Series["Posts"].Points.AddXY(graphEndDate.AddMinutes(5), 0);
            return chart;
        }

        protected void AddTermEventDataToChart(Chart chart, DateTime startDate,
          DateTime endDate, MOE.Common.Business.AnalysisPhaseCollection analysisPhaseCollection, 
            string signalId, bool showVolume, bool showPlanStripes)
        {
            foreach (MOE.Common.Business.AnalysisPhase phase in analysisPhaseCollection.Items)
            {

                if (phase.TerminationEvents.Count > 0)
                {

                    foreach (MOE.Common.Business.ControllerEvent TermEvent in phase.ConsecutiveGapOuts)
                    {

                        chart.Series["GapOut"].Points.AddXY(TermEvent.TimeStamp, phase.PhaseNumber);
                    }

                    foreach (MOE.Common.Business.ControllerEvent TermEvent in phase.ConsecutiveMaxOut)
                    {
                        chart.Series["MaxOut"].Points.AddXY(TermEvent.TimeStamp, phase.PhaseNumber);
                    }

                    foreach (MOE.Common.Business.ControllerEvent TermEvent in phase.ConsecutiveForceOff)
                    {
                        chart.Series["ForceOff"].Points.AddXY(TermEvent.TimeStamp, phase.PhaseNumber);
                    }

                    foreach (MOE.Common.Business.ControllerEvent TermEvent in phase.UnknownTermination)
                    {
                        chart.Series["UnknownTermination"].Points.AddXY(TermEvent.TimeStamp, phase.PhaseNumber);
                    }

                    if (showVolume)
                    {
                        foreach (MOE.Common.Business.ControllerEvent PedEvent in phase.PedestrianEvents)
                        {
                            chart.Series["Ped Walk Begin"].Points.AddXY(PedEvent.TimeStamp, (phase.PhaseNumber + .3));
                        }
                    }

                }
                if (showPlanStripes)
                {
                    SetSimplePlanStrips(analysisPhaseCollection.Plans, chart, startDate);
                }
            }

        }

        /// <summary>
        /// Adds plan strips that lack many of the statisctics to the chart
        /// </summary>
        /// <param name="planCollection"></param>
        /// <param name="chart"></param>
        /// <param name="graphStartDate"></param>
        protected void SetSimplePlanStrips(MOE.Common.Business.PlanCollection planCollection, Chart chart, DateTime graphStartDate)
        {
            int backGroundColor = 1;
            foreach (MOE.Common.Business.Plan plan in planCollection.PlanList)
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
                stripline.IntervalOffsetType = DateTimeIntervalType.Hours;
                stripline.Interval = 1;
                stripline.IntervalOffset = (plan.StartTime - graphStartDate).TotalHours;
                stripline.StripWidth = (plan.EndTime - plan.StartTime).TotalHours;
                stripline.StripWidthType = DateTimeIntervalType.Hours;

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
                Plannumberlabel.LabelMark = LabelMarkStyle.LineSideMark;
                Plannumberlabel.ForeColor = Color.Black;
                Plannumberlabel.RowIndex = 6;


                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(Plannumberlabel);


                backGroundColor++;

            }
        }
    }
}
