using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.TimingAndActuations
{
    public class TimingAndActuationsForPhase
    {
        private readonly bool _getPermissivePhase;
        private readonly int _phaseNumber;
        public Approach Approach { get; set; }
        public List<Plan> Plans { get; set; }
        public TimingAndActuationsOptions Options { get; }
        public List<TimingAndActuationCycle> Cycles { get; set; }

        private List<Models.Controller_Event_Log> PedestrianEvents { get; set; }
        private List<Controller_Event_Log> StopBarEvents { get; set; }
        private List<Controller_Event_Log> LaneByLaneEvents { get; set; }
        private List<Controller_Event_Log> AdvancePresenceEvents { get; set; }
        private List<Controller_Event_Log> AdvanceCountEvents { get; set; }
        private List<Controller_Event_Log> PhaseCustom1Events { get; set; }
        private List<Controller_Event_Log> PhaseCustom2Events { get; set; }
        private List<Controller_Event_Log> GlobalCustom1Events { get; set; }
        private List<Controller_Event_Log> GlobalCustom2Events { get; set; }

        public TimingAndActuationsForPhase(Approach approach, List<Plan> plans, bool getPermissivePhase, TimingAndActuationsOptions options)
        {
            _getPermissivePhase = getPermissivePhase;
            Approach = approach;
            Plans = plans;
            Options = options;
            _phaseNumber = getPermissivePhase ? Approach.PermissivePhaseNumber.Value : Approach.ProtectedPhaseNumber;
            Cycles = CycleFactory.GetTimingAndActuationCycles(Options.StartDate, Options.EndDate, approach, getPermissivePhase);

            GetPedestrianEvents();
        }

        private void GetPedestrianEvents()
        {
            var controllerEventLogRepository = MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            PedestrianEvents = controllerEventLogRepository.GetEventsByEventCodesParam(Approach.SignalID, Options.StartDate, Options.EndDate, new List<int> {21, 22, 23, 89, 90}, _phaseNumber);


        }
    }
}
