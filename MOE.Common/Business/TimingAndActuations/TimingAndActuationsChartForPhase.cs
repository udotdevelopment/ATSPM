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
            /*******************************************************************************************************
            *  For some unknown reason, the stripes only show up when there is data in the chart!  So add a point  *
            *  at the orgin.  This makes the stipes be displayed even if there is no other data for the chart!     *
            *******************************************************************************************************/
            var redSeries = new Series();
            redSeries.ChartType = SeriesChartType.Point;
            redSeries.Color = Color.Red;
            redSeries.Name = "Change to Red";
            redSeries.Points.AddXY(TimingAndActuationsForPhase.Options.StartDate.Hour, 0);
            redSeries.XValueType = ChartValueType.DateTime;

            Chart.Series.Add(redSeries);
            Chart.Titles.Add(ChartTitleFactory.GetChartName(TimingAndActuationsForPhase.Options.MetricTypeID));
            Chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(
                TimingAndActuationsForPhase.Options.SignalID, TimingAndActuationsForPhase.Options.StartDate, TimingAndActuationsForPhase.Options.EndDate));
            DateTime endOfLastCycle = TimingAndActuationsForPhase.Options.EndDate; //    StartDate; Not sure if start or end date
            switch (Chart.ChartAreas[0].AxisX.IntervalType)
            { 
                case DateTimeIntervalType.Minutes:
                    foreach (var cycle in TimingAndActuationsForPhase.Cycles)
                    {
                        for (int CycleColor = 1; CycleColor < 6; CycleColor++)
                        {
                            var minStripLine = new StripLine();
                            switch (CycleColor)
                            {
                                case 1:
                                    minStripLine.BackColor = Color.FromArgb(120, Color.DarkGreen);
                                    minStripLine.IntervalOffset =
                                        (cycle.StartGreen - TimingAndActuationsForPhase.Options.StartDate).TotalMinutes;
                                    minStripLine.StripWidth = (cycle.EndMinGreen - cycle.StartGreen).TotalMinutes;
                                    break;
                                case 2:
                                    minStripLine.BackColor = Color.FromArgb(120, Color.LightGreen);
                                    minStripLine.IntervalOffset =
                                        (cycle.EndMinGreen - TimingAndActuationsForPhase.Options.StartDate).TotalMinutes;
                                    minStripLine.StripWidth = (cycle.StartYellow - cycle.EndMinGreen).TotalMinutes;
                                    break;
                                case 3:
                                    minStripLine.BackColor = Color.FromArgb(120, Color.Yellow);
                                    minStripLine.IntervalOffset =
                                        (cycle.StartYellow - TimingAndActuationsForPhase.Options.StartDate).TotalMinutes;
                                    minStripLine.StripWidth = (cycle.StartRedClearance - cycle.StartYellow).TotalMinutes;
                                    break;
                                case 4:
                                    minStripLine.BackColor = Color.FromArgb(120, Color.DarkRed);
                                    minStripLine.IntervalOffset =
                                        (cycle.StartRedClearance - TimingAndActuationsForPhase.Options.StartDate).TotalMinutes;
                                    minStripLine.StripWidth = (cycle.StartRed - cycle.StartRedClearance).TotalMinutes;
                                    break;
                                case 5:
                                    minStripLine.BackColor = Color.FromArgb(120, Color.LightCoral);
                                    minStripLine.IntervalOffset =
                                        (cycle.StartRed - TimingAndActuationsForPhase.Options.StartDate).TotalMinutes;
                                    minStripLine.StripWidth = (cycle.EndRed - cycle.StartRed).TotalMinutes;
                                    break;
                                default:
                                    break;
                            }
                            minStripLine.IntervalOffsetType = DateTimeIntervalType.Minutes;
                            minStripLine.IntervalType = DateTimeIntervalType.Hours;
                            minStripLine.Interval = 1;
                            minStripLine.StripWidthType = DateTimeIntervalType.Minutes;
                            Chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(minStripLine);
                        }
                    }
                    break;
                case DateTimeIntervalType.Hours:
                    foreach (var cycle in TimingAndActuationsForPhase.Cycles)
                    {
                        for (int CycleColor = 1; CycleColor < 6; CycleColor++)
                        {
                            var minStripLine = new StripLine();
                            switch (CycleColor)
                            {
                                case 1:
                                    minStripLine.BackColor = Color.FromArgb(120, Color.DarkGreen);
                                    minStripLine.IntervalOffset =
                                        (cycle.StartGreen - TimingAndActuationsForPhase.Options.StartDate).TotalHours;
                                    minStripLine.StripWidth = (cycle.EndMinGreen - cycle.StartGreen).TotalHours;
                                    break;
                                case 2:
                                    minStripLine.BackColor = Color.FromArgb(120, Color.LightGreen);
                                    minStripLine.IntervalOffset =
                                        (cycle.EndMinGreen - TimingAndActuationsForPhase.Options.StartDate).TotalHours;
                                    minStripLine.StripWidth = (cycle.StartYellow - cycle.EndMinGreen).TotalHours;
                                    break;
                                case 3:
                                    minStripLine.BackColor = Color.FromArgb(120, Color.Yellow);
                                    minStripLine.IntervalOffset =
                                        (cycle.StartYellow - TimingAndActuationsForPhase.Options.StartDate).TotalHours;
                                    minStripLine.StripWidth = (cycle.StartRedClearance - cycle.StartYellow).TotalHours;
                                    break;
                                case 4:
                                    minStripLine.BackColor = Color.FromArgb(120, Color.DarkRed);
                                    minStripLine.IntervalOffset =
                                        (cycle.StartRedClearance - TimingAndActuationsForPhase.Options.StartDate).TotalHours;
                                    minStripLine.StripWidth = (cycle.StartRed - cycle.StartRedClearance).TotalHours;
                                    break;
                                case 5:
                                    minStripLine.BackColor = Color.FromArgb(120, Color.LightCoral);
                                    minStripLine.IntervalOffset =
                                        (cycle.StartRed - TimingAndActuationsForPhase.Options.StartDate).TotalHours;
                                    minStripLine.StripWidth = (cycle.EndRed - cycle.StartRed).TotalHours;
                                    break;
                                default:
                                    break;
                            }
                            minStripLine.IntervalOffsetType = DateTimeIntervalType.Hours;
                            minStripLine.IntervalType = DateTimeIntervalType.Days;
                            minStripLine.Interval = 1;
                            minStripLine.StripWidthType = DateTimeIntervalType.Hours;
                            Chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(minStripLine);
                        }
                    }
                    break;
                case DateTimeIntervalType.Days:
                    foreach (var cycle in TimingAndActuationsForPhase.Cycles)
                    {
                        for (int CycleColor = 1; CycleColor < 6; CycleColor++)
                        {
                            var minStripLine = new StripLine();
                            switch (CycleColor)
                            {
                                case 1:
                                    minStripLine.BackColor = Color.FromArgb(120, Color.DarkGreen);
                                    minStripLine.IntervalOffset =
                                        (cycle.StartGreen - TimingAndActuationsForPhase.Options.StartDate).TotalDays;
                                    minStripLine.StripWidth = (cycle.EndMinGreen - cycle.StartGreen).TotalDays;
                                    break;
                                case 2:
                                    minStripLine.BackColor = Color.FromArgb(120, Color.LightGreen);
                                    minStripLine.IntervalOffset =
                                        (cycle.EndMinGreen - TimingAndActuationsForPhase.Options.StartDate).TotalDays;
                                    minStripLine.StripWidth = (cycle.StartYellow - cycle.EndMinGreen).TotalDays;
                                    break;
                                case 3:
                                    minStripLine.BackColor = Color.FromArgb(120, Color.Yellow);
                                    minStripLine.IntervalOffset =
                                        (cycle.StartYellow - TimingAndActuationsForPhase.Options.StartDate).TotalDays;
                                    minStripLine.StripWidth = (cycle.StartRedClearance - cycle.StartYellow).TotalDays;
                                    break;
                                case 4:
                                    minStripLine.BackColor = Color.FromArgb(120, Color.DarkRed);
                                    minStripLine.IntervalOffset =
                                        (cycle.StartRedClearance - TimingAndActuationsForPhase.Options.StartDate).TotalDays;
                                    minStripLine.StripWidth = (cycle.StartRed - cycle.StartRedClearance).TotalDays;
                                    break;
                                case 5:
                                    minStripLine.BackColor = Color.FromArgb(120, Color.LightCoral);
                                    minStripLine.IntervalOffset =
                                        (cycle.StartRed - TimingAndActuationsForPhase.Options.StartDate).TotalDays;
                                    minStripLine.StripWidth = (cycle.EndRed - cycle.StartRed).TotalDays;
                                    break;
                                default:
                                    break;
                            }
                            minStripLine.IntervalOffsetType = DateTimeIntervalType.Days;
                            minStripLine.IntervalType = DateTimeIntervalType.Months;
                            minStripLine.Interval = 1;
                            minStripLine.StripWidthType = DateTimeIntervalType.Days;
                            Chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(minStripLine);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
