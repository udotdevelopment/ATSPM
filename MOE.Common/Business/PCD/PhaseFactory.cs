using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOE.Common.Business.PCD
{
    class PhaseFactory
    {
        private WCFServiceLibrary.PCDOptions options;
        public List<Plan> Plans;
        public List<PCD.Phase> Phases { get; set; }

        public PhaseFactory(WCFServiceLibrary.PCDOptions options, List<Plan> Plans)
        {
            Phases = new List<Phase>();
            this.options = options;
            this.Plans = Plans;
        }

        public List<PCD.Phase> CreatePCDPhases()
        {
            foreach (Models.Approach approach in options.Signal.Approaches)
            {
                Phases.Add(new Phase(approach.ProtectedPhaseNumber));
            }
            return Phases;
            //if (detectors.Count > 0)
            //{
            //    foreach (Models.Graph_Detectors row in detectors)
            //    {
            //        var existingPhase = Phases.Where(p => p.PhaseNumbe)
            //        PCD.Phase Phase = new PCD.Phase();

            //        //try not to add the same direction twice
            //        var ExsitingPhases = from MOE.Common.Business.SignalPhase phase in this.SignalPhaseList
            //                             where phase.Direction == signalPhase.Direction
            //                             select phase;

            //        if (ExsitingPhases.Count() < 1)
            //        {
            //            this.SignalPhaseList.Add(signalPhase);
            //        }

            //    }
            //}
        }
    }
}
