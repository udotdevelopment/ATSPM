using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.WCFServiceLibrary;

namespace MOE.Common.Business.Preempt
{
    public class PreemptDetailChart
    {
        public Chart Chart = new Chart();

        public PreemptDetailChart(PreemptDetailOptions options,
            ControllerEventLogs dttb)
        {
            Options = options;
            var preemptNumber = 0;
            if (dttb.Events.Count > 0)
            {
                var r = (from e in dttb.Events where e.EventCode != 99 select e).FirstOrDefault();
                preemptNumber = r.EventParam;
            }
            var reportTimespan = Options.EndDate - Options.StartDate;
            AddTitleAndLegend(Chart, preemptNumber);
            AddChartArea(Chart);
            AddSeries(Chart);
            AddDataToChart(Chart, dttb);
            var plans = PlanFactory.GetBasicPlans(Options.StartDate, Options.EndDate, Options.SignalID);
            //SetSimplePlanStrips(plans, Chart, Options.StartDate, Options.EndDate, dttb);
        }

        public PreemptDetailOptions Options { get; set; }

        //public static void SetSimplePlanStrips(List<Plan> plans, Chart chart, DateTime startDate, ControllerEventLogs eventLog)
        //{
        //    int backGroundColor = 1;
        //    foreach (Plan plan in plans)
        //    {
        //        StripLine stripline = new StripLine();
        //        //Creates alternating backcolor to distinguish the plans
        //        if (backGroundColor % 2 == 0)
        //        {
        //            stripline.BackColor = Color.FromArgb(120, Color.LightGray);
        //        }
        //        else
        //        {
        //            stripline.BackColor = Color.FromArgb(120, Color.LightBlue);
        //        }

        //        //Set the stripline properties
        //        stripline.IntervalOffsetType = DateTimeIntervalType.Hours;
        //        stripline.Interval = 1;
        //        stripline.IntervalOffset = (plan.StartTime - startDate).TotalHours;
        //        stripline.StripWidth = (plan.EndTime - plan.StartTime).TotalHours;
        //        stripline.StripWidthType = DateTimeIntervalType.Hours;

        //        chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

        //        //Add a corrisponding custom label for each strip
        //        CustomLabel plannumberlabel = new CustomLabel();
        //        plannumberlabel.FromPosition = plan.StartTime.ToOADate();
        //        plannumberlabel.ToPosition = plan.EndTime.ToOADate();
        //        switch (plan.PlanNumber)
        //        {
        //            case 254:
        //                plannumberlabel.Text = "Free";
        //                break;
        //            case 255:
        //                plannumberlabel.Text = "Flash";
        //                break;
        //            case 0:
        //                plannumberlabel.Text = "Unknown";
        //                break;
        //            default:
        //                plannumberlabel.Text = "Plan " + plan.PlanNumber.ToString();

        //                break;
        //        }
        //        plannumberlabel.LabelMark = LabelMarkStyle.LineSideMark;
        //        plannumberlabel.ForeColor = Color.Black;
        //        plannumberlabel.RowIndex = 6;


        //        chart.ChartAreas[0].AxisX2.CustomLabels.Add(plannumberlabel);

        //        CustomLabel planPreemptsLabel = new CustomLabel();
        //        planPreemptsLabel.FromPosition = plan.StartTime.ToOADate();
        //        planPreemptsLabel.ToPosition = plan.EndTime.ToOADate();

        //        var c = from Models.Controller_Event_Log r in eventLog.Events
        //                where r.EventCode == 107 && r.Timestamp > plan.StartTime && r.Timestamp < plan.EndTime
        //                select r;


        //        string premptCount = c.Count().ToString();
        //        planPreemptsLabel.Text = "Preempts Serviced During Plan: " + premptCount;
        //        planPreemptsLabel.LabelMark = LabelMarkStyle.LineSideMark;
        //        planPreemptsLabel.ForeColor = Color.Red;
        //        planPreemptsLabel.RowIndex = 7;

        //        chart.ChartAreas[0].AxisX2.CustomLabels.Add(planPreemptsLabel);

        //        backGroundColor++;

        //    }
        //}

