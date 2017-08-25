using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.ModelObjectHelpers
{
    public class GraphDetectorModelHelper
    {
        public Models.Detector Detector { get; set; }
        public int Phase { get; set; }
        public string Direction { get; set; }
        public int MovementDelay { get; set; }
        public int DecisionPoint { get; set; }
        public int MPH { get; set; }
        public bool IsOverlap { get; set; }
        public string SignalID { get; set; }
        public string LaneType { get; set; }
        public string LaneTypeAbbr { get; set; }

        public GraphDetectorModelHelper(MOE.Common.Models.Detector Detector)
        {
            

        Phase = Detector.Lane.ProtectedPhaseNumber;
        Direction = Detector.Lane.Approach.DirectionType.Description;
        //TODO:ConfigChange
        //MovementDelay = Detector.Lane.Approach.Movement_Delay.Value;
        //DecisionPoint = Detector.Lane.Approach.Decision_Point.Value;
        MPH = Detector.Lane.Approach.MPH.Value;
        IsOverlap = Detector.Lane.IsProtectedPhaseOverlap;
        SignalID = Detector.Lane.Approach.SignalID;
        LaneType = Detector.Lane.LaneGroupType.Description;
        LaneTypeAbbr = Detector.Lane.LaneGroupType.Abbreviation;



        }

        public static Models.Detector GetDetectorByLaneByDetectionType(Models.Lane laneGroup, int detectionTypeID)
        {
            List<Models.Detector> dets = new List<Models.Detector>();
            
            foreach(Models.Detector d in laneGroup.Detectors)
            {
                foreach(Models.DetectionType m in d.DetectionTypes)
                {
                if(m.DetectionTypeID == detectionTypeID)   
                {
                    dets.Add(d);

                }

                }
            }
            if (dets.Count > 0)
            {
                return dets[0];
            }
            else
            {
                return null;
            }
        }
    }
}
