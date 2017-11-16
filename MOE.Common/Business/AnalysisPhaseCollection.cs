using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
    public class AnalysisPhaseCollection
    {

        public List<AnalysisPhase> Items = new List<AnalysisPhase>();
        public List<PlanSplitMonitor> Plans { get; private set; }
        public int MaxPhaseInUse { get; }
        public string SignalId { get; }

        public AnalysisPhaseCollection(string signalId, DateTime startTime, DateTime endTime, int consecutivecount)
        {
            SignalId = signalId;
            Models.Repositories.IControllerEventLogRepository cel = Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            List<Models.Controller_Event_Log> ptedt = cel.GetSignalEventsByEventCodes(SignalId, startTime, endTime, new List<int>() { 1, 11, 4, 5, 6, 7, 21, 23 });
            List<Models.Controller_Event_Log> dapta = cel.GetSignalEventsByEventCodes(SignalId, startTime, endTime, new List<int>() { 1 });
            ptedt = ptedt.OrderByDescending(i => i.Timestamp).ToList();
            var phasesInUse = dapta.Where(r => r.EventCode == 1).Select(r => r.EventParam).Distinct();
            Plans = PlanFactory.GetSplitMonitorPlans(startTime, endTime, SignalId);
            foreach (int row in phasesInUse)
            {
                AnalysisPhase aPhase = new AnalysisPhase(row, ptedt, consecutivecount);
                Items.Add(aPhase);
            }
            OrderPhases();
            MaxPhaseInUse = FindMaxPhase(Items);
        }

        private void OrderPhases()
        {
            Items = Items.OrderBy(i => i.PhaseNumber).ToList();
        }
        
        public AnalysisPhaseCollection(string signalId, DateTime startTime, DateTime endTime)
        {
            Models.Repositories.IControllerEventLogRepository cel = Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            List<Models.Controller_Event_Log> ptedt = cel.GetSignalEventsByEventCodes(signalId, startTime, endTime, new List<int>() { 1, 11, 4, 5, 6, 7, 21, 23 });
            List<Models.Controller_Event_Log> dapta = cel.GetSignalEventsByEventCodes(signalId, startTime, endTime, new List<int>() { 1 });
            var phasesInUse = dapta.Where(d => d.EventCode == 1).Select(d => d.EventParam).Distinct();
            Plans = PlanFactory.GetSplitMonitorPlans(startTime, endTime, signalId);
            foreach (int row in phasesInUse)
            {
                AnalysisPhase aPhase = new AnalysisPhase( row ,signalId, ptedt);
                Items.Add(aPhase);
            }
            OrderPhases();
        }

        private int FindMaxPhase(List<AnalysisPhase> analysisPhases)
        {
            return analysisPhases.Select(phase => phase.PhaseNumber).Concat(new[] {0}).Max();
        }

    }
}
