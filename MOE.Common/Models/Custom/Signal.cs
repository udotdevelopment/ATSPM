using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models
{
    public partial class Signal
    {
        [NotMapped]
        public String SignalDescription => SignalID + " - " + PrimaryName + " " + SecondaryName;

        [NotMapped]
        public List<Models.Controller_Event_Log> PlanEvents {get; set;}       
        
        public void SetPlanEvents(DateTime startTime, DateTime endTime)
        {
                MOE.Common.Models.Repositories.IControllerEventLogRepository repository =
                    MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();
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
            foreach (Models.Approach a in this.Approaches)
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
            return this.PrimaryName + " @ " + this.SecondaryName;
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


        public List<Models.Detector> GetDetectorsForSignal()
        {
            List<Models.Detector> detectors = new List<Models.Detector>();
            foreach(Models.Approach a in Approaches.OrderBy(a => a.ProtectedPhaseNumber))
            {
                foreach(Models.Detector d in a.Detectors)
                {
                    detectors.Add(d);
                }
            }
            return detectors.OrderBy(d => d.DetectorID).ToList();
        }


        public List<Models.Detector> GetDetectorsForSignalThatSupportAMetric(int MetricTypeID)
        {
            MOE.Common.Models.Repositories.IDetectorRepository gdr = 
                MOE.Common.Models.Repositories.DetectorRepositoryFactory.Create();
            List<Models.Detector> detectors = new List<Models.Detector>();
            foreach (Models.Detector d in GetDetectorsForSignal())
            {
                if (gdr.CheckReportAvialbility(d.DetectorID, MetricTypeID))
                {
                    detectors.Add(d);
                }
            }                
            return detectors;
        }

        public Models.Detector GetDetectorForSignalByChannel(int detectorChannel)
        {
            Models.Detector returnDet = null;


            foreach (Models.Approach a in Approaches)
            {
                if (a.Detectors.Count > 0)
                {
                    foreach (Models.Detector det in a.Detectors)
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
            MOE.Common.Models.Repositories.IDetectorRepository gdr = 
                MOE.Common.Models.Repositories.DetectorRepositoryFactory.Create();
            List<Models.Detector> detectors = new List<Detector>();
            foreach (Models.Detector d in GetDetectorsForSignal())
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

        public List<Models.Detector> GetDetectorsForSignalThatSupportAMetricByApproachDirection(int MetricTypeID, string Direction)
        {
            MOE.Common.Models.Repositories.IDetectorRepository gdr = 
                MOE.Common.Models.Repositories.DetectorRepositoryFactory.Create();
            List<Models.Detector> detectors = new List<Detector>();
            foreach (Models.Detector d in GetDetectorsForSignal())
            {
                if (gdr.CheckReportAvialbility(d.DetectorID, MetricTypeID) && d.Approach.DirectionType.Description == Direction)
                {
                    detectors.Add(d);
                }
            }
            return detectors;
        }

        public List<Models.Detector> GetDetectorsForSignalThatSupportAMetricByPhaseNumber(int MetricTypeID, int PhaseNumber)
        {
            MOE.Common.Models.Repositories.IDetectorRepository gdr = 
                MOE.Common.Models.Repositories.DetectorRepositoryFactory.Create();
            List<Models.Detector> detectors = new List<Models.Detector>();

            foreach (Models.Detector d in GetDetectorsForSignal())
            {
                if (gdr.CheckReportAvialbility(d.DetectorID, MetricTypeID) && 
                    (d.Approach.ProtectedPhaseNumber == PhaseNumber || d.Approach.PermissivePhaseNumber == PhaseNumber))
                {
                    detectors.Add(d);
                }
            }
            return detectors;
        }

        public List<Models.Detector> GetDetectorsForSignalByPhaseNumber( int PhaseNumber)
        {
            List<Models.Detector> dets = new List<Models.Detector>();
            foreach (Models.Detector d in GetDetectorsForSignal())
            {
                if (d.Approach.ProtectedPhaseNumber == PhaseNumber || d.Approach.PermissivePhaseNumber == PhaseNumber)
                {
                    dets.Add(d);
                }
            }
            return dets;
        }

        public List<Models.MetricType> GetAvailableMetricsVisibleToWebsite()
        {
            MOE.Common.Models.Repositories.IMetricTypeRepository repository =
                MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();

            List<Models.MetricType> availableMetrics = repository.GetBasicMetrics();
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
            return availableMetrics.Distinct().OrderBy(a => a.MetricID).ToList();
        }

        public List<Models.MetricType> GetAvailableMetrics()
        {
            MOE.Common.Models.Repositories.IMetricTypeRepository repository =
                MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();

            List<Models.MetricType> availableMetrics = repository.GetBasicMetrics();
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
            
        private List<Models.MetricType> GetBasicMetrics()
        {
            MOE.Common.Models.Repositories.IMetricTypeRepository repository =
                MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();
            return repository.GetBasicMetrics();
        }
        public bool Equals(Signal signalToCompare)
        {
            return CompareSignalProperties(signalToCompare);
        }

        private bool CompareSignalProperties(Signal signalToCompare)
        {
            if(signalToCompare != null
                && this.SignalID == signalToCompare.SignalID
                && this.PrimaryName == signalToCompare.PrimaryName
                && this.SecondaryName == signalToCompare.SecondaryName
                && this.IPAddress == signalToCompare.IPAddress
                && this.Latitude == signalToCompare.Latitude
                && this.Longitude == signalToCompare.Longitude
                && this.RegionID == signalToCompare.RegionID
                && this.ControllerTypeID == signalToCompare.ControllerTypeID
                && this.Enabled == signalToCompare.Enabled
                && this.Approaches.Count() == signalToCompare.Approaches.Count()
                )
            {
                return true;
            }
            return false;
        }
        public static Signal CopyVersion(Signal origVersion)
        {
            MOE.Common.Models.Signal newVersion = new Models.Signal();

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
            newSignal.Approaches = new List<Models.Approach>();

            foreach (Models.Approach a in origSignal.Approaches)
            {
                Approach aForNewSignal = Models.Approach.CopyApproachForSignal(a.ApproachID); //this does the db.Save inside.
                newSignal.Approaches.Add(aForNewSignal);

            }
        }
        public static MOE.Common.Models.Signal CopySignal(MOE.Common.Models.Signal origSignal, string newSignalID)
        {
            
            MOE.Common.Models.Signal newSignal = new Models.Signal();

            CopyCommonSignalSettings(origSignal, newSignal);

            newSignal.SignalID = newSignalID;

            return newSignal;
        }

        public List<Approach> GetApproachesForSignalThatSupportMetric(int metricTypeID)
        {
            List<Approach> approachesForMeticType = new List<Approach>();
            foreach(Approach a in this.Approaches)
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
    }
}
