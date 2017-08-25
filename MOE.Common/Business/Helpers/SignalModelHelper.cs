using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.ModelObjectHelpers
{
    public class SignalModelHelper
    {
        public Models.Signal Signal { get; set; }
        public List<Models.Approach> Approaches;
        public List<Models.Lane> LaneGroups;
        //public List<int> Phases;
        public List<Models.Detector> Detectors;


        public SignalModelHelper(String SignalID)
        {
            MOE.Common.Models.Repositories.ISignalsRepository signals = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            Signal = signals.GetSignalBySignalID(SignalID);
            Approaches = GetApproachesForSignal();
            LaneGroups = GetLaneGroupsForSignal();
            Detectors = GetDetectorsForSignal();
        }

        public SignalModelHelper(MOE.Common.Models.Signal signal)
        {
            Signal = signal;
            Approaches = GetApproachesForSignal();
            LaneGroups = GetLaneGroupsForSignal();
            Detectors = GetDetectorsForSignal();
        }

        public List<Models.Approach> GetApproachesForSignal()
        {
            List<Models.Approach> apprs = new List<Models.Approach>();
                foreach (Models.Approach a in Signal.Approaches)
                {
                    apprs.Add(a);
                }
            return apprs;
        }

        public List<Models.Lane> GetLaneGroupsForSignal()
        {
            List<Models.Lane> lgs = new List<Models.Lane>();
            List<Models.Approach> apprs = GetApproachesForSignal();
            foreach (Models.Approach a in apprs)
            {
                foreach (Models.Lane lg in a.Lanes)
                {
                    lgs.Add(lg);
                }
            }
            return lgs;
            }

        public string GetMetricTypesString()
        {
            string metricTypesString = string.Empty;
            foreach (var metric in GetAvailableMetrics())
            {
                metricTypesString += metric.MetricID.ToString() + ",";
            }
            if (!String.IsNullOrEmpty(metricTypesString))
            {
                metricTypesString.TrimEnd(',');
            }
            return metricTypesString;
        }

        public List<int> GetPhasesForSignal()
        {
            List<int> phases = new List<int>();

            List<Models.Lane> lgs = GetLaneGroupsForSignal();

            foreach (Models.Lane lg in lgs)
            {

                phases.Add(lg.PermissivePhaseNumber.Value);
                phases.Add(lg.ProtectedPhaseNumber);

            }

            return phases;
        }

        public string GetSignalLocation()
        {
         string location = this.Signal.PrimaryName + " @ " + this.Signal.SecondaryName;
         return location;
        }

        public List<Models.Detector> GetDetectorsForSignal()
        {
            List<Models.Detector> dets = new List<Models.Detector>();

            List<Models.Lane> laneGroups = GetLaneGroupsForSignal();

            foreach (Models.Lane laneGroup in laneGroups)
            {

                foreach (Models.Detector det in laneGroup.Detectors)
                {
                    dets.Add(det);
                }
            }

            return dets;
        }

        public Models.Detector GetDetectorForSignalByChannel(int detectorChannel)
        {
            Models.Detector returnDet = null;

            List<Models.Lane> laneGroups = GetLaneGroupsForSignal();

            foreach (Models.Lane laneGroup in laneGroups)
            {

                foreach (Models.Detector det in laneGroup.Detectors)
                {
                    if (det.DetChannel == detectorChannel)
                    {
                        returnDet = det;
                        
                    }
                }
            }

            return returnDet;

            
        }


        public List<Models.Detector> GetDetectorsForSignalThatSupportAMetric(int MetricTypeID)
        {
            MOE.Common.Models.Repositories.IGraphDetectorRepository gdr = MOE.Common.Models.Repositories.GraphDetectorRepositoryFactory.Create();

            List<Models.Detector> dets = new List<Models.Detector>();

            foreach (Models.Detector d in Detectors)
            {
                if (gdr.CheckReportAvialbility(d.DetectorID, MetricTypeID))
                {
                    dets.Add(d);
                }
            }

            return dets;
        }

        public bool CheckReportAvailabilityForSignal(int MetricTypeID)
        {
            MOE.Common.Models.Repositories.IGraphDetectorRepository gdr = MOE.Common.Models.Repositories.GraphDetectorRepositoryFactory.Create();

            List<Models.Detector> dets = new List<Models.Detector>();

            foreach (Models.Detector d in Detectors)
            {
                if (gdr.CheckReportAvialbility(d.DetectorID, MetricTypeID))
                {
                    dets.Add(d);
                }
            }

            if(dets.Count>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Models.Detector> GetDetectorsForSignalThatSupportAMetricByApproachDirection(int MetricTypeID, string Direction)
        {
            MOE.Common.Models.Repositories.IGraphDetectorRepository gdr = MOE.Common.Models.Repositories.GraphDetectorRepositoryFactory.Create();

            List<Models.Detector> dets = new List<Models.Detector>();

            foreach (Models.Detector d in Detectors)
            {
                if (gdr.CheckReportAvialbility(d.DetectorID, MetricTypeID) && d.Lane.Approach.DirectionType.Description == Direction)
                {
                    dets.Add(d);
                }
            }

            return dets;
        }

        public List<Models.Detector> GetDetectorsForSignalThatSupportAMetricByPhaseNumber(int MetricTypeID, int PhaseNumber)
        {
            MOE.Common.Models.Repositories.IGraphDetectorRepository gdr = MOE.Common.Models.Repositories.GraphDetectorRepositoryFactory.Create();

            List<Models.Detector> dets = new List<Models.Detector>();

            foreach (Models.Detector d in Detectors)
            {
                if (gdr.CheckReportAvialbility(d.DetectorID, MetricTypeID) && (d.Lane.ProtectedPhaseNumber == PhaseNumber || d.Lane.PermissivePhaseNumber == PhaseNumber))
                {
                    dets.Add(d);
                }
            }

            return dets;
        }

        public List<Models.Detector> GetDetectorsForSignalByPhaseNumber( int PhaseNumber)
        {
            MOE.Common.Models.Repositories.IGraphDetectorRepository gdr = MOE.Common.Models.Repositories.GraphDetectorRepositoryFactory.Create();

            List<Models.Detector> dets = new List<Models.Detector>();

            foreach (Models.Detector d in Detectors)
            {
                if (d.Lane.ProtectedPhaseNumber == PhaseNumber || d.Lane.PermissivePhaseNumber == PhaseNumber)
                {
                    dets.Add(d);
                }
            }

            return dets;
        }


        public static bool DoesSignalHaveDetection(string SignalID)
        {
            MOE.Common.Business.ModelObjectHelpers.SignalModelHelper smh = new Business.ModelObjectHelpers.SignalModelHelper(SignalID);

            if (smh.Detectors.Count > 0)
            {
                return true;
            }
            else { return false; }
        }



        public List<Models.MetricType> GetAvailableMetrics()
        {
            List<Models.MetricType> availableMetrics = new List<Models.MetricType>();

                foreach (var a in Signal.Approaches)
                {
                    foreach (var lg in a.Lanes)
                    {
                            foreach(var d in lg.Detectors)
                            {
                                foreach(var dt in d.DetectionTypes)
                                {
                                    foreach (var m in dt.MetricTypes)
                                    {
                                        availableMetrics.Add(m);
                                    }
                                }
                            }
                        
                    }                    
                }
            return availableMetrics.Distinct().ToList();
                }
            
        private List<Models.MetricType> GetBasicMetrics()
        {
            MOE.Common.Models.Repositories.IMetricTypeRepository repository =
                MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();
            return repository.GetBasicMetrics();
        }





    }
}

