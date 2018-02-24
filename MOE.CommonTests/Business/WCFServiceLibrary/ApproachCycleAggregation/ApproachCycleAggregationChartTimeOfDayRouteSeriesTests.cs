using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.WCFServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.Bins;
using MOE.Common.Business.FilterExtensions;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Models;

namespace MOE.Common.Business.WCFServiceLibrary.Tests
{
    [TestClass()]
    public class ApproachCycleAggregationChartTimeOfDayRouteSeriesTests
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
                    Db.PopulateApproachCycleAggregationsWithRandomRecords(Convert.ToDateTime("1/1/2016"), Convert.ToDateTime("1/1/2018"), approach);
                }
            }
            ApproachCycleAggregationRepositoryFactory.SetApplicationEventRepository(new InMemoryApproachCycleAggregationRepository(Db));
            MOE.Common.Models.Repositories.SignalsRepositoryFactory.SetSignalsRepository(new InMemorySignalsRepository(Db));
            MetricTypeRepositoryFactory.SetMetricsRepository(new InMemoryMetricTypeRepository(Db));
            ApplicationEventRepositoryFactory.SetApplicationEventRepository(new InMemoryApplicationEventRepository(Db));
            Models.Repositories.DirectionTypeRepositoryFactory.SetDirectionsRepository(new InMemoryDirectionTypeRepository());
            SignalsRepository = SignalsRepositoryFactory.Create();
        }
        

        [TestMethod()]
        public void CreateTimeMetric15MinuteBinStartToFinishTest()
        {
            ApproachCycleAggregationOptions options = new ApproachCycleAggregationOptions(); options.SeriesWidth = 3; 
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/28/2017");
            options.SelectedXAxisType = XAxisType.TimeOfDay;
            options.SelectedSeries = SeriesType.Route;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/28/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.FilterSignals.Add(new FilterSignal{SignalId = "101", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "102", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "103", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "104", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "105", Exclude = false});   
            options.SelectedChartType = SeriesChartType.Column;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.Line;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedArea;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 8);
        }

        [TestMethod()]
        public void CreateTimeMetric15MinuteBinTimePeriodTest()
        {
            ApproachCycleAggregationOptions options = new ApproachCycleAggregationOptions(); options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.SelectedXAxisType = XAxisType.TimeOfDay;
            options.SelectedSeries = SeriesType.Route;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                6, 0, 10, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.FilterSignals.Add(new FilterSignal{SignalId = "101", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "102", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "103", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "104", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "105", Exclude = false}); 
            options.SelectedChartType = SeriesChartType.Column;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.Line;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedArea;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 8);
        }

        [TestMethod()]
        public void CreateTimeMetric30MinuteBinStartToFinishTest()
        {
            ApproachCycleAggregationOptions options = new ApproachCycleAggregationOptions(); options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedXAxisType = XAxisType.TimeOfDay;
            options.SelectedSeries = SeriesType.Route;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.ThirtyMinute,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.FilterSignals.Add(new FilterSignal{SignalId = "101", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "102", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "103", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "104", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "105", Exclude = false}); 
            options.SelectedChartType = SeriesChartType.Column;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.Line;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedArea;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 8);
        }

        [TestMethod()]
        public void CreateTimeMetric30MinuteBinTimePeriodTest()
        {
            ApproachCycleAggregationOptions options = new ApproachCycleAggregationOptions(); options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedXAxisType = XAxisType.TimeOfDay;
            options.SelectedSeries = SeriesType.Route;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                6, 0, 10, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSize.ThirtyMinute,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.FilterSignals.Add(new FilterSignal{SignalId = "101", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "102", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "103", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "104", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "105", Exclude = false}); 
            options.SelectedChartType = SeriesChartType.Column;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.Line;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedArea;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 8);
        }

        [TestMethod()]
        public void CreateTimeMetricHourBinStartToFinishTest()
        {
            ApproachCycleAggregationOptions options = new ApproachCycleAggregationOptions(); options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedXAxisType = XAxisType.TimeOfDay;
            options.SelectedSeries = SeriesType.Route;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.FilterSignals.Add(new FilterSignal{SignalId = "101", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "102", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "103", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "104", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "105", Exclude = false}); 
            options.SelectedChartType = SeriesChartType.Column;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.Line;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedArea;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 8);
        }

        [TestMethod()]
        public void CreateTimeMetricHourBinTimePeriodTest()
        {
            ApproachCycleAggregationOptions options = new ApproachCycleAggregationOptions(); options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedXAxisType = XAxisType.TimeOfDay;
            options.SelectedSeries = SeriesType.Route;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                6, 0, 10, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSize.ThirtyMinute,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.FilterSignals.Add(new FilterSignal{SignalId = "101", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "102", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "103", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "104", Exclude = false}); options.FilterSignals.Add(new FilterSignal{SignalId = "105", Exclude = false}); 
            options.SelectedChartType = SeriesChartType.Column;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.Line;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedArea;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 8);
        }


       
    }
}