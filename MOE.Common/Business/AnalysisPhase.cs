using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business
{
    public class AnalysisPhase
    {
        public List<Controller_Event_Log> ConsecutiveForceOff = new List<Controller_Event_Log>();
        public List<Controller_Event_Log> ConsecutiveGapOuts = new List<Controller_Event_Log>();
        public List<Controller_Event_Log> ConsecutiveMaxOut = new List<Controller_Event_Log>();
        public AnalysisPhaseCycleCollection Cycles;

        public List<Controller_Event_Log> PedestrianEvents = new List<Controller_Event_Log>();


        public List<Controller_Event_Log> TerminationEvents = new List<Controller_Event_Log>();

        public List<Controller_Event_Log> UnknownTermination = new List<Controller_Event_Log>();

        /// <summary>
        ///     Constructor used for Phase Termination Chart
        /// </summary>
        /// <param name="phaseNumber"></param>
        /// <param name="terminationeventstable"></param>
        /// <param name="consecutiveCount"></param>
        public AnalysisPhase(int phasenumber, List<Controller_Event_Log> terminationeventstable, int consecutiveCount)
        {
            PhaseNumber = phasenumber;
            TerminationEvents = FindTerminationEvents(terminationeventstable, PhaseNumber);
            PedestrianEvents = FindPedEvents(terminationeventstable, PhaseNumber);
            ConsecutiveGapOuts = FindConsecutiveEvents(TerminationEvents, 4, consecutiveCount);
            ConsecutiveMaxOut = FindConsecutiveEvents(TerminationEvents, 5, consecutiveCount);
            ConsecutiveForceOff = FindConsecutiveEvents(TerminationEvents, 6, consecutiveCount);
            UnknownTermination = FindUnknownTerminationEvents(TerminationEvents);
            PercentMaxOuts = FindPercentageConsecutiveEvents(TerminationEvents, 5, consecutiveCount);
            PercentForceOffs = FindPercentageConsecutiveEvents(TerminationEvents, 6, consecutiveCount);
            TotalPhaseTerminations = TerminationEvents.Count;
        }


        /// <summary>
        ///     Constructor Used for Split monitor
        /// </summary>
        /// <param name="phasenumber"></param>
        /// <param name="signalID"></param>
        /// <param name="CycleEventsTable"></param>
        public AnalysisPhase(int phasenumber, string signalID, List<Controller_Event_Log> CycleEventsTable)
        {
            var repository =
                SignalsRepositoryFactory.Create();
            var signal = repository.GetLatestVersionOfSignalBySignalID(signalID);
            PhaseNumber = phasenumber;
            SignalID = signalID;
            IsOverlap = false;
            var pedEvents = FindPedEvents(CycleEventsTable, phasenumber);
            var phaseEvents = FindPhaseEvents(CycleEventsTable, phasenumber);
            Cycles = new AnalysisPhaseCycleCollection(phasenumber, SignalID, phaseEvents, pedEvents);
            var approach = signal.Approaches.FirstOrDefault(a => a.ProtectedPhaseNumber == phasenumber);
            Direction = approach != null ? approach.DirectionType.Description : "Unknown";
        }

        public int PhaseNumber { get; }

        public string SignalID { get; }

        public double PercentMaxOuts { get; }

        public double PercentForceOffs { get; }

        public int TotalPhaseTerminations { get; }

        public string Direction { get; set; }
        public bool IsOverlap { get; set; }

        public List<Controller_Event_Log> FindTerminationEvents(List<Controller_Event_Log> terminationeventstable,
            int phasenumber)
        {
            var events = (from row in terminationeventstable
                where row.EventParam == phasenumber && (row.EventCode == 4 ||
                                                        row.EventCode == 5 || row.EventCode == 6
                                                        || row.EventCode == 7
                      )
                select row).ToList();

            var sortedEvents = events.OrderBy(x => x.Timestamp).ThenBy(y => y.EventCode).ToList();
            var duplicateList = new List<Controller_Event_Log>();
            for (int i = 0; i < sortedEvents.Count - 1; i++)
            {
                var event1 = sortedEvents[i];
                var event2 = sortedEvents[i + 1];
                if (event1.Timestamp == event2.Timestamp)
                {
                    if(event1.EventCode == 7)
                        duplicateList.Add(event1);
                    if(event2.EventCode == 7)
                        duplicateList.Add(event2);
                }
            }

            foreach (var e in duplicateList)
            {
                sortedEvents.Remove(e);
            }
            return sortedEvents;
        }

        public List<Controller_Event_Log> FindPedEvents(List<Controller_Event_Log> terminationeventstable,
            int phasenumber)
        {
            var events = (from row in terminationeventstable
                where row.EventParam == phasenumber && (row.EventCode == 21 || row.EventCode == 23)
                orderby row.Timestamp
                select row).ToList();

            return events;
        }

        public List<Controller_Event_Log> FindPhaseEvents(List<Controller_Event_Log> PhaseEventsTable, int PhaseNumber)
        {
            var events = (from row in PhaseEventsTable
                where row.EventParam == PhaseNumber
                orderby row.Timestamp
                select row).ToList();

            return events;
        }

        private List<Controller_Event_Log> FindConsecutiveEvents(List<Controller_Event_Log> terminationEvents,
            int eventtype, int consecutiveCount)
        {
            var ConsecutiveEvents = new List<Controller_Event_Log>();
            var runningConsecCount = 0;
            // Order the events by datestamp
            var eventsInOrder = terminationEvents.OrderBy(TerminationEvent => TerminationEvent.Timestamp);
            foreach (var termEvent in eventsInOrder)
                if (termEvent.EventCode != 7)
                {
                    if (termEvent.EventCode == eventtype)
                        runningConsecCount++;
                    else
                        runningConsecCount = 0;

                    if (runningConsecCount >= consecutiveCount)
                        ConsecutiveEvents.Add(termEvent);
                }
            return ConsecutiveEvents;
        }

        private List<Controller_Event_Log> FindUnknownTerminationEvents(List<Controller_Event_Log> terminationEvents)
        {
            return terminationEvents.Where(t => t.EventCode == 7).ToList();
        }


        private double FindPercentageConsecutiveEvents(List<Controller_Event_Log> terminationEvents, int eventtype,
            int consecutiveCount)
        {
            double percentile = 0;
            double total = terminationEvents.Count(t => t.EventCode != 7);
            //Get all termination events of the event type
            var terminationEventsOfType = terminationEvents.Count(terminationEvent => terminationEvent.EventCode == eventtype);

            if (terminationEvents.Any())
                percentile = terminationEventsOfType / total;
            return percentile;
        }
    }
}