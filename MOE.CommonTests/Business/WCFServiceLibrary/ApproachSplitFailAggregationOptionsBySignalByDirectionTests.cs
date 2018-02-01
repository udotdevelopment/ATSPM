using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.WCFServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Business.Bins;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Models;

namespace MOE.Common.Business.WCFServiceLibrary.Tests
{
    [TestClass()]
    public class ApproachSplitFailAggregationSignalDirectionTests
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
                    Db.PopulateApproachSplitFailAggregationsWithRandomRecords(Convert.ToDateTime("1/1/2016"),
                        Convert.ToDateTime("1/1/2018"), approach);
                }
            }
            ApproachSplitFailAggregationRepositoryFactory.SetApplicationEventRepository(
                new InMemoryApproachSplitFailAggregationRepository(Db));
            MOE.Common.Models.Repositories.SignalsRepositoryFactory.SetSignalsRepository(
                new InMemorySignalsRepository(Db));
            MetricTypeRepositoryFactory.SetMetricsRepository(new InMemoryMetricTypeRepository(Db));
            ApplicationEventRepositoryFactory.SetApplicationEventRepository(
                new InMemoryApplicationEventRepository(Db));
            Models.Repositories.DirectionTypeRepositoryFactory.SetDirectionsRepository(
                new InMemoryDirectionTypeRepository());
            SignalsRepository = SignalsRepositoryFactory.Create();
        }



        //**************************************Direction********************************************************************
        //*************************************************************************************************************************

        [TestMethod()]
        public void CreateTimeMetric15MinuteBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.SignalByDirection;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("101"); options.SignalIds.Add("102"); options.SignalIds.Add("103"); options.SignalIds.Add("104"); options.SignalIds.Add("105");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Pie;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 10);
        }

        [TestMethod()]
        public void CreateTimeMetric15MinuteBinTimePeriodTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.SignalByDirection;
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
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("101"); options.SignalIds.Add("102"); options.SignalIds.Add("103"); options.SignalIds.Add("104"); options.SignalIds.Add("105");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Pie;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 10);
        }

        [TestMethod()]
        public void CreateTimeMetric30MinuteBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.SignalByDirection;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("101"); options.SignalIds.Add("102"); options.SignalIds.Add("103"); options.SignalIds.Add("104"); options.SignalIds.Add("105");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Pie;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 10);
        }

        [TestMethod()]
        public void CreateTimeMetric30MinuteBinTimePeriodTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.SignalByDirection;
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
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("101"); options.SignalIds.Add("102"); options.SignalIds.Add("103"); options.SignalIds.Add("104"); options.SignalIds.Add("105");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Pie;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 10);
        }

        [TestMethod()]
        public void CreateTimeMetricHourBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.SignalByDirection;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("101"); options.SignalIds.Add("102"); options.SignalIds.Add("103"); options.SignalIds.Add("104"); options.SignalIds.Add("105");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Pie;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 10);
        }

        [TestMethod()]
        public void CreateTimeMetricHourBinTimePeriodTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.SignalByDirection;
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
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("101"); options.SignalIds.Add("102"); options.SignalIds.Add("103"); options.SignalIds.Add("104"); options.SignalIds.Add("105");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Pie;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 10);
        }


        [TestMethod()]
        public void CreateTimeMetricDayBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("11/1/2017");
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.SignalByDirection;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("11/1/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("101"); options.SignalIds.Add("102"); options.SignalIds.Add("103"); options.SignalIds.Add("104"); options.SignalIds.Add("105");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Pie;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 10);
        }

        [TestMethod()]
        public void CreateTimeMetricDayBinTimePeriodTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("11/1/2017");
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.SignalByDirection;
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
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("101"); options.SignalIds.Add("102"); options.SignalIds.Add("103"); options.SignalIds.Add("104"); options.SignalIds.Add("105");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Pie;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 10);
        }

        [TestMethod()]
        public void CreateTimeMetricMonthBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("1/1/2018");
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.SignalByDirection;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("1/1/2018"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("101"); options.SignalIds.Add("102"); options.SignalIds.Add("103"); options.SignalIds.Add("104"); options.SignalIds.Add("105");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Pie;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 10);
        }

        [TestMethod()]
        public void CreateTimeMetricMonthBinTimePeriodTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("1/1/2018");
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.SignalByDirection;
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
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("101"); options.SignalIds.Add("102"); options.SignalIds.Add("103"); options.SignalIds.Add("104"); options.SignalIds.Add("105");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Pie;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 10);
        }

        [TestMethod()]
        public void CreateTimeMetricYearBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("1/1/2018");
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.SignalByDirection;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("1/1/2018"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("101"); options.SignalIds.Add("102"); options.SignalIds.Add("103"); options.SignalIds.Add("104"); options.SignalIds.Add("105");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Pie;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 10);
        }

        [TestMethod()]
        public void CreateTimeMetricYearBinTimePeriodTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("1/1/2018");
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.SignalByDirection;
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
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("101"); options.SignalIds.Add("102"); options.SignalIds.Add("103"); options.SignalIds.Add("104"); options.SignalIds.Add("105");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Pie;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.CreateMetric();
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count == 10);
        }

    }
}