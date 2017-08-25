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
        public String SignalDescription { get { return SignalID + " - " + PrimaryName + " " + SecondaryName; } }

        [NotMapped]
        public List<Models.Controller_Event_Log> PlanEvents {get; set;}       
        
        public void SetPlanEvents(DateTime startTime, DateTime endTime)
        {
                MOE.Common.Models.Repositories.IControllerEventLogRepository repository =
                    MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();
                PlanEvents = repository.GetSignalEventsByEventCode(SignalID, startTime, endTime, 131);
        }

        //public List<Models.Lane> GetLaneGroupsForSignal()
        //{
        //    List<Models.Lane> laneGroups = new List<Lane>();
        //    foreach (Models.Approach a in this.Approaches)
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
            else
            {
                return false;
            }
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
            else
            {
                return false;
            }
                
        }
       

        public static MOE.Common.Models.Signal CopySignal(MOE.Common.Models.Signal incommingSignal, string newSignalID)
        {
            Models.SPM db = new SPM();
            MOE.Common.Models.Signal newSignal = new Models.Signal();
            newSignal.IPAddress = "10.10.10.10";
            newSignal.PrimaryName = incommingSignal.PrimaryName;
            newSignal.SecondaryName = incommingSignal.SecondaryName;
            newSignal.Longitude = incommingSignal.Longitude;
            newSignal.Latitude = incommingSignal.Latitude;
            newSignal.RegionID = incommingSignal.RegionID;
            newSignal.ControllerTypeID = incommingSignal.ControllerTypeID;
            newSignal.Enabled = incommingSignal.Enabled;            
            newSignal.Approaches = new List<Models.Approach>();
            //Models.Repositories.ISignalsRepository signalRepository =
            //    Models.Repositories.SignalsRepositoryFactory.Create();
            //signalRepository.AddOrUpdate(newSignal);
            //Models.Repositories.IApproachRepository approachRepository =
            //   Models.Repositories.ApproachRepositoryFactory.Create();
            foreach (Models.Approach a in incommingSignal.Approaches)
            {
                Approach aForNewSignal = Models.Approach.CopyApproachForSignal(a.ApproachID); //this does the db.Save inside.
                newSignal.Approaches.Add(aForNewSignal);
                //approachRepository.AddOrUpdate(aForNewSignal);  
            }
            newSignal.SignalID = newSignalID;
            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (Exception ex)
            //{
            //    Models.Repositories.IApplicationEventRepository eventRepository =
            //        Models.Repositories.ApplicationEventRepositoryFactory.Create();
            //    ApplicationEvent error = new ApplicationEvent();
            //    error.ApplicationName = "MOE.Common";
            //    error.Class = "Models.Signal.cs";
            //    error.Function = "CopySignal";
            //    error.Description = ex.Message;
            //    error.SeverityLevel = ApplicationEvent.SeverityLevels.Medium;
            //    error.Timestamp = DateTime.Now;
            //    eventRepository.Add(error);
            //    throw;
            //}
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
