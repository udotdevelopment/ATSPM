using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.PEDDelay
{
    public class PedDelaySignal
    {
        protected string _SignalID;
        public string SignalID { get { return _SignalID; } }

        protected DateTime _StartDate;
        public DateTime StartDate { get { return _StartDate; } }

        protected DateTime _EndDate;
        public DateTime EndDate { get { return _EndDate; } }

        protected List<PedPhase> _PedPhases = new List<PedPhase>();
        public List<PedPhase> PedPhases { get { return _PedPhases; } }

        protected List<PlanBase> Plans;

        public PedDelaySignal(string signalId, DateTime startDate,
            DateTime endDate) 
        {
            _SignalID = signalId;
            _StartDate = startDate;
            _EndDate = endDate;

            Plans =  PlanFactory.GetPlansFromDatabase(signalId, startDate, endDate);

            List<int> pedPhaseNumbers = ControllerEventLogs.GetPedPhases(signalId, startDate, endDate);
            
            Parallel.ForEach(pedPhaseNumbers, currentPhase =>
            //foreach(int currentPhase in pedPhaseNumbers)
            {
                _PedPhases.Add(new PedPhase(currentPhase, signalId, startDate, endDate, Plans));
            }
                );
            _PedPhases.Sort((x, y) => x.PhaseNumber.CompareTo(y.PhaseNumber));
        }
    }
}
