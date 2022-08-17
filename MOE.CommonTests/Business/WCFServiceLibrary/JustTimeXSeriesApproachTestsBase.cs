using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Business.WCFServiceLibrary.Tests;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.WCFServiceLibrary.Tests
{
    public abstract class JustTimeXSeriesApproachTestsBase : ApproachAggregationCreateMetricTestsBase
    {
       

        public JustTimeXSeriesApproachTestsBase()
        {
            Db.ClearTables();
            Db.PopulateSignal();
            Db.PopulateSignalsWithApproaches();
            Db.PopulateApproachesWithDetectors();
            
            MOE.Common.Models.Repositories.SignalsRepositoryFactory.SetSignalsRepository(
                new InMemorySignalsRepository(Db));
            MetricTypeRepositoryFactory.SetMetricsRepository(new InMemoryMetricTypeRepository(Db));
            ApplicationEventRepositoryFactory.SetApplicationEventRepository(
                new InMemoryApplicationEventRepository(Db));
            

            MOE.Common.Models.Repositories.SignalEventCountAggregationRepositoryFactory.SetRepository
                (new InMemorySignalEventCountAggregationRepository(Db));

            MOE.Common.Models.Repositories.DirectionTypeRepositoryFactory.SetDirectionsRepository(
                new InMemoryDirectionTypeRepository());
            ApproachRepositoryFactory.SetApproachRepository(new InMemoryApproachRepository(Db));
            SetSpecificAggregateRepositoriesForTest();
        }

        protected override void PopulateApproachData(Approach approach)
        {
            throw new NotImplementedException();
        }

        protected override void SetSpecificAggregateRepositoriesForTest()
        {
            throw new NotImplementedException();
        }

        public virtual void CreateTimeMetricStartToFinishAllBinSizesAllAggregateDataTypesTest(ApproachAggregationMetricOptions options)
        {

            List<SeriesType> seriesTypes = new List<SeriesType>();

            seriesTypes.Add(SeriesType.Direction);
            seriesTypes.Add(SeriesType.PhaseNumber);


            setOptionDefaults(options);
         
                options.SelectedXAxisType = XAxisType.TimeOfDay;
                foreach (var seriesType in seriesTypes)
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
                                    Assert.IsTrue(options.ReturnList.Count == 2 || options.ReturnList.Count == 4);
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
}
