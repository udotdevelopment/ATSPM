using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
    /// <summary>
    /// Data that represents a red to red cycle for a signal phase
    /// </summary>
    public class RLMCycle
    {
        public enum NextEventResponse{GroupOK, GroupMissingData, GroupComplete};
        public enum EventType { BeginYellowClearance, BeginRedClearance, 
            BeginRed, EndRed, Unknown };
        
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

        /// <summary>
        /// Y coordinate for the yellow clearance begin line on the chart
        /// </summary>
        protected double yellowClearanceBeginY;
        public double YellowClearanceBeginY
        {
            get
            {
                return yellowClearanceBeginY;
            }
        }

        /// <summary>
        /// Y coordinate for the yellow clearance begin line on the chart
        /// </summary>
        protected double redClearanceBeginY;
        public double RedClearanceBeginY
        {
            get
            {
                return redClearanceBeginY;
            }
        }

        /// <summary>
        /// Y coordinate for the yellow line on the chart
        /// </summary>
        protected double redBeginY;
        public double RedBeginY
        {
            get
            {
                return redBeginY;
            }
        }

        /// <summary>
        /// Y coordinate for the red line on the chart
        /// </summary>
        protected double redEndY;
        public double RedEndY
        {
            get
            {
                return redEndY;
            }
        }

        /// <summary>
        /// The next event status
        /// </summary>
        protected NextEventResponse status;
        public NextEventResponse Status
        {
            get
            {
                return status;
            }
        }

        private double violations = -1;
        public double Violations
        {
            get
            {
                if (violations == -1)
                {
                    violations = 0;
                    foreach (RLMDetectorDataPoint d in DetectorCollection)
                    {
                        if (d.TimeStamp > this.RedClearanceEvent)
                        {
                            violations++;
                            totalViolationTime += (d.TimeStamp - this.RedClearanceEvent).TotalSeconds;

                        }
                    }
                }
                return violations;
            }
        }

        private double yellowOccurrences = -1;
        public double YellowOccurrences
        {
            get
            {
                if (yellowOccurrences == -1)
                {
                    yellowOccurrences = 0;
                    foreach (RLMDetectorDataPoint d in DetectorCollection)
                    {
                        if (d.TimeStamp < this.RedClearanceEvent)
                        {
                            yellowOccurrences++;
                            totalYellowTime += (d.TimeStamp - this.RedClearanceEvent).TotalSeconds;

                        }
                    }
                }
                return yellowOccurrences;
            }
        }

        private double totalYellowTime = 0;
        public double TotalYellowTime
        {
            get
            {
                //because YellowActivations is lazy loaded make sure it is set which also
                //sets YellowActivations
                if (totalYellowTime == -1)
                {
                    double temp = YellowOccurrences;
                }
                return totalYellowTime;
            }
        }

        private double totalViolationTime = 0;
        public double TotalViolationTime
        {
            get
            {
                //because violations is lazy loaded make sure it is set which also
                //sets totalViolationTime
                if(violations== -1)
                {
                    double temp = Violations;
                }
                return totalViolationTime;
            }
        }

        private double srlvSeconds=0;
        public double SRLVSeconds
        {
            get
            {
                return srlvSeconds;
            }
        }

        private double srlv = -1;
        public double Srlv
        {
            get
            {
                if (srlv == -1)
                {
                    srlv = 0;
                    foreach (RLMDetectorDataPoint d in DetectorCollection)
                    {
                        if (d.TimeStamp > this.RedClearanceEvent.AddSeconds(SRLVSeconds))
                        {
                            srlv++;
                        }
                    }
                }
                return srlv;
            }
        }

        /// <summary>
        /// A collection of detector activations for the cycle
        /// </summary>
        protected List<RLMDetectorDataPoint> detectorCollection;
        public List<RLMDetectorDataPoint> DetectorCollection
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
        /// Yellow time of the Cycle
        /// </summary>
        protected DateTime yellowClearanceEvent;
        public DateTime YellowClearanceEvent
        {
            get
            {
                return yellowClearanceEvent;
            }
        }

        /// <summary>
        /// Green time of the Cycle
        /// </summary>
        protected DateTime redClearanceEvent;
        public DateTime RedClearanceEvent
        {
            get
            {
                return redClearanceEvent;
            }
        }

        /// <summary>
        /// Green time of the Cycle
        /// </summary>
        protected DateTime redEvent;
        public DateTime RedEvent
        {
            get
            {
                return redEvent;
            }
        }

        /// <summary>
        /// Green time of the Cycle
        /// </summary>
        protected DateTime redEndEvent;
        public DateTime RedEndEvent
        {
            get
            {
                return redEndEvent;
            }
        }

        /// <summary>
        /// Constructor for the PCDDataPointGroup
        /// </summary>
        /// <param name="cycleStartTime"></param>
        public RLMCycle(DateTime startTime, double srlvSeconds)
        {
            this.srlvSeconds = srlvSeconds;
            this.startTime = startTime;
            yellowClearanceBeginY = 0;
            redClearanceBeginY = 0;
            redBeginY = 0;
            redEndY = 0;

            yellowClearanceEvent = DateTime.MinValue;
            redClearanceEvent = DateTime.MinValue;
            redEvent = DateTime.MinValue;
            redEndEvent = DateTime.MinValue;
            detectorCollection = new List<RLMDetectorDataPoint>();
        }

        public void AddDetector(RLMDetectorDataPoint ddp)
        {
            detectorCollection.Add(ddp);
        }


        /// <summary>
        /// Gets the next event in the cycle
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="timeStamp"></param>

        public void ClearDetectorData()
        {
            
            detectorCollection.Clear();
        }
        

        public void NextEvent(EventType eventType, DateTime timeStamp)
        {
            //if the event is yellow add its' data
            if (eventType == EventType.BeginYellowClearance)
            {
                //Check for bad data
                if (startTime != DateTime.MinValue)
                {
                    yellowClearanceBeginY = 0;
                    yellowClearanceEvent = timeStamp;
                    status = NextEventResponse.GroupOK;
                }
                //Mark the group as having bad data
                else
                {
                    status = NextEventResponse.GroupMissingData;
                }               
                
            }
            // if the change event is red clearance add its' data
            else if (eventType ==  EventType.BeginRedClearance)
            {
                // check to see that the last event was not a change to red clearance
                if (RedClearanceBeginY == 0 && yellowClearanceEvent != DateTime.MinValue)
                {
                    //check that the yellowline y coordinate was already added
                    //then add the data
                    if (startTime != DateTime.MinValue)
                    {
                        redClearanceBeginY = (timeStamp - startTime).TotalSeconds;
                        redClearanceEvent = timeStamp;
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
                    status = NextEventResponse.GroupMissingData;
                }
            }
            //check to see if the event is a change to red
            else if (eventType ==  EventType.BeginRed)
            {
                if (RedBeginY == 0)
                {
                    //check to see if the yellow, red clearance was added
                    //if not create the next group
                    if (startTime != DateTime.MinValue && yellowClearanceEvent!= DateTime.MinValue
                        && RedClearanceEvent != DateTime.MinValue)
                    {
                        redEvent = timeStamp;
                        redBeginY = (timeStamp - startTime).TotalSeconds;
                        status = NextEventResponse.GroupOK;
                    }
                    else
                    {
                        status = NextEventResponse.GroupMissingData;
                    }
                }                
            }
            // if the change event is red clearance add its' data
            else if (eventType == EventType.EndRed )
            {
                // check to see that the last event was not a change to red end
                if (redEndY == 0)
                {
                    //check that the greenline y coordinate was already added
                    //then add the data
                    if (startTime != DateTime.MinValue && YellowClearanceEvent != DateTime.MinValue
                        && RedEvent != DateTime.MinValue && RedClearanceEvent != DateTime.MinValue)
                    {
                        redEndY = (timeStamp - startTime).TotalSeconds;
                        redEndEvent = timeStamp;
                        status = NextEventResponse.GroupComplete;
                        endTime = timeStamp;
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
            //keep processing
            else
            {
                status = NextEventResponse.GroupOK;
            }


        }
    }
}
