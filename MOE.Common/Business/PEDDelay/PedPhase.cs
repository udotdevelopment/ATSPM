using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MOE.Common.Business.PEDDelay
{
    public class PedPhase:ControllerEventLogs
    {
        private int _PhaseNumber;
        public int PhaseNumber { get{return _PhaseNumber;} }

        protected string _SignalID;
        public string SignalID { get { return _SignalID; } }

        protected DateTime _StartDate;
        public DateTime StartDate { get { return _StartDate; } }

        protected DateTime _EndDate;
        public DateTime EndDate { get { return _EndDate; } }        

        public double PedActuations { get{return _Plans.Sum(p => p.PedActuations);} }

        private double _MinDelay;

        public double MinDelay
        {
            get { return _MinDelay; }
        }

        private double _MaxDelay;

        public double MaxDelay
        {
            get { return _MaxDelay; }
        }

        private double _AverageDelay;

        public double AverageDelay
        {
            get { return _AverageDelay; }
        }

        private List<PedCycle> _Cycles = new List<PedCycle>();

        public List<PedCycle> Cycles
        {
            get { return _Cycles; }
        }
        

        protected List<PedPlan> _Plans = new List<PedPlan>();
        public List<PedPlan> Plans{ get{ return _Plans; } }

        private List<PedHourlyTotal> _HourlyTotals = new List<PedHourlyTotal>();

        public List<PedHourlyTotal> HourlyTotals
        {
            get { return _HourlyTotals; }
        }
        

        public PedPhase(int phaseNumber, string signalID, DateTime startDate, DateTime endDate,
            PlansBase plansData):base(signalID,startDate, endDate, phaseNumber, new List<int>{21,22,45})
        {
            _SignalID = signalID;
            _StartDate = startDate;
            _EndDate = endDate;
            _PhaseNumber = phaseNumber;

            for (int i = 0; i < plansData.Events.Count; i++)
            {
                //if this is the last plan then we want the end of the plan
                //to cooincide with the end of the graph
                if (plansData.Events.Count - 1 == i)
                {
                    PedPlan plan = new PedPlan(signalID, phaseNumber, plansData.Events[i].Timestamp, endDate,
                        plansData.Events[i].EventParam);
                    _Plans.Add(plan);
                }
                //else we add the plan with the next plans' time stamp as the end of the plan
                else
                {

                    PedPlan plan = new PedPlan(signalID, phaseNumber, plansData.Events[i].Timestamp, 
                        plansData.Events[i + 1].Timestamp, plansData.Events[i].EventParam);

                    _Plans.Add(plan);

                }
            }

            GetCycles();
            AddCyclesToPlans();
            SetHourlyTotals();

        }

        private void AddCyclesToPlans()
        {
            foreach(PedPlan p in Plans)
            {
                List<PedCycle> cycles = (from c in Cycles
                                        where c.CallRegistered >= p.StartDate &&
                                        c.CallRegistered < p.EndDate
                                        select c).ToList<PedCycle>();
                p.Cycles = cycles;
            }
        }

        private void GetCycles()
        {
            
            for (int i = 0; i < Events.Count; i++)
            {
                if (i < Events.Count - 2 && Events[i].EventCode == 21 &&
                    Events[i + 1].EventCode == 45 && Events[i + 2].EventCode == 22)
                {
                    Cycles.Add(new PedCycle(Events[i].Timestamp, Events[i + 1].Timestamp));
                    i = i+2;
                }
                else if (i < Events.Count - 2 && Events[i].EventCode == 22 &&
                    Events[i + 1].EventCode == 45 && Events[i + 2].EventCode == 21)
                {
                    Cycles.Add(new PedCycle(Events[i+1].Timestamp, Events[i + 2].Timestamp));
                    i = i + 2;
                }
            }
        }

        private void SetHourlyTotals()
        {
            //Get Min Max and Average
            if (Cycles.Count > 0)
            {
                _MinDelay = Cycles.Min(c => c.Delay);
                _MaxDelay = Cycles.Max(c => c.Delay);
                _AverageDelay = Cycles.Average(c => c.Delay);


                DateTime dt = new DateTime(this.StartDate.Year, StartDate.Month, StartDate.Day, StartDate.Hour, 0, 0);
                DateTime nextDt = dt.AddHours(1);
                while (dt < this.EndDate)
                {
                    double hourDelay = (from c in Cycles
                                        where c.CallRegistered >= dt &&
                                        c.CallRegistered < nextDt
                                        select c.Delay).Sum();
                    _HourlyTotals.Add(new PedHourlyTotal(dt, hourDelay));
                    dt = dt.AddHours(1);
                    nextDt = nextDt.AddHours(1);
                }
            }
        }
    }
}

