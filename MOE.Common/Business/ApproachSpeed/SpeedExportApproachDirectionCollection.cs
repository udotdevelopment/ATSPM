using System;
using System.Collections.Generic;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business
{
    public class SpeedExportApproachDirectionCollection
    {
        public List<SpeedExportApproachDirection> List = new List<SpeedExportApproachDirection>();

        public SpeedExportApproachDirectionCollection(DateTime startDate, DateTime endDate, string signalID,
            int binSize)
        {
            var signalRepository =
                SignalsRepositoryFactory.Create();
            var signal = signalRepository.GetLatestVersionOfSignalBySignalID(signalID);
            var distinctPhases = signal.GetDetectorsForSignalThatSupportAMetric(10);


            //If there are phases in the database add the charts
            if (distinctPhases.Count > 0)
                foreach (var detector in distinctPhases)
                {
                    //Get the phase
                    var phase = detector.Approach.ProtectedPhaseNumber;
                    var direction = detector.Approach.DirectionType.Description;
                    var movementDelay = detector.MovementDelay.Value;
                    var Decision_Point = detector.DecisionPoint.Value;
                    var MPH = detector.Approach.MPH.Value;

                    var approachDirection =
                        new SpeedExportApproachDirection(
                            startDate, endDate, signalID, phase, detector.DetectorID
                            , direction, MPH, movementDelay, Decision_Point,
                            binSize, detector.MinSpeedFilter.Value,
                            detector.DistanceFromStopBar.Value);
                    List.Add(approachDirection);
                }
        }
    }
}