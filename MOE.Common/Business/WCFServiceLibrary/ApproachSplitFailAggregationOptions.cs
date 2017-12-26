using System;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.DataAggregation;
using static MOE.Common.Business.WCFServiceLibrary.AggregationMetricOptions;

namespace MOE.Common.Business.WCFServiceLibrary
{
    public class ApproachSplitFailAggregationOptions: AggregationMetricOptions
    {
        public  void ApproachSplitFailAggregationOption()
        {
            MetricTypeID = 20;
            var metricTypeRepository = MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();
            MetricType = metricTypeRepository.GetMetricsByID(MetricTypeID);
        }

        public override List<string> CreateMetric()
        {
            MetricTypeID = 20;
            var metricTypeRepository = MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();
            MetricType = metricTypeRepository.GetMetricsByID(MetricTypeID);
            base.CreateMetric();
            SpliFailAggregationBySignal spliFailAggregationBySignal = new SpliFailAggregationBySignal(this);
            int i = 1;
            Chart chart = ChartFactory.CreateSplitFailureAggregationChart(this);
            foreach (var approachSplitFails in spliFailAggregationBySignal.Containers)
            {
                if (i > 10)
                {
                    i = 1;
                }
                AddSeries(chart, approachSplitFails, i);
                i++;
            }
            string chartName = CreateFileName("SFA");
            MetricFileLocation = ConfigurationManager.AppSettings["ImageLocation"];
            MetricWebPath = ConfigurationManager.AppSettings["ImageWebLocation"];
            chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);
            ReturnList.Add(MetricWebPath + chartName);
            return ReturnList;
        }

        public string CreateFileName(string MetricAbbreviation)
        {
            string fileName = MetricAbbreviation +
                              "-" +
                              StartDate.Year.ToString() +
                              StartDate.ToString("MM") +
                              StartDate.ToString("dd") +
                              StartDate.ToString("HH") +
                              StartDate.ToString("mm") +
                              "-" +
                              EndDate.Year.ToString() +
                              EndDate.ToString("MM") +
                              EndDate.ToString("dd") +
                              EndDate.ToString("HH") +
                              EndDate.ToString("mm-");




            Random r = new Random();
            fileName += r.Next().ToString();
            fileName += ".jpg";

            return fileName;
        }

        private void AddSeries(Chart chart, ApproachSplitFailAggregationContainer approachSplitFailAggregationContainer, int colorNumber)
        {
            Series series = new Series();
            series.ChartType = SeriesChartType.StackedColumn;
            series.Color = GetSeriesColorByNumber(colorNumber);
            series.Name = approachSplitFailAggregationContainer.Approach.DirectionType.Abbreviation + " " +
                          approachSplitFailAggregationContainer.Approach.ProtectedPhaseNumber;
            series.ChartArea = "ChartArea1";
            series.BorderWidth = 1;
            series.BorderColor = Color.Black;
            series.BorderDashStyle = ChartDashStyle.Solid;
            foreach (var splitFail in approachSplitFailAggregationContainer.SplitFails)
            {
                series.Points.AddXY(splitFail.BinStartTime, splitFail.SplitFailures);
            }
            chart.Series.Add(series);
        }

        private Color GetSeriesColorByNumber(int colorNumber)
        {
            switch (colorNumber)
            {
                case 1:
                    return Color.FromArgb(178, 4, 0);

                case 2:
                    return Color.FromArgb(235, 126, 110);

                case 3:
                    return Color.FromArgb(239, 160, 43);

                case 4:
                    return Color.FromArgb(253, 208, 125);

                case 5:
                    return Color.FromArgb(185, 204, 18);

                case 6:
                    return Color.FromArgb(95, 147, 23);

                case 7:
                    return Color.FromArgb(44, 92, 18);

                case 8:
                    return Color.FromArgb(101, 114, 148);

                case 9:
                    return Color.FromArgb(58, 61, 115);

                case 10:
                    return Color.FromArgb(25, 17, 64);

                default:
                    return Color.Black;
            }
        }

    }


}
