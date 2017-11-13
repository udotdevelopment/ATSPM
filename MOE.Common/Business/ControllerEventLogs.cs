using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business
{
    public class ControllerEventLogs
    {
        private Models.SPM db = new Models.SPM();
        public string SignalId { get; }
        public DateTime StartDate { get; protected set; }
        public DateTime EndDate { get; protected set; }
        public List<int> EventCodes { get; }
        public List<Models.Controller_Event_Log> Events { get; set; }

        public ControllerEventLogs(string signalId, DateTime startDate, DateTime endDate)
        {
            SignalId = signalId;
            StartDate = startDate;
            EndDate = endDate;
            EventCodes = new List<int>();
            Events = new List<Models.Controller_Event_Log>();
        }

        public ControllerEventLogs()
        {
            Events = new List<Models.Controller_Event_Log>();
        }

        public void FillforPreempt(string signalId, DateTime startDate, DateTime endDate)
        {
            List<int> Codes = new List<int>();

            for (int i = 101; i <= 111; i++)
            {
                Codes.Add(i);
            }

            Models.SPM db = new Models.SPM();
           
            var events = (from s in db.Controller_Event_Log
                          where s.SignalID == signalId &&
                          s.Timestamp >= startDate &&
                          s.Timestamp <= endDate &&
                          Codes.Contains(s.EventCode)
                          select s).ToList();

            this.Events.AddRange(events);
            OrderEventsBytimestamp();

        }

        public void Add105Events(string signalId, DateTime startDate, DateTime endDate)
        {
            var events = (from s in db.Controller_Event_Log
                          where s.SignalID == signalId &&
                          s.Timestamp >= startDate &&
                          s.Timestamp <= endDate &&
                          (s.EventCode == 105 || s.EventCode == 111)
                          select s).ToList();
            foreach (var v in events)
            {
                v.EventCode = 99;
                this.Events.Add(v);
            }
            OrderEventsBytimestamp();
        }

        public void OrderEventsBytimestamp()
        {

            List<Models.Controller_Event_Log> tempEvents =  this.Events.OrderBy(x => x.Timestamp).ToList();

            this.Events.Clear();
            this.Events.AddRange(tempEvents);

        }

        public static DateTime GetMostRecentRecordTimestamp(string signalId)
        {
            Models.SPM db = new Models.SPM();

            DateTime twoDaysAgo = DateTime.Now.AddDays(-2);

            MOE.Common.Models.Controller_Event_Log row = (from r in db.Controller_Event_Log
                                                           where r.SignalID == signalId && r.Timestamp > twoDaysAgo
                                                           orderby r.Timestamp descending
                                                         select r).Take(1).FirstOrDefault();
                                                         

                                                        
            if (row != null)
            {
                return row.Timestamp;
            }
            return twoDaysAgo;
        }


        public void MergeEvents(ControllerEventLogs newEvents)
        {
            if (newEvents.StartDate < this.StartDate)
            {
                this.StartDate = newEvents.StartDate;
            }

            if (newEvents.EndDate > this.EndDate)
            {
                this.EndDate = newEvents.EndDate;
            }

            var incomingEventCodes = (from s in newEvents.EventCodes
            select s).Distinct();

            foreach (int i in incomingEventCodes)
            {
                if (!this.EventCodes.Contains(i))
                {
                    this.EventCodes.AddRange(newEvents.EventCodes);
                }
            }

            this.Events.AddRange(newEvents.Events);
            Events = Events.Distinct().ToList();
            Events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp)); 
        }

        public ControllerEventLogs(string signalId, DateTime startDate, DateTime endDate, List<int> eventCodes)
        {
            SignalId = signalId;
            StartDate = startDate;
            EndDate = endDate;
            EventCodes = eventCodes;

            var events = from s in db.Controller_Event_Log
                         where s.SignalID == signalId && 
                         s.Timestamp >= startDate &&
                         s.Timestamp <= endDate &&
                         eventCodes.Contains(s.EventCode)
                         select s;

            Events = events.ToList<Models.Controller_Event_Log>();
            Events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp)); 
        }

        public ControllerEventLogs(string signalId, DateTime startDate, DateTime endDate, int eventParam, List<int> eventCodes)
        {
            SignalId = signalId;
            StartDate = startDate;
            EndDate = endDate;
            EventCodes = eventCodes;

            var events = from s in db.Controller_Event_Log
                         where s.SignalID == signalId &&
                         s.Timestamp >= startDate &&
                         s.Timestamp <= endDate &&                         
                         eventCodes.Contains(s.EventCode) &&
                         s.EventParam == eventParam
                         select s;

            Events = events.ToList<Models.Controller_Event_Log>();
            Events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp)); 
        }

        public static List<int> GetPedPhases(string signalId, DateTime startDate, DateTime endDate)
        {
            Models.SPM db = new Models.SPM();
            List<int> pedEventCodes = new List<int>{21,45,90,23};
            var events = (from s in db.Controller_Event_Log
                         where s.SignalID == signalId &&
                         s.Timestamp >= startDate &&
                         s.Timestamp <= endDate &&
                         pedEventCodes.Contains(s.EventCode)
                         select s.EventParam).Distinct();
            return events.ToList<int>();
        }

        public static int GetPreviousPlan(string signalId, DateTime startDate)
        {
            Models.SPM db = new Models.SPM();
            DateTime endDate = startDate.AddHours(-12);


            var planRecord= from r in db.Controller_Event_Log 
                         where r.SignalID == signalId &&
                             r.Timestamp >= endDate &&
                             r.Timestamp <= startDate &&
                             r.EventCode == 131
                            select r;

            if (planRecord.Any())
            {return planRecord.OrderByDescending(s=>s.Timestamp).FirstOrDefault().EventParam;}
            return 0;
        }

        public static MOE.Common.Models.Controller_Event_Log GetEventBeforeEvent(string signalId, int phase, DateTime startDate)
        {
            Models.SPM db = new Models.SPM();
            DateTime endDate = startDate.AddHours(-12);
            var eventRecord = (from s in db.Controller_Event_Log
                              orderby s.Timestamp descending
                              where s.SignalID == signalId &&
                              s.EventParam == phase &&
                              s.Timestamp <= startDate &&
                              s.Timestamp >= endDate
                              select s
                          ).DefaultIfEmpty(null).First();
            return eventRecord;
        }
    }
}
