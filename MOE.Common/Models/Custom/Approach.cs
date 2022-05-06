using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Models
{
    public partial class Approach
    {
        [NotMapped]
        public string Index { get; set; }

        [NotMapped]
        public string ApproachRouteDescription => Signal.SignalDescription + " " +
                                                  DirectionType.Description + " Phase " + ProtectedPhaseNumber;

        public List<Detector> GetAllDetectorsOfDetectionType(int detectionTypeID)
        {
            if (Detectors != null)
            {
                foreach (var d in Detectors)
                    if (d.DetectionTypeIDs == null)
                    {
                        d.DetectionTypeIDs = new List<int>();
                        foreach (var dt in d.DetectionTypes)
                            d.DetectionTypeIDs.Add(dt.DetectionTypeID);
                    }
                return Detectors
                    .Where(d => d.DetectionTypeIDs.Contains(detectionTypeID))
                    .ToList();
            }
            return new List<Detector>();
        }

        public bool Equals(Approach approachToCompare)
        {
            return CompareApproachProperties(approachToCompare);
        }

        private bool CompareApproachProperties(Approach approachToCompare)
        {
            if (approachToCompare != null
                && SignalID == approachToCompare.SignalID
                && ApproachID == approachToCompare.ApproachID
                && DirectionTypeID == approachToCompare.DirectionTypeID
                && Description == approachToCompare.Description
                && MPH == approachToCompare.MPH
                && Detectors == approachToCompare.Detectors
                && ProtectedPhaseNumber == approachToCompare.ProtectedPhaseNumber
                && IsProtectedPhaseOverlap == approachToCompare.IsProtectedPhaseOverlap
                && PermissivePhaseNumber == approachToCompare.PermissivePhaseNumber
                && PedestrianPhaseNumber == approachToCompare.PedestrianPhaseNumber
                && IsPedestrianPhaseOverlap == approachToCompare.IsPedestrianPhaseOverlap
                && PedestrianDetectors == approachToCompare.PedestrianDetectors
            )
                return true;
            return false;
        }

        public static Approach CreateNewApproachWithDefaultValues(Signal signal, DirectionType dir, SPM db)
        {
            var appr = new Approach();
            appr.Description = signal.SignalID + dir.Abbreviation;
            appr.DirectionTypeID = dir.DirectionTypeID;
            appr.SignalID = signal.SignalID;
            appr.MPH = 0;
            return appr;
        }

        public static void AddDefaultObjectsToApproach(Approach appr, SPM db)
        {
            var D = new Detector();

            var basic = (from r in db.DetectionTypes
                where r.DetectionTypeID == 1
                select r).FirstOrDefault();

            D.LaneTypeID = 1;
            D.MovementTypeID = 1;
            //D.Description = appr.Description + "Thru (Phase Only)";
            appr.Detectors.Add(D);

            //LeftTurn
            var D2 = new Detector();
            D2.LaneTypeID = 1;
            D2.MovementTypeID = 3;
            //LG2.Description = appr.Description + "Left (Phase Only)";      
            appr.Detectors.Add(D2);
        }

        public static Approach CopyApproachCommonProperties(Approach approachToCopy, bool isVersionOrSignalCopy)
        {
            var newApproach = new Approach();
            newApproach.SignalID = approachToCopy.SignalID;
            newApproach.VersionID = approachToCopy.VersionID;
            newApproach.DirectionTypeID = approachToCopy.DirectionTypeID;
            if (!isVersionOrSignalCopy)
                newApproach.Description = approachToCopy.Description + " Copy";
            else
                newApproach.Description = approachToCopy.Description;
            newApproach.MPH = approachToCopy.MPH;
            newApproach.ProtectedPhaseNumber = approachToCopy.ProtectedPhaseNumber;
            newApproach.IsProtectedPhaseOverlap = approachToCopy.IsProtectedPhaseOverlap;
            newApproach.IsPermissivePhaseOverlap = approachToCopy.IsPermissivePhaseOverlap;
            newApproach.PermissivePhaseNumber = approachToCopy.PermissivePhaseNumber;
            newApproach.PedestrianPhaseNumber = approachToCopy.PedestrianPhaseNumber;
            newApproach.IsPedestrianPhaseOverlap = approachToCopy.IsPedestrianPhaseOverlap;
            newApproach.PedestrianDetectors = approachToCopy.PedestrianDetectors;
            newApproach.Detectors = new List<Detector>();
            return newApproach;
        }

        public static Approach CopyApproach(int approachIDToCopy)
        {
            var approachRepository =
                ApproachRepositoryFactory.Create();
            var approachToCopy = approachRepository.GetApproachByApproachID(approachIDToCopy);
            var newApproach = CopyApproachCommonProperties(approachToCopy, false);
            if (approachToCopy.Detectors != null)
            {
                foreach (var d in approachToCopy.Detectors)
                {
                    var
                        dForNewApproach =
                            Detector.CopyDetector(d.ID,
                                true); //need to increase DetChannel if not copying the whole signal.
                    newApproach.Detectors.Add(dForNewApproach);
                }
                if (newApproach.Detectors.Count > 1)
                    newApproach = SetDetChannelWhenMultipleDetectorsExist(newApproach);
            }
            return newApproach;
        }

        private static Approach SetDetChannelWhenMultipleDetectorsExist(Approach newApproach)
        {
            var detChannel = newApproach.Detectors.ToList()[0].DetChannel + 1;
            for (var i = 1; i < newApproach.Detectors.Count; i++)
            {
                newApproach.Detectors.ToList()[i].DetChannel = detChannel;
                newApproach.Detectors.ToList()[i].DetectorID = newApproach.SignalID +
                                                               detChannel;
                detChannel++;
            }
            return newApproach;
        }

        public static Approach CopyApproachForSignal(Approach approachToCopy)
        {
            var newApproach = CopyApproachCommonProperties(approachToCopy, true);
            foreach (var d in approachToCopy.Detectors)
            {
                var dForNewApproach = Detector.CopyDetector(d.ID, false);
                newApproach.Detectors.Add(dForNewApproach);
            }
            return newApproach;
        }


        public List<Detector> GetDetectorsForMetricType(int metricTypeID)
        {
            var detectorsForMetricType = new List<Detector>();
            if (Detectors != null)
            {
                foreach (var d in Detectors)
                {
                    if (d.DetectorSupportsThisMetric(metricTypeID))
                    {
                        detectorsForMetricType.Add(d);
                    }
                }
            }
            return detectorsForMetricType;
        }
    }
}