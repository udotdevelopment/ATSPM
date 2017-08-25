using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace MOE.Common.Business.CustomReport
{
    public class CustomChart
    {
        public List<Chart> Charts = new List<Chart>();

        private MOE.Common.Business.CustomReport.Signal _Signal;

        public MOE.Common.Business.CustomReport.Signal Signal
        {
            get { return _Signal; }
            set { _Signal = value; }
        }

        private DateTime _StartDate;

        public DateTime StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; }
        }

        private DateTime _EndDate;

        public DateTime EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; }
        }
        
        
        
        public CustomChart(string signalID, DateTime startDate, DateTime endDate, 
            bool showPlans, bool showRedYellowGreen, List<CustomReport.GraphSeries> series
            )
        {
            List<int> eventCodes = new List<int>();
            foreach(CustomReport.GraphSeries g in series)
            {
                eventCodes.Add(g.EventCode);
            }
            _StartDate = startDate;
            _EndDate = endDate;
            _Signal = new CustomReport.Signal(signalID, startDate, endDate, eventCodes, 10);

            

            int chartIndex = 0;
            foreach (CustomReport.Phase phase in _Signal.Phases)
            {
                Charts.Add(new Chart());

                CreateChartDefaults(chartIndex);

                if (showPlans)
                {
                    ShowPlans(signalID, startDate, endDate, chartIndex);
                }
                
                if (showRedYellowGreen)
                {
                    AddRedYellowGreen(phase, chartIndex);
                }
                if(series.Count > 0)
                {
                    AddAdditionalSeries(series, chartIndex, phase);
                }

                chartIndex++;
            }
        }

        private void AddAdditionalSeries(List<GraphSeries> series, int chartIndex, CustomReport.Phase phase)
        {
            //Add the user defined series
            int i = 0;
            foreach (GraphSeries g in series)
            {
                string chartName = "Custom Series " + i.ToString();
                Series customSeries = new Series();
                customSeries.ChartType = g.SeriesType;
                customSeries.Color = g.SeriesColor;
                customSeries.Name = chartName;
                customSeries.XValueType = ChartValueType.DateTime;
                Charts[chartIndex].Series.Add(customSeries);
                i++;

                foreach (CustomReport.Cycle c in phase.Cycles)
                {
                    List<Models.Controller_Event_Log> customPoints =
                        (from ge in phase.Events
                         where ge.EventCode == g.EventCode &&
                         ge.Timestamp > c.CycleStart &&
                         ge.Timestamp < c.ChangeToRed
                         select ge).ToList();

                    foreach (Models.Controller_Event_Log l in customPoints)
                    {
                        Charts[chartIndex].Series[chartName].Points.AddXY(
                        l.Timestamp.ToOADate(), (l.Timestamp - c.CycleStart).TotalSeconds);
                    }
                }
            }
        }

        private void AddRedYellowGreen(CustomReport.Phase phase, int chartIndex)
        {
            //Add the green series
            Series greenSeries = new Series();
            greenSeries.ChartType = SeriesChartType.Line;
            greenSeries.Color = Color.DarkGreen;
            greenSeries.Name = "Change to Green";
            greenSeries.XValueType = ChartValueType.DateTime;
            greenSeries.BorderWidth = 1;
            Charts[chartIndex].Series.Add(greenSeries);

            //Add the yellow series
            Series yellowSeries = new Series();
            yellowSeries.ChartType = SeriesChartType.Line;
            yellowSeries.Color = Color.Yellow;
            yellowSeries.Name = "Change to Yellow";
            yellowSeries.XValueType = ChartValueType.DateTime;
            Charts[chartIndex].Series.Add(yellowSeries);

            //Add the red series
            Series redSeries = new Series();
            redSeries.ChartType = SeriesChartType.Line;
            redSeries.Color = Color.Red;
            redSeries.Name = "Change to Red";
            redSeries.XValueType = ChartValueType.DateTime;
            Charts[chartIndex].Series.Add(redSeries);

            foreach(CustomReport.Cycle cycle in phase.Cycles)
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
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Title = "Cycle Time (Seconds) ";

            chartArea.AxisX.Title = "Time (Hour of Day)";
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.LabelStyle.Format = "HH";
            chartArea.AxisX.Minimum = _StartDate.ToOADate();
            chartArea.AxisX.Maximum = _EndDate.ToOADate();

            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisX2.MajorTickMark.Enabled = true;
            chartArea.AxisX2.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX2.LabelStyle.Format = "HH";
            chartArea.AxisX2.LabelAutoFitStyle = LabelAutoFitStyles.None;
            chartArea.AxisX2.Interval = 1;
            chartArea.AxisX2.Minimum = _StartDate.ToOADate();
            chartArea.AxisX2.Maximum = _EndDate.ToOADate();

            Charts[chartIndex].ChartAreas.Add(chartArea);
        }

        private void ShowPlans(string signalID, DateTime startDate, DateTime endDate, int chartIndex)
        {
            PlansBase plansBase = new PlansBase(signalID, startDate, endDate);
            List<CustomReport.Plan> plans = new List<CustomReport.Plan>();
            
            for (int i = 0; i < plansBase.Events.Count; i++)
            {
                //if this is the last plan then we want the end of the plan
                //to cooincide with the end of the graph
                if (plansBase.Events.Count - 1 == i)
                {
                    Plan plan = new Plan(signalID, plansBase.Events[i].Timestamp, endDate,
                        plansBase.Events[i].EventParam);
                    plans.Add(plan);
                }
                //else we add the plan with the next plans' time stamp as the end of the plan
                else
                {

                    Plan plan = new Plan(signalID, plansBase.Events[i].Timestamp, 
                        plansBase.Events[i + 1].Timestamp, plansBase.Events[i].EventParam);

                    plans.Add(plan);

                }
            }
           
            int backGroundColor = 1;
            foreach (Plan plan in plans)
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
                stripline.IntervalOffset = (plan.StartDate - startDate).TotalHours;
                stripline.StripWidth = (plan.EndDate - plan.StartDate).TotalHours;
                stripline.StripWidthType = DateTimeIntervalType.Hours;

                Charts[chartIndex].ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

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
                Plannumberlabel.RowIndex = 1;

                Charts[chartIndex].ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(Plannumberlabel);
            }
        }
    }
}
