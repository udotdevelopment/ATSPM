using MOE.Common.Models;

using System.Collections.Generic;

namespace MOE.Common.ModelGenerator
{
    public static class DetectorGenerator
    {
        static int DetectorIndex = 1;

        public static List<Detector> GetDetectors(int numberOfDetectorsToRetrieve, int approachId, int movementTypeId, int? channel, int detectionHardwareId = 2)
        {
            List<Detector> detectors = new List<Detector>();
            for (int i = 1; i <= numberOfDetectorsToRetrieve; i++)
            {
                detectors.Add(new Detector()
                {
                    ID = DetectorIndex,
                    ApproachID = approachId,
                    MovementTypeID = movementTypeId,
                    LatencyCorrection = 1.2,
                    DetChannel = channel.HasValue ? channel.Value : i,
                    DetectionTypeIDs = new List<int>() { 1, 2, 3, 4, 5, 6, 7 },
                    DetectionHardwareID = detectionHardwareId
                }
                    );
                DetectorIndex++;
            }
            return detectors;
        }
    }
}
