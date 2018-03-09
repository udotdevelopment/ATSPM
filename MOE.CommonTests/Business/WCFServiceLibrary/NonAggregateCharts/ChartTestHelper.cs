using MOE.Common.Models.Repositories;
using MOE.CommonTests.Helpers;
using MOE.CommonTests.Models;

namespace MOE.CommonTests.Business.WCFServiceLibrary.NonAggregateCharts
{
    public class ChartTestHelper
    {
        public static void InitializeTestDataFor7185Feb022018(InMemoryMOEDatabase db)
        {
            db.ClearTables();
            XmlToListImporter.LoadControllerEventLog("7185Events02_01_2018.Xml", db);
            XmlToListImporter.LoadSignals("signals.xml", db);
            XmlToListImporter.LoadApproaches("approachesfor7185.xml", db);
            XmlToListImporter.LoadDetectors("detectorsFor7185.xml", db);
            XmlToListImporter.AddDetectionTypesToDetectors
                ("DetectorTypesforDetectorsFor7185.xml", db);
            XmlToListImporter.AddDetectionTypesToMetricTypes("mtdt.xml", db);
            MOE.Common.Models.Repositories.SignalsRepositoryFactory.SetSignalsRepository(
                new InMemorySignalsRepository(db));
            MetricTypeRepositoryFactory.SetMetricsRepository(new InMemoryMetricTypeRepository(db));
            ApplicationEventRepositoryFactory.SetApplicationEventRepository(
                new InMemoryApplicationEventRepository(db));
            DirectionTypeRepositoryFactory.SetDirectionsRepository(
                new InMemoryDirectionTypeRepository());
            SpeedEventRepositoryFactory.SetSignalsRepository(new InMemorySpeedEventRepository(db));
            ApproachRepositoryFactory.SetApproachRepository(new InMemoryApproachRepository(db));
            ControllerEventLogRepositoryFactory.SetRepository(new InMemoryControllerEventLogRepository(db));
            DetectorRepositoryFactory.SetDetectorRepository(new InMemoryDetectorRepository(db));
            XmlToListImporter.LoadSpeedEvents("7185speed.xml", db);
        }
    }
}