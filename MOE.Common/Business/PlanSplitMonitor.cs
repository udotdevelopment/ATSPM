using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MOE.Common.Business
{
    public class PlanSplitMonitor:Plan
    {
        
        public PlanSplitMonitor(DateTime start, DateTime end, int planNumber, Models.Approach approach, List<Cycle> cyclesForPlan):base( start,  end,  planNumber, approach,  cyclesForPlan)
        {
        }

        public void SetProgrammedSplits(string signalId)
        {
            Splits.Clear();
            List<int> l = new List<int>();
            for (int i = 130; i <= 151; i++)
            {
                l.Add(i);
            }
            ControllerEventLogs splitsDt = new ControllerEventLogs(signalId, this.StartTime, this.StartTime.AddSeconds(2), l);
            foreach (MOE.Common.Models.Controller_Event_Log row in splitsDt.Events)
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

        public void SetHighCycleCount(Business.AnalysisPhaseCollection phases)
        {
            //find all the phases cycles within the plan
            int HighCycleCount = 0;
            foreach (Business.AnalysisPhase phase in phases.Items)
            {
                var Cycles = from cycle in phase.Cycles.Items
                    where cycle.StartTime > this.StartTime && cycle.EndTime < this.EndTime
                    select cycle;

                if (Cycles.Count() > HighCycleCount)
                {
                    HighCycleCount = Cycles.Count();
                }
            }
            CycleCount = HighCycleCount;
        }

    }
}
