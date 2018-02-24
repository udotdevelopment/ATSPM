using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.WCFServiceLibrary;

namespace MOE.CommonTests.Business.WCFServiceLibrary
{
    public class AggregationTestHelper
    {
        public static void CreateMetrics(SignalAggregationMetricOptions options)
        {
            options.SelectedChartType = SeriesChartType.Column;
            options.SelectedAggregationType = AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.Line;
            options.SelectedAggregationType = AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.Pie;
            options.SelectedAggregationType = AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType = AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedArea;
            options.SelectedAggregationType = AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationType.Average;
            options.CreateMetric();
        }
    }
}
