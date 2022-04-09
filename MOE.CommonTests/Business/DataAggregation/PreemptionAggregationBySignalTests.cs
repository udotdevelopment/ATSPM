using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.DataAggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.Bins;
using MOE.Common.Business.FilterExtensions;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Models;

namespace MOE.Common.Business.DataAggregation.Tests
{
    [TestClass()]
    public class PreemptionAggregationBySignalTests
    {
        public InMemoryMOEDatabase Db = new InMemoryMOEDatabase();
        public ISignalsRepository SignalsRepository;

        public PreemptionAggregationBySignalTests()
        {
            Db.ClearTables();
            Db.PopulateSignal();
            var signals = Db.Signals;
            foreach (var signal in signals)
            {
                 Db.PopulatePreemptionAggregationsWithValue3(Convert.ToDateTime("1/1/2016"), Convert.ToDateTime("1/1/2018"), signal);
                Db.PopulateSignalEventCountwithRandomValues(Convert.ToDateTime("1/1/2016"), Convert.ToDateTime("1/1/2018"), signal);
            }
            SignalEventCountAggregationRepositoryFactory.SetRepository(new InMemorySignalEventCountAggregationRepository(Db));
            PreemptAggregationDatasRepositoryFactory.SetArchivedMetricsRepository(new InMemoryPreemptAggregationDatasRepository(Db));
            MOE.Common.Models.Repositories.SignalsRepositoryFactory.SetSignalsRepository(new InMemorySignalsRepository(Db));
            MetricTypeRepositoryFactory.SetMetricsRepository(new InMemoryMetricTypeRepository(Db));
            ApplicationEventRepositoryFactory.SetApplicationEventRepository(new InMemoryApplicationEventRepository(Db));
            Models.Repositories.DirectionTypeRepositoryFactory.SetDirectionsRepository(new InMemoryDirectionTypeRepository());
            SignalsRepository = SignalsRepositoryFactory.Create();
        }

