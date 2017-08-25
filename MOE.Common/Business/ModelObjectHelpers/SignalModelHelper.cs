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
        public List<Models.LaneGroup> LaneGroups;
        //public List<int> Phases;
        public List<Models.Graph_Detectors> Detectors;


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

        public List<Models.LaneGroup> GetLaneGroupsForSignal()
        {
            List<Models.LaneGroup> lgs = new List<Models.LaneGroup>();
            List<Models.Approach> apprs = GetApproachesForSignal();
            foreach (Models.Approach a in apprs)
            {
                foreach (Models.LaneGroup lg in a.LaneGroups)
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

            List<Models.LaneGroup> lgs = GetLaneGroupsForSignal();

            foreach (Models.LaneGroup lg in lgs)
            {

                phases.Add(lg.PermissivePhaseNumber.Value);
                phases.Add(lg.ProtectedPhaseNumber);

            }

            return phases;
        }

        public string GetSignalLocation()
        {
         string location = this.Signal.Primary_Name + " @ " + this.Signal.Secondary_Name;
         return location;
        }

        public List<Models.Graph_Detectors> GetDetectorsForSignal()
        {
            List<Models.Graph_Detectors> dets = new List<Models.Graph_Detectors>();

            List<Models.LaneGroup> laneGroups = GetLaneGroupsForSignal();

            foreach (Models.LaneGroup laneGroup in laneGroups)
            {

                foreach (Models.Graph_Detectors det in laneGroup.Detectors)
                {
                    dets.Add(det);
                }
            }

            return dets;
        }

        public Models.Graph_Detectors GetDetectorForSignalByChannel(int detectorChannel)
        {
            Models.Graph_Detectors returnDet = null;

            List<Models.LaneGroup> laneGroups = GetLaneGroupsForSignal();

            foreach (Models.LaneGroup laneGroup in laneGroups)
            {

                foreach (Models.Graph_Detectors det in laneGroup.Detectors)
                {
                    if (det.Det_Channel == detectorChannel)
                    {
                        returnDet = det;
                        
                    }
                }
            }

            return returnDet;

            
        }


        public List<Models.Graph_Detectors> GetDetectorsForSignalThatSupportAMetric(int MetricTypeID)
        {
            MOE.Common.Models.Repositories.IGraphDetectorRepository gdr = MOE.Common.Models.Repositories.GraphDetectorRepositoryFactory.Create();

            List<Models.Graph_Detectors> dets = new List<Models.Graph_Detectors>();

            foreach (Models.Graph_Detectors d in Detectors)
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

            List<Models.Graph_Detectors> dets = new List<Models.Graph_Detectors>();

            foreach (Models.Graph_Detectors d in Detectors)
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

        public List<Models.Graph_Detectors> GetDetectorsForSignalThatSupportAMetricByApproachDirection(int MetricTypeID, string Direction)
        {
            MOE.Common.Models.Repositories.IGraphDetectorRepository gdr = MOE.Common.Models.Repositories.GraphDetectorRepositoryFactory.Create();

            List<Models.Graph_Detectors> dets = new List<Models.Graph_Detectors>();

            foreach (Models.Graph_Detectors d in Detectors)
            {
                if (gdr.CheckReportAvialbility(d.DetectorID, MetricTypeID) && d.LaneGroup.Approach.DirectionType.Description == Direction)
                {
                    dets.Add(d);
                }
            }

            return dets;
        }

        public List<Models.Graph_Detectors> GetDetectorsForSignalThatSupportAMetricByPhaseNumber(int MetricTypeID, int PhaseNumber)
        {
            MOE.Common.Models.Repositories.IGraphDetectorRepository gdr = MOE.Common.Models.Repositories.GraphDetectorRepositoryFactory.Create();

            List<Models.Graph_Detectors> dets = new List<Models.Graph_Detectors>();

            foreach (Models.Graph_Detectors d in Detectors)
            {
                if (gdr.CheckReportAvialbility(d.DetectorID, MetricTypeID) && (d.LaneGroup.ProtectedPhaseNumber == PhaseNumber || d.LaneGroup.PermissivePhaseNumber == PhaseNumber))
                {
                    dets.Add(d);
                }
            }

            return dets;
        }

        public List<Models.Graph_Detectors> GetDetectorsForSignalByPhaseNumber( int PhaseNumber)
        {
            MOE.Common.Models.Repositories.IGraphDetectorRepository gdr = MOE.Common.Models.Repositories.GraphDetectorRepositoryFactory.Create();

            List<Models.Graph_Detectors> dets = new List<Models.Graph_Detectors>();

            foreach (Models.Graph_Detectors d in Detectors)
            {
                if (d.LaneGroup.ProtectedPhaseNumber == PhaseNumber || d.LaneGroup.PermissivePhaseNumber == PhaseNumber)
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
                    foreach (var lg in a.LaneGroups)
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

