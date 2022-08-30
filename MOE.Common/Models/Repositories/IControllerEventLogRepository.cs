using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IControllerEventLogRepository
    {
        double GetTmcVolume(DateTime startDate, DateTime endDate, string signalId, int phase);
        List<Controller_Event_Log> GetSplitEvents(string signalId, DateTime startTime, DateTime endTime);

        List<Controller_Event_Log> GetSignalEventsByEventCode(string signalId,
            DateTime startTime, DateTime endTime, int eventCode);

        List<Controller_Event_Log> GetSignalEventsByEventCodes(string signalId,
            DateTime startTime, DateTime endTime, List<int> eventCodes);

        List<Controller_Event_Log> GetEventsByEventCodesParam(string signalId,
            DateTime startTime, DateTime endTime, List<int> eventCodes, int param);

        List<Controller_Event_Log> GetTopEventsAfterDateByEventCodesParam(string signalId, DateTime timestamp,
            List<int> eventCodes, int param, int top);

        int GetEventCountByEventCodesParamDateTimeRange(string signalId,
            DateTime startTime, DateTime endTime, int startHour, int startMinute, int endHour, int endMinute,
            List<int> eventCodes, int param);

        List<Controller_Event_Log> GetEventsByEventCodesParamDateTimeRange(string signalId,
            DateTime startTime, DateTime endTime, int startHour, int startMinute, int endHour, int endMinute,
            List<int> eventCodes, int param);

        List<Controller_Event_Log> GetEventsByEventCodesParamWithOffsetAndLatencyCorrection(string signalId,
            DateTime startTime, DateTime endTime, List<int> eventCodes, int param, double offset,
            double latencyCorrection);

        List<Controller_Event_Log> GetEventsByEventCodesParamWithLatencyCorrection(string signalId,
            DateTime startTime, DateTime endTime, List<int> eventCodes, int param,
            double latencyCorrection);

        Controller_Event_Log GetFirstEventBeforeDate(string signalId,
            int eventCode, DateTime date);

        List<Controller_Event_Log> GetSignalEventsBetweenDates(string signalId,
            DateTime startTime, DateTime endTime);

        List<Controller_Event_Log> GetTopNumberOfSignalEventsBetweenDates(string signalId, int numberOfRecords,
            DateTime startTime, DateTime endTime);

        int GetDetectorActivationCount(string signalId,
            DateTime startTime, DateTime endTime, int detectorChannel);

        int GetRecordCount(string signalId, DateTime startTime, DateTime endTime);

        int GetRecordCountByParameterAndEvent(string signalId, DateTime startTime, DateTime endTime,
            List<int> eventParameters, List<int> events);

        List<Controller_Event_Log> GetRecordsByParameterAndEvent(string signalId, DateTime startTime, DateTime endTime,
            List<int> eventParameters, List<int> eventCodes);

        List<Controller_Event_Log> GetAllAggregationCodes(string signalId, DateTime startTime, DateTime endTime);

        Controller_Event_Log GetFirstEventBeforeDateByEventCodeAndParameter(string signalId, int eventCode,
            int eventParam, DateTime date);

        List<Controller_Event_Log> GetEventsBetweenDates(DateTime startTime, DateTime endTime);

        int GetSignalEventsCountBetweenDates(string signalId, DateTime startTime, DateTime endTime);

        int GetApproachEventsCountBetweenDates(int approachId, DateTime startTime, DateTime endTime,
            int phaseNumber);
        DateTime GetMostRecentRecordTimestamp(string signalID);
        bool CheckForRecords(string signalId, DateTime startTime, DateTime endTime);
    }
}