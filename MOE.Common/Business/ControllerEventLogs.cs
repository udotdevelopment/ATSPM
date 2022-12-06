using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;

namespace MOE.Common.Business
{
    public class ControllerEventLogs
    {
        public string SignalId { get; }
        public DateTime StartDate { get; protected set; }
        public DateTime EndDate { get; protected set; }
        public List<int> EventCodes { get; }
        public List<Controller_Event_Log> Events { get; set; }

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

            var repository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();

            var events = repository.GetSignalEventsByEventCodes(signalID, startDate, endDate, eventCodes);  

            Events = events.ToList();
            Events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));
        }

        public ControllerEventLogs(string signalID, DateTime startDate, DateTime endDate, List<int> eventCodes, SPM db)
        {
            SignalId = signalID;
            StartDate = startDate;
            EndDate = endDate;
            EventCodes = eventCodes;


            var repository = Models.Repositories.ControllerEventLogRepositoryFactory.Create(db);

            var events = repository.GetSignalEventsByEventCodes(signalID, startDate, endDate, eventCodes);
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

            var repository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();

            var events = repository.GetEventsByEventCodesParam(signalID, startDate, endDate, eventCodes, eventParam);

            Events = events.ToList();
            Events = Events.OrderBy(e => e.Timestamp).ThenBy(e => e.EventCode).ToList();
        }

        public ControllerEventLogs(string signalID, DateTime startDate, DateTime endDate, List<int> eventParams,
          List<int> eventCodes)
        {
            SignalId = signalID;
            StartDate = startDate;
            EndDate = endDate;
            EventCodes = eventCodes;

            var repository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();

            Events = repository.GetSignalEventsByEventCodesParams(signalID, startDate, endDate, eventCodes, eventParams)
                .OrderBy(e => e.Timestamp).ThenBy(e => e.EventCode).ToList();
        }

        public void FillforPreempt(string signalID, DateTime startDate, DateTime endDate)
        {
            var Codes = new List<int>();

            for (var i = 101; i <= 111; i++)
                Codes.Add(i);


            var repository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();

            var events = repository.GetSignalEventsByEventCodes(signalID, startDate, endDate, Codes);
            Events.AddRange(events);
            OrderEventsBytimestamp();
        }

        public void Add105Events(string signalId, DateTime startDate, DateTime endDate)
        {

            var repository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();

            var events = repository.GetSignalEventsByEventCodes(signalId, startDate, endDate, new List<int> { 105, 111 });
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
            var twoDaysAgo = DateTime.Now.AddDays(-2);
            var repository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            var row = repository.GetTopEventAfterDate(signalID, twoDaysAgo);
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

        public static int GetPreviousPlan(string signalID, DateTime startDate)
        {
            var endDate = startDate.AddHours(-12);
            var repository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();

            var planRecord = repository.GetSignalEventsByEventCode(signalID, startDate, endDate, 131);
            if (planRecord.Count() > 0)
                return planRecord.OrderByDescending(s => s.Timestamp).FirstOrDefault().EventParam;
            return 0;
        }

        public static Controller_Event_Log GetEventFromPreviousBin(string signalID, int phase, DateTime currentTime, List<int> chosenEvents, TimeSpan lookbackTime)
        {
            var repository = Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            var startTime = currentTime - lookbackTime;
            return repository.GetEventsByEventCodesParam(signalID, startTime, currentTime, chosenEvents, phase)
                .OrderByDescending(s => s.Timestamp)
                .FirstOrDefault();
        }  
    }
}