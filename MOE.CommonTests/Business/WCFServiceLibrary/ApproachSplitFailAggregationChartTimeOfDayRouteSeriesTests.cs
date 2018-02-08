using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.WCFServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.Bins;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Models;

namespace MOE.Common.Business.WCFServiceLibrary.Tests
{
    [TestClass()]
    public class ApproachSplitFailAggregationChartTimeOfDayRouteSeriesTests
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
                foreach (var approach in signal.Approaches)
                {
                    Db.PopulateApproachSplitFailAggregationsWithRandomRecords(Convert.ToDateTime("1/1/2016"), Convert.ToDateTime("1/1/2018"), approach);
                }
            }
            ApproachSplitFailAggregationRepositoryFactory.SetApplicationEventRepository(new InMemoryApproachSplitFailAggregationRepository(Db));
            MOE.Common.Models.Repositories.SignalsRepositoryFactory.SetSignalsRepository(new InMemorySignalsRepository(Db));
            MetricTypeRepositoryFactory.SetMetricsRepository(new InMemoryMetricTypeRepository(Db));
            ApplicationEventRepositoryFactory.SetApplicationEventRepository(new InMemoryApplicationEventRepository(Db));
            Models.Repositories.DirectionTypeRepositoryFactory.SetDirectionsRepository(new InMemoryDirectionTypeRepository());
            SignalsRepository = SignalsRepositoryFactory.Create();
        }
        

        [TestMethod()]
        public void CreateTimeMetric15MinuteBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions(); options.SeriesWidth = 3; 
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/28/2017");
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.TimeOfDay;
            options.SelectedSeries = AggregationMetricOptions.SeriesType.Route;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/28/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("101"); options.SignalIds.Add("102"); options.SignalIds.Add("103"); options.SignalIds.Add("104"); options.SignalIds.Add("105");   
            options.SelectedChartType = SeriesChartType.Column;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.Line;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedArea;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 8);
        }

        [TestMethod()]
        public void CreateTimeMetric15MinuteBinTimePeriodTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions(); options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.TimeOfDay;
            options.SelectedSeries = AggregationMetricOptions.SeriesType.Route;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                6, 0, 10, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("101"); options.SignalIds.Add("102"); options.SignalIds.Add("103"); options.SignalIds.Add("104"); options.SignalIds.Add("105"); 
            options.SelectedChartType = SeriesChartType.Column;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.Line;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedArea;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 8);
        }

        [TestMethod()]
        public void CreateTimeMetric30MinuteBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions(); options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.TimeOfDay;
            options.SelectedSeries = AggregationMetricOptions.SeriesType.Route;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.ThirtyMinute,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("101"); options.SignalIds.Add("102"); options.SignalIds.Add("103"); options.SignalIds.Add("104"); options.SignalIds.Add("105"); 
            options.SelectedChartType = SeriesChartType.Column;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.Line;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedArea;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 8);
        }

        [TestMethod()]
        public void CreateTimeMetric30MinuteBinTimePeriodTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions(); options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.TimeOfDay;
            options.SelectedSeries = AggregationMetricOptions.SeriesType.Route;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                6, 0, 10, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSize.ThirtyMinute,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("101"); options.SignalIds.Add("102"); options.SignalIds.Add("103"); options.SignalIds.Add("104"); options.SignalIds.Add("105"); 
            options.SelectedChartType = SeriesChartType.Column;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.Line;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedArea;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 8);
        }

        [TestMethod()]
        public void CreateTimeMetricHourBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions(); options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.TimeOfDay;
            options.SelectedSeries = AggregationMetricOptions.SeriesType.Route;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("101"); options.SignalIds.Add("102"); options.SignalIds.Add("103"); options.SignalIds.Add("104"); options.SignalIds.Add("105"); 
            options.SelectedChartType = SeriesChartType.Column;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.Line;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedArea;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 8);
        }

        [TestMethod()]
        public void CreateTimeMetricHourBinTimePeriodTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions(); options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.TimeOfDay;
            options.SelectedSeries = AggregationMetricOptions.SeriesType.Route;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                6, 0, 10, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSize.ThirtyMinute,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("101"); options.SignalIds.Add("102"); options.SignalIds.Add("103"); options.SignalIds.Add("104"); options.SignalIds.Add("105"); 
            options.SelectedChartType = SeriesChartType.Column;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.Line;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedArea;
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 8);
        }


       
    }
}