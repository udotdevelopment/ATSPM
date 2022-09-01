using ATSPM.Application.Models;
using System;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface IControllerEventLogRepository
    {
        double GetTmcVolume(DateTime startDate, DateTime endDate, string signalId, int phase);
        List<ControllerEventLog> GetSplitEvents(string signalId, DateTime startTime, DateTime endTime);

        List<ControllerEventLog> GetSignalEventsByEventCode(string signalId,
            DateTime startTime, DateTime endTime, int eventCode);

        List<ControllerEventLog> GetSignalEventsByEventCodes(string signalId,
            DateTime startTime, DateTime endTime, List<int> eventCodes);

        List<ControllerEventLog> GetEventsByEventCodesParam(string signalId,
            DateTime startTime, DateTime endTime, List<int> eventCodes, int param);

        List<ControllerEventLog> GetTopEventsAfterDateByEventCodesParam(string signalId, DateTime timestamp,
            List<int> eventCodes, int param, int top);

        List<ControllerEventLog> GetTopEventsBeforeDateByEventCodesParam(string signalId, DateTime timestamp,
            List<int> eventCodes, int param, int top);

        int GetEventCountByEventCodesParamDateTimeRange(string signalId,
            DateTime startTime, DateTime endTime, int startHour, int startMinute, int endHour, int endMinute,
            List<int> eventCodes, int param);

        List<ControllerEventLog> GetEventsByEventCodesParamDateTimeRange(string signalId,
            DateTime startTime, DateTime endTime, int startHour, int startMinute, int endHour, int endMinute,
            List<int> eventCodes, int param);

        List<ControllerEventLog> GetEventsByEventCodesParamWithOffsetAndLatencyCorrection(string signalId,
            DateTime startTime, DateTime endTime, List<int> eventCodes, int param, double offset,
            double latencyCorrection);

        List<ControllerEventLog> GetEventsByEventCodesParamWithLatencyCorrection(string signalId,
            DateTime startTime, DateTime endTime, List<int> eventCodes, int param,
            double latencyCorrection);

        ControllerEventLog GetFirstEventBeforeDate(string signalId,
            int eventCode, DateTime date);

        List<ControllerEventLog> GetSignalEventsBetweenDates(string signalId,
            DateTime startTime, DateTime endTime);

        List<ControllerEventLog> GetTopNumberOfSignalEventsBetweenDates(string signalId, int numberOfRecords,
            DateTime startTime, DateTime endTime);

        int GetDetectorActivationCount(string signalId,
            DateTime startTime, DateTime endTime, int detectorChannel);

        int GetRecordCount(string signalId, DateTime startTime, DateTime endTime);

        int GetRecordCountByParameterAndEvent(string signalId, DateTime startTime, DateTime endTime,
            List<int> eventParameters, List<int> events);

        List<ControllerEventLog> GetRecordsByParameterAndEvent(string signalId, DateTime startTime, DateTime endTime,
            List<int> eventParameters, List<int> eventCodes);

        List<ControllerEventLog> GetAllAggregationCodes(string signalId, DateTime startTime, DateTime endTime);

        ControllerEventLog GetFirstEventBeforeDateByEventCodeAndParameter(string signalId, int eventCode,
            int eventParam, DateTime date);

        List<ControllerEventLog> GetEventsBetweenDates(DateTime startTime, DateTime endTime);

        int GetSignalEventsCountBetweenDates(string signalId, DateTime startTime, DateTime endTime);

        int GetApproachEventsCountBetweenDates(int approachId, DateTime startTime, DateTime endTime,
            int phaseNumber, IApproachRepository approachRepository);
        DateTime GetMostRecentRecordTimestamp(string signalID);
        bool CheckForRecords(string signalId, DateTime startTime, DateTime endTime);

        ControllerEventLog GetFirstEventAfterDateByEventCodesAndParameter(string signalId, List<int> eventCodes,
            int eventParam, DateTime start, int secondsToSearch);
    }
}