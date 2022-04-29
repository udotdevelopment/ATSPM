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
        [Display(Name = "Percent Acceptable Cycles w/ Peds")]
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
            chartLegend.Docking = Docking.Left;
            chart.Legends.Add(chartLegend);

            //Create the chart area
            if (YAxisMax > 0)
                chart.ChartAreas[0].AxisY.Maximum = YAxisMax.Value;
            else
                chart.ChartAreas[0].AxisY.Maximum = 100;

            if (YAxisMin > 0)
                chart.ChartAreas[0].AxisY.Minimum = YAxisMin;
            else
                chart.ChartAreas[0].AxisY.Minimum = 0;

            chart.ChartAreas[0].AxisY.Title = "Percent";
            chart.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chart.ChartAreas[0].AxisY.Interval = 10;
            chart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;

            chart.ChartAreas[0].AxisX.Minimum = StartDate.ToOADate();
            chart.ChartAreas[0].AxisX.Maximum = EndDate.ToOADate();


            var gapSeries = new Series();
            gapSeries.ChartType = SeriesChartType.Line;
            gapSeries.Color = Color.DarkBlue;
            gapSeries.Name = "% of Accectable Cycles w/ Peds";
            gapSeries.XValueType = ChartValueType.DateTime;
            chart.Series.Add(gapSeries);
            
            var demandSeries = new Series();
            demandSeries.ChartType = SeriesChartType.Line;
            demandSeries.Color = Color.Orange;
            demandSeries.Name = "% of Cycles w/ Split Failure";
            demandSeries.XValueType = ChartValueType.DateTime;
            chart.Series.Add(demandSeries);

            return chart;
        }

        private void SetChartTitles(Chart chart)
        {
            chart.Titles.Add("Percent of Cycles(Pedestrians and Split Failure)");
        }

        protected void AddDataToChart(Chart chart)
        {
            if (PercentPedsList != null)
            {
                foreach (var bucket in PercentPedsList)
                {
                    chart.Series["% of Accectable Cycles w/ Peds"].Points.AddXY(bucket.Key.ToOADate(), bucket.Value *100);
                }
            }
            if (PercentFailuresList != null)
            {
                foreach (var bucket in PercentFailuresList)
                {
                    chart.Series["% of Cycles w/ Split Failure"].Points.AddXY(bucket.Key.ToOADate(), bucket.Value *100);
                }
            }
        }

    }
}