using System;
using System.Collections.Generic;
using System.Text;
using MOE.Common.Models;

namespace MOE.Common.ModelGenerator
{
    public static class ApproachGenerator
    {
        static int ApproachIndex = 1;
        public static List<Approach> GetApproaches(int numberOfApproachesToRetrieve, int versionId, string signalId, int protectedPhaseNumber, int? permissivePhaseNumber, int movementTypeId =1, int directionTypeId=1)
        {   
            List<Approach> approaches = new List<Approach>();
            for (int i = 1; i <= numberOfApproachesToRetrieve; i++)
            {
                approaches.Add(new Approach()
                {
                    VersionID = versionId,
                    ApproachID = ApproachIndex, 
                    Description = "Test Approach " + i,
                    SignalID = signalId, 
                    ProtectedPhaseNumber = protectedPhaseNumber,
                    PermissivePhaseNumber = permissivePhaseNumber, 
                    DirectionTypeID = directionTypeId, 
                    Detectors = DetectorGenerator.GetDetectors(5, ApproachIndex, movementTypeId, null)
                }
                    ) ;
                ApproachIndex++;
            }
            return approaches;
        }
    }
}
