using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
    public class AnalysisPhaseCycleCollection
    {
        public List<Business.PhaseCycleBase> Cycles = new List<PhaseCycleBase>();

        public string SignalId { get; set; }
        public int PhaseNumber { get; set; }

        /// <summary>
        /// Collection of phase events primarily used for the split monitor and Phase Termination Chart
        /// </summary>
        /// <param name="phasenumber"></param>
        /// <param name="signalId"></param>
        /// <param name="cycleEventsTable"></param>
        public AnalysisPhaseCycleCollection(int phasenumber, string signalId, List<Models.Controller_Event_Log> cycleEventsTable, List<Models.Controller_Event_Log> PedEvents)
        {
           
        }



        
        }

    }


