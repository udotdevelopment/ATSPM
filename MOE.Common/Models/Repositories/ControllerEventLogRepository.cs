using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class ControllerEventLogRepository:IControllerEventLogRepository
    {
        Models.SPM db = new SPM();
        public int GetDetectorActivationCount(string signalID,
             DateTime startTime, DateTime endTime, int detectorChannel)
        {
            int count = (from cel in db.Controller_Event_Log
                         where cel.Timestamp >= startTime
                            && cel.Timestamp < endTime
                            && cel.SignalID == signalID
                            && cel.EventParam == detectorChannel
                            && cel.EventCode == 82
                            select cel).Count();
            return count;
        }

        public double GetTMCVolume(DateTime startDate, DateTime endDate, string signalID, int Phase)
        {
            MOE.Common.Models.Repositories.ISignalsRepository repository =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            var signal = repository.GetSignalBySignalID(signalID);
            List<Models.Detector> graphDetectors = signal.GetDetectorsForSignalByPhaseNumber(Phase);

            List<int> tmcChannels = new List<int>();
            foreach(Models.Detector gd in graphDetectors)
            {
        
                foreach(DetectionType dt in gd.DetectionTypes)
                {
                    if(dt.DetectionTypeID == 4)
                    {
                        tmcChannels.Add(gd.DetChannel);
                    }
                }
           }

            

            double count = (from cel in db.Controller_Event_Log
                           where cel.Timestamp >= startDate
                           && cel.Timestamp < endDate
                           && cel.SignalID == signalID
                           && tmcChannels.Contains(cel.EventParam)
                           && cel.EventCode == 82
                           select cel).Count();

            return count;
        }

        public List<MOE.Common.Models.Controller_Event_Log> GetSplitEvents(string signalID, DateTime startTime, DateTime endTime)
        {
            List<MOE.Common.Models.Controller_Event_Log> results =  (from r in db.Controller_Event_Log
                                                             where r.SignalID == signalID && r.Timestamp > startTime && r.Timestamp < endTime
                                                             && r.EventCode > 130 && r.EventCode < 150
                                                                         select r).ToList();

            return results;

        }

         public List<MOE.Common.Models.Controller_Event_Log> GetSignalEventsBetweenDates(string signalID, 
            DateTime startTime, DateTime endTime)
        {
            try
            {
                return (from r in db.Controller_Event_Log
                        where r.SignalID == signalID
                        && r.Timestamp >= startTime
                        && r.Timestamp < endTime                        
                        select r).ToList();
            }
            catch (Exception ex)
            {
                MOE.Common.Models.Repositories.IApplicationEventRepository logRepository =
                    MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                MOE.Common.Models.ApplicationEvent e = new MOE.Common.Models.ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = this.GetType().ToString();
                e.Function = "GetSignalEventsBetweenDates";
                e.SeverityLevel = MOE.Common.Models.ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }

         public List<MOE.Common.Models.Controller_Event_Log> GetTopNumberOfSignalEventsBetweenDates(string signalID, int NumberOfRecords,
             DateTime startTime, DateTime endTime)
         {
             try
             {
                 List<MOE.Common.Models.Controller_Event_Log> events = 
                     (from r in db.Controller_Event_Log
                         where r.SignalID == signalID
                         && r.Timestamp >= startTime
                         && r.Timestamp < endTime
                      select r).Take(NumberOfRecords).ToList();

                 if(events != null)
                 {
                     return events;
                 }
                 else
                 {
                     List<MOE.Common.Models.Controller_Event_Log> EmptyEvents = new List<MOE.Common.Models.Controller_Event_Log>();
                     return EmptyEvents;

                 }
             }
             catch (Exception ex)
             {
                 MOE.Common.Models.Repositories.IApplicationEventRepository logRepository =
                     MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                 MOE.Common.Models.ApplicationEvent e = new MOE.Common.Models.ApplicationEvent();
                 e.ApplicationName = "MOE.Common";
                 e.Class = this.GetType().ToString();
                 e.Function = "GetTopNumberOfSignalEventsBetweenDates";
                 e.SeverityLevel = MOE.Common.Models.ApplicationEvent.SeverityLevels.High;
                 e.Timestamp = DateTime.Now;
                 e.Description = ex.Message;
                 logRepository.Add(e);
                 throw;
             }
         }

         public int GetRecordCount(string signalID, DateTime startTime, DateTime endTime)
         {
             try
             {
                 return 
                     (from r in db.Controller_Event_Log
                      where r.SignalID == signalID
                      && r.Timestamp >= startTime
                      && r.Timestamp < endTime
                      select r).Count();                 
             }
             catch (Exception ex)
             {
                 MOE.Common.Models.Repositories.IApplicationEventRepository logRepository =
                     MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                 MOE.Common.Models.ApplicationEvent e = new MOE.Common.Models.ApplicationEvent();
                 e.ApplicationName = "MOE.Common";
                 e.Class = this.GetType().ToString();
                 e.Function = "GetRecordCount";
                 e.SeverityLevel = MOE.Common.Models.ApplicationEvent.SeverityLevels.High;
                 e.Timestamp = DateTime.Now;
                 e.Description = ex.Message;
                 logRepository.Add(e);
                 throw;
             }
         }



        public List<MOE.Common.Models.Controller_Event_Log> GetSignalEventsByEventCode(string signalID, 
            DateTime startTime, DateTime endTime, int eventCode)
        {
            try
            {
                return (from r in db.Controller_Event_Log
                        where r.SignalID == signalID
                        && r.Timestamp >= startTime
                        && r.Timestamp < endTime
                        && r.EventCode == eventCode
                        select r).ToList();
            }
            catch (Exception ex)
            {
                MOE.Common.Models.Repositories.IApplicationEventRepository logRepository =
                    MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                MOE.Common.Models.ApplicationEvent e = new MOE.Common.Models.ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = this.GetType().ToString();
                e.Function = "GetSignalEventsByEventCode";
                e.SeverityLevel = MOE.Common.Models.ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }

        public List<MOE.Common.Models.Controller_Event_Log> GetSignalEventsByEventCodes(string signalID,
            DateTime startTime, DateTime endTime, List<int> eventCodes)
        {
            try
            {
                var events = (from s in db.Controller_Event_Log
                              where s.SignalID == signalID &&
                              s.Timestamp >= startTime &&
                              s.Timestamp <= endTime &&
                              eventCodes.Contains(s.EventCode)
                              select s).ToList();
                events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));
                return events;
            }
            catch (Exception ex)
            {
                MOE.Common.Models.Repositories.IApplicationEventRepository logRepository =
                    MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                MOE.Common.Models.ApplicationEvent e = new MOE.Common.Models.ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = this.GetType().ToString();
                e.Function = "GetSignalEventsByEventCodes";
                e.SeverityLevel = MOE.Common.Models.ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }

        public List<MOE.Common.Models.Controller_Event_Log> GetEventsByEventCodesParam(string signalID,
            DateTime startTime, DateTime endTime, List<int> eventCodes, int param)
        {
            try
            {
                var events = (from s in db.Controller_Event_Log
                              where s.SignalID == signalID &&
                              s.Timestamp >= startTime &&
                              s.Timestamp <= endTime &&
                              s.EventParam == param &&
                              eventCodes.Contains(s.EventCode)
                              select s).ToList();
                events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));
                return events;
            }
            catch (Exception ex)
            {
                MOE.Common.Models.Repositories.IApplicationEventRepository logRepository =
                    MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                MOE.Common.Models.ApplicationEvent e = new MOE.Common.Models.ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = this.GetType().ToString();
                e.Function = "GetEventsByEventCodesParam";
                e.SeverityLevel = MOE.Common.Models.ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }

        public List<MOE.Common.Models.Controller_Event_Log> GetEventsByEventCodesParamWithOffset(string signalID,
           DateTime startTime, DateTime endTime, List<int> eventCodes, int param, double offset)
        {
            try
            {
                var events = (from s in db.Controller_Event_Log
                              where s.SignalID == signalID &&
                              s.Timestamp >= startTime &&
                              s.Timestamp <= endTime &&
                              s.EventParam == param &&
                              eventCodes.Contains(s.EventCode)
                              select s).ToList();
                events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));
                foreach(var cel in events)
                {
                    cel.Timestamp = cel.Timestamp.AddMilliseconds(offset);
                }
                return events;
            }
            catch (Exception ex)
            {
                MOE.Common.Models.Repositories.IApplicationEventRepository logRepository =
                    MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                MOE.Common.Models.ApplicationEvent e = new MOE.Common.Models.ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = this.GetType().ToString();
                e.Function = "GetEventsByEventCodesParamWithOffset";
                e.SeverityLevel = MOE.Common.Models.ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }

        }

        public MOE.Common.Models.Controller_Event_Log GetFirstEventBeforeDate(string signalID,
            int eventCode, DateTime date)
        {
            try
            {
                var events = (from s in db.Controller_Event_Log
                              where s.SignalID == signalID &&
                              s.Timestamp >= date.AddDays(-1) &&
                              s.Timestamp < date &&
                              s.EventCode == eventCode
                              select s).ToList();
                events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));
                return events.Last();
            }
            catch (Exception ex)
            {
                MOE.Common.Models.Repositories.IApplicationEventRepository logRepository =
                    MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                MOE.Common.Models.ApplicationEvent e = new MOE.Common.Models.ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = this.GetType().ToString();
                e.Function = "GetEventsByEventCodesParamWithOffset";
                e.SeverityLevel = MOE.Common.Models.ApplicationEvent.SeverityLevels.High;
                e.Description = ex.Message;
                e.Timestamp = DateTime.Now;
                logRepository.Add(e);
                throw;
            }
        }
    }
}
