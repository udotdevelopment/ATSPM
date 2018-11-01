using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Business.PCD
{
    [TestClass]
    public class PCDUnitTest
    {
        [TestMethod]
        public void PCDVolumeUnitTest()
        {
            DateTime start = Convert.ToDateTime("1/1/2014 12:00 AM");
            DateTime end = Convert.ToDateTime("1/1/2014 12:15 AM");
            var directionRepository = DirectionTypeRepositoryFactory.Create();
            var directions = directionRepository.GetAllDirections();
            var signalRepository = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            var signal = signalRepository.GetVersionOfSignalByDate("7328", start);
            var approaches = signal.GetApproachesForSignalThatSupportMetric(6);

            List<Common.Business.SignalPhase> signalPhaseList = new List<Common.Business.SignalPhase>();
            foreach (MOE.Common.Models.Approach approach in approaches)
            {
                var protectedSignalPhase = new Common.Business.SignalPhase(start, end, approach, false, 15,
                    6, false);
                signalPhaseList.Add(protectedSignalPhase);
                if (approach.PermissivePhaseNumber.HasValue)
                {
                    var permissiveSignalPhase = new Common.Business.SignalPhase(start, end, approach, false, 15,
                        6, true);
                    signalPhaseList.Add(permissiveSignalPhase);
                }
            }

            ApproachVolumeOptions approachVolumeOptions = new ApproachVolumeOptions("7328", start, end, null, 15, true, true, true, true, true, true);
            Common.Business.ApproachVolume.ApproachVolume nSadvanceCountApproachVolume =null;
            Common.Business.ApproachVolume.ApproachVolume nSlaneByLaneCountApproachVolume = null;
            Common.Business.ApproachVolume.ApproachVolume eWadvanceCountApproachVolume = null;
            Common.Business.ApproachVolume.ApproachVolume eWlaneByLaneCountApproachVolume = null;
            if (directions.Any(d => d.Description == "Northbound") || directions.Any(d => d.Description == "Southbound"))
            {
                DirectionType northboundDirection = directions.FirstOrDefault(d => d.Description == "Northbound");
                DirectionType southboundDirection = directions.FirstOrDefault(d => d.Description == "Southbound");
                List<MOE.Common.Models.Approach> northboundApproaches = signal.Approaches
                    .Where(a => a.DirectionTypeID == northboundDirection.DirectionTypeID && a.ProtectedPhaseNumber != 0).ToList();
                List<MOE.Common.Models.Approach> southboundApproaches = signal.Approaches
                    .Where(a => a.DirectionTypeID == southboundDirection.DirectionTypeID && a.ProtectedPhaseNumber != 0).ToList();
                nSadvanceCountApproachVolume =
                    new Common.Business.ApproachVolume.ApproachVolume(northboundApproaches, southboundApproaches, approachVolumeOptions, northboundDirection, southboundDirection, 2);
                nSlaneByLaneCountApproachVolume =
                    new Common.Business.ApproachVolume.ApproachVolume(northboundApproaches, southboundApproaches, approachVolumeOptions, northboundDirection, southboundDirection, 4);
            }
            if (directions.Any(d => d.Description == "Westbound") || directions.Any(d => d.Description == "Eastbound"))
            {
                DirectionType eastboundDirection = directions.FirstOrDefault(d => d.Description == "Eastbound");
                DirectionType westboundDirection = directions.FirstOrDefault(d => d.Description == "Westbound");
                var eastboundApproaches = signal.Approaches
                    .Where(a => a.DirectionTypeID == eastboundDirection.DirectionTypeID && a.ProtectedPhaseNumber != 0).ToList();
                var westboundApproaches = signal.Approaches
                    .Where(a => a.DirectionTypeID == westboundDirection.DirectionTypeID && a.ProtectedPhaseNumber != 0).ToList();
                eWadvanceCountApproachVolume =
                    new Common.Business.ApproachVolume.ApproachVolume(eastboundApproaches, westboundApproaches, approachVolumeOptions, eastboundDirection, westboundDirection, 2);
                eWlaneByLaneCountApproachVolume =
                    new Common.Business.ApproachVolume.ApproachVolume(eastboundApproaches, westboundApproaches, approachVolumeOptions, eastboundDirection, westboundDirection, 4);
            }

            double pcdVolume = signalPhaseList.Sum(s => s.TotalVolume);
            double approachVolume = nSadvanceCountApproachVolume.CombinedDirectionsVolumes.Items.Sum(d => d.DetectorCount) + nSlaneByLaneCountApproachVolume.CombinedDirectionsVolumes.Items.Sum(d => d.DetectorCount) +
                                    eWadvanceCountApproachVolume.CombinedDirectionsVolumes.Items.Sum(d => d.DetectorCount) + eWlaneByLaneCountApproachVolume.CombinedDirectionsVolumes.Items.Sum(d => d.DetectorCount);

            List<DateTime> signalPhaseDetectorActivations = new List<DateTime>();
            foreach (var signalPhase in signalPhaseList)
            {
                foreach (var signalPhaseCycle in signalPhase.Cycles)
                {
                    foreach (var detectorDataPoint in signalPhaseCycle.DetectorEvents)
                    {
                        signalPhaseDetectorActivations.Add(detectorDataPoint.TimeStamp);
                    }
                }
            }
            signalPhaseDetectorActivations.Sort();

            List<DateTime> approachVolumeDetectorActivations = new List<DateTime>();
            foreach (var combinedDirectionsVolume in nSadvanceCountApproachVolume.PrimaryDetectorEvents)
            {
                approachVolumeDetectorActivations.Add(combinedDirectionsVolume.Timestamp);
            }
            foreach (var combinedDirectionsVolume in nSlaneByLaneCountApproachVolume.PrimaryDetectorEvents)
            {
                approachVolumeDetectorActivations.Add(combinedDirectionsVolume.Timestamp);
            }
            foreach (var combinedDirectionsVolume in eWadvanceCountApproachVolume.PrimaryDetectorEvents)
            {
                approachVolumeDetectorActivations.Add(combinedDirectionsVolume.Timestamp);
            }
            foreach (var combinedDirectionsVolume in eWlaneByLaneCountApproachVolume.PrimaryDetectorEvents)
            {
                approachVolumeDetectorActivations.Add(combinedDirectionsVolume.Timestamp);
            }
            approachVolumeDetectorActivations.Sort();

            for (int i = 0; i < approachVolumeDetectorActivations.Count; i++)
            {
                DateTime signalPhaseTimeStamp = signalPhaseDetectorActivations[i];
                DateTime approachVolumeTimeStamp = approachVolumeDetectorActivations[i + 14];
                Assert.IsTrue( signalPhaseTimeStamp == approachVolumeTimeStamp);
            }
            //Common.Business.ApproachVolume.ApproachVolume advanceCountApproachVolume =
            //    new Common.Business.ApproachVolume.ApproachVolume(primaryApproaches, opposingApproaches, this, primaryDirection, opposingDirection, 2);
        }

    }
}
