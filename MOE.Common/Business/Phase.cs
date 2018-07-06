using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Business
{
    public class Phase 
    {
        public enum Direction
        {
            Northbound,
            Southbound,
            Eastbound,
            Westbound
        }

        public String ApproachDirection
        {
            get;
            set;
        }


        public bool IsOverlap
        {
            get;
            set;
        }


        public List<Models.Controller_Event_Log> Events;

       

        public List<PhaseCycleBase> Cycles { get; set; }

        public Models.Approach Approach { get; set; }

        public string SignalId { get; set; }


        public DateTime StartDate { get; set; }


 
        public DateTime EndDate { get; set; }
  
        

        public int PhaseNumber { get; set; }

        

        

        public Phase(Models.Approach approach,
            DateTime startDate, DateTime endDate, List<int> eventCodes, int startofCycleEvent, bool usePermissivePhase)
           
                
        {
            startDate = startDate.AddMinutes(-1);
            endDate = endDate.AddMinutes(+1);

            Approach = approach;
            
            StartDate = startDate;
            EndDate = endDate;
            if (!usePermissivePhase)
            {
                PhaseNumber = Approach.ProtectedPhaseNumber;
            }
            else
            {
                PhaseNumber = Approach.PermissivePhaseNumber??0;
            }
            IsOverlap = false;
            SignalId = Approach.Signal.SignalID;


            MOE.Common.Models.Repositories.IControllerEventLogRepository cer = MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();

            Events = cer.GetEventsByEventCodesParam(SignalId, startDate, endDate,
                eventCodes, PhaseNumber);

            Cycles = PhaseCycleFactory.GetCycles(startofCycleEvent, Events, StartDate, EndDate);
        }

       
        

        
    }
}
    


       
    

