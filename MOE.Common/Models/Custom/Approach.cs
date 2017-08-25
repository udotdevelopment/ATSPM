using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace MOE.Common.Models
{
    public partial class Approach
    {
        [NotMapped]
        public string Index { get; set; }
        [NotMapped]
        public string ApproachRouteDescription { get{
            return this.Signal.SignalDescription + " " +  
            this.DirectionType.Description + " Phase " + this.ProtectedPhaseNumber.ToString();} }
        public List<Detector> GetAllDetectorsOfDetectionType(int detectionTypeID)
        {
            if (Detectors != null)
            {
                foreach (Detector d in Detectors)
                {
                    if (d.DetectionTypeIDs == null)
                    {
                        d.DetectionTypeIDs = new List<int>();
                        foreach (DetectionType dt in d.DetectionTypes)
                        {
                            d.DetectionTypeIDs.Add(dt.DetectionTypeID);
                        }
                    }
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
                &&this.SignalID == approachToCompare.SignalID
                && this.ApproachID == approachToCompare.ApproachID
                && this.DirectionTypeID == approachToCompare.DirectionTypeID
                && this.Description == approachToCompare.Description
                && this.MPH == approachToCompare.MPH
                && this.Detectors == approachToCompare.Detectors
                && this.ProtectedPhaseNumber == approachToCompare.ProtectedPhaseNumber
                && this.IsProtectedPhaseOverlap == approachToCompare.IsProtectedPhaseOverlap
                && this.PermissivePhaseNumber == approachToCompare.PermissivePhaseNumber
                )
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static Models.Approach CreateNewApproachWithDefaultValues(Models.Signal signal, Models.DirectionType dir, MOE.Common.Models.SPM db)
        {
            Models.Approach appr = new Models.Approach();
            appr.Description = signal.SignalID + dir.Abbreviation;
            appr.DirectionTypeID = dir.DirectionTypeID;
            appr.SignalID = signal.SignalID;
            appr.MPH = 0;
            return appr;
        }

        public static void AddDefaultObjectsToApproach(Models.Approach appr, MOE.Common.Models.SPM db)
        {
            Models.Detector D = new Models.Detector();

            Models.DetectionType basic = (from r in db.DetectionTypes
                                          where r.DetectionTypeID == 1
                                          select r).FirstOrDefault();

            D.LaneTypeID = 1;
            D.MovementTypeID = 1;
            //D.Description = appr.Description + "Thru (Phase Only)";
            appr.Detectors.Add(D);

            //LeftTurn
            Models.Detector D2 = new Models.Detector();
            D2.LaneTypeID = 1;
            D2.MovementTypeID = 3;
            //LG2.Description = appr.Description + "Left (Phase Only)";      
            appr.Detectors.Add(D2);

        }

        public static Approach CopyApproachCommonProperties(Approach approachToCopy)
        {
            Models.Approach newApproach = new Models.Approach();
            newApproach.SignalID = approachToCopy.SignalID;
            newApproach.DirectionTypeID = approachToCopy.DirectionTypeID;
            newApproach.Description = approachToCopy.Description + " Copy";
            newApproach.MPH = approachToCopy.MPH;
            newApproach.ProtectedPhaseNumber = approachToCopy.ProtectedPhaseNumber;
            newApproach.IsProtectedPhaseOverlap = approachToCopy.IsProtectedPhaseOverlap;
            newApproach.PermissivePhaseNumber = approachToCopy.PermissivePhaseNumber;
            newApproach.Detectors = new List<MOE.Common.Models.Detector>();
            return newApproach;
        }

        public static Approach CopyApproach(int approachIDToCopy)
        {
            MOE.Common.Models.Repositories.IApproachRepository approachRepository =
                MOE.Common.Models.Repositories.ApproachRepositoryFactory.Create();
            Approach approachToCopy = approachRepository.GetApproachByApproachID(approachIDToCopy);
            Approach newApproach = CopyApproachCommonProperties(approachToCopy);
            foreach (Detector d in approachToCopy.Detectors)
            {
                Detector dForNewApproach = Detector.CopyDetector(d.ID, true); //need to increase DetChannel if not copying the whole signal.
                newApproach.Detectors.Add(dForNewApproach);
            }
            if (newApproach.Detectors.Count > 1)
            {
                newApproach = SetDetChannelWhenMultipleDetectorsExist(newApproach);
            }
            return newApproach;
        }

        private static Approach SetDetChannelWhenMultipleDetectorsExist(Approach newApproach)
        {
            int detChannel = newApproach.Detectors.ToList()[0].DetChannel + 1;
            for (int i = 1; i < newApproach.Detectors.Count; i++)
            {
                newApproach.Detectors.ToList()[i].DetChannel = detChannel;
                newApproach.Detectors.ToList()[i].DetectorID = newApproach.SignalID +
                    detChannel;
                detChannel++;
            }
            return newApproach;
        }

        public static Approach CopyApproachForSignal(int approachIDToCopy)
        {
            MOE.Common.Models.Repositories.IApproachRepository approachRepository =
                MOE.Common.Models.Repositories.ApproachRepositoryFactory.Create();
            Approach approachToCopy = approachRepository.GetApproachByApproachID(approachIDToCopy);
            Approach newApproach = CopyApproachCommonProperties(approachToCopy);
            foreach (Detector d in approachToCopy.Detectors)
            {
                Detector dForNewApproach = Detector.CopyDetector(d.ID, false);
                newApproach.Detectors.Add(dForNewApproach);
            }
            return newApproach;
        }


        public List<Detector> GetDetectorsForMetricType(int metricTypeID)
        {
            List<Detector> detectorsForMetricType = new List<Detector>();
            foreach (Detector d in this.Detectors)
            {
                if (d.DetectorSupportsThisMetric(metricTypeID))
                {
                    detectorsForMetricType.Add(d);
                    
                }
            }
            return detectorsForMetricType;
        }
    }
}
