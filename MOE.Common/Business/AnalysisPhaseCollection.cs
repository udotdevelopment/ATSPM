using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
    public class AnalysisPhaseCollection
    {

        public List<Business.AnalysisPhase> Items = new List<Business.AnalysisPhase>();
        public Business.PlanCollection Plans;

        private int maxPhaseInUse;
        public int MaxPhaseInUse
        {
            get
            {
                return maxPhaseInUse;
            }
        }

        
        /// <summary>
        /// constructor Used for Termination chart.
        /// </summary>
        /// <param name="signalId"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="consecutivecount"></param>
        public AnalysisPhaseCollection(string signalId, DateTime starttime, DateTime endtime, int consecutivecount)
        {
            MOE.Common.Models.Repositories.IControllerEventLogRepository cel = MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();



            List<Models.Controller_Event_Log> PTEDT = cel.GetSignalEventsByEventCodes(signalId, starttime, endtime, new List<int>() { 1, 11, 4, 5, 6, 7, 21, 23 });
            List<Models.Controller_Event_Log> DAPTA = cel.GetSignalEventsByEventCodes(signalId, starttime, endtime, new List<int>() { 1 });

            PTEDT.OrderByDescending(i => i.Timestamp);

            var PhasesInUse = (from r in DAPTA
                            where r.EventCode == 1
                            select r.EventParam).Distinct();

            
        


            Plans = new PlanCollection(starttime, endtime, signalId);
            

            foreach (int row in PhasesInUse)
            {
                //Business.AnalysisPhase aPhase = new AnalysisPhase(row.EventParam, starttime, endtime, PTEDT, consecutivecount);
                Business.AnalysisPhase aPhase = new AnalysisPhase(row, PTEDT, consecutivecount);

                Items.Add(aPhase);
            }
            OrderPhases();
            maxPhaseInUse = FindMaxPhase(Items);


        }

        private void OrderPhases()
        {
            Items = Items.OrderBy(i => i.PhaseNumber).ToList();
        }

        
        /// <summary>
        /// Constructor used for split monitor
        /// </summary>
        /// <param name="signalId"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        public AnalysisPhaseCollection(string signalId, DateTime starttime, DateTime endtime)
        {


            MOE.Common.Models.Repositories.IControllerEventLogRepository cel = MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();



            List<Models.Controller_Event_Log> PTEDT = cel.GetSignalEventsByEventCodes(signalId, starttime, endtime, new List<int>() { 1, 11, 4, 5, 6, 7, 21, 23 });
            List<Models.Controller_Event_Log> DAPTA = cel.GetSignalEventsByEventCodes(signalId, starttime, endtime, new List<int>() { 1 });


            var PhasesInUse = (from r in DAPTA
                               where r.EventCode == 1
                               select r.EventParam).Distinct();


            Plans = new PlanCollection(starttime, endtime, signalId);


            foreach (int row in PhasesInUse)
            {
                Business.AnalysisPhase aPhase = new AnalysisPhase( row ,signalId, PTEDT);

                Items.Add(aPhase);
            }
            OrderPhases();
        }

        protected int FindMaxPhase(List<Business.AnalysisPhase> Items)
        {
            int maxPhaseNumber = 0;

            foreach (Business.AnalysisPhase phase in Items)
            {
                if (phase.PhaseNumber > maxPhaseNumber)
                {
                    maxPhaseNumber = phase.PhaseNumber;
                }
            }

            return maxPhaseNumber;
        }

    }
}
