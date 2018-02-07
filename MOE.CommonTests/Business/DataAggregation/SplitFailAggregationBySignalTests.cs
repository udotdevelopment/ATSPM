using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.DataAggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Models;

namespace MOE.Common.Business.DataAggregation.Tests
{
    [TestClass()]
    public class SplitFailAggregationBySignalTests
    {
        public InMemoryMOEDatabase Db = new InMemoryMOEDatabase();
        public ISignalsRepository SignalsRepository;

        public SplitFailAggregationBySignalTests()
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
                    Db.PopulateApproachSplitFailAggregationsWithValue3(Convert.ToDateTime("1/1/2016"), Convert.ToDateTime("1/1/2018"), approach);
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
        public void SplitFailAggregationBySignalSartToEnd15MinuteBinAllDayTypesTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("102");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SignalIds.Add("103");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.SignalIds.Add("104");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignal = new SplitFailAggregationBySignal(options, options.Signals[0]);
            Assert.IsTrue(splitAggregationBySignal.ApproachSplitFailures.Count == 4);

            //Test Approach level stats
            foreach (var approachSplitFailAggregationContainer in splitAggregationBySignal.ApproachSplitFailures)
            {
                Assert.IsTrue(approachSplitFailAggregationContainer.BinsContainers.Count == 1);
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    Assert.IsTrue(binsContainer.Bins.Count == 96);
                    var approachBinsContainer = approachSplitFailAggregationContainer.BinsContainers[i];
                    Assert.IsTrue(binsContainer.Start == approachBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == approachBinsContainer.End);
                    Assert.IsTrue(approachBinsContainer.SumValue == 288);
                    Assert.IsTrue(approachBinsContainer.AverageValue == 3);
                    for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                    {
                        var bin = binsContainer.Bins[binIndex];
                        var approachBin = approachBinsContainer.Bins[binIndex];
                        Assert.IsTrue(bin.Start == approachBin.Start);
                        Assert.IsTrue(bin.End == approachBin.End);
                        Assert.IsTrue(approachBin.Sum == 3);
                        Assert.IsTrue(approachBin.Average == 3);
                    }
                }
            }
            //Test Signal Level Stats
            for (int i = 0; i < binsContainers.Count; i++)
            {
                var binsContainer = binsContainers[i];
                var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                Assert.IsTrue(signalBinsContainer.SumValue == 1152);
                Assert.IsTrue(signalBinsContainer.AverageValue == 12);
                for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                {
                    var bin = binsContainer.Bins[binIndex];
                    var signalBin = signalBinsContainer.Bins[binIndex];
                    Assert.IsTrue(bin.Start == signalBin.Start);
                    Assert.IsTrue(bin.End == signalBin.End);
                    Assert.IsTrue(signalBin.Sum == 12);
                }
            }
            Assert.IsTrue(splitAggregationBySignal.TotalSplitFailures == 1152);
            Assert.IsTrue(splitAggregationBySignal.AverageSplitFailures == 12);
        }

        [TestMethod()]
        public void SplitFailAggregationBySignalTimePeriod15MinuteBinAllDayTypesTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 0, 8, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("102");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SignalIds.Add("103");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.SignalIds.Add("104");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignal = new SplitFailAggregationBySignal(options, options.Signals[0]);
            Assert.IsTrue(splitAggregationBySignal.ApproachSplitFailures.Count == 4);

            //Test Approach level stats
            foreach (var approachSplitFailAggregationContainer in splitAggregationBySignal.ApproachSplitFailures)
            {
                Assert.IsTrue(approachSplitFailAggregationContainer.BinsContainers.Count == 1);
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    Assert.IsTrue(binsContainer.Bins.Count == 4);
                    var approachBinsContainer = approachSplitFailAggregationContainer.BinsContainers[i];
                    Assert.IsTrue(binsContainer.Start == approachBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == approachBinsContainer.End);
                    Assert.IsTrue(approachBinsContainer.SumValue == 12);
                    Assert.IsTrue(approachBinsContainer.AverageValue == 3);
                    for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                    {
                        var bin = binsContainer.Bins[binIndex];
                        var approachBin = approachBinsContainer.Bins[binIndex];
                        Assert.IsTrue(bin.Start == approachBin.Start);
                        Assert.IsTrue(bin.End == approachBin.End);
                        Assert.IsTrue(approachBin.Sum == 3);
                        Assert.IsTrue(approachBin.Average == 3);
                    }
                }
            }
            //Test Signal Level Stats
            for (int i = 0; i < binsContainers.Count; i++)
            {
                var binsContainer = binsContainers[i];
                var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                Assert.IsTrue(signalBinsContainer.SumValue == 48);
                Assert.IsTrue(signalBinsContainer.AverageValue == 12);
                for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                {
                    var bin = binsContainer.Bins[binIndex];
                    var signalBin = signalBinsContainer.Bins[binIndex];
                    Assert.IsTrue(bin.Start == signalBin.Start);
                    Assert.IsTrue(bin.End == signalBin.End);
                    Assert.IsTrue(signalBin.Sum == 12);
                }
            }
            Assert.IsTrue(splitAggregationBySignal.TotalSplitFailures == 48);
            Assert.IsTrue(splitAggregationBySignal.AverageSplitFailures == 12);
        }


        [TestMethod()]
        public void SplitFailAggregationBySignalSartToEnd30MinuteBinAllDayTypesTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.ThirtyMinute,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("102");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SignalIds.Add("103");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.SignalIds.Add("104");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignal = new SplitFailAggregationBySignal(options, options.Signals[0]);
            Assert.IsTrue(splitAggregationBySignal.ApproachSplitFailures.Count == 4);

            //Test Approach level stats
            foreach (var approachSplitFailAggregationContainer in splitAggregationBySignal.ApproachSplitFailures)
            {
                Assert.IsTrue(approachSplitFailAggregationContainer.BinsContainers.Count == 1);
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    Assert.IsTrue(binsContainer.Bins.Count == 48);
                    var approachBinsContainer = approachSplitFailAggregationContainer.BinsContainers[i];
                    Assert.IsTrue(binsContainer.Start == approachBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == approachBinsContainer.End);
                    Assert.IsTrue(approachBinsContainer.SumValue == 288);
                    Assert.IsTrue(approachBinsContainer.AverageValue == 6);
                    for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                    {
                        var bin = binsContainer.Bins[binIndex];
                        var approachBin = approachBinsContainer.Bins[binIndex];
                        Assert.IsTrue(bin.Start == approachBin.Start);
                        Assert.IsTrue(bin.End == approachBin.End);
                        Assert.IsTrue(approachBin.Sum == 6);
                        Assert.IsTrue(approachBin.Average == 6);
                    }
                }
            }
            //Test Signal Level Stats
            for (int i = 0; i < binsContainers.Count; i++)
            {
                var binsContainer = binsContainers[i];
                var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                Assert.IsTrue(signalBinsContainer.SumValue == 1152);
                Assert.IsTrue(signalBinsContainer.AverageValue == 24);
                for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                {
                    var bin = binsContainer.Bins[binIndex];
                    var signalBin = signalBinsContainer.Bins[binIndex];
                    Assert.IsTrue(bin.Start == signalBin.Start);
                    Assert.IsTrue(bin.End == signalBin.End);
                    Assert.IsTrue(signalBin.Sum == 24);
                }
            }
            Assert.IsTrue(splitAggregationBySignal.TotalSplitFailures == 1152);
            Assert.IsTrue(splitAggregationBySignal.AverageSplitFailures == 24);
        }

        [TestMethod()]
        public void SplitFailAggregationBySignalTimePeriod30MinuteBinAllDayTypesTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 0, 8, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSize.ThirtyMinute,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("102");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SignalIds.Add("103");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.SignalIds.Add("104");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignal = new SplitFailAggregationBySignal(options, options.Signals[0]);
            Assert.IsTrue(splitAggregationBySignal.ApproachSplitFailures.Count == 4);

            //Test Approach level stats
            foreach (var approachSplitFailAggregationContainer in splitAggregationBySignal.ApproachSplitFailures)
            {
                Assert.IsTrue(approachSplitFailAggregationContainer.BinsContainers.Count == 1);
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    Assert.IsTrue(binsContainer.Bins.Count == 2);
                    var approachBinsContainer = approachSplitFailAggregationContainer.BinsContainers[i];
                    Assert.IsTrue(binsContainer.Start == approachBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == approachBinsContainer.End);
                    Assert.IsTrue(approachBinsContainer.SumValue == 12);
                    Assert.IsTrue(approachBinsContainer.AverageValue == 6);
                    for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                    {
                        var bin = binsContainer.Bins[binIndex];
                        var approachBin = approachBinsContainer.Bins[binIndex];
                        Assert.IsTrue(bin.Start == approachBin.Start);
                        Assert.IsTrue(bin.End == approachBin.End);
                        Assert.IsTrue(approachBin.Sum == 6);
                        Assert.IsTrue(approachBin.Average == 6);
                    }
                }
            }
            //Test Signal Level Stats
            for (int i = 0; i < binsContainers.Count; i++)
            {
                var binsContainer = binsContainers[i];
                var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                Assert.IsTrue(signalBinsContainer.SumValue == 48);
                Assert.IsTrue(signalBinsContainer.AverageValue == 24);
                for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                {
                    var bin = binsContainer.Bins[binIndex];
                    var signalBin = signalBinsContainer.Bins[binIndex];
                    Assert.IsTrue(bin.Start == signalBin.Start);
                    Assert.IsTrue(bin.End == signalBin.End);
                    Assert.IsTrue(signalBin.Sum == 24);
                }
            }
            Assert.IsTrue(splitAggregationBySignal.TotalSplitFailures == 48);
            Assert.IsTrue(splitAggregationBySignal.AverageSplitFailures == 24);
        }

        [TestMethod()]
        public void SplitFailAggregationBySignalSartToEndHourMinuteBinAllDayTypesTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("102");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SignalIds.Add("103");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.SignalIds.Add("104");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignal = new SplitFailAggregationBySignal(options, options.Signals[0]);
            Assert.IsTrue(splitAggregationBySignal.ApproachSplitFailures.Count == 4);

            //Test Approach level stats
            foreach (var approachSplitFailAggregationContainer in splitAggregationBySignal.ApproachSplitFailures)
            {
                Assert.IsTrue(approachSplitFailAggregationContainer.BinsContainers.Count == 1);
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    Assert.IsTrue(binsContainer.Bins.Count == 24);
                    var approachBinsContainer = approachSplitFailAggregationContainer.BinsContainers[i];
                    Assert.IsTrue(binsContainer.Start == approachBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == approachBinsContainer.End);
                    Assert.IsTrue(approachBinsContainer.SumValue == 288);
                    Assert.IsTrue(approachBinsContainer.AverageValue == 12);
                    for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                    {
                        var bin = binsContainer.Bins[binIndex];
                        var approachBin = approachBinsContainer.Bins[binIndex];
                        Assert.IsTrue(bin.Start == approachBin.Start);
                        Assert.IsTrue(bin.End == approachBin.End);
                        Assert.IsTrue(approachBin.Sum == 12);
                        Assert.IsTrue(approachBin.Average == 12);
                    }
                }
            }
            //Test Signal Level Stats
            for (int i = 0; i < binsContainers.Count; i++)
            {
                var binsContainer = binsContainers[i];
                var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                Assert.IsTrue(signalBinsContainer.SumValue == 1152);
                Assert.IsTrue(signalBinsContainer.AverageValue == 48);
                for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                {
                    var bin = binsContainer.Bins[binIndex];
                    var signalBin = signalBinsContainer.Bins[binIndex];
                    Assert.IsTrue(bin.Start == signalBin.Start);
                    Assert.IsTrue(bin.End == signalBin.End);
                    Assert.IsTrue(signalBin.Sum == 48);
                }
            }
            Assert.IsTrue(splitAggregationBySignal.TotalSplitFailures == 1152);
            Assert.IsTrue(splitAggregationBySignal.AverageSplitFailures == 48);
        }

        [TestMethod()]
        public void SplitFailAggregationBySignalTimePeriodHourBinAllDayTypesTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                6, 0, 10, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSize.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("102");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SignalIds.Add("103");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.SignalIds.Add("104");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignal = new SplitFailAggregationBySignal(options, options.Signals[0]);
            Assert.IsTrue(splitAggregationBySignal.ApproachSplitFailures.Count == 4);

            //Test Approach level stats
            foreach (var approachSplitFailAggregationContainer in splitAggregationBySignal.ApproachSplitFailures)
            {
                Assert.IsTrue(approachSplitFailAggregationContainer.BinsContainers.Count == 1);
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    Assert.IsTrue(binsContainer.Bins.Count == 4);
                    var approachBinsContainer = approachSplitFailAggregationContainer.BinsContainers[i];
                    Assert.IsTrue(binsContainer.Start == approachBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == approachBinsContainer.End);
                    Assert.IsTrue(approachBinsContainer.SumValue == 48);
                    Assert.IsTrue(approachBinsContainer.AverageValue == 12);
                    for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                    {
                        var bin = binsContainer.Bins[binIndex];
                        var approachBin = approachBinsContainer.Bins[binIndex];
                        Assert.IsTrue(bin.Start == approachBin.Start);
                        Assert.IsTrue(bin.End == approachBin.End);
                        Assert.IsTrue(approachBin.Sum == 12);
                        Assert.IsTrue(approachBin.Average == 12);
                    }
                }
            }
            //Test Signal Level Stats
            for (int i = 0; i < binsContainers.Count; i++)
            {
                var binsContainer = binsContainers[i];
                var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                Assert.IsTrue(signalBinsContainer.SumValue == 192);
                Assert.IsTrue(signalBinsContainer.AverageValue == 48);
                for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                {
                    var bin = binsContainer.Bins[binIndex];
                    var signalBin = signalBinsContainer.Bins[binIndex];
                    Assert.IsTrue(bin.Start == signalBin.Start);
                    Assert.IsTrue(bin.End == signalBin.End);
                    Assert.IsTrue(signalBin.Sum == 48);
                }
            }
            Assert.IsTrue(splitAggregationBySignal.TotalSplitFailures == 192);
            Assert.IsTrue(splitAggregationBySignal.AverageSplitFailures == 48);
        }

        [TestMethod()]
        public void SplitFailAggregationBySignalSartToEndDayBinAllDayTypesTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("11/1/2017");
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("11/1/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("102");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SignalIds.Add("103");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.SignalIds.Add("104");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignal = new SplitFailAggregationBySignal(options, options.Signals[0]);
            Assert.IsTrue(splitAggregationBySignal.ApproachSplitFailures.Count == 4);

            //Test Approach level stats
            foreach (var approachSplitFailAggregationContainer in splitAggregationBySignal.ApproachSplitFailures)
            {
                Assert.IsTrue(approachSplitFailAggregationContainer.BinsContainers.Count == 1);
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    Assert.IsTrue(binsContainer.Bins.Count == 31);
                    var approachBinsContainer = approachSplitFailAggregationContainer.BinsContainers[i];
                    Assert.IsTrue(approachBinsContainer.Bins.Count == 31);
                    Assert.IsTrue(binsContainer.Start == approachBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == approachBinsContainer.End);
                    Assert.IsTrue(approachBinsContainer.SumValue == 8928);
                    Assert.IsTrue(approachBinsContainer.AverageValue == 288);
                    for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                    {
                        var bin = binsContainer.Bins[binIndex];
                        var approachBin = approachBinsContainer.Bins[binIndex];
                        Assert.IsTrue(bin.Start == approachBin.Start);
                        Assert.IsTrue(bin.End == approachBin.End);
                        Assert.IsTrue(approachBin.Sum == 288);
                        Assert.IsTrue(approachBin.Average == 288);
                    }
                }
            }
            //Test Signal Level Stats
            for (int i = 0; i < binsContainers.Count; i++)
            {
                var binsContainer = binsContainers[i];
                var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                Assert.IsTrue(signalBinsContainer.SumValue == 35712);
                Assert.IsTrue(signalBinsContainer.AverageValue == 1152);
                for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                {
                    var bin = binsContainer.Bins[binIndex];
                    var signalBin = signalBinsContainer.Bins[binIndex];
                    Assert.IsTrue(bin.Start == signalBin.Start);
                    Assert.IsTrue(bin.End == signalBin.End);
                    Assert.IsTrue(signalBin.Sum == 1152);
                }
            }
            Assert.IsTrue(splitAggregationBySignal.TotalSplitFailures == 35712);
            Assert.IsTrue(splitAggregationBySignal.AverageSplitFailures == 1152);
        }

        [TestMethod()]
        public void SplitFailAggregationBySignalTimePeriodDayBinAllDayTypesTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("11/1/2017");
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("11/1/2017"),
                6, 0, 10, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSize.Day,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("102");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SignalIds.Add("103");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.SignalIds.Add("104");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignal = new SplitFailAggregationBySignal(options, options.Signals[0]);
            Assert.IsTrue(splitAggregationBySignal.ApproachSplitFailures.Count == 4);

            //Test Approach level stats
            foreach (var approachSplitFailAggregationContainer in splitAggregationBySignal.ApproachSplitFailures)
            {
                Assert.IsTrue(approachSplitFailAggregationContainer.BinsContainers.Count == 1);
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    Assert.IsTrue(binsContainer.Bins.Count == 31);
                    var approachBinsContainer = approachSplitFailAggregationContainer.BinsContainers[i];
                    Assert.IsTrue(binsContainer.Start == approachBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == approachBinsContainer.End);
                    Assert.IsTrue(approachBinsContainer.SumValue == 1488);
                    Assert.IsTrue(approachBinsContainer.AverageValue == 48);
                    for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                    {
                        var bin = binsContainer.Bins[binIndex];
                        var approachBin = approachBinsContainer.Bins[binIndex];
                        Assert.IsTrue(bin.Start == approachBin.Start);
                        Assert.IsTrue(bin.End == approachBin.End);
                        Assert.IsTrue(approachBin.Sum == 48);
                        Assert.IsTrue(approachBin.Average == 48);
                    }
                }
            }
            //Test Signal Level Stats
            for (int i = 0; i < binsContainers.Count; i++)
            {
                var binsContainer = binsContainers[i];
                var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                Assert.IsTrue(signalBinsContainer.SumValue == 5952);
                Assert.IsTrue(signalBinsContainer.AverageValue == 192);
                for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                {
                    var bin = binsContainer.Bins[binIndex];
                    var signalBin = signalBinsContainer.Bins[binIndex];
                    Assert.IsTrue(bin.Start == signalBin.Start);
                    Assert.IsTrue(bin.End == signalBin.End);
                    Assert.IsTrue(signalBin.Sum == 192);
                }
            }
            Assert.IsTrue(splitAggregationBySignal.TotalSplitFailures == 5952);
            Assert.IsTrue(splitAggregationBySignal.AverageSplitFailures == 192);
        }

        [TestMethod()]
        public void SplitFailAggregationBySignalSartToEndMonthBinAllDayTypesTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("1/1/2018");
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("1/1/2018"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("102");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SignalIds.Add("103");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.SignalIds.Add("104");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignal = new SplitFailAggregationBySignal(options, options.Signals[0]);
            Assert.IsTrue(splitAggregationBySignal.ApproachSplitFailures.Count == 4);

            //Test Approach level stats
            foreach (var approachSplitFailAggregationContainer in splitAggregationBySignal.ApproachSplitFailures)
            {
                Assert.IsTrue(approachSplitFailAggregationContainer.BinsContainers.Count == 1);
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    Assert.IsTrue(binsContainer.Bins.Count == 12);
                    var approachBinsContainer = approachSplitFailAggregationContainer.BinsContainers[i];
                    Assert.IsTrue(approachBinsContainer.Bins.Count == 12);
                    Assert.IsTrue(binsContainer.Start == approachBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == approachBinsContainer.End);
                    Assert.IsTrue(approachBinsContainer.SumValue == 105120);
                    Assert.IsTrue(approachBinsContainer.AverageValue == 8760);
                    for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                    {
                        var bin = binsContainer.Bins[binIndex];
                        var approachBin = approachBinsContainer.Bins[binIndex];
                        Assert.IsTrue(bin.Start == approachBin.Start);
                        Assert.IsTrue(bin.End == approachBin.End);
                        Assert.IsTrue(approachBin.Sum == 288 * DateTime.DaysInMonth(approachBin.Start.Year, approachBin.Start.Month));
                        Assert.IsTrue(approachBin.Sum == 288 * DateTime.DaysInMonth(approachBin.Start.Year, approachBin.Start.Month));
                    }
                }
            }
            //Test Signal Level Stats
            for (int i = 0; i < binsContainers.Count; i++)
            {
                var binsContainer = binsContainers[i];
                var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                Assert.IsTrue(signalBinsContainer.SumValue == 420480);
                Assert.IsTrue(signalBinsContainer.AverageValue == 35040);
                for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                {
                    var bin = binsContainer.Bins[binIndex];
                    var signalBin = signalBinsContainer.Bins[binIndex];
                    Assert.IsTrue(bin.Start == signalBin.Start);
                    Assert.IsTrue(bin.End == signalBin.End);
                    Assert.IsTrue(signalBin.Sum == 1152 * DateTime.DaysInMonth(bin.Start.Year, bin.Start.Month));
                }
            }
            Assert.IsTrue(splitAggregationBySignal.TotalSplitFailures == 420480);
            Assert.IsTrue(splitAggregationBySignal.AverageSplitFailures == 35040);
        }

        [TestMethod()]
        public void SplitFailAggregationBySignalTimePeriodMonthBinAllDayTypesTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("1/1/2018");
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("1/1/2018"),
                7, 0, 8, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSize.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("102");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SignalIds.Add("103");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.SignalIds.Add("104");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignal = new SplitFailAggregationBySignal(options, options.Signals[0]);
            Assert.IsTrue(splitAggregationBySignal.ApproachSplitFailures.Count == 4);

            //Test Approach level stats
            foreach (var approachSplitFailAggregationContainer in splitAggregationBySignal.ApproachSplitFailures)
            {
                Assert.IsTrue(approachSplitFailAggregationContainer.BinsContainers.Count == 12);
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    Assert.IsTrue(binsContainer.Bins.Count == DateTime.DaysInMonth(approachSplitFailAggregationContainer.BinsContainers[i].Start.Year, approachSplitFailAggregationContainer.BinsContainers[i].Start.Month));
                    var approachBinsContainer = approachSplitFailAggregationContainer.BinsContainers[i];
                    Assert.IsTrue(binsContainer.Start == approachBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == approachBinsContainer.End);
                    Assert.IsTrue(approachBinsContainer.SumValue == 12 * DateTime.DaysInMonth(approachSplitFailAggregationContainer.BinsContainers[i].Start.Year, approachSplitFailAggregationContainer.BinsContainers[i].Start.Month));
                    Assert.IsTrue(approachBinsContainer.AverageValue == 12);
                    for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                    {
                        var bin = binsContainer.Bins[binIndex];
                        var approachBin = approachBinsContainer.Bins[binIndex];
                        Assert.IsTrue(bin.Start == approachBin.Start);
                        Assert.IsTrue(bin.End == approachBin.End);
                        Assert.IsTrue(approachBin.Sum == 12);
                        Assert.IsTrue(approachBin.Average == 12);
                    }
                }
            }
            //Test Signal Level Stats
            for (int i = 0; i < binsContainers.Count; i++)
            {
                var binsContainer = binsContainers[i];
                var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                Assert.IsTrue(signalBinsContainer.SumValue == 48 * DateTime.DaysInMonth(binsContainers[i].Start.Year, binsContainers[i].Start.Month));
                Assert.IsTrue(signalBinsContainer.AverageValue == 48);
                for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                {
                    var bin = binsContainer.Bins[binIndex];
                    var signalBin = signalBinsContainer.Bins[binIndex];
                    Assert.IsTrue(bin.Start == signalBin.Start);
                    Assert.IsTrue(bin.End == signalBin.End);
                    Assert.IsTrue(signalBin.Sum == 48);
                }
            }
            Assert.IsTrue(splitAggregationBySignal.TotalSplitFailures == 17520);
            Assert.IsTrue(splitAggregationBySignal.AverageSplitFailures == 1460);
        }

        [TestMethod()]
        public void SplitFailAggregationBySignalSartToEndYearBinAllDayTypesTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("1/1/2018");
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("1/1/2018"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("102");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SignalIds.Add("103");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.SignalIds.Add("104");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignal = new SplitFailAggregationBySignal(options, options.Signals[0]);
            Assert.IsTrue(splitAggregationBySignal.ApproachSplitFailures.Count == 4);

            //Test Approach level stats
            foreach (var approachSplitFailAggregationContainer in splitAggregationBySignal.ApproachSplitFailures)
            {
                Assert.IsTrue(approachSplitFailAggregationContainer.BinsContainers.Count == 1);
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    Assert.IsTrue(binsContainer.Bins.Count == 2);
                    var approachBinsContainer = approachSplitFailAggregationContainer.BinsContainers[i];
                    Assert.IsTrue(approachBinsContainer.Bins.Count == 2);
                    Assert.IsTrue(binsContainer.Start == approachBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == approachBinsContainer.End);
                    Assert.IsTrue(approachBinsContainer.SumValue == 210528);
                    Assert.IsTrue(approachBinsContainer.AverageValue == 105264);
                    for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                    {
                        var bin = binsContainer.Bins[binIndex];
                        var approachBin = approachBinsContainer.Bins[binIndex];
                        Assert.IsTrue(bin.Start == approachBin.Start);
                        Assert.IsTrue(bin.End == approachBin.End);
                        if (DateTime.IsLeapYear(approachBin.Start.Year))
                        {
                            Assert.IsTrue(approachBin.Sum == 105408);
                        }
                        else
                        {
                            Assert.IsTrue(approachBin.Sum == 105120);
                        }
                    }
                }
            }
            //Test Signal Level Stats
            for (int i = 0; i < binsContainers.Count; i++)
            {
                var binsContainer = binsContainers[i];
                var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                Assert.IsTrue(signalBinsContainer.SumValue == 842112);
                Assert.IsTrue(signalBinsContainer.AverageValue == 421056);
                for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                {
                    var bin = binsContainer.Bins[binIndex];
                    var signalBin = signalBinsContainer.Bins[binIndex];
                    Assert.IsTrue(bin.Start == signalBin.Start);
                    Assert.IsTrue(bin.End == signalBin.End);
                    if (DateTime.IsLeapYear(bin.Start.Year))
                    {
                        Assert.IsTrue(signalBin.Sum == 421632);
                    }
                    else
                    {
                        Assert.IsTrue(signalBin.Sum == 420480);
                    }
                }
            }
            Assert.IsTrue(splitAggregationBySignal.TotalSplitFailures == 842112);
            Assert.IsTrue(splitAggregationBySignal.AverageSplitFailures == 421056);
        }

        [TestMethod()]
        public void SplitFailAggregationBySignalTimePeriodYearBinAllDayTypesTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("1/1/2018");
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("1/1/2018"),
                7, 0, 8, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSize.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("102");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SignalIds.Add("103");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.SignalIds.Add("104");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignal = new SplitFailAggregationBySignal(options, options.Signals[0]);
            Assert.IsTrue(splitAggregationBySignal.ApproachSplitFailures.Count == 4);

            //Test Approach level stats
            foreach (var approachSplitFailAggregationContainer in splitAggregationBySignal.ApproachSplitFailures)
            {
                Assert.IsTrue(approachSplitFailAggregationContainer.BinsContainers.Count == 2);
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    var approachBinsContainer = approachSplitFailAggregationContainer.BinsContainers[i];
                    if (DateTime.IsLeapYear(binsContainer.Start.Year))
                    {
                        Assert.IsTrue(binsContainer.Bins.Count == 366);
                        Assert.IsTrue(approachBinsContainer.SumValue == 4392);
                        Assert.IsTrue(approachBinsContainer.AverageValue == 12);
                    }
                    else
                    {
                        Assert.IsTrue(binsContainer.Bins.Count == 365);
                        Assert.IsTrue(approachBinsContainer.SumValue == 4380);
                        Assert.IsTrue(approachBinsContainer.AverageValue == 12);
                    }
                    Assert.IsTrue(binsContainer.Start == approachBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == approachBinsContainer.End);
                    for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                    {
                        var bin = binsContainer.Bins[binIndex];
                        var approachBin = approachBinsContainer.Bins[binIndex];
                        Assert.IsTrue(bin.Start == approachBin.Start);
                        Assert.IsTrue(bin.End == approachBin.End);
                        Assert.IsTrue(approachBin.Sum == 12);
                    }
                }
            }
            //Test Signal Level Stats
            for (int i = 0; i < binsContainers.Count; i++)
            {
                var binsContainer = binsContainers[i];
                var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                if (DateTime.IsLeapYear(binsContainer.Start.Year))
                {
                    Assert.IsTrue(signalBinsContainer.SumValue == 17568);
                }
                else
                {
                    Assert.IsTrue(signalBinsContainer.SumValue == 17520);
                }
                Assert.IsTrue(signalBinsContainer.AverageValue == 48);
                for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                {
                    var bin = binsContainer.Bins[binIndex];
                    var signalBin = signalBinsContainer.Bins[binIndex];
                    Assert.IsTrue(bin.Start == signalBin.Start);
                    Assert.IsTrue(bin.End == signalBin.End);
                    Assert.IsTrue(signalBin.Sum == 48);
                }
            }
            Assert.IsTrue(splitAggregationBySignal.TotalSplitFailures == 35088);
            Assert.IsTrue(splitAggregationBySignal.AverageSplitFailures == 17544);
        }

        [TestMethod()]
        public void SplitFailAggregationBySignalTestByPhaseStartToFinish()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            //options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.SignalByPhase;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null, BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("102");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignal = new SplitFailAggregationBySignal(options, options.Signals[0], 2);
            Assert.IsTrue(splitAggregationBySignal.ApproachSplitFailures.Count == 1);

            //Test Approach level stats
            foreach (var approachSplitFailAggregationContainer in splitAggregationBySignal.ApproachSplitFailures)
            {
                Assert.IsTrue(approachSplitFailAggregationContainer.BinsContainers.Count == 1);
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    Assert.IsTrue(binsContainer.Bins.Count == 96);
                    var approachBinsContainer = approachSplitFailAggregationContainer.BinsContainers[i];
                    Assert.IsTrue(binsContainer.Start == approachBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == approachBinsContainer.End);
                    Assert.IsTrue(approachBinsContainer.SumValue == 288);
                    Assert.IsTrue(approachBinsContainer.AverageValue == 3);
                    for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                    {
                        var bin = binsContainer.Bins[binIndex];
                        var approachBin = approachBinsContainer.Bins[binIndex];
                        Assert.IsTrue(bin.Start == approachBin.Start);
                        Assert.IsTrue(bin.End == approachBin.End);
                        Assert.IsTrue(approachBin.Sum == 3);
                        Assert.IsTrue(approachBin.Average == 3);
                    }
                }
            }
            //Test Signal Level Stats
            for (int i = 0; i < binsContainers.Count; i++)
            {
                var binsContainer = binsContainers[i];
                var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                Assert.IsTrue(signalBinsContainer.SumValue == 288);
                Assert.IsTrue(signalBinsContainer.AverageValue == 3);
                for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                {
                    var bin = binsContainer.Bins[binIndex];
                    var signalBin = signalBinsContainer.Bins[binIndex];
                    Assert.IsTrue(bin.Start == signalBin.Start);
                    Assert.IsTrue(bin.End == signalBin.End);
                    Assert.IsTrue(signalBin.Sum == 3);
                }
            }
            Assert.IsTrue(splitAggregationBySignal.TotalSplitFailures == 288);
            Assert.IsTrue(splitAggregationBySignal.AverageSplitFailures == 3);
        }

        [TestMethod()]
        public void SplitFailAggregationBySignalTestByPhaseTimePeriod()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            //options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.SignalByPhase;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 0, 8, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday }, BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("102");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignal = new SplitFailAggregationBySignal(options, options.Signals[0]);
            Assert.IsTrue(splitAggregationBySignal.ApproachSplitFailures.Count == 4);

            //Test Approach level stats
            foreach (var approachSplitFailAggregationContainer in splitAggregationBySignal.ApproachSplitFailures)
            {
                Assert.IsTrue(approachSplitFailAggregationContainer.BinsContainers.Count == 1);
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    Assert.IsTrue(binsContainer.Bins.Count == 4);
                    var approachBinsContainer = approachSplitFailAggregationContainer.BinsContainers[i];
                    Assert.IsTrue(binsContainer.Start == approachBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == approachBinsContainer.End);
                    Assert.IsTrue(approachBinsContainer.SumValue == 12);
                    Assert.IsTrue(approachBinsContainer.AverageValue == 3);
                    for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                    {
                        var bin = binsContainer.Bins[binIndex];
                        var approachBin = approachBinsContainer.Bins[binIndex];
                        Assert.IsTrue(bin.Start == approachBin.Start);
                        Assert.IsTrue(bin.End == approachBin.End);
                        Assert.IsTrue(approachBin.Sum == 3);
                        Assert.IsTrue(approachBin.Average == 3);
                    }
                }
            }
            //Test Signal Level Stats
            for (int i = 0; i < binsContainers.Count; i++)
            {
                var binsContainer = binsContainers[i];
                var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                Assert.IsTrue(signalBinsContainer.SumValue == 48);
                Assert.IsTrue(signalBinsContainer.AverageValue == 12);
                for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                {
                    var bin = binsContainer.Bins[binIndex];
                    var signalBin = signalBinsContainer.Bins[binIndex];
                    Assert.IsTrue(bin.Start == signalBin.Start);
                    Assert.IsTrue(bin.End == signalBin.End);
                    Assert.IsTrue(signalBin.Sum == 12);
                }
            }
            Assert.IsTrue(splitAggregationBySignal.TotalSplitFailures == 48);
            Assert.IsTrue(splitAggregationBySignal.AverageSplitFailures == 12);
        }

        [TestMethod()]
        public void SplitFailAggregationBySignalTestByDirectionStartToFinish()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            //options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.SignalByPhase;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null, BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("102");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignal = new SplitFailAggregationBySignal(options, options.Signals[0], options.Signals.FirstOrDefault().Approaches.FirstOrDefault().DirectionType);
            Assert.IsTrue(splitAggregationBySignal.ApproachSplitFailures.Count == 1);

            //Test Approach level stats
            foreach (var approachSplitFailAggregationContainer in splitAggregationBySignal.ApproachSplitFailures)
            {
                Assert.IsTrue(approachSplitFailAggregationContainer.BinsContainers.Count == 1);
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    Assert.IsTrue(binsContainer.Bins.Count == 96);
                    var approachBinsContainer = approachSplitFailAggregationContainer.BinsContainers[i];
                    Assert.IsTrue(binsContainer.Start == approachBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == approachBinsContainer.End);
                    Assert.IsTrue(approachBinsContainer.SumValue == 288);
                    Assert.IsTrue(approachBinsContainer.AverageValue == 3);
                    for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                    {
                        var bin = binsContainer.Bins[binIndex];
                        var approachBin = approachBinsContainer.Bins[binIndex];
                        Assert.IsTrue(bin.Start == approachBin.Start);
                        Assert.IsTrue(bin.End == approachBin.End);
                        Assert.IsTrue(approachBin.Sum == 3);
                        Assert.IsTrue(approachBin.Average == 3);
                    }
                }
            }
            //Test Signal Level Stats
            for (int i = 0; i < binsContainers.Count; i++)
            {
                var binsContainer = binsContainers[i];
                var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                Assert.IsTrue(signalBinsContainer.SumValue == 288);
                Assert.IsTrue(signalBinsContainer.AverageValue == 3);
                for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                {
                    var bin = binsContainer.Bins[binIndex];
                    var signalBin = signalBinsContainer.Bins[binIndex];
                    Assert.IsTrue(bin.Start == signalBin.Start);
                    Assert.IsTrue(bin.End == signalBin.End);
                    Assert.IsTrue(signalBin.Sum == 3);
                }
            }
            Assert.IsTrue(splitAggregationBySignal.TotalSplitFailures == 288);
            Assert.IsTrue(splitAggregationBySignal.AverageSplitFailures == 3);
        }

        [TestMethod()]
        public void SplitFailAggregationSignalTestByDirectionTimePeriod()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            //options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.SignalByPhase;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 0, 8, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday }, BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.SignalIds.Add("102");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignal = new SplitFailAggregationBySignal(options, options.Signals[0], options.Signals.FirstOrDefault().Approaches.FirstOrDefault().DirectionType);
            Assert.IsTrue(splitAggregationBySignal.ApproachSplitFailures.Count == 1);

            //Test Approach level stats
            foreach (var approachSplitFailAggregationContainer in splitAggregationBySignal.ApproachSplitFailures)
            {
                Assert.IsTrue(approachSplitFailAggregationContainer.BinsContainers.Count == 1);
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    Assert.IsTrue(binsContainer.Bins.Count == 4);
                    var approachBinsContainer = approachSplitFailAggregationContainer.BinsContainers[i];
                    Assert.IsTrue(binsContainer.Start == approachBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == approachBinsContainer.End);
                    Assert.IsTrue(approachBinsContainer.SumValue == 12);
                    Assert.IsTrue(approachBinsContainer.AverageValue == 3);
                    for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                    {
                        var bin = binsContainer.Bins[binIndex];
                        var approachBin = approachBinsContainer.Bins[binIndex];
                        Assert.IsTrue(bin.Start == approachBin.Start);
                        Assert.IsTrue(bin.End == approachBin.End);
                        Assert.IsTrue(approachBin.Sum == 3);
                        Assert.IsTrue(approachBin.Average == 3);
                    }
                }
            }
            //Test Signal Level Stats
            for (int i = 0; i < binsContainers.Count; i++)
            {
                var binsContainer = binsContainers[i];
                var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                Assert.IsTrue(signalBinsContainer.SumValue == 12);
                Assert.IsTrue(signalBinsContainer.AverageValue == 3);
                for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                {
                    var bin = binsContainer.Bins[binIndex];
                    var signalBin = signalBinsContainer.Bins[binIndex];
                    Assert.IsTrue(bin.Start == signalBin.Start);
                    Assert.IsTrue(bin.End == signalBin.End);
                    Assert.IsTrue(signalBin.Sum == 3);
                }
            }
            Assert.IsTrue(splitAggregationBySignal.TotalSplitFailures == 12);
            Assert.IsTrue(splitAggregationBySignal.AverageSplitFailures == 3);
        }

        [TestMethod()]
        public void GetSplitFailsByDirectionTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("102");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SignalIds.Add("103");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.SignalIds.Add("104");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignal = new SplitFailAggregationBySignal(options, options.Signals[0]);
            Assert.IsTrue(splitAggregationBySignal.GetSplitFailsByDirection(options.Signals.FirstOrDefault().Approaches.FirstOrDefault().DirectionType) == 288);

        }

        [TestMethod()]
        public void GetAverageSplitFailsByDirectionTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("102");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SignalIds.Add("103");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.SignalIds.Add("104");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignal = new SplitFailAggregationBySignal(options, options.Signals[0]);
            Assert.IsTrue(splitAggregationBySignal.GetAverageSplitFailsByDirection(options.Signals.FirstOrDefault().Approaches.FirstOrDefault().DirectionType) == 288);
        }

        [TestMethod()]
        public void TestSumValuesOnDifferentBinSizesTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("1/1/2018");
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("1/1/2018"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("102");
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignal15Minute = new SplitFailAggregationBySignal(options, options.Signals[0]);
            int fifteenMinuteSum = 0;
            foreach (var binsContainer in splitAggregationBySignal15Minute.BinsContainers)
            {
                fifteenMinuteSum += binsContainer.SumValue;
            }
            splitAggregationBySignal15Minute = null;
            options.TimeOptions.SelectedBinSize = BinFactoryOptions.BinSize.ThirtyMinute;
            binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignal30Minute = new SplitFailAggregationBySignal(options, options.Signals[0]);
            int thirtyMinuteSum = 0;
            foreach (var binsContainer in splitAggregationBySignal30Minute.BinsContainers)
            {
                thirtyMinuteSum += binsContainer.SumValue;
            }
            splitAggregationBySignal30Minute = null;
            Assert.IsTrue(fifteenMinuteSum == thirtyMinuteSum);
            options.TimeOptions.SelectedBinSize = BinFactoryOptions.BinSize.Hour;
            binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignalHour = new SplitFailAggregationBySignal(options, options.Signals[0]);
            int hourSum = 0;
            foreach (var binsContainer in splitAggregationBySignalHour.BinsContainers)
            {
                hourSum += binsContainer.SumValue;
            }
            splitAggregationBySignalHour = null;
            Assert.IsTrue(fifteenMinuteSum == hourSum);
            options.TimeOptions.SelectedBinSize = BinFactoryOptions.BinSize.Day;
            binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignalDay = new SplitFailAggregationBySignal(options, options.Signals[0]);
            int daySum = 0;
            foreach (var binsContainer in splitAggregationBySignalDay.BinsContainers)
            {
                daySum += binsContainer.SumValue;
            }
            splitAggregationBySignalDay = null;
            Assert.IsTrue(fifteenMinuteSum == daySum);
            options.TimeOptions.SelectedBinSize = BinFactoryOptions.BinSize.Month;
            binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignalMonth = new SplitFailAggregationBySignal(options, options.Signals[0]);
            int monthSum = 0;
            foreach (var binsContainer in splitAggregationBySignalMonth.BinsContainers)
            {
                monthSum += binsContainer.SumValue;
            }
            splitAggregationBySignalMonth = null;
            Assert.IsTrue(fifteenMinuteSum == monthSum);
            options.TimeOptions.SelectedBinSize = BinFactoryOptions.BinSize.Year;
            binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignalYear = new SplitFailAggregationBySignal(options, options.Signals[0]);
            int yearSum = 0;
            foreach (var binsContainer in splitAggregationBySignalYear.BinsContainers)
            {
                yearSum += binsContainer.SumValue;
            }
            splitAggregationBySignalYear = null;
        }

        [TestMethod()]
        public void TimeSumToSignalSumTest()
        {
            var signalsRepository = SignalsRepositoryFactory.Create();
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("1/2/2017");
            options.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options.SelectedXAxisType = AggregationMetricOptions.XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
            Convert.ToDateTime("1/2/2017"),
            null, null, null, null, null,
            BinFactoryOptions.BinSize.FifteenMinute,
            BinFactoryOptions.TimeOptions.StartToEnd);
            options.SignalIds.Add("102");
            foreach (var signal in options.SignalIds)
            {
                options.Signals.Add(signalsRepository.GetLatestVersionOfSignalBySignalID(signal));
            }
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            SplitFailAggregationBySignal splitAggregationBySignal15Minute =
                new SplitFailAggregationBySignal(options, options.Signals[0]);


            ApproachSplitFailAggregationOptions options2 = new ApproachSplitFailAggregationOptions();
            options2.StartDate = Convert.ToDateTime("1/1/2017");
            options2.EndDate = Convert.ToDateTime("1/2/2017");
            options2.SelectedAggregationType = AggregationMetricOptions.AggregationType.Sum;
            options2.SelectedXAxisType = AggregationMetricOptions.XAxisType.Signal;
            options2.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("1/2/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options2.SignalIds.Add("102");
            foreach (var signal in options2.SignalIds)
            {
                options2.Signals.Add(signalsRepository.GetLatestVersionOfSignalBySignalID(signal));
            }
            options2.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options2.SelectedChartType = SeriesChartType.Column;
            Assert.IsTrue(splitAggregationBySignal15Minute.BinsContainers.FirstOrDefault().SumValue == options2.GetSignalSumDataPoint(options.Signals.FirstOrDefault()));

        }

    }

}