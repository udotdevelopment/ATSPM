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
    public class ApproachSplitFailAggregationSignalOptionsTests
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

      

        //**************************************Signal Aggregation********************************************************************
        //*************************************************************************************************************************

        [TestMethod()]
        public void CreateApproachColumnMetricMultipleSignalsStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 0, 8, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0); 
             
        }

        [TestMethod()]
        public void CreateApproachColumnMetricMultipleApproachesStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod()]
        public void CreateApproachLineMetricStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachColumnMetricStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachStackedColumnMetricStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricAreaStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricLineHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricColumnHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricStackedColumnHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricAreaHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }


        //Sixty Minute Bin tests

        [TestMethod()]
        public void CreateApproachMetricLine60MinuteBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachColumnMetric60MinuteBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachStackedColumnMetric60MinuteBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricArea60MinuteBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricLine60MinuteBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricColumn60MinuteBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricStackedColumn60MinuteBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricArea60MinuteBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        //Day Bin tests

        [TestMethod()]
        public void CreateApproachMetricLineDayBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachColumnMetricDayBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachStackedColumnMetricDayBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricAreaDayBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricLineDayBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricColumnDayBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricStackedColumnDayBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricAreaDayBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        //Month Bin tests

        [TestMethod()]
        public void CreateApproachMetricLineMonthBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachColumnMetricMonthBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachStackedColumnMetricMonthBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricAreaMonthBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricLineMonthBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricColumnMonthBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricStackedColumnMonthBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricAreaMonthBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricAreaMonthBinSingleDayType()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Wednesday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricStackedColumnMonthBinSingleDayType()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Wednesday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        //Month Bin tests

        [TestMethod()]
        public void CreateApproachMetricLineYearBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachColumnMetricYearBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachStackedColumnMetricYearBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricAreaYearBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricLineYearBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday },
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricColumnYearBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday },
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricStackedColumnYearBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday },
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricAreaYearBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday },
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }



        //Thirty Minute Bin tests

        [TestMethod()]
        public void CreateApproachMetricLine30MinuteBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachColumnMetric30MinuteBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachStackedColumnMetric30MinuteBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricArea30MinuteBinStartToFinishTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricLine30MinuteBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricColumn30MinuteBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricStackedColumn30MinuteBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricArea30MinuteBinHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }


        //*****************Average Tests****************************


        [TestMethod()]
        public void CreateApproachColumnMetricMultipleApproachesStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod()]
        public void CreateApproachLineMetricStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachColumnMetricStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachStackedColumnMetricStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricAreaStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricLineHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricColumnHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricStackedColumnHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricAreaHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }


        //Sixty Minute Bin tests

        [TestMethod()]
        public void CreateApproachMetricLine60MinuteBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachColumnMetric60MinuteBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachStackedColumnMetric60MinuteBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricArea60MinuteBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricLine60MinuteBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricColumn60MinuteBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricStackedColumn60MinuteBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricArea60MinuteBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        //Day Bin tests

        [TestMethod()]
        public void CreateApproachMetricLineDayBinStartToFinishTestAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachColumnMetricDayBinStartToFinishTestAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachStackedColumnMetricDayBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricAreaDayBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricLineDayBinHourAverages()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricColumnDayBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricStackedColumnDayBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricAreaDayBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("10/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("10/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        //Month Bin tests

        [TestMethod()]
        public void CreateApproachMetricLineMonthBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachColumnMetricMonthBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachStackedColumnMetricMonthBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricAreaMonthBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricLineMonthBinAverageHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricColumnMonthBinAverageHours()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricStackedColumnMonthBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricAreaMonthBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricAreaMonthBinSingleDayTypeAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Wednesday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricStackedColumnMonthBinSingleDayTypeAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Wednesday },
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        //Month Bin tests

        [TestMethod()]
        public void CreateApproachMetricLineYearBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachColumnMetricYearBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachStackedColumnMetricYearBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricAreaYearBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricLineYearBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday },
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricColumnYearBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday },
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricStackedColumnYearBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday },
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricAreaYearBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("12/31/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday },
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }



        //Thirty Minute Bin tests

        [TestMethod()]
        public void CreateApproachMetricLine30MinuteBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachColumnMetric30MinuteBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachStackedColumnMetric30MinuteBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricArea30MinuteBinStartToFinishAverageTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricLine30MinuteBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricColumn30MinuteBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }

        [TestMethod]
        public void CreateApproachMetricStackedColumn30MinuteBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
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
        public void CreateApproachMetricArea30MinuteBinHoursAverage()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOperation = AggregationMetricOptions.AggregationOperations.Average;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 30, 9, 30, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("7185");
            options.SignalIds.Add("5114");
            options.SignalIds.Add("1010");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
           Assert.IsTrue(options.CreateMetric().Count > 0);  
        }





    }
}