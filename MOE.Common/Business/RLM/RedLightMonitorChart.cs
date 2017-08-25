using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace MOE.Common.Business
{
    public class RedLightMonitorChart
    {
        public Chart chart = new Chart();
        private double totalCars = 0;

        public RedLightMonitorChart(DateTime graphStartDate, DateTime graphEndDate, 
            MOE.Common.Business.RLMSignalPhase signalPhase, int binSize, 
            string location, string region, string yAxisMax, string yAxis2Max, 
            bool showVolume, bool showPlanStatistics)
        {
            double y = 0;
            string extendedDirection = string.Empty;
            string movementType = "Phase";
            if(signalPhase.Approach.IsProtectedPhaseOverlap)
            {
                movementType = "Overlap";
            }


            //Gets direction for the title
            switch (signalPhase.Approach.DirectionType.Description)
            {
                case "SB":
                    extendedDirection = "Southbound";
                    break;
                case "NB":
                    extendedDirection = "Northbound";
                    break;
                default:
                    extendedDirection = signalPhase.Approach.DirectionType.Description;
                    break;
            }

            //Set the chart properties
            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
            chart.ImageType = ChartImageType.Jpeg;
            chart.Height = 450;
            chart.Width = 1100;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Raised;
            chart.BorderSkin.BorderColor = Color.Black;
            chart.BorderSkin.BorderWidth = 1;
            
           
            
            

            //Set the chart title
            chart.Titles.Add(location + "Signal " + signalPhase.Approach.SignalID.ToString() + " "
                + movementType + ": " + signalPhase.Approach.ProtectedPhaseNumber.ToString() +
                " " + extendedDirection + "\n" + graphStartDate.ToString("f") +
                " - " + graphEndDate.ToString("f"));

            //Create the chart legend
            Legend chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            chart.Legends.Add(chartLegend);


            //Create the chart area
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            if (double.TryParse(yAxisMax, out y))
            {
                chartArea.AxisY.Maximum = y;
            }
            else
            {
                chartArea.AxisY.Maximum = 60;
            }
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Title = "Volume (Vehicles Per Hour)";
            chartArea.AxisY.Interval = 500;
            chartArea.AxisY2.Title = "Percent AoR";
            chartArea.AxisY2.Maximum = 100;
            chartArea.AxisY2.Interval = 10;
            chartArea.AxisY2.Enabled = AxisEnabled.True;
            
 

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
            chart.Series["Posts"].Points.AddXY(graphStartDate, 0);
            chart.Series["Posts"].Points.AddXY(graphEndDate, 0);

            AddDataToChart(chart, signalPhase, graphStartDate, graphEndDate, binSize, signalPhase.Approach.SignalID, 
                region, showVolume, false);
            SetPlanStrips(signalPhase.Plans.PlanList, chart, graphStartDate, showPlanStatistics);
        }



        protected void AddDataToChart(Chart chart, MOE.Common.Business.RLMSignalPhase signalPhase, 
            DateTime startDate, DateTime endDate, int binSize, string signalId, string region, 
            bool showVolume, bool showAOG)
        {

            double totalDetectorHits = 0;
            int yAxisHolder = 0;

            

            foreach (MOE.Common.Business.RLMPlan plan in signalPhase.Plans.PlanList)
            {
                if (plan.RLMCycleCollection.Count > 0)
                {
                    
                    DateTime dt = plan.StartTime;

                    while (dt < plan.EndTime)
                    {

                        double binTotalStops = 0;
                        //double hitCount = 0;
                        double binPercentAoR = 0;
                        double binDetectorHits = 0;

                        var rlmCycles = from item in plan.RLMCycleCollection
                                   where item.StartTime > dt && item.EndTime  < dt.AddMinutes(binSize)
                                    select item;

                        
                        chart.Series["Percent Arrivals on Red"].Points.AddXY(dt, binPercentAoR);
                        chart.Series["Total Vehicles"].Points.AddXY(dt, (binDetectorHits * (60/binSize)));
                        chart.Series["Arrivals on Red"].Points.AddXY(dt, (binTotalStops * (60/binSize)));
                        dt = dt.AddMinutes(binSize);

                        if (yAxisHolder < (binDetectorHits * (60 / binSize)))
                        {
                            yAxisHolder = Convert.ToInt16(binDetectorHits * (60 / binSize));
                            yAxisHolder = RoundToNearest(yAxisHolder, 100);
                            chart.ChartAreas[0].AxisY.Maximum = yAxisHolder + 250;
                        }
                            
                        if (yAxisHolder < (binTotalStops * (60 / binSize)))
                        {
                            yAxisHolder = Convert.ToInt16(binDetectorHits * (60 / binSize));
                            yAxisHolder = RoundToNearest(yAxisHolder, 100);
                            chart.ChartAreas[0].AxisY.Maximum = yAxisHolder + 250;
                        }
                    }
                }
            }
            totalCars = totalDetectorHits;


            chart.Titles.Add("Test");

           
        }


        protected void SetPlanStrips(List<RLMPlan> planCollection, Chart chart, DateTime graphStartDate, bool showPlanStatistics)
        {
            int backGroundColor = 1;
            foreach (MOE.Common.Business.RLMPlan plan in planCollection)
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
                    //CustomLabel aogLabel = new CustomLabel();
                    //aogLabel.FromPosition = plan.StartTime.ToOADate();
                    //aogLabel.ToPosition = plan.EndTime.ToOADate();
                    //aogLabel.Text = (100 - plan.PercentArrivalOnGreen).ToString() + "% AoR\n";
                    //aogLabel.LabelMark = LabelMarkStyle.LineSideMark;
                    //aogLabel.ForeColor = Color.Blue;
                    //aogLabel.RowIndex = 2;
                    //chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(aogLabel);

                    //CustomLabel statisticlabel = new CustomLabel();
                    //statisticlabel.FromPosition = plan.StartTime.ToOADate();
                    //statisticlabel.ToPosition = plan.EndTime.ToOADate();
                    //statisticlabel.Text = (100 - plan.PercentGreen).ToString() + "% RT";
                    //statisticlabel.ForeColor = Color.Red;
                    //statisticlabel.RowIndex = 1;
                    //chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(statisticlabel);
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
