using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace MOE.Common.Business
{
    public class ArriveOnRedChart
    {
        public Chart chart = new Chart();
        private double totalAoR = 0;
        private double totalPercentAoR = 0;
        private double totalCars = 0;
        private int MetricTypeID = 9;
        public WCFServiceLibrary.AoROptions Options { get; set; }
        public ArriveOnRedChart(WCFServiceLibrary.AoROptions options, 
            MOE.Common.Business.SignalPhase signalPhase)
        {
            Options = options;
            TimeSpan reportTimespan = Options.EndDate - Options.StartDate;

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
            if (Options.YAxisMax != null)
            {
                chartArea.AxisY.Maximum = Options.YAxisMax.Value;
            }

            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Title = "Volume (Vehicles Per Hour)";
            chartArea.AxisY.Interval = 500;
            chartArea.AxisY2.Title = "Percent AoR";
            chartArea.AxisY2.Maximum = 100;
            chartArea.AxisY2.Interval = 10;
            chartArea.AxisY2.Enabled = AxisEnabled.True;
            
 

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

            Series AoRSeries = new Series();
            AoRSeries.ChartType = SeriesChartType.Line;            
            AoRSeries.BorderDashStyle = ChartDashStyle.Dash;
            AoRSeries.Color = Color.Red;
            AoRSeries.Name = "Arrivals on Red";
            AoRSeries.XValueType = ChartValueType.DateTime;

            Series TVSeries = new Series();
            TVSeries.ChartType = SeriesChartType.Line;
            TVSeries.BorderDashStyle = ChartDashStyle.Dash;
            TVSeries.Color = Color.Black;
            TVSeries.Name = "Total Vehicles";
            TVSeries.XValueType = ChartValueType.DateTime;

            Series PARSeries = new Series();
            PARSeries.ChartType = SeriesChartType.Line;
            PARSeries.Color = Color.Red;
            PARSeries.Name = "Percent Arrivals on Red";
            PARSeries.BorderWidth = 2;
            PARSeries.XValueType = ChartValueType.DateTime;
            PARSeries.YAxisType = AxisType.Secondary;


            Series pointSeries = new Series();
            pointSeries.ChartType = SeriesChartType.Point;
            pointSeries.Color = Color.White;
            pointSeries.Name = "Posts";
            pointSeries.XValueType = ChartValueType.DateTime;
            pointSeries.IsVisibleInLegend = false;
            
            
            chart.Series.Add(pointSeries);
            chart.Series.Add(AoRSeries);
            chart.Series.Add(PARSeries);
            chart.Series.Add(TVSeries);




            //Add points at the start and and of the x axis to ensure
            //the graph covers the entire period selected by the user
            //whether there is data or not
            chart.Series["Posts"].Points.AddXY(Options.StartDate, 0);
            chart.Series["Posts"].Points.AddXY(Options.EndDate, 0);

            AddDataToChart(chart, signalPhase);
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
        }


        protected void AddDataToChart(Chart chart, MOE.Common.Business.SignalPhase signalPhase)
        {
           
            double totalDetectorHits = 0;
            int yAxisHolder = 0;



            foreach (MOE.Common.Business.Plan plan in signalPhase.Plans.PlanList)
            {
                if (plan.CycleCollection.Count > 0)
                {

                    DateTime dt = plan.StartTime;

                    while (dt < plan.EndTime)
                    {
                        double binTotalStops = 0;
                        double binPercentAoR = 0;
                        double binDetectorHits = 0;
                        var pcds = from item in plan.CycleCollection
                                   where item.StartTime > dt && item.EndTime < dt.AddMinutes(Options.SelectedBinSize)
                                   select item;
                        foreach (MOE.Common.Business.Cycle pcd in pcds)
                        {
                            totalDetectorHits += pcd.DetectorCollection.Count;
                            binDetectorHits += pcd.DetectorCollection.Count;
                            foreach (MOE.Common.Business.DetectorDataPoint detectorPoint in pcd.DetectorCollection)
                            {
                                if (detectorPoint.YPoint < pcd.GreenLineY) //&& detectorPoint.YPoint < pcd.RedLineY)
                                {
                                    binTotalStops++;
                                    totalAoR++;
                                }
                            }

                            if (binDetectorHits > 0)
                            {
                                binPercentAoR = ((binTotalStops / binDetectorHits) * 100);
                            }
                        }
                        chart.Series["Percent Arrivals on Red"].Points.AddXY(dt, binPercentAoR);
                        chart.Series["Total Vehicles"].Points.AddXY(dt, (binDetectorHits * (60 / Options.SelectedBinSize)));
                        chart.Series["Arrivals on Red"].Points.AddXY(dt, (binTotalStops * (60 / Options.SelectedBinSize)));
                        dt = dt.AddMinutes(Options.SelectedBinSize);
                        if (yAxisHolder < (binTotalStops * (60 / Options.SelectedBinSize)) && Options.YAxisMax == null)
                        {
                            yAxisHolder = Convert.ToInt16(binDetectorHits * (60 / Options.SelectedBinSize));
                            yAxisHolder = RoundToNearest(yAxisHolder, 100);
                            chart.ChartAreas[0].AxisY.Maximum = yAxisHolder + 250;
                        }
                    }
                }
            }
            totalCars = totalDetectorHits;

            if (totalDetectorHits > 0)
            {
                totalPercentAoR = ((totalAoR / totalCars) * 100);
            }
            Dictionary<string, string> statistics = new Dictionary<string, string>();
            statistics.Add("Total Detector Hits", totalCars.ToString());
            statistics.Add("Total AoR", totalAoR.ToString());
            statistics.Add("Percent AoR for the select period", Math.Round(totalPercentAoR).ToString());
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

                if (showPlanStatistics)
                {
                    CustomLabel aogLabel = new CustomLabel();
                    aogLabel.FromPosition = plan.StartTime.ToOADate();
                    aogLabel.ToPosition = plan.EndTime.ToOADate();
                    aogLabel.Text = (100 - plan.PercentArrivalOnGreen).ToString() + "% AoR\n";
                    aogLabel.LabelMark = LabelMarkStyle.LineSideMark;
                    aogLabel.ForeColor = Color.Blue;
                    aogLabel.RowIndex = 2;
                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(aogLabel);

                    CustomLabel statisticlabel = new CustomLabel();
                    statisticlabel.FromPosition = plan.StartTime.ToOADate();
                    statisticlabel.ToPosition = plan.EndTime.ToOADate();
                    statisticlabel.Text = (100 - plan.PercentGreen).ToString() + "% RT";
                    statisticlabel.ForeColor = Color.Red;
                    statisticlabel.RowIndex = 1;
                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(statisticlabel);
                }
                //Change the background color counter for alternating color
                backGroundColor++;

            }
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
