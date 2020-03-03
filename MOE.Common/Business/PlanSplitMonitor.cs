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

                if (row.EventCode == 348 && !Splits.ContainsKey(17))
                    Splits.Add(17, row.EventParam);
                else if (row.EventCode == 348 && row.EventParam > 0)
                    Splits[17] = row.EventParam;

                if (row.EventCode == 349 && !Splits.ContainsKey(18))
                    Splits.Add(18, row.EventParam);
                else if (row.EventCode == 349 && row.EventParam > 0)
                    Splits[18] = row.EventParam;

                if (row.EventCode == 350 && !Splits.ContainsKey(19))
                    Splits.Add(19, row.EventParam);
                else if (row.EventCode == 350 && row.EventParam > 0)
                    Splits[19] = row.EventParam;

                if (row.EventCode == 351 && !Splits.ContainsKey(20))
                    Splits.Add(20, row.EventParam);
                else if (row.EventCode == 351 && row.EventParam > 0)
                    Splits[20] = row.EventParam;

                if (row.EventCode == 352 && !Splits.ContainsKey(21))
                    Splits.Add(21, row.EventParam);
                else if (row.EventCode == 352 && row.EventParam > 0)
                    Splits[21] = row.EventParam;

                if (row.EventCode == 353 && !Splits.ContainsKey(22))
                    Splits.Add(22, row.EventParam);
                else if (row.EventCode == 353 && row.EventParam > 0)
                    Splits[22] = row.EventParam;

                if (row.EventCode == 354 && !Splits.ContainsKey(23))
                    Splits.Add(23, row.EventParam);
                else if (row.EventCode == 354 && row.EventParam > 0)
                    Splits[23] = row.EventParam;

                if (row.EventCode == 355 && !Splits.ContainsKey(24))
                    Splits.Add(24, row.EventParam);
                else if (row.EventCode == 355 && row.EventParam > 0)
                    Splits[24] = row.EventParam;

                if (row.EventCode == 356 && !Splits.ContainsKey(25))
                    Splits.Add(25, row.EventParam);
                else if (row.EventCode == 356 && row.EventParam > 0)
                    Splits[25] = row.EventParam;

                if (row.EventCode == 357 && !Splits.ContainsKey(26))
                    Splits.Add(26, row.EventParam);
                else if (row.EventCode == 357 && row.EventParam > 0)
                    Splits[26] = row.EventParam;

                if (row.EventCode == 358 && !Splits.ContainsKey(27))
                    Splits.Add(27, row.EventParam);
                else if (row.EventCode == 358 && row.EventParam > 0)
                    Splits[27] = row.EventParam;

                if (row.EventCode == 359 && !Splits.ContainsKey(28))
                    Splits.Add(28, row.EventParam);
                else if (row.EventCode == 359 && row.EventParam > 0)
                    Splits[28] = row.EventParam;

                if (row.EventCode == 360 && !Splits.ContainsKey(29))
                    Splits.Add(29, row.EventParam);
                else if (row.EventCode == 360 && row.EventParam > 0)
                    Splits[29] = row.EventParam;

                if (row.EventCode == 361 && !Splits.ContainsKey(30))
                    Splits.Add(30, row.EventParam);
                else if (row.EventCode == 361 && row.EventParam > 0)
                    Splits[30] = row.EventParam;

                if (row.EventCode == 362 && !Splits.ContainsKey(31))
                    Splits.Add(31, row.EventParam);
                else if (row.EventCode == 362 && row.EventParam > 0)
                    Splits[31] = row.EventParam;

                if (row.EventCode == 363 && !Splits.ContainsKey(32))
                    Splits.Add(32, row.EventParam);
                else if (row.EventCode == 363 && row.EventParam > 0)
                    Splits[32] = row.EventParam;

                if (row.EventCode == 364 && !Splits.ContainsKey(33))
                    Splits.Add(33, row.EventParam);
                else if (row.EventCode == 364 && row.EventParam > 0)
                    Splits[33] = row.EventParam;

                if (row.EventCode == 365 && !Splits.ContainsKey(34))
                    Splits.Add(34, row.EventParam);
                else if (row.EventCode == 365 && row.EventParam > 0)
                    Splits[34] = row.EventParam;

                if (row.EventCode == 366 && !Splits.ContainsKey(35))
                    Splits.Add(35, row.EventParam);
                else if (row.EventCode == 366 && row.EventParam > 0)
                    Splits[35] = row.EventParam;

                if (row.EventCode == 367 && !Splits.ContainsKey(36))
                    Splits.Add(36, row.EventParam);
                else if (row.EventCode == 367 && row.EventParam > 0)
                    Splits[36] = row.EventParam;

                if (row.EventCode == 368 && !Splits.ContainsKey(37))
                    Splits.Add(37, row.EventParam);
                else if (row.EventCode == 368 && row.EventParam > 0)
                    Splits[37] = row.EventParam;

                if (row.EventCode == 369 && !Splits.ContainsKey(38))
                    Splits.Add(38, row.EventParam);
                else if (row.EventCode == 369 && row.EventParam > 0)
                    Splits[38] = row.EventParam;

                if (row.EventCode == 370 && !Splits.ContainsKey(39))
                    Splits.Add(39, row.EventParam);
                else if (row.EventCode == 370 && row.EventParam > 0)
                    Splits[39] = row.EventParam;

                if (row.EventCode == 371 && !Splits.ContainsKey(40))
                    Splits.Add(40, row.EventParam);
                else if (row.EventCode == 371 && row.EventParam > 0)
                    Splits[40] = row.EventParam;


            }

            if (Splits.Count == 0)
                for (var i = 0; i < 40; i++)
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