using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
    public class RLMPlanCollection 
    {
        

        public double Violations
        {
            get
            {
                return PlanList.Sum(d => d.Violations);
            }
        }

        private List<RLMPlan> planList = new List<RLMPlan>();
        public List<RLMPlan> PlanList
        {
            get { return planList; }
        }

        private double srlvSeconds = 0;
        public double SRLVSeconds
        {
            get
            {
                return srlvSeconds;
            }
        }

        public Models.Approach Approach { get; set; }

        /// <summary>
        /// Default Constructor Used for PCDs
        /// </summary>
        /// <param name="cycleEvents"></param>
        /// <param name="detectortable"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="signalId"></param>
        /// <param name="region"></param>
        public RLMPlanCollection(List<MOE.Common.Models.Controller_Event_Log> cycleEvents, DateTime startdate,
            DateTime enddate,  double srlvSeconds, Models.Approach approach)
        {
            this.Approach = approach;
            this.srlvSeconds = srlvSeconds;
            GetPlanCollection(startdate, enddate, cycleEvents);
        }



        public void GetPlanCollection(DateTime startDate, DateTime endDate, 
            List<MOE.Common.Models.Controller_Event_Log> cycleEvents)
        {

            MOE.Common.Business.ControllerEventLogs ds = 
                new ControllerEventLogs(Approach.SignalID, startDate, endDate, new List<int>() { 131 });
            Models.Controller_Event_Log row = new Models.Controller_Event_Log();
            row.Timestamp = startDate;
            row.SignalID = Approach.SignalID;
            row.EventCode = 131;
            try
            {
                row.EventParam = ControllerEventLogs.GetPreviousPlan(Approach.SignalID, startDate);

                ds.Events.Insert(0, row);
            }
            catch
            {
                row.EventParam = 0;
                ds.Events.Insert(0, row);

            }
            // remove duplicate plan entries
            ds.MergeEvents(ds);
            for (int i = 0; i < ds.Events.Count(); i++)
            {
                //if this is the last plan then we want the end of the plan
                //to cooincide with the end of the graph
                if (ds.Events.Count() - 1 == i)
                {
                    if (ds.Events[i].Timestamp != endDate)
                    {
                        RLMPlan plan = new RLMPlan(ds.Events[i].Timestamp, endDate, ds.Events[i].EventParam, 
                            cycleEvents, this.SRLVSeconds, Approach);
                        this.AddItem(plan);
                    }
                }
                //else we add the plan with the next plans' time stamp as the end of the plan
                else
                {
                    if (ds.Events[i].Timestamp != ds.Events[i + 1].Timestamp)
                    {
                        RLMPlan plan = new RLMPlan(ds.Events[i].Timestamp, 
                            ds.Events[i + 1].Timestamp,ds.Events[i].EventParam,  cycleEvents, this.SRLVSeconds, Approach);
                        this.AddItem(plan);
                    }

                }
            }
        }
       
        public void AddItem(RLMPlan item)
        {
            this.PlanList.Add(item);
        }
        
      
    }
}
