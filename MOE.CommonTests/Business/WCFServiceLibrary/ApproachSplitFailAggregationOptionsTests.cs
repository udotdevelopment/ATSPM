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

        //[TestMethod()]
        //public void CreateMetricTest()
        //{


        //    ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();


        //    options.StartDate = DateTime.Now.AddDays(-1);//Convert.ToDateTime("10/17/2017");
        //    options.EndDate = DateTime.Now; //Convert.ToDateTime("10/18/2017");
        //    options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
        //    options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
        //    options.TimeOptions = new BinFactoryOptions(
        //        DateTime.Now.AddDays(-1),//Convert.ToDateTime("10/17/2017 7:00 AM"), 
        //        DateTime.Now,//Convert.ToDateTime("10/17/2017 8:00 AM"), 
        //        null, null, null, null, null, 
        //        BinFactoryOptions.BinSizes.FifteenMinutes, 
        //        BinFactoryOptions.TimeOptions.StartToEnd);
        //    options.SignalIds.Add("101");
        //    //options.SignalIds.Add("8279");
        //    //options.SignalIds.Add("7185");
        //    PopulateApproachesWithSplitFailtAggregationRecords(options);


        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Column;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Line;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
        //    Assert.IsTrue(options.CreateMetric().Count > 0);
        //}

        [TestMethod()]
        public void CreateColumnMetricMultipleSignalsStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.SignalByDirection;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
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

        //[TestMethod()]
        //public void CreateHourMetricTest()
        //{

        //    ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();


        //    options.StartDate = DateTime.Now.AddDays(-1);//Convert.ToDateTime("10/17/2017");
        //    options.EndDate = DateTime.Now; //Convert.ToDateTime("10/18/2017");
        //    options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
        //    options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
        //    options.TimeOptions = new BinFactoryOptions(
        //        DateTime.Today.AddHours(7),//Convert.ToDateTime("10/17/2017 7:00 AM"), 
        //        DateTime.Today.AddHours(8),//Convert.ToDateTime("10/17/2017 8:00 AM"), 
        //        null, null, null, null, null,
        //        BinFactoryOptions.BinSizes.Hour,
        //        BinFactoryOptions.TimeOptions.StartToEnd);

        //    options.SignalIds.Add("101");
        //    //options.SignalIds.Add("8279");
        //    //options.SignalIds.Add("7185");
        //    PopulateApproachesWithSplitFailtAggregationRecords(options);


        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Column;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Line;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
        //    Assert.IsTrue(options.CreateMetric().Count > 0);
        //}

        //[TestMethod()]
        //public void CreateDayMetricTest()
        //{

        //    ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();


        //    options.StartDate = DateTime.Now.AddDays(-7);//Convert.ToDateTime("10/17/2017");
        //    options.EndDate = DateTime.Now; //Convert.ToDateTime("10/18/2017");
        //    options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
        //    options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
        //    options.TimeOptions = new BinFactoryOptions(
        //        DateTime.Now.AddDays(-7),//Convert.ToDateTime("10/17/2017 7:00 AM"), 
        //        DateTime.Now,//Convert.ToDateTime("10/17/2017 8:00 AM"), 
        //        null, null, null, null, null,
        //        BinFactoryOptions.BinSizes.Day,
        //        BinFactoryOptions.TimeOptions.StartToEnd);

        //    options.SignalIds.Add("101");
        //    //options.SignalIds.Add("8279");
        //    //options.SignalIds.Add("7185");
        //    PopulateApproachesWithSplitFailtAggregationRecords(options);


        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Column;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Line;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
        //    Assert.IsTrue(options.CreateMetric().Count > 0);
        //}

        //[TestMethod()]
        //public void CreateWeekMetricTest()
        //{

        //    ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();


        //    options.StartDate = DateTime.Now.AddDays(-21);//Convert.ToDateTime("10/17/2017");
        //    options.EndDate = DateTime.Now; //Convert.ToDateTime("10/18/2017");
        //    options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
        //    options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
        //    options.TimeOptions = new BinFactoryOptions(
        //        DateTime.Now.AddDays(-21),//Convert.ToDateTime("10/17/2017 7:00 AM"), 
        //        DateTime.Now,//Convert.ToDateTime("10/17/2017 8:00 AM"), 
        //        null, null, null, null, null,
        //        BinFactoryOptions.BinSizes.Week,
        //        BinFactoryOptions.TimeOptions.StartToEnd);

        //    options.SignalIds.Add("101");
        //    options.SignalIds.Add("102");
        //    options.SignalIds.Add("103");
        //    PopulateApproachesWithSplitFailtAggregationRecords(options);




        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Column;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Line;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
        //    Assert.IsTrue(options.CreateMetric().Count > 0);
        //}

        //[TestMethod()]
        //public void CreateMonthMetricTest()
        //{


        //    ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();


        //    options.StartDate = DateTime.Now.AddMonths(-3);//Convert.ToDateTime("10/17/2017");
        //    options.EndDate = DateTime.Now; //Convert.ToDateTime("10/18/2017");
        //    options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
        //    options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
        //    options.TimeOptions = new BinFactoryOptions(
        //        DateTime.Now.AddMonths(-3),//Convert.ToDateTime("10/17/2017 7:00 AM"), 
        //        DateTime.Now,//Convert.ToDateTime("10/17/2017 8:00 AM"), 
        //        null, null, null, null, null,
        //        BinFactoryOptions.BinSizes.Month,
        //        BinFactoryOptions.TimeOptions.StartToEnd);

        //    options.SignalIds.Add("101");
        //    //options.SignalIds.Add("8279");
        //    //options.SignalIds.Add("7185");
        //    PopulateApproachesWithSplitFailtAggregationRecords(options);


        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Column;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Line;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
        //    Assert.IsTrue(options.CreateMetric().Count > 0);
        //}



        //[TestMethod()]
        //public void CreateYearMetricTest()
        //{
        //    ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();


        //    options.StartDate = DateTime.Now.AddYears(-3);//Convert.ToDateTime("10/17/2017");
        //    options.EndDate = DateTime.Now; //Convert.ToDateTime("10/18/2017");
        //    options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
        //    options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
        //    options.TimeOptions = new BinFactoryOptions(
        //        DateTime.Now.AddYears(-3),//Convert.ToDateTime("10/17/2017 7:00 AM"), 
        //        DateTime.Now,//Convert.ToDateTime("10/17/2017 8:00 AM"), 
        //        null, null, null, null, null,
        //        BinFactoryOptions.BinSizes.Year,
        //        BinFactoryOptions.TimeOptions.StartToEnd);

        //    options.SignalIds.Add("101");
        //    //options.SignalIds.Add("8279");
        //    //options.SignalIds.Add("7185");
        //    PopulateApproachesWithSplitFailtAggregationRecords(options);

        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Column;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Line;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
        //    Assert.IsTrue(options.CreateMetric().Count > 0);
        //}

        //[TestMethod()]
        //public void YearOfWednesdaysTest()
        //{
        //    List<DayOfWeek> daysofWeek = new List<DayOfWeek>();
        //    daysofWeek.Add(DayOfWeek.Wednesday);
        //    ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();


        //    options.StartDate = DateTime.Now.AddYears(-3);//Convert.ToDateTime("10/17/2017");
        //    options.EndDate = DateTime.Now; //Convert.ToDateTime("10/18/2017");
        //    options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
        //    options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
        //    options.TimeOptions = new BinFactoryOptions(
        //        DateTime.Now.AddYears(-3),//Convert.ToDateTime("10/17/2017 7:00 AM"), 
        //        DateTime.Now,//Convert.ToDateTime("10/17/2017 8:00 AM"), 
        //        null, null, null, null, daysofWeek,
        //        BinFactoryOptions.BinSizes.Year,
        //        BinFactoryOptions.TimeOptions.TimePeriod);

        //    options.SignalIds.Add("101");
        //    //options.SignalIds.Add("8279");
        //    //options.SignalIds.Add("7185");
        //    PopulateApproachesWithSplitFailtAggregationRecords(options);
        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Column;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Line;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
        //    Assert.IsTrue(options.CreateMetric().Count > 0);
        //}

        //[TestMethod()]
        //public void YearOfWednesdaysAndFullYearProduceDifferentResultsTest()
        //{
        //    List<DayOfWeek> daysofWeek = new List<DayOfWeek>();
        //    daysofWeek.Add(DayOfWeek.Wednesday);
        //    ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();


        //    options.StartDate = DateTime.Now.AddYears(-3);//Convert.ToDateTime("10/17/2017");
        //    options.EndDate = DateTime.Now; //Convert.ToDateTime("10/18/2017");
        //    options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
        //    options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
        //    options.TimeOptions = new BinFactoryOptions(
        //        DateTime.Now.AddYears(-3),//Convert.ToDateTime("10/17/2017 7:00 AM"), 
        //        DateTime.Now,//Convert.ToDateTime("10/17/2017 8:00 AM"), 
        //        null, null, null, null, daysofWeek,
        //        BinFactoryOptions.BinSizes.Year,
        //        BinFactoryOptions.TimeOptions.StartToEnd);

        //    options.SignalIds.Add("101");
        //    //options.SignalIds.Add("8279");
        //    //options.SignalIds.Add("7185");
        //    PopulateApproachesWithSplitFailtAggregationRecords(options);
        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Column;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Line;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;

        //    options.TimeOptions = new BinFactoryOptions(
        //        DateTime.Now.AddYears(-3),//Convert.ToDateTime("10/17/2017 7:00 AM"), 
        //        DateTime.Now,//Convert.ToDateTime("10/17/2017 8:00 AM"), 
        //        null, null, null, null, null,
        //        BinFactoryOptions.BinSizes.Year,
        //        BinFactoryOptions.TimeOptions.StartToEnd);


        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Column;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Line;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;



        //    Assert.IsTrue(options.CreateMetric().Count > 0);
        //}

        //[TestMethod()]
        //public void CreateSignalMetricTest()
        //{
        //    ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();


        //    options.StartDate = DateTime.Now.AddMonths(-1);
        //    options.EndDate = DateTime.Now;
        //    options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
        //    options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
        //    options.TimeOptions = new BinFactoryOptions(
        //        DateTime.Now.AddMonths(-1),
        //        DateTime.Now,
        //        null, null, null, null, null,
        //        BinFactoryOptions.BinSizes.Day,
        //        BinFactoryOptions.TimeOptions.StartToEnd);

        //    options.SignalIds.Add("101");
        //    options.SignalIds.Add("102");
        //    options.SignalIds.Add("103");
        //    PopulateApproachesWithSplitFailtAggregationRecords(options);

        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Column;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Line;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
        //    Assert.IsTrue(options.CreateMetric().Count > 0);
        //}

        //[TestMethod]
        //public void CreateSignalByDirectionMetricTest()
        //{
        //    ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();


        //    options.StartDate = DateTime.Now.AddMonths(-1);
        //    options.EndDate = DateTime.Now;
        //    options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
        //    options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.SignalByDirection;
        //    options.TimeOptions = new BinFactoryOptions(
        //        DateTime.Now.AddMonths(-1),
        //        DateTime.Now,
        //        null, null, null, null, null,
        //        BinFactoryOptions.BinSizes.Day,
        //        BinFactoryOptions.TimeOptions.StartToEnd);

        //    options.SignalIds.Add("101");
        //    options.SignalIds.Add("102");
        //    options.SignalIds.Add("103");
        //    PopulateApproachesWithSplitFailtAggregationRecords(options);

        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Column;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.Line;
        //    options.CreateMetric();
        //    options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
        //    Assert.IsTrue(options.CreateMetric().Count > 0);
        //}




    }
}