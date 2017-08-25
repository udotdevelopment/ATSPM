using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MOE.Common.Business
{
    public class RLMPlan
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

        public double Violations
        {
            get
            {
                return RLMCycleCollection.Sum(d => d.Violations);
            }
        }

        public double YellowOccurrences
        {
            get
            {
                return RLMCycleCollection.Sum(d => d.YellowOccurrences);
            }
        }
    
       

        protected List<RLMCycle> rlmCycleCollection = new List<RLMCycle>();
        public List<RLMCycle> RLMCycleCollection
        {
            get
            {
                return rlmCycleCollection;
            }
        }
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


       


       

      

        //private double totalplantime;
        //private double totalgreenphasetime;
        //public SortedDictionary<int,int> phaseCountDictionary = new SortedDictionary<int,int>();

        public SortedDictionary<int, int> Splits = new SortedDictionary<int, int>();

        /// <summary>
        /// The plan number
        /// </summary>
        private int planNumber;
        public int PlanNumber
        {
            get
            {
                return planNumber;
            }
        }

        private double srlvSeconds = 0;
        public double SRLVSeconds
        {
            get
            {
                return srlvSeconds;
            }
        }

        public double Srlv
        {
            get
            {
                return RLMCycleCollection.Sum(d => d.Srlv);
            }
        }

        private double totalVolume = 0;
        public double TotalVolume
        {
            get
            {
                return totalVolume;
            }
        }

        
        public double TotalViolationTime
        {
            get
            {
                return RLMCycleCollection.Sum(d => d.TotalViolationTime);
            }
        }

        public double TotalYellowTime
        {
            get
            {
                return RLMCycleCollection.Sum(d => d.TotalYellowTime);
            }
        }

        public double AverageTRLV
        {
            get
            {
                return Math.Round(TotalViolationTime/Violations,1);
            }
        }

        public double AverageTYLO
        {
            get
            {
                return Math.Round(TotalYellowTime / YellowOccurrences, 1);
            }
        }

        public double PercentViolations
        {
            get
            {
                if(TotalVolume > 0)
                {
                    return Math.Round((Violations / TotalVolume)*100,0);
                }
                else
                {
                    return 0;
                }
            }
        }

        public double PercentYellowOccurrences
        {
            get
            {
                if (TotalVolume > 0)
                {
                    return Math.Round((YellowOccurrences / TotalVolume) * 100, 0);
                }
                else
                {
                    return 0;
                }
            }
        }

        public double PercentSevereViolations
        {
            get
            {
                if (TotalVolume > 0)
                {
                    return Math.Round((Srlv / TotalVolume) * 100, 2);
                }
                else
                {
                    return 0;
                }
            }
        }

        public MOE.Common.Models.Approach Approach { get; set; }


        public RLMPlan(DateTime start, DateTime end, int planNumber,
            List<MOE.Common.Models.Controller_Event_Log> cycleEvents, 
            double srlvSeconds, Models.Approach approach)
        {
            Approach = approach;
            startTime = start;
            endTime = end;
            this.planNumber = planNumber;
            bool usePermissivePhase = false;
            MOE.Common.Models.SPM db = new MOE.Common.Models.SPM();
            List<int> l = new List<int> { 1, 8, 9, 10, 11 };
            List<MOE.Common.Models.Controller_Event_Log> permEvents = null;
            if (Approach.PermissivePhaseNumber != null)
            {
                usePermissivePhase = true;
                MOE.Common.Models.Repositories.IControllerEventLogRepository cveRepository =
                MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();
                permEvents = cveRepository.GetEventsByEventCodesParam(Approach.SignalID,
                    start, end, l, Approach.ProtectedPhaseNumber);
                foreach (Models.Controller_Event_Log row in cycleEvents)
                {
                    if (row.Timestamp >= start && row.Timestamp <= end && GetEventType(row.EventCode) == RLMCycle.EventType.BeginYellowClearance)
                    {                        
                        foreach (MOE.Common.Models.Controller_Event_Log permRow in permEvents)
                        {
                            if(GetEventType(permRow.EventCode) == RLMCycle.EventType.BeginYellowClearance)
                            {
                                if(row.Timestamp == permRow.Timestamp)
                                {
                                    usePermissivePhase = false;
                                }
                            }
                        }
                    }
                }
            }
            this.srlvSeconds = srlvSeconds;
            startTime = start;
            endTime = end;
            if (usePermissivePhase)
            {
               GetRedCycle(start, end, permEvents);
            }
            else
            { 
                GetRedCycle(start, end, cycleEvents);                
            }
        }

        public RLMPlan(DateTime start, DateTime end, int plan, double srlvSeconds, MOE.Common.Models.Approach approach)
        {
            this.srlvSeconds = srlvSeconds;
            startTime = start;
            endTime = end;
            planNumber = plan;
            Approach = approach;            
        }

        private RLMCycle.EventType GetEventType(int EventCode)
        {
            switch (EventCode)
            {
                case 8:
                    return RLMCycle.EventType.BeginYellowClearance;
                // overlap yellow
                case 63:
                    return RLMCycle.EventType.BeginYellowClearance;
                
                case 9:
                    return RLMCycle.EventType.BeginRedClearance;
                // overlap red
                case 64:
                    return RLMCycle.EventType.BeginRedClearance;  

                case 11:
                    return RLMCycle.EventType.BeginRed;
                
                case 1:
                    return RLMCycle.EventType.EndRed;
                // overlap green
                case 61:
                    return RLMCycle.EventType.EndRed;
                
                default:
                    return RLMCycle.EventType.Unknown;
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

        public void SetProgrammedSplits(string signalID)
        {

            MOE.Common.Models.Repositories.IControllerEventLogRepository cer = MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            List<MOE.Common.Models.Controller_Event_Log> SplitsDT = cer.GetSplitEvents(signalID, startTime, endTime);
            Splits.Clear();

            foreach (MOE.Common.Models.Controller_Event_Log row in SplitsDT)
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

        private void GetRedCycle(DateTime startTime, DateTime endTime,
            List<MOE.Common.Models.Controller_Event_Log> cycleEvents)
        {
           
            RLMCycle cycle = null;
            //use a counter to help determine when we are on the last row
            int counter = 0;

            foreach (Models.Controller_Event_Log row in cycleEvents)
            {
                
                //use a counter to help determine when we are on the last row
                counter++;
                if (row.Timestamp >= startTime && row.Timestamp <= endTime)
                {                    
                    if (cycle == null && GetEventType(row.EventCode)== RLMCycle.EventType.BeginYellowClearance)
                    {
                        cycle = new RLMCycle(row.Timestamp, SRLVSeconds);
                        cycle.NextEvent(GetEventType(row.EventCode), row.Timestamp);
                        if(cycle.Status == RLMCycle.NextEventResponse.GroupMissingData)
                        {
                            cycle = null;
                        }
                    }
                    else if (cycle != null)
                    {
                        cycle.NextEvent(GetEventType(row.EventCode), row.Timestamp);
                        if (cycle.Status == RLMCycle.NextEventResponse.GroupComplete)
                        {
                            rlmCycleCollection.Add(cycle);
                            cycle = null;
                        }
                        else if (cycle.Status == RLMCycle.NextEventResponse.GroupMissingData)
                        {
                            cycle = null;
                        }
                    }

                }
            }
            AddDetectorData();
        }

        private void AddDetectorData()
        {
            MOE.Common.Models.Repositories.IControllerEventLogRepository repository =
                MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            var detectors = Approach.GetDetectorsForMetricType(11);
            List<Models.Controller_Event_Log> detectorActivations = new List<Models.Controller_Event_Log>();
            foreach(var d in detectors)
            {
                detectorActivations.AddRange(repository.GetEventsByEventCodesParam(Approach.SignalID, this.StartTime, this.EndTime,
                    new List<int> { 82 }, d.DetChannel));
            }
            this.totalVolume = detectorActivations.Count;
            foreach(RLMCycle cycle in rlmCycleCollection)
            {
                var events = detectorActivations.Where(d => d.Timestamp >= cycle.StartTime && d.Timestamp < cycle.EndTime);
                foreach(var cve in events)
                {
                    RLMDetectorDataPoint ddp = new RLMDetectorDataPoint(cycle.StartTime, cve.Timestamp);
                    cycle.AddDetector(ddp);
                }
            }
        }
    }
}
