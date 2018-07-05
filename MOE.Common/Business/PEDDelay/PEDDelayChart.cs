using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.WCFServiceLibrary;

namespace MOE.Common.Business.PEDDelay
{
    public class PEDDelayChart
    {
        public Chart chart = new Chart();
        private readonly PedPhase PedPhase;

        public PEDDelayChart(PedDelayOptions options,
            PedPhase pp)
        {
            PedPhase = pp;
            var extendedDirection = string.Empty;
            var reportTimespan = options.EndDate - options.StartDate;

            //Set the chart properties
            ChartFactory.SetImageProperties(chart);


            SetChartTitle(chart, pp, options);

            //Create the chart legend
            var chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            chart.Legends.Add(chartLegend);


            //Create the chart area
            var chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            chartArea.AxisY.Title = "Pedestrian Delay\nby Actuation(minutes)";
            chartArea.AxisY.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisY.Interval = 1;
            chartArea.AxisY.Minimum = DateTime.Today.ToOADate();
            chartArea.AxisY.LabelStyle.Format = "mm:ss";
            if (options.YAxisMax != null)
                chartArea.AxisY.Maximum = DateTime.Today.AddMinutes(options.YAxisMax.Value).ToOADate();

            chartArea.AxisX.Title = "Time (Hour of Day)";
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.LabelStyle.Format = "HH";
            chartArea.AxisX2.LabelStyle.Format = "HH";
            chartArea.AxisX.Minimum = PedPhase.StartDate.ToOADate();
            chartArea.AxisX.Maximum = PedPhase.EndDate.ToOADate();
            if (reportTimespan.Days < 1)
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
            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisX2.MajorTickMark.Enabled = true;
            chartArea.AxisX2.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX2.LabelAutoFitStyle = LabelAutoFitStyles.None;
            chartArea.AxisX2.Minimum = PedPhase.StartDate.ToOADate();
            chartArea.AxisX2.Maximum = PedPhase.EndDate.ToOADate();

            chart.ChartAreas.Add(chartArea);


            //Add the point series
            var PedestrianDelaySeries = new Series();
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

        private void SetChartTitle(Chart chart, PedPhase pp, PedDelayOptions options)
        {
            chart.Titles.Add(ChartTitleFactory.GetChartName(options.MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(
                options.SignalID, options.StartDate, options.EndDate));
            chart.Titles.Add(ChartTitleFactory.GetPhase(pp.PhaseNumber));
            var statistics = new Dictionary<string, string>();
            statistics.Add("Ped Actuations(PA)", pp.PedActuations.ToString());
            statistics.Add("Min Delay", DateTime.Today.AddMinutes(pp.MinDelay / 60).ToString("mm:ss"));
            statistics.Add("Max Delay", DateTime.Today.AddMinutes(pp.MaxDelay / 60).ToString("mm:ss"));
            statistics.Add("Average Delay(AD)", DateTime.Today.AddMinutes(pp.AverageDelay / 60).ToString("mm:ss"));
            chart.Titles.Add(ChartTitleFactory.GetStatistics(statistics));
        }


        protected void AddDataToChart()
        {
            foreach (var pp in PedPhase.Plans)
            foreach (var pc in pp.Cycles)
                chart.Series["Pedestrian Delay\nby Actuation"].Points
                    .AddXY(pc.BeginWalk, DateTime.Today.AddMinutes(pc.Delay / 60));
        }


        protected void SetPlanStrips()
        {
            var backGroundColor = 1;
            foreach (var plan in PedPhase.Plans)
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
                stripline.IntervalOffset = (plan.StartDate - PedPhase.StartDate).TotalHours;
                stripline.StripWidth = (plan.EndDate - plan.StartDate).TotalHours;
                stripline.StripWidthType = DateTimeIntervalType.Hours;

                chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

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
                Plannumberlabel.RowIndex = 3;

                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(Plannumberlabel);


                var pedActuationsLabel = new CustomLabel();
                pedActuationsLabel.FromPosition = plan.StartDate.ToOADate();
                pedActuationsLabel.ToPosition = plan.EndDate.ToOADate();
                pedActuationsLabel.Text = plan.PedActuations + " PA";
                pedActuationsLabel.LabelMark = LabelMarkStyle.LineSideMark;
                pedActuationsLabel.RowIndex = 2;
                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(pedActuationsLabel);


                var avgDelayLabel = new CustomLabel();
                avgDelayLabel.FromPosition = plan.StartDate.ToOADate();
                avgDelayLabel.ToPosition = plan.EndDate.ToOADate();
                avgDelayLabel.Text = Math.Round(plan.AvgDelay / 60) + " AD";
                avgDelayLabel.RowIndex = 1;
                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(avgDelayLabel);

                //Change the background color counter for alternating color
                backGroundColor++;
            }
        }
    }
}