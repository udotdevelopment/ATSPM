using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IDetectorRepository
    {
        MOE.Common.Models.Detector GetDetectorByDetectorID(string DetectorID);
        List<MOE.Common.Models.Detector> GetDetectorsBySignalID(string SignalID);
        MOE.Common.Models.Detector GetDetectorByID(int ID);
        //List<MOE.Common.Models.Detectors> GetDetectorsBySignalIDAndPhase(string SignalID, int PhaseNumber);
        List<MOE.Common.Models.Detector> GetDetectorsBySignalIDAndMetricType(string SignalID, int MetricID);
        Detector Add(Models.Detector Detector);
        void Update(Models.Detector Detector);
        void Remove(Models.Detector Detector);
        void Remove(int ID);
        bool CheckReportAvialbility(string detectorID, int metricID);
        int GetMaximumDetectorChannel(string signalID);

    }
}
