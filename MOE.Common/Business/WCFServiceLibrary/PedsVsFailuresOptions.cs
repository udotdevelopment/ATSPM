using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Runtime.Serialization;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.Speed;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class PedsVsFailuresOptions : MetricOptions
    {
        public PedsVsFailuresOptions(
            string signalID,
            DateTime startDate,
            DateTime endDate,
            double yAxisMax,
            double yAxisMin,
            int metricTypeID, 
            Dictionary<DateTime, double> percentCyclesWithPeds, 
            Dictionary<DateTime, double> percentCyclesWithSplitFails)
        {
            SignalID = signalID;
            StartDate = startDate;
            EndDate = endDate;
            YAxisMax = yAxisMax;
            YAxisMin = yAxisMin;
            MetricTypeID = metricTypeID;
            PercentPedsList = percentCyclesWithPeds;
            PercentFailuresList = percentCyclesWithSplitFails;
        }


        public PedsVsFailuresOptions()
        {
            SetDefaults();
        }

        [Required]
        [DataMember]
        [Display(Name = "Percent of Cycles w/ Peds")]
        public Dictionary<DateTime, double> PercentPedsList { get; internal set; }
        [Required]
        [DataMember]
        [Display(Name = "Percent Cycles w/ Split Failure")]
        public Dictionary<DateTime, double> PercentFailuresList { get; internal set; }
        

        

        public void SetDefaults()
        {
            YAxisMax = 100;
            YAxisMin = 0;
            MetricTypeID = 31;
            var metricTypeRepository = MetricTypeRepositoryFactory.Create();
            MetricType = metricTypeRepository.GetMetricsByID(MetricTypeID);
            var settingsRepository = Models.Repositories.ApplicationSettingsRepositoryFactory.Create();
            var settings = settingsRepository.GetGeneralSettings();
            MetricFileLocation = settings.ImagePath;
        }


        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            var chart = GetNewChart();
            AddDataToChart(chart);
            var chartName = CreateFileName();
            chart.ImageLocation = MetricFileLocation + chartName;
            chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);
            ReturnList.Add(MetricWebPath + chartName);
            return ReturnList;
        }

        private Chart GetNewChart()
        {
            var chart = ChartFactory.CreateDefaultChartNoX2Axis(this);
            ChartFactory.SetImageProperties(chart);


            //Set the chart title
            SetChartTitles(chart);

            //Create the chart legend
            var chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Top;
            chartLegend.Alignment = StringAlignment.Center;
            chartLegend.TextWrapThreshold = 50;
            chartLegend.Font = new Font(chartLegend.Font.FontFamily, 14);
            chart.Legends.Add(chartLegend);

            //Create the chart area
            ChartArea chartArea = chart.ChartAreas[0];

            if (YAxisMax > 0)
                chartArea.AxisY.Maximum = YAxisMax.Value;
            else
                chartArea.AxisY.Maximum = 100;

            if (YAxisMin > 0)
                chartArea.AxisY.Minimum = YAxisMin;
            else
                chartArea.AxisY.Minimum = 0;

            chartArea.AxisY.Title = "Percent";
            chartArea.AxisY.TitleFont = new Font("Arial", 15f);
            chartArea.AxisY.LabelStyle.Font = new Font("Arial", 10f);
            chartArea.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chartArea.AxisY.LabelStyle.Format = "{0.00} %";
            chartArea.AxisY.Interval = 10;
            chartArea.AxisY.MajorGrid.LineColor = chart.BackColor;

            chartArea.AxisX.Title = "Duration by 15 Minute Bins";
            chartArea.AxisX.TitleFont = new Font("Arial", 15f);
            chartArea.AxisX.LabelStyle.Font = new Font("Arial", 11f);
            chartArea.AxisX.Minimum = StartDate.ToOADate();
            chartArea.AxisX.Maximum = EndDate.ToOADate();
            chartArea.AxisX.MajorGrid.LineColor = chart.BackColor;
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Minutes;
            chartArea.AxisX.Interval = 30;
            chartArea.AxisX.LabelStyle.Angle = -90;


            var gapSeries = new Series();
            gapSeries.ChartType = SeriesChartType.Line;
            gapSeries.Color = Color.FromArgb(92,136,218);
            gapSeries.Name = "% of Acceptable Cycles w/ Peds";
            gapSeries.XValueType = ChartValueType.DateTime;
            gapSeries.Font = new Font("Arial", 10f);
            gapSeries.BorderWidth = 3;
            chart.Series.Add(gapSeries);
            
            var demandSeries = new Series();
            demandSeries.ChartType = SeriesChartType.Line;
            demandSeries.Color = Color.FromArgb(232,119,34);
            demandSeries.Name = "% of Cycles w/ Split Failure";
            demandSeries.XValueType = ChartValueType.DateTime;
            demandSeries.Font = new Font("Arial", 10f);
            demandSeries.BorderWidth = 3;
            chart.Series.Add(demandSeries);

            return chart;
        }

        private void SetChartTitles(Chart chart)
        {
            Title title = new Title();
            title.Font = new Font("Arial", 18);
            title.Text = "Percent of Cycles(Pedestrians and Split Failure)";
            chart.Titles.Add(title);
        }

        protected void AddDataToChart(Chart chart)
        {
            if (PercentPedsList != null)
            {
                foreach (var bucket in PercentPedsList)
                {
                    chart.Series["% of Cycles w/ Peds"].Points.AddXY(bucket.Key.ToOADate(), bucket.Value * 100);
                }
            }
            if (PercentFailuresList != null)
            {
                foreach (var bucket in PercentFailuresList)
                {
                    chart.Series["% of Cycles w/ Split Failure"].Points.AddXY(bucket.Key.ToOADate(), bucket.Value * 100);
                }
            }
        }

    }
}