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
    public class PreemptAggregationApproachXDetectorSeriesTests
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
            DetectorAggregationsRepositoryFactory.SetDetectorAggregationRepository(new InMemoryDetectorAggregationsRepository(Db));

            Models.Repositories.DirectionTypeRepositoryFactory.SetDirectionsRepository(
                new InMemoryDirectionTypeRepository());

            SignalsRepository = SignalsRepositoryFactory.Create();

           PreemptAggregationDatasRepositoryFactory.SetArchivedMetricsRepository(new InMemoryPreemptAggregationDatasRepository(Db));
        }

       

        [TestMethod()]
        public void CreateTimeXPreemptSeries()
        {
            SignalPreemptionAggregationOptions  options = new SignalPreemptionAggregationOptions();
            options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedXAxisType = XAxisType.Time;
            options.SelectedSeries = SeriesType.Signal;
            options.SelectedAggregatedDataType = new AggregatedDataType { Id = 0, DataName =  "PreemptRequests" };
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.StartToEnd);
            SetFilterSignal(options);
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType = AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 2);
        }

        private void SetFilterSignal(SignalPreemptionAggregationOptions options)
        {
            List<FilterSignal> filterSignals = new List<FilterSignal>();
            var signal = Db.Signals.FirstOrDefault();
            var filterSignal = new FilterSignal { SignalId = signal.SignalID, Exclude = false };
            foreach (var approach in signal.Approaches)
            {
                var filterApproach = new FilterApproach
                {
                    ApproachId = approach.ApproachID,
                    Description = String.Empty,
                    Exclude = false
                };
                filterSignal.FilterApproaches.Add(filterApproach);
                foreach (var detector in approach.Detectors)
                {
                    filterApproach.FilterDetectors.Add(new FilterDetector { Id = detector.ID, Description = String.Empty, Exclude = false });
                }
            }
            options.FilterSignals.Add(filterSignal);
            options.FilterDirections = new List<FilterDirection>();
            options.FilterDirections.Add(new FilterDirection { Description = "", DirectionTypeId = 0, Include = true });
            options.FilterDirections.Add(new FilterDirection { Description = "", DirectionTypeId = 1, Include = true });
            options.FilterDirections.Add(new FilterDirection { Description = "", DirectionTypeId = 2, Include = true });
            options.FilterDirections.Add(new FilterDirection { Description = "", DirectionTypeId = 3, Include = true });
            options.FilterMovements = new List<FilterMovement>();
            options.FilterMovements.Add(new FilterMovement { Description = "", MovementTypeId = 0, Include = true });
            options.FilterMovements.Add(new FilterMovement { Description = "", MovementTypeId = 1, Include = true });
            options.FilterMovements.Add(new FilterMovement { Description = "", MovementTypeId = 2, Include = true });
            options.FilterMovements.Add(new FilterMovement { Description = "", MovementTypeId = 3, Include = true });
        }

        [TestMethod()]
        public void CreateTimeXAxisPhaseNumberSeries()
        {
            SignalPreemptionAggregationOptions options = new SignalPreemptionAggregationOptions();
            options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedXAxisType = XAxisType.Time;
            options.SelectedSeries = SeriesType.Signal;
            options.SelectedAggregatedDataType = new AggregatedDataType { Id = 0, DataName =  "PreemptRequests" };
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                6, 0, 10, 0,
                new List<DayOfWeek>
                {
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
                    DayOfWeek.Saturday,
                    DayOfWeek.Sunday
                },
                BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.TimePeriod);
            SetFilterSignal(options);
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 2);
        }

        [TestMethod()]
        public void CreateTimeXAxisSignalSeries()
        {
            SignalPreemptionAggregationOptions options = new SignalPreemptionAggregationOptions(); options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedXAxisType = XAxisType.Time;
            options.SelectedSeries = SeriesType.Signal;
            options.SelectedAggregatedDataType = new AggregatedDataType { Id = 0, DataName = "PreemptRequests" };
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.ThirtyMinute,
                BinFactoryOptions.TimeOptions.StartToEnd);
            SetFilterSignal(options);
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 2);
        }

        [TestMethod()]
        public void CreateTimeMetric30MinuteBinTimePeriodTest()
        {
            SignalPreemptionAggregationOptions options = new SignalPreemptionAggregationOptions(); options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedXAxisType = XAxisType.Time;
            options.SelectedSeries = SeriesType.Signal;
            options.SelectedAggregatedDataType = new AggregatedDataType { Id = 0, DataName =  "PreemptRequests" };
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                6, 0, 10, 0,
                new List<DayOfWeek>
                {
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
                    DayOfWeek.Saturday,
                    DayOfWeek.Sunday
                },
                BinFactoryOptions.BinSize.ThirtyMinute,
                BinFactoryOptions.TimeOptions.TimePeriod);
            SetFilterSignal(options);
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 2);
        }

        [TestMethod()]
        public void CreateTimeMetricHourBinStartToFinishTest()
        {
            SignalPreemptionAggregationOptions options = new SignalPreemptionAggregationOptions(); options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedXAxisType = XAxisType.Time;
            options.SelectedSeries = SeriesType.Signal;
            options.SelectedAggregatedDataType = new AggregatedDataType { Id = 0, DataName =  "PreemptRequests" };
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            SetFilterSignal(options);
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 2);
        }

        [TestMethod()]
        public void CreateTimeMetricHourBinTimePeriodTest()
        {
            SignalPreemptionAggregationOptions options = new SignalPreemptionAggregationOptions(); options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedXAxisType = XAxisType.Time;
            options.SelectedSeries = SeriesType.Signal;
            options.SelectedAggregatedDataType = new AggregatedDataType { Id = 0, DataName = "PreemptRequests" };
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                6, 0, 10, 0,
                new List<DayOfWeek>
                {
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
                    DayOfWeek.Saturday,
                    DayOfWeek.Sunday
                },
                BinFactoryOptions.BinSize.ThirtyMinute,
                BinFactoryOptions.TimeOptions.TimePeriod);
            SetFilterSignal(options);
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 2);
        }


        [TestMethod()]
        public void CreateTimeMetricDayBinStartToFinishTest()
        {
            SignalPreemptionAggregationOptions options = new SignalPreemptionAggregationOptions(); options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("11/1/2017");
            options.SelectedXAxisType = XAxisType.Time;
            options.SelectedSeries = SeriesType.Signal;
            options.SelectedAggregatedDataType = new AggregatedDataType { Id = 0, DataName =  "PreemptRequests" };
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("11/1/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);
            SetFilterSignal(options);
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 2);
        }

        [TestMethod()]
        public void CreateTimeMetricDayBinTimePeriodTest()
        {
            SignalPreemptionAggregationOptions options = new SignalPreemptionAggregationOptions(); options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("11/1/2017");
            options.SelectedXAxisType = XAxisType.Time;
            options.SelectedSeries = SeriesType.Signal;
            options.SelectedAggregatedDataType = new AggregatedDataType { Id = 0, DataName =  "PreemptRequests" };
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("11/1/2017"),
                6, 0, 10, 0,
                new List<DayOfWeek>
                {
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
                    DayOfWeek.Saturday,
                    DayOfWeek.Sunday
                },
                BinFactoryOptions.BinSize.Day,
                BinFactoryOptions.TimeOptions.TimePeriod);
            SetFilterSignal(options);
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 2);
        }

        [TestMethod()]
        public void CreateTimeMetricMonthBinStartToFinishTest()
        {
            SignalPreemptionAggregationOptions options = new SignalPreemptionAggregationOptions(); options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("1/1/2018");
            options.SelectedXAxisType = XAxisType.Time;
            options.SelectedSeries = SeriesType.Signal;
            options.SelectedAggregatedDataType = new AggregatedDataType { Id = 0, DataName =  "PreemptRequests" };
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("1/1/2018"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);
            SetFilterSignal(options);
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 2);
        }

        [TestMethod()]
        public void CreateTimeMetricMonthBinTimePeriodTest()
        {
            SignalPreemptionAggregationOptions options = new SignalPreemptionAggregationOptions();
            options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("1/1/2018");
            options.SelectedXAxisType = XAxisType.Time;
            options.SelectedSeries = SeriesType.Signal;
            options.SelectedAggregatedDataType = new AggregatedDataType { Id = 0, DataName =  "PreemptRequests" };
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("1/1/2018"),
                6, 0, 10, 0,
                new List<DayOfWeek>
                {
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
                    DayOfWeek.Saturday,
                    DayOfWeek.Sunday
                },
                BinFactoryOptions.BinSize.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            SetFilterSignal(options);
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 2);
        }

        [TestMethod()]
        public void CreateTimeMetricYearBinStartToFinishTest()
        {
            SignalPreemptionAggregationOptions options = new SignalPreemptionAggregationOptions();
            options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("1/1/2018");
            options.SelectedXAxisType = XAxisType.Time;
            options.SelectedSeries = SeriesType.Signal;
            options.SelectedAggregatedDataType = new AggregatedDataType { Id = 0, DataName =  "PreemptRequests" };
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("1/1/2018"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);
            SetFilterSignal(options);
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 2);
        }

        [TestMethod()]
        public void CreateTimeMetricYearBinTimePeriodTest()
        {
            SignalPreemptionAggregationOptions options = new SignalPreemptionAggregationOptions(); options.SeriesWidth = 3;
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("1/1/2018");
            options.SelectedXAxisType = XAxisType.Time;
            options.SelectedSeries = SeriesType.Signal;
            options.SelectedAggregatedDataType = new AggregatedDataType { Id = 0, DataName =  "PreemptRequests" };
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("1/1/2018"),
                6, 0, 10, 0,
                new List<DayOfWeek>
                {
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
                    DayOfWeek.Saturday,
                    DayOfWeek.Sunday
                },
                BinFactoryOptions.BinSize.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);
            SetFilterSignal(options);
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType =AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType =AggregationType.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 2);
        }
    }
}

