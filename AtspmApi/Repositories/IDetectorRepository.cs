using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public interface IDetectorRepository
    {
        Models.AtspmApi GetContext();


        Detector GetDetectorByDetectorID(string DetectorID);
        List<Detector> GetDetectorsBySignalID(string SignalID);

        Detector GetDetectorByID(int ID);

        //List<AtspmApi.Models.Detectors> GetDetectorsBySignalIDAndPhase(string SignalID, int PhaseNumber);
        //List<Detector> GetDetectorsBySignalIDAndMetricType(string SignalID, int MetricID);

        Detector Add(Detector Detector);
        void Update(Detector Detector);
        void Remove(Detector Detector);
        void Remove(int ID);
        //bool CheckReportAvialbility(string detectorID, int metricID);
        //bool CheckReportAvialbilityByDetector(Detector gd, int metricID);
        //int GetMaximumDetectorChannel(int versionId);

        List<Detector> GetDetectorsByIds(List<int> excludedDetectorIds);
    }
}