using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.CustomReport
{
    public class Signal
    {
        private List<Models.Detector> _Detectors;

        public List<Models.Detector> Detectors
        {
            get { return _Detectors; }
            set { _Detectors = value; }
        }

        private List<Phase> _Phases = new List<Phase>();

        public List<Phase> Phases
        {
            get { return _Phases; }
            set { _Phases = value; }
        }

        private PlansBase _Plans;

        public PlansBase Plans
        {
            get { return _Plans; }
            set { _Plans = value; }
        }

        private string _SignalID;

        public string SignalID
        {
            get { return _SignalID; }
            set { _SignalID = value; }
        }

        private DateTime _StartDate;

        public DateTime StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; }
        }

        private DateTime _EndDate;

        public DateTime EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; }
        }

        private List<int> _EventCodes;

        public List<int> EventCodes
        {
            get { return _EventCodes; }
            set { _EventCodes = value; }
        }

        public Models.Signal SignalModel { get; set; }

        public Signal(string signalID, DateTime startDate, DateTime endDate, 
            List<int> eventCodes, int startOfCycle)
        {
            _SignalID = signalID;
            _StartDate = startDate;
            _EndDate = endDate;
            _EventCodes = eventCodes;
            MOE.Common.Models.Repositories.ISignalsRepository repository =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            SignalModel = repository.GetSignalBySignalID(signalID);
            _Detectors = SignalModel.GetDetectorsForSignal();
            _Plans = new PlansBase(signalID, startDate, endDate);
            
            GetPhases(startOfCycle);
        }

        private void GetPhases(int startOfCycle)
        {

            foreach (Models.Approach a in SignalModel.Approaches)
            {
                
                _Phases.Add(new Phase( a,StartDate, _EndDate, _EventCodes, startOfCycle, false));

                if (a.PermissivePhaseNumber != null && a.PermissivePhaseNumber > 0)
                {
                    _Phases.Add(new Phase(a, StartDate, _EndDate, _EventCodes, startOfCycle, true));
                }
            }
        }

    }
}
