using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.WCFServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Business.Bins;

namespace MOE.Common.Business.WCFServiceLibrary.Tests
{
    [TestClass()]
    public class ApproachSplitFailAggregationOptionsTests
    {
        [TestMethod()]
        public void CreateMetricTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017 7:00 AM"), 
                Convert.ToDateTime("10/17/2017 8:00 AM"), null, null, null, null, null, 
                BinFactoryOptions.BinSizes.FifteenMinutes, 
                BinFactoryOptions.TimeOptions.StartToEnd);
            
            //options.SignalIds.Add("8279");
            //options.SignalIds.Add("7185");
            options.SignalIds.Add("5605");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }
    }
}