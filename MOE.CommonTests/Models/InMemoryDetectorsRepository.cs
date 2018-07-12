using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    class InMemoryDetectorRepository : IDetectorRepository
    {



        public Detector Add(Detector Detector)
        {
            throw new NotImplementedException();
        }

        public bool CheckReportAvialbility(string detectorID, int metricID)
        {
            throw new NotImplementedException();
        }

        public Detector GetDetectorByDetectorID(string DetectorID)
        {
            throw new NotImplementedException();
        }

        public Detector GetDetectorByID(int ID)
        {
            throw new NotImplementedException();
        }

        public List<Detector> GetDetectorsBySignalID(string SignalID)
        {
            throw new NotImplementedException();
        }

        public List<Detector> GetDetectorsBySignalIDAndMetricType(string SignalID, int MetricID)
        {
            throw new NotImplementedException();
        }

        public int GetMaximumDetectorChannel(string signalID)
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
    }
}
