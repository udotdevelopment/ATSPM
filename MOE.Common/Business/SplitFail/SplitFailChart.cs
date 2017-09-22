using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;

namespace MOE.Common.Business.SplitFail
{
    public class SplitFailChart
    {
        public Chart chart = new Chart();
        public WCFServiceLibrary.SplitFailOptions Options { get; set; }
        public MOE.Common.Business.CustomReport.Phase Phase  { get; set; }
        public SplitFailPhase SplitFailPhase { get;}
        public SplitFailChart(CustomReport.Phase phase, WCFServiceLibrary.SplitFailOptions options, SplitFailPhase splitFailPhase)
        {
            Options = options;
            SplitFailPhase = splitFailPhase;
            Phase = phase;
            TimeSpan reportTimespan = Options.EndDate - Options.StartDate;


            //Set the chart properties
            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
            chart.ImageType = ChartImageType.Jpeg;
            chart.Height = 550;
            chart.Width = 1100;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.None;
            chart.BorderSkin.BorderColor = Color.Black;
            chart.BorderSkin.BorderWidth = 1;

            

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
            else
            {
                chartArea.AxisY.Maximum = 100;
            }
            chartArea.AxisY.Title = "Occupancy Ratio (percent)";
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Interval = 10;

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
            chartArea.AxisX2.Minimum = Options.StartDate.ToOADate();
            chartArea.AxisX.Minimum = Options.StartDate.ToOADate();
            chartArea.AxisX2.Maximum = Options.EndDate.ToOADate();
            chartArea.AxisX.Maximum = Options.EndDate.ToOADate();
 
            //Axis for Plan Strips
            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisX2.MajorTickMark.Enabled = true;
            chartArea.AxisX2.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX2.LabelAutoFitStyle = LabelAutoFitStyles.None;

            chart.ChartAreas.Add(chartArea);

            AddSeries(chart);


            //Add points at the start and and of the x axis to ensure
            //the graph covers the entire period selected by the user
            //whether there is data or not
            chart.Series["Posts"].Points.AddXY(Options.StartDate, 0);
            chart.Series["Posts"].Points.AddXY(Options.EndDate, 0);

            AddDataToChart(chart);


        }

        private void AddSeries(Chart chart)
        {
            Series GORGapSeries = new Series();
            GORGapSeries.MarkerSize = 4;
            GORGapSeries.ChartType = SeriesChartType.Point;
            GORGapSeries.MarkerStyle = MarkerStyle.Triangle;
            GORGapSeries.Color = Color.LimeGreen;
            GORGapSeries.Name = "GOR - GapOut";
            GORGapSeries.XValueType = ChartValueType.DateTime;

            Series GORForceSeries = new Series();
            GORForceSeries.MarkerSize = 4;
            GORForceSeries.ChartType = SeriesChartType.Point;
            GORForceSeries.MarkerStyle = MarkerStyle.Square;
            GORForceSeries.Color = Color.ForestGreen;
            GORForceSeries.Name = "GOR - ForceOff";
            GORForceSeries.XValueType = ChartValueType.DateTime;

            Series RORGapSeries = new Series();
            RORGapSeries.MarkerSize = 4;
            RORGapSeries.ChartType = SeriesChartType.Point;
            RORGapSeries.MarkerStyle = MarkerStyle.Triangle;
            RORGapSeries.Color = Color.HotPink;
            RORGapSeries.Name = "ROR - GapOut";
            RORGapSeries.XValueType = ChartValueType.DateTime;

            Series RORForceSeries = new Series();
            RORForceSeries.MarkerSize = 4;
            RORForceSeries.ChartType = SeriesChartType.Point;
            RORForceSeries.MarkerStyle = MarkerStyle.Square;
            RORForceSeries.Color = Color.Red;
            RORForceSeries.Name = "ROR - ForceOff";
            RORForceSeries.XValueType = ChartValueType.DateTime;

            Series SplitFailSeries = new Series();
            SplitFailSeries.ChartType = SeriesChartType.Column;
            SplitFailSeries.Color = Color.Gold;
            SplitFailSeries.Name = "SplitFail";
            SplitFailSeries.XValueType = ChartValueType.DateTime;

            Series GORAvg = new Series();
            GORAvg.ChartType = SeriesChartType.StepLine;
            GORAvg.BorderDashStyle = ChartDashStyle.Solid;
            GORAvg.BorderWidth = 2;
            GORAvg.Color = Color.DarkGreen;
            GORAvg.Name = "Avg. GOR";
            GORAvg.XValueType = ChartValueType.DateTime;

            Series RORAvg = new Series();
            RORAvg.ChartType = SeriesChartType.StepLine;
            RORAvg.BorderDashStyle = ChartDashStyle.Solid;
            RORAvg.BorderWidth = 2;
            RORAvg.Color = Color.DarkRed;
            RORAvg.Name = "Avg. ROR";
            RORAvg.XValueType = ChartValueType.DateTime;

            Series BinSplitFailSeries = new Series();
            BinSplitFailSeries.ChartType = SeriesChartType.StepLine;
            BinSplitFailSeries.BorderDashStyle = ChartDashStyle.Dash;
            BinSplitFailSeries.BorderWidth = 2;
            BinSplitFailSeries.Color = Color.Blue;
            BinSplitFailSeries.Name = "Percent Fails";
            BinSplitFailSeries.XValueType = ChartValueType.DateTime;
            //BinSplitFailSeries.YAxisType = AxisType.Secondary;


            //Add the Posts series to ensure the chart is the size of the selected timespan
            Series posts = new Series();
            posts.IsVisibleInLegend = false;
            posts.ChartType = SeriesChartType.Point;
            posts.Color = Color.White;
            posts.Name = "Posts";
            posts.XValueType = ChartValueType.DateTime;

            chart.Series.Add(posts);
            chart.Series.Add(SplitFailSeries);
            //chart.Series.Add(GORMaxSeries);
            chart.Series.Add(GORGapSeries);
            chart.Series.Add(GORForceSeries);
            //chart.Series.Add(GORUnknownSeries);
            //chart.Series.Add(RORMaxSeries);
            chart.Series.Add(RORGapSeries);
            chart.Series.Add(RORForceSeries);
            //chart.Series.Add(RORUnknownSeries);
            chart.Series.Add(RORAvg);
            chart.Series.Add(GORAvg);
            chart.Series.Add(BinSplitFailSeries);




            chart.Series["SplitFail"].CustomProperties = "DrawingStyle = Cylinder,PixelPointWidth = 1";


        }

