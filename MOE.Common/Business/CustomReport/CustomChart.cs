using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;

namespace MOE.Common.Business.CustomReport
{
    public class CustomChart
    {
        public List<Chart> Charts = new List<Chart>();


        public CustomChart(string signalID, DateTime startDate, DateTime endDate,
            bool showPlans, bool showRedYellowGreen, List<GraphSeries> series
        )
        {
            var eventCodes = new List<int>();
            foreach (var g in series)
                eventCodes.Add(g.EventCode);
            StartDate = startDate;
            EndDate = endDate;
            Signal = new Signal(signalID, startDate, endDate, eventCodes, 10);


            var chartIndex = 0;
            foreach (var phase in Signal.Phases)
            {
                Charts.Add(new Chart());

                CreateChartDefaults(chartIndex);

                if (showPlans)
                    ShowPlans(signalID, startDate, endDate, chartIndex);

                if (showRedYellowGreen)
                    AddRedYellowGreen(phase, chartIndex);
                if (series.Count > 0)
                    AddAdditionalSeries(series, chartIndex, phase);

                chartIndex++;
            }
        }

        public Signal Signal { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        private void AddAdditionalSeries(List<GraphSeries> series, int chartIndex, Phase phase)
        {
            //Add the user defined series
            var i = 0;
            foreach (var g in series)
            {
                var chartName = "Custom Series " + i;
                var customSeries = new Series();
                customSeries.ChartType = g.SeriesType;
                customSeries.Color = g.SeriesColor;
                customSeries.Name = chartName;
                customSeries.XValueType = ChartValueType.DateTime;
                Charts[chartIndex].Series.Add(customSeries);
                i++;

                foreach (var c in phase.Cycles)
                {
                    var customPoints =
                    (from ge in phase.Events
                        where ge.EventCode == g.EventCode &&
                              ge.Timestamp > c.CycleStart &&
                              ge.Timestamp < c.ChangeToRed
                        select ge).ToList();

                    foreach (var l in customPoints)
                        Charts[chartIndex].Series[chartName].Points.AddXY(
                            l.Timestamp.ToOADate(), (l.Timestamp - c.CycleStart).TotalSeconds);
                }
            }
        }

        private void AddRedYellowGreen(Phase phase, int chartIndex)
        {
            //Add the green series
            var greenSeries = new Series();
            greenSeries.ChartType = SeriesChartType.Line;
            greenSeries.Color = Color.DarkGreen;
            greenSeries.Name = "Change to Green";
            greenSeries.XValueType = ChartValueType.DateTime;
            greenSeries.BorderWidth = 1;
            Charts[chartIndex].Series.Add(greenSeries);

            //Add the yellow series
            var yellowSeries = new Series();
            yellowSeries.ChartType = SeriesChartType.Line;
            yellowSeries.Color = Color.Yellow;
            yellowSeries.Name = "Change to Yellow";
            yellowSeries.XValueType = ChartValueType.DateTime;
            Charts[chartIndex].Series.Add(yellowSeries);

            //Add the red series
            var redSeries = new Series();
            redSeries.ChartType = SeriesChartType.Line;
            redSeries.Color = Color.Red;
            redSeries.Name = "Change to Red";
            redSeries.XValueType = ChartValueType.DateTime;
            Charts[chartIndex].Series.Add(redSeries);

            foreach (var cycle in phase.Cycles)
            {
                Charts[chartIndex].Series["Change to Green"].Points.AddXY(
                    cycle.ChangeToGreen, (cycle.ChangeToGreen - cycle.CycleStart).TotalSeconds);
                Charts[chartIndex].Series["Change to Yellow"].Points.AddXY(
                    cycle.BeginYellowClear, (cycle.BeginYellowClear - cycle.CycleStart).TotalSeconds);
                Charts[chartIndex].Series["Change to Red"].Points.AddXY(
                    cycle.ChangeToRed, (cycle.ChangeToRed - cycle.CycleStart).TotalSeconds);
            }
        }

        private void CreateChartDefaults(int chartIndex)
        {
            //Set the chart properties
            Charts[chartIndex].ImageStorageMode = ImageStorageMode.UseImageLocation;
            Charts[chartIndex].ImageType = ChartImageType.Jpeg;
            Charts[chartIndex].Height = 450;
            Charts[chartIndex].Width = 1100;


            //Create the chart area
            //Create the chart area
            var chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Title = "Cycle Time (Seconds) ";

            chartArea.AxisX.Title = "Time (Hour of Day)";
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.LabelStyle.Format = "HH";
            chartArea.AxisX.Minimum = StartDate.ToOADate();
            chartArea.AxisX.Maximum = EndDate.ToOADate();

            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisX2.MajorTickMark.Enabled = true;
            chartArea.AxisX2.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX2.LabelStyle.Format = "HH";
            chartArea.AxisX2.LabelAutoFitStyle = LabelAutoFitStyles.None;
            chartArea.AxisX2.Interval = 1;
            chartArea.AxisX2.Minimum = StartDate.ToOADate();
            chartArea.AxisX2.Maximum = EndDate.ToOADate();

            Charts[chartIndex].ChartAreas.Add(chartArea);
        }

        private void ShowPlans(string signalID, DateTime startDate, DateTime endDate, int chartIndex)
        {
            var plansBase = new PlansBase(signalID, startDate, endDate);
            var plans = new List<Plan>();

            for (var i = 0; i < plansBase.Events.Count; i++)
                //if this is the last plan then we want the end of the plan
                //to cooincide with the end of the graph
                if (plansBase.Events.Count - 1 == i)
                {
                    var plan = new Plan(plansBase.Events[i].Timestamp, endDate,
                        plansBase.Events[i].EventParam);
                    plans.Add(plan);
                }
                //else we add the plan with the next plans' time stamp as the end of the plan
                else
                {
                    var plan = new Plan(plansBase.Events[i].Timestamp,
                        plansBase.Events[i + 1].Timestamp, plansBase.Events[i].EventParam);

                    plans.Add(plan);
                }

            var backGroundColor = 1;
            foreach (var plan in plans)
            {
                var stripline = new StripLine();
                //Creates alternating backcolor to distinguish the plans
                if (backGroundColor % 2 == 0)
                    stripline.BackColor = Color.FromArgb(120, Color.LightGray);
                else
                    stripline.BackColor = Color.FromArgb(120, Color.LightBlue);

                //Set the stripline properties
                stripline.IntervalOffsetType = DateTimeIntervalType.Hours;
                stripline.Interval = 1;
                stripline.IntervalOffset = (plan.StartDate - startDate).TotalHours;
                stripline.StripWidth = (plan.EndDate - plan.StartDate).TotalHours;
                stripline.StripWidthType = DateTimeIntervalType.Hours;

                Charts[chartIndex].ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

                //Add a corrisponding custom label for each strip
                var Plannumberlabel = new CustomLabel();
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
                        Plannumberlabel.Text = "Plan " + plan.PlanNumber;

                        break;
                }

                Plannumberlabel.ForeColor = Color.Black;
                Plannumberlabel.RowIndex = 1;

                Charts[chartIndex].ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(Plannumberlabel);
            }
        }
    }
}