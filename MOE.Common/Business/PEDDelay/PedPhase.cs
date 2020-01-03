using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;

namespace MOE.Common.Business.PEDDelay
{
    public class PedPhase : ControllerEventLogs
    {
        public PedPhase(int phaseNumber, Models.Signal signal, DateTime startDate, DateTime endDate,
            PlansBase plansData) : base(signal.SignalID, startDate, endDate, phaseNumber, new List<int> { 21, 22, 45, 90 })
        {
            SignalID = signal.SignalID;
            StartDate = startDate;
            EndDate = endDate;
            PhaseNumber = phaseNumber;
            StartDate = startDate;
            EndDate = endDate;
            Plans = new List<PedPlan>();
            Cycles = new List<PedCycle>();
            HourlyTotals = new List<PedHourlyTotal>();

            for (var i = 0; i < plansData.Events.Count; i++)
                //if this is the last plan then we want the end of the plan
                //to cooincide with the end of the graph
                if (plansData.Events.Count - 1 == i)
                {
                    var plan = new PedPlan(SignalID, phaseNumber, plansData.Events[i].Timestamp, endDate,
                        plansData.Events[i].EventParam);
                    Plans.Add(plan);
                }
                //else we add the plan with the next plans' time stamp as the end of the plan
                else
                {
                    var plan = new PedPlan(SignalID, phaseNumber, plansData.Events[i].Timestamp,
                        plansData.Events[i + 1].Timestamp, plansData.Events[i].EventParam);

                    Plans.Add(plan);
                }

            GetCycles(signal);
            AddCyclesToPlans();
            SetHourlyTotals();
        }

        public int PhaseNumber { get; }
        public string SignalID { get; }

        public double PedActuations
        {
            get { return Plans.Sum(p => p.PedActuations); }
        }

        public List<PedCycle> Cycles { get; }
        public List<PedPlan> Plans { get; }
        public List<PedHourlyTotal> HourlyTotals { get; }
        public double MinDelay { get; private set; }
        public double AverageDelay { get; private set; }
        public double MaxDelay { get; private set; }
        public double TotalDelay { get; set; }

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

        private void GetCycles(Signal signal)
        {
            // both types work the same by only looking at code 90 between 21 and 22
            //if (signal.ControllerTypeID == 4)
            //{
                GetCyclesForMaxtimeControllers();
            //}
            //else
            //{
              //  GetCyclesForNonMaxtimeControllers();
            //}
        }

        private void GetCyclesForMaxtimeControllers()
        {
            CombineSequential90Events();
            if (Events[0].EventCode == 90 && Events[1].EventCode == 21)
            {
                Cycles.Add(new PedCycle(Events[1].Timestamp, Events[0].Timestamp));  // Middle of the event
            }
            for (var i = 0; i < Events.Count -2; i++)
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
                    Events[i + 1].EventCode == 90 && Events[i + 2].EventCode == 21)
                {
                    Cycles.Add(new PedCycle(Events[i + 2].Timestamp, Events[i + 1].Timestamp));  // this is case 1
                    i++;
                }
                else if (Events[i].EventCode == 21 &&
                         Events[i + 1].EventCode == 90 && Events[i + 2].EventCode == 22)
                {
                    Cycles.Add(new PedCycle(Events[i + 1].Timestamp, Events[i + 1].Timestamp));  // this is case 2
                    i++;
                }
                else if (Events[i].EventCode == 21 &&
                         Events[i + 1].EventCode == 90 && Events[i + 2].EventCode == 22)
                {
                    i++;                                                                   // ignore this - case 3
                }
                else if (i < Events.Count - 2 && Events[i].EventCode == 21 &&
                         Events[i + 1].EventCode == 90 && Events[i + 2].EventCode == 21)
                {
                    Cycles.Add(new PedCycle(Events[i + 2].Timestamp, Events[i + 1].Timestamp));  // this is case 4
                    i++;
                }
            }
        }

        private void CombineSequential90Events()
        {
            var tempEvents = new List<Models.Controller_Event_Log>();
            for (int i = 0;  i < Events.Count; i++)
            {
                if (i < Events.Count - 3)
                {
                    var eventCode1 = Events[i].EventCode;
                    var eventCode2 = Events[i + 1].EventCode;
                    var eventCode3 = Events[i + 2].EventCode;
                    var eventCode4 = Events[i + 3].EventCode;
                }
                if (Events[i].EventCode == 90)
                {
                    tempEvents.Add(Events[i]);
                    int count = 1;
                    while (i < Events.Count && Events[i].EventCode == 90)
                    {
                        i++;
                        count++;
                    }

                    if (count > 1)
                    {
                        i--;
                    }
                }
                else
                {
                    if(Events[i].EventCode != 45)
                        tempEvents.Add(Events[i]);
                }
            }
            Events = tempEvents.OrderBy(t =>t.Timestamp).ToList();
        }

        private void GetCyclesForNonMaxtimeControllers()
        {
            CombineSequential90Events();

            for (var i = 0; i < Events.Count; i++)
            {
                if (i < Events.Count - 3)
                {
                    var eventCode1 = Events[i].EventCode;
                    var eventCode2 = Events[i + 1].EventCode;
                    var eventCode3 = Events[i + 2].EventCode;
                    var ecentcode4 = Events[i + 3].EventCode;
                }

                if (i < Events.Count - 2 && Events[i].EventCode == 22 &&
                    Events[i + 1].EventCode == 90 && Events[i + 2].EventCode == 21)
                {
                    Cycles.Add(new PedCycle(Events[i + 2].Timestamp, Events[i + 1].Timestamp));  // this is good
                    i = i + 2;
                }
                else if (i < Events.Count - 2 && Events[i].EventCode == 21 &&
                         Events[i + 1].EventCode == 90 && Events[i + 2].EventCode == 22)
                {
                    Cycles.Add(new PedCycle(Events[i + 2].Timestamp, Events[i + 1].Timestamp));
                    i = i + 2;
                }
            }
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