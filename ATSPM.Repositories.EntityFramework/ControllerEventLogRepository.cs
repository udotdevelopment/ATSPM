using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class ControllerEventLogRepository : IControllerEventLogRepository
    {

        private readonly MOEContext _db;
        public ControllerEventLogRepository(MOEContext db)
        {
            _db = db;
            //var i = 0;
            //for ( i = 0; i < 10; i++)
            //{
            //    try
            //    {

            //        //_db.Database.CommandTimeout = 180;
            //        db.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            //        db.Configuration.AutoDetectChangesEnabled = false;
            //        _db = db;
            //        break;
            //    }
            //    catch 
            //    {
            //        Console.WriteLine(" Inside a catch statement for setting the TRANSACTION ISOLATION LEVEL."
            //                          + "  Number of times is : {0}.", i);
            //        Console.WriteLine("Now wait for 120 seconds.");
            //        System.Threading.Thread.Sleep(120000);
            //    }
            //}
        }
        public ControllerEventLogRepository()
        {
            //_db.Database.CommandTimeout = 180;
            //_db.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            //_db.Configuration.AutoDetectChangesEnabled = false;
        }

        public int GetRecordCountByParameterAndEvent(string signalId, DateTime startTime, DateTime endTime,
            List<int> eventParameters,
            List<int> eventCodes)
        {
            var query = _db.ControllerEventLogs.Where(c =>
                c.SignalId == signalId && c.Timestamp >= startTime && c.Timestamp <= endTime);
            if (eventParameters != null && eventParameters.Count > 0)
                query = query.Where(c => eventParameters.Contains(c.EventParam));
            if (eventCodes != null && eventCodes.Count > 0)
                query = query.Where(c => eventCodes.Contains(c.EventCode));
            return query.Count();
        }

        public List<ControllerEventLog> GetRecordsByParameterAndEvent(string signalId, DateTime startTime,
            DateTime endTime, List<int> eventParameters, List<int> eventCodes)
        {
            var query = _db.ControllerEventLogs.Where(c =>
                c.SignalId == signalId && c.Timestamp >= startTime && c.Timestamp <= endTime);
            if (eventParameters != null && eventParameters.Count > 0)
                query = query.Where(c => eventParameters.Contains(c.EventParam));
            if (eventCodes != null && eventCodes.Count > 0)
                query = query.Where(c => eventCodes.Contains(c.EventCode));
            return query.ToList();
        }

        public List<ControllerEventLog> GetAllAggregationCodes(string signalId, DateTime startTime, DateTime endTime)
        {
            var codes = new List<int> { 150, 114, 113, 112, 105, 102, 1 };
            var records = _db.ControllerEventLogs
                .Where(c => c.SignalId == signalId && c.Timestamp >= startTime && c.Timestamp <= endTime &&
                            codes.Contains(c.EventCode))
                .ToList();
            return records;
        }

        public int GetDetectorActivationCount(string signalId,
            DateTime startTime, DateTime endTime, int detectorChannel)
        {
            var count = (from cel in _db.ControllerEventLogs
                         where cel.Timestamp >= startTime
                               && cel.Timestamp < endTime
                               && cel.SignalId == signalId
                               && cel.EventParam == detectorChannel
                               && cel.EventCode == 82
                         select cel).Count();
            return count;
        }

        public double GetTmcVolume(DateTime startDate, DateTime endDate, string signalId, int phase)
        {
            throw new NotImplementedException();
            //var repository = new SignalsRepository();
            //var signal = repository.GetVersionOfSignalByDate(signalId, startDate);
            //var graphDetectors = signal.GetDetectorsForSignalByPhaseNumber(phase);

            //var tmcChannels = new List<int>();
            //foreach (var gd in graphDetectors)
            //    foreach (var dt in gd.DetectionTypes)
            //        if (dt.DetectionTypeID == 4)
            //            tmcChannels.Add(gd.DetChannel);


            //double count = (from cel in _db.ControllerEventLogs
            //                where cel.Timestamp >= startDate
            //                      && cel.Timestamp < endDate
            //                      && cel.SignalId == signalId
            //                      && tmcChannels.Contains(cel.EventParam)
            //                      && cel.EventCode == 82
            //                select cel).Count();

            //return count;
        }

        public List<ControllerEventLog> GetSplitEvents(string signalId, DateTime startTime, DateTime endTime)
        {
            var results = (from r in _db.ControllerEventLogs
                           where r.SignalId == signalId && r.Timestamp > startTime && r.Timestamp < endTime
                                 && r.EventCode > 130 && r.EventCode < 150
                           select r).ToList();

            return results;
        }

        public List<ControllerEventLog> GetSignalEventsBetweenDates(string signalId,
            DateTime startTime, DateTime endTime)
        {
            try
            {
                return (from r in _db.ControllerEventLogs
                        where r.SignalId == signalId
                              && r.Timestamp >= startTime
                              && r.Timestamp < endTime
                        select r).ToList();
            }
            catch (Exception ex)
            {
                var logRepository =
                    new ApplicationEventRepository(_db);
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetSignalEventsBetweenDates";
                e.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }

        public List<ControllerEventLog> GetTopNumberOfSignalEventsBetweenDates(string signalId, int numberOfRecords,
            DateTime startTime, DateTime endTime)
        {
            try
            {
                var events =
                (from r in _db.ControllerEventLogs
                 where r.SignalId == signalId
                       && r.Timestamp >= startTime
                       && r.Timestamp < endTime
                 select r).Take(numberOfRecords).ToList();

                if (events != null)
                    return events;
                var emptyEvents = new List<ControllerEventLog>();
                return emptyEvents;
            }
            catch (Exception ex)
            {
                var logRepository =
                    new ApplicationEventRepository(_db);
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetTopNumberOfSignalEventsBetweenDates";
                e.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }

        public int GetRecordCount(string signalId, DateTime startTime, DateTime endTime)
        {
            try
            {
                return _db.ControllerEventLogs.Count(r => r.SignalId == signalId
                                                           && r.Timestamp >= startTime
                                                           && r.Timestamp < endTime);
            }
            catch (Exception ex)
            {
                var logRepository =
                    new ApplicationEventRepository(_db);
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetRecordCount";
                e.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = signalId + " - " + ex.Message;
                logRepository.Add(e);
                throw ex;
            }
        }

        public bool CheckForRecords(string signalId, DateTime startTime, DateTime endTime)
        {
            try
            {
                return _db.ControllerEventLogs.Any(r => r.SignalId == signalId
                                                         && r.Timestamp >= startTime
                                                         && r.Timestamp < endTime);
            }
            catch (Exception ex)
            {
                var logRepository = new ApplicationEventRepository(_db);
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "CheckForRecords";
                e.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }


        public List<ControllerEventLog> GetSignalEventsByEventCode(string signalId,
            DateTime startTime, DateTime endTime, int eventCode)
        {
            try
            {
                return (from r in _db.ControllerEventLogs
                        where r.SignalId == signalId
                              && r.Timestamp >= startTime
                              && r.Timestamp < endTime
                              && r.EventCode == eventCode
                        select r).ToList();
            }
            catch (Exception ex)
            {
                var logRepository = new ApplicationEventRepository(_db);
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetSignalEventsByEventCode";
                e.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }

        public List<ControllerEventLog> GetSignalEventsByEventCodes(string signalId,
            DateTime startTime, DateTime endTime, List<int> eventCodes)
        {
            try
            {
                var events = (from s in _db.ControllerEventLogs
                              where s.SignalId == signalId &&
                                    s.Timestamp >= startTime &&
                                    s.Timestamp <= endTime &&
                                    eventCodes.Contains(s.EventCode)
                              select s).ToList();
                events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));
                return events;
            }
            //catch (EntityCommandExecutionException ex)
            //{

            //}

            catch (Exception ex)
            {
                var logRepository = new ApplicationEventRepository(_db);
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetSignalEventsByEventCodes";
                e.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = signalId + " - " + ex.Message;
                logRepository.Add(e);
                throw ex;
            }
        }

        public List<ControllerEventLog> GetEventsByEventCodesParam(string signalId, DateTime startTime,
            DateTime endTime, List<int> eventCodes, int param)
        {
            try
            {
                var events = _db.ControllerEventLogs.Where(s => s.SignalId == signalId &&
                                   s.Timestamp >= startTime &&
                                   s.Timestamp <= endTime &&
                                   s.EventParam == param &&
                                   eventCodes.Contains(s.EventCode)).ToList();
                events = events.OrderBy(e => e.Timestamp).ThenBy(e => e.EventParam).ToList();
                return events;
            }
            catch (Exception ex)
            {
                var logRepository = new ApplicationEventRepository(_db);
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetEventsByEventCodesParam";
                e.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }

        public List<ControllerEventLog> GetTopEventsAfterDateByEventCodesParam(string signalId,
            DateTime timestamp, List<int> eventCodes, int param, int top)
        {
            var settings = _db.ApplicationSettings.FirstOrDefault();
            var secondsToCompleteCycle = 900;
            if (settings != null)
                secondsToCompleteCycle = Convert.ToInt32(settings.CycleCompletionSeconds);
            try
            {
                var endDate = timestamp.AddSeconds(secondsToCompleteCycle);
                var events = _db.ControllerEventLogs.Where(c =>
                    c.SignalId == signalId &&
                    c.Timestamp > timestamp &&
                    c.Timestamp < endDate &&
                    c.EventParam == param &&
                    eventCodes.Contains(c.EventCode)).ToList();
                return events
                    .OrderBy(s => s.Timestamp)
                    .Take(top).ToList();
            }
            catch (Exception e)
            {
                var errorLog = new ApplicationEventRepository(_db);
                errorLog.QuickAdd(Assembly.GetExecutingAssembly().FullName,
                    GetType().Name, e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.Low, e.Message);
                return null;
            }
        }

        public List<ControllerEventLog> GetTopEventsBeforeDateByEventCodesParam(string signalId,
            DateTime timestamp, List<int> eventCodes, int param, int top)
        {
            var settings = _db.ApplicationSettings.FirstOrDefault();
            var secondsToCompleteCycle = 900;
            if (settings != null)
                secondsToCompleteCycle = Convert.ToInt32(settings.CycleCompletionSeconds);
            try
            {
                var start = timestamp.AddSeconds(secondsToCompleteCycle * -1);
                var events = _db.ControllerEventLogs.Where(c =>
                    c.SignalId == signalId &&
                    c.Timestamp < timestamp &&
                    c.Timestamp > start &&
                    c.EventParam == param &&
                    eventCodes.Contains(c.EventCode)).ToList();
                return events
                    .OrderByDescending(s => s.Timestamp)
                    .Take(top).ToList();
            }
            catch (Exception e)
            {
                var errorLog = new ApplicationEventRepository(_db);
                errorLog.QuickAdd(Assembly.GetExecutingAssembly().FullName,
                    GetType().Name, e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.Low, e.Message);
                return null;
            }
        }


        public int GetEventCountByEventCodesParamDateTimeRange(string signalId,
            DateTime startTime, DateTime endTime, int startHour, int startMinute, int endHour, int endMinute,
            List<int> eventCodes, int param)
        {
            try
            {
                return
                (from s in _db.ControllerEventLogs
                 where s.SignalId == signalId &&
                       s.Timestamp >= startTime &&
                       s.Timestamp <= endTime &&
                       (s.Timestamp.Hour > startHour && s.Timestamp.Hour < endHour ||
                        s.Timestamp.Hour == startHour && s.Timestamp.Hour == endHour &&
                        s.Timestamp.Minute >= startMinute && s.Timestamp.Minute <= endMinute ||
                        s.Timestamp.Hour == startHour && s.Timestamp.Hour < endHour &&
                        s.Timestamp.Minute >= startMinute ||
                        s.Timestamp.Hour < startHour && s.Timestamp.Hour == endHour &&
                        s.Timestamp.Minute <= endMinute)
                       &&
                       s.EventParam == param &&
                       eventCodes.Contains(s.EventCode)
                 select s).Count();
            }
            catch (Exception ex)
            {

                var logRepository = new ApplicationEventRepository(_db);
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetEventCountByEventCodesParamDateTimeRange";
                e.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }


        public List<ControllerEventLog> GetEventsByEventCodesParamDateTimeRange(string signalId,
            DateTime startTime, DateTime endTime, int startHour, int startMinute, int endHour, int endMinute,
            List<int> eventCodes, int param)
        {
            try
            {
                var events = (from s in _db.ControllerEventLogs
                              where s.SignalId == signalId &&
                                    s.Timestamp >= startTime &&
                                    s.Timestamp <= endTime &&
                                    (s.Timestamp.Hour > startHour && s.Timestamp.Hour < endHour ||
                                     s.Timestamp.Hour == startHour && s.Timestamp.Hour == endHour &&
                                     s.Timestamp.Minute >= startMinute && s.Timestamp.Minute <= endMinute ||
                                     s.Timestamp.Hour == startHour && s.Timestamp.Hour < endHour &&
                                     s.Timestamp.Minute >= startMinute ||
                                     s.Timestamp.Hour < startHour && s.Timestamp.Hour == endHour &&
                                     s.Timestamp.Minute <= endMinute)
                                    &&
                                    s.EventParam == param &&
                                    eventCodes.Contains(s.EventCode)
                              select s).ToList();
                events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));
                return events;
            }
            catch (Exception ex)
            {

                var logRepository = new ApplicationEventRepository(_db);
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetSignalEventsByEventCodesParamDateTimeRange";
                e.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }


        public List<ControllerEventLog> GetEventsByEventCodesParamWithOffsetAndLatencyCorrection(string signalId,
            DateTime startTime, DateTime endTime, List<int> eventCodes, int param, double offset,
            double latencyCorrection)
        {

            try
            {
                var events = (from s in _db.ControllerEventLogs
                              where s.SignalId == signalId &&
                                    s.Timestamp >= startTime &&
                                    s.Timestamp <= endTime &&
                                    s.EventParam == param &&
                                    eventCodes.Contains(s.EventCode)
                              select s).ToList();
                events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));
                foreach (var cel in events)
                {
                    cel.Timestamp = cel.Timestamp.AddMilliseconds(offset);
                    cel.Timestamp = cel.Timestamp.AddSeconds(0 - latencyCorrection);
                }
                return events;
            }
            catch (Exception ex)
            {
                var logRepository = new ApplicationEventRepository(_db);
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetEventsByEventCodesParamWithOffsetAndLatencyCorrection";
                e.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }

        public List<ControllerEventLog> GetEventsByEventCodesParamWithLatencyCorrection(string signalId,
            DateTime startTime, DateTime endTime, List<int> eventCodes, int param, double latencyCorrection)
        {
            try
            {
                var events = _db.ControllerEventLogs.Where(s => s.SignalId == signalId &&
                          s.Timestamp >= startTime &&
                          s.Timestamp <= endTime &&
                          s.EventParam == param &&
                          eventCodes.Contains(s.EventCode)).ToList();
                foreach (var cel in events)
                {
                    cel.Timestamp = cel.Timestamp.AddSeconds(0 - latencyCorrection);
                }
                return events.OrderBy(e => e.Timestamp).ThenBy(e => e.EventCode).ToList();
            }
            catch (Exception ex)
            {

                var logRepository = new ApplicationEventRepository(_db);
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetEventsByEventCodesParamWithLatencyCorrection";
                e.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }

        public ControllerEventLog GetFirstEventBeforeDate(string signalId,
            int eventCode, DateTime date)
        {
            try
            {
                var tempDate = date.AddHours(-1);
                var lastEvent = _db.ControllerEventLogs.Where(c => c.SignalId == signalId &&
                                                                    c.Timestamp >= tempDate &&
                                                                    c.Timestamp < date &&
                                                                    c.EventCode == eventCode)
                    .OrderByDescending(c => c.Timestamp).FirstOrDefault();
                return lastEvent;
            }
            catch (Exception ex)
            {
                var logRepository = new ApplicationEventRepository(_db);
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetEventsByEventCodesParamWithOffsetAndLatencyCorrection";
                e.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                e.Description = ex.Message;
                e.Timestamp = DateTime.Now;
                logRepository.Add(e);
                return null;
            }
        }

        public ControllerEventLog GetFirstEventAfterDateByEventCodesAndParameter(string signalId, List<int> eventCodes,
            int eventParam, DateTime start, int secondsToSearch)
        {

            if (!string.IsNullOrEmpty(signalId))
            {
                try
                {
                    //TODO
                    //_db.Database.CommandTimeout = 10;
                    var tempDate = start.AddSeconds(secondsToSearch);
                    var controllerEvent = _db.ControllerEventLogs.Where(c => c.SignalId == signalId &&
                                                                        c.Timestamp > start &&
                                                                        c.Timestamp <= tempDate &&
                                                                        c.EventParam == eventParam &&
                                                                        eventCodes.Contains(c.EventCode))
                        .OrderBy(c => c.Timestamp).FirstOrDefault();
                    return controllerEvent;
                }

                catch (Exception ex)
                {
                    var logRepository = new ApplicationEventRepository(_db);
                    var e = new ApplicationEvent();
                    e.ApplicationName = "MOE.Common";
                    e.Class = GetType().ToString();
                    e.Function = "GetEventsByEventCodesParamWithOffsetAndLatencyCorrection";
                    e.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                    e.Description = ex.Message;
                    e.Timestamp = DateTime.Now;
                    logRepository.Add(e);
                    return null;
                }
            }

            return null;
        }

        public ControllerEventLog GetFirstEventBeforeDateByEventCodeAndParameter(string signalId, int eventCode,
            int eventParam, DateTime date)
        {
            if (!string.IsNullOrEmpty(signalId))
            {
                try
                {
                    //_db.Database.CommandTimeout = 10;
                    var tempDate = date.AddDays(-1);
                    var lastEvent = _db.ControllerEventLogs.Where(c => c.SignalId == signalId &&
                                                                        c.Timestamp >= tempDate &&
                                                                        c.Timestamp < date &&
                                                                        c.EventCode == eventCode &&
                                                                        c.EventParam == eventParam)
                        .OrderByDescending(c => c.Timestamp).FirstOrDefault();
                    return lastEvent;
                }

                catch (Exception ex)
                {
                    var logRepository = new ApplicationEventRepository(_db);
                    var e = new ApplicationEvent();
                    e.ApplicationName = "MOE.Common";
                    e.Class = GetType().ToString();
                    e.Function = "GetEventsByEventCodesParamWithOffsetAndLatencyCorrection";
                    e.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                    e.Description = ex.Message;
                    e.Timestamp = DateTime.Now;
                    logRepository.Add(e);
                    return null;
                }
            }

            return null;
        }

        public int GetSignalEventsCountBetweenDates(string signalId, DateTime startTime, DateTime endTime)
        {
            return _db.ControllerEventLogs.Count(r => r.SignalId == signalId &&
                                                r.Timestamp >= startTime
                                                && r.Timestamp < endTime);
        }

        public List<ControllerEventLog> GetEventsBetweenDates(DateTime startTime, DateTime endTime)
        {
            return _db.ControllerEventLogs.Where(r => r.Timestamp >= startTime
                                                       && r.Timestamp < endTime).ToList();
        }

        public int GetApproachEventsCountBetweenDates(int approachId, DateTime startTime, DateTime endTime,
            int phaseNumber, IApproachRepository approachRepository)
        {
            var approachCodes = new List<int> { 1, 8, 10 };
            Approach approach = approachRepository.GetApproachByApproachID(approachId);

            var results = _db.ControllerEventLogs.Where(r =>
                r.SignalId == approach.SignalId && r.Timestamp > startTime && r.Timestamp < endTime
                && approachCodes.Contains(r.EventCode) && r.EventParam == phaseNumber);

            return results.Count();
        }

        public DateTime GetMostRecentRecordTimestamp(string signalID)
        {
            ControllerEventLog row = (from r in _db.ControllerEventLogs
                                                          where r.SignalId == signalID
                                                          orderby r.Timestamp descending
                                                          select r).Take(1).FirstOrDefault();
            if (row != null)
            {
                return row.Timestamp;
            }
            else
            {
                return new DateTime();
            }
        }
    }
}