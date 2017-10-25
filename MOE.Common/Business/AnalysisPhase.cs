using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
    public class AnalysisPhase
    {

        private int phaseNumber;
        public int PhaseNumber
        {
            get
            {
                return phaseNumber;
            }
        }



        private string signalId;
        public string SignalID
        {
            get
            {
                return signalId;
            }
        }

        private double percentMaxOuts;
        public double PercentMaxOuts
        {
            get
            {
                return percentMaxOuts;
            }
        }

        private double percentForceOffs;
        public double PercentForceOffs
        {
            get
            {
                return percentForceOffs;
            }
        }

        private int totalPhaseTerminations;
        public int TotalPhaseTerminations
        {
            get
            {
                return totalPhaseTerminations;
            }
        }

        public string Direction { get; set; }
        public bool IsOverlap { get; set; }

        public List<Models.Controller_Event_Log> PedestrianEvents = new List<Models.Controller_Event_Log>();
        public List<Models.Controller_Event_Log> TerminationEvents = new List<Models.Controller_Event_Log>();
        public List<Models.Controller_Event_Log> ConsecutiveGapOuts = new List<Models.Controller_Event_Log>();
        public List<Models.Controller_Event_Log> ConsecutiveMaxOut = new List<Models.Controller_Event_Log>();
        public List<Models.Controller_Event_Log> ConsecutiveForceOff = new List<Models.Controller_Event_Log>();
        public List<Models.Controller_Event_Log> UnknownTermination = new List<Models.Controller_Event_Log>();
        public Business.AnalysisPhaseCycleCollection Cycles;

        public List<Models.Controller_Event_Log> FindTerminationEvents(List<Models.Controller_Event_Log> terminationeventstable, int phasenumber)
        {

            List<Models.Controller_Event_Log> events = (from row in terminationeventstable
                          where row.EventParam == phasenumber && (row.EventCode == 4 || 
                          row.EventCode == 5 || row.EventCode == 6
                          || row.EventCode == 7
                          )
                          
                          select row).ToList();

            List<Models.Controller_Event_Log> sortedEvents = events.OrderBy(x => x.Timestamp).ThenByDescending(y => y.EventCode).ToList();
            return sortedEvents;
        }

        public List<Models.Controller_Event_Log> FindPedEvents(List<Models.Controller_Event_Log> terminationeventstable, int phasenumber)
        {
            List<Models.Controller_Event_Log> events = (from row in terminationeventstable
                         where row.EventParam == phasenumber && (row.EventCode == 21 || row.EventCode == 23)
                          orderby row.Timestamp
                          select row).ToList();

            return events;
        }

        public List<Models.Controller_Event_Log> FindPhaseEvents(List<Models.Controller_Event_Log> PhaseEventsTable, int PhaseNumber)
        {
            List<Models.Controller_Event_Log> events = (from row in PhaseEventsTable
                                                        where row.EventParam == PhaseNumber
                                                        orderby row.Timestamp
                                                        select row).ToList();

            return events;
        }
        
        /// <summary>
        /// Constructor used for Phase Termination Chart
        /// </summary>
        /// <param name="phasenumber"></param>
        /// <param name="terminationeventstable"></param>
        /// <param name="consecutiveCount"></param>
        public AnalysisPhase(int phasenumber, List<Models.Controller_Event_Log> terminationeventstable, int consecutiveCount)
        {

            this.phaseNumber = phasenumber;
            TerminationEvents = FindTerminationEvents(terminationeventstable, phaseNumber);

            

            PedestrianEvents = FindPedEvents(terminationeventstable, phaseNumber);


            ConsecutiveGapOuts = FindConsecutiveEvents(TerminationEvents, 4, consecutiveCount);
            ConsecutiveMaxOut = FindConsecutiveEvents(TerminationEvents, 5, consecutiveCount);
            ConsecutiveForceOff = FindConsecutiveEvents(TerminationEvents, 6, consecutiveCount);
            UnknownTermination = FindUnknownTerminationEvents(TerminationEvents);
            percentMaxOuts = FindPercentageConsecutiveEvents(TerminationEvents, 5, consecutiveCount);
            percentForceOffs = FindPercentageConsecutiveEvents(TerminationEvents, 6, consecutiveCount);
            totalPhaseTerminations = TerminationEvents.Count;
    
        }

        
        /// <summary>
        /// Constructor Used for Split monitor
        /// </summary>
        /// <param name="phasenumber"></param>
        /// <param name="signalID"></param>
        /// <param name="CycleEventsTable"></param>
        public AnalysisPhase(int phasenumber, string signalID, List<Models.Controller_Event_Log> CycleEventsTable)
        {
            MOE.Common.Models.Repositories.ISignalsRepository repository =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            var signal = repository.GetSignalBySignalID(signalID);     
            this.phaseNumber = phasenumber;
            this.signalId = signalID;
            this.IsOverlap = false;
            List<Models.Controller_Event_Log> PedEvents = FindPedEvents(CycleEventsTable, phasenumber);
            List<Models.Controller_Event_Log> PhaseEvents = FindPhaseEvents(CycleEventsTable, phasenumber);
            Cycles = new AnalysisPhaseCycleCollection(phasenumber, signalId, PhaseEvents, PedEvents);
            Models.Approach approach = signal.Approaches.Where(a => a.ProtectedPhaseNumber == phasenumber).FirstOrDefault();
            if (approach != null)
            {
                this.Direction = approach.DirectionType.Description;
            }
            else
            {
                this.Direction = "Unknown";
            }            
        }

        private List<Models.Controller_Event_Log> FindConsecutiveEvents(List<Models.Controller_Event_Log> terminationEvents, 
            int eventtype, int consecutiveCount)
        {
            List<Models.Controller_Event_Log> ConsecutiveEvents = new List<Models.Controller_Event_Log>();
            int runningConsecCount = 0;
            // Order the events by datestamp
            var eventsInOrder = terminationEvents.OrderBy(TerminationEvent => TerminationEvent.Timestamp);
            foreach (Models.Controller_Event_Log termEvent in eventsInOrder)
            {
                if (termEvent.EventCode != 7)
                {
                    if (termEvent.EventCode == eventtype)
                    {
                        runningConsecCount++;
                    }
                    else
                    {
                        runningConsecCount = 0;
                    }

                    if (runningConsecCount >= consecutiveCount)
                    {
                        ConsecutiveEvents.Add(termEvent);
                    }
                }
            }
            return ConsecutiveEvents;
        }

        private List<Models.Controller_Event_Log> FindUnknownTerminationEvents(List<Models.Controller_Event_Log> terminationEvents)
        {
            List<Models.Controller_Event_Log> unknownTermEvents = new List<Models.Controller_Event_Log>();
            for(int x=0; x + 1 < terminationEvents.Count;x++)
            {
                Models.Controller_Event_Log currentEvent = terminationEvents[x];
                Models.Controller_Event_Log nextEvent = terminationEvents[x + 1];

                if(currentEvent.EventCode == 7 && nextEvent.EventCode == 7)
                {
                    //if (x + 2 <= terminationEvents.Count)
                    //{
                    //    TimeSpan t = terminationEvents[x + 2].Timestamp - terminationEvents[x + 1].Timestamp;

                        unknownTermEvents.Add(currentEvent);
                    //}
                }
            }
            return unknownTermEvents;
        }


        private double FindPercentageConsecutiveEvents(List<Models.Controller_Event_Log> terminationEvents, int eventtype, 
            int consecutiveCount)
        {
            double percentile = 0;
            double total = terminationEvents.Where(t => t.EventCode != 7).Count();
            //Get all termination events of the event type
            int terminationEventsOfType = terminationEvents.Where(
                TerminationEvent => TerminationEvent.EventCode == eventtype).Count();

            if (terminationEvents.Count() > 0)
            {
                percentile= terminationEventsOfType / total;
            }
            return percentile;
        }
    }
}