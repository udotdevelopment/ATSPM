using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Models
{
    public partial class Signal
    {
        [NotMapped]
        public String SignalDescription => SignalID + " - " + PrimaryName + " " + SecondaryName;

        [NotMapped]
        public List<Controller_Event_Log> PlanEvents {get; set;}       
        
        public void SetPlanEvents(DateTime startTime, DateTime endTime)
        {
                IControllerEventLogRepository repository =
                    ControllerEventLogRepositoryFactory.Create();
                PlanEvents = repository.GetSignalEventsByEventCode(SignalID, startTime, endTime, 131);
        }

        [NotMapped]
        public List<Signal> VersionList {
            get;

            set; }

        [NotMapped]
        public DateTime FirstDate
        {

            get { return Convert.ToDateTime("1/1/2011"); }
                
         }
         




        //public List<Models.Lane> GetLaneGroupsForSignal()
        //{
        //    List<Models.Lane> laneGroups = new List<Lane>();
        //    foreach (Models.Approach a in this.RouteSignals)
        //    {
        //        foreach (Models.Lane lg in a.Lanes)
        //        {
        //            laneGroups.Add(lg);
        //        }
        //    }
        //    return laneGroups;
        //    }

        public string GetMetricTypesString()
        {
            string metricTypesString = string.Empty;
            foreach (var metric in GetAvailableMetrics())
            {
                metricTypesString += metric.MetricID + ",";
            }

            if (!String.IsNullOrEmpty(metricTypesString))
            {
                metricTypesString =  metricTypesString.TrimEnd(',');
            }
            return metricTypesString;
        }

        public List<int> GetPhasesForSignal()
        {
            List<int> phases = new List<int>();
            foreach (Approach a in Approaches)
            {
                if (a.PermissivePhaseNumber != null)
                {
                    phases.Add(a.PermissivePhaseNumber.Value);
                }
                phases.Add(a.ProtectedPhaseNumber);                
            }
            return phases.Select(p => p).Distinct().ToList();
        }

        public string GetSignalLocation()
        {
            return PrimaryName + " @ " + SecondaryName;
        }

        [NotMapped]
        public string SelectListName
        {
            get
            {
                if (Start == DateTime.MaxValue || Start == Convert.ToDateTime("12/31/9999"))
                {
                    return "Current";
                }
                return Start.ToShortDateString() + " - " + Note;
            } 

        }


        public List<Detector> GetDetectorsForSignal()
        {
            List<Detector> detectors = new List<Detector>();
            foreach(Approach a in Approaches.OrderBy(a => a.ProtectedPhaseNumber))
            {
                foreach(Detector d in a.Detectors)
                {
                    
                    detectors.Add(d);
                }
            }
            return detectors.OrderBy(d => d.DetectorID).ToList();
        }


        public List<Detector> GetDetectorsForSignalThatSupportAMetric(int MetricTypeID)
        {
            IDetectorRepository gdr = 
                DetectorRepositoryFactory.Create();
            List<Detector> detectors = new List<Detector>();
            foreach (Detector d in GetDetectorsForSignal())
            {
                if (gdr.CheckReportAvialbility(d.DetectorID, MetricTypeID))
                {
                    detectors.Add(d);
                }
            }                
            return detectors;
        }

        public Detector GetDetectorForSignalByChannel(int detectorChannel)
        {
            Detector returnDet = null;


            foreach (Approach a in Approaches)
            {
                if (a.Detectors.Count > 0)
                {
                    foreach (Detector det in a.Detectors)
                    {
                        if (det.DetChannel == detectorChannel)
                        {
                            returnDet = det;

                        }
                    }
                }
            }

            return returnDet;


        }

        public bool CheckReportAvailabilityForSignal(int MetricTypeID)
        {
            IDetectorRepository gdr = 
                DetectorRepositoryFactory.Create();
            List<Detector> detectors = new List<Detector>();
            foreach (Detector d in GetDetectorsForSignal())
            {
                if (gdr.CheckReportAvialbility(d.DetectorID, MetricTypeID))
                {
                    detectors.Add(d);
                }
            }
            if(detectors.Count>0)
            {
                return true;
            }
            return false;
        }

        public List<Detector> GetDetectorsForSignalThatSupportAMetricByApproachDirection(int MetricTypeID, string Direction)
        {
            IDetectorRepository gdr = 
                DetectorRepositoryFactory.Create();
            List<Detector> detectors = new List<Detector>();
            foreach (Detector d in GetDetectorsForSignal())
            {
                if (gdr.CheckReportAvialbility(d.DetectorID, MetricTypeID) && d.Approach.DirectionType.Description == Direction)
                {
                    detectors.Add(d);
                }
            }
            return detectors;
        }

        public List<Detector> GetDetectorsForSignalThatSupportAMetricByPhaseNumber(int metricTypeId, int phaseNumber)
        {
            IDetectorRepository gdr = DetectorRepositoryFactory.Create();
            List<Detector> detectors = new List<Detector>();
            foreach (Detector d in GetDetectorsForSignal())
            {
                if (gdr.CheckReportAvialbilityByDetector(d, metricTypeId) && 
                    (d.Approach.ProtectedPhaseNumber == phaseNumber || d.Approach.PermissivePhaseNumber == phaseNumber))
                {
                    detectors.Add(d);
                }
            }
            return detectors;
        }

        public List<Detector> GetDetectorsForSignalByPhaseNumber( int phaseNumber)
        {
            List<Detector> dets = new List<Detector>();
            foreach (Detector d in GetDetectorsForSignal())
            {
                if (d.Approach.ProtectedPhaseNumber == phaseNumber || d.Approach.PermissivePhaseNumber == phaseNumber)
                {
                    dets.Add(d);
                }
            }
            return dets;
        }

        public List<MetricType> GetAvailableMetricsVisibleToWebsite()
        {
//TODO: The list really should be filtered by active timestamp.  We Will do it if we have time. 
            IMetricTypeRepository metRep =
                MetricTypeRepositoryFactory.Create();

            ISignalsRepository sigRep = SignalsRepositoryFactory.Create();

            List<Signal> versions = sigRep.GetAllVersionsOfSignalBySignalID(signalID);

            List<MetricType> availableMetrics = metRep.GetBasicMetrics();
            foreach (var version in versions)
            {

                if (version.VersionActionId != 3)
                {
                    foreach (var d in GetDetectorsForSignal())
                    {
                        foreach (var dt in d.DetectionTypes)
                        {
                            if (dt.DetectionTypeID != 1)
                            {
                                foreach (var m in dt.MetricTypes)
                                {
                                    if (m.ShowOnWebsite)
                                    {
                                        availableMetrics.Add(m);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return availableMetrics.Distinct().OrderBy(a => a.MetricID).ToList();
        }

        public List<MetricType> GetAvailableMetrics()
        {
            IMetricTypeRepository repository =
                MetricTypeRepositoryFactory.Create();

            List<MetricType> availableMetrics = repository.GetBasicMetrics();
            foreach(var d in GetDetectorsForSignal())
            {
                foreach(var dt in d.DetectionTypes)
                {
                    if (dt.DetectionTypeID != 1)
                    {
                        foreach (var m in dt.MetricTypes)
                        {
                            availableMetrics.Add(m);
                        }
                    }
                }
            }
            return availableMetrics.Distinct().ToList();
        }
            
        private List<MetricType> GetBasicMetrics()
        {
            IMetricTypeRepository repository =
                MetricTypeRepositoryFactory.Create();
            return repository.GetBasicMetrics();
        }
        public bool Equals(Signal signalToCompare)
        {
            return CompareSignalProperties(signalToCompare);
        }

        private bool CompareSignalProperties(Signal signalToCompare)
        {
            if(signalToCompare != null
                && SignalID == signalToCompare.SignalID
                && PrimaryName == signalToCompare.PrimaryName
                && SecondaryName == signalToCompare.SecondaryName
                && IPAddress == signalToCompare.IPAddress
                && Latitude == signalToCompare.Latitude
                && Longitude == signalToCompare.Longitude
                && RegionID == signalToCompare.RegionID
                && ControllerTypeID == signalToCompare.ControllerTypeID
                && Enabled == signalToCompare.Enabled
                && Approaches.Count() == signalToCompare.Approaches.Count()
                )
            {
                return true;
            }
            return false;
        }
        public static Signal CopyVersion(Signal origVersion)
        {
            Signal newVersion = new Signal();

            CopyCommonSignalSettings(origVersion, newVersion);

            newVersion.SignalID = origVersion.SignalID;

            return newVersion;
        }

        private static void CopyCommonSignalSettings(Signal origSignal, Signal newSignal)
        {
            newSignal.IPAddress = "10.10.10.10";
            newSignal.PrimaryName = origSignal.PrimaryName;
            newSignal.SecondaryName = origSignal.SecondaryName;
            newSignal.Longitude = origSignal.Longitude;
            newSignal.Latitude = origSignal.Latitude;
            newSignal.RegionID = origSignal.RegionID;
            newSignal.ControllerTypeID = origSignal.ControllerTypeID;
            newSignal.Enabled = origSignal.Enabled;
            newSignal.Approaches = new List<Approach>();

            if (origSignal.Approaches != null)
            {
                foreach (Approach a in origSignal.Approaches)
                {
                    Approach aForNewSignal =
                        Approach.CopyApproachForSignal(a.ApproachID); //this does the db.Save inside.
                    newSignal.Approaches.Add(aForNewSignal);

                }
            }
        }
        public static Signal CopySignal(Signal origSignal, string newSignalID)
        {
            
            Signal newSignal = new Signal();

            CopyCommonSignalSettings(origSignal, newSignal);

            newSignal.SignalID = newSignalID;

            return newSignal;
        }

        public List<Approach> GetApproachesForSignalThatSupportMetric(int metricTypeID)
        {
            List<Approach> approachesForMeticType = new List<Approach>();
            foreach(Approach a in Approaches)
            {
                foreach(Detector d in a.Detectors)
                {
                    if(d.DetectorSupportsThisMetric(metricTypeID))
                    {
                        approachesForMeticType.Add(a);
                        break;
                    }
                }
            }
            return approachesForMeticType.OrderBy(a => a.ProtectedPhaseNumber).ThenBy(a =>a.DirectionType.Description).ToList();
        }

        public List<DirectionType> GetAvailableDirections()
        {
            List<DirectionType> directions = Approaches.Select(a => a.DirectionType).Distinct().ToList();
            return directions;
        }
    }
}
