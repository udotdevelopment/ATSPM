using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace MOE.Common.Business.PEDDelay
{
    public class PEDDelayChart
    {
        public Chart chart = new Chart();
        private PedPhase PedPhase;

        public PEDDelayChart(MOE.Common.Business.WCFServiceLibrary.PedDelayOptions options,
            PedPhase pp )
        {
            PedPhase = pp;
            string extendedDirection = string.Empty;
            TimeSpan reportTimespan = options.EndDate - options.StartDate;

            //Set the chart properties
            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
            chart.ImageType = ChartImageType.Jpeg;
            chart.Height = 550;
            chart.Width = 1100;
            

            SetChartTitle(chart, pp, options);

            //Create the chart legend
            Legend chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            chart.Legends.Add(chartLegend);


            //Create the chart area
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            chartArea.AxisY.Title = "Pedestrian Delay\nby Actuation(minutes)";
            chartArea.AxisY.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisY.Interval = 1;
            chartArea.AxisY.Minimum = DateTime.Today.ToOADate();
            chartArea.AxisY.LabelStyle.Format = "mm:ss";
            if (options.YAxisMax != null)
            {
                chartArea.AxisY.Maximum = DateTime.Today.AddMinutes(options.YAxisMax.Value).ToOADate();
            }

            chartArea.AxisX.Title = "Time (Hour of Day)";
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.LabelStyle.Format = "HH";
            chartArea.AxisX2.LabelStyle.Format = "HH";
            chartArea.AxisX.Minimum = PedPhase.StartDate.ToOADate();
            chartArea.AxisX.Maximum = PedPhase.EndDate.ToOADate();
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
            chartArea.AxisX2.Minimum = PedPhase.StartDate.ToOADate();
            chartArea.AxisX2.Maximum = PedPhase.EndDate.ToOADate();

            chart.ChartAreas.Add(chartArea);


            //Add the point series
            Series PedestrianDelaySeries = new Series();
            PedestrianDelaySeries.ChartType = SeriesChartType.Column;            
            PedestrianDelaySeries.BorderDashStyle = ChartDashStyle.Dash;
            PedestrianDelaySeries.Color = Color.Blue;
            PedestrianDelaySeries.Name = "Pedestrian Delay\nby Actuation";
            PedestrianDelaySeries.XValueType = ChartValueType.DateTime;
            

            chart.Series.Add(PedestrianDelaySeries);
            chart.Series["Pedestrian Delay\nby Actuation"]["PixelPointWidth"] = "2";
            

            AddDataToChart();
            SetPlanStrips();
        }

        private void SetChartTitle(Chart chart, PedPhase pp, WCFServiceLibrary.PedDelayOptions options)
        {
            chart.Titles.Add(ChartTitleFactory.GetChartName(options.MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(
                options.SignalID, options.StartDate, options.EndDate));
            chart.Titles.Add(ChartTitleFactory.GetPhase(pp.PhaseNumber));
            Dictionary<string, string> statistics = new Dictionary<string, string>();
            statistics.Add("Ped Actuations(PA)", pp.PedActuations.ToString());
            statistics.Add("Min Delay", DateTime.Today.AddMinutes(pp.MinDelay / 60).ToString("mm:ss"));
            statistics.Add("Max Delay", DateTime.Today.AddMinutes(pp.MaxDelay / 60).ToString("mm:ss"));
            statistics.Add("Average Delay(AD)", DateTime.Today.AddMinutes(pp.AverageDelay / 60).ToString("mm:ss"));
            chart.Titles.Add(ChartTitleFactory.GetStatistics(statistics));
        }



        protected void AddDataToChart()
        {   
            foreach(PedPlan pp in PedPhase.Plans)     
            {
                foreach(PedCycle pc in pp.Cycles)
                {
                    chart.Series["Pedestrian Delay\nby Actuation"].Points.AddXY(pc.BeginWalk, DateTime.Today.AddMinutes(pc.Delay / 60));
                }
            }       
        }


        protected void SetPlanStrips()
        {
            int backGroundColor = 1;
            foreach (PedPlan plan in PedPhase.Plans)
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
                stripline.IntervalOffset = (plan.StartDate - PedPhase.StartDate).TotalHours;
                stripline.StripWidth = (plan.EndDate - plan.StartDate).TotalHours;
                stripline.StripWidthType = DateTimeIntervalType.Hours;

                chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

                //Add a corrisponding custom label for each strip
                CustomLabel Plannumberlabel = new CustomLabel();
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
                        Plannumberlabel.Text = "Plan " + plan.PlanNumber.ToString();

                        break;
                }

                Plannumberlabel.ForeColor = Color.Black;
                Plannumberlabel.RowIndex = 3;

                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(Plannumberlabel);


                CustomLabel pedActuationsLabel = new CustomLabel();
                pedActuationsLabel.FromPosition = plan.StartDate.ToOADate();
                pedActuationsLabel.ToPosition = plan.EndDate.ToOADate();
                pedActuationsLabel.Text = plan.PedActuations + " PA";
                pedActuationsLabel.LabelMark = LabelMarkStyle.LineSideMark;
                pedActuationsLabel.RowIndex = 2;
                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(pedActuationsLabel);
                

                CustomLabel avgDelayLabel = new CustomLabel();
                avgDelayLabel.FromPosition = plan.StartDate.ToOADate();
                avgDelayLabel.ToPosition = plan.EndDate.ToOADate();
                avgDelayLabel.Text = Math.Round(plan.AvgDelay / 60).ToString() + " AD";
                avgDelayLabel.RowIndex = 1;
                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(avgDelayLabel);
               
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



