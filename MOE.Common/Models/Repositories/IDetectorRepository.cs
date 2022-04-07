using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace MOE.Common.Models.Repositories
{
    public interface IDetectorRepository
    {
        SPM GetContext();


        Detector GetDetectorByDetectorID(string DetectorID);
        List<Detector> GetDetectorsBySignalID(string SignalID);

        List<Detector> GetDetectorsByApproachID(int approachID);
        Detector GetDetectorByID(int ID);

        //List<MOE.Common.Models.Detectors> GetDetectorsBySignalIDAndPhase(string SignalID, int PhaseNumber);
        List<Detector> GetDetectorsBySignalIDAndMetricType(string SignalID, int MetricID);

        Detector Add(Detector Detector);
        void Update(Detector Detector);
        void Remove(Detector Detector);
        void Remove(int ID);
        bool CheckReportAvialbility(string detectorID, int metricID);
        bool CheckReportAvialbilityByDetector(Detector gd, int metricID);
        int GetMaximumDetectorChannel(int versionId);

        List<Detector> GetDetectorsByIds(List<int> excludedDetectorIds);
    }
}