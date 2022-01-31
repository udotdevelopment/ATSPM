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


            //Set the chart title
            SetChartTitles(chart);

            //Create the chart legend
            var chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            chart.Legends.Add(chartLegend);

            //Create the chart area
            if (YAxisMax > 0)
                chart.ChartAreas[0].AxisY.Maximum = YAxisMax.Value;
            else
                chart.ChartAreas[0].AxisY.Maximum = 60;

            if (YAxisMin > 0)
                chart.ChartAreas[0].AxisY.Minimum = YAxisMin;
            else
                chart.ChartAreas[0].AxisY.Minimum = 0;

            chart.ChartAreas[0].AxisY.Title = "Count of Vehicles";
            chart.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chart.ChartAreas[0].AxisY.Interval = 10;
            chart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;

            chart.ChartAreas[0].AxisX.Minimum = StartDate.ToOADate();
            chart.ChartAreas[0].AxisX.Maximum = EndDate.ToOADate();


            var gapSeries = new Series();
            gapSeries.ChartType = SeriesChartType.Line;
            gapSeries.Color = Color.DarkBlue;
            gapSeries.Name = "Number of Aceptable Gaps";
            gapSeries.XValueType = ChartValueType.DateTime;
            chart.Series.Add(gapSeries);
            
            var demandSeries = new Series();
            demandSeries.ChartType = SeriesChartType.Line;
            demandSeries.Color = Color.Orange;
            demandSeries.Name = "Demand List";
            demandSeries.XValueType = ChartValueType.DateTime;
            chart.Series.Add(demandSeries);

            return chart;
        }

        private void SetChartTitles(Chart chart)
        {
            chart.Titles.Add("Acceptable Gaps versus Demand");
        }

        protected void AddDataToChart(Chart chart)
        {
            if (AcceptableGapList != null)
            {
                foreach (var bucket in AcceptableGapList)
                {
                    chart.Series["Number of Aceptable Gaps"].Points.AddXY(bucket.Key.ToOADate(), bucket.Value);
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