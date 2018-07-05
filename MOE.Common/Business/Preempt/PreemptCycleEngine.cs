using System;
using System.Collections.Generic;
using MOE.Common.Models;

namespace MOE.Common.Business.Preempt
{
    public class PreemptCycleEngine
    {
        public List<PreemptCycle> CreatePreemptCycle(ControllerEventLogs DTTB)
        {
            var CycleCollection = new List<PreemptCycle>();
            PreemptCycle cycle = null;


            //foreach (MOE.Common.Models.Controller_Event_Log row in DTTB.Events)
            for (var x = 0; x < DTTB.Events.Count; x++)
            {
                //It can happen that there is no defined terminaiton event.
                if (x + 1 < DTTB.Events.Count)
                {
                    var t = DTTB.Events[x + 1].Timestamp - DTTB.Events[x].Timestamp;
                    if (cycle != null && t.TotalMinutes > 20 && DTTB.Events[x].EventCode != 111 &&
                        DTTB.Events[x].EventCode != 105)
                    {
                        EndCycle(cycle, DTTB.Events[x], CycleCollection);
                        cycle = null;
                        continue;
                    }
                }

                switch (DTTB.Events[x].EventCode)
                {
                    case 102:

                        if (cycle != null)
                            cycle.InputOn.Add(DTTB.Events[x].Timestamp);

                        if (cycle == null && DTTB.Events[x].Timestamp != DTTB.Events[x + 1].Timestamp &&
                            DTTB.Events[x + 1].EventCode == 105)
                            cycle = StartCycle(DTTB.Events[x]);

                        break;

                    case 103:

                        if (cycle != null && cycle.GateDown == DateTime.MinValue)
                            cycle.GateDown = DTTB.Events[x].Timestamp;


                        break;

                    case 104:

                        if (cycle != null)
                            cycle.InputOff.Add(DTTB.Events[x].Timestamp);

                        break;

                    case 105:


                        ////If we run into an entry start after cycle start (event 102)
                        if (cycle != null && cycle.HasDelay)
                        {
                            cycle.EntryStarted = DTTB.Events[x].Timestamp;
                            break;
                        }

                        if (cycle != null)
                        {
                            EndCycle(cycle, DTTB.Events[x], CycleCollection);
                            cycle = null;
                            cycle = StartCycle(DTTB.Events[x]);
                            break;
                        }

                        if (cycle == null)
                            cycle = StartCycle(DTTB.Events[x]);
                        break;

                    case 106:
                        if (cycle != null)
                        {
                            cycle.BeginTrackClearance = DTTB.Events[x].Timestamp;

                            if (x + 1 < DTTB.Events.Count)
                                if (!DoesTrackClearEndNormal(DTTB, x))
                                    cycle.BeginDwellService = FindNext111Event(DTTB, x);
                        }
                        break;

                    case 107:

                        if (cycle != null)
                        {
                            cycle.BeginDwellService = DTTB.Events[x].Timestamp;

                            if (x + 1 < DTTB.Events.Count)
                                if (!DoesTheCycleEndNormal(DTTB, x))
                                {
                                    cycle.BeginExitInterval = DTTB.Events[x + 1].Timestamp;

                                    EndCycle(cycle, DTTB.Events[x + 1], CycleCollection);

                                    cycle = null;
                                }
                        }


                        break;

                    case 108:
                        if (cycle != null)
                            cycle.LinkActive = DTTB.Events[x].Timestamp;
                        break;

                    case 109:
                        if (cycle != null)
                            cycle.LinkInactive = DTTB.Events[x].Timestamp;

                        break;

                    case 110:
                        if (cycle != null)
                            cycle.MaxPresenceExceeded = DTTB.Events[x].Timestamp;
                        break;

                    case 111:
                        // 111 can usually be considered "cycle complete"
                        if (cycle != null)
                        {
                            cycle.BeginExitInterval = DTTB.Events[x].Timestamp;

                            EndCycle(cycle, DTTB.Events[x], CycleCollection);


                            cycle = null;
                        }
                        break;
                }


                if (x + 1 >= DTTB.Events.Count && cycle != null)
                {
                    cycle.BeginExitInterval = DTTB.Events[x].Timestamp;
                    EndCycle(cycle, DTTB.Events[x], CycleCollection);
                    break;
                }
            }

            return CycleCollection;
        }

        private DateTime FindNext111Event(ControllerEventLogs DTTB, int counter)
        {
            var Next111Event = new DateTime();
            for (var x = counter; x < DTTB.Events.Count; x++)
                if (DTTB.Events[x].EventCode == 111)
                {
                    Next111Event = DTTB.Events[x].Timestamp;
                    x = DTTB.Events.Count;
                }
            return Next111Event;
        }

        private bool DoesTheCycleEndNormal(ControllerEventLogs DTTB, int counter)
        {
            var foundEvent111 = false;

            for (var x = counter; x < DTTB.Events.Count; x++)
                switch (DTTB.Events[x].EventCode)
                {
                    case 102:
                        foundEvent111 = false;
                        x = DTTB.Events.Count;
                        break;
                    case 105:
                        foundEvent111 = false;
                        x = DTTB.Events.Count;
                        break;

                    case 111:
                        foundEvent111 = true;
                        x = DTTB.Events.Count;
                        break;
                }

            return foundEvent111;
        }

        private bool DoesTrackClearEndNormal(ControllerEventLogs DTTB, int counter)
        {
            var foundEvent107 = false;

            for (var x = counter; x < DTTB.Events.Count; x++)
                switch (DTTB.Events[x].EventCode)
                {
                    case 107:
                        foundEvent107 = true;
                        x = DTTB.Events.Count;
                        break;

                    case 111:
                        foundEvent107 = false;
                        x = DTTB.Events.Count;
                        break;
                }

            return foundEvent107;
        }

        private void EndCycle(PreemptCycle cycle, Controller_Event_Log controller_Event_Log,
            List<PreemptCycle> CycleCollection)
        {
            cycle.CycleEnd = controller_Event_Log.Timestamp;
            CycleCollection.Add(cycle);
        }


        private PreemptCycle StartCycle(Controller_Event_Log controller_Event_Log)
        {
            var cycle = new PreemptCycle();


            cycle.CycleStart = controller_Event_Log.Timestamp;

            if (controller_Event_Log.EventCode == 105)
            {
                cycle.EntryStarted = controller_Event_Log.Timestamp;
                cycle.HasDelay = false;
            }

            if (controller_Event_Log.EventCode == 102)
            {
                cycle.StartInputOn = controller_Event_Log.Timestamp;
                cycle.HasDelay = true;
            }

            return cycle;
        }
    }
}