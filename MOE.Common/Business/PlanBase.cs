using System;
using System.Collections.Generic;
using MOE.Common.Models;

namespace MOE.Common.Business
{
    public class PlansBase : ControllerEventLogs
    {
        public PlansBase(string signalID, DateTime startDate, DateTime endDate) :
            base(signalID, startDate, endDate, new List<int> {131})
        {
            //Get the plan Previous to the start date
            //if(this.Events.Count > 0)
            //{
            var tempEvent = new Controller_Event_Log();
            tempEvent.SignalID = signalID;
            tempEvent.Timestamp = startDate;
            tempEvent.EventCode = 131;
            tempEvent.EventParam = GetPreviousPlan(signalID, startDate);

            Events.Insert(0, tempEvent);
            //}

            //Remove Duplicate Plans
            var x = -1;
            var temp = new List<Controller_Event_Log>();
            foreach (var cel in Events)
                temp.Add(cel);
            foreach (var cel in temp)
                if (x == -1)
                {
                    x = cel.EventParam;
                }
                else if (x != cel.EventParam)
                {
                    x = cel.EventParam;
                }
                else if (x == cel.EventParam)
                {
                    x = cel.EventParam;
                    Events.Remove(cel);
                }
        }
    }
}