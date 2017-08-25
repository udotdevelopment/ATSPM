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

        protected PlansBase _Plans;

        public PedDelaySignal(string signalID, DateTime startDate,
            DateTime endDate) 
        {
            _SignalID = signalID;
            _StartDate = startDate;
            _EndDate = endDate;

            _Plans = new PlansBase(signalID, startDate, endDate);

            List<int> pedPhaseNumbers = ControllerEventLogs.GetPedPhases(signalID, startDate, endDate);
            
            Parallel.ForEach(pedPhaseNumbers, currentPhase =>
            //foreach(int currentPhase in pedPhaseNumbers)
            {
                _PedPhases.Add(new PedPhase(currentPhase, signalID, startDate, endDate, _Plans));
            }
                );
            _PedPhases.Sort((x, y) => x.PhaseNumber.CompareTo(y.PhaseNumber));
        }
    }
}
