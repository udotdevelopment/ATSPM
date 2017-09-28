using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
    /// <summary>
    /// Data that represents a red to red cycle for a signal phase
    /// </summary>
    public class Cycle
    {
        public enum NextEventResponse{GroupOK, GroupMissingData, GroupComplete};
        public enum EventType {ChangeToRed, ChangeToGreen, ChangeToYellow, GreenTermination, BeginYellowClearance, EndYellowClearance,Unknown };
        public enum TerminationType { ForceOff, GapOut, MaxOut, Unknown };

        public DateTime StartTime { get; protected set; }

        public List<Models.Speed_Events> SpeedsForCycle;

        public DateTime EndTime { get; protected set; }

        public DateTime BeginYellowClearance { get; }

        public double GreenLineY { get; protected set; }

        public double YellowLineY { get; protected set; }

        public double RedLineY { get; protected set; }

        public TerminationType Termination { get; set; }

        public NextEventResponse Status { get; protected set; }

        public List<DetectorDataPoint> DetectorCollection { get; set; }

        public List<DetectorDataPoint> PreemptCollection { get; set; }

        public DateTime GreenEvent { get; protected set; }

        public DateTime YellowEvent { get; protected set; }

        private double totalArrivalOnGreen = -1;
        public double TotalArrivalOnGreen
        {
            get {
                if (totalArrivalOnGreen == -1)
                    totalArrivalOnGreen = DetectorCollection.Count(d => d.ArrivalType == ArrivalType.ArrivalOnGreen);
                return totalArrivalOnGreen;
            }
        }

        private double totalArrivalOnYellow = -1;
        public double TotalArrivalOnYellow
        {
            get
            {
                if (totalArrivalOnYellow == -1)
                    totalArrivalOnYellow = DetectorCollection.Count(d => d.ArrivalType == ArrivalType.ArrivalOnYellow);
                return totalArrivalOnYellow;
            }
        }
        private double totalArrivalOnRed = -1;
        public double TotalArrivalOnRed
        {
            get { 
                if(totalArrivalOnRed == -1)
                    totalArrivalOnRed = DetectorCollection.Count(d => d.ArrivalType == ArrivalType.ArrivalOnRed);
                return totalArrivalOnRed;
            }
        }

        public double TotalDelay
        {
            get
            {
                return DetectorCollection.Sum(d => d.Delay);
            }
        }

        private double totalVolume = -1;
        public double TotalVolume
        {
            get
            {
                if (totalVolume == -1)
                    totalVolume = DetectorCollection.Count;
                return totalVolume;
            }

        }

        

        private double totalGreenTime = -1;
        public double TotalGreenTime
        {
            get{
                if (totalGreenTime == -1)
                {
                    totalGreenTime = (EndTime - GreenEvent).TotalSeconds;
                }
                return totalGreenTime;
            }
        }

        private double totalYellowTime = -1;
        public double TotalYellowTime
        {
            get
            {
                if (totalYellowTime == -1)
                {
                    totalYellowTime = (EndTime - YellowEvent).TotalSeconds;
                }
                return totalYellowTime;
            }
        }

        private double totalRedTime = -1;
        public double TotalRedTime
        {
            get
            {
                if (totalRedTime == -1)
                {
                    totalRedTime = (YellowEvent - StartTime).TotalSeconds;
                }
                return totalRedTime;
            }
        }

        public double TotalTime
        {
            get
            {
                return (EndTime - StartTime).TotalSeconds;
            }
        }

        /// <summary>
        /// Constructor for the Cycle
        /// </summary>
        /// <param name="startTime"></param>
        public Cycle(DateTime startTime)
        {
            StartTime = startTime;
            GreenLineY = 0;
            YellowLineY = 0;
            RedLineY = 0;
            DetectorCollection = new List<DetectorDataPoint>();
            PreemptCollection = new List<DetectorDataPoint>();
        }

        public void AddDetector(DetectorDataPoint ddp)
        {
            DetectorCollection.Add(ddp);
        }

        public void AddPreempt(DetectorDataPoint ddp)
        {
            PreemptCollection.Add(ddp);
        }

        public void ClearDetectorData()
        {
            totalArrivalOnRed = -1;
            totalGreenTime = -1;
            totalArrivalOnGreen = -1;
            totalVolume = -1;
            DetectorCollection.Clear();
        }

        public void FindSpeedEventsForCycle(List<Models.Speed_Events> Speeds)
        {
            SpeedsForCycle = (from r in Speeds
                             where r.timestamp > this.StartTime
                             && r.timestamp < this.EndTime
                             select r).ToList();
        }
        

        public void NextEvent(EventType eventType, DateTime timeStamp)
        {
            //if the event is green add its' data
            if (eventType == EventType.ChangeToGreen)
            {
                //Check to see that the last event was not a change to green
                if (GreenLineY == 0)
                {
                    //Check for bad data
                    if (StartTime != DateTime.MinValue)
                    {
                        GreenLineY = (timeStamp - StartTime).TotalSeconds;
                        GreenEvent = timeStamp;
                        Status = NextEventResponse.GroupOK;
                    }
                    //Mark the group as having bad data
                    else
                    {
                       Status = NextEventResponse.GroupMissingData;
                    }
                }
                //Dont add anything but keep processing
                else
                {
                    Status = NextEventResponse.GroupOK;
                }
            }
            // if the change event is yellow add its' data
            else if (eventType ==  EventType.ChangeToYellow)
            {
                // check to see that the last event was not a change to yellow
                if (YellowLineY == 0)
                {
                    //check that the greenline y coordinate was already added
                    //then add the data
                    if (StartTime != DateTime.MinValue && GreenLineY != 0)
                    {
                        YellowLineY = (timeStamp - StartTime).TotalSeconds;
                        YellowEvent = timeStamp;
                        Status = NextEventResponse.GroupOK;
                    }
                    //flag the group as bad data
                    else
                    {
                        Status = NextEventResponse.GroupMissingData;
                    }
                }
                //keep processing
                else
                {
                    Status = NextEventResponse.GroupOK;
                }
            }
            //check to see if the event is a change to red
            else if (eventType ==  EventType.ChangeToRed)
            {
                //check to see if the green, yellow, and starting red was added
                //if not create the next group
                if (StartTime == DateTime.MinValue && YellowLineY == 0 && GreenLineY == 0 && RedLineY == 0)
                {
                    StartTime = timeStamp;
                    Status = NextEventResponse.GroupOK;
                }
                //add the event to the existing group
                else
                {
                    //if the yellow and green y coordinates have been added and the 
                    // start time is valid add the red event as the ending red
                    if (StartTime != DateTime.MinValue && YellowLineY != 0 && GreenLineY != 0)
                    {
                        RedLineY = (timeStamp - StartTime).TotalSeconds;
                        Status = NextEventResponse.GroupComplete;
                        EndTime = timeStamp;
                    }
                    //mark the group as missing data
                    else
                    {
                        Status = NextEventResponse.GroupMissingData;
                    }
                }
                
            }
            //keep processing
            else
            {
                Status = NextEventResponse.GroupOK;
            }
        }
    }
}
