using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business
{
    public class PlansBase:ControllerEventLogs
    {        

        public PlansBase(string signalID, DateTime startDate, DateTime endDate):
            base(signalID, startDate, endDate, new List<int> {131})
        {
            //Get the plan Previous to the start date
            //if(this.Events.Count > 0)
            //{
                Models.Controller_Event_Log tempEvent = new Models.Controller_Event_Log();
                tempEvent.SignalID =signalID;
                tempEvent.Timestamp = startDate;
                tempEvent.EventCode = 131;
                tempEvent.EventParam = ControllerEventLogs.GetPreviousPlan(signalID, startDate);

                this.Events.Insert(0, tempEvent);
            //}

            //Remove Duplicate Plans
            int x = -1;
            List<Models.Controller_Event_Log> temp = new List<Models.Controller_Event_Log>();
            foreach (Models.Controller_Event_Log cel in Events)
            {
                temp.Add(cel);
            }
            foreach(Models.Controller_Event_Log cel in temp)
            {
                if(x==-1)
                {
                    x = cel.EventParam;
                }
                else if (x != cel.EventParam)
                {
                    x = cel.EventParam;
                    continue;
                }
                else if (x == cel.EventParam)
                {
                    x = cel.EventParam;
                    this.Events.Remove(cel);
                    continue;

                }
            }
        }


    }
}