        [TestMethod()]
        public void PreemptionAggregationBySignalSartToEnd15MinuteBinAllDayTypesTest()
        {
            PreemptionAggregationOptions options = new PreemptionAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedAggregationType =AggregationType.Sum;
            options.SelectedXAxisType = XAxisType.Time;
            options.ShowEventCount = true;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.FilterSignals.Add(new FilterSignal{SignalId = "102", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "103", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "104", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            
            var preemptionDatas = options.AggregatedDataTypes;
            foreach (var preemptionData in preemptionDatas)
            {
                options.SelectedAggregatedDataType = preemptionData;
                PreemptionAggregationBySignal preemptionAggregationBySignal =
                    new PreemptionAggregationBySignal(options, options.Signals[0]);

                //Test Signal Level Stats
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    var signalBinsContainer = preemptionAggregationBySignal.BinsContainers[i];
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
                Assert.IsTrue(preemptionAggregationBySignal.Total == 288);
                Assert.IsTrue(preemptionAggregationBySignal.Average == 3);
            }
        }

        [TestMethod()]
        public void SignalPreemptionAggregationBySignalTimePeriod15MinuteBinAllDayTypesTest()
        {
            PreemptionAggregationOptions options = new PreemptionAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedAggregationType =AggregationType.Sum;
            options.SelectedXAxisType = XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 0, 8, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.FilterSignals.Add(new FilterSignal{SignalId = "102", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "103", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "104", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions); var preemptionDatas = options.AggregatedDataTypes;
            foreach (var preemptionData in preemptionDatas)
            {
                options.SelectedAggregatedDataType = preemptionData;
                PreemptionAggregationBySignal preemptionAggregationBySignal =
                    new PreemptionAggregationBySignal(options, options.Signals[0]);

                //Test Signal Level Stats
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    var signalBinsContainer = preemptionAggregationBySignal.BinsContainers[i];
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
                Assert.IsTrue(preemptionAggregationBySignal.Total == 12);
                Assert.IsTrue(preemptionAggregationBySignal.Average == 3);
            }
        }


        [TestMethod()]
        public void PreemptionAggregationBySignalSartToEnd30MinuteBinAllDayTypesTest()
        {
            PreemptionAggregationOptions options = new PreemptionAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedAggregationType =AggregationType.Sum;
            options.SelectedXAxisType = XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, new List<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday },
                BinFactoryOptions.BinSize.ThirtyMinute,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.FilterSignals.Add(new FilterSignal{SignalId = "102", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "103", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "104", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            var preemptionDatas = options.AggregatedDataTypes;
            foreach (var preemptionData in preemptionDatas)
            {
                options.SelectedAggregatedDataType = preemptionData;
                PreemptionAggregationBySignal preemptionAggregationBySignal =
                    new PreemptionAggregationBySignal(options, options.Signals[0]);

                //Test Signal Level Stats
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    var signalBinsContainer = preemptionAggregationBySignal.BinsContainers[i];
                    Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                    Assert.IsTrue(signalBinsContainer.SumValue == 288);
                    Assert.IsTrue(signalBinsContainer.AverageValue == 6);
                    for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                    {
                        var bin = binsContainer.Bins[binIndex];
                        var signalBin = signalBinsContainer.Bins[binIndex];
                        Assert.IsTrue(bin.Start == signalBin.Start);
                        Assert.IsTrue(bin.End == signalBin.End);
                        Assert.IsTrue(signalBin.Sum == 6);
                    }
                }
                Assert.IsTrue(preemptionAggregationBySignal.Total == 288);
                Assert.IsTrue(preemptionAggregationBySignal.Average == 6);
            }
        }

        [TestMethod()]
        public void PreemptionAggregationBySignalTimePeriod30MinuteBinAllDayTypesTest()
        {
            PreemptionAggregationOptions options = new PreemptionAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedAggregationType =AggregationType.Sum;
            options.SelectedXAxisType = XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 0, 8, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSize.ThirtyMinute,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.FilterSignals.Add(new FilterSignal{SignalId = "102", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "103", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "104", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            var preemptionDatas = options.AggregatedDataTypes;
            foreach (var preemptionData in preemptionDatas)
            {
                options.SelectedAggregatedDataType = preemptionData;
                PreemptionAggregationBySignal splitAggregationBySignal =
                    new PreemptionAggregationBySignal(options, options.Signals[0]);

                //Test Signal Level Stats
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                    Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                    Assert.IsTrue(signalBinsContainer.SumValue == 12);
                    Assert.IsTrue(signalBinsContainer.AverageValue == 6);
                    for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                    {
                        var bin = binsContainer.Bins[binIndex];
                        var signalBin = signalBinsContainer.Bins[binIndex];
                        Assert.IsTrue(bin.Start == signalBin.Start);
                        Assert.IsTrue(bin.End == signalBin.End);
                        Assert.IsTrue(signalBin.Sum == 6);
                    }
                }
                Assert.IsTrue(splitAggregationBySignal.Total == 12);
                Assert.IsTrue(splitAggregationBySignal.Average == 6);
            }
        }

        [TestMethod()]
        public void PreemptionAggregationBySignalSartToEndHourMinuteBinAllDayTypesTest()
        {
            PreemptionAggregationOptions options = new PreemptionAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedAggregationType =AggregationType.Sum;
            options.SelectedXAxisType = XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.FilterSignals.Add(new FilterSignal{SignalId = "102", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "103", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "104", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            var preemptionDatas = options.AggregatedDataTypes;
            foreach (var preemptionData in preemptionDatas)
            {
                options.SelectedAggregatedDataType = preemptionData;
                PreemptionAggregationBySignal splitAggregationBySignal =
                    new PreemptionAggregationBySignal(options, options.Signals[0]);

                //Test Signal Level Stats
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                    Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                    Assert.IsTrue(signalBinsContainer.SumValue == 288);
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
                Assert.IsTrue(splitAggregationBySignal.Total == 288);
                Assert.IsTrue(splitAggregationBySignal.Average == 12);
            }
        }

        [TestMethod()]
        public void PreemptionAggregationBySignalTimePeriodHourBinAllDayTypesTest()
        {
            PreemptionAggregationOptions options = new PreemptionAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedAggregationType =AggregationType.Sum;
            options.SelectedXAxisType = XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                6, 0, 10, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSize.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.FilterSignals.Add(new FilterSignal{SignalId = "102", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "103", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "104", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            var preemptionDatas = options.AggregatedDataTypes;
            foreach (var preemptionData in preemptionDatas)
            {
                options.SelectedAggregatedDataType = preemptionData;
                PreemptionAggregationBySignal splitAggregationBySignal =
                    new PreemptionAggregationBySignal(options, options.Signals[0]);

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
                Assert.IsTrue(splitAggregationBySignal.Total == 48);
                Assert.IsTrue(splitAggregationBySignal.Average == 12);
            }
        }

        [TestMethod()]
        public void PreemptionAggregationBySignalSartToEndDayBinAllDayTypesTest()
        {
            PreemptionAggregationOptions options = new PreemptionAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("11/1/2017");
            options.SelectedAggregationType =AggregationType.Sum;
            options.SelectedXAxisType = XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("11/1/2017"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.FilterSignals.Add(new FilterSignal{SignalId = "102", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "103", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "104", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            var preemptionDatas = options.AggregatedDataTypes;
            foreach (var preemptionData in preemptionDatas)
            {
                options.SelectedAggregatedDataType = preemptionData;
                PreemptionAggregationBySignal splitAggregationBySignal =
                    new PreemptionAggregationBySignal(options, options.Signals[0]);

                //Test Signal Level Stats
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                    Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                    Assert.IsTrue(signalBinsContainer.SumValue == 9216);
                    //TODO:Last bucket is zero. Check end date comparison 
                    Assert.IsTrue(signalBinsContainer.AverageValue == 288);
                    for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                    {
                        var bin = binsContainer.Bins[binIndex];
                        var signalBin = signalBinsContainer.Bins[binIndex];
                        Assert.IsTrue(bin.Start == signalBin.Start);
                        Assert.IsTrue(bin.End == signalBin.End);
                        Assert.IsTrue(signalBin.Sum == 288);
                    }
                }
                Assert.IsTrue(splitAggregationBySignal.Total == 9216);
                Assert.IsTrue(splitAggregationBySignal.Average == 288);
            }
        }

        [TestMethod()]
        public void PreemptionAggregationBySignalTimePeriodDayBinAllDayTypesTest()
        {
            PreemptionAggregationOptions options = new PreemptionAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/1/2017");
            options.EndDate = Convert.ToDateTime("11/1/2017");
            options.SelectedAggregationType =AggregationType.Sum;
            options.SelectedXAxisType = XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017"),
                Convert.ToDateTime("11/1/2017"),
                6, 0, 10, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSize.Day,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.FilterSignals.Add(new FilterSignal{SignalId = "102", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "103", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "104", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            var preemptionDatas = options.AggregatedDataTypes;
            foreach (var preemptionData in preemptionDatas)
            {
                options.SelectedAggregatedDataType = preemptionData;
                PreemptionAggregationBySignal splitAggregationBySignal =
                    new PreemptionAggregationBySignal(options, options.Signals[0]);

                //Test Signal Level Stats
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                    Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                    Assert.IsTrue(signalBinsContainer.SumValue == 1536);
                    //TODO:Last bucket is zero. Check end date comparison 
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
                Assert.IsTrue(splitAggregationBySignal.Total == 1536);
                Assert.IsTrue(splitAggregationBySignal.Average == 48);
            }
        }

        [TestMethod()]
        public void PreemptionAggregationBySignalSartToEndMonthBinAllDayTypesTest()
        {
            PreemptionAggregationOptions options = new PreemptionAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("1/1/2018");
            options.SelectedAggregationType =AggregationType.Sum;
            options.SelectedXAxisType = XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("1/1/2018"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.FilterSignals.Add(new FilterSignal{SignalId = "102", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "103", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "104", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            var preemptionDatas = options.AggregatedDataTypes;
            foreach (var preemptionData in preemptionDatas)
            {
                options.SelectedAggregatedDataType = preemptionData;
                PreemptionAggregationBySignal splitAggregationBySignal =
                    new PreemptionAggregationBySignal(options, options.Signals[0]);

                //Test Signal Level Stats
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                    Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                    Assert.IsTrue(signalBinsContainer.SumValue == 105120);
                    Assert.IsTrue(signalBinsContainer.AverageValue == 8760);
                    for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                    {
                        var bin = binsContainer.Bins[binIndex];
                        var signalBin = signalBinsContainer.Bins[binIndex];
                        Assert.IsTrue(bin.Start == signalBin.Start);
                        Assert.IsTrue(bin.End == signalBin.End);
                        Assert.IsTrue(signalBin.Sum == 288 * DateTime.DaysInMonth(bin.Start.Year, bin.Start.Month));
                    }
                }
                Assert.IsTrue(splitAggregationBySignal.Total == 105120);
                Assert.IsTrue(splitAggregationBySignal.Average == 8760);
            }
        }

        [TestMethod()]
        public void PreemptionAggregationBySignalTimePeriodMonthBinAllDayTypesTest()
        {
            PreemptionAggregationOptions options = new PreemptionAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("1/1/2018");
            options.SelectedAggregationType =AggregationType.Sum;
            options.SelectedXAxisType = XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("1/1/2018"),
                7, 0, 8, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSize.Month,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.FilterSignals.Add(new FilterSignal{SignalId = "102", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "103", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "104", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            var preemptionDatas = options.AggregatedDataTypes;
            foreach (var preemptionData in preemptionDatas)
            {
                options.SelectedAggregatedDataType = preemptionData;
                PreemptionAggregationBySignal splitAggregationBySignal =
                    new PreemptionAggregationBySignal(options, options.Signals[0]);

                //Test Signal Level Stats
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                    Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                    Assert.IsTrue(signalBinsContainer.SumValue ==
                                  12 * DateTime.DaysInMonth(binsContainers[i].Start.Year,
                                      binsContainers[i].Start.Month));
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
                Assert.IsTrue(splitAggregationBySignal.Total == 4380);
                Assert.IsTrue(splitAggregationBySignal.Average == 365);
            }
        }

        [TestMethod()]
        public void PreemptionAggregationBySignalSartToEndYearBinAllDayTypesTest()
        {
            PreemptionAggregationOptions options = new PreemptionAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("1/1/2018");
            options.SelectedAggregationType =AggregationType.Sum;
            options.SelectedXAxisType = XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("1/1/2018"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.FilterSignals.Add(new FilterSignal{SignalId = "102", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "103", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "104", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            var preemptionDatas = options.AggregatedDataTypes;
            foreach (var preemptionData in preemptionDatas)
            {
                options.SelectedAggregatedDataType = preemptionData;
                PreemptionAggregationBySignal splitAggregationBySignal =
                    new PreemptionAggregationBySignal(options, options.Signals[0]);

                //Test Signal Level Stats
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                    Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                    Assert.IsTrue(signalBinsContainer.SumValue == 210528);
                    Assert.IsTrue(signalBinsContainer.AverageValue == 105264);
                    for (int binIndex = 0; binIndex < binsContainer.Bins.Count; binIndex++)
                    {
                        var bin = binsContainer.Bins[binIndex];
                        var signalBin = signalBinsContainer.Bins[binIndex];
                        Assert.IsTrue(bin.Start == signalBin.Start);
                        Assert.IsTrue(bin.End == signalBin.End);
                        if (DateTime.IsLeapYear(bin.Start.Year))
                        {
                            Assert.IsTrue(signalBin.Sum == 105408);
                        }
                        else
                        {
                            Assert.IsTrue(signalBin.Sum == 105120);
                        }
                    }
                }
                Assert.IsTrue(splitAggregationBySignal.Total == 210528);
                Assert.IsTrue(splitAggregationBySignal.Average == 105264);
            }
        }

        [TestMethod()]
        public void PreemptionAggregationBySignalTimePeriodYearBinAllDayTypesTest()
        {
            PreemptionAggregationOptions options = new PreemptionAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2016");
            options.EndDate = Convert.ToDateTime("1/1/2018");
            options.SelectedAggregationType =AggregationType.Sum;
            options.SelectedXAxisType = XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("1/1/2018"),
                7, 0, 8, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                BinFactoryOptions.BinSize.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.FilterSignals.Add(new FilterSignal{SignalId = "102", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "103", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("103"));
            options.FilterSignals.Add(new FilterSignal{SignalId = "104", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("104"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            var preemptionDatas = options.AggregatedDataTypes;
            foreach (var preemptionData in preemptionDatas)
            {
                options.SelectedAggregatedDataType = preemptionData;
                PreemptionAggregationBySignal splitAggregationBySignal =
                    new PreemptionAggregationBySignal(options, options.Signals[0]);

                //Test Signal Level Stats
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    var binsContainer = binsContainers[i];
                    var signalBinsContainer = splitAggregationBySignal.BinsContainers[i];
                    Assert.IsTrue(binsContainer.Start == signalBinsContainer.Start);
                    Assert.IsTrue(binsContainer.End == signalBinsContainer.End);
                    if (DateTime.IsLeapYear(binsContainer.Start.Year))
                    {
                        Assert.IsTrue(signalBinsContainer.SumValue == 4392);
                    }
                    else
                    {
                        Assert.IsTrue(signalBinsContainer.SumValue == 4380);
                    }
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
                Assert.IsTrue(splitAggregationBySignal.Total == 8772);
                Assert.IsTrue(splitAggregationBySignal.Average == 4386);
            }
        }

       

        [TestMethod()]
        public void PreemptionAggregationBySignalTestByPhaseTimePeriod()
        {
            PreemptionAggregationOptions options = new PreemptionAggregationOptions();
            options.StartDate = Convert.ToDateTime("10/17/2017");
            options.EndDate = Convert.ToDateTime("10/18/2017");
            options.SelectedAggregationType =AggregationType.Sum;
            //options.XAxisAggregationSeriesOption = ApproachAggregationMetricOptions.XAxisAggregationSeriesOptions.SignalByPhase;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017"),
                Convert.ToDateTime("10/18/2017"),
                7, 0, 8, 0, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday }, BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.TimePeriod);
            options.FilterSignals.Add(new FilterSignal{SignalId = "102", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            var preemptionDatas = options.AggregatedDataTypes;
            foreach (var preemptionData in preemptionDatas)
            {
                options.SelectedAggregatedDataType = preemptionData;
                PreemptionAggregationBySignal splitAggregationBySignal =
                    new PreemptionAggregationBySignal(options, options.Signals[0]);

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
                Assert.IsTrue(splitAggregationBySignal.Total == 12);
                Assert.IsTrue(splitAggregationBySignal.Average == 3);
            }
        }

       

       

        


        [TestMethod()]
        public void TestSumValuesOnDifferentBinSizesTest()
        {
            PreemptionAggregationOptions options = new PreemptionAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("1/1/2018");
            options.SelectedAggregationType =AggregationType.Sum;
            options.SelectedXAxisType = XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
                Convert.ToDateTime("1/1/2018"),
                null, null, null, null, null,
                BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.StartToEnd);
            options.FilterSignals.Add(new FilterSignal{SignalId = "102", Exclude = false});
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SelectedChartType = SeriesChartType.Column;
            var preemptionDatas = options.AggregatedDataTypes;
            foreach (var preemptionData in preemptionDatas)
            {
                options.SelectedAggregatedDataType = preemptionData;
                PreemptionAggregationBySignal splitAggregationBySignal15Minute =
                    new PreemptionAggregationBySignal(options, options.Signals[0]);
                double fifteenMinuteSum = 0;
                foreach (var binsContainer in splitAggregationBySignal15Minute.BinsContainers)
                {
                    fifteenMinuteSum += binsContainer.SumValue;
                }
                options.TimeOptions.SelectedBinSize = BinFactoryOptions.BinSize.ThirtyMinute;
                PreemptionAggregationBySignal splitAggregationBySignal30Minute =
                    new PreemptionAggregationBySignal(options, options.Signals[0]);
                double thirtyMinuteSum = 0;
                foreach (var binsContainer in splitAggregationBySignal30Minute.BinsContainers)
                {
                    thirtyMinuteSum += binsContainer.SumValue;
                }
                Assert.IsTrue(fifteenMinuteSum == thirtyMinuteSum);

                options.TimeOptions.SelectedBinSize = BinFactoryOptions.BinSize.Hour;
                PreemptionAggregationBySignal splitAggregationBySignalHour =
                    new PreemptionAggregationBySignal(options, options.Signals[0]);
                double hourSum = 0;
                foreach (var binsContainer in splitAggregationBySignalHour.BinsContainers)
                {
                    hourSum += binsContainer.SumValue;
                }
                Assert.IsTrue(fifteenMinuteSum == hourSum);

                options.TimeOptions.SelectedBinSize = BinFactoryOptions.BinSize.Day;
                PreemptionAggregationBySignal splitAggregationBySignalDay =
                    new PreemptionAggregationBySignal(options, options.Signals[0]);
                double daySum = 0;
                foreach (var binsContainer in splitAggregationBySignalDay.BinsContainers)
                {
                    daySum += binsContainer.SumValue;
                }
                Assert.IsTrue(fifteenMinuteSum == daySum);
                options.TimeOptions.SelectedBinSize = BinFactoryOptions.BinSize.Month;
                PreemptionAggregationBySignal splitAggregationBySignalMonth =
                    new PreemptionAggregationBySignal(options, options.Signals[0]);
                double monthSum = 0;
                foreach (var binsContainer in splitAggregationBySignalMonth.BinsContainers)
                {
                    monthSum += binsContainer.SumValue;
                }
                Assert.IsTrue(fifteenMinuteSum == monthSum);

                options.TimeOptions.SelectedBinSize = BinFactoryOptions.BinSize.Year;
                PreemptionAggregationBySignal splitAggregationBySignalYear =
                    new PreemptionAggregationBySignal(options, options.Signals[0]);
                double yearSum = 0;
                foreach (var binsContainer in splitAggregationBySignalYear.BinsContainers)
                {
                    yearSum += binsContainer.SumValue;
                }
                splitAggregationBySignalYear = null;
            }
        }

        [TestMethod()]
        public void TimeSumToSignalSumTest()
        {
            var signalsRepository = SignalsRepositoryFactory.Create();
            PreemptionAggregationOptions options = new PreemptionAggregationOptions();
            options.StartDate = Convert.ToDateTime("1/1/2017");
            options.EndDate = Convert.ToDateTime("1/2/2017");
            options.SelectedAggregationType =AggregationType.Sum;
            options.SelectedXAxisType = XAxisType.Time;
            options.TimeOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017"),
            Convert.ToDateTime("1/2/2017"),
            null, null, null, null, null,
            BinFactoryOptions.BinSize.FifteenMinute,
            BinFactoryOptions.TimeOptions.StartToEnd);
            options.FilterSignals.Add(new FilterSignal{SignalId = "102", Exclude = false});
            foreach (var signal in options.FilterSignals)
            {
                options.Signals.Add(signalsRepository.GetLatestVersionOfSignalBySignalID(signal.SignalId));
            }
            options.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
            options.SelectedChartType = SeriesChartType.Column;
            List<BinsContainer> binsContainers = BinFactory.GetBins(options.TimeOptions);
            var preemptionDatas = options.AggregatedDataTypes;
            foreach (var preemptionData in preemptionDatas)
            {
                options.SelectedAggregatedDataType = preemptionData;
                PreemptionAggregationBySignal splitAggregationBySignal15Minute =
                    new PreemptionAggregationBySignal(options, options.Signals[0]);


                PreemptionAggregationOptions options2 = new PreemptionAggregationOptions();
                options2.StartDate = Convert.ToDateTime("1/1/2017");
                options2.EndDate = Convert.ToDateTime("1/2/2017");
                options2.SelectedAggregationType = AggregationType.Sum;
                options2.SelectedXAxisType = XAxisType.Signal;
                options2.TimeOptions = new BinFactoryOptions(
                    Convert.ToDateTime("1/1/2017"),
                    Convert.ToDateTime("1/2/2017"),
                    null, null, null, null, null,
                    BinFactoryOptions.BinSize.FifteenMinute,
                    BinFactoryOptions.TimeOptions.StartToEnd);
                options2.FilterSignals.Add(new FilterSignal {SignalId = "102", Exclude = false});
                foreach (var signal in options2.FilterSignals)
                {
                    options2.Signals.Add(signalsRepository.GetLatestVersionOfSignalBySignalID(signal.SignalId));
                }
                options2.Signals.Add(SignalsRepository.GetLatestVersionOfSignalBySignalID("102"));
                options2.SelectedChartType = SeriesChartType.Column;
                //  Assert.IsTrue(splitAggregationBySignal15Minute.BinsContainers.FirstOrDefault().SumValue == options2.GetSignalSumDataPoint(options.Signals.FirstOrDefault()));
            }

        }

    }

}