using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.WCFServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.Bins;
using MOE.Common.Business.FilterExtensions;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Models;

namespace MOE.Common.Business.WCFServiceLibrary.Tests
{
    [TestClass()]
    public abstract class ApproachAggregationCreateMetricTestsBase
    {
        public InMemoryMOEDatabase Db = new InMemoryMOEDatabase();

        public ApproachAggregationCreateMetricTestsBase()
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
                    PopulateApproachData(approach);
                }
            }
            SetSpecificAggregateRepositoriesForTest();
            MOE.Common.Models.Repositories.SignalsRepositoryFactory.SetSignalsRepository(
                new InMemorySignalsRepository(Db));
            MetricTypeRepositoryFactory.SetMetricsRepository(new InMemoryMetricTypeRepository(Db));
            ApplicationEventRepositoryFactory.SetApplicationEventRepository(
                new InMemoryApplicationEventRepository(Db));
            Models.Repositories.DirectionTypeRepositoryFactory.SetDirectionsRepository(
                new InMemoryDirectionTypeRepository());
        }

        protected abstract void SetSpecificAggregateRepositoriesForTest();

        protected abstract void PopulateApproachData(Approach approach);

        protected abstract ApproachCycleAggregationOptions getOptionDefaults();


        protected void SetFilterSignal(ApproachAggregationMetricOptions options)
        {
            List<FilterSignal> filterSignals = new List<FilterSignal>();
            var signal = Db.Signals.FirstOrDefault();
            var filterSignal = new FilterSignal { SignalId = signal.SignalID, Exclude = false };
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
                    filterApproach.FilterDetectors.Add(new FilterDetector { Id = detector.ID, Description = String.Empty, Exclude = false });
                }
            }
            filterSignals.Add(filterSignal);
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

        protected void CreateStackedColumnChart(ApproachAggregationMetricOptions options)
        {
            options.SelectedChartType = SeriesChartType.StackedColumn;
            options.SelectedAggregationType = AggregationType.Sum;
            options.CreateMetric();
            options.SelectedAggregationType = AggregationType.Average;
            options.CreateMetric();
        }

        public void CreateAllCharts(ApproachCycleAggregationOptions options)
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