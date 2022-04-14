using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.Bins;
using MOE.Common.Business.FilterExtensions;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Models;

namespace MOE.CommonTests.Business.WCFServiceLibrary
{
    public abstract class SignalAggregationCreateMetricTestsBase
    {
        public InMemoryMOEDatabase Db = new InMemoryMOEDatabase();

        public SignalAggregationCreateMetricTestsBase()
        {
            Db.ClearTables();
            Db.PopulateSignal();
            Db.PopulateSignalsWithApproaches();
            Db.PopulateApproachesWithDetectors();
            MOE.Common.Models.Repositories.SignalsRepositoryFactory.SetSignalsRepository(new InMemorySignalsRepository(Db));
            MetricTypeRepositoryFactory.SetMetricsRepository(new InMemoryMetricTypeRepository(Db));
            ApplicationEventRepositoryFactory.SetApplicationEventRepository(new InMemoryApplicationEventRepository(Db));
            Common.Models.Repositories.DirectionTypeRepositoryFactory.SetDirectionsRepository(new InMemoryDirectionTypeRepository());
            ApproachRepositoryFactory.SetApproachRepository(new InMemoryApproachRepository(Db));
            

            MOE.Common.Models.Repositories.SignalEventCountAggregationRepositoryFactory.SetRepository
                (new InMemorySignalEventCountAggregationRepository(Db));
            SetSpecificAggregateRepositoriesForTest();

        }

        private List<SeriesType> approvedSeries;
        private List<XAxisType> approvedXaxis;

        protected void SetOptionDefaults(SignalAggregationMetricOptions options)
        {
            options.SeriesWidth = 3;
            SetFilterSignal(options);
            options.ShowEventCount = true;
        }


        public virtual void CreateTimeMetricStartToFinishAllBinSizesAllAggregateDataTypesTest(SignalAggregationMetricOptions options)
        {
            AddValidValuestoLists();
            SetOptionDefaults(options);
           
            foreach (var xAxisType in Enum.GetValues(typeof(XAxisType)).Cast<XAxisType>().ToList())
            {
                options.SelectedXAxisType = xAxisType;
                foreach (var seriesType in Enum.GetValues(typeof(SeriesType)).Cast<SeriesType>().ToList())
                {
                    options.SelectedSeries = seriesType;
                    foreach (var tempBinSize in Enum.GetValues(typeof(BinFactoryOptions.BinSize))
                        .Cast<BinFactoryOptions.BinSize>().ToList())
                    {
                        SetTimeOptionsBasedOnBinSize(options, tempBinSize);
                        options.TimeOptions.SelectedBinSize = tempBinSize;
                        foreach (var aggregatedDataType in options.AggregatedDataTypes)
                        {
                            options.SelectedAggregatedDataType = aggregatedDataType;
                            try
                            {
                                if (IsValidCombination(options))
                                {
                                    CreateStackedColumnChart(options);
                                    Assert.IsTrue(options.ReturnList.Count == 1);
                                }
                            }
                            catch (InvalidBinSizeException e)
                            {
                                Debug.WriteLine(e.Message);
                            }
                            options.ReturnList = new List<string>();
                        }
                    }
                }
            }
        }

        private void AddValidValuestoLists()
        {
            approvedSeries = new List<SeriesType>();
            approvedSeries.Add(SeriesType.Signal);
            approvedSeries.Add(SeriesType.Route);

            approvedXaxis = new List<XAxisType>();
            approvedXaxis.Add(XAxisType.Time);
            approvedXaxis.Add(XAxisType.TimeOfDay);
            approvedXaxis.Add(XAxisType.Signal);
        }

