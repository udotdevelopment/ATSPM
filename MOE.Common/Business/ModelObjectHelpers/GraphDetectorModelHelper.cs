using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.ModelObjectHelpers
{
    public class GraphDetectorModelHelper
    {
        public Models.Graph_Detectors Detector { get; set; }
        public int Phase { get; set; }
        public string Direction { get; set; }
        public int MovementDelay { get; set; }
        public int DecisionPoint { get; set; }
        public int MPH { get; set; }
        public bool IsOverlap { get; set; }
        public string SignalID { get; set; }
        public string LaneType { get; set; }
        public string LaneTypeAbbr { get; set; }

        public GraphDetectorModelHelper(MOE.Common.Models.Graph_Detectors Detector)
        {
            

        Phase = Detector.LaneGroup.ProtectedPhaseNumber;
        Direction = Detector.LaneGroup.Approach.DirectionType.Description;
        MovementDelay = Detector.LaneGroup.Approach.Movement_Delay.Value;
        DecisionPoint = Detector.LaneGroup.Approach.Decision_Point.Value;
        MPH = Detector.LaneGroup.Approach.MPH.Value;
        IsOverlap = Detector.LaneGroup.IsProtectedPhaseOverlap;
        SignalID = Detector.LaneGroup.Approach.SignalID;
        LaneType = Detector.LaneGroup.LaneGroupType.Description;
        LaneTypeAbbr = Detector.LaneGroup.LaneGroupType.Abbreviation;



        }

        public static Models.Graph_Detectors GetDetectorByLaneByDetectionType(Models.LaneGroup laneGroup, int detectionTypeID)
        {
            List<Models.Graph_Detectors> dets = new List<Models.Graph_Detectors>();
            
            foreach(Models.Graph_Detectors d in laneGroup.Detectors)
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
