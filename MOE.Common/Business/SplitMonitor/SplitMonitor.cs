using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Migrations;

namespace MOE.Common.Business.SplitMonitor
{
    public class SplitMonitor
    {

        public SplitMonitor(string signalID, DateTime startDate, DateTime endDate)
        {
            SignalID = signalID;
            StartDate = startDate;
            EndDate = endDate;
            Phases = new AnalysisPhaseCollection(SignalID, StartDate, EndDate);
            if (Phases.Items.Count > 0)
            {
                foreach (var plan in Phases.Plans)
                {
                    plan.SetProgrammedSplits(SignalID);
                    plan.SetHighCycleCount(Phases);
                }
            }

            var phasesInOrder = (Phases.Items.Select(r => r)).OrderBy(r => r.PhaseNumber);
            foreach (var phase in phasesInOrder)
            {
                if (phase.Cycles.Items.Count > 0)
                {
                    var maxSplitLength = 0;
                    foreach (var plan in Phases.Plans)
                    {
                        var highestSplit = plan.FindHighestRecordedSplitPhase();
                        plan.FillMissingSplits(highestSplit);
                        if (plan.Splits[phase.PhaseNumber] > maxSplitLength)
                            maxSplitLength = plan.Splits[phase.PhaseNumber];
                    }
                }
            }
        }

        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public AnalysisPhaseCollection Phases { get; private set; }
        public string SignalID { get; }
    }
}
