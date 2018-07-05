using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOE.Common.Business.PEDDelay
{
    public class PedDelaySignal
    {
        protected DateTime _EndDate;

        protected List<PedPhase> _PedPhases = new List<PedPhase>();

        protected PlansBase _Plans;
        protected string _SignalID;

        protected DateTime _StartDate;

        public PedDelaySignal(string signalID, DateTime startDate,
            DateTime endDate)
        {
            _SignalID = signalId;
            _StartDate = startDate;
            _EndDate = endDate;

            Plans =  PlanFactory.GetPlansFromDatabase(signalId, startDate, endDate);

            var pedPhaseNumbers = ControllerEventLogs.GetPedPhases(signalID, startDate, endDate);

            Parallel.ForEach(pedPhaseNumbers, currentPhase =>
                    //foreach(int currentPhase in pedPhaseNumbers)
                {
                    _PedPhases.Add(new PedPhase(currentPhase, signalID, startDate, endDate, _Plans));
                }
            );
            _PedPhases = _PedPhases.OrderBy(x => x.PhaseNumber).ToList();
        }

        public string SignalID => _SignalID;
        public DateTime StartDate => _StartDate;
        public DateTime EndDate => _EndDate;
        public List<PedPhase> PedPhases => _PedPhases;
    }
}