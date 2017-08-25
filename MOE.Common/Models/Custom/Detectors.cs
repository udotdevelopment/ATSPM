using MOE.Common.Business;
using MvcCheckBoxList.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models
{
    public partial class Detector
    {
        [NotMapped]
        public List<DetectionType> AllDetectionTypes { get; set; }
        [NotMapped]
        public List<DetectionHardware> AllHardwareTypes { get; set; }
        [NotMapped]
        public String[] DetectionIDs { get; set; }
        [NotMapped]
        public HtmlListInfo htmlListInfo = new HtmlListInfo(HtmlTag.table, 2, new { @class="class_name" }, TextLayout.Default, TemplateIsUsed.Yes);
        [NotMapped]
        public String Index { get; set; }
        
       
        public List<MOE.Common.Models.Controller_Event_Log> GetDetectorOnEvents(DateTime startTime, DateTime endTime)
        {
            MOE.Common.Models.Repositories.IControllerEventLogRepository repository =
                MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            return repository.GetEventsByEventCodesParam(this.Approach.SignalID, startTime,
                endTime, new List<int>() { 82 }, DetChannel);
        }

        public bool Equals(Detector graphDetectorToCompare)
        {
            return CompareGraph_DetectorProperties(graphDetectorToCompare);
        }

        public int GetVolumeForPeriod(DateTime StartDate, DateTime EndDate)
        {
            MOE.Common.Models.Repositories.IControllerEventLogRepository repository =
               MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();
           return repository.GetDetectorActivationCount(this.Approach.SignalID, StartDate, EndDate, this.DetChannel);
        }

        private bool CompareGraph_DetectorProperties(Detector graphDetectorToCompare)
        {
            if (graphDetectorToCompare != null
                && this.DetectorID == graphDetectorToCompare.DetectorID
                && this.DetChannel == graphDetectorToCompare.DetChannel
                && this.DistanceFromStopBar == graphDetectorToCompare.DistanceFromStopBar
                && this.MinSpeedFilter == graphDetectorToCompare.MinSpeedFilter
                && this.DateAdded == graphDetectorToCompare.DateAdded
                && this.DetectionTypeIDs == graphDetectorToCompare.DetectionTypeIDs
                && this.DecisionPoint == graphDetectorToCompare.DecisionPoint
                && this.MovementDelay == graphDetectorToCompare.MovementDelay
                && this.LaneNumber == graphDetectorToCompare.LaneNumber

                )
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public double GetOffset()
        {
            if (this.DecisionPoint == null)
                this.DecisionPoint = 0;
            double offset = Convert.ToDouble(((DistanceFromStopBar / (this.Approach.MPH * 1.467)) - this.DecisionPoint) * 1000);
            return offset;
        }
        
        public static Models.Detector CopyDetector(int ID, bool increaseChannel) //still need to add this detector to the collection of its associated Approach
        {
            Models.Repositories.IDetectorRepository detectorRepository =
                Models.Repositories.DetectorRepositoryFactory.Create();
            Models.Detector detectorToCopy = detectorRepository.GetDetectorByID(ID);
            Models.Detector newGD = new Models.Detector();

            Models.Repositories.IDetectionTypeRepository dtr = Models.Repositories.DetectionTypeRepositoryFactory.Create();
            newGD.AllDetectionTypes = dtr.GetAllDetectionTypesNoBasic();

            newGD.DateAdded = DateTime.Now;
            newGD.DetectionTypeIDs = new List<int>();
            foreach (DetectionType d in detectorToCopy.DetectionTypes)
            {
                newGD.DetectionTypeIDs.Add(d.DetectionTypeID);
            }

            newGD.DistanceFromStopBar = detectorToCopy.DistanceFromStopBar;
            newGD.LaneNumber = detectorToCopy.LaneNumber;
            newGD.MinSpeedFilter = detectorToCopy.MinSpeedFilter;
            newGD.MovementTypeID = detectorToCopy.MovementTypeID;
            newGD.LaneTypeID = detectorToCopy.LaneTypeID;
            newGD.DecisionPoint = detectorToCopy.DecisionPoint;
            newGD.MovementDelay = detectorToCopy.MovementDelay;
            newGD.DetectionHardwareID = detectorToCopy.DetectionHardwareID;
            newGD.DetectorComments = new List<MOE.Common.Models.DetectorComment>();
            

            if (increaseChannel)
            {
                newGD.DetChannel = detectorRepository.GetMaximumDetectorChannel(detectorToCopy.Approach.SignalID) + 1;
            }
            else //when copying signals, signalID changes, and DetChannel should be kept the same.
            {
                newGD.DetChannel = detectorToCopy.DetChannel;
            }

            if (newGD.DetChannel < 10)
            {
                newGD.DetectorID = detectorToCopy.Approach.SignalID + "0" + newGD.DetChannel;
            }
            else
            {
                newGD.DetectorID = detectorToCopy.Approach.SignalID + newGD.DetChannel;
            }
            return newGD;
        }

        public Models.Signal GetTheSignalThatContainsThisDetector()
        {
            return (this.Approach.Signal);
        }

        public bool DetectorSupportsThisMetric(int metricID)
        {
            bool result = false;
            if (this.DetectionTypes != null)
            {
                foreach (var dt in this.DetectionTypes)
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
            int phaseNumber = 0;

            switch (directionType)
            {
                case 1: //NB
                    if (!isLeft)
                    {
                        phaseNumber = 2;
                    }
                    else
                    {
                        phaseNumber = 1;
                    }
                    break;
                case 2://SB
                    if (!isLeft)
                    {
                        phaseNumber = 4;
                    }
                    else
                    {
                        phaseNumber = 3;
                    }
                    break;
                case 3://EB
                    if (!isLeft)
                    {
                        phaseNumber = 8;
                    }
                    else
                    {
                        phaseNumber = 7;
                    }
                    break;
                case 4://WB
                    if (!isLeft)
                    {
                        phaseNumber = 6;
                    }
                    else
                    {
                        phaseNumber = 5;
                    }
                    break;
            }

            return phaseNumber;
        }



    }
}
