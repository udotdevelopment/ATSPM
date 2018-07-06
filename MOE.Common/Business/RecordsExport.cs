using System;

namespace MOE.Common.Business
{
    public class RecordsExport
    {
        public ControllerEventLogs RecordsTable;

        //public MOE.Common.Data.MOE.Controller_Event_LogDataTable RecordsTable = new Data.MOE.Controller_Event_LogDataTable();

        public RecordsExport(string signalId, DateTime startDate, DateTime endDate)
        {
            //MOE.Common.Data.MOETableAdapters.Controller_Event_LogTableAdapter eventlogTA = new Data.MOETableAdapters.Controller_Event_LogTableAdapter();

            RecordsTable = new ControllerEventLogs(signalId, startDate, endDate);
            //eventlogTA.FillbySignalBetweenDates(RecordsTable, signalId, startDate, endDate);
        }
    }
}