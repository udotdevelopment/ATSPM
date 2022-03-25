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
    public class GapVsDemandOptions : MetricOptions
    {
        public GapVsDemandOptions(
            string signalID,
            DateTime startDate,
            DateTime endDate,
            double yAxisMax,
            double yAxisMin,
            int metricTypeID, 
            Dictionary<DateTime, double> acceptableGapList, 
            Dictionary<DateTime, double> demandList)
        {
            SignalID = signalID;
            StartDate = startDate;
            EndDate = endDate;
            YAxisMax = yAxisMax;
            YAxisMin = yAxisMin;
            MetricTypeID = metricTypeID;
            AcceptableGapList = acceptableGapList;
            DemandList = demandList;
        }


        public GapVsDemandOptions()
        {
            SetDefaults();
        }

        [Required]
        [DataMember]
        [Display(Name = "Acceptable Gap List")]
        public Dictionary<DateTime, double> AcceptableGapList { get; internal set; }
        [Required]
        [DataMember]
        [Display(Name = "Demand List")]
        public Dictionary<DateTime, double> DemandList { get; internal set; }
        

        

        public void SetDefaults()
        {
            YAxisMax = 60;
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

            chart.BorderlineColor = chart.BackColor;


            //Set the chart title
            SetChartTitles(chart);

            //Create the chart legend
            var chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Top;
            chartLegend.Alignment = StringAlignment.Center;
            chartLegend.Font = new Font(chartLegend.Font.FontFamily, 14);
            chart.Legends.Add(chartLegend);

            //Create the chart area
            ChartArea chartArea = chart.ChartAreas[0];

            if (YAxisMax > 0)
                chartArea.AxisY.Maximum = YAxisMax.Value;
            else
                chartArea.AxisY.Maximum = 60;

            if (YAxisMin > 0)
                chartArea.AxisY.Minimum = YAxisMin;
            else
                chartArea.AxisY.Minimum = 0;

            chartArea.AxisY.Title = "Count of Vehicles";
            chartArea.AxisY.TitleFont = new Font("Arial", 15f);
            chartArea.AxisY.LabelStyle.Font = new Font("Arial", 10f);
            chartArea.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chartArea.AxisY.Interval = 10;
            chartArea.AxisY.MajorGrid.LineColor = chart.BackColor;

            chartArea.AxisX.Title = "Duration by 15 Minute Bins";
            chartArea.AxisX.TitleFont = new Font("Arial", 15f);
            chartArea.AxisX.LabelStyle.Font = new Font("Arial", 11f);
            chartArea.AxisX.MajorGrid.LineColor = chart.BackColor;
            chartArea.AxisX.Minimum = StartDate.ToOADate();
            chartArea.AxisX.Maximum = EndDate.ToOADate();
            chartArea.AxisX.LabelStyle.Angle = -90;


            var gapSeries = new Series();
            gapSeries.ChartType = SeriesChartType.Line;
            gapSeries.Color = Color.DarkBlue;
            gapSeries.Name = "Number of Acceptable Gaps";
            gapSeries.XValueType = ChartValueType.DateTime;
            gapSeries.BorderWidth = 3;
            gapSeries.Font = new Font("Arial", 10f);
            chart.Series.Add(gapSeries);
            
            var demandSeries = new Series();
            demandSeries.ChartType = SeriesChartType.Line;
            demandSeries.Color = Color.Orange;
            demandSeries.Name = "Demand List";
            demandSeries.XValueType = ChartValueType.DateTime;
            demandSeries.BorderWidth = 3;
            demandSeries.Font = new Font("Arial", 10f);
            chart.Series.Add(demandSeries);

            return chart;
        }

        private void SetChartTitles(Chart chart)
        {
            Title title = new Title();
            title.Font = new Font("Arial", 18);
            title.Text = "Acceptable Gaps versus Demand";
            chart.Titles.Add(title);
        }

        protected void AddDataToChart(Chart chart)
        {
            if (AcceptableGapList != null)
            {
                foreach (var bucket in AcceptableGapList)
                {
                    chart.Series["Number of Acceptable Gaps"].Points.AddXY(bucket.Key.ToOADate(), bucket.Value);
                }
            }
            if (DemandList != null)
            {
                foreach (var bucket in DemandList)
                {
                    chart.Series["Demand List"].Points.AddXY(bucket.Key.ToOADate(), bucket.Value);
                }
            }
        }

    }
}