        protected void AddDataToChart(Chart chart)
        {
            foreach (var gorGapOut in SplitFailPhase.GorGapOut)
            {
                chart.Series["GOR - GapOut"].Points.AddXY(gorGapOut.Item1, gorGapOut.Item2);
            }
            foreach (var rorGapOut in SplitFailPhase.RorGapOut)
            {
                chart.Series["ROR - GapOut"].Points.AddXY(rorGapOut.Item1, rorGapOut.Item2);
            }
            foreach (var gorForceOff in SplitFailPhase.GorForceOff)
            {
                chart.Series["GOR - ForceOff"].Points.AddXY(gorForceOff.Item1, gorForceOff.Item2);
            }
            foreach (var rorForceOff in SplitFailPhase.RorForceOff)
            {
                chart.Series["ROR - ForceOff"].Points.AddXY(rorForceOff.Item1, rorForceOff.Item2);
            }
            if (Options.ShowFailLines)
            {
                foreach (var splitFail in SplitFailPhase.SplitFails)
                {
                    chart.Series["SplitFail"].Points.AddXY(splitFail, 100);
                }
            }
            DateTime counterTime = Options.StartDate;
            if (Options.ShowPercentFailLines)
            {
                foreach (var percentFail in SplitFailPhase.PercentFails)
                {
                    chart.Series["Percent Fails"].Points.AddXY(counterTime, Convert.ToInt32(percentFail));
                }
            }
            if (Options.ShowAvgLines)
            {
                foreach (var averageGor in SplitFailPhase.AverageGors)
                {
                    chart.Series["Avg. GOR"].Points.AddXY(averageGor.Item1, averageGor.Item2);
                }
                foreach (var averageRor in SplitFailPhase.AverageRors)
                {
                    chart.Series["Avg. ROR"].Points.AddXY(averageRor.Item1, averageRor.Item2);
                }
            }
            SetChartTitle(SplitFailPhase.Statistics);
            AddPlanStrips(chart, Phase, Options.StartDate, Options.EndDate);
        }

        private void SetChartTitle(Dictionary<string, string> statistics)
        {
            chart.Titles.Add(ChartTitleFactory.GetChartName(Options.MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(Options.SignalID, Options.StartDate, Options.EndDate));
            chart.Titles.Add(ChartTitleFactory.GetPhaseAndPhaseDescriptions(Phase.PhaseNumber, Phase.Approach.DirectionType.Description));
            chart.Titles.Add(ChartTitleFactory.GetStatistics(statistics));
        }

        protected void AddPlanStrips(Chart chart, MOE.Common.Business.CustomReport.Phase phase, DateTime startDate, DateTime endDate)
        {
            PlanCollection planCollection = new PlanCollection(startDate, endDate, phase.SignalID);
            
            int backGroundColor = 1;

            //Parallel.ForEach(planCollection.PlanList, plan =>
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
                 stripline.IntervalOffset = (plan.StartTime - startDate).TotalHours;
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
                 Plannumberlabel.RowIndex = 4;
                 Plannumberlabel.LabelMark = LabelMarkStyle.LineSideMark;
                 chart.ChartAreas[0].AxisX2.CustomLabels.Add(Plannumberlabel);

                 CustomLabel PlanMetrics = new CustomLabel();
                 PlanMetrics.FromPosition = plan.StartTime.ToOADate();
                 PlanMetrics.ToPosition = plan.EndTime.ToOADate();

                 var cycleInPlan = from c in phase.Cycles
                                   where c.CycleStart > plan.StartTime && c.CycleEnd < plan.EndTime
                                   select c;

                 var failsInPlan = from s in SplitFailPhase.PercentFails
                                   where s.Item1 > plan.StartTime && s.Item1 < plan.EndTime
                                   select s;

                 PlanMetrics.Text += failsInPlan.Count().ToString() + " SF";

                 if (cycleInPlan.Count() > 0)
                 {
                     double p = Convert.ToDouble(failsInPlan.Count()) / Convert.ToDouble(cycleInPlan.Count());
                     PlanMetrics.Text += "\n" + Convert.ToInt32(p * 100).ToString() + "% SF";
                 }

                 PlanMetrics.ForeColor = Color.Black;
                 PlanMetrics.RowIndex = 3;
                 chart.ChartAreas[0].AxisX2.CustomLabels.Add(PlanMetrics);



                 //Change the background color counter for alternating color
                 backGroundColor++;

             }
             //);
        
        }
    }
}

