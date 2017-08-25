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

        protected string _SignalID;
        public string SignalID { get { return _SignalID; } }

        protected DateTime _StartDate;
        public DateTime StartDate { get { return _StartDate; } }

        protected DateTime _EndDate;
        public DateTime EndDate { get { return _EndDate; } }

        protected List<int> _EventCodes;
        public List<int> EventCodes { get { return _EventCodes; } }

        public List<Models.Controller_Event_Log> Events { get; set; }

        public ControllerEventLogs(string signalID, DateTime startDate, DateTime endDate)
        {
            _SignalID = signalID;
            _StartDate = startDate;
            _EndDate = endDate;
            _EventCodes = new List<int>();
            Events = new List<Models.Controller_Event_Log>();
        }

        public ControllerEventLogs()
        {
            Events = new List<Models.Controller_Event_Log>();
        }

        public void FillforPreempt(string signalID, DateTime startDate, DateTime endDate)
        {
            List<int> Codes = new List<int>();

            for (int i = 101; i <= 111; i++)
            {
                Codes.Add(i);
            }

            Models.SPM db = new Models.SPM();
           
            var events = (from s in db.Controller_Event_Log
                          where s.SignalID == signalID &&
                          s.Timestamp >= startDate &&
                          s.Timestamp <= endDate &&
                          Codes.Contains(s.EventCode)
                          select s).ToList();

            this.Events.AddRange(events);
            OrderEventsBytimestamp();

        }

        public void Add105Events(string signalID, DateTime startDate, DateTime endDate)
        {
            var events = (from s in db.Controller_Event_Log
                          where s.SignalID == signalID &&
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

        static public DateTime GetMostRecentRecordTimestamp(string signalID)
        {
            Models.SPM db = new Models.SPM();

            DateTime twoDaysAgo = DateTime.Now.AddDays(-2);

            MOE.Common.Models.Controller_Event_Log row = (from r in db.Controller_Event_Log
                                                           where r.SignalID == signalID && r.Timestamp > twoDaysAgo
                                                           orderby r.Timestamp descending
                                                         select r).Take(1).FirstOrDefault();
                                                         

                                                        
            if (row != null)
            {
                return row.Timestamp;
            }
            else
            {
                return twoDaysAgo;
            }
        }


        public void MergeEvents(ControllerEventLogs newEvents)
        {
            if (newEvents._StartDate < this._StartDate)
            {
                this._StartDate = newEvents._StartDate;
            }

            if (newEvents._EndDate > this._EndDate)
            {
                this._EndDate = newEvents._EndDate;
            }

            var incomingEventCodes = (from s in newEvents.EventCodes
            select s).Distinct();

            foreach (int i in incomingEventCodes)
            {
                if (!this._EventCodes.Contains(i))
                {
                    this._EventCodes.AddRange(newEvents._EventCodes);
                }
            }

            this.Events.AddRange(newEvents.Events);
            Events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp)); 
        }

        public ControllerEventLogs(string signalID, DateTime startDate, DateTime endDate, List<int> eventCodes)
        {
            _SignalID = signalID;
            _StartDate = startDate;
            _EndDate = endDate;
            _EventCodes = eventCodes;

            var events = from s in db.Controller_Event_Log
                         where s.SignalID == signalID && 
                         s.Timestamp >= startDate &&
                         s.Timestamp <= endDate &&
                         eventCodes.Contains(s.EventCode)
                         select s;

            Events = events.ToList<Models.Controller_Event_Log>();
            Events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp)); 
        }

        public ControllerEventLogs(string signalID, DateTime startDate, DateTime endDate, int eventParam, List<int> eventCodes)
        {
            _SignalID = signalID;
            _StartDate = startDate;
            _EndDate = endDate;
            _EventCodes = eventCodes;

            var events = from s in db.Controller_Event_Log
                         where s.SignalID == signalID &&
                         s.Timestamp >= startDate &&
                         s.Timestamp <= endDate &&                         
                         eventCodes.Contains(s.EventCode) &&
                         s.EventParam == eventParam
                         select s;

            Events = events.ToList<Models.Controller_Event_Log>();
            Events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp)); 
        }

        static public List<int> GetPedPhases(string signalID, DateTime startDate, DateTime endDate)
        {
            Models.SPM db = new Models.SPM();
            List<int> pedEventCodes = new List<int>{21,45,90,23};
            var events = (from s in db.Controller_Event_Log
                         where s.SignalID == signalID &&
                         s.Timestamp >= startDate &&
                         s.Timestamp <= endDate &&
                         pedEventCodes.Contains(s.EventCode)
                         select s.EventParam).Distinct();
            return events.ToList<int>();
        }

        static public int GetPreviousPlan(string signalID, DateTime startDate)
        {
            Models.SPM db = new Models.SPM();
            DateTime endDate = startDate.AddHours(-12);


            var planRecord= from r in db.Controller_Event_Log 
                         where r.SignalID == signalID &&
                             r.Timestamp >= endDate &&
                             r.Timestamp <= startDate &&
                             r.EventCode == 131
                            select r;

            if (planRecord.Count() > 0)
            {return planRecord.OrderByDescending(s=>s.Timestamp).FirstOrDefault().EventParam;}
            else
            { return 0; }
        }

        static public MOE.Common.Models.Controller_Event_Log GetEventBeforeEvent(string signalID, int phase, DateTime startDate)
        {
            Models.SPM db = new Models.SPM();
            DateTime endDate = startDate.AddHours(-12);
            var eventRecord = (from s in db.Controller_Event_Log
                              orderby s.Timestamp descending
                              where s.SignalID == signalID &&
                              s.EventParam == phase &&
                              s.Timestamp <= startDate &&
                              s.Timestamp >= endDate
                              select s
                          ).DefaultIfEmpty(null).First();
            return eventRecord;
        }
    }
}