        private static void SetTimeOptionsBasedOnBinSize(SignalAggregationMetricOptions options,
            BinFactoryOptions.BinSize binSize)
        {
            if (binSize == BinFactoryOptions.BinSize.Day)
            {
                options.StartDate = Convert.ToDateTime("10/1/2017");
                options.EndDate = Convert.ToDateTime("11/1/2017");
                options.TimeOptions = new BinFactoryOptions(
                    Convert.ToDateTime("10/1/2017"),
                    Convert.ToDateTime("11/1/2017"),
                    null, null, null, null,
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
                    BinFactoryOptions.TimeOptions.StartToEnd);
            }
            else if (binSize == BinFactoryOptions.BinSize.Month)
            {
                options.StartDate = Convert.ToDateTime("1/1/2017");
                options.EndDate = Convert.ToDateTime("1/1/2018");
                options.TimeOptions = new BinFactoryOptions(
                    Convert.ToDateTime("1/1/2017"),
                    Convert.ToDateTime("1/1/2018"),
                    null, null, null, null,
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
                    BinFactoryOptions.TimeOptions.StartToEnd);
            }
            else if (binSize == BinFactoryOptions.BinSize.Year)
            {
                options.StartDate = Convert.ToDateTime("1/1/2016");
                options.EndDate = Convert.ToDateTime("1/1/2018");
                options.TimeOptions = new BinFactoryOptions(
                    Convert.ToDateTime("1/1/2016"),
                    Convert.ToDateTime("1/1/2018"),
                    null, null, null, null,
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
                    BinFactoryOptions.TimeOptions.StartToEnd);
            }
            else
            {
                options.StartDate = Convert.ToDateTime("10/17/2017");
                options.EndDate = Convert.ToDateTime("10/18/2017");
                options.TimeOptions = new BinFactoryOptions(
                    Convert.ToDateTime("10/17/2017"),
                    Convert.ToDateTime("10/18/2017"),
                    null, null, null, null,
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
                    BinFactoryOptions.TimeOptions.StartToEnd);
            }
            if (options.SelectedXAxisType == XAxisType.TimeOfDay)
            {
                options.TimeOptions.TimeOfDayStartHour = 7;
                options.TimeOptions.TimeOfDayStartMinute = 0;
                options.TimeOptions.TimeOfDayEndHour = 10;
                options.TimeOptions.TimeOfDayStartHour = 0;
            }
        }

        protected abstract void SetSpecificAggregateRepositoriesForTest();

        protected abstract void PopulateSignalData(Signal signal);


        protected void SetFilterSignal(SignalAggregationMetricOptions options)
        {
            List<FilterSignal> filterSignals = new List<FilterSignal>();
            var signals = Db.Signals.Take(2);
            foreach (var signal in signals)
            {
                var filterSignal = new FilterSignal {SignalId = signal.SignalID, Exclude = false};
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
                        filterApproach.FilterDetectors.Add(new FilterDetector
                        {
                            Id = detector.ID,
                            Description = String.Empty,
                            Exclude = false
                        });
                    }
                }
                filterSignals.Add(filterSignal);
            }
            options.FilterSignals = filterSignals;
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

        protected void CreateStackedColumnChart(SignalAggregationMetricOptions options)
        {
            try
            {
                options.SelectedChartType = SeriesChartType.StackedColumn;
                options.SelectedAggregationType = AggregationType.Sum;
                options.CreateMetric();
                //options.SelectedAggregationType = AggregationType.Average;
                //options.CreateMetric();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        protected bool IsValidCombination(SignalAggregationMetricOptions options)
        {


            if(!approvedSeries.Contains(options.SelectedSeries))
            return false;

            if (!approvedXaxis.Contains(options.SelectedXAxisType))
                return false;

            if (options.SelectedXAxisType == XAxisType.Approach)
                return false;

            if(options.SelectedSeries == SeriesType.Route && options.SelectedXAxisType == XAxisType.Signal)
                return false;

            if (options.SelectedXAxisType == XAxisType.TimeOfDay &&
                (options.TimeOptions.SelectedBinSize == BinFactoryOptions.BinSize.Day ||
                 options.TimeOptions.SelectedBinSize == BinFactoryOptions.BinSize.Month ||
                 options.TimeOptions.SelectedBinSize == BinFactoryOptions.BinSize.Year))
                return false;
            if (options.SelectedXAxisType == XAxisType.Direction && options.SelectedSeries == SeriesType.PhaseNumber)
                return false;
            if (options.SelectedXAxisType == XAxisType.Approach && options.SelectedSeries == SeriesType.Direction)
                return false;
            if (options.SelectedXAxisType == XAxisType.Detector)
                return false;
            if (options.SelectedSeries == SeriesType.Detector)
                return false;
            if (options.SelectedSeries == SeriesType.Direction)
                return false;
            if (options.SelectedSeries == SeriesType.Direction)
                return false;
            if ((options.SelectedXAxisType == XAxisType.Direction || options.SelectedXAxisType == XAxisType.Approach) &&
                (options.SelectedSeries == SeriesType.Signal || options.SelectedSeries == SeriesType.Route))
                return false;

            return true;
        }

        public void CreateAllCharts(SignalAggregationMetricOptions options)
        {
            options.SelectedChartType = SeriesChartType.Column;
            options.SelectedAggregationType = AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.Line;
            options.SelectedAggregationType = AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.Pie;
            options.SelectedAggregationType = AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType = AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationType.Average;
            options.CreateMetric();
            options.SelectedChartType = SeriesChartType.StackedArea;
            options.SelectedAggregationType = AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationType.Average;
            options.CreateMetric();

        }

    }
}