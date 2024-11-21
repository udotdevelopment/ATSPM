using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Internal;
using MOE.Common.Business.Parquet;

namespace MOE.Common.Models.Repositories
{
    public class ControllerEventLogRepository : IControllerEventLogRepository
    {

        private readonly SPM _db = new SPM();
        private const string LocalArchiveDirectory = "LocalArchiveDirectory";
        private readonly string _localPath = ParquetArchive.GetSetting(LocalArchiveDirectory);

        public ControllerEventLogRepository(SPM db)
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
            var query = _db.Controller_Event_Log.Where(c =>
                c.SignalID == signalId && c.Timestamp >= startTime && c.Timestamp <= endTime);
            if (eventParameters != null && eventParameters.Count > 0)
                query = query.Where(c => eventParameters.Contains(c.EventParam));
            if (eventCodes != null && eventCodes.Count > 0)
                query = query.Where(c => eventCodes.Contains(c.EventCode));

            if (query.Any())
                return query.Count();

            //Check the archive if no data in DB
            var archivedData = ParquetArchive.GetDataFromArchive(_localPath, signalId, startTime, endTime);

            if (eventParameters != null && eventParameters.Count > 0)
                archivedData = archivedData.Where(c => eventParameters.Contains(c.EventParam)).ToList();
            if (eventCodes != null && eventCodes.Count > 0)
                archivedData = archivedData.Where(c => eventCodes.Contains(c.EventCode)).ToList();

            return archivedData.Count;
        }

        public List<Controller_Event_Log> GetRecordsByParameterAndEvent(string signalId, DateTime startTime,
            DateTime endTime, List<int> eventParameters, List<int> eventCodes)
        {
            var query = _db.Controller_Event_Log.Where(c =>
                c.SignalID == signalId && c.Timestamp >= startTime && c.Timestamp <= endTime);
            if (eventParameters != null && eventParameters.Count > 0)
                query = query.Where(c => eventParameters.Contains(c.EventParam));
            if (eventCodes != null && eventCodes.Count > 0)
                query = query.Where(c => eventCodes.Contains(c.EventCode));

            if (query.Any())
                return query.ToList();

            var archivedData = ParquetArchive.GetDataFromArchive(_localPath, signalId, startTime, endTime);

            if (eventParameters != null && eventParameters.Count > 0)
                archivedData = archivedData.Where(c => eventParameters.Contains(c.EventParam)).ToList();
            if (eventCodes != null && eventCodes.Count > 0)
                archivedData = archivedData.Where(c => eventCodes.Contains(c.EventCode)).ToList();

            return archivedData.ToList();
        }

        public List<Controller_Event_Log> GetAllAggregationCodes(string signalId, DateTime startTime, DateTime endTime)
        {
            var codes = new List<int> { 150, 114, 113, 112, 105, 102, 1 };
            var records = _db.Controller_Event_Log
                .Where(c => c.SignalID == signalId && c.Timestamp >= startTime && c.Timestamp <= endTime &&
                            codes.Contains(c.EventCode))
                .ToList();

            if (records.Any()) return records;

            var archivedData = ParquetArchive.GetDataFromArchive(_localPath, signalId, startTime, endTime);
            records = archivedData.Where(x => codes.Contains(x.EventCode)).ToList();

            return records;
        }

        public int GetDetectorActivationCount(string signalId,
            DateTime startTime, DateTime endTime, int detectorChannel)
        {
            var count = (from cel in _db.Controller_Event_Log
                         where cel.Timestamp >= startTime
                               && cel.Timestamp < endTime
                               && cel.SignalID == signalId
                               && cel.EventParam == detectorChannel
                               && cel.EventCode == 82
                         select cel).Count();

            if (count > 0) return count;

            var archivedData = ParquetArchive.GetDataFromArchive(_localPath, signalId, startTime, endTime);
            count = archivedData.Count(x => x.EventParam == detectorChannel && x.EventCode == 82);
            return count;
        }

        public double GetTmcVolume(DateTime startDate, DateTime endDate, string signalId, int phase)
        {
            var repository =
                SignalsRepositoryFactory.Create();
            var signal = repository.GetVersionOfSignalByDate(signalId, startDate);
            var graphDetectors = signal.GetDetectorsForSignalByPhaseNumber(phase);

            var tmcChannels = new List<int>();
            foreach (var gd in graphDetectors)
                foreach (var dt in gd.DetectionTypes)
                    if (dt.DetectionTypeID == 4)
                        tmcChannels.Add(gd.DetChannel);


            double count = (from cel in _db.Controller_Event_Log
                            where cel.Timestamp >= startDate
                                  && cel.Timestamp < endDate
                                  && cel.SignalID == signalId
                                  && tmcChannels.Contains(cel.EventParam)
                                  && cel.EventCode == 82
                            select cel).Count();

            if (count > 0) return count;

            var archivedData = ParquetArchive.GetDataFromArchive(_localPath, signalId, startDate, endDate);
            count = archivedData.Count(x => x.EventCode == 82 && tmcChannels.Contains(x.EventParam));

            return count;
        }

        public List<Controller_Event_Log> GetSplitEvents(string signalId, DateTime startTime, DateTime endTime)
        {
            var results = (from r in _db.Controller_Event_Log
                           where r.SignalID == signalId && r.Timestamp > startTime && r.Timestamp < endTime
                                 && r.EventCode > 130 && r.EventCode < 150
                           select r).ToList();

            if (results.Any()) return results;

            var archivedData = ParquetArchive.GetDataFromArchive(_localPath, signalId, startTime, endTime);
            results = archivedData.Where(x => x.EventCode > 130 && x.EventCode < 150).ToList();

            return results;
        }

        public List<Controller_Event_Log> GetSignalEventsBetweenDates(string signalId,
            DateTime startTime, DateTime endTime)
        {
            try
            {
                var events = (from r in _db.Controller_Event_Log
                              where r.SignalID == signalId
                                    && r.Timestamp >= startTime
                                    && r.Timestamp < endTime
                              select r).ToList();
                if (events.Any()) return events;

                return ParquetArchive.GetDataFromArchive(_localPath, signalId, startTime, endTime);
            }
            catch (Exception ex)
            {
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetSignalEventsBetweenDates";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }

        public List<Controller_Event_Log> GetTopNumberOfSignalEventsBetweenDates(string signalId, int numberOfRecords,
            DateTime startTime, DateTime endTime)
        {
            try
            {
                var events =
                (from r in _db.Controller_Event_Log
                 where r.SignalID == signalId
                       && r.Timestamp >= startTime
                       && r.Timestamp < endTime
                 select r).Take(numberOfRecords).ToList();

                if (events.Any())
                    return events;

                var archivedData = ParquetArchive.GetDataFromArchive(_localPath, signalId, startTime, endTime);
                events = archivedData.Take(numberOfRecords).ToList();

                return events;
            }
            catch (Exception ex)
            {
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetTopNumberOfSignalEventsBetweenDates";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
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
                var count = _db.Controller_Event_Log.Count(r => r.SignalID == signalId
                                                           && r.Timestamp >= startTime
                                                           && r.Timestamp < endTime);

                if (count > 0)
                    return count;

                var archivedData = ParquetArchive.GetDataFromArchive(_localPath, signalId, startTime, endTime);
                return archivedData.Count;
            }
            catch (Exception ex)
            {
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetRecordCount";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
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
                var hasRecords = _db.Controller_Event_Log.Any(r => r.SignalID == signalId
                                                         && r.Timestamp >= startTime
                                                         && r.Timestamp < endTime);
                if (hasRecords)
                    return hasRecords;

                var archivedData = ParquetArchive.GetDataFromArchive(_localPath, signalId, startTime, endTime);
                return archivedData.Any();
            }
            catch (Exception ex)
            {
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "CheckForRecords";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }


        public List<Controller_Event_Log> GetSignalEventsByEventCode(string signalId,
            DateTime startTime, DateTime endTime, int eventCode)
        {
            try
            {
                var events = (from r in _db.Controller_Event_Log
                              where r.SignalID == signalId
                                    && r.Timestamp >= startTime
                                    && r.Timestamp < endTime
                                    && r.EventCode == eventCode
                              select r).ToList();

                if (!events.Any())
                {
                    var logs = ParquetArchive.GetDataFromArchive(_localPath, signalId, startTime, endTime);
                    events = (from s in logs
                              where s.SignalID == signalId &&
                                    s.Timestamp >= startTime &&
                                    s.Timestamp < endTime &&
                                    s.EventCode == eventCode
                              select s).ToList();
                }

                return events;
            }
            catch (Exception ex)
            {
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetSignalEventsByEventCode";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }

        public List<Controller_Event_Log> GetSignalEventsByEventCodes(string signalId,
            DateTime startTime, DateTime endTime, List<int> eventCodes)
        {
            try
            {
                var events = (from s in _db.Controller_Event_Log
                              where s.SignalID == signalId &&
                                    s.Timestamp >= startTime &&
                                    s.Timestamp <= endTime &&
                                    eventCodes.Contains(s.EventCode)
                              select s).ToList();

                if (!events.Any())
                {

                    var logs = ParquetArchive.GetDataFromArchive(_localPath, signalId, startTime, endTime);
                    events = (from s in logs
                              where s.SignalID == signalId &&
                                    s.Timestamp >= startTime &&
                                    s.Timestamp <= endTime &&
                                    eventCodes.Contains(s.EventCode)
                              select s).ToList();
                }

                events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));
                return events;
            }
            catch (Exception ex)
            {
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetSignalEventsByEventCodes";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = signalId + " - " + ex.Message;
                logRepository.Add(e);
                throw ex;
            }
        }

        public List<Controller_Event_Log> GetEventsByEventCodesParam(string signalId, DateTime startTime,
            DateTime endTime, List<int> eventCodes, int param)
        {
            try
            {
                var events = _db.Controller_Event_Log.Where(s => s.SignalID == signalId &&
                                   s.Timestamp >= startTime &&
                                   s.Timestamp <= endTime &&
                                   s.EventParam == param &&
                                   eventCodes.Contains(s.EventCode)).ToList();

                if (!events.Any())
                {
                    var logs = ParquetArchive.GetDataFromArchive(_localPath, signalId, startTime, endTime);
                    events = logs.Where(x => x.EventParam == param && eventCodes.Contains(x.EventCode)).ToList();
                }

                events = events.OrderBy(e => e.Timestamp).ThenBy(e => e.EventParam).ToList();
                return events;
            }
            catch (Exception ex)
            {
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetEventsByEventCodesParam";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }

        public List<Controller_Event_Log> GetTopEventsAfterDateByEventCodesParam(string signalId,
            DateTime timestamp, List<int> eventCodes, int param, int top)
        {
            var settings = _db.GeneralSettings.FirstOrDefault();
            var secondsToCompleteCycle = 900;
            if (settings != null)
                secondsToCompleteCycle = Convert.ToInt32(settings.CycleCompletionSeconds);
            try
            {
                var endDate = timestamp.AddSeconds(secondsToCompleteCycle);
                var events = _db.Controller_Event_Log.Where(c =>
                    c.SignalID == signalId &&
                    c.Timestamp > timestamp &&
                    c.Timestamp < endDate &&
                    c.EventParam == param &&
                    eventCodes.Contains(c.EventCode)).OrderBy(s => s.Timestamp).Take(top).ToList();

                if (events.Any())
                    return events;

                var archivedData = ParquetArchive.GetDataFromArchive(_localPath, signalId, timestamp, endDate);
                events = archivedData.Where(x => x.EventParam == param && eventCodes.Contains(x.EventCode))
                    .OrderBy(x => x.Timestamp).Take(top).ToList();

                return events;
            }
            catch (Exception e)
            {
                var errorLog = ApplicationEventRepositoryFactory.Create();
                errorLog.QuickAdd(Assembly.GetExecutingAssembly().FullName,
                    GetType().Name, e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.Low, e.Message);
                return null;
            }
        }

        public List<Controller_Event_Log> GetTopEventsBeforeDateByEventCodesParam(string signalId,
            DateTime timestamp, List<int> eventCodes, int param, int top)
        {
            var settings = _db.GeneralSettings.FirstOrDefault();
            var secondsToCompleteCycle = 900;
            if (settings != null)
                secondsToCompleteCycle = Convert.ToInt32(settings.CycleCompletionSeconds);
            try
            {
                var start = timestamp.AddSeconds(secondsToCompleteCycle *-1);
                var events = _db.Controller_Event_Log.Where(c =>
                    c.SignalID == signalId &&
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
                var errorLog = ApplicationEventRepositoryFactory.Create();
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
                var count =
                (from s in _db.Controller_Event_Log
                 where s.SignalID == signalId &&
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

                if (count <= 0)
                {
                    var archivedData = ParquetArchive.GetDataFromArchive(_localPath, signalId, startTime, endTime);
                    count = (from s in archivedData
                             where s.SignalID == signalId &&
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

                return count;
            }
            catch (Exception ex)
            {
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetEventCountByEventCodesParamDateTimeRange";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }


        public List<Controller_Event_Log> GetEventsByEventCodesParamDateTimeRange(string signalId,
            DateTime startTime, DateTime endTime, int startHour, int startMinute, int endHour, int endMinute,
            List<int> eventCodes, int param)
        {
            try
            {
                var events = (from s in _db.Controller_Event_Log
                              where s.SignalID == signalId &&
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

                if (!events.Any())
                {
                    var archivedData = ParquetArchive.GetDataFromArchive(_localPath, signalId, startTime, endTime);
                    events = (from s in archivedData
                              where s.SignalID == signalId &&
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
                }

                events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));
                return events;
            }
            catch (Exception ex)
            {
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetSignalEventsByEventCodesParamDateTimeRange";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }


        public List<Controller_Event_Log> GetEventsByEventCodesParamWithOffsetAndLatencyCorrection(string signalId,
            DateTime startTime, DateTime endTime, List<int> eventCodes, int param, double offset,
            double latencyCorrection)
        {

            try
            {
                var events = (from s in _db.Controller_Event_Log
                              where s.SignalID == signalId &&
                                    s.Timestamp >= startTime &&
                                    s.Timestamp <= endTime &&
                                    s.EventParam == param &&
                                    eventCodes.Contains(s.EventCode)
                              select s).ToList();

                if (!events.Any())
                {
                    var archivedData = ParquetArchive.GetDataFromArchive(_localPath, signalId, startTime, endTime);
                    events = (from s in archivedData
                              where s.SignalID == signalId &&
                                    s.Timestamp >= startTime &&
                                    s.Timestamp <= endTime &&
                                    s.EventParam == param &&
                                    eventCodes.Contains(s.EventCode)
                              select s).ToList();
                }

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
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetEventsByEventCodesParamWithOffsetAndLatencyCorrection";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }

        public List<Controller_Event_Log> GetEventsByEventCodesParamWithLatencyCorrection(string signalId,
            DateTime startTime, DateTime endTime, List<int> eventCodes, int param, double latencyCorrection)
        {
            try
            {
                var events = _db.Controller_Event_Log.Where(s => s.SignalID == signalId &&
                          s.Timestamp >= startTime &&
                          s.Timestamp <= endTime &&
                          s.EventParam == param &&
                          eventCodes.Contains(s.EventCode)).ToList();

                if (!events.Any())
                {
                    var archivedData = ParquetArchive.GetDataFromArchive(_localPath, signalId, startTime, endTime);
                    events = archivedData.Where(s => s.SignalID == signalId &&
                                                     s.Timestamp >= startTime &&
                                                     s.Timestamp <= endTime &&
                                                     s.EventParam == param &&
                                                     eventCodes.Contains(s.EventCode)).ToList();
                }

                foreach (var cel in events)
                {
                    cel.Timestamp = cel.Timestamp.AddSeconds(0 - latencyCorrection);
                }
                return events.OrderBy(e => e.Timestamp).ThenBy(e => e.EventCode).ToList();
            }
            catch (Exception ex)
            {
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetEventsByEventCodesParamWithLatencyCorrection";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }

        public Controller_Event_Log GetFirstEventBeforeDate(string signalId,
            int eventCode, DateTime date)
        {
            try
            {
                var tempDate = date.AddHours(-1);
                var lastEvent = _db.Controller_Event_Log.Where(c => c.SignalID == signalId &&
                                                                    c.Timestamp >= tempDate &&
                                                                    c.Timestamp < date &&
                                                                    c.EventCode == eventCode)
                    .OrderByDescending(c => c.Timestamp).FirstOrDefault();

                if (lastEvent == null)
                {
                    var archivedData = ParquetArchive.GetDataFromArchive(_localPath, signalId, tempDate, date);
                    lastEvent = archivedData.Where(c => c.SignalID == signalId &&
                                                        c.Timestamp >= tempDate &&
                                                        c.Timestamp < date &&
                                                        c.EventCode == eventCode)
                        .OrderByDescending(c => c.Timestamp).FirstOrDefault();
                }

                return lastEvent;
            }
            catch (Exception ex)
            {
                var logRepository = ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                e.Class = GetType().ToString();
                e.Function = "GetEventsByEventCodesParamWithOffsetAndLatencyCorrection";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Description = ex.Message;
                e.Timestamp = DateTime.Now;
                logRepository.Add(e);
                return null;
            }
        }

        public Controller_Event_Log GetFirstEventAfterDateByEventCodesAndParameter(string signalId, List<int> eventCodes,
            int eventParam, DateTime start, int secondsToSearch)
        {
            
            if (!String.IsNullOrEmpty(signalId))
            {
                try
                {
                    _db.Database.CommandTimeout = 10;
                    var tempDate = start.AddSeconds(secondsToSearch);
                    var controllerEvent = _db.Controller_Event_Log.Where(c => c.SignalID == signalId &&
                                                                        c.Timestamp > start &&
                                                                        c.Timestamp <= tempDate && 
                                                                        c.EventParam == eventParam&&
                                                                        eventCodes.Contains(c.EventCode)  )
                        .OrderBy(c => c.Timestamp).FirstOrDefault();
                    return controllerEvent;
                }

                catch (Exception ex)
                {
                    var logRepository = ApplicationEventRepositoryFactory.Create();
                    var e = new ApplicationEvent();
                    e.ApplicationName = "MOE.Common";
                    e.Class = GetType().ToString();
                    e.Function = "GetEventsByEventCodesParamWithOffsetAndLatencyCorrection";
                    e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                    e.Description = ex.Message;
                    e.Timestamp = DateTime.Now;
                    logRepository.Add(e);
                    return null;
                }
            }

            return null;
        }

        public Controller_Event_Log GetFirstEventBeforeDateByEventCodeAndParameter(string signalId, int eventCode,
            int eventParam, DateTime date)
        {
            if (!String.IsNullOrEmpty(signalId))
            {
                try
                {
                    _db.Database.CommandTimeout = 10;
                    var tempDate = date.AddDays(-1);
                    var lastEvent = _db.Controller_Event_Log.Where(c => c.SignalID == signalId &&
                                                                        c.Timestamp >= tempDate &&
                                                                        c.Timestamp < date &&
                                                                        c.EventCode == eventCode &&
                                                                        c.EventParam == eventParam)
                        .OrderByDescending(c => c.Timestamp).FirstOrDefault();

                    if (lastEvent == null)
                    {
                        var archivedData = ParquetArchive.GetDataFromArchive(_localPath, signalId, tempDate, date);
                        lastEvent = archivedData.Where(c => c.SignalID == signalId &&
                                                                        c.Timestamp >= tempDate &&
                                                                        c.Timestamp < date &&
                                                                        c.EventCode == eventCode &&
                                                                        c.EventParam == eventParam)
                        .OrderByDescending(c => c.Timestamp).FirstOrDefault();
                    }

                    return lastEvent;
                }

                catch (Exception ex)
                {
                    var logRepository = ApplicationEventRepositoryFactory.Create();
                    var e = new ApplicationEvent();
                    e.ApplicationName = "MOE.Common";
                    e.Class = GetType().ToString();
                    e.Function = "GetEventsByEventCodesParamWithOffsetAndLatencyCorrection";
                    e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
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
            var count = _db.Controller_Event_Log.Count(r => r.SignalID == signalId &&
                                                r.Timestamp >= startTime
                                                && r.Timestamp < endTime);

            if (count <= 0)
            {
                var archivedData = ParquetArchive.GetDataFromArchive(_localPath, signalId, startTime, endTime);
                count = archivedData.Count(r =>
                    r.SignalID == signalId && r.Timestamp >= startTime && r.Timestamp < endTime);
            }

            return count;
        }

        //Not used and not supported with Parquet
        public List<Controller_Event_Log> GetEventsBetweenDates(DateTime startTime, DateTime endTime)
        {
            var events = _db.Controller_Event_Log.Where(r => r.Timestamp >= startTime
                                                       && r.Timestamp < endTime).ToList();
            
            return events;
        }

        public int GetApproachEventsCountBetweenDates(int approachId, DateTime startTime, DateTime endTime,
            int phaseNumber)
        {
            var approachCodes = new List<int> { 1, 8, 10 };
            var ar = ApproachRepositoryFactory.Create();
            Approach approach = ar.GetApproachByApproachID(approachId);

            var results = _db.Controller_Event_Log.Where(r =>
                r.SignalID == approach.SignalID && r.Timestamp > startTime && r.Timestamp < endTime
                && approachCodes.Contains(r.EventCode) && r.EventParam == phaseNumber).ToList();

            if (!results.Any())
            {
                var archivedData = ParquetArchive.GetDataFromArchive(_localPath, approach.SignalID, startTime, endTime);
                results = archivedData.Where(r =>
                    r.SignalID == approach.SignalID && r.Timestamp > startTime && r.Timestamp < endTime
                    && approachCodes.Contains(r.EventCode) && r.EventParam == phaseNumber).ToList();
            }

            return results.Count();
        }

        public DateTime GetMostRecentRecordTimestamp(string signalID)
        {
            MOE.Common.Models.Controller_Event_Log row = (from r in _db.Controller_Event_Log
                where r.SignalID == signalID
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