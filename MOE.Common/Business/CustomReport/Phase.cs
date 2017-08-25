using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

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

        public String ApproachDirection
        {
            get;
            set;
        }


        public bool IsOverlap
        {
            get;
            set;
        }

        private List<DateTime> _DetectorActivations;

        public List<DateTime> DetectorActivations
        {
            get { return _DetectorActivations; }
            set { _DetectorActivations = value; }
        }

        public List<Models.Controller_Event_Log> Events;

        private List<CustomReport.Cycle> _Cycles = new List<Cycle>();

        public List<CustomReport.Cycle> Cycles
        {
            get { return _Cycles; }
            set { _Cycles = value; }
        }
        public Models.Approach Approach { get; set; }

        public string SignalID { get; set; }


        public DateTime StartDate { get; set; }


        private DateTime _EndDate;

        public DateTime EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; }
        }

        
        
        private int p1;
        private string SignalID1;
        private DateTime StartDate1;
        private DateTime EndDate1;
        private List<int> list;
        private int p2;

        public int PhaseNumber { get; set; }

        

        

        public Phase(Models.Approach approach,
            DateTime startDate, DateTime endDate, List<int> eventCodes, int StartofCycleEvent, bool UsePermissivePhase)
           
                
        {
            startDate = startDate.AddMinutes(-1);
            endDate = endDate.AddMinutes(+1);

            Approach = approach;
            
            StartDate = startDate;
            _EndDate = endDate;
            if (!UsePermissivePhase)
            {
                PhaseNumber = Approach.ProtectedPhaseNumber;
            }
            else
            {
                PhaseNumber = Approach.PermissivePhaseNumber??0;
            }
            IsOverlap = false;
            SignalID = Approach.Signal.SignalID;


            MOE.Common.Models.Repositories.IControllerEventLogRepository cer = MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();

            Events = cer.GetEventsByEventCodesParam(SignalID, startDate, endDate,
                eventCodes, PhaseNumber);

            GetCycles(StartofCycleEvent);
        }

       
        

        private void GetCycles(int StartofCycleEvent)
        {
 

            if (Events.Exists(s => s.EventCode == 1))
            {
                IsOverlap = false;
                
                CustomReport.Cycle.TerminationCause termEvent = new Cycle.TerminationCause();
                termEvent = Cycle.TerminationCause.Unknown;

                for (int i = 0; i < Events.Count; i++)
                {
                    DateTime CycleStart = new DateTime();
                    DateTime changeToGreen = new DateTime();
                    DateTime beginYellowClear = new DateTime();
                    DateTime endYellowClear = new DateTime();
                    DateTime changeToRed = new DateTime();
                    DateTime greenTerm = new DateTime();
                    DateTime cycleEnd = new DateTime();
   
                    if (Events[i].EventCode == StartofCycleEvent)
                    {
                        if (i + 1 >= Events.Count)
                        {
                            break;
                        }
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
                        
                        int s = i + 1;
                        
                        
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
                                if(
                                    CycleStart > DateTime.MinValue 
                                   )
                                {
                                    if (changeToGreen == DateTime.MinValue)
                                    { changeToGreen = CycleStart; }

                                    if (beginYellowClear == DateTime.MinValue)
                                    { beginYellowClear = CycleStart.AddSeconds(1); }

                                    if (endYellowClear == DateTime.MinValue)
                                    { endYellowClear = CycleStart.AddSeconds(4); }


                                    if (changeToRed == DateTime.MinValue)
                                    { changeToRed = CycleStart.AddSeconds(5); }


                                    if (Events.Last().EventCode == 1)
                                    {
                                        cycleEnd = Events.Last().Timestamp;
                                    }
                                    else
                                    {
                                        cycleEnd = this.EndDate;
                                    }

                                    CustomReport.Cycle _Cycle = new CustomReport.Cycle(CycleStart, changeToGreen,
                                   beginYellowClear, changeToRed, cycleEnd);
                                    _Cycle.EndYellowClearance = endYellowClear;
                                    _Cycle.TerminationEvent = termEvent;

                                    _Cycles.Add(_Cycle);

                                }
                                break;
                            }
                        }
                        
                        if (s >= Events.Count)
                        {
                            break;
                        }
                        
                        i = s-1;
                        cycleEnd = Events[s].Timestamp;

                       
                        if (
                            CycleStart > DateTime.MinValue &&
                            changeToGreen > DateTime.MinValue &&
                            beginYellowClear > DateTime.MinValue &&
                            changeToRed > DateTime.MinValue 
                            )
                        {
                            CustomReport.Cycle _Cycle = new CustomReport.Cycle(CycleStart, changeToGreen,
                           beginYellowClear, changeToRed, cycleEnd);
                            _Cycle.EndYellowClearance = endYellowClear;
                            _Cycle.TerminationEvent = termEvent;

                            _Cycles.Add(_Cycle);
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
    


       
    

