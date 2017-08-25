using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOE.Common.Business
{
    public class SpeedExportApproachDirectionCollection
    {
        public List<SpeedExportApproachDirection> List = new List<SpeedExportApproachDirection>();
        public SpeedExportApproachDirectionCollection(DateTime startDate, DateTime endDate, string signalID,
            int binSize)
        {
            Models.Repositories.ISignalsRepository signalRepository =
                Models.Repositories.SignalsRepositoryFactory.Create();
            var signal = signalRepository.GetSignalBySignalID(signalID);
            List<Models.Detector> distinctPhases = signal.GetDetectorsForSignalThatSupportAMetric(10);
                 

            
             //If there are phases in the database add the charts
            if (distinctPhases.Count > 0)
            {
                foreach (Models.Detector detector in distinctPhases)
                    {
                        //Get the phase
                        int phase = detector.Approach.ProtectedPhaseNumber;
                        string direction = detector.Approach.DirectionType.Description;
                        int movementDelay = detector.MovementDelay.Value;
                        int Decision_Point = detector.DecisionPoint.Value;
                        int MPH = detector.Approach.MPH.Value;

                        Business.SpeedExportApproachDirection approachDirection =
                            new MOE.Common.Business.SpeedExportApproachDirection(
                            startDate, endDate, signalID, phase, detector.DetectorID
                            , direction, MPH, movementDelay, Decision_Point,
                            binSize, detector.MinSpeedFilter.Value,
                            detector.DistanceFromStopBar.Value);
                        this.List.Add(approachDirection);
                    }
                
            }
        }
    }
}
