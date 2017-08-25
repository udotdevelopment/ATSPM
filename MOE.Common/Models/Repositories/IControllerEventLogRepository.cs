using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IControllerEventLogRepository
    {
        double GetTMCVolume(DateTime startDate, DateTime endDate, string signalID, int phase);
        List<MOE.Common.Models.Controller_Event_Log> GetSplitEvents(string signalID, DateTime startTime, DateTime endTime);
        List<MOE.Common.Models.Controller_Event_Log> GetSignalEventsByEventCode(string signalID, 
            DateTime startTime, DateTime endTime, int eventCode);
        List<MOE.Common.Models.Controller_Event_Log> GetSignalEventsByEventCodes(string signalID,
            DateTime startTime, DateTime endTime, List<int> eventCodes);
        List<MOE.Common.Models.Controller_Event_Log> GetEventsByEventCodesParam(string signalID,
           DateTime startTime, DateTime endTime, List<int> eventCodes, int param);
        List<MOE.Common.Models.Controller_Event_Log> GetEventsByEventCodesParamWithOffset(string signalID,
           DateTime startTime, DateTime endTime, List<int> eventCodes, int param, double offset);
        MOE.Common.Models.Controller_Event_Log GetFirstEventBeforeDate(string signalID,
            int eventCode, DateTime date);
        List<MOE.Common.Models.Controller_Event_Log> GetSignalEventsBetweenDates(string signalID,
             DateTime startTime, DateTime endTime);
        List<MOE.Common.Models.Controller_Event_Log> GetTopNumberOfSignalEventsBetweenDates(string signalID, int NumberOfRecords,
                     DateTime startTime, DateTime endTime);
        int GetDetectorActivationCount(string signalID,
             DateTime startTime, DateTime endTime, int detectorChannel);
        int GetRecordCount(string signalID, DateTime startTime, DateTime endTime);

    }
}
