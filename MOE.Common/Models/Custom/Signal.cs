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
        public string SignalDescription => SignalID + " - " + PrimaryName + " " + SecondaryName;

        [NotMapped]
        public List<Controller_Event_Log> PlanEvents { get; set; }

        [NotMapped]
        public List<Signal> VersionList { get; set; }

        [NotMapped]
        public DateTime FirstDate => Convert.ToDateTime("1/1/2011");

        [NotMapped]
        public string SelectListName
        {
            get
            {
                return Start.ToShortDateString() + " - " + Note;
            }
        }

        public void SetPlanEvents(DateTime startTime, DateTime endTime)
        {
            var repository =
                ControllerEventLogRepositoryFactory.Create();
            PlanEvents = repository.GetSignalEventsByEventCode(SignalID, startTime, endTime, 131);
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
            var metricTypesString = string.Empty;
            foreach (var metric in GetAvailableMetrics())
                metricTypesString += metric.MetricID + ",";

            if (!string.IsNullOrEmpty(metricTypesString))
                metricTypesString = metricTypesString.TrimEnd(',');
            return metricTypesString;
        }

        public string GetAreasString()
        {
            var areasString = string.Empty;
            foreach (var area in GetAreas())
                areasString += area.Id + ",";

            if (!string.IsNullOrEmpty(areasString))
                areasString = areasString.TrimEnd(',');
            else
                areasString = "null";

            return areasString;
        }

        public List<int> GetPhasesForSignal()
        {
            var phases = new List<int>();
            foreach (var a in Approaches)
            {
                if (a.PermissivePhaseNumber != null)
                    phases.Add(a.PermissivePhaseNumber.Value);
                phases.Add(a.ProtectedPhaseNumber);
            }
            return phases.Select(p => p).Distinct().ToList();
        }

        public string GetSignalLocation()
        {
            return PrimaryName + " @ " + SecondaryName;
        }


        public List<Detector> GetDetectorsForSignal()
        {
            var detectors = new List<Detector>();
            if (Approaches != null)
            {
                foreach (var a in Approaches.OrderBy(a => a.ProtectedPhaseNumber))
                foreach (var d in a.Detectors)
                    detectors.Add(d);
            }

            return detectors.OrderBy(d => d.DetectorID).ToList();
        }


        public List<Detector> GetDetectorsForSignalThatSupportAMetric(int MetricTypeID)
        {
            var gdr =
                DetectorRepositoryFactory.Create();
            var detectors = new List<Detector>();
            foreach (var d in GetDetectorsForSignal())
                if (gdr.CheckReportAvialbility(d.DetectorID, MetricTypeID))
                    detectors.Add(d);
            return detectors;
        }

        public Detector GetDetectorForSignalByChannel(int detectorChannel)
        {
            Detector returnDet = null;


            foreach (var a in Approaches)
                if (a.Detectors.Count > 0)
                    foreach (var det in a.Detectors)
                        if (det.DetChannel == detectorChannel)
                            returnDet = det;

            return returnDet;
        }

        public bool CheckReportAvailabilityForSignal(int MetricTypeID)
        {
            var gdr =
                DetectorRepositoryFactory.Create();
            var detectors = new List<Detector>();
            foreach (var d in GetDetectorsForSignal())
                if (gdr.CheckReportAvialbility(d.DetectorID, MetricTypeID))
                    detectors.Add(d);
            if (detectors.Count > 0)
                return true;
            return false;
        }

        public List<Detector> GetDetectorsForSignalThatSupportAMetricByApproachDirection(int MetricTypeID,
            string Direction)
        {
            var gdr =
                DetectorRepositoryFactory.Create();
            var detectors = new List<Detector>();
            foreach (var d in GetDetectorsForSignal())
                if (gdr.CheckReportAvialbility(d.DetectorID, MetricTypeID) &&
                    d.Approach.DirectionType.Description == Direction)
                    detectors.Add(d);
            return detectors;
        }

        public List<Detector> GetDetectorsForSignalThatSupportAMetricByPhaseNumber(int metricTypeId, int phaseNumber)
        {
            var gdr = DetectorRepositoryFactory.Create();
            var detectors = new List<Detector>();
            foreach (var d in GetDetectorsForSignal())
                if (gdr.CheckReportAvialbilityByDetector(d, metricTypeId) &&
                    (d.Approach.ProtectedPhaseNumber == phaseNumber || d.Approach.PermissivePhaseNumber == phaseNumber))
                    detectors.Add(d);
            return detectors;
        }

        public List<Detector> GetDetectorsForSignalByPhaseNumber(int phaseNumber)
        {
            var dets = new List<Detector>();
            foreach (var d in GetDetectorsForSignal())
                if (d.Approach.ProtectedPhaseNumber == phaseNumber || d.Approach.PermissivePhaseNumber == phaseNumber)
                    dets.Add(d);
            return dets;
        }

        public List<MetricType> GetAvailableMetricsVisibleToWebsite()
        {
//TODO: The list really should be filtered by active timestamp.  We Will do it if we have time. 
            var metRep =
                MetricTypeRepositoryFactory.Create();

            var sigRep = SignalsRepositoryFactory.Create();

            var versions = sigRep.GetAllVersionsOfSignalBySignalID(signalID);

            var availableMetrics = metRep.GetBasicMetrics();
            foreach (var version in versions)
                if (version.VersionActionId != 3)
                    foreach (var d in GetDetectorsForSignal())
                    foreach (var dt in d.DetectionTypes)
                        if (dt.DetectionTypeID != 1)
                            foreach (var m in dt.MetricTypes)
                                if (m.ShowOnWebsite&& !availableMetrics.Contains(m))
                                    availableMetrics.Add(m);
            //availableMetrics = availableMetrics.Distinct().OrderBy(m => m.DisplayOrder).ToList();
            //return availableMetrics.OrderBy(a => a.MetricID).ToList();
            return availableMetrics;
        }

        public List<MetricType> GetAvailableMetrics()
        {
            var repository =
                MetricTypeRepositoryFactory.Create();

            var availableMetrics = repository.GetBasicMetrics();
            foreach (var d in GetDetectorsForSignal())
            foreach (var dt in d.DetectionTypes)
                if (dt.DetectionTypeID != 1)
                    foreach (var m in dt.MetricTypes)
                        availableMetrics.Add(m);
            return availableMetrics.Distinct().ToList();
        }

        public List<Area> GetAreas()
        {
            var repository =
                AreaRepositoryFactory.Create();

            var areas = repository.GetListOfAreasForSignal(SignalID);
            return areas.ToList();
        }

        private List<MetricType> GetBasicMetrics()
        {
            var repository =
                MetricTypeRepositoryFactory.Create();
            return repository.GetBasicMetrics();
        }

        public bool Equals(Signal signalToCompare)
        {
            return CompareSignalProperties(signalToCompare);
        }

        private bool CompareSignalProperties(Signal signalToCompare)
        {
            if (signalToCompare != null
                && SignalID == signalToCompare.SignalID
                && PrimaryName == signalToCompare.PrimaryName
                && SecondaryName == signalToCompare.SecondaryName
                && IPAddress == signalToCompare.IPAddress
                && Latitude == signalToCompare.Latitude
                && Longitude == signalToCompare.Longitude
                && RegionID == signalToCompare.RegionID
                && ControllerTypeID == signalToCompare.ControllerTypeID
                && Enabled == signalToCompare.Enabled
                && Pedsare1to1 == signalToCompare.Pedsare1to1
                && Approaches.Count() == signalToCompare.Approaches.Count()
            )
                return true;
            return false;
        }

        public static Signal CopyVersion(Signal origVersion)
        {
            var signalRepository = Repositories.SignalsRepositoryFactory.Create();
            var newVersion = new Signal();
            CopyCommonSignalSettings(origVersion, newVersion);
            newVersion.SignalID = origVersion.SignalID;
            newVersion.IPAddress = newVersion.IPAddress;
            newVersion.Start = DateTime.Now;
            newVersion.Note = "Copy of " + origVersion.Note;
            newVersion.Comments = new List<MetricComment>();
            newVersion.VersionList = signalRepository.GetAllVersionsOfSignalBySignalID(newVersion.SignalID);
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
            newSignal.Pedsare1to1 = origSignal.Pedsare1to1;
            newSignal.Approaches = new List<Approach>();
            newSignal.JurisdictionId = origSignal.JurisdictionId;

            if (origSignal.Approaches != null)
                foreach (var a in origSignal.Approaches)
                {
                    var aForNewSignal =
                        Approach.CopyApproachForSignal(a); //this does the db.Save inside.
                    newSignal.Approaches.Add(aForNewSignal);
                }
        }

        public static Signal CopySignal(Signal origSignal, string newSignalID)
        {
            var newSignal = new Signal();

            CopyCommonSignalSettings(origSignal, newSignal);

            newSignal.SignalID = newSignalID;

            return newSignal;
        }

        public List<Approach> GetApproachesForSignalThatSupportMetric(int metricTypeID)
        {
            var approachesForMeticType = new List<Approach>();
            foreach (var a in Approaches)
            foreach (var d in a.Detectors)
                if (d.DetectorSupportsThisMetric(metricTypeID))
                {
                    approachesForMeticType.Add(a);
                    break;
                }
            //return approachesForMeticType;
            return approachesForMeticType.OrderBy(a => a.PermissivePhaseNumber).ThenBy(a => a.ProtectedPhaseNumber).ThenBy(a => a.DirectionType.Description)
                .ToList();
        }

        public List<DirectionType> GetAvailableDirections()
        {
            var directions = Approaches.Select(a => a.DirectionType).Distinct().ToList();
            return directions;
        }
    }
}