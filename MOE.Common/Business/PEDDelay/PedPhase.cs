using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;

namespace MOE.Common.Business.PEDDelay
{
    public class PedPhase : ControllerEventLogs
    {
        public PedPhase(Approach approach, Signal signal, int timeBuffer, DateTime startDate, DateTime endDate,
            PlansBase plansData) : base(signal.SignalID, startDate, endDate, approach.GetPedDetectorsFromApproach(), approach.IsPedestrianPhaseOverlap ? new List<int> {67, 68, 45, 90} : new List<int> { 21, 22, 45, 90 })
        {
            SignalID = signal.SignalID;
            TimeBuffer = timeBuffer;
            StartDate = startDate;
            EndDate = endDate;
            Approach = approach;
            PhaseNumber = approach.ProtectedPhaseNumber;
            EndDate = endDate;
            Plans = new List<PedPlan>();
            Cycles = new List<PedCycle>();
            PedBeginWalkEvents = new List<Controller_Event_Log>();
            HourlyTotals = new List<PedHourlyTotal>();

            for (var i = 0; i < plansData.Events.Count; i++)
            {
                //if this is the last plan then we want the end of the plan
                //to coincide with the end of the graph
                var endTime = i == plansData.Events.Count - 1 ? endDate : plansData.Events[i + 1].Timestamp;

                var plan = new PedPlan(PhaseNumber, plansData.Events[i].Timestamp, endTime,
                    plansData.Events[i].EventParam);

                plan.Events = (from e in Events
                                where e.Timestamp > plan.StartDate && e.Timestamp < plan.EndDate
                                select e).ToList();

                plan.UniquePedDetections = CountUniquePedDetections(plan.Events);
                Plans.Add(plan);
            }

            if (Approach.IsPedestrianPhaseOverlap)
            {
                BeginWalkEvent = 67;
                BeginClearanceEvent = 68;
            }
            else
            {
                BeginWalkEvent = 21;
                BeginClearanceEvent = 22;
            }

            GetCycles();
            AddCyclesToPlans();
            SetHourlyTotals();
        }
        public Approach Approach { get; set; }
        public int PhaseNumber { get; }
        public string SignalID { get; }
        public List<PedCycle> Cycles { get; }
        public List<PedPlan> Plans { get; }
        public List<PedHourlyTotal> HourlyTotals { get; }
        public double MinDelay { get; private set; }
        public double AverageDelay { get; private set; }
        public double MaxDelay { get; private set; }
        public double TotalDelay { get; set; }
        public int TimeBuffer { get; set; }
        public int PedPresses { get; private set; }
        public int UniquePedDetections { get; set; }
        public int PedRequests { get; private set; }
        public int ImputedPedCallsRegistered { get; set; }
        public int PedBeginWalkCount { get; set; }
        public List<Controller_Event_Log> PedBeginWalkEvents { get; set; }
        public int PedCallsRegisteredCount { get; set; }
        private int BeginWalkEvent { get; set; }
        private int BeginClearanceEvent { get; set; }

        private void AddCyclesToPlans()
        {
            foreach (var p in Plans)
            {
                var cycles = (from c in Cycles
                              where c.CallRegistered >= p.StartDate &&
                                    c.CallRegistered < p.EndDate
                              select c).ToList();
                p.Cycles = cycles;
            }
        }

        private void GetCycles()
        {
            PedPresses = Events.Count(e => e.EventCode == 90);
            UniquePedDetections = CountUniquePedDetections(Events);

            CombineSequential90s();

            PedRequests = (Events.Count(e => e.EventCode == 90));
            PedCallsRegisteredCount = Events.Count(e => e.EventCode == 45);

            Remove45s();

            PedBeginWalkCount = Events.Count(e => e.EventCode == BeginWalkEvent);
            ImputedPedCallsRegistered = CountImputedPedCalls(Events);

            if (Events.Count > 1 && Events[0].EventCode == 90 && Events[1].EventCode == BeginWalkEvent)
            {
                Cycles.Add(new PedCycle(Events[1].Timestamp, Events[0].Timestamp));  // Middle of the event
            }

            for (var i = 0; i < Events.Count - 2; i++)
            {
                // there are four possibilities:
                // 1) 22, 90 , 21
                //   time between 90 and 21, count++
                // 2) 21, 90, 22
                //    time = 0 , count++
                // 3) 22, 90, 22 
                //    ignore this possibility
                // 4) 21, 90, 21
                //    time betweeen 90 and last 21, count++
                //
                if (Events[i].EventCode == BeginClearanceEvent &&
                    Events[i + 1].EventCode == 90 &&
                    Events[i + 2].EventCode == BeginWalkEvent)
                {
                    Cycles.Add(new PedCycle(Events[i + 2].Timestamp, Events[i + 1].Timestamp));  // this is case 1
                    i++;
                }
                else if (Events[i].EventCode == BeginWalkEvent &&
                         Events[i + 1].EventCode == 90 &&
                         Events[i + 2].EventCode == BeginClearanceEvent)
                {
                    Cycles.Add(new PedCycle(Events[i + 1].Timestamp, Events[i + 1].Timestamp));  // this is case 2
                    i++;
                }
                else if (Events[i].EventCode == BeginWalkEvent &&
                         Events[i + 1].EventCode == 90 &&
                         Events[i + 2].EventCode == BeginWalkEvent)
                {
                    Cycles.Add(new PedCycle(Events[i + 2].Timestamp, Events[i + 1].Timestamp));  // this is case 4
                    i++;
                }
                else if (Events[i].EventCode == BeginWalkEvent && (Cycles.Count == 0 || Events[i].Timestamp != Cycles.Last().BeginWalk))
                {
                    PedBeginWalkEvents.Add(Events[i]); // collected loose BeginWalkEvents for chart
                }
            }
            if (Events.Count >= 1)
            {
                if (Events[Events.Count - 1].EventCode == BeginWalkEvent)
                    PedBeginWalkEvents.Add(Events[Events.Count - 1]);
            }
            if (Events.Count >= 2)
            {
                if (Events[Events.Count - 2].EventCode == BeginWalkEvent)
                    PedBeginWalkEvents.Add(Events[Events.Count - 2]);
            }
        }

        private void CombineSequential90s()
        {
            var tempEvents = new List<Models.Controller_Event_Log>();
            for (int i = 0; i < Events.Count; i++)
            {
                if (Events[i].EventCode == 90)
                {
                    tempEvents.Add(Events[i]);

                    while (i + 1 < Events.Count && Events[i + 1].EventCode == 90)
                    {
                        i++;
                    }
                }
                else
                {
                    tempEvents.Add(Events[i]);
                }
            }
            Events = tempEvents.OrderBy(t => t.Timestamp).ToList();
        }

        private void Remove45s()
        {
            Events = Events.Where(e => e.EventCode != 45).OrderBy(t => t.Timestamp).ToList();
        }

        private int CountImputedPedCalls(List<Controller_Event_Log> events)
        {
            var tempEvents = events.Where(e => e.EventCode == 90 || e.EventCode == BeginWalkEvent).ToList();
            
            if (tempEvents.Count == 0) return 0;

            var previousEventCode = GetEventFromPreviousBin(SignalID, PhaseNumber, events.FirstOrDefault().Timestamp, new List<int> { BeginWalkEvent, 90 }, TimeSpan.FromMinutes(15));
            tempEvents.Insert(0, previousEventCode);

            int pedCalls = 0;

            for (var i = 1; i < tempEvents.Count; i++)
            {
                if (tempEvents[i].EventCode == 90 && tempEvents[i - 1]?.EventCode == BeginWalkEvent)
                {
                    pedCalls++;
                }
            }
            return pedCalls;
        }

        private int CountUniquePedDetections(List<Controller_Event_Log> events)
        {
            var tempEvents = events.Where(e => e.EventCode == 90).ToList();

            if (tempEvents.Count == 0) return 0;

            int pedDetections = 0;
            var previousEventCode = GetEventFromPreviousBin(SignalID, PhaseNumber, events.FirstOrDefault().Timestamp, new List<int> { 90 }, TimeSpan.FromSeconds(TimeBuffer));

            if (previousEventCode != null)
            {
                tempEvents.Insert(0, previousEventCode);
            }
            else
            {
                pedDetections++;
            }

            var previousSelectedTimestamp = 0;

            for (var i = 1; i < tempEvents.Count; i++)
            {
                if (tempEvents[i].Timestamp.Subtract(tempEvents[previousSelectedTimestamp].Timestamp).TotalSeconds >= TimeBuffer)
                {
                    pedDetections++;
                    previousSelectedTimestamp = i;
                }
            }

            return pedDetections;
        }

        private void SetHourlyTotals()
        {
            //Get Min Max and Average
            if (Cycles.Count > 0)
            {
                MinDelay = Cycles.Min(c => c.Delay);
                MaxDelay = Cycles.Max(c => c.Delay);
                AverageDelay = Cycles.Average(c => c.Delay);
                TotalDelay = Cycles.Sum(c => c.Delay);

                var dt = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartDate.Hour, 0, 0);
                var nextDt = dt.AddHours(1);
                while (dt < EndDate)
                {
                    var hourDelay = (from c in Cycles
                                     where c.CallRegistered >= dt &&
                                           c.CallRegistered < nextDt
                                     select c.Delay).Sum();
                    HourlyTotals.Add(new PedHourlyTotal(dt, hourDelay));
                    dt = dt.AddHours(1);
                    nextDt = nextDt.AddHours(1);
                }
            }
        }
    }
}
