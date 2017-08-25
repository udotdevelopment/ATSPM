using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business
{
    public class CycleEngine
    {

        /// <summary>
        /// Translates an event code to an event type
        /// </summary>
        /// <param name="EventCode"></param>
        /// <returns></returns>
        private Cycle.EventType GetEventType(int EventCode)
        {
            switch (EventCode)
            {

                case 1:
                    return Cycle.EventType.ChangeToGreen;
                // overlap green
                case 61:
                    return Cycle.EventType.ChangeToGreen;
                case 7:
                    return Cycle.EventType.GreenTermination;
                case 8:
                    return Cycle.EventType.ChangeToYellow;
                case 9:
                    return Cycle.EventType.EndYellowClearance;
                // overlap yellow
                case 63:
                    return Cycle.EventType.ChangeToYellow;
                case 10:
                    return Cycle.EventType.ChangeToRed;
                // overlap red
                case 64:
                    return Cycle.EventType.ChangeToRed;
                default:
                    return Cycle.EventType.Unknown;
            }
        }

        public List<Cycle> CreateCycles(String signalid, int phase, DateTime startTime, DateTime endTime,
    MOE.Common.Business.ControllerEventLogs eventstable)
        {
        
            List<Cycle> cycleCollection = new List<Cycle>();

            Cycle cycle = null;
            //use a counter to help determine when we are on the last row
            int counter = 0;

            foreach (MOE.Common.Models.Controller_Event_Log row in eventstable.Events)
            {
                //use a counter to help determine when we are on the last row
                counter++;
                if (row.Timestamp >= startTime && row.Timestamp <= endTime)
                {
                    //If this is the first cycle we need to handle a special case
                    //where the pcd starts at the start of the requested period to 
                    //make sure we include all data
                    if (cycleCollection.Count == 0 && cycle == null)
                    {
                        //Make the first cycle start on at the exact start of the requested period
                        cycle = new Cycle(startTime);
                        //Add a green event if the first event is yellow
                        if (GetEventType(row.EventCode) == Cycle.EventType.ChangeToYellow)
                        {
                            cycle.NextEvent(Cycle.EventType.ChangeToGreen, startTime.AddMilliseconds(1));
                        }
                        //Add a green and yellow event if first event is red
                        else if (GetEventType(row.EventCode) == Cycle.EventType.ChangeToRed)
                        {
                            cycle.NextEvent(Cycle.EventType.ChangeToGreen, startTime.AddMilliseconds(1));
                            cycle.NextEvent(Cycle.EventType.ChangeToYellow, startTime.AddMilliseconds(2));
                        }

                    }

                    //Check to see if the event is a change to red
                    //The 64 event is for overlaps.
                    if (row.EventCode == 10 || row.EventCode == 64)
                    {
                        //If it is red and the pcd group is empy create a new one
                        if (cycle == null)
                        {
                            cycle = new Cycle(row.Timestamp);
                        }
                        //If the group is not empty than it is the end of the group and the start
                        //of the next group
                        else
                        {
                            cycle.NextEvent(GetEventType(row.EventCode), row.Timestamp);
                            //if the nextevent response is complete add it and start the next group
                            if (cycle.Status == Cycle.NextEventResponse.GroupComplete)
                            {
                                //pcd.setdetectorcollection(detectortable);
                                cycleCollection.Add(cycle);
                                cycle = new Cycle(row.Timestamp);
                            }
                        }
                    }
                    //If the event is not red and the group is not empty
                    //add the event and set the next event
                    else if (cycle != null)
                    {
                        cycle.NextEvent(GetEventType(row.EventCode), row.Timestamp);
                        if (cycle.Status == Cycle.NextEventResponse.GroupComplete)
                        {
                            cycleCollection.Add(cycle);
                            cycle = new Cycle(row.Timestamp);
                        }
                    }
                    if (cycle != null && cycle.Status == Cycle.NextEventResponse.GroupMissingData)
                    {
                        cycle = null;
                    }

                    //If this is the last PCD Group we need to handle a special case
                    //where the pcd starts at the start of the requested period to 
                    //make sure we include all data 
                    else if (counter == eventstable.Events.Count && cycle != null)
                    {



                        //if the last event is red create a new group to consume the remaining 
                        //time in the period

                        if (GetEventType(row.EventCode) == Cycle.EventType.ChangeToRed)
                        {
                            cycle.NextEvent(Cycle.EventType.ChangeToGreen, endTime.AddMilliseconds(-2));
                            cycle.NextEvent(Cycle.EventType.ChangeToYellow, endTime.AddMilliseconds(-1));
                            cycle.NextEvent(Cycle.EventType.ChangeToRed, endTime);
                        }
                        else if (GetEventType(row.EventCode) == Cycle.EventType.ChangeToGreen)
                        {
                            cycle.NextEvent(Cycle.EventType.ChangeToYellow, endTime.AddMilliseconds(-1));
                            cycle.NextEvent(Cycle.EventType.ChangeToRed, endTime);
                        }
                        else if (GetEventType(row.EventCode) == Cycle.EventType.ChangeToYellow)
                        {
                            cycle.NextEvent(Cycle.EventType.ChangeToRed, endTime);
                        }

                        if (cycle.Status != Cycle.NextEventResponse.GroupMissingData)
                        {
                            cycleCollection.Add(cycle);
                        }
                    }

                }
            }
            //if there are no records at all for the selected time, then the line
            //and counts don't show.  This next bit fixes that.
            if (cycleCollection.Count == 0 && (startTime != endTime))
            {
                //then we need to make a dummy PDC group
                //the pcd assumes it starts on red.
                cycle = new Cycle(startTime);

                //and find out what phase state the controller was in by looking for the next phase event 
                //after the end of the plan.



                    MOE.Common.Models.Controller_Event_Log eventBeforePattern = MOE.Common.Business.ControllerEventLogs.GetEventBeforeEvent(signalid, phase, startTime);
                       




                if (eventBeforePattern != null)
                {
                    if (GetEventType(eventBeforePattern.EventCode) == Cycle.EventType.ChangeToRed)
                    {
                        //let it dwell in red (we don't have to add anything).



                        //then add a green phase, a yellow phase and a red phase at the end to complete the cycle
                        cycle.NextEvent(Cycle.EventType.ChangeToGreen, endTime.AddMilliseconds(-2));
                        cycle.NextEvent(Cycle.EventType.ChangeToYellow, endTime.AddMilliseconds(-1));
                        cycle.NextEvent(Cycle.EventType.ChangeToRed, endTime);






                    }

                    else if (GetEventType(eventBeforePattern.EventCode) == Cycle.EventType.ChangeToYellow)
                    {
                        //we were in yellow, though this will probably never happen
                        //We have to add a green to our dummy phase.

                        cycle.NextEvent(Cycle.EventType.ChangeToGreen, startTime.AddMilliseconds(1));
                        //then make it dwell in yellow
                        cycle.NextEvent(Cycle.EventType.ChangeToYellow, startTime.AddMilliseconds(2));
                        //then add a red phase at the end to complete the cycle
                        cycle.NextEvent(Cycle.EventType.ChangeToRed, endTime);


                    }

                    else if (GetEventType(eventBeforePattern.EventCode) == Cycle.EventType.ChangeToGreen)
                    {

                        // make it dwell in green
                        cycle.NextEvent(Cycle.EventType.ChangeToGreen, startTime.AddMilliseconds(1));

                        //then add a yellow phase and a red phase at the end to complete the cycle
                        cycle.NextEvent(Cycle.EventType.ChangeToYellow, endTime.AddMilliseconds(-1));
                        cycle.NextEvent(Cycle.EventType.ChangeToRed, endTime);


                    }


                }
                if (cycle.Status == Cycle.NextEventResponse.GroupComplete)
                {
                    cycleCollection.Add(cycle);
                }
            }



            return cycleCollection;

        }
    }
}
