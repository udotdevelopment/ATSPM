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


namespace MOE.Common.Business.Preempt
{
    public class PreemptDetailChart
        {
        public Chart chart = new Chart();
        public WCFServiceLibrary.PreemptDetailOptions Options { get; set; }

        public PreemptDetailChart(WCFServiceLibrary.PreemptDetailOptions options, 
            MOE.Common.Business.ControllerEventLogs DTTB)
        {
            Options = options;
            int PreemptNumber = 0;
            if (DTTB.Events.Count > 0)
            {
                var r = (from e in DTTB.Events where e.EventCode != 99 select e).FirstOrDefault();
                PreemptNumber = r.EventParam;
            }
            TimeSpan reportTimespan = Options.EndDate - Options.StartDate;

            AddTitleAndLegend(chart, PreemptNumber);

            AddChartArea(chart, reportTimespan);

            AddSeries(chart);

            AddDataToChart(chart, DTTB, PreemptNumber);
            PlanCollection plans = new PlanCollection(Options.StartDate, Options.EndDate, Options.SignalID);
           // SetSimplePlanStrips(plans, chart, graphStartDate, graphEndDate, DTTB);

        }

        private void AddTitleAndLegend(Chart chart, int PreemptNumber)
        {
            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
            chart.ImageType = ChartImageType.Jpeg;
            chart.Height = 350;
            chart.Width = 1100;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.None;
            chart.BorderSkin.BorderColor = Color.Black;
            chart.BorderSkin.BorderWidth = 1;

            //Set the chart title
            chart.Titles.Add(ChartTitleFactory.GetChartName(Options.MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(Options.SignalID, 
                Options.StartDate, Options.EndDate));
            if (PreemptNumber > 0)
            {
                chart.Titles.Add(ChartTitleFactory.GetBoldTitle(" Preempt Number: " + PreemptNumber.ToString()));                
            }

            //Create the chart legend
            Legend chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            chart.Legends.Add(chartLegend);
        }

        private void AddChartArea(Chart chart, TimeSpan reportTimespan)
        {
            //Create the chart area
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";

            chartArea.AxisY.Title = "Seconds Since Request";
            chartArea.AxisY.Minimum = 0;            
            chartArea.AxisY.Interval = 10;

            chartArea.AxisX.Title = "Preempts";
            chartArea.AxisX.Minimum = 0;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.MaximumAutoSize = 100;
            chart.ChartAreas.Add(chartArea);
        }

        private void AddSeries(Chart chart)
        {
           

            Series Delay = new Series();
            Delay.ChartType = SeriesChartType.StackedColumn;
            Delay.Color = Color.FromArgb(255, 241, 113, 96);
            Delay.Name = "Delay";
            Delay.XValueType = ChartValueType.Int32;


            Series TimeToServiceSeries = new Series();
            TimeToServiceSeries.ChartType = SeriesChartType.StackedColumn;
            TimeToServiceSeries.Color = Color.FromArgb(255, 255, 193, 94);
            TimeToServiceSeries.Name = "Time to Service";
            TimeToServiceSeries.XValueType = ChartValueType.Int32;

            

            Series TrackClearSeries = new Series();
            TrackClearSeries.ChartType = SeriesChartType.StackedColumn;
            TrackClearSeries.BorderDashStyle = ChartDashStyle.Dash;
            TrackClearSeries.Color = Color.FromArgb(255, 151, 206, 245);
            TrackClearSeries.Name = "Track Clear";
            TrackClearSeries.XValueType = ChartValueType.Int32;

            Series DwellTimeSeries = new Series();
            DwellTimeSeries.ChartType = SeriesChartType.StackedColumn;
            DwellTimeSeries.Color = Color.FromArgb(255, 164, 238, 140);
            DwellTimeSeries.Name = "Dwell Time";
            DwellTimeSeries.XValueType = ChartValueType.Int32;

            //Series EndCallSeries = new Series();
            //EndCallSeries.ChartType = SeriesChartType.StackedColumn;
            //EndCallSeries.BorderDashStyle = ChartDashStyle.Dash;
            //EndCallSeries.Color = Color.Black;
            //EndCallSeries.Name = "End Call";
            //EndCallSeries.XValueType = ChartValueType.Int32;



            Series CallMaxOutSeries = new Series();
            CallMaxOutSeries.ChartType = SeriesChartType.Point;
            CallMaxOutSeries.BorderDashStyle = ChartDashStyle.Dash;
            CallMaxOutSeries.MarkerStyle = MarkerStyle.Cross;
            CallMaxOutSeries.Color = Color.Red;
            CallMaxOutSeries.Name = "Call Max Out";
            CallMaxOutSeries.XValueType = ChartValueType.Int32;

            Series InputOnSeries = new Series();
            InputOnSeries.ChartType = SeriesChartType.Point;
            InputOnSeries.BorderDashStyle = ChartDashStyle.Dash;
            InputOnSeries.MarkerStyle = MarkerStyle.Circle;
            InputOnSeries.Color = Color.Green;
            InputOnSeries.Name = "Input On";
            InputOnSeries.XValueType = ChartValueType.Int32;

            Series InputOffSeries = new Series();
            InputOffSeries.ChartType = SeriesChartType.Point;
            InputOffSeries.BorderDashStyle = ChartDashStyle.Dash;
            InputOffSeries.MarkerStyle = MarkerStyle.Circle;
            InputOffSeries.Color = Color.Black;
            InputOffSeries.Name = "Input Off";
            InputOffSeries.XValueType = ChartValueType.Int32;



            Series GateDownSeries = new Series();
            GateDownSeries.ChartType = SeriesChartType.Point;
            GateDownSeries.BorderDashStyle = ChartDashStyle.Dash;
            GateDownSeries.MarkerStyle = MarkerStyle.Triangle;
            GateDownSeries.Color = Color.Purple;
            GateDownSeries.Name = "Gate Down";
            GateDownSeries.XValueType = ChartValueType.Int32;


            chart.Series.Add(Delay);
            chart.Series.Add(TimeToServiceSeries);
            chart.Series.Add(TrackClearSeries);
            chart.Series.Add(DwellTimeSeries);
            

            //chart.Series.Add(EndCallSeries);
            chart.Series.Add(CallMaxOutSeries);
            chart.Series.Add(InputOnSeries);
            chart.Series.Add(InputOffSeries);
            chart.Series.Add(GateDownSeries);

        }

        protected void AddDataToChart(Chart chart, MOE.Common.Business.ControllerEventLogs DTTB,  int preemptNumber)
        {
            PreemptCycleEngine engine = new PreemptCycleEngine();
            List<PreemptCycle> cycles = engine.CreatePreemptCycle(DTTB);

            int x = 1;

            foreach(PreemptCycle cycle in cycles)
            {

                if (cycle.HasDelay)
                {
                    DataPoint point = new DataPoint();
                    point.SetValueXY(x, cycle.Delay);
                    chart.Series["Delay"].Points.Add(point);
                    chart.Series["Time to Service"].Points.AddXY(x,cycle.TimeToService);
                    point.AxisLabel = cycle.CycleStart.ToShortTimeString();
                }
                else
                {
                    DataPoint point = new DataPoint();
                    point.SetValueXY(x, cycle.TimeToService);
                    chart.Series["Delay"].Points.AddXY(x,0);
                    chart.Series["Time to Service"].Points.Add(point);
                    point.AxisLabel = cycle.CycleStart.ToShortTimeString();
                }
                    chart.Series["Track Clear"].Points.AddXY(x, cycle.TimeToTrackClear);
                    chart.Series["Dwell Time"].Points.AddXY(x, cycle.DwellTime);


                    if (cycle.TimeToCallMaxOut > 0)
                    {

                        chart.Series["Call Max Out"].Points.AddXY(x, cycle.TimeToCallMaxOut);
                       
                    }

                    if (cycle.TimeToGateDown > 0)
                    {

                        chart.Series["Gate Down"].Points.AddXY(x, cycle.TimeToGateDown);
                     
                    }



                foreach(DateTime d in cycle.InputOn)
                {

                    if(d >= cycle.CycleStart && d <= cycle.CycleEnd)
                    {
                        chart.Series["Input On"].Points.AddXY(x, (d - cycle.CycleStart).TotalSeconds);
                
                    }
                }

                foreach (DateTime d in cycle.InputOff)
                {
                    if (d >= cycle.CycleStart && d <= cycle.CycleEnd)
                    {
                        chart.Series["Input Off"].Points.AddXY(x, (d - cycle.CycleStart).TotalSeconds);
                  
                    }
                }

                x++;
            }

            
            if(x <= 6)
            {
                chart.ChartAreas[0].AxisX.Maximum = 8;
                

            }


        }



       

       

        protected void SetSimplePlanStrips(MOE.Common.Business.PlanCollection planCollection, Chart chart, DateTime graphStartDate, DateTime graphEndDate, MOE.Common.Business.ControllerEventLogs DTTB)
        {
            int backGroundColor = 1;
            foreach (MOE.Common.Business.Plan plan in planCollection.PlanList)
            {
                if (plan.StartTime > graphStartDate && plan.EndTime < graphEndDate)
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


                    chart.ChartAreas[0].AxisX2.CustomLabels.Add(Plannumberlabel);

                    CustomLabel planPreemptsLabel = new CustomLabel();
                    planPreemptsLabel.FromPosition = plan.StartTime.ToOADate();
                    planPreemptsLabel.ToPosition = plan.EndTime.ToOADate();

                    var c = from r in DTTB.Events
                            where r.EventCode == 107 && r.Timestamp > plan.StartTime && r.Timestamp < plan.EndTime
                            select r;



                    string preemptCount = c.Count().ToString();
                    planPreemptsLabel.Text = "Preempts Serviced During Plan: " + preemptCount;
                    planPreemptsLabel.LabelMark = LabelMarkStyle.LineSideMark;
                    planPreemptsLabel.ForeColor = Color.Red;
                    planPreemptsLabel.RowIndex = 7;

                    chart.ChartAreas[0].AxisX2.CustomLabels.Add(planPreemptsLabel);

                    backGroundColor++;

                }
            }
        }

        }
}
