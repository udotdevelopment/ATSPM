using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;

namespace MOE.Common.Business
{
    public class ControllerEventLogs
    {
        //private readonly SPM db = new SPM();

        MOE.Common.Models.Repositories.IControllerEventLogRepository CELRepo = Models.Repositories.ControllerEventLogRepositoryFactory.Create();


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

        public ControllerEventLogs(string signalId, DateTime startDate, DateTime endDate, List<int> eventCodes)
        {
            SignalId = signalId;
            StartDate = startDate;
            EndDate = endDate;
            EventCodes = eventCodes;

            var events = CELRepo.GetSignalEventsByEventCodes(signalId, startDate, endDate, eventCodes);


            Events = events.ToList();
            
        }

        public ControllerEventLogs(string signalId, DateTime startDate, DateTime endDate, int eventParam,
            List<int> eventCodes)
        {
            SignalId = signalId;
            StartDate = startDate;
            EndDate = endDate;
            EventCodes = eventCodes;

            var events = CELRepo.GetEventsByEventCodesParam(signalId, startDate, endDate, eventCodes, eventParam);

            Events = events.ToList();
            Events = Events.OrderBy(e => e.Timestamp).ThenBy(e => e.EventCode).ToList();
            Events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));
        }

        public string SignalId { get; }
        public DateTime StartDate { get; protected set; }
        public DateTime EndDate { get; protected set; }
        public List<int> EventCodes { get; }
        public List<Controller_Event_Log> Events { get; set; }

        public void FillforPreempt(string signalId, DateTime startDate, DateTime endDate)
        {
            var codes = new List<int>();

            for (var i = 101; i <= 111; i++)
                codes.Add(i);

            

            var sid = Convert.ToInt16(signalId);

            var events = CELRepo.GetSignalEventsByEventCodes(signalId, startDate, endDate, codes);


            Events.AddRange(events);
            OrderEventsBytimestamp();
        }

        public void Add105Events(string signalId, DateTime startDate, DateTime endDate)
        {
            var codes = new List<int>();
            codes.Add(105);
            codes.Add(111);
            var events = CELRepo.GetSignalEventsByEventCodes(signalId, startDate, endDate, codes);
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

        public static int GetPreviousPlan(string signalId, DateTime startDate)
        {
            var CELRepo = MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();

            var endDate = startDate.AddHours(-12);

            var planRecord = CELRepo.GetSignalEventsByEventCode(signalId, endDate, startDate, 131);

            if (planRecord.Count() > 0)
                return planRecord.OrderByDescending(s => s.Timestamp).FirstOrDefault().EventParam;
            return 0;
        }

       
    }
}