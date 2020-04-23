using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using MOE.Common.Models.Repositories;
using MvcCheckBoxList.Model;
using NuGet;

namespace MOE.Common.Models
{
    public partial class Detector
    {
        [NotMapped] public HtmlListInfo htmlListInfo = new HtmlListInfo(HtmlTag.table, 2, new {@class = "class_name"},
            TextLayout.Default, TemplateIsUsed.Yes);

        [NotMapped]
        public List<DetectionType> AllDetectionTypes { get; set; }

        [NotMapped]
        public List<DetectionHardware> AllHardwareTypes { get; set; }

        [NotMapped]
        public string[] DetectionIDs { get; set; }

        [NotMapped]
        public string Index { get; set; }


        public List<Controller_Event_Log> GetDetectorOnEvents(DateTime startTime, DateTime endTime)
        {
            var repository =
                ControllerEventLogRepositoryFactory.Create();
            return repository.GetEventsByEventCodesParam(Approach.SignalID, startTime,
                endTime, new List<int> {82}, DetChannel);
        }

        public bool Equals(Detector graphDetectorToCompare)
        {
            return CompareGraph_DetectorProperties(graphDetectorToCompare);
        }

        public int GetVolumeForPeriod(DateTime StartDate, DateTime EndDate)
        {
            var repository =
                ControllerEventLogRepositoryFactory.Create();
            return repository.GetDetectorActivationCount(Approach.SignalID, StartDate, EndDate, DetChannel);
        }

        private bool CompareGraph_DetectorProperties(Detector graphDetectorToCompare)
        {
            if (graphDetectorToCompare != null
                && DetectorID == graphDetectorToCompare.DetectorID
                && DetChannel == graphDetectorToCompare.DetChannel
                && DistanceFromStopBar == graphDetectorToCompare.DistanceFromStopBar
                && MinSpeedFilter == graphDetectorToCompare.MinSpeedFilter
                && DateAdded == graphDetectorToCompare.DateAdded
                && DetectionTypeIDs == graphDetectorToCompare.DetectionTypeIDs
                && DecisionPoint == graphDetectorToCompare.DecisionPoint
                && MovementDelay == graphDetectorToCompare.MovementDelay
                && LaneNumber == graphDetectorToCompare.LaneNumber
            )
                return true;
            return false;
        }

        public double GetOffset()
        {
            if (DecisionPoint == null)
                DecisionPoint = 0;
            if (Approach.MPH.HasValue && Approach.MPH > 0)
            {
                return Convert.ToDouble((DistanceFromStopBar / (Approach.MPH * 1.467) - DecisionPoint) * 1000);
            }
            else
            {
                return 0;
            }

        }

        public static Detector
            CopyDetector(int ID,
                bool increaseChannel) //still need to add this detector to the collection of its associated Approach
        {
            var detectorRepository =
                DetectorRepositoryFactory.Create();
            var detectorToCopy = detectorRepository.GetDetectorByID(ID);


            var newGD = new Detector();

            var dtr = DetectionTypeRepositoryFactory.Create();
            newGD.AllDetectionTypes = dtr.GetAllDetectionTypesNoBasic();

            newGD.DateAdded = DateTime.Now;
            newGD.DetectionTypeIDs = new List<int>();
            newGD.DetectionTypes = new List<DetectionType>();

            

            foreach (var dt in detectorToCopy.DetectionTypes)
            {
                newGD.DetectionTypeIDs.Add(dt.DetectionTypeID);
            }

           

            newGD.DistanceFromStopBar = detectorToCopy.DistanceFromStopBar;
            newGD.LaneNumber = detectorToCopy.LaneNumber;
            newGD.MinSpeedFilter = detectorToCopy.MinSpeedFilter;
            newGD.MovementTypeID = detectorToCopy.MovementTypeID;
            newGD.LaneTypeID = detectorToCopy.LaneTypeID;
            newGD.DecisionPoint = detectorToCopy.DecisionPoint;
            newGD.MovementDelay = detectorToCopy.MovementDelay;
            newGD.DetectionHardwareID = detectorToCopy.DetectionHardwareID;
            newGD.MovementDelay = detectorToCopy.MovementDelay;
            newGD.DetectorComments = new List<DetectorComment>();


            if (increaseChannel)
                newGD.DetChannel = detectorRepository.GetMaximumDetectorChannel(detectorToCopy.Approach.VersionID) + 1;
            else //when copying signals, signalID changes, and DetChannel should be kept the same.
                newGD.DetChannel = detectorToCopy.DetChannel;

            if (newGD.DetChannel < 10)
                newGD.DetectorID = detectorToCopy.Approach.SignalID + "0" + newGD.DetChannel;
            else
                newGD.DetectorID = detectorToCopy.Approach.SignalID + newGD.DetChannel;
            return newGD;
        }

        public Signal GetTheSignalThatContainsThisDetector()
        {
            return Approach.Signal;
        }

        public bool DetectorSupportsThisMetric(int metricID)
        {
            var result = false;
            if (DetectionTypes != null)
            {
                foreach (var dt in DetectionTypes)
                {
                    foreach (var m in dt.MetricTypes)
                    {
                        if (m.MetricID == metricID)
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public static int GetDefaultPhaseNumberByDirectionsAndMovementTypes(int directionType, bool isLeft)
        {
            var phaseNumber = 0;

            switch (directionType)
            {
                case 1: //NB
                    if (!isLeft)
                        phaseNumber = 2;
                    else
                        phaseNumber = 1;
                    break;
                case 2: //SB
                    if (!isLeft)
                        phaseNumber = 4;
                    else
                        phaseNumber = 3;
                    break;
                case 3: //EB
                    if (!isLeft)
                        phaseNumber = 8;
                    else
                        phaseNumber = 7;
                    break;
                case 4: //WB
                    if (!isLeft)
                        phaseNumber = 6;
                    else
                        phaseNumber = 5;
                    break;
            }

            return phaseNumber;
        }
    }
}