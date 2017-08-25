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

        public SplitFailChart(WCFServiceLibrary.SplitFailOptions options,
            MOE.Common.Business.CustomReport.Phase phase)
        {
            Options = options;
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


            List<ControllerEventLogs> Tables = new List<ControllerEventLogs>();
            List<DateTime> SplitFails = new List<DateTime>();

            int totalFails = 0;



            Models.Repositories.ISignalsRepository signalRepository =
                 Models.Repositories.SignalsRepositoryFactory.Create();
            //var signal = signalRepository.GetSignalBySignalID(phase.SignalID);
            var ApproachDetectors = Phase.Approach.GetDetectorsForMetricType(12);
            
            //get occupancy events for each lane for the approach.
            if (ApproachDetectors.Count() > 0)
            {
                foreach (Models.Detector row in ApproachDetectors)
                {

                    MOE.Common.Business.ControllerEventLogs TEMPdetectortable = new ControllerEventLogs(Phase.SignalID, Options.StartDate, Options.EndDate, 
                    row.DetChannel, new List<int>(){81,82});


                    if (TEMPdetectortable.Events.Count > 0)
                    {
                        Tables.Add(TEMPdetectortable);
                    }

                }



                Dictionary<string, string> statistics = new Dictionary<string, string>();
                if (Tables.Count > 0 )
                {
                    //for (int CurCycleIndex = 0; CurCycleIndex < phase.Cycles.Count -1 ; CurCycleIndex++)
                    //int tempCycleCounter = 0;
                    //Parallel.ForEach(phase.Cycles, c =>
                    foreach (MOE.Common.Business.CustomReport.Cycle c in Phase.Cycles)
                    {
                        //tempCycleCounter++;
                        //SplitFailDetectorActivationCollection activations = new SplitFailDetectorActivationCollection();


                        //for each lane
                        //Parallel.ForEach(Tables, table =>
                        foreach (ControllerEventLogs table in Tables)
                        {

                            int channel = table.Events[0].EventParam;

                            List<MOE.Common.Models.Controller_Event_Log> DetectorHitsForCycle = new List<MOE.Common.Models.Controller_Event_Log>();


                            //Parallel.ForEach(table.Events, e =>
                            foreach (MOE.Common.Models.Controller_Event_Log e in table.Events)
                            {

                                if (e.Timestamp >= c.CycleStart && e.Timestamp <= c.CycleEnd)
                                {
                                    DetectorHitsForCycle.Add(e);
                                }
                            }
                            //);

                            if (DetectorHitsForCycle.Count > 0)
                            {
                                var eventsInOrder = DetectorHitsForCycle.OrderBy(r => r.Timestamp);
                                if (eventsInOrder.Count() > 1)
                                {
                                    for (int i = 0; i < eventsInOrder.Count() - 1; i++)
                                    {


                                        MOE.Common.Models.Controller_Event_Log current = eventsInOrder.ElementAt(i);

                                        MOE.Common.Models.Controller_Event_Log next = eventsInOrder.ElementAt(i + 1);


                                        if (current.Timestamp.Ticks == next.Timestamp.Ticks)
                                        {
                                          
                                            continue;
                                        }

                                        //If the first event is 'Off', then set 'On' to cyclestart
                                        if (i == 0 && current.EventCode == 81)
                                        {
                                            SplitFailDetectorActivation da = new SplitFailDetectorActivation();
                                            da.DetectorOn = c.CycleStart;
                                            da.DetectorOff = current.Timestamp;

                                            c.Activations.AddActivation(da);
                                            




                                        }

                                        //This is the prefered sequence; an 'On'  followed by an 'off'
                                        if (current.EventCode == 82 && next.EventCode == 81)
                                        {
                                            SplitFailDetectorActivation da = new SplitFailDetectorActivation();
                                            da.DetectorOn = current.Timestamp;
                                            da.DetectorOff = next.Timestamp;
                                            c.Activations.AddActivation(da);
                                            continue;

                                        }

                                        //if we are at the penultimate event, and the last event is 'on', set 'off' as CycleEnd.
                                        if (i + 2 == eventsInOrder.Count() && next.EventCode == 82)
                                        {
                                            SplitFailDetectorActivation da = new SplitFailDetectorActivation();
                                            da.DetectorOn = next.Timestamp;
                                            da.DetectorOff = c.CycleEnd;
                                            c.Activations.AddActivation(da);
                                            continue;


                                        }




                                    }
                                }
                                else
                                {
                                    SplitFailDetectorActivation da = new SplitFailDetectorActivation();
                                    MOE.Common.Models.Controller_Event_Log current = eventsInOrder.First();
                                    switch (current.EventCode)
                                    {


                                        //if the only event is off
                                        case 81:
                                            da.DetectorOn = c.CycleStart;
                                            da.DetectorOff = current.Timestamp;
                                            c.Activations.AddActivation(da);

                                            break;
                                        //if the only event is on
                                        case 82:

                                            da.DetectorOn = current.Timestamp;
                                            da.DetectorOff = c.CycleEnd;
                                            c.Activations.AddActivation(da);

                                            break;
                                    }
                                }
                            }
                            //if there are no hits in the cycle, we need to determine if the a previous detector activaition lasts the entire cycle
                            else if (DetectorHitsForCycle.Count <= 0)
                            {

                                SplitFailDetectorActivation da = new SplitFailDetectorActivation();

                                DateTime earlierTime = c.CycleStart.AddMinutes(-30);


                                List<int> li = new List<int> { 81, 82 };

                                ControllerEventLogs cs = new ControllerEventLogs(Phase.SignalID, earlierTime, c.CycleStart, channel, li);

                                //if the last detecotr eventCodes was ON, and there is no matching off event, assume the detector was on for the whole cycle
                                if (cs.Events.Count > 0 && cs.Events.LastOrDefault().EventCode == 82)
                                {


                                    da.DetectorOn = c.CycleStart;
                                    da.DetectorOff = c.CycleEnd;
                                    c.Activations.AddActivation(da);
                                }
                                //}
                            }
                        }
                        //);
                        //end of Lane loop







                        //merge the detectors for the different lanes
                        for (int i = 0; i < c.Activations.Activations.Count - 1; )
                        {
                            SplitFailDetectorActivation current = c.Activations.Activations.ElementAt(i).Value;
                            SplitFailDetectorActivation next = c.Activations.Activations.ElementAt(i + 1).Value;

                            //if the next activaiton is between the previos one, remove the nextone and start again.
                            if (next.DetectorOn >= current.DetectorOn && next.DetectorOff <= current.DetectorOff)
                            {
                                c.Activations.Activations.RemoveAt(i + 1);
                                continue;
                            }
                            //if the next activaiton starts during the current, but ends later, atler current end time, and remove next, and start over. 
                            else if (next.DetectorOn >= current.DetectorOn && next.DetectorOn < current.DetectorOff && next.DetectorOff > current.DetectorOff)
                            {
                                current.DetectorOff = next.DetectorOff;
                                c.Activations.Activations.RemoveAt(i + 1);
                                continue;
                            }
                            else
                            {
                                i++;
                            }

                        }



                        //if (c.Activations.Activations.Count > 0 && c.CycleStart > startDate && c.CycleStart < endDate)
                        if (c.CycleStart > Options.StartDate && c.CycleStart < Options.EndDate)
                        {

                            double gor = c.Activations.GreenOccupancy(c) * 100;
                            double ror = c.Activations.StartOfRedOccupancy(c, Options.FirstSecondsOfRed) * 100;

                            if (c.TerminationEvent == MOE.Common.Business.CustomReport.Cycle.TerminationCause.GapOut)
                            {

                                chart.Series["GOR - GapOut"].Points.AddXY(c.CycleStart, gor);
                                chart.Series["ROR - GapOut"].Points.AddXY(c.CycleStart, ror);

                            }

                            else
                            {
                                chart.Series["GOR - ForceOff"].Points.AddXY(c.CycleStart, gor);
                                chart.Series["ROR - ForceOff"].Points.AddXY(c.CycleStart, ror);

                            }


                            if ((gor > 79 && ror > 79))
                            {
                                if (Options.ShowFailLines)
                                {
                                    chart.Series["SplitFail"].Points.AddXY(c.CycleStart, 100);
                                }
                                SplitFails.Add(c.CycleStart);
                                totalFails++;

                            }
                        }
                    }
                    //);
                    
                    
                    statistics.Add("Total Split Failures ", totalFails.ToString());


                    //end of Cycle loop

                  
                        //Average Loop

                    DateTime counterTime = Options.StartDate;

                        do
                        {
                            double binTotalGOR = 0;
                            double binTotalROR = 0;

                            var CycleBin = from cur in Phase.Cycles
                                           where cur.CycleStart >= counterTime
                                           && cur.CycleStart <= counterTime.AddMinutes(15)
                                           orderby cur.CycleStart
                                           select cur;

                            var failsInBin = from s in SplitFails
                                             where s >= counterTime && s <= counterTime.AddMinutes(15)
                                             select s;

                            double binFails = failsInBin.Count();

                            //Parallel.ForEach(CycleBin, c =>
                                
                            foreach (MOE.Common.Business.CustomReport.Cycle c in CycleBin)
                            {
                                binTotalGOR += c.Activations.GreenOccupancy(c) * 100;
                                binTotalROR += c.Activations.StartOfRedOccupancy(c, Options.FirstSecondsOfRed) * 100;



                            }
                        //);
                            if (Options.ShowPercentFailLines)
                            {
                                if (binFails > 0 && CycleBin.Count() > 0)
                                {
                                    double binFailPercent = (binFails / Convert.ToDouble(CycleBin.Count()));
                                    chart.Series["Percent Fails"].Points.AddXY(counterTime, Convert.ToInt32(binFailPercent * 100));
                                }
                                else
                                {
                                    chart.Series["Percent Fails"].Points.AddXY(counterTime, 0);
                                }
                            }
                            if (Options.ShowAvgLines)
                            {
                                if (CycleBin.Count() > 0)
                                {


                                    double avggor = binTotalGOR / CycleBin.Count();
                                    double avgror = binTotalROR / CycleBin.Count();

                                    chart.Series["Avg. GOR"].Points.AddXY(counterTime, avggor);
                                    chart.Series["Avg. ROR"].Points.AddXY(counterTime, avgror);

                                }

                                if (CycleBin.Count() == 0)
                                {
                                    chart.Series["Avg. GOR"].Points.AddXY(counterTime, 0);
                                    chart.Series["Avg. ROR"].Points.AddXY(counterTime, 0);
                                }
                            }







                            counterTime = counterTime.AddMinutes(15);




                        } while (counterTime < Options.EndDate.AddMinutes(15));

                        //End Average Loop
                    
                                

            }
                SetChartTitle(statistics);
                AddPlanStrips(chart, Phase, Options.StartDate, Options.EndDate, SplitFails);
            }
        }

        private void SetChartTitle(Dictionary<string, string> statistics)
        {
            chart.Titles.Add(ChartTitleFactory.GetChartName(Options.MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(Options.SignalID, Options.StartDate, Options.EndDate));
            chart.Titles.Add(ChartTitleFactory.GetPhaseAndPhaseDescriptions(Phase.PhaseNumber, Phase.Approach.DirectionType.Description));
            chart.Titles.Add(ChartTitleFactory.GetStatistics(statistics));
        }

        protected void AddPlanStrips(Chart chart, MOE.Common.Business.CustomReport.Phase phase, DateTime startDate, DateTime endDate, List<DateTime> splitFails )
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

                 var failsInPlan = from s in splitFails
                                   where s > plan.StartTime && s < plan.EndTime
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

