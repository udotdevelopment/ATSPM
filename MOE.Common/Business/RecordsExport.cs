using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business
{
    public class RecordsExport
    {
        public MOE.Common.Business.ControllerEventLogs RecordsTable = null;

        //public MOE.Common.Data.MOE.Controller_Event_LogDataTable RecordsTable = new Data.MOE.Controller_Event_LogDataTable();

        public RecordsExport(String signalId, DateTime startDate, DateTime endDate)
        {
            //MOE.Common.Data.MOETableAdapters.Controller_Event_LogTableAdapter eventlogTA = new Data.MOETableAdapters.Controller_Event_LogTableAdapter();

            RecordsTable = new ControllerEventLogs(signalId, startDate, endDate);
            //eventlogTA.FillbySignalBetweenDates(RecordsTable, signalId, startDate, endDate);
            
        }
    }
}
