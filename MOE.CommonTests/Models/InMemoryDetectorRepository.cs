using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public class InMemoryDetectorRepository : IDetectorRepository
    {
        private InMemoryMOEDatabase _db;

        public InMemoryDetectorRepository(InMemoryMOEDatabase db)
        {
            this._db = db;
        }

        public InMemoryDetectorRepository()
        {
            this._db = new InMemoryMOEDatabase();
        }

        public Detector Add(Detector Detector)
        {
            _db.Detectors.Add(Detector);
            return Detector;
        }

        public bool CheckReportAvialbility(string detectorID, int metricID)
        {
            throw new NotImplementedException();
        }

        public bool CheckReportAvialbilityByDetector(Detector gd, int metricID)
        {
            throw new NotImplementedException();
        }

        public Detector GetDetectorByDetectorID(string DetectorID)
        {
            var det = _db.Detectors.Where(d => d.DetectorID == DetectorID).FirstOrDefault();
            return det;
        }

        public Detector GetDetectorByID(int ID)
        {
            var det = _db.Detectors.Where(d => d.ID == ID).FirstOrDefault();
            return det;
        }

        public List<Detector> GetDetectorsBySignalID(string SignalID)
        {
            List < MOE.Common.Models.Detector > dets = _db.Signals.Find(s=> s.SignalID == SignalID).GetDetectorsForSignal();
            return dets;
        }

        public List<Detector> GetDetectorsBySignalIDAndMetricType(string SignalID, int MetricID)
        {
            throw new NotImplementedException();
        }

        public int GetMaximumDetectorChannel(int versionId)
        {
            int max = 0;
            var signal = _db.Signals.Where(s => s.VersionID == versionId).FirstOrDefault();
            if (signal != null)
            {
                var detectors = signal.GetDetectorsForSignal();
                if (detectors.Count() > 0)
                {
                    max = detectors.Max(g => g.DetChannel);
                }
            }
            return max;
        }

        public List<Detector> GetDetectorsByIds(List<int> excludedDetectorIds)
        {
            throw new NotImplementedException();
        }

        public void Remove(Detector Detector)
        {
            throw new NotImplementedException();
        }

        public void Remove(int ID)
        {
            throw new NotImplementedException();
        }

        public void Update(Detector Detector)
        {
            throw new NotImplementedException();
        }

        public Common.Models.SPM GetContext()
        {
            throw new NotImplementedException();
        }

        public List<Detector> GetDetectorsBySignalIdMovementTypeIdDirectionTypeId(string signalId, int directionTypeId, List<int> movementTypeIds)
        {
            throw new NotImplementedException();
        }
    }
}
