using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.CustomReport
{
    public class Phase //: ControllerEventLogs
    {
        public enum Direction
        {
            Northbound,
            Southbound,
            Eastbound,
            Westbound
        }


        public List<Controller_Event_Log> Events;


        public Phase(Approach approach,
            DateTime startDate, DateTime endDate, List<int> eventCodes, int StartofCycleEvent, bool UsePermissivePhase)
        {
            
            startDate = startDate.AddMinutes(-1);
            endDate = endDate.AddMinutes(+1);
            Approach = approach;
            StartDate = startDate;
            EndDate = endDate;
            if (!UsePermissivePhase)
                PhaseNumber = Approach.ProtectedPhaseNumber;
            else
                PhaseNumber = Approach.PermissivePhaseNumber ?? 0;
            IsOverlap = false;
            SignalID = Approach.Signal.SignalID;
            var cer = ControllerEventLogRepositoryFactory.Create();
            Events = cer.GetEventsByEventCodesParam(SignalID, startDate, endDate, eventCodes, PhaseNumber);
            GetCycles(StartofCycleEvent);
        }

        public string ApproachDirection { get; set; }


        public bool IsOverlap { get; set; }

        public List<DateTime> DetectorActivations { get; set; }

        public List<Cycle> Cycles { get; set; } = new List<Cycle>();

        public Approach Approach { get; set; }

        public string SignalID { get; set; }


        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


        public int PhaseNumber { get; set; }


        private void GetCycles(int StartofCycleEvent)
        {
            if (Events.Exists(s => s.EventCode == 1))
            {
                IsOverlap = false;

                var termEvent = new Cycle.TerminationCause();
                termEvent = Cycle.TerminationCause.Unknown;

                for (var i = 0; i < Events.Count; i++)
                {
                    var CycleStart = new DateTime();
                    var changeToGreen = new DateTime();
                    var beginYellowClear = new DateTime();
                    var endYellowClear = new DateTime();
                    var changeToRed = new DateTime();
                    var greenTerm = new DateTime();
                    var cycleEnd = new DateTime();

                    if (Events[i].EventCode == StartofCycleEvent)
                    {
                        if (i + 1 >= Events.Count)
                            break;
                        CycleStart = Events[i].Timestamp;
                        switch (Events[i].EventCode)
                        {
                            case 1:
                                changeToGreen = Events[i].Timestamp;
                                break;
                            case 4:
                                termEvent = Cycle.TerminationCause.GapOut;
                                break;
                            case 5:
                                termEvent = Cycle.TerminationCause.MaxOut;
                                break;
                            case 6:
                                termEvent = Cycle.TerminationCause.ForceOff;
                                break;
                            case 7:
                                greenTerm = Events[i].Timestamp;
                                break;
                            case 8:
                                beginYellowClear = Events[i].Timestamp;
                                break;
                            case 9:
                                endYellowClear = Events[i].Timestamp;
                                changeToRed = Events[i].Timestamp;
                                break;
                            //case 10:
                            //    changeToRed = Events[i].Timestamp;
                            //    break;
                        }

                        var s = i + 1;


                        while (Events[s].EventCode != StartofCycleEvent && s != Events.Count)
                        {
                            switch (Events[s].EventCode)
                            {
                                case 1:
                                    changeToGreen = Events[s].Timestamp;
                                    break;
                                case 4:
                                    termEvent = Cycle.TerminationCause.GapOut;
                                    break;
                                case 5:
                                    termEvent = Cycle.TerminationCause.MaxOut;
                                    break;
                                case 6:
                                    termEvent = Cycle.TerminationCause.ForceOff;
                                    break;
                                case 7:
                                    greenTerm = Events[s].Timestamp;
                                    break;
                                case 8:
                                    beginYellowClear = Events[s].Timestamp;
                                    break;
                                case 9:
                                    endYellowClear = Events[s].Timestamp;
                                    changeToRed = Events[s].Timestamp;
                                    break;
                                //case 10:
                                //    changeToRed = Events[s].Timestamp;
                                //    break;
                            }
                            s++;
                            if (s >= Events.Count)
                            {
                                i = s;

                                //deal with the very last cycle
                                if (
                                    CycleStart > DateTime.MinValue
                                )
                                {
                                    if (changeToGreen == DateTime.MinValue)
                                        changeToGreen = CycleStart;

                                    if (beginYellowClear == DateTime.MinValue)
                                        beginYellowClear = CycleStart.AddSeconds(1);

                                    if (endYellowClear == DateTime.MinValue)
                                        endYellowClear = CycleStart.AddSeconds(4);


                                    if (changeToRed == DateTime.MinValue)
                                        changeToRed = CycleStart.AddSeconds(5);


                                    if (Events.Last().EventCode == 1)
                                        cycleEnd = Events.Last().Timestamp;
                                    else
                                        cycleEnd = EndDate;

                                    var _Cycle = new Cycle(CycleStart, changeToGreen,
                                        beginYellowClear, changeToRed, cycleEnd);
                                    _Cycle.EndYellowClearance = endYellowClear;
                                    _Cycle.TerminationEvent = termEvent;

                                    Cycles.Add(_Cycle);
                                }
                                break;
                            }
                        }

                        if (s >= Events.Count)
                            break;

                        i = s - 1;
                        cycleEnd = Events[s].Timestamp;


                        if (
                            CycleStart > DateTime.MinValue &&
                            changeToGreen > DateTime.MinValue &&
                            beginYellowClear > DateTime.MinValue &&
                            changeToRed > DateTime.MinValue
                        )
                        {
                            var _Cycle = new Cycle(CycleStart, changeToGreen,
                                beginYellowClear, changeToRed, cycleEnd);
                            _Cycle.EndYellowClearance = endYellowClear;
                            _Cycle.TerminationEvent = termEvent;

                            Cycles.Add(_Cycle);
                        }
                        //else 
                    }
                }
            }


            //if (Events.Exists(s => s.EventCode == 64))
            //{
            //    IsOverlap = true;
            //    for (int i = 0; i < Events.Count - 4; i++)
            //    {
            //        if (Events[i].EventCode == 64 && Events[i + 1].EventCode == 61 &&
            //            Events[i + 2].EventCode == 63 && Events[i + 3].EventCode == 64)
            //        {
            //            if ((Events[i + 3].Timestamp - Events[i].Timestamp).TotalSeconds < 300)
            //            {
            //                _Cycles.Add(new CustomReport.Cycle(Events[i].Timestamp, Events[i + 1].Timestamp,
            //                Events[i + 2].Timestamp, Events[i + 3].Timestamp, Events[i + 3].Timestamp));

            //                i = i + 3;
            //            }
            //        }
            //    }
            //}
        }
    }
}