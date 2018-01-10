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
        public InMemoryMOEDatabase Db = new InMemoryMOEDatabase();

        [TestInitialize]
        public void Initialize()
        {
            
            Db.ClearTables();
            Db.PopulateSignal();
            Db.PopulateSignalsWithApproaches();
            Db.PopulateApproachesWithDetectors();

        }
        [TestMethod()]
        public void CreateMetricTest()
        {
            ApproachSplitFailAggregationRepositoryFactory.SetApplicationEventRepository(new InMemoryApproachSplitFailAggregationRepository(Db));

            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            

            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017 7:00 AM"), 
                Convert.ToDateTime("10/17/2017 8:00 AM"), null, null, null, null, null, 
                BinFactoryOptions.BinSizes.FifteenMinutes, 
                BinFactoryOptions.TimeOptions.StartToEnd);
            
            //options.SignalIds.Add("8279");
            //options.SignalIds.Add("7185");
            options.SignalIds.Add("5605");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod()]
        public void CreateHourMetricTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017 7:00 AM"),
                Convert.ToDateTime("10/17/2017 8:00 AM"), null, null, null, null, null,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);

            //options.SignalIds.Add("8279");
            //options.SignalIds.Add("7185");
            options.SignalIds.Add("5605");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod()]
        public void CreateDayMetricTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/01/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/01/2017 7:00 AM"),
                Convert.ToDateTime("10/17/2017 7:00 AM"), null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);

            //options.SignalIds.Add("8279");
            //options.SignalIds.Add("7185");
            options.SignalIds.Add("5605");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod()]
        public void CreateWeekMetricTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/01/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/01/2017 7:00 AM"),
                Convert.ToDateTime("10/17/2017 7:00 AM"), null, null, null, null, null,
                BinFactoryOptions.BinSizes.Week,
                BinFactoryOptions.TimeOptions.StartToEnd);

            //options.SignalIds.Add("8279");
            //options.SignalIds.Add("7185");
            options.SignalIds.Add("5605");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod()]
        public void CreateMonthMetricTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("8/01/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("8/01/2017 7:00 AM"),
                Convert.ToDateTime("10/17/2017 7:00 AM"), null, null, null, null, null,
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);

            //options.SignalIds.Add("8279");
            //options.SignalIds.Add("7185");
            options.SignalIds.Add("5605");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }
        [TestMethod()]
        public void CreateYearMetricTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("01/01/2014");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("01/01/2014 00:01 AM"),
                Convert.ToDateTime("12/31/2017 23:59 PM"), null, null, null, null, null,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);

            //options.SignalIds.Add("8279");
            //options.SignalIds.Add("7185");
            options.SignalIds.Add("5605");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod()]
        public void YearOfWednesdaysTest()
        {
            List<DayOfWeek> daysofWeek = new List<DayOfWeek>();
            daysofWeek.Add(DayOfWeek.Wednesday);
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("01/01/2017");
            options.EndDate = Convert.ToDateTime("12/31/2017");
            
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("01/01/2017 00:01 AM"),
                Convert.ToDateTime("12/31/2017 23:59 PM"), null, null, null, null, daysofWeek,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.TimePeriod);

            //options.SignalIds.Add("8279");
            //options.SignalIds.Add("7185");
            options.SignalIds.Add("5605");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod()]
        public void CreateApproachMetricTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Approach;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017 7:00 AM"),
                Convert.ToDateTime("10/17/2017 8:00 AM"), null, null, null, null, null,
                BinFactoryOptions.BinSizes.FifteenMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);

            //options.SignalIds.Add("8279");
            //options.SignalIds.Add("7185");
            options.SignalIds.Add("5605");
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }
    }
}