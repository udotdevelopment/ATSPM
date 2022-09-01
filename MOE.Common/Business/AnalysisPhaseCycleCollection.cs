using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;

namespace MOE.Common.Business
{
    public class AnalysisPhaseCycleCollection
    {
        public List<AnalysisPhaseCycle> Items = new List<AnalysisPhaseCycle>();

        /// <summary>
        ///     Collection of phase events primarily used for the split monitor and Phase Termination Chart
        /// </summary>
        /// <param name="phasenumber"></param>
        /// <param name="signalid"></param>
        /// <param name="CycleEventsTable"></param>
        public AnalysisPhaseCycleCollection(int phasenumber, string signalId,
            List<Controller_Event_Log> CycleEventsTable, List<Controller_Event_Log> PedEvents)
        {
            AnalysisPhaseCycle Cycle = null;
            SignalID = signalId;
            PhaseNumber = phasenumber;


            foreach (var row in CycleEventsTable)
            {
                if (row.EventCode == 1 && row.EventParam == phasenumber)
                    Cycle = new AnalysisPhaseCycle(signalId, phasenumber, row.Timestamp);

                if (Cycle != null && row.EventParam == phasenumber &&
                    (row.EventCode == 4 || row.EventCode == 5 || row.EventCode == 6))
                    Cycle.SetTerminationEvent(row.EventCode);

                if (Cycle != null && row.EventParam == phasenumber && row.EventCode == 8)
                    Cycle.YellowEvent = row.Timestamp;

                if (Cycle != null && row.EventParam == phasenumber && row.EventCode == 11)
                {
                    Cycle.SetEndTime(row.Timestamp);
                    Items.Add(Cycle);
                }
            }

            foreach (var c in Items)
            {
                var PedEventsForCycle = (from r in PedEvents
                    where r.Timestamp >=
                          c.StartTime && r.Timestamp <= c.EndTime
                    select r).ToList();

                if ((c.EndTime - c.StartTime).Seconds > PedEventsForCycle.Count)
                {
                    SetPedTimesForCycle(PedEventsForCycle, c);
                }
            }
        }

        public string SignalID { get; set; }
        public int PhaseNumber { get; set; }


        public void SetPedTimesForCycle(List<Controller_Event_Log> PedEventsForCycle, AnalysisPhaseCycle Cycle)
        {
            if (PedEventsForCycle.Count > 0)
            {
                var eventsInOrder = PedEventsForCycle.OrderBy(r => r.Timestamp);
                if (eventsInOrder.Count() > 1)
                {
                    for (var i = 0; i < eventsInOrder.Count() - 1; i++)
                    {
                        var current = eventsInOrder.ElementAt(i);

                        var next = eventsInOrder.ElementAt(i + 1);


                        if (current.Timestamp.Ticks == next.Timestamp.Ticks)
                            continue;

                        //If the first event is 'Off', then set duration to 0
                        if (i == 0 && current.EventCode == 23)
                        {
                            Cycle.SetPedStart(Cycle.StartTime);
                            //Cycle.SetPedEnd(current.Timestamp);
                            Cycle.SetPedEnd(Cycle.StartTime);
                        }

                        //This is the prefered sequence; an 'On'  followed by an 'off'
                        if (current.EventCode == 21 && next.EventCode == 23)
                        {
                            if (Cycle.PedStartTime == DateTime.MinValue)
                                Cycle.SetPedStart(current.Timestamp);
                            else if (Cycle.PedStartTime > current.Timestamp)
                                Cycle.SetPedStart(current.Timestamp);

                            if (Cycle.PedEndTime == DateTime.MinValue)
                                Cycle.SetPedEnd(next.Timestamp);
                            else if (Cycle.PedEndTime < next.Timestamp)
                                Cycle.SetPedEnd(next.Timestamp);

                            continue;
                        }

                        //if we are at the penultimate event, and the last event is 'on' then set duration to 0.
                        if (i + 2 == eventsInOrder.Count() && next.EventCode == 21)
                        {
                            Cycle.SetPedStart(Cycle.StartTime);
                            //Cycle.SetPedEnd(Cycle.YellowEvent);
                            Cycle.SetPedEnd(Cycle.StartTime);
                        }
                    }
                }
                else
                {
                    var current = eventsInOrder.First();
                    switch (current.EventCode)
                    {
                        //if the only event is off
                        case 23:
                            Cycle.SetPedStart(Cycle.StartTime);
                            Cycle.SetPedEnd(Cycle.StartTime);
                            //Cycle.SetPedEnd(current.Timestamp);

                            break;
                        //if the only event is on
                        case 21:

                            Cycle.SetPedStart(current.Timestamp);
                            Cycle.SetPedEnd(current.Timestamp);
                            //Cycle.SetPedEnd(Cycle.YellowEvent);

                            break;
                    }
                }
            }
        }
    }
}