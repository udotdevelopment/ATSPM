using System;
using System.Collections.Generic;

namespace MOE.Common.Business
{
    /// <summary>
    ///     Data that represents a red to red cycle for a signal phase
    /// </summary>
    public class RLMCycle
    {
        public enum EventType
        {
            BeginYellowClearance,
            BeginRedClearance,
            BeginRed,
            EndRed,
            Unknown
        }

        public enum NextEventResponse
        {
            GroupOK,
            GroupMissingData,
            GroupComplete
        }

        /// <summary>
        ///     Start time of the Cycle
        /// </summary>
        protected DateTime endTime;

        /// <summary>
        ///     Y coordinate for the yellow line on the chart
        /// </summary>
        protected double redBeginY;

        /// <summary>
        ///     Y coordinate for the yellow clearance begin line on the chart
        /// </summary>
        protected double redClearanceBeginY;

        /// <summary>
        ///     Green time of the Cycle
        /// </summary>
        protected DateTime redClearanceEvent;

        /// <summary>
        ///     Green time of the Cycle
        /// </summary>
        protected DateTime redEndEvent;

        /// <summary>
        ///     Y coordinate for the red line on the chart
        /// </summary>
        protected double redEndY;

        /// <summary>
        ///     Green time of the Cycle
        /// </summary>
        protected DateTime redEvent;

        private double srlv = -1;

        /// <summary>
        ///     Start time of the cycle
        /// </summary>
        protected DateTime startTime;

        /// <summary>
        ///     The next event status
        /// </summary>
        protected NextEventResponse status;

        private double totalViolationTime;

        private double totalYellowTime;

        private double violations = -1;

        /// <summary>
        ///     Y coordinate for the yellow clearance begin line on the chart
        /// </summary>
        protected double yellowClearanceBeginY;


        /// <summary>
        ///     Yellow time of the Cycle
        /// </summary>
        protected DateTime yellowClearanceEvent;

        private double yellowOccurrences = -1;

        /// <summary>
        ///     Constructor for the PCDDataPointGroup
        /// </summary>
        /// <param name="cycleStartTime"></param>
        public RLMCycle(DateTime startTime, double srlvSeconds)
        {
            SRLVSeconds = srlvSeconds;
            this.startTime = startTime;
            yellowClearanceBeginY = 0;
            redClearanceBeginY = 0;
            redBeginY = 0;
            redEndY = 0;

            yellowClearanceEvent = DateTime.MinValue;
            redClearanceEvent = DateTime.MinValue;
            redEvent = DateTime.MinValue;
            redEndEvent = DateTime.MinValue;
            DetectorCollection = new List<RLMDetectorDataPoint>();
        }

        public DateTime StartTime => startTime;

        public DateTime EndTime => endTime;

        public double YellowClearanceBeginY => yellowClearanceBeginY;

        public double RedClearanceBeginY => redClearanceBeginY;

        public double RedBeginY => redBeginY;

        public double RedEndY => redEndY;

        public NextEventResponse Status => status;

        public double Violations
        {
            get
            {
                if (violations == -1)
                {
                    violations = 0;
                    foreach (var d in DetectorCollection)
                        if (d.TimeStamp > RedClearanceEvent)
                        {
                            violations++;
                            totalViolationTime += (d.TimeStamp - RedClearanceEvent).TotalSeconds;
                        }
                }
                return violations;
            }
        }

        public double YellowOccurrences
        {
            get
            {
                if (yellowOccurrences == -1)
                {
                    yellowOccurrences = 0;
                    foreach (var d in DetectorCollection)
                        if (d.TimeStamp <= RedClearanceEvent)
                        {
                            yellowOccurrences++;
                            totalYellowTime += (d.TimeStamp - RedClearanceEvent).TotalSeconds;
                        }
                }
                return yellowOccurrences;
            }
        }

        public double TotalYellowTime
        {
            get
            {
                //because YellowActivations is lazy loaded make sure it is set which also
                //sets YellowActivations
                if (totalYellowTime == -1)
                {
                    var temp = YellowOccurrences;
                }
                return totalYellowTime;
            }
        }

        public double TotalViolationTime
        {
            get
            {
                //because violations is lazy loaded make sure it is set which also
                //sets totalViolationTime
                if (violations == -1)
                {
                    var temp = Violations;
                }
                return totalViolationTime;
            }
        }

        public double SRLVSeconds { get; }

        public double SevereRedLightViolations
        {
            get
            {
                if (srlv == -1)
                {
                    srlv = 0;
                    foreach (var d in DetectorCollection)
                        if (d.TimeStamp > RedClearanceEvent.AddSeconds(SRLVSeconds))
                            srlv++;
                }
                return srlv;
            }
        }

        /// <summary>
        ///     A collection of detector activations for the cycle
        /// </summary>

        public List<RLMDetectorDataPoint> DetectorCollection { get; set; }

        public DateTime YellowClearanceEvent => yellowClearanceEvent;

        public DateTime RedClearanceEvent => redClearanceEvent;

        public DateTime RedEvent => redEvent;

        public DateTime RedEndEvent => redEndEvent;

        public void AddDetector(RLMDetectorDataPoint ddp)
        {
            DetectorCollection.Add(ddp);
        }


        /// <summary>
        ///     Gets the next event in the cycle
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="timeStamp"></param>
        public void ClearDetectorData()
        {
            DetectorCollection.Clear();
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
            else if (eventType == EventType.BeginRedClearance)
            {
                // check to see that the last event was not a change to red clearance
                if (RedClearanceBeginY == 0 && yellowClearanceEvent != DateTime.MinValue)
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
                //keep processing
                else
                    status = NextEventResponse.GroupMissingData;
            }
            //check to see if the event is a change to red
            else if (eventType == EventType.BeginRed)
            {
                if (RedBeginY == 0)
                    if (startTime != DateTime.MinValue && yellowClearanceEvent != DateTime.MinValue
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
            // if the change event is red clearance add its' data
            else if (eventType == EventType.EndRed)
            {
                // check to see that the last event was not a change to red end
                if (redEndY == 0)
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
                //keep processing
                else
                    status = NextEventResponse.GroupOK;
            }
            //keep processing
            else
            {
                status = NextEventResponse.GroupOK;
            }
        }
    }
}