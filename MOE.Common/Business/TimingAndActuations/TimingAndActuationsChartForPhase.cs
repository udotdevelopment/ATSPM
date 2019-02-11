using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;

namespace MOE.Common.Business.TimingAndActuations
{
    public class TimingAndActuationsChartForPhase
    {
        public Chart Chart { get; set; }
        private TimingAndActuationsForPhase TimingAndActuationsForPhase { get; set; }
        public TimingAndActuationsChartForPhase(TimingAndActuationsForPhase timingAndActuationsForPhase)
        {
            TimingAndActuationsForPhase = timingAndActuationsForPhase;
            Chart = ChartFactory.CreateDefaultChart(TimingAndActuationsForPhase.Options);
            SetCycleStrips();
        }

        void SetCycleStrips()
        {
            DateTime endOfLastCycle = TimingAndActuationsForPhase.Options.StartDate;
            foreach (var cycle in TimingAndActuationsForPhase.Cycles)
            {
                var minGreenStripline = new StripLine();
                minGreenStripline.BackColor = Color.FromArgb(120, Color.DarkGreen);
                minGreenStripline.IntervalOffset = (cycle.StartGreen - TimingAndActuationsForPhase.Options.StartDate).TotalHours;
                minGreenStripline.IntervalOffsetType = DateTimeIntervalType.Hours;
                minGreenStripline.Interval = 1;
                minGreenStripline.IntervalType = DateTimeIntervalType.Days;
                minGreenStripline.StripWidth = (cycle.EndMinGreen - cycle.StartGreen).TotalHours;
                minGreenStripline.StripWidthType = DateTimeIntervalType.Hours;
                Chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(minGreenStripline);

                //var GreenStripline = new StripLine();
                //minGreenStripline.BackColor = Color.FromArgb(120, Color.DarkGreen);
                //minGreenStripline.IntervalOffset = (cycle.EndMinGreen - cycle.StartGreen).TotalHours;
                //minGreenStripline.IntervalOffsetType = DateTimeIntervalType.Hours;
                //minGreenStripline.Interval = 1;
                //minGreenStripline.IntervalType = DateTimeIntervalType.Days;
                //minGreenStripline.StripWidth = (cycle.EndMinGreen - cycle.StartGreen).TotalHours;
                //minGreenStripline.StripWidthType = DateTimeIntervalType.Hours;
                //Chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(GreenStripline);
            }
        }
    }
}