        private void AddTitleAndLegend(Chart chart, int preemptNumber)
        {
            
            ChartFactory.SetImageProperties(chart);
            chart.BorderSkin.SkinStyle = BorderSkinStyle.None;
            chart.BorderSkin.BorderColor = Color.Black;
            chart.BorderSkin.BorderWidth = 1;

            //Set the chart title
            chart.Titles.Add(ChartTitleFactory.GetChartName(Options.MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(Options.SignalID,
                Options.StartDate, Options.EndDate));
            if (preemptNumber > 0)
                chart.Titles.Add(ChartTitleFactory.GetBoldTitle(" Preempt Number: " + preemptNumber));

            //Create the chart legend
            var chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            chart.Legends.Add(chartLegend);
        }

        private void AddChartArea(Chart chart)
        {
            //Create the chart area
            var chartArea = new ChartArea();
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
            var delay = new Series();
            delay.ChartType = SeriesChartType.StackedColumn;
            delay.Color = Color.FromArgb(255, 241, 113, 96);
            delay.Name = "Delay";
            delay.XValueType = ChartValueType.Int32;


            var timeToServiceSeries = new Series();
            timeToServiceSeries.ChartType = SeriesChartType.StackedColumn;
            timeToServiceSeries.Color = Color.FromArgb(255, 255, 193, 94);
            timeToServiceSeries.Name = "Time to Service";
            timeToServiceSeries.XValueType = ChartValueType.Int32;


            var trackClearSeries = new Series();
            trackClearSeries.ChartType = SeriesChartType.StackedColumn;
            trackClearSeries.BorderDashStyle = ChartDashStyle.Dash;
            trackClearSeries.Color = Color.FromArgb(255, 151, 206, 245);
            trackClearSeries.Name = "Track Clear";
            trackClearSeries.XValueType = ChartValueType.Int32;

            var dwellTimeSeries = new Series();
            dwellTimeSeries.ChartType = SeriesChartType.StackedColumn;
            dwellTimeSeries.Color = Color.FromArgb(255, 164, 238, 140);
            dwellTimeSeries.Name = "Dwell Time";
            dwellTimeSeries.XValueType = ChartValueType.Int32;

            //Series EndCallSeries = new Series();
            //EndCallSeries.ChartType = SeriesChartType.StackedColumn;
            //EndCallSeries.BorderDashStyle = ChartDashStyle.Dash;
            //EndCallSeries.Color = Color.Black;
            //EndCallSeries.Name = "Start Call";
            //EndCallSeries.XValueType = ChartValueType.Int32;


            var callMaxOutSeries = new Series();
            callMaxOutSeries.ChartType = SeriesChartType.Point;
            callMaxOutSeries.BorderDashStyle = ChartDashStyle.Dash;
            callMaxOutSeries.MarkerStyle = MarkerStyle.Cross;
            callMaxOutSeries.Color = Color.Red;
            callMaxOutSeries.Name = "Call Max Out";
            callMaxOutSeries.XValueType = ChartValueType.Int32;

            var inputOnSeries = new Series();
            inputOnSeries.ChartType = SeriesChartType.Point;
            inputOnSeries.BorderDashStyle = ChartDashStyle.Dash;
            inputOnSeries.MarkerStyle = MarkerStyle.Circle;
            inputOnSeries.Color = Color.Green;
            inputOnSeries.Name = "Input On";
            inputOnSeries.XValueType = ChartValueType.Int32;

            var inputOffSeries = new Series();
            inputOffSeries.ChartType = SeriesChartType.Point;
            inputOffSeries.BorderDashStyle = ChartDashStyle.Dash;
            inputOffSeries.MarkerStyle = MarkerStyle.Circle;
            inputOffSeries.Color = Color.Black;
            inputOffSeries.Name = "Input Off";
            inputOffSeries.XValueType = ChartValueType.Int32;


            var gateDownSeries = new Series();
            gateDownSeries.ChartType = SeriesChartType.Point;
            gateDownSeries.BorderDashStyle = ChartDashStyle.Dash;
            gateDownSeries.MarkerStyle = MarkerStyle.Triangle;
            gateDownSeries.Color = Color.Purple;
            gateDownSeries.Name = "Gate Down";
            gateDownSeries.XValueType = ChartValueType.Int32;


            chart.Series.Add(delay);
            chart.Series.Add(timeToServiceSeries);
            chart.Series.Add(trackClearSeries);
            chart.Series.Add(dwellTimeSeries);


            //chart.Series.Add(EndCallSeries);
            chart.Series.Add(callMaxOutSeries);
            chart.Series.Add(inputOnSeries);
            chart.Series.Add(inputOffSeries);
            chart.Series.Add(gateDownSeries);
        }

        protected void AddDataToChart(Chart chart, ControllerEventLogs dttb)
        {
            var engine = new PreemptCycleEngine();
            var cycles = engine.CreatePreemptCycle(dttb);
            var x = 1;
            foreach (var cycle in cycles)
            {
                if (cycle.HasDelay)
                {
                    var point = new DataPoint();
                    point.SetValueXY(x, cycle.Delay);
                    chart.Series["Delay"].Points.Add(point);
                    chart.Series["Time to Service"].Points.AddXY(x, cycle.TimeToService);
                    point.AxisLabel = cycle.CycleStart.ToShortTimeString();
                }
                else
                {
                    var point = new DataPoint();
                    point.SetValueXY(x, cycle.TimeToService);
                    chart.Series["Delay"].Points.AddXY(x, 0);
                    chart.Series["Time to Service"].Points.Add(point);
                    point.AxisLabel = cycle.CycleStart.ToShortTimeString();
                }
                chart.Series["Track Clear"].Points.AddXY(x, cycle.TimeToTrackClear);
                chart.Series["Dwell Time"].Points.AddXY(x, cycle.DwellTime);
                if (cycle.TimeToCallMaxOut > 0)
                    chart.Series["Call Max Out"].Points.AddXY(x, cycle.TimeToCallMaxOut);
                if (cycle.TimeToGateDown > 0)
                    chart.Series["Gate Down"].Points.AddXY(x, cycle.TimeToGateDown);
                foreach (var d in cycle.InputOn)
                    if (d >= cycle.CycleStart && d <= cycle.CycleEnd)
                        chart.Series["Input On"].Points.AddXY(x, (d - cycle.CycleStart).TotalSeconds);
                foreach (var d in cycle.InputOff)
                    if (d >= cycle.CycleStart && d <= cycle.CycleEnd)
                        chart.Series["Input Off"].Points.AddXY(x, (d - cycle.CycleStart).TotalSeconds);
                x++;
            }
            if (x <= 6)
                chart.ChartAreas[0].AxisX.Maximum = 8;
        }

        //protected void SetSimplePlanStrips(List<Plan> plans, Chart chart, DateTime graphStartDate, DateTime graphEndDate, ControllerEventLogs DTTB)
        //{
        //    int backGroundColor = 1;
        //    foreach (Plan plan in plans)
        //    {
        //        if (plan.StartTime >= graphStartDate && plan.EndTime <= graphEndDate)
        //        {
        //            StripLine stripline = new StripLine();
        //            //Creates alternating backcolor to distinguish the plans
        //            if (backGroundColor % 2 == 0)
        //            {
        //                stripline.BackColor = Color.FromArgb(120, Color.LightGray);
        //            }
        //            else
        //            {
        //                stripline.BackColor = Color.FromArgb(120, Color.LightBlue);
        //            }

        //            //Set the stripline properties
        //            stripline.IntervalOffsetType = DateTimeIntervalType.Hours;
        //            stripline.Interval = 1;
        //            stripline.IntervalOffset = (plan.StartTime - graphStartDate).TotalHours;
        //            stripline.StripWidth = (plan.EndTime - plan.StartTime).TotalHours;
        //            stripline.StripWidthType = DateTimeIntervalType.Hours;

        //            chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

        //            //Add a corrisponding custom label for each strip
        //            CustomLabel Plannumberlabel = new CustomLabel();
        //            Plannumberlabel.FromPosition = plan.StartTime.ToOADate();
        //            Plannumberlabel.ToPosition = plan.EndTime.ToOADate();
        //            switch (plan.PlanNumber)
        //            {
        //                case 254:
        //                    Plannumberlabel.Text = "Free";
        //                    break;
        //                case 255:
        //                    Plannumberlabel.Text = "Flash";
        //                    break;
        //                case 0:
        //                    Plannumberlabel.Text = "Unknown";
        //                    break;
        //                default:
        //                    Plannumberlabel.Text = "Plan " + plan.PlanNumber.ToString();

        //                    break;
        //            }
        //            Plannumberlabel.LabelMark = LabelMarkStyle.LineSideMark;
        //            Plannumberlabel.ForeColor = Color.Black;
        //            Plannumberlabel.RowIndex = 6;


        //            chart.ChartAreas[0].AxisX2.CustomLabels.Add(Plannumberlabel);

        //            CustomLabel planPreemptsLabel = new CustomLabel();
        //            planPreemptsLabel.FromPosition = plan.StartTime.ToOADate();
        //            planPreemptsLabel.ToPosition = plan.EndTime.ToOADate();

        //            var c = from r in DTTB.Events
        //                    where r.EventCode == 107 && r.Timestamp > plan.StartTime && r.Timestamp < plan.EndTime
        //                    select r;


        //            string preemptCount = c.Count().ToString();
        //            planPreemptsLabel.Text = "Preempts Serviced During Plan: " + preemptCount;
        //            planPreemptsLabel.LabelMark = LabelMarkStyle.LineSideMark;
        //            planPreemptsLabel.ForeColor = Color.Red;
        //            planPreemptsLabel.RowIndex = 7;

        //            chart.ChartAreas[0].AxisX2.CustomLabels.Add(planPreemptsLabel);

        //            backGroundColor++;

        //        }
        //    }
        //}
    }
}