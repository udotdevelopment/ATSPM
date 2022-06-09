﻿using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;

namespace MOE.Common.Business.PEDDelay
{
    public class PedPhase : ControllerEventLogs
    {
        public PedPhase(int phaseNumber, Models.Signal signal, int timeBuffer, DateTime startDate, DateTime endDate,
            PlansBase plansData) : base(signal.SignalID, startDate, endDate, phaseNumber, new List<int> { 21, 22, 45, 90 })
        {
            SignalID = signal.SignalID;
            TimeBuffer = timeBuffer;
            StartDate = startDate;
            EndDate = endDate;
            PhaseNumber = phaseNumber;
            EndDate = endDate;
            Plans = new List<PedPlan>();
            Cycles = new List<PedCycle>();
            PedBeginWalkEvents = new List<Controller_Event_Log>();
            HourlyTotals = new List<PedHourlyTotal>();

            for (var i = 0; i < plansData.Events.Count; i++)
                //if this is the last plan then we want the end of the plan
                //to coincide with the end of the graph
                if (plansData.Events.Count - 1 == i)
                {
                    var plan = new PedPlan(SignalID, phaseNumber, plansData.Events[i].Timestamp, endDate,
                        plansData.Events[i].EventParam);
                    var events = (from e in Events
                                  where e.Timestamp >= plan.StartDate && e.Timestamp < plan.EndDate
                                  select e).ToList();
                    plan.PedBeginWalkCount = events.Where(e => e.EventCode == 21).Count();
                    plan.PedCallsRegisteredCount = events.Where(e => e.EventCode == 45).Count();
                    plan.ImputedPedCallsRegistered = CountUniquePedDetections(events);
                    Plans.Add(plan);
                }
                //else we add the plan with the next plan's timestamp as the end of the plan
                else
                {
                    var plan = new PedPlan(SignalID, phaseNumber, plansData.Events[i].Timestamp,
                        plansData.Events[i + 1].Timestamp, plansData.Events[i].EventParam);
                    var events = (from e in Events
                                  where e.Timestamp >= plan.StartDate && e.Timestamp < plan.EndDate
                                  select e).ToList();
                    plan.PedBeginWalkCount = events.Where(e => e.EventCode == 21).Count();
                    plan.PedCallsRegisteredCount = events.Where(e => e.EventCode == 45).Count();
                    plan.ImputedPedCallsRegistered = CountUniquePedDetections(events);
                    Plans.Add(plan);
                }

            GetCycles();
            AddCyclesToPlans();
            SetHourlyTotals();
        }

        public int PhaseNumber { get; }
        public string SignalID { get; }
        public double PedActuations { get; private set; }
        public List<PedCycle> Cycles { get; }
        public List<PedPlan> Plans { get; }
        public List<PedHourlyTotal> HourlyTotals { get; }
        public double MinDelay { get; private set; }
        public double AverageDelay { get; private set; }
        public double MaxDelay { get; private set; }
        public double TotalDelay { get; set; }
        public int TimeBuffer { get; set; }
        public int ImputedPedCallsRegistered { get; set; }
        public int UniquePedDetections { get; set; }
        public int PedBeginWalkCount { get; set; }
        public List<Controller_Event_Log> PedBeginWalkEvents { get; set; }
        public int PedCallsRegisteredCount { get; set; }

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
            //count before combining 90s
            UniquePedDetections = CountUniquePedDetections(Events);

            CombineSequential90s();

            PedActuations = Convert.ToDouble(Events.Count(e => e.EventCode == 90));
            PedCallsRegisteredCount = Events.Count(e => e.EventCode == 45);

            Remove45s();

            PedBeginWalkCount = Events.Count(e => e.EventCode == 21);
            ImputedPedCallsRegistered = CountImputedPedCalls(Events);

            if (Events[0].EventCode == 90 && Events[1].EventCode == 21)
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
                if (Events[i].EventCode == 22 &&
                    Events[i + 1].EventCode == 90 &&
                    Events[i + 2].EventCode == 21)
                {
                    Cycles.Add(new PedCycle(Events[i + 2].Timestamp, Events[i + 1].Timestamp));  // this is case 1
                    i++;
                }
                else if (Events[i].EventCode == 21 &&
                         Events[i + 1].EventCode == 90 &&
                         Events[i + 2].EventCode == 22)
                {
                    Cycles.Add(new PedCycle(Events[i + 1].Timestamp, Events[i + 1].Timestamp));  // this is case 2
                    i++;
                }
                else if (Events[i].EventCode == 21 &&
                         Events[i + 1].EventCode == 90 && 
                         Events[i + 2].EventCode == 21)
                {
                    Cycles.Add(new PedCycle(Events[i + 2].Timestamp, Events[i + 1].Timestamp));  // this is case 4
                    i++;
                } 
                else if (Cycles.Count == 0 || Events[i].EventCode == 21 && Events[i].Timestamp != Cycles.Last().BeginWalk)
                {
                    PedBeginWalkEvents.Add(Events[i]); // collected loose 21s for chart
                }
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
            var tempEvents = new List<Models.Controller_Event_Log>();
            for (int i = 0; i < Events.Count; i++)
            {
                if (Events[i].EventCode == 45)
                {
                    continue;
                }

                tempEvents.Add(Events[i]);
            }
            Events = tempEvents.OrderBy(t => t.Timestamp).ToList();
        }

        private int CountImputedPedCalls(List<Controller_Event_Log> events)
        {
            if (events.Count == 0) return 0;

            var tempEvents = new List<Models.Controller_Event_Log>();

            for (var i = 0; i < events.Count; i++)
            {
                if (events[i].EventCode == 21 || events[i].EventCode == 90)
                {
                    tempEvents.Add(events[i]);
                }
            }

            var previousEventCode = GetEventFromPreviousBin(SignalID, PhaseNumber, events.FirstOrDefault().Timestamp, new List<int> { 21, 90 }, TimeSpan.FromMinutes(15));
            tempEvents.Insert(0, previousEventCode);

            int pedCalls = 0;

            for (var i = 1; i < tempEvents.Count; i++)
            {
                if (tempEvents[i].EventCode == 90 && tempEvents[i - 1]?.EventCode == 21)
                {
                    pedCalls++;
                }
            }
            return pedCalls;
        }

        private int CountUniquePedDetections(List<Controller_Event_Log> events)
        {
            List<Controller_Event_Log> list = new List<Controller_Event_Log>();

            for (var i = 0; i < events.Count; i++)
            {
                if (events[i].EventCode == 90)
                {
                    list.Add(events[i]);
                }
            }

            if (list.Count == 0) return 0;

            int pedDetections = 0;
            var previousEventCode = GetEventFromPreviousBin(SignalID, PhaseNumber, events.FirstOrDefault().Timestamp, new List<int> { 90 }, TimeSpan.FromSeconds(TimeBuffer));

            if (previousEventCode != null)
            {
                list.Insert(0, previousEventCode);
            }
            else
            {
                pedDetections++;
            }

            var previousSelectedTimestamp = 0;

            for (var i = 1; i < list.Count; i++)
            {
                if (list[i].Timestamp.Subtract(list[previousSelectedTimestamp].Timestamp).TotalSeconds >= TimeBuffer)
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