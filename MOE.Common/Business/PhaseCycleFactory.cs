using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Business
{
    public static class PhaseCycleFactory
    {
        public static PhaseCycleBase GetSpeedCycles()
        {
            throw new System.NotImplementedException();
        }

        public static List<PhaseCycleBase> GetSplitMonitorCycles(int phasenumber, string signalId, List<Models.Controller_Event_Log> cycleeventsTable, List<Models.Controller_Event_Log> Pedevents)
        {
            PhaseCycleBase cycle = null;

            List < PhaseCycleBase > cycles = new List<PhaseCycleBase>();

            foreach (MOE.Common.Models.Controller_Event_Log row in cycleeventsTable)
            {
                if (row.EventCode == 1 && row.EventParam == phasenumber)
                {
                    //if (cycle == null)
                    {
                        cycle = new PhaseCycleBase
                        {
                            SignalId = signalId,
                            PhaseNumber = phasenumber,
                            CycleStart = row.Timestamp
                        };

                    }

                }

                if (cycle != null && row.EventParam == phasenumber && (row.EventCode == 4 || row.EventCode == 5 || row.EventCode == 6))
                {
                    cycle.SetTerminationEvent(row.EventCode);
                }

                if (cycle != null && row.EventParam == phasenumber && row.EventCode == 8)
                {
                    cycle.YellowStart = row.Timestamp;


                }

                if (cycle != null && row.EventParam == phasenumber && row.EventCode == 11)
                {
                    cycle.CycleEnd = row.Timestamp;
                    cycles.Add(cycle);

                }


            }

            foreach (PhaseCycleBase c in cycles)
            {
                List<Models.Controller_Event_Log> pedeventsForCycle = (from r in Pedevents
                    where r.Timestamp >=
                          c.CycleStart && r.Timestamp <= c.CycleEnd
                    select r).ToList();


                SetPedTimesForCycle(pedeventsForCycle, c);

            }

            return cycles;
        }

        public static List<PhaseCycleBase> GetCycles(int startofCycleEvent, List<Models.Controller_Event_Log> events, DateTime start, DateTime end)
        {

            List < PhaseCycleBase > cycles = new List<PhaseCycleBase>();


                if (events.Exists(s => s.EventCode == 1))
                {
                    


                    PhaseCycleBase.TerminationType termEvent = PhaseCycleBase.TerminationType.Unknown;

                    for (int i = 0; i < events.Count; i++)
                    {
                        DateTime CycleStart = new DateTime();
                        DateTime changeToGreen = new DateTime();
                        DateTime beginYellowClear = new DateTime();
                        DateTime endYellowClear = new DateTime();
                        DateTime changeToRed = new DateTime();
                        DateTime greenTerm = new DateTime();
                        DateTime cycleEnd = new DateTime();

                        if (events[i].EventCode == startofCycleEvent)
                        {
                            if (i + 1 >= events.Count)
                            {
                                break;
                            }
                            CycleStart = events[i].Timestamp;
                            switch (events[i].EventCode)
                            {
                                case 1:
                                    changeToGreen = events[i].Timestamp;
                                    break;
                                case 4:
                                    termEvent = PhaseCycleBase.TerminationType.GapOut;
                                    break;
                                case 5:
                                    termEvent = PhaseCycleBase.TerminationType.MaxOut;
                                    break;
                                case 6:
                                    termEvent = PhaseCycleBase.TerminationType.ForceOff;
                                    break;
                                case 7:
                                    greenTerm = events[i].Timestamp;
                                    break;
                                case 8:
                                    beginYellowClear = events[i].Timestamp;
                                    break;
                                case 9:
                                    endYellowClear = events[i].Timestamp;
                                    changeToRed = events[i].Timestamp;
                                    break;
                                    //case 10:
                                    //    changeToRed = events[i].Timestamp;
                                    //    break;
                            }

                            int s = i + 1;


                            while (events[s].EventCode != startofCycleEvent && s != events.Count)
                            {
                                switch (events[s].EventCode)
                                {
                                    case 1:
                                        changeToGreen = events[s].Timestamp;
                                        break;
                                    case 4:
                                        termEvent = PhaseCycleBase.TerminationType.GapOut;
                                        break;
                                    case 5:
                                        termEvent = PhaseCycleBase.TerminationType.MaxOut;
                                        break;
                                    case 6:
                                        termEvent = PhaseCycleBase.TerminationType.ForceOff;
                                        break;
                                    case 7:
                                        greenTerm = events[s].Timestamp;
                                        break;
                                    case 8:
                                        beginYellowClear = events[s].Timestamp;
                                        break;
                                    case 9:
                                        endYellowClear = events[s].Timestamp;
                                        changeToRed = events[s].Timestamp;
                                        break;
                                        //case 10:
                                        //    changeToRed = events[s].Timestamp;
                                        //    break;




                                }
                                s++;
                                if (s >= events.Count)
                                {
                                    i = s;

                                    //deal with the very last cycle
                                    if (
                                        CycleStart > DateTime.MinValue
                                       )
                                    {
                                        if (changeToGreen == DateTime.MinValue)
                                        { changeToGreen = CycleStart; }

                                        if (beginYellowClear == DateTime.MinValue)
                                        { beginYellowClear = CycleStart.AddSeconds(1); }

                                        if (endYellowClear == DateTime.MinValue)
                                        { endYellowClear = CycleStart.AddSeconds(4); }


                                        if (changeToRed == DateTime.MinValue)
                                        { changeToRed = CycleStart.AddSeconds(5); }


                                        if (events.Last().EventCode == 1)
                                        {
                                            cycleEnd = events.Last().Timestamp;
                                        }
                                        else
                                        {
                                            cycleEnd = end;
                                        }

                                        PhaseCycleBase cycle = new PhaseCycleBase
                                        {
                                            CycleStart = CycleStart,
                                            GreenStart = changeToGreen,
                                            RedStart = changeToRed,
                                            YellowStart = beginYellowClear,
                                            CycleEnd = cycleEnd

                                        };

                                        cycle.TerminationEvent = termEvent;

                                        cycles.Add(cycle);

                                    }
                                    break;
                                }
                            }

                            if (s >= events.Count)
                            {
                                break;
                            }

                            i = s - 1;
                            cycleEnd = events[s].Timestamp;


                            if (
                                CycleStart > DateTime.MinValue &&
                                changeToGreen > DateTime.MinValue &&
                                beginYellowClear > DateTime.MinValue &&
                                changeToRed > DateTime.MinValue
                                )
                            {
                                var cycle = new PhaseCycleBase
                                {
                                    CycleStart = CycleStart,
                                    GreenStart = changeToGreen,
                                    YellowStart = beginYellowClear,
                                    RedClearStart = changeToRed,
                                    CycleEnd = cycleEnd,
                                    TerminationEvent = termEvent
                                };

                                cycles.Add(cycle);
                            }

                        }



                    }
                }


            return cycles;

        }

        public static void SetPedTimesForCycle(List<Models.Controller_Event_Log> pedeventsForCycle, PhaseCycleBase cycle)
        {
            if (pedeventsForCycle.Count > 0)
            {
                var orderedevents = pedeventsForCycle.OrderBy(r => r.Timestamp);
                if (orderedevents.Count() > 1)
                {
                    for (int i = 0; i < orderedevents.Count() - 1; i++)
                    {


                        MOE.Common.Models.Controller_Event_Log current = orderedevents.ElementAt(i);

                        MOE.Common.Models.Controller_Event_Log next = orderedevents.ElementAt(i + 1);


                        if (current.Timestamp.Ticks == next.Timestamp.Ticks)
                        {
                            //i++;
                            continue;
                        }

                        //If the first event is 'Off', then set duration to 0
                        if (i == 0 && current.EventCode == 23)
                        {
                            cycle.PedStart = cycle.CycleStart;
                            //cycle.SetPedEnd(current.Timestamp);
                            cycle.PedEnd = cycle.CycleEnd;

                        }

                        //This is the prefered sequence; an 'On'  followed by an 'off'
                        if (current.EventCode == 21 && next.EventCode == 23)
                        {
                            if (cycle.PedStart == DateTime.MinValue)
                            {
                                cycle.PedStart = current.Timestamp;
                            }
                            else if ((cycle.PedStart > current.Timestamp))
                            {
                                cycle.PedStart = current.Timestamp;
                            }

                            if (cycle.PedEnd == DateTime.MinValue)
                            {
                                cycle.PedEnd = next.Timestamp;
                            }
                            else if ((cycle.PedEnd < next.Timestamp))
                            {
                                cycle.PedEnd = next.Timestamp;
                            }

                            continue;

                        }

                        //if we are at the penultimate event, and the last event is 'on' then set duration to 0.
                        if (i + 2 == orderedevents.Count() && next.EventCode == 21)
                        {
                            cycle.PedStart = cycle.CycleStart;
                            //cycle.SetPedEnd(cycle.YellowEvent);
                            cycle.PedEnd = cycle.CycleEnd;
                            ;
                            continue;


                        }




                    }
                }
                else
                {

                    MOE.Common.Models.Controller_Event_Log current = orderedevents.First();
                    switch (current.EventCode)
                    {


                        //if the only event is off
                        case 23:
                            cycle.PedStart = cycle.CycleStart;
                            cycle.PedEnd = cycle.CycleStart;
                            //cycle.SetPedEnd(current.Timestamp);

                            break;
                        //if the only event is on
                        case 21:

                            cycle.PedStart = current.Timestamp;
                            cycle.PedEnd = current.Timestamp;
                            //cycle.SetPedEnd(cycle.YellowEvent);

                            break;
                    }
                }
            }

        }


    }
}