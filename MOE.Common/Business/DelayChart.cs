using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace MOE.Common.Business
{
    public class DelayChart
    {

        private int MetricTypeID = 8;
        public Chart chart = new Chart();
        public WCFServiceLibrary.ApproachDelayOptions Options  { get; set; }


        public DelayChart(WCFServiceLibrary.ApproachDelayOptions options, MOE.Common.Business.SignalPhase signalPhase)
        {
            Options = options;
            TimeSpan reportTimespan = Options.EndDate - Options.StartDate;
            string extendedDirection = signalPhase.Approach.DirectionType.Description;
            //Set the chart properties
            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
            chart.ImageType = ChartImageType.Jpeg;
            chart.Height = 550;
            chart.Width = 1100;
            

            //Create the chart legend
            Legend chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            chart.Legends.Add(chartLegend);


            //Create the chart area
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";

            //Primary Y axis (delay per vehicle)
            if (Options.ShowDelayPerVehicle)
            {
                if (Options.YAxisMax != null)
                {
                    chartArea.AxisY.Maximum = Options.YAxisMax.Value;
                }
                chartArea.AxisY.Minimum = 0;
                chartArea.AxisY.Enabled = AxisEnabled.True;
                chartArea.AxisY.MajorTickMark.Enabled = true;
                chartArea.AxisY.MajorGrid.Enabled = true;
                chartArea.AxisY.Interval = 5;
                chartArea.AxisY.TitleForeColor = Color.Blue;
                chartArea.AxisY.Title = "Delay Per Vehicle (Seconds) ";
            }

            //secondary y axis (total delay)
            if (Options.ShowDelayPerVehicle)
            {
                if (Options.Y2AxisMax != null && Options.Y2AxisMax > 0)
                {
                    chartArea.AxisY2.Maximum = Options.Y2AxisMax.Value;
                }
                else
                {
                    chartArea.AxisY2.Maximum = 50000;
                }
                chartArea.AxisY2.Minimum = 0;
                chartArea.AxisY2.Enabled = AxisEnabled.True;
                chartArea.AxisY2.MajorTickMark.Enabled = true;
                chartArea.AxisY2.MajorGrid.Enabled = false;
                chartArea.AxisY2.Interval = 5000;
                chartArea.AxisY2.Title = "Delay per Hour (Seconds) ";
                chartArea.AxisY2.TitleForeColor = Color.Red;
                
            }
            chartArea.AxisX.Title = "Time (Hour of Day)";
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.LabelStyle.Format = "HH";
            chartArea.AxisX2.LabelStyle.Format = "HH";
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
            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisX2.MajorTickMark.Enabled = true;
            chartArea.AxisX2.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX2.LabelAutoFitStyle = LabelAutoFitStyles.None;

            chart.ChartAreas.Add(chartArea);


            //Add the point series

            Series delayPerVehicleSeries = new Series();
            delayPerVehicleSeries.ChartType = SeriesChartType.Line;
            delayPerVehicleSeries.Color = Color.Blue;
            delayPerVehicleSeries.Name = "Approach Delay Per Vehicle";
            delayPerVehicleSeries.YAxisType = AxisType.Primary;
            delayPerVehicleSeries.XValueType = ChartValueType.DateTime;

            Series delaySeries = new Series();
            delaySeries.ChartType = SeriesChartType.Line;
            delaySeries.Color = Color.Red;
            delaySeries.Name = "Approach Delay";
            delaySeries.YAxisType = AxisType.Secondary;
            delaySeries.XValueType = ChartValueType.DateTime;




            Series pointSeries = new Series();
            pointSeries.ChartType = SeriesChartType.Point;
            pointSeries.Color = Color.White;
            pointSeries.Name = "Posts";
            pointSeries.XValueType = ChartValueType.DateTime;
            pointSeries.IsVisibleInLegend = false;
            
            
            chart.Series.Add(pointSeries);
            chart.Series.Add(delaySeries);
            chart.Series.Add(delayPerVehicleSeries);



            //Add points at the start and and of the x axis to ensure
            //the graph covers the entire period selected by the user
            //whether there is data or not
            chart.Series["Posts"].Points.AddXY(Options.StartDate, 0);
            chart.Series["Posts"].Points.AddXY(Options.EndDate, 0);

            AddDataToChart(chart, signalPhase, Options.StartDate, Options.EndDate, Options.SelectedBinSize, 
                signalPhase.Approach.SignalID, Options.ShowTotalDelayPerHour, Options.ShowDelayPerVehicle);
            SetPlanStrips(signalPhase.Plans.PlanList, chart, Options.StartDate, Options.ShowPlanStatistics);
        }

        private void SetChartTitles(SignalPhase signalPhase, Dictionary<string, string> statistics)
        {
            chart.Titles.Add(ChartTitleFactory.GetChartName(Options.MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(
                Options.SignalID, Options.StartDate, Options.EndDate));
            if (!signalPhase.Approach.IsProtectedPhaseOverlap)
            {
                chart.Titles.Add(ChartTitleFactory.GetPhaseAndPhaseDescriptions(signalPhase.Approach.ProtectedPhaseNumber, signalPhase.Approach.DirectionType.Description));
            }
            else
            {
                chart.Titles.Add(ChartTitleFactory.GetPhaseAndPhaseDescriptions(signalPhase.Approach.ProtectedPhaseNumber, " Overlap"));
            }
            chart.Titles.Add(ChartTitleFactory.GetStatistics(statistics));
            chart.Titles.Add(ChartTitleFactory.GetTitle("Simplified Approach Delay. Displays time between approach activation during the red phase and when the phase turns green."
                               + " \n Does NOT account for start up delay, deceleration, or queue length that exceeds the detection zone."));
            chart.Titles.LastOrDefault().Docking = Docking.Bottom;
        }



        protected void AddDataToChart(Chart chart, MOE.Common.Business.SignalPhase signalPhase, DateTime startDate,
     DateTime endDate, int binSize, string signalId, bool showDelayPerHour, bool showDelayPerVehicle)
        {


            

            foreach (MOE.Common.Business.Plan plan in signalPhase.Plans.PlanList)
            {
                //double totalPlanAoR = 0;
                //double totalPlanDelay = 0;
                //double avgPlanDelay = 0;
                //double totalPlanDetHits = 0;


                if (plan.CycleCollection.Count > 0)
                {
                    
                    DateTime dt = plan.StartTime;
                   
                    //int Yvalueholder = 0;
                    //int Y2valueholder = 0;


                    while (dt < plan.EndTime)
                    {

                        var pcds = from item in plan.CycleCollection
                                   where item.StartTime > dt && item.EndTime  < dt.AddMinutes(binSize)
                                    select item;

                       
                            

                      
                        if (showDelayPerVehicle)
                            {
                                if (pcds.Count() > 0)
                                {
                                    chart.Series["Approach Delay Per Vehicle"].Points.AddXY(dt, pcds.Sum(d => d.TotalDelay) / pcds.Sum(d=> d.TotalVolume));
                                }
                                else
                                {
                                    chart.Series["Approach Delay Per Vehicle"].Points.AddXY(dt, 0);
                                }


                            }

                            if (showDelayPerHour)
                            {

                                chart.Series["Approach Delay"].Points.AddXY(dt, (pcds.Sum(d => d.TotalDelay) * (60 / binSize)));







                            }
                        
                        dt = dt.AddMinutes(binSize);
                    }
                }


                

            }
            Dictionary<string, string> statistics = new Dictionary<string, string>();
            statistics.Add("Average Delay Per Vehicle (AD)", Math.Round(signalPhase.AvgDelay) + " seconds");
            statistics.Add("Total Delay For Selected Period (TD)", Math.Round(signalPhase.TotalDelay) + " seconds");
            SetChartTitles(signalPhase, statistics);

        }


        protected void SetPlanStrips(List<Plan> planCollection, Chart chart, DateTime graphStartDate, bool showPlanStatistics)
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

                Plannumberlabel.ForeColor = Color.Black;
                Plannumberlabel.RowIndex = 3;

                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(Plannumberlabel);

   
                double avgDelay = Math.Round(plan.AvgDelay, 0);
                double totalDelay = Math.Round(plan.TotalDelay);
                    /// stripline.StripWidth), 0);

                if (showPlanStatistics)
                {
                    CustomLabel aogLabel = new CustomLabel();
                    aogLabel.FromPosition = plan.StartTime.ToOADate();
                    aogLabel.ToPosition = plan.EndTime.ToOADate();
                    aogLabel.Text = totalDelay.ToString() + " TD";
                    aogLabel.LabelMark = LabelMarkStyle.LineSideMark;
                    aogLabel.ForeColor = Color.Red;
                    aogLabel.RowIndex = 1;
                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(aogLabel);

                    CustomLabel statisticlabel = new CustomLabel();
                    statisticlabel.FromPosition = plan.StartTime.ToOADate();
                    statisticlabel.ToPosition = plan.EndTime.ToOADate();
                    //statisticlabel.LabelMark = LabelMarkStyle.LineSideMark;
                    statisticlabel.Text = avgDelay.ToString() + " AD\n";
                    statisticlabel.ForeColor = Color.Blue;
                    statisticlabel.RowIndex = 2;
                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(statisticlabel);
                }
                //Change the background color counter for alternating color
                backGroundColor++;

            }
        }

        ///<summary>
        ///Trucates doubles to significant place
        ///<param>
        ///name="d"
        ///name="digit"
        ///</param>
        /// </summary>

        public static double SetSigFigs(double d, int digits)
        {

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);

            return scale * Math.Round(d / scale, digits);
        }

        private static int RoundToNearest(int iNumberToRound, int iToNearest)
        {
            //int iToNearest = 100;
            int iNearest = 0;
            bool bIsUpper = false;

            int iRest = iNumberToRound % iToNearest;
            if (iNumberToRound == 550) bIsUpper = true;

            if (bIsUpper == true)
            {
                iNearest = (iNumberToRound - iRest) + iToNearest;
                return iNearest;
            }
            else if (iRest > (iToNearest / 2))
            {
                iNearest = (iNumberToRound - iRest) + iToNearest;
                return iNearest;
            }
            else if (iRest < (iToNearest / 2))
            {
                iNearest = (iNumberToRound - iRest);
                return iNearest;
            }

            return 0;
        }



    }
}
