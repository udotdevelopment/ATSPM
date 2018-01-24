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
    public class ApproachSplitFailAggregationOptionsTests
    {
        //public InMemoryMOEDatabase Db = new InMemoryMOEDatabase();

        [TestInitialize]
        public void Initialize()
        {
            
            //Db.ClearTables();
            //Db.PopulateSignal();
            //Db.PopulateSignalsWithApproaches();
            //Db.PopulateApproachesWithDetectors();
            //ApproachSplitFailAggregationRepositoryFactory.SetApplicationEventRepository(new InMemoryApproachSplitFailAggregationRepository(Db));
            //MOE.Common.Models.Repositories.SignalsRepositoryFactory.SetSignalsRepository(new InMemorySignalsRepository(Db));
            //MetricTypeRepositoryFactory.SetMetricsRepository(new InMemoryMetricTypeRepository(Db));
            //ApplicationEventRepositoryFactory.SetApplicationEventRepository(new InMemoryApplicationEventRepository(Db));
            //Models.Repositories.DirectionTypeRepositoryFactory.SetDirectionsRepository(new InMemoryDirectionTypeRepository());
        }

        private void PopulateApproachesWithSplitFailtAggregationRecords(ApproachSplitFailAggregationOptions options)
        {
            //foreach (var signalId in options.SignalIds)
            //{
            //    var approaches = from r in Db.Approaches
            //        where r.SignalID == signalId
            //        select r;

            //    foreach (var appr in approaches)
            //    {
            //        Db.PopulateApproachSplitFailAggregations(options.StartDate, options.EndDate, appr.ApproachID);
            //    }
            //}
        }

        

        [TestMethod()]
        public void CreateColumnMetricMultipleSignalsStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 0, 8, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("8279");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod()]
        public void CreateColumnMetricMultipleApproachesStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Approach;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod()]
        public void CreateLineMetricStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"), 
                Convert.ToDateTime("10/18/2017"), 
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateColumnMetricStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateStackedColumnMetricStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricAreaStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricLineHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricColumnHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricStackedColumnHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricAreaHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }


        //Sixty Minute Bin tests

        [TestMethod()]
        public void CreateMetricLine60MinuteBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateColumnMetric60MinuteBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateStackedColumnMetric60MinuteBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricArea60MinuteBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricLine60MinuteBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricColumn60MinuteBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricStackedColumn60MinuteBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricArea60MinuteBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        //Day Bin tests

        [TestMethod()]
        public void CreateMetricLineDayBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateColumnMetricDayBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateStackedColumnMetricDayBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricAreaDayBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricLineDayBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricColumnDayBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricStackedColumnDayBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricAreaDayBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        //Month Bin tests

        [TestMethod()]
        public void CreateMetricLineMonthBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateColumnMetricMonthBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateStackedColumnMetricMonthBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricAreaMonthBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricLineMonthBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricColumnMonthBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricStackedColumnMonthBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricAreaMonthBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricAreaMonthBinSingleDayType()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Wednesday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricStackedColumnMonthBinSingleDayType()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Wednesday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        //Month Bin tests

        [TestMethod()]
        public void CreateMetricLineYearBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateColumnMetricYearBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateStackedColumnMetricYearBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricAreaYearBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricLineYearBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday},
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricColumnYearBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday},
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricStackedColumnYearBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday},
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricAreaYearBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday},
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        

        //Thirty Minute Bin tests

        [TestMethod()]
        public void CreateMetricLine30MinuteBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateColumnMetric30MinuteBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateStackedColumnMetric30MinuteBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricArea30MinuteBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricLine30MinuteBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricColumn30MinuteBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricStackedColumn30MinuteBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricArea30MinuteBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }


        //*****************Average Tests****************************


        [TestMethod()]
        public void CreateColumnMetricMultipleApproachesStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Approach;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod()]
        public void CreateLineMetricStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateColumnMetricStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateStackedColumnMetricStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricAreaStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricLineHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricColumnHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricStackedColumnHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricAreaHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }


        //Sixty Minute Bin tests

        [TestMethod()]
        public void CreateMetricLine60MinuteBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateColumnMetric60MinuteBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateStackedColumnMetric60MinuteBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricArea60MinuteBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricLine60MinuteBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricColumn60MinuteBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricStackedColumn60MinuteBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricArea60MinuteBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        //Day Bin tests

        [TestMethod()]
        public void CreateMetricLineDayBinStartToFinishTestAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateColumnMetricDayBinStartToFinishTestAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateStackedColumnMetricDayBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricAreaDayBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricLineDayBinHourAverages()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricColumnDayBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricStackedColumnDayBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricAreaDayBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        //Month Bin tests

        [TestMethod()]
        public void CreateMetricLineMonthBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateColumnMetricMonthBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateStackedColumnMetricMonthBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricAreaMonthBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricLineMonthBinAverageHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricColumnMonthBinAverageHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricStackedColumnMonthBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricAreaMonthBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricAreaMonthBinSingleDayTypeAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Wednesday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricStackedColumnMonthBinSingleDayTypeAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Wednesday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        //Month Bin tests

        [TestMethod()]
        public void CreateMetricLineYearBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateColumnMetricYearBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateStackedColumnMetricYearBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricAreaYearBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricLineYearBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday },
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricColumnYearBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday },
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricStackedColumnYearBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday },
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricAreaYearBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday },
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("5114");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }



        //Thirty Minute Bin tests

        [TestMethod()]
        public void CreateMetricLine30MinuteBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateColumnMetric30MinuteBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateStackedColumnMetric30MinuteBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricArea30MinuteBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricLine30MinuteBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricColumn30MinuteBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricStackedColumn30MinuteBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateMetricArea30MinuteBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }


    }
}