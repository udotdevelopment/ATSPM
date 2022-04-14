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
using MOE.Common.Business.FilterExtensions;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.CommonTests.Models;
using MOE.Common.Models.Repositories;
using System.Xml.Linq;
using System.Xml;
using System.Web.UI.WebControls;

namespace MOE.Common.Business.Tests
{
    [TestClass()]
    public class ChartFactoryTests
    {
        public InMemoryMOEDatabase Db = new InMemoryMOEDatabase();
        public ISignalsRepository SignalsRepository;

        [TestInitialize]
        public void Initialize()
        {
            Db.ClearTables();
            Db.PopulateSignal();
            Db.PopulateSignalsWithApproaches();
            Db.PopulateApproachesWithDetectors();
            var signals = Db.Signals;
            foreach (var signal in signals)
            {
                Db.PopulatePreemptAggregations(Convert.ToDateTime("1/1/2016"), Convert.ToDateTime("1/1/2018"), signal.SignalID, signal.VersionID);

            }

            ApproachRepositoryFactory.SetApproachRepository(new InMemoryApproachRepository(Db));
            PreemptAggregationDatasRepositoryFactory.SetArchivedMetricsRepository(
                new InMemoryPreemptAggregationDatasRepository(Db));
            MOE.Common.Models.Repositories.SignalsRepositoryFactory.SetSignalsRepository(
                new InMemorySignalsRepository(Db));
            MetricTypeRepositoryFactory.SetMetricsRepository(new InMemoryMetricTypeRepository(Db));
            ApplicationEventRepositoryFactory.SetApplicationEventRepository(new InMemoryApplicationEventRepository(Db));
            DetectorEventCountAggregationRepositoryFactory.SetApplicationEventRepository(new InMemoryDetectorAggregationsRepository(Db));
            

            Models.Repositories.DirectionTypeRepositoryFactory.SetDirectionsRepository(
                new InMemoryDirectionTypeRepository());

            SignalsRepository = SignalsRepositoryFactory.Create();

            PreemptAggregationDatasRepositoryFactory.SetArchivedMetricsRepository(new InMemoryPreemptAggregationDatasRepository(Db));
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
            options.SelectedAggregationType = AggregationType.Sum;
            options.SelectedXAxisType = XAxisType.Approach;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 0, 8, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSize.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.FilterSignals.Add(new FilterSignal { SignalId = "7185", Exclude = false });
            options.FilterSignals.Add(new FilterSignal { SignalId = "5114", Exclude = false });
            options.SelectedChartType = SeriesChartType.Column;
            options.SelectedAggregatedDataType = new AggregatedDataType { DataName = "SplitFailures" };

            return options;

        }

        

        private Chart CreateATestChart()
        {
            Chart chart = new Chart();
            ChartArea cha1 = new ChartArea();
            
            Series s1 = new Series();
            Series s2 = new Series();
            s1.Name = "FirstSeries";
            s1.ChartType = SeriesChartType.Line;

            s2.Name = "SecondSeries";
            s2.ChartType = SeriesChartType.Column;

            AddPointsToTestSeries(s1);
            AddPointsToTestSeries(s2);




            s1.XAxisType = AxisType.Primary;
            s2.XAxisType = AxisType.Secondary;

            chart.Series.Add(s1);
            chart.Series.Add(s2);
            chart.ChartAreas.Add(cha1);


            return chart;

        }

        private void AddPointsToTestSeries(Series s)
        {
            for (int i = 1; i < 7; i++)
            {
                var dp = new DataPoint(i, i);
                s.Points.Add(dp);

            }
        }

        
    }
}