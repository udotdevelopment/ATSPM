using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Business
{
    public class PlanSplitMonitor : Plan
    {
        public SortedDictionary<int, int> Splits = new SortedDictionary<int, int>();

        public PlanSplitMonitor(DateTime start, DateTime end, int planNumber) : base(start, end, planNumber)
        {
        }

        public int CycleLength { get; set; }
        public int OffsetLength { get; set; }
        public int CycleCount { get; set; }

        public void SetProgrammedSplits(string signalId)
        {
            Splits.Clear();
            var l = new List<int>();
            for (var i = 130; i <= 151; i++)
                l.Add(i);
            var splitsDt = new ControllerEventLogs(signalId, StartTime, StartTime.AddSeconds(2), l);
            foreach (var row in splitsDt.Events)
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

        public void SetHighCycleCount(AnalysisPhaseCollection phases)
        {
            //find all the phases cycles within the plan
            var HighCycleCount = 0;
            foreach (var phase in phases.Items)
            {
                var Cycles = from cycle in phase.Cycles.Items
                    where cycle.StartTime > StartTime && cycle.EndTime < EndTime
                    select cycle;

                if (Cycles.Count() > HighCycleCount)
                    HighCycleCount = Cycles.Count();
            }
            CycleCount = HighCycleCount;
        }
    }
}