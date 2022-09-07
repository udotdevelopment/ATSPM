using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business
{
    public class RLMPlan
    {
        protected int cycleCount;


        /// <summary>
        ///     The end time of the plan
        /// </summary>
        protected DateTime endTime;

        /// <summary>
        ///     The plan number
        /// </summary>
        private readonly int planNumber;


        protected List<RLMCycle> rlmCycleCollection = new List<RLMCycle>();


        //private double totalplantime;
        //private double totalgreenphasetime;
        //public SortedDictionary<int,int> phaseCountDictionary = new SortedDictionary<int,int>();

        public SortedDictionary<int, int> Splits = new SortedDictionary<int, int>();

        /// <summary>
        ///     The start time of the plan
        /// </summary>
        protected DateTime startTime;


        public RLMPlan(DateTime start, DateTime end, int planNumber,
            List<RLMCycle> cycles,
            double srlvSeconds, Approach approach)
        {
            Approach = approach;
            startTime = start;
            endTime = end;
            this.planNumber = planNumber;
            SRLVSeconds = srlvSeconds;
            startTime = start;
            endTime = end;
            RLMCycleCollection = cycles.Where(c => c.StartTime >= startTime && c.StartTime < endTime).ToList();
            //GetRedCycle(start, end, cycleEvents);
            CycleCount = RLMCycleCollection.Count;
        }

        public RLMPlan(DateTime start, DateTime end, int plan, double srlvSeconds, Approach approach)
        {
            SRLVSeconds = srlvSeconds;
            startTime = start;
            endTime = end;
            planNumber = plan;
            Approach = approach;
        }

        public DateTime StartTime => startTime;

        public double Violations
        {
            get { return RLMCycleCollection.Sum(d => d.Violations); }
        }

        public double YellowOccurrences
        {
            get { return RLMCycleCollection.Sum(d => d.YellowOccurrences); }
        }

        public List<RLMCycle> RLMCycleCollection
        {
            get => rlmCycleCollection;
            set => rlmCycleCollection = value;
        }

        public DateTime EndTime => endTime;

        public int CycleCount
        {
            get => cycleCount;
            set => cycleCount = value;
        }

        public int CycleLength { get; set; }

        public int OffsetLength { get; set; }

        public int PlanNumber => planNumber;

        public double SRLVSeconds { get; }

        public double SevereRedLightViolations
        {
            get { return RLMCycleCollection.Sum(d => d.SevereRedLightViolations); }
        }

        public double TotalVolume { get; private set; }


        public double TotalViolationTime
        {
            get { return RLMCycleCollection.Sum(d => d.TotalViolationTime); }
        }

        public double TotalYellowTime
        {
            get { return RLMCycleCollection.Sum(d => d.TotalYellowTime); }
        }

        public double AverageTRLV => Math.Round(TotalViolationTime / Violations, 1);

        public double AverageTYLO => Math.Round(TotalYellowTime / YellowOccurrences, 1);

        public double PercentViolations
        {
            get
            {
                if (TotalVolume > 0)
                    return Math.Round(Violations / TotalVolume * 100, 0);
                return 0;
            }
        }

        public double PercentYellowOccurrences
        {
            get
            {
                if (TotalVolume > 0)
                    return Math.Round(YellowOccurrences / TotalVolume * 100, 0);
                return 0;
            }
        }

        public double PercentSevereViolations
        {
            get
            {
                if (TotalVolume > 0)
                    return Math.Round(SevereRedLightViolations / TotalVolume * 100, 2);
                return 0;
            }
        }

        public Approach Approach { get; set; }
        public double ViolationTime
        {
            get { return rlmCycleCollection.Sum(c => c.TotalViolationTime); }
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

                case 65:
                    return RLMCycle.EventType.BeginRed;
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


        public void SetHighCycleCount(AnalysisPhaseCollection phases)
        {
            //find all the phases cycles within the plan
            var HighCycleCount = 0;
            foreach (var phase in phases.Items)
            {
                var Cycles = from cycle in phase.Cycles.Items
                    where cycle.StartTime > StartTime && cycle.EndTime < endTime
                    select cycle;

                if (Cycles.Count() > HighCycleCount)
                    HighCycleCount = Cycles.Count();

                //phaseCountDictionary.Add(phase.PhaseNumber, Cycles.Count());
            }

            cycleCount = HighCycleCount;
        }

        public void SetProgrammedSplits(string signalID)
        {
            var cer = ControllerEventLogRepositoryFactory.Create();
            var SplitsDT = cer.GetSplitEvents(signalID, startTime, endTime);
            Splits.Clear();

            foreach (var row in SplitsDT)
            {
                if (row.EventCode == 132)
                    CycleLength = row.EventParam;

                if (row.EventCode == 133)
                    OffsetLength = row.EventParam;

                if (row.EventCode == 134 && !Splits.ContainsKey(1))
                    Splits.Add(1, row.EventParam);
                else if (row.EventCode == 134 && row.EventParam > 0)
                    Splits[1] = row.EventParam;

                if (row.EventCode == 135 && !Splits.ContainsKey(2))
                    Splits.Add(2, row.EventParam);
                else if (row.EventCode == 135 && row.EventParam > 0)
                    Splits[2] = row.EventParam;

                if (row.EventCode == 136 && !Splits.ContainsKey(3))
                    Splits.Add(3, row.EventParam);
                else if (row.EventCode == 136 && row.EventParam > 0)
                    Splits[3] = row.EventParam;

                if (row.EventCode == 137 && !Splits.ContainsKey(4))
                    Splits.Add(4, row.EventParam);
                else if (row.EventCode == 137 && row.EventParam > 0)
                    Splits[4] = row.EventParam;

                if (row.EventCode == 138 && !Splits.ContainsKey(5))
                    Splits.Add(5, row.EventParam);
                else if (row.EventCode == 138 && row.EventParam > 0)
                    Splits[5] = row.EventParam;

                if (row.EventCode == 139 && !Splits.ContainsKey(6))
                    Splits.Add(6, row.EventParam);
                else if (row.EventCode == 139 && row.EventParam > 0)
                    Splits[6] = row.EventParam;

                if (row.EventCode == 140 && !Splits.ContainsKey(7))
                    Splits.Add(7, row.EventParam);
                else if (row.EventCode == 140 && row.EventParam > 0)
                    Splits[7] = row.EventParam;

                if (row.EventCode == 141 && !Splits.ContainsKey(8))
                    Splits.Add(8, row.EventParam);
                else if (row.EventCode == 141 && row.EventParam > 0)
                    Splits[8] = row.EventParam;

                if (row.EventCode == 142 && !Splits.ContainsKey(9))
                    Splits.Add(9, row.EventParam);
                else if (row.EventCode == 142 && row.EventParam > 0)
                    Splits[9] = row.EventParam;

                if (row.EventCode == 143 && !Splits.ContainsKey(10))
                    Splits.Add(10, row.EventParam);
                else if (row.EventCode == 143 && row.EventParam > 0)
                    Splits[10] = row.EventParam;

                if (row.EventCode == 144 && !Splits.ContainsKey(11))
                    Splits.Add(11, row.EventParam);
                else if (row.EventCode == 144 && row.EventParam > 0)
                    Splits[11] = row.EventParam;

                if (row.EventCode == 145 && !Splits.ContainsKey(12))
                    Splits.Add(12, row.EventParam);
                else if (row.EventCode == 145 && row.EventParam > 0)
                    Splits[12] = row.EventParam;

                if (row.EventCode == 146 && !Splits.ContainsKey(13))
                    Splits.Add(13, row.EventParam);
                else if (row.EventCode == 146 && row.EventParam > 0)
                    Splits[13] = row.EventParam;

                if (row.EventCode == 147 && !Splits.ContainsKey(14))
                    Splits.Add(14, row.EventParam);
                else if (row.EventCode == 147 && row.EventParam > 0)
                    Splits[14] = row.EventParam;

                if (row.EventCode == 148 && !Splits.ContainsKey(15))
                    Splits.Add(15, row.EventParam);
                else if (row.EventCode == 148 && row.EventParam > 0)
                    Splits[15] = row.EventParam;

                if (row.EventCode == 149 && !Splits.ContainsKey(16))
                    Splits.Add(16, row.EventParam);
                else if (row.EventCode == 149 && row.EventParam > 0)
                    Splits[16] = row.EventParam;
            }

            if (Splits.Count == 0)
                for (var i = 0; i < 16; i++)
                    Splits.Add(i, 0);
        }

        public int FindHighestRecordedSplitPhase()
        {
            var phase = 0;

            var maxkey = Splits.Max(x => x.Key);

            phase = maxkey;

            return phase;
        }

        public void FillMissingSplits(int highestSplit)
        {
            for (var counter = 0; counter < highestSplit + 1; counter++)
                if (Splits.ContainsKey(counter))
                {
                }
                else
                {
                    Splits.Add(counter, 0);
                }
        }

        private void GetRedCycle(DateTime startTime, DateTime endTime,
            List<Controller_Event_Log> cycleEvents)
        {
            RLMCycle cycle = null;
            //use a counter to help determine when we are on the last row
            var counter = 0;

            foreach (var row in cycleEvents)
            {
                //use a counter to help determine when we are on the last row
                counter++;
                if (row.Timestamp >= startTime && row.Timestamp <= endTime)
                    if (cycle == null && GetEventType(row.EventCode) == RLMCycle.EventType.BeginYellowClearance)
                    {
                        cycle = new RLMCycle(row.Timestamp, SRLVSeconds);
                        cycle.NextEvent(GetEventType(row.EventCode), row.Timestamp);
                        if (cycle.Status == RLMCycle.NextEventResponse.GroupMissingData)
                            cycle = null;
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
            AddDetectorData();
        }

        private void AddDetectorData()
        {
            var repository =
                ControllerEventLogRepositoryFactory.Create();
            var detectors = Approach.GetDetectorsForMetricType(11);
            var detectorActivations = new List<Controller_Event_Log>();
            foreach (var d in detectors)
                detectorActivations.AddRange(repository.GetEventsByEventCodesParamWithOffsetAndLatencyCorrection(Approach.SignalID,
                    StartTime, EndTime,
                    new List<int> {82}, d.DetChannel, 0, d.LatencyCorrection));
            TotalVolume = detectorActivations.Count;
            foreach (var cycle in rlmCycleCollection)
            {
                var events =
                    detectorActivations.Where(d => d.Timestamp >= cycle.StartTime && d.Timestamp < cycle.EndTime);
                foreach (var cve in events)
                {
                    var ddp = new RLMDetectorDataPoint(cycle.StartTime, cve.Timestamp);
                    cycle.AddDetector(ddp);
                }
            }
        }
    }
}