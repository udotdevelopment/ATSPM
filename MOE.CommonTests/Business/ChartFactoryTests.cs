using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.CommonTests.Models;

namespace MOE.Common.Business.Tests
{
    [TestClass()]
    public class ChartFactoryTests
    {
        [TestInitialize]
        public void Initialize()
        {

        }

        [TestMethod()]
        public void CreateLineSeriesTest()
        {

            var s = ChartFactory.CreateLineSeries("TestSeries", Color.Green);

            Assert.IsTrue(s.ChartType == SeriesChartType.Line);
        }

        [TestMethod()]
        public void CreateStackedAreaSeriesTest()
        {

            var s = ChartFactory.CreateStackedAreaSeries("TestSeries", Color.Green);

            Assert.IsTrue(s.ChartType == SeriesChartType.StackedArea);
        }

        [TestMethod()]
        public void CreateColumnSeriesTest()
        {

            var s = ChartFactory.CreateColumnSeries("TestSeries", Color.Green);

            Assert.IsTrue(s.ChartType == SeriesChartType.Column);
        }

        [TestMethod()]
        public void CreateStackedColumnSeriesTest()
        {

            var s = ChartFactory.CreateStackedColumnSeries("TestSeries", Color.Green);

            Assert.IsTrue(s.ChartType == SeriesChartType.StackedColumn);
        }

        [TestMethod()]
        public void CreatePurdueSplitFailureAggregationChartTest()
        {
            MOE.CommonTests.Models.InMemoryMOEDatabase _db = new InMemoryMOEDatabase();
            _db.PopulateSignal();
            _db.PopulateSignalsWithApproaches();
            _db.PopulateApproachesWithDetectors();
            //int apprId = _db.PopulateApproachSplitFailAggregationsWithRandomRecords();

            MOE.CommonTests.Models.InMemoryApproachSplitFailAggregationRepository asfs = new InMemoryApproachSplitFailAggregationRepository(_db);




            MOE.Common.Models.Repositories.ApproachSplitFailAggregationRepositoryFactory.SetApplicationEventRepository(asfs);

            //Chart chart = ChartFactory.CreatePurdueSplitFailureAggregationChart(options);

            //Assert.IsNotNull(chart);

            string path = @"c:\SPMImages\testchart" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString()
                + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + ".jpeg";

            //chart.SaveImage(path);

            Assert.IsTrue(File.Exists(path));

        }

        [TestMethod()]
        public void CreateStringXIntYChartTest()
        {
            ApproachSplitFailAggregationOptions options = NewOptionsForTest(); ;

            Chart chart = ChartFactory.CreateStringXIntYChart(options);

           Assert.IsNotNull(chart);
            Assert.IsTrue(chart.Titles[0].Text.Contains("Aggregation"));

        }

        private ApproachSplitFailAggregationOptions NewOptionsForTest()
        {
            
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Approach;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 0, 8, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;

            return options;

        }
    }
}