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
        
        /// <summary>
        /// Start time of the cycle
        /// </summary>
        protected DateTime startTime;
        public DateTime StartTime
        {
            get{
                return startTime;
            }
        }


        public List<Models.Speed_Events> SpeedsForCycle;

        /// <summary>
        /// End time of the Cycle
        /// </summary>
        protected DateTime endTime;
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
        }

        protected DateTime beginYellowClearance;
        public DateTime BeginYellowClearance
        {
            get
            {
                return beginYellowClearance;
            }
        }

        /// <summary>
        /// Y coordinate for the green line on the chart
        /// </summary>
        protected double greenLineY;
        public double GreenLineY
        {
            get
            {
                return greenLineY;
            }
        }

        /// <summary>
        /// Y coordinate for the yellow line on the chart
        /// </summary>
        protected double yellowLineY;
        public double YellowLineY
        {
            get
            {
                return yellowLineY;
            }
        }

        /// <summary>
        /// Y coordinate for the red line on the chart
        /// </summary>
        protected double redLineY;
        public double RedLineY
        {
            get
            {
                return redLineY;
            }
        }

        /// <summary>
        /// The next event status
        /// </summary>
        protected TerminationType termination;
        public TerminationType Termination
        {
            get
            {
                return termination;
            }
            set
            {
                termination = value;
            }
        }

        protected NextEventResponse status;
        public NextEventResponse Status
        {
            get
            {
                return status;
            }
        }

        /// <summary>
        /// A collection of detector activations for the cycle
        /// </summary>
        protected List<DetectorDataPoint> detectorCollection;
        public List<DetectorDataPoint> DetectorCollection
        {
            get
            {
                return detectorCollection;
            }
            set
            {
                detectorCollection = value;
            }
        }

        /// <summary>
        /// A collection of preempt activations for the cycle
        /// </summary>
        protected List<DetectorDataPoint> preemptCollection;
        public List<DetectorDataPoint> PreemptCollection
        {
            get
            {
                return preemptCollection;
            }
            set
            {
                preemptCollection = value;
            }
        }

        /// <summary>
        /// Green time of the Cycle
        /// </summary>
        protected DateTime greenEvent;
        public DateTime GreenEvent
        {
            get
            {
                return greenEvent;
            }
        }

        /// <summary>
        /// Yellow time of the Cycle
        /// </summary>
        protected DateTime yellowEvent;
        public DateTime YellowEvent
        {
            get
            {
                return yellowEvent;
            }
        }

        private double totalArrivalOnGreen = -1;
        public double TotalArrivalOnGreen
        {
            get {
                if (totalArrivalOnGreen == -1)
                    totalArrivalOnGreen = DetectorCollection.Where(d => d.ArrivalOnGreen == true).Count();
                return totalArrivalOnGreen;
            }
        }

        private double totalArrivalOnRed = -1;
        public double TotalArrivalOnRed
        {
            get { 
                if(totalArrivalOnRed == -1)
                    totalArrivalOnRed = DetectorCollection.Where(d => d.ArrivalOnGreen == false).Count();
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
            //get
            //{
            //    if (EndTime > GreenEvent)
            //    {
            //        return (EndTime - GreenEvent).TotalSeconds;
            //    }
            //    else
            //    {
            //        return 0;
            //    }
            //}
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
                    totalRedTime = (YellowEvent - startTime).TotalSeconds;
                }
                return totalRedTime;
            }
        }

        public double TotalTime
        {
            get
            {
                return (EndTime - startTime).TotalSeconds;
            }
        }

        /// <summary>
        /// Constructor for the Cycle
        /// </summary>
        /// <param name="cycleStartTime"></param>
        public Cycle(DateTime starttime)
        {
            startTime = starttime;
            greenLineY = 0;
            yellowLineY = 0;
            redLineY = 0;
            detectorCollection = new List<DetectorDataPoint>();
            preemptCollection = new List<DetectorDataPoint>();
        }

        public void AddDetector(DetectorDataPoint ddp)
        {
            detectorCollection.Add(ddp);
        }

        public void AddPreempt(DetectorDataPoint ddp)
        {
            preemptCollection.Add(ddp);
        }

        /// <summary>
        /// Gets the next event in the cycle
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="timeStamp"></param>

        public void ClearDetectorData()
        {
            totalArrivalOnRed = -1;
            totalGreenTime = -1;
            totalArrivalOnGreen = -1;
            totalVolume = -1;
            detectorCollection.Clear();
        }

        public void FindSpeedEventsForCycle(List<Models.Speed_Events> Speeds)
        {
            SpeedsForCycle = (from r in Speeds
                             where r.timestamp > this.startTime
                             && r.timestamp < this.endTime
                             select r).ToList();
        }
        

        public void NextEvent(EventType eventType, DateTime timeStamp)
        {
            //if the event is green add its' data
            if (eventType == EventType.ChangeToGreen)
            {
                //Check to see that the last event was not a change to green
                if (greenLineY == 0)
                {
                    //Check for bad data
                    if (startTime != DateTime.MinValue)
                    {
                        greenLineY = (timeStamp - startTime).TotalSeconds;
                        greenEvent = timeStamp;
                        status = NextEventResponse.GroupOK;
                    }
                    //Mark the group as having bad data
                    else
                    {
                       status = NextEventResponse.GroupMissingData;
                    }
                }
                //Dont add anything but keep processing
                else
                {
                    status = NextEventResponse.GroupOK;
                }
            }
            // if the change event is yellow add its' data
            else if (eventType ==  EventType.ChangeToYellow)
            {
                // check to see that the last event was not a change to yellow
                if (yellowLineY == 0)
                {
                    //check that the greenline y coordinate was already added
                    //then add the data
                    if (startTime != DateTime.MinValue && greenLineY != 0)
                    {
                        yellowLineY = (timeStamp - startTime).TotalSeconds;
                        yellowEvent = timeStamp;
                        status = NextEventResponse.GroupOK;
                    }
                    //flag the group as bad data
                    else
                    {
                        status = NextEventResponse.GroupMissingData;
                    }
                }
                //keep processing
                else
                {
                    status = NextEventResponse.GroupOK;
                }
            }
            //check to see if the event is a change to red
            else if (eventType ==  EventType.ChangeToRed)
            {
                //check to see if the green, yellow, and starting red was added
                //if not create the next group
                if (startTime == DateTime.MinValue && yellowLineY == 0 && greenLineY == 0 && redLineY == 0)
                {
                    startTime = timeStamp;
                    status = NextEventResponse.GroupOK;
                }
                //add the event to the existing group
                else
                {
                    //if the yellow and green y coordinates have been added and the 
                    // start time is valid add the red event as the ending red
                    if (startTime != DateTime.MinValue && yellowLineY != 0 && greenLineY != 0)
                    {
                        redLineY = (timeStamp - startTime).TotalSeconds;
                        status = NextEventResponse.GroupComplete;
                        endTime = timeStamp;
                    }
                    //mark the group as missing data
                    else
                    {
                        status = NextEventResponse.GroupMissingData;
                    }
                }
                
            }
            //keep processing
            else
            {
                status = NextEventResponse.GroupOK;
            }
        }
    }
}
