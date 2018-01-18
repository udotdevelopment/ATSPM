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
            ApproachSplitFailAggregationRepositoryFactory.SetApplicationEventRepository(new InMemoryApproachSplitFailAggregationRepository(Db));
            MOE.Common.Models.Repositories.SignalsRepositoryFactory.SetSignalsRepository(new InMemorySignalsRepository(Db));
            MetricTypeRepositoryFactory.SetMetricsRepository(new InMemoryMetricTypeRepository(Db));
            ApplicationEventRepositoryFactory.SetApplicationEventRepository(new InMemoryApplicationEventRepository(Db));
            Models.Repositories.DirectionTypeRepositoryFactory.SetDirectionsRepository(new InMemoryDirectionTypeRepository());
        }

        private void PopulateApproachesWithSplitFailtAggregationRecords(ApproachSplitFailAggregationOptions options)
        {
            foreach (var signalId in options.SignalIds)
            {
                var approaches = from r in Db.Approaches
                    where r.SignalID == signalId
                    select r;

                foreach (var appr in approaches)
                {
                    Db.PopulateApproachSplitFailAggregations(options.StartDate, options.EndDate, appr.ApproachID);
                }
            }
        }

        [TestMethod()]
        public void CreateMetricTest()
        {
  

            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();


            options.StartDate = DateTime.Now.AddDays(-1);//Convert.ToDateTime("10/17/2017");
            options.EndDate = DateTime.Now; //Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                DateTime.Now.AddDays(-1),//Convert.ToDateTime("10/17/2017 7:00 AM"), 
                DateTime.Now,//Convert.ToDateTime("10/17/2017 8:00 AM"), 
                null, null, null, null, null, 
                BinFactoryOptions.BinSizes.FifteenMinutes, 
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("101");
            //options.SignalIds.Add("8279");
            //options.SignalIds.Add("7185");
            PopulateApproachesWithSplitFailtAggregationRecords(options);

            
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
            ApproachSplitFailAggregationRepositoryFactory.SetApplicationEventRepository(new InMemoryApproachSplitFailAggregationRepository(Db));
            MOE.Common.Models.Repositories.SignalsRepositoryFactory.SetSignalsRepository(new InMemorySignalsRepository(Db));

            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();


            options.StartDate = DateTime.Now.AddDays(-1);//Convert.ToDateTime("10/17/2017");
            options.EndDate = DateTime.Now; //Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                DateTime.Today.AddHours(7),//Convert.ToDateTime("10/17/2017 7:00 AM"), 
                DateTime.Today.AddHours(8),//Convert.ToDateTime("10/17/2017 8:00 AM"), 
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);

            options.SignalIds.Add("101");
            //options.SignalIds.Add("8279");
            //options.SignalIds.Add("7185");
            PopulateApproachesWithSplitFailtAggregationRecords(options);

            
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
            ApproachSplitFailAggregationRepositoryFactory.SetApplicationEventRepository(new InMemoryApproachSplitFailAggregationRepository(Db));
            MOE.Common.Models.Repositories.SignalsRepositoryFactory.SetSignalsRepository(new InMemorySignalsRepository(Db));

            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();


            options.StartDate = DateTime.Now.AddDays(-7);//Convert.ToDateTime("10/17/2017");
            options.EndDate = DateTime.Now; //Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                DateTime.Now.AddDays(-7),//Convert.ToDateTime("10/17/2017 7:00 AM"), 
                DateTime.Now,//Convert.ToDateTime("10/17/2017 8:00 AM"), 
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);

            options.SignalIds.Add("101");
            //options.SignalIds.Add("8279");
            //options.SignalIds.Add("7185");
            PopulateApproachesWithSplitFailtAggregationRecords(options);

            
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
            ApproachSplitFailAggregationRepositoryFactory.SetApplicationEventRepository(new InMemoryApproachSplitFailAggregationRepository(Db));
            MOE.Common.Models.Repositories.SignalsRepositoryFactory.SetSignalsRepository(new InMemorySignalsRepository(Db));

            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();


            options.StartDate = DateTime.Now.AddDays(-21);//Convert.ToDateTime("10/17/2017");
            options.EndDate = DateTime.Now; //Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                DateTime.Now.AddDays(-21),//Convert.ToDateTime("10/17/2017 7:00 AM"), 
                DateTime.Now,//Convert.ToDateTime("10/17/2017 8:00 AM"), 
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Week,
                BinFactoryOptions.TimeOptions.StartToEnd);

            options.SignalIds.Add("101");
            options.SignalIds.Add("102");
            options.SignalIds.Add("103");
            PopulateApproachesWithSplitFailtAggregationRecords(options);
            


         
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


            options.StartDate = DateTime.Now.AddMonths(-3);//Convert.ToDateTime("10/17/2017");
            options.EndDate = DateTime.Now; //Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                DateTime.Now.AddMonths(-3),//Convert.ToDateTime("10/17/2017 7:00 AM"), 
                DateTime.Now,//Convert.ToDateTime("10/17/2017 8:00 AM"), 
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);

            options.SignalIds.Add("101");
            //options.SignalIds.Add("8279");
            //options.SignalIds.Add("7185");
            PopulateApproachesWithSplitFailtAggregationRecords(options);

            
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


            options.StartDate = DateTime.Now.AddYears(-3);//Convert.ToDateTime("10/17/2017");
            options.EndDate = DateTime.Now; //Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                DateTime.Now.AddYears(-3),//Convert.ToDateTime("10/17/2017 7:00 AM"), 
                DateTime.Now,//Convert.ToDateTime("10/17/2017 8:00 AM"), 
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);

            options.SignalIds.Add("101");
            //options.SignalIds.Add("8279");
            //options.SignalIds.Add("7185");
            PopulateApproachesWithSplitFailtAggregationRecords(options);

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


            options.StartDate = DateTime.Now.AddYears(-3);//Convert.ToDateTime("10/17/2017");
            options.EndDate = DateTime.Now; //Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                DateTime.Now.AddYears(-3),//Convert.ToDateTime("10/17/2017 7:00 AM"), 
                DateTime.Now,//Convert.ToDateTime("10/17/2017 8:00 AM"), 
                null, null, null, null, daysofWeek,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);

            options.SignalIds.Add("101");
            //options.SignalIds.Add("8279");
            //options.SignalIds.Add("7185");
            PopulateApproachesWithSplitFailtAggregationRecords(options);
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
        public void YearOfWednesdaysAndFullYearProduceDifferentResultsTest()
        {
            List<DayOfWeek> daysofWeek = new List<DayOfWeek>();
            daysofWeek.Add(DayOfWeek.Wednesday);
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();


            options.StartDate = DateTime.Now.AddYears(-3);//Convert.ToDateTime("10/17/2017");
            options.EndDate = DateTime.Now; //Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
            options.TimeOptions = new BinFactoryOptions(
                DateTime.Now.AddYears(-3),//Convert.ToDateTime("10/17/2017 7:00 AM"), 
                DateTime.Now,//Convert.ToDateTime("10/17/2017 8:00 AM"), 
                null, null, null, null, daysofWeek,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);

            options.SignalIds.Add("101");
            //options.SignalIds.Add("8279");
            //options.SignalIds.Add("7185");
            PopulateApproachesWithSplitFailtAggregationRecords(options);
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;

            options.TimeOptions = new BinFactoryOptions(
                DateTime.Now.AddYears(-3),//Convert.ToDateTime("10/17/2017 7:00 AM"), 
                DateTime.Now,//Convert.ToDateTime("10/17/2017 8:00 AM"), 
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);


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
        public void CreateSignalMetricTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();


            options.StartDate = DateTime.Now.AddMonths(-1);
            options.EndDate = DateTime.Now;
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Signal;
            options.TimeOptions = new BinFactoryOptions(
                DateTime.Now.AddMonths(-1),
                DateTime.Now,
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);

            options.SignalIds.Add("101");
            options.SignalIds.Add("102");
            options.SignalIds.Add("103");
            PopulateApproachesWithSplitFailtAggregationRecords(options);

            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        [TestMethod]
        public void CreateSignalByDirectionMetricTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();


            options.StartDate = DateTime.Now.AddMonths(-1);
            options.EndDate = DateTime.Now;
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.SignalByDirection;
            options.TimeOptions = new BinFactoryOptions(
                DateTime.Now.AddMonths(-1),
                DateTime.Now,
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);

            options.SignalIds.Add("101");
            options.SignalIds.Add("102");
            options.SignalIds.Add("103");
            PopulateApproachesWithSplitFailtAggregationRecords(options);

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