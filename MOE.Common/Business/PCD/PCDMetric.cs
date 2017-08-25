using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.PCD
{
    public class PCDMetric
    {
        public List<PCD.Phase> Phases { get; set; }
        public List<PCD.Plan> Plans { get; set; }
        private MOE.Common.Business.WCFServiceLibrary.PCDOptions Options;

        public PCDMetric(MOE.Common.Business.WCFServiceLibrary.PCDOptions options)
        {
            this.Options = options;
            PlanFactory planFactory = new PlanFactory(options.Signal.PlanEvents, options.StartDate, options.EndDate,
                options.SignalID);
            Plans = planFactory.CreatePlanList();
            PhaseFactory phaseFactory = new PhaseFactory(options, Plans);
            SetPhases();
        }        

        private void SetPhases()
        {            
            
        }
    }
}
