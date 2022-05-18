using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;

namespace MOE.Common.Business
{
    public class ControllerEventLogs
    {
        private readonly SPM _db = new SPM();

        public ControllerEventLogs(string signalId, DateTime startDate, DateTime endDate)
        {
            SignalId = signalId;
            StartDate = startDate;
            EndDate = endDate;
            EventCodes = new List<int>();
            Events = new List<Controller_Event_Log>();
        }

        public ControllerEventLogs()
        {
            Events = new List<Controller_Event_Log>();
        }

        public ControllerEventLogs(string signalID, DateTime startDate, DateTime endDate, List<int> eventCodes)
        {
            SignalId = signalID;
            StartDate = startDate;
            EndDate = endDate;
            EventCodes = eventCodes;

            var events = from s in _db.Controller_Event_Log
                         where s.SignalID == signalID &&
                               s.Timestamp >= startDate &&
                               s.Timestamp <= endDate &&
                               eventCodes.Contains(s.EventCode)
                         select s;

            Events = events.ToList();
            Events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));
        }

        public ControllerEventLogs(string signalID, DateTime startDate, DateTime endDate, List<int> eventCodes, SPM db)
        {
            SignalId = signalID;
            StartDate = startDate;
            EndDate = endDate;
            EventCodes = eventCodes;

            var events = from s in db.Controller_Event_Log
                         where s.SignalID == signalID &&
                               s.Timestamp >= startDate &&
                               s.Timestamp <= endDate &&
                               eventCodes.Contains(s.EventCode)
                         select s;

            Events = events.ToList();
            Events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));
        }

        public ControllerEventLogs(string signalID, DateTime startDate, DateTime endDate, int eventParam,
            List<int> eventCodes)
        {
            SignalId = signalID;
            StartDate = startDate;
            EndDate = endDate;
            EventCodes = eventCodes;

            var events = from s in _db.Controller_Event_Log
                         where s.SignalID == signalID &&
                               s.Timestamp >= startDate &&
                               s.Timestamp <= endDate &&
                               eventCodes.Contains(s.EventCode) &&
                               s.EventParam == eventParam
                         select s;

            Events = events.ToList();
            Events = Events.OrderBy(e => e.Timestamp).ThenBy(e => e.EventCode).ToList();
            //Events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));
        }

        public string SignalId { get; }
        public DateTime StartDate { get; protected set; }
        public DateTime EndDate { get; protected set; }
        public List<int> EventCodes { get; }
        public List<Controller_Event_Log> Events { get; set; }

        public void FillforPreempt(string signalID, DateTime startDate, DateTime endDate)
        {
            var Codes = new List<int>();

            for (var i = 101; i <= 111; i++)
                Codes.Add(i);

            var db = new SPM();

            var events = (from s in db.Controller_Event_Log
                          where s.SignalID == signalID &&
                                s.Timestamp >= startDate &&
                                s.Timestamp <= endDate &&
                                Codes.Contains(s.EventCode)
                          select s).ToList();

            Events.AddRange(events);
            OrderEventsBytimestamp();
        }

        public void Add105Events(string signalId, DateTime startDate, DateTime endDate)
        {
            var events = (from s in _db.Controller_Event_Log
                          where s.SignalID == signalId &&
                                s.Timestamp >= startDate &&
                                s.Timestamp <= endDate &&
                                (s.EventCode == 105 || s.EventCode == 111)
                          select s).ToList();
            foreach (var v in events)
            {
                v.EventCode = 99;
                Events.Add(v);
            }
            OrderEventsBytimestamp();
        }

        public void OrderEventsBytimestamp()
        {
            var tempEvents = Events.OrderBy(x => x.Timestamp).ToList();

            Events.Clear();
            Events.AddRange(tempEvents);
        }

        public static DateTime GetMostRecentRecordTimestamp(string signalID)
        {
            var db = new SPM();

            var twoDaysAgo = DateTime.Now.AddDays(-2);

            var row = (from r in db.Controller_Event_Log
                       where r.SignalID == signalID && r.Timestamp > twoDaysAgo
                       orderby r.Timestamp descending
                       select r).Take(1).FirstOrDefault();


            if (row != null)
                return row.Timestamp;
            return twoDaysAgo;
        }


        public void MergeEvents(ControllerEventLogs newEvents)
        {
            if (newEvents.StartDate < StartDate)
                StartDate = newEvents.StartDate;

            if (newEvents.EndDate > EndDate)
                EndDate = newEvents.EndDate;

            var incomingEventCodes = (from s in newEvents.EventCodes
                                      select s).Distinct();

            foreach (var i in incomingEventCodes)
                if (!EventCodes.Contains(i))
                    EventCodes.AddRange(newEvents.EventCodes);

            Events.AddRange(newEvents.Events);
            Events = Events.Distinct().ToList();
            Events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));
        }

        public static List<int> GetPedPhases(string signalID, DateTime startDate, DateTime endDate)
        {
            var db = new SPM();
            var pedEventCodes = new List<int> { 21, 45, 90, 22 };

            var events = (from s in db.Controller_Event_Log
                          where s.SignalID == signalID &&
                                s.Timestamp >= startDate &&
                                s.Timestamp <= endDate &&
                                pedEventCodes.Contains(s.EventCode)
                          select s.EventParam).Distinct();
            return events.ToList();
        }

        public static int GetPreviousPlan(string signalID, DateTime startDate)
        {
            var db = new SPM();
            var endDate = startDate.AddHours(-12);
            var planRecord = from r in db.Controller_Event_Log
                             where r.SignalID == signalID &&
                                   r.Timestamp >= endDate &&
                                   r.Timestamp <= startDate &&
                                   r.EventCode == 131
                             select r;
            if (planRecord.Count() > 0)
                return planRecord.OrderByDescending(s => s.Timestamp).FirstOrDefault().EventParam;
            return 0;
        }

        public static Controller_Event_Log GetEventBeforeEvent(string signalID, int phase, DateTime startDate)
        {
            var db = new SPM();
            var endDate = startDate.AddHours(-12);
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

        public static Controller_Event_Log GetEventFromPreviousBin(string signalID, int phase, DateTime currentTime, List<int> chosenEvents, TimeSpan lookbackTime)
        {
            var db = new SPM();
            var startTime = currentTime - lookbackTime;
            var eventRecord = (from s in db.Controller_Event_Log
                               where s.SignalID == signalID &&
                                     s.EventParam == phase &&
                                     s.Timestamp > startTime &&
                                     s.Timestamp < currentTime &&
                                     chosenEvents.Contains(s.EventCode)
                               orderby s.Timestamp descending
                               select s
            ).FirstOrDefault();

            return eventRecord;
        }  
    }
}