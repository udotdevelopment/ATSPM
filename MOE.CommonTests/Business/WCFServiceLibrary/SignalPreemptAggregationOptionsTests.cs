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
    public class SignalPreemptAggregationOptionsTests
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
           SignalsRepositoryFactory.SetSignalsRepository(new InMemorySignalsRepository(Db));
            MetricTypeRepositoryFactory.SetMetricsRepository(new InMemoryMetricTypeRepository(Db));
            ApplicationEventRepositoryFactory.SetApplicationEventRepository(new InMemoryApplicationEventRepository(Db));
            DirectionTypeRepositoryFactory.SetDirectionsRepository(new InMemoryDirectionTypeRepository(Db));
            PreemptAggregationDatasRepositoryFactory.SetArchivedMetricsRepository(new InMemoryPreemptAggregationDatasRepository(Db));

        }

        [TestMethod()]
        public void GetPreemptByTimeTest()
        {
            SignalPreemptAggregationOptions options = new SignalPreemptAggregationOptions();



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
            PopulateSignalsWithPreemptionAggregationRecords(options);
            options.GetSignalObjects();

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
        public void GetPreemptByRouteTest()
        {
            SignalPreemptAggregationOptions options = new SignalPreemptAggregationOptions();



            options.StartDate = DateTime.Now.AddDays(-21);//Convert.ToDateTime("10/17/2017");
            options.EndDate = DateTime.Now; //Convert.ToDateTime("10/18/2017");
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Route;
            options.TimeOptions = new BinFactoryOptions(
                DateTime.Now.AddDays(-21),//Convert.ToDateTime("10/17/2017 7:00 AM"), 
                DateTime.Now,//Convert.ToDateTime("10/17/2017 8:00 AM"), 
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Week,
                BinFactoryOptions.TimeOptions.StartToEnd);

            options.SignalIds.Add("101");
            options.SignalIds.Add("102");
            options.SignalIds.Add("103");
            PopulateSignalsWithPreemptionAggregationRecords(options);
            options.GetSignalObjects();

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
        public void GetPreemptBySignalByPhaseChartTest()
        {
            SignalPreemptAggregationOptions options = new SignalPreemptAggregationOptions();


            options.StartDate = DateTime.Now.AddMonths(-1);
            options.EndDate = DateTime.Now;
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.SignalByPhase;
            options.TimeOptions = new BinFactoryOptions(
                DateTime.Now.AddMonths(-1),
                DateTime.Now,
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);

            options.SignalIds.Add("101");
            //options.SignalIds.Add("102");
            //options.SignalIds.Add("103");
            PopulateSignalsWithPreemptionAggregationRecords(options);
            options.GetSignalObjects();

            options.ChartType = AggregationMetricOptions.ChartTypes.StackedColumn;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.Line;
            options.CreateMetric();
            options.ChartType = AggregationMetricOptions.ChartTypes.StackedLine;
            Assert.IsTrue(options.CreateMetric().Count > 0);
        }

        private void PopulateSignalsWithPreemptionAggregationRecords(SignalPreemptAggregationOptions options)
        {
            int i = 1;
            foreach (var signalId in options.SignalIds)
            {
                    Db.PopulatePreemptionAggregations(options.StartDate, options.EndDate, options.SignalID, i);
                i++;
            }
        }
    }
}