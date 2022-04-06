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
            Axis X = chart.ChartAreas[0].AxisX;
            Axis Y = chart.ChartAreas[0].AxisY;

            if (YAxisMax > 0)
                Y.Maximum = YAxisMax.Value;
            else
                Y.Maximum = 60;

            if (YAxisMin > 0)
                Y.Minimum = YAxisMin;
            else
                Y.Minimum = 0;

            Y.Title = "Count of Vehicles";
            Y.TitleFont = new Font("Arial", 15f);
            Y.LabelStyle.Font = new Font("Arial", 10f);
            Y.IntervalAutoMode = IntervalAutoMode.FixedCount;
            if (YAxisMax <= 30)
                Y.Interval = 5;
            else
                Y.Interval = 10;
            Y.MajorTickMark.TickMarkStyle = TickMarkStyle.InsideArea;
            Y.MajorGrid.LineColor = chart.BackColor;

            X.Title = "Duration by 15 Minute Bins";
            X.TitleFont = new Font("Arial", 15f);
            X.LabelStyle.Font = new Font("Arial", 11f);
            X.MajorGrid.LineColor = chart.BackColor;
            X.Minimum = StartDate.ToOADate();
            X.Maximum = EndDate.ToOADate();
            X.MajorTickMark.TickMarkStyle = TickMarkStyle.InsideArea;
            X.IntervalType = DateTimeIntervalType.Minutes;
            X.Interval = 30;
            X.LabelStyle.Angle = -90;


            var gapSeries = new Series();
            gapSeries.ChartType = SeriesChartType.Line;
            gapSeries.Color = Color.FromArgb(92, 136, 218);
            gapSeries.Name = "Number of Acceptable Gaps";
            gapSeries.XValueType = ChartValueType.DateTime;
            gapSeries.BorderWidth = 3;
            gapSeries.Font = new Font("Arial", 10f);
            chart.Series.Add(gapSeries);
            
            var demandSeries = new Series();
            demandSeries.ChartType = SeriesChartType.Line;
            demandSeries.Color = Color.FromArgb(232, 119, 34);
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