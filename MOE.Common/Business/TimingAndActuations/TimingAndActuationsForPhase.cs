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
        public Approach Approach { get; set; }
        public List<Plan> Plans { get; set; }
        public TimingAndActuationsOptions Options { get; }
        public List<TimingAndActuationCycle> Cycles { get; set; }

        private List<ControllerEventLogs> PedestrianEvents { get; set; }
        private List<ControllerEventLogs> StopBarEvents { get; set; }
        private List<ControllerEventLogs> LaneByLaneEvents { get; set; }
        private List<ControllerEventLogs> AdvancePresenceEvents { get; set; }
        private List<ControllerEventLogs> AdvanceCountEvents { get; set; }
        private List<ControllerEventLogs> PhaseCustom1Events { get; set; }
        private List<ControllerEventLogs> PhaseCustom2Events { get; set; }
        private List<ControllerEventLogs> GlobalCustom1Events { get; set; }
        private List<ControllerEventLogs> GlobalCustom2Events { get; set; }

        public TimingAndActuationsForPhase(Approach approach, List<Plan> plans, bool getPermissivePhase, TimingAndActuationsOptions options)
        {
            Approach = approach;
            Plans = plans;
            Options = options;
            Cycles = CycleFactory.GetTimingAndActuationCycles(Options.StartDate, Options.EndDate, approach, getPermissivePhase);
        }
    }
}
