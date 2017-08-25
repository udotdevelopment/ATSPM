using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MOE.Common.Business
{
    public class Plan
    {
        /// <summary>
        /// The start time of the plan
        /// </summary>
        protected DateTime startTime;
        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
        }


        public int TotalDetectorHits { get; set; }

        public AvgSpeedBucketCollection AvgSpeedBucketCollection { get; set; }

        public int EightyFifth { get; set; }

        public int AvgSpeed { get; set; }

        public int StdDevAvgSpeed { get; set; }


       public double PercentGreen
        {
            get
            {
                if (TotalTime > 0)
                {
                    return Math.Round(((TotalGreenTime / TotalTime) * 100));
                }
                else
                {
                    return 0;
                }
            }
        }


       public List<Cycle> CycleCollection = new List<Cycle>();

        /// <summary>
        /// The end time of the plan
        /// </summary>
        protected DateTime endTime;
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
            //set
            //{
            //    endTime = value;
            //}
        }

        protected int cycleCount;
        public int CycleCount
        {
            get
            {
                return cycleCount;
            }
            set
            {
                cycleCount = value;
            }
        }


        private int cycleLength;
        public int CycleLength
        {
            get
            {
                return cycleLength;
            }
            set
            {
                cycleLength = value;
            }
        }

        private int offsetLength;
        public int OffsetLength
        {
            get
            {
                return offsetLength;
            }
            set
            {
                offsetLength = value;
            }
        }

        /// <summary>
        /// The total number of acitivations for this plan while the phase was on green
        /// </summary>
        //public Double  TotalArrivalOnGreen { get; set; }


        public int MinSpeedFilter { get; set; }

        
        public double AvgDelay
        {
            get
            {
                return TotalDelay/TotalVolume;
            }           
        }


        /// <summary>
        /// A calculation to get the percent of activations on green
        /// </summary>
        public double PercentArrivalOnGreen
        {
            get
            {
                if (TotalVolume > 0)
                {
                    return Math.Round(((TotalArrivalOnGreen / TotalVolume) * 100));
                }
                else
                {
                    return 0;
                }
            }
        }

        public double PlatoonRatio
        {
            get
            {
                if (TotalVolume > 0)
                {
                    return Math.Round((PercentArrivalOnGreen / PercentGreen),2);
                }
                else
                {
                    return 0;
                }
            }
        }


        private double totalArrivalOnGreen = -1;
        public double TotalArrivalOnGreen
        {
            get
            {
                if (totalArrivalOnGreen == -1)
                    totalArrivalOnGreen = CycleCollection.Sum(d => d.TotalArrivalOnGreen);
                return totalArrivalOnGreen;
            }
        }
        

        private double totalArrivalOnRed = -1;
        public double TotalArrivalOnRed
        {
            get { 
                    if (totalArrivalOnRed == -1)
                        totalArrivalOnRed = CycleCollection.Sum(d => d.TotalArrivalOnRed);
                    return totalArrivalOnRed;
            }
        }

        public double TotalDelay
        {
            get
            {
                return CycleCollection.Sum(d => d.TotalDelay);
            }
        }

        private double totalVolume = -1;
        public double TotalVolume
        {
            get
            {
                if(totalVolume == -1)
                {
                    totalVolume = CycleCollection.Sum(d=> d.TotalVolume);
                }
                return totalVolume;
            }

        }

        private double totalGreenTime = -1;
        public double TotalGreenTime
        {
            get
            {
                if(totalGreenTime == -1)
                {
                    totalGreenTime = CycleCollection.Sum(d => d.TotalGreenTime);
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
                    totalYellowTime = CycleCollection.Sum(d => d.TotalYellowTime);
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
                    totalRedTime = CycleCollection.Sum(d => d.TotalRedTime);
                }
                return totalRedTime;
            }
        }

        public double TotalTime
        {
            get
            {
                return CycleCollection.Sum(d => d.TotalTime);
            }
        }

        //private double totalplantime;
        //private double totalgreenphasetime;
        //public SortedDictionary<int,int> phaseCountDictionary = new SortedDictionary<int,int>();

        public SortedDictionary<int, int> Splits = new SortedDictionary<int, int>();

        /// <summary>
        /// The plan number
        /// </summary>
        public int PlanNumber { get; set; }

        /// <summary>
        /// The signal number
        /// </summary>
        public string SignalID { get; set; }

        /// <summary>
        /// The plan number
        /// </summary>
        public int PhaseNumber { get; set; }



        /// <summary>
        /// Constructor that sets the start time, end time and plan, and creates the the lower level objects and statistics
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="plan"></param>
        /// <param name="signaltable"></param>
        /// <param name="detectortable"></param>
        public Plan(DateTime start, DateTime end, int planNumber, List<Models.Controller_Event_Log> signaltable,
            List<Models.Controller_Event_Log> detectortable, List<Models.Controller_Event_Log> preempttable, 
            Models.Approach approach)
        {
            this.SignalID = approach.SignalID;
            this.PhaseNumber = approach.ProtectedPhaseNumber;
            startTime = start;
            endTime = end;
            PlanNumber = planNumber;
            GetGreenYellowRedCycle(start, end, signaltable, detectortable, preempttable);
            //pcdCollection = new PCDDataPointCollection(start, end, signaltable, detectortable, preempttable, signalid, phase);
            //setstatistics();
        }

        /// <summary>
        /// Constructor that sets the start time, end time and plan and nothing else
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="plan"></param>
        public Plan(DateTime start, DateTime end, int planNumber)
        {
            startTime = start;
            endTime = end;
            PlanNumber = planNumber;
            
        }

        /// <summary>
        /// Translates an event code to an event type
        /// </summary>
        /// <param name="EventCode"></param>
        /// <returns></returns>
        private Cycle.EventType GetEventType(int EventCode)
        {
            switch (EventCode)
            {
                    
                case 1:
                    return Cycle.EventType.ChangeToGreen;
                // overlap green
                case 61:
                    return Cycle.EventType.ChangeToGreen;
                case 8:
                    return Cycle.EventType.ChangeToYellow;
                // overlap yellow
                case 63:
                    return Cycle.EventType.ChangeToYellow;
                case 10:
                    return Cycle.EventType.ChangeToRed;
                // overlap red
                case 64:
                    return Cycle.EventType.ChangeToRed;
                default:
                    return Cycle.EventType.Unknown;
            }
        }

       

        public void SetHighCycleCount(Business.AnalysisPhaseCollection phases)
        {
            //find all the phases cycles within the plan
            int HighCycleCount = 0;
            foreach (Business.AnalysisPhase phase in phases.Items)
            {
                var Cycles = from cycle in phase.Cycles.Items
                             where cycle.StartTime > this.StartTime && cycle.EndTime < this.endTime
                             select cycle;

                if (Cycles.Count() > HighCycleCount)
                {
                    HighCycleCount = Cycles.Count();
                }

                //phaseCountDictionary.Add(phase.PhaseNumber, Cycles.Count());
            }

            cycleCount = HighCycleCount;

        }

        public void SetProgrammedSplits(string signalId)
        {

            Splits.Clear();
            List<int> l = new List<int>();
            for (int i = 130; i <= 151; i++)
            {
                l.Add(i);
            }
            MOE.Common.Business.ControllerEventLogs SplitsDT = new ControllerEventLogs(signalId, this.StartTime, this.StartTime.AddSeconds(2), l);

            foreach (MOE.Common.Models.Controller_Event_Log row in SplitsDT.Events)
            {
                if (row.EventCode == 132)
                {
                    this.CycleLength = row.EventParam;
                }

                if (row.EventCode == 133)
                {
                    this.OffsetLength = row.EventParam;
                }

                if (row.EventCode == 134 && !Splits.ContainsKey(1))
                {
                    this.Splits.Add(1, row.EventParam);
                }
                else if (row.EventCode == 134 && row.EventParam > 0)
                {
                    this.Splits[1] = row.EventParam;
                }

                if (row.EventCode == 135 && !Splits.ContainsKey(2))
                {
                    this.Splits.Add(2, row.EventParam);
                }
                else if (row.EventCode == 135 && row.EventParam > 0)
                {
                    this.Splits[2] = row.EventParam;
                }

                if (row.EventCode == 136 && !Splits.ContainsKey(3))
                {
                    this.Splits.Add(3, row.EventParam);
                }
                else if (row.EventCode == 136 && row.EventParam > 0)
                {
                    this.Splits[3] = row.EventParam;
                }

                if (row.EventCode == 137 && !Splits.ContainsKey(4))
                {
                    this.Splits.Add(4, row.EventParam);
                }
                else if (row.EventCode == 137 && row.EventParam > 0)
                {
                    this.Splits[4] = row.EventParam;
                }

                if (row.EventCode == 138 && !Splits.ContainsKey(5))
                {
                    this.Splits.Add(5, row.EventParam);
                }
                else if (row.EventCode == 138 && row.EventParam > 0)
                {
                    this.Splits[5] = row.EventParam;
                }

                if (row.EventCode == 139 && !Splits.ContainsKey(6))
                {
                    this.Splits.Add(6, row.EventParam);
                }
                else if (row.EventCode == 139 && row.EventParam > 0)
                {
                    this.Splits[6] = row.EventParam;
                }

                if (row.EventCode == 140 && !Splits.ContainsKey(7))
                {
                    this.Splits.Add(7, row.EventParam);
                }
                else if (row.EventCode == 140 && row.EventParam > 0)
                {
                    this.Splits[7] = row.EventParam;
                }

                if (row.EventCode == 141 && !Splits.ContainsKey(8))
                {
                    this.Splits.Add(8, row.EventParam);
                }
                else if (row.EventCode == 141 && row.EventParam > 0)
                {
                    this.Splits[8] = row.EventParam;
                }

                if (row.EventCode == 142 && !Splits.ContainsKey(9))
                {
                    this.Splits.Add(9, row.EventParam);
                }
                else if (row.EventCode == 142 && row.EventParam > 0)
                {
                    this.Splits[9] = row.EventParam;
                }

                if (row.EventCode == 143 && !Splits.ContainsKey(10))
                {
                    this.Splits.Add(10, row.EventParam);
                }
                else if (row.EventCode == 143 && row.EventParam > 0)
                {
                    this.Splits[10] = row.EventParam;
                }

                if (row.EventCode == 144 && !Splits.ContainsKey(11))
                {
                    this.Splits.Add(11, row.EventParam);
                }
                else if (row.EventCode == 144 && row.EventParam > 0)
                {
                    this.Splits[11] = row.EventParam;
                }

                if (row.EventCode == 145 && !Splits.ContainsKey(12))
                {
                    this.Splits.Add(12, row.EventParam);
                }
                else if (row.EventCode == 145 && row.EventParam > 0)
                {
                    this.Splits[12] = row.EventParam;
                }

                if (row.EventCode == 146 && !Splits.ContainsKey(13))
                {
                    this.Splits.Add(13, row.EventParam);
                }
                else if (row.EventCode == 146 && row.EventParam > 0)
                {
                    this.Splits[13] = row.EventParam;
                }

                if (row.EventCode == 147 && !Splits.ContainsKey(14))
                {
                    this.Splits.Add(14, row.EventParam);
                }
                else if (row.EventCode == 147 && row.EventParam > 0)
                {
                    this.Splits[14] = row.EventParam;
                }

                if (row.EventCode == 148 && !Splits.ContainsKey(15))
                {
                    this.Splits.Add(15, row.EventParam);
                }
                else if (row.EventCode == 148 && row.EventParam > 0)
                {
                    this.Splits[15] = row.EventParam;
                }

                if (row.EventCode == 149 && !Splits.ContainsKey(16))
                {
                    this.Splits.Add(16, row.EventParam);
                }
                else if (row.EventCode == 149 && row.EventParam > 0)
                {
                    this.Splits[16] = row.EventParam;
                }
                
            }

            if (Splits.Count == 0)
            {
                for (int i = 0; i < 16; i++)
                {
                    this.Splits.Add(i, 0);
                }
            }
        }

        public int FindHighestRecordedSplitPhase()
        {
            int phase = 0;

            var maxkey = Splits.Max(x => x.Key);

            phase = maxkey;

            return phase;
        }

        public void FillMissingSplits(int highestSplit)
        {
            for (int counter = 0; counter < highestSplit + 1; counter++)
            {
                if (this.Splits.ContainsKey(counter))
                {
                }
                else
                {
                this.Splits.Add(counter,0);
                }
            }
            
        }

        private void GetGreenYellowRedCycle(DateTime startTime, DateTime endTime,
            List<Models.Controller_Event_Log> cycleEvents, List<Models.Controller_Event_Log> detectorEvents,
            List<Models.Controller_Event_Log> preemptEvents)
        {
           
            Cycle pcd = null;
            //use a counter to help determine when we are on the last row
            int counter = 0;

            foreach (MOE.Common.Models.Controller_Event_Log row in cycleEvents)
            {
                //use a counter to help determine when we are on the last row
                counter++;
                if (row.Timestamp >= startTime && row.Timestamp <= endTime)
                {
                    //If this is the first PCD Group we need to handle a special case
                    //where the pcd starts at the start of the requested period to 
                    //make sure we include all data
                    if (CycleCollection.Count == 0 && pcd == null)
                    {
                        //Make the first group start on at the exact start of the requested period
                        pcd = new Cycle(startTime);
                        //Add a green event if the first event is yellow
                        if (GetEventType(row.EventCode) == Cycle.EventType.ChangeToYellow)
                        {
                            pcd.NextEvent(Cycle.EventType.ChangeToGreen, startTime.AddMilliseconds(1));
                        }
                        //Add a green and yellow event if first event is red
                        else if (GetEventType(row.EventCode) == Cycle.EventType.ChangeToRed)
                        {
                            pcd.NextEvent(Cycle.EventType.ChangeToGreen, startTime.AddMilliseconds(1));
                            pcd.NextEvent(Cycle.EventType.ChangeToYellow, startTime.AddMilliseconds(2));
                        }

                    }

                    //Check to see if the event is a change to red
                    //The 64 event is for overlaps.
                    if (row.EventCode == 10 || row.EventCode == 64)
                    {
                        //If it is red and the pcd group is empy create a new one
                        if (pcd == null)
                        {
                            pcd = new Cycle(row.Timestamp);
                        }
                        //If the group is not empty than it is the end of the group and the start
                        //of the next group
                        else
                        {
                            pcd.NextEvent(GetEventType(row.EventCode), row.Timestamp);
                            //if the nextevent response is complete add it and start the next group
                            if (pcd.Status == Cycle.NextEventResponse.GroupComplete)
                            {
                                //pcd.setdetectorcollection(detectortable);
                                CycleCollection.Add(pcd);
                                pcd = new Cycle(row.Timestamp);
                            }
                        }
                    }
                    //If the event is not red and the group is not empty
                    //add the event and set the next event
                    else if (pcd != null)
                    {
                        pcd.NextEvent(GetEventType(row.EventCode), row.Timestamp);
                        if (pcd.Status == Cycle.NextEventResponse.GroupComplete)
                        {
                            CycleCollection.Add(pcd);
                            pcd = new Cycle(row.Timestamp);
                        }
                    }
                    if (pcd != null && pcd.Status == Cycle.NextEventResponse.GroupMissingData)
                    {
                        pcd = null;
                    }

                    //If this is the last PCD Group we need to handle a special case
                    //where the pcd starts at the start of the requested period to 
                    //make sure we include all data 
                    else if (counter == cycleEvents.Count() && pcd != null)
                    {
                        //if the last event is red create a new group to consume the remaining 
                        //time in the period

                        if (GetEventType(row.EventCode) == Cycle.EventType.ChangeToRed)
                        {
                            pcd.NextEvent(Cycle.EventType.ChangeToGreen, endTime.AddMilliseconds(-2));
                            pcd.NextEvent(Cycle.EventType.ChangeToYellow, endTime.AddMilliseconds(-1));
                            pcd.NextEvent(Cycle.EventType.ChangeToRed, endTime);
                        }
                        else if (GetEventType(row.EventCode) == Cycle.EventType.ChangeToGreen)
                        {
                            pcd.NextEvent(Cycle.EventType.ChangeToYellow, endTime.AddMilliseconds(-1));
                            pcd.NextEvent(Cycle.EventType.ChangeToRed, endTime);
                        }
                        else if (GetEventType(row.EventCode) == Cycle.EventType.ChangeToYellow)
                        {
                            pcd.NextEvent(Cycle.EventType.ChangeToRed, endTime);
                        }

                        if (pcd.Status != Cycle.NextEventResponse.GroupMissingData)
                        {
                            CycleCollection.Add(pcd);
                        }
                    }
                }
            }
            //if there are no records at all for the selected time, then the line
            //and counts don't show.  This next bit fixes that.
            if (CycleCollection.Count == 0 && (startTime != endTime))
            {
                //then we need to make a dummy PDC group
                //the pcd assumes it starts on red.
                pcd = new Cycle(startTime);

                //and find out what phase state the controller was in by looking for the next phase event 
                //after the end of the plan.
                
                MOE.Common.Models.Controller_Event_Log eventBeforePattern = null;
                try
                {
                    eventBeforePattern = MOE.Common.Business.ControllerEventLogs.GetEventBeforeEvent(this.SignalID, this.PhaseNumber, startTime);

                }
                finally
                {

                }



                if (eventBeforePattern != null)
                {
                    if (GetEventType(eventBeforePattern.EventCode) == Cycle.EventType.ChangeToRed)
                    {
                        //let it dwell in red (we don't have to add anything).

                        //then add a green phase, a yellow phase and a red phase at the end to complete the cycle
                        pcd.NextEvent(Cycle.EventType.ChangeToGreen, endTime.AddMilliseconds(-2));
                        pcd.NextEvent(Cycle.EventType.ChangeToYellow, endTime.AddMilliseconds(-1));
                        pcd.NextEvent(Cycle.EventType.ChangeToRed, endTime);

                    }

                    else if (GetEventType(eventBeforePattern.EventCode) == Cycle.EventType.ChangeToYellow)
                    {
                        //we were in yellow, though this will probably never happen
                        //We have to add a green to our dummy phase.

                        pcd.NextEvent(Cycle.EventType.ChangeToGreen, startTime.AddMilliseconds(1));
                        //then make it dwell in yellow
                        pcd.NextEvent(Cycle.EventType.ChangeToYellow, startTime.AddMilliseconds(2));
                        //then add a red phase at the end to complete the cycle
                        pcd.NextEvent(Cycle.EventType.ChangeToRed, endTime);


                    }

                    else if (GetEventType(eventBeforePattern.EventCode) == Cycle.EventType.ChangeToGreen)
                    {

                        // make it dwell in green
                        pcd.NextEvent(Cycle.EventType.ChangeToGreen, startTime.AddMilliseconds(1));

                        //then add a yellow phase and a red phase at the end to complete the cycle
                        pcd.NextEvent(Cycle.EventType.ChangeToYellow, endTime.AddMilliseconds(-1));
                        pcd.NextEvent(Cycle.EventType.ChangeToRed, endTime);


                    }


                }
                if (pcd.Status == Cycle.NextEventResponse.GroupComplete)
                {
                    CycleCollection.Add(pcd);
                }
            }



            AddDetectorData(detectorEvents);
            AddPreemptData(preemptEvents);

        }

        public void LinkPivotAddDetectorData(List<Models.Controller_Event_Log> detectorEvents)
        {
            totalArrivalOnRed = -1;
            totalVolume = -1;
            totalGreenTime = -1;
            totalArrivalOnGreen = -1;
       
            foreach (Cycle pcd in CycleCollection)
            {
                pcd.ClearDetectorData();                
            }
            AddDetectorData(detectorEvents);
        }

        private void AddDetectorData(List<Models.Controller_Event_Log> detectorEvents)
        {
            totalArrivalOnRed = -1;
            totalVolume = -1;
            totalGreenTime = -1;
            totalArrivalOnGreen = -1;

            //TODO:This goes through all detector activations for each plan. It may be quicker to only
            //go through relevant detectors
            foreach (MOE.Common.Models.Controller_Event_Log row in detectorEvents)
            {
                var query = from item in CycleCollection
                            where item.StartTime < row.Timestamp && item.EndTime > row.Timestamp
                            select item;


                foreach (var pcd in query)
                {
                    DetectorDataPoint ddp = new DetectorDataPoint(pcd.StartTime, row.Timestamp, 
                        pcd.GreenEvent, pcd.EndTime);
                    pcd.AddDetector(ddp);
                }

            }
        }

        private void AddPreemptData(List<Models.Controller_Event_Log> preemptEvents)
        {
            foreach (MOE.Common.Models.Controller_Event_Log row in preemptEvents)
            {
                var query = from item in CycleCollection
                            where item.StartTime < row.Timestamp && item.EndTime > row.Timestamp
                            select item;


                foreach (var pcd in query)
                {
                    DetectorDataPoint ddp = new DetectorDataPoint(pcd.StartTime, row.Timestamp, 
                        pcd.GreenEvent, pcd.EndTime);
                    pcd.AddPreempt(ddp);
                }

            }
        }

        public void SetSpeedStatistics(int minSpeedFilter)
        {


            List<int> rawSpeeds = new List<int>();

            //get the speed hits for the plan
            List<Cycle> cycles = (from cycle in this.CycleCollection
                        where (cycle.StartTime > this.startTime && cycle.EndTime < this.endTime)
                        select cycle).ToList();

            foreach (Cycle Cy in cycles)
            {
                foreach (Models.Speed_Events speed in Cy.SpeedsForCycle)
                {
                    if (speed.MPH > minSpeedFilter)
                    {
                        rawSpeeds.Add(speed.MPH);
                    }
                }
            }

            //find stddev of average
            if (rawSpeeds.Count > 0)
            {
                double rawaverage = rawSpeeds.Average();
                AvgSpeed = Convert.ToInt32(Math.Round(rawaverage));
                StdDevAvgSpeed = Convert.ToInt32(Math.Round(Math.Sqrt(rawSpeeds.Average(v => Math.Pow(v - rawaverage, 2)))));
            }

            //Find 85% of raw speeds
            rawSpeeds.Sort();
            if (rawSpeeds.Count > 3)
            {
                double EighyFiveIndex = ((rawSpeeds.Count * .85) + .5);
                int EighyFiveIndexInt = 0;
                if ((EighyFiveIndex % 1) == 0)
                {
                    EighyFiveIndexInt = Convert.ToInt16(EighyFiveIndex);
                    this.EightyFifth = rawSpeeds.ElementAt(EighyFiveIndexInt - 2);
                }
                else
                {
                    double IndexMod = (EighyFiveIndex % 1);
                    EighyFiveIndexInt = Convert.ToInt16(EighyFiveIndex);
                    int Speed1 = rawSpeeds.ElementAt(EighyFiveIndexInt - 2);
                    int Speed2 = rawSpeeds.ElementAt(EighyFiveIndexInt - 1);
                    double RawEightyfifth = (1 - IndexMod) * Speed1 + IndexMod * Speed2;
                    this.EightyFifth = Convert.ToInt32(Math.Round(RawEightyfifth));
                }

            }


        }
    
   
    }
}
