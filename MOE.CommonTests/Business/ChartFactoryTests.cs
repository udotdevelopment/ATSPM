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
            int apprId = _db.PopulateApproachSplitFailAggregationsWithRandomRecords();

            MOE.CommonTests.Models.InMemoryApproachSplitFailAggregationRepository asfs = new InMemoryApproachSplitFailAggregationRepository(_db);
            

           // var options = new AggregationMetricOptions();

            //options.BinSize = 15;
            //options.StartDate = DateTime.Now.AddDays(-1);
            //options.EndDate = DateTime.Now;
            //options.Approaches.Add((from a in _db.Approaches where a.ApproachID == apprId select a).FirstOrDefault());
            //options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            //options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Approach;
            //options.GroupBy = AggregationMetricOptions.XAxisTimeTypes.Hour;

            MOE.Common.Models.Repositories.ApproachSplitFailAggregationRepositoryFactory.SetApplicationEventRepository(asfs);

            //Chart chart = ChartFactory.CreatePurdueSplitFailureAggregationChart(options);

            //Assert.IsNotNull(chart);

            string path = @"c:\SPMImages\testchart" + DateTime.Now.Month.ToString() +"_"+ DateTime.Now.Day.ToString() 
                + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + ".jpeg";

            //chart.SaveImage(path);

           Assert.IsTrue( File.Exists(path));

        }
    }
}