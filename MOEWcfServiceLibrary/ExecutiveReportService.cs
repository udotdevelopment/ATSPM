using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;

namespace MOEWcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ExecutiveReportService" in both code and config file together.
    public class ExecutiveReportService : IExecutiveReportService
    {
        public string FullReport(DateTime startDate, DateTime endDate, int startHour, int endHour, List<DayOfWeek> dayTypes)
        {
            MOE.Common.Business.ExecutiveReporting.ArchiveMetrics am =
                new MOE.Common.Business.ExecutiveReporting.ArchiveMetrics(
                    startDate, endDate, startHour, endHour, dayTypes);

            MOE.Common.Data.ExecutiveReports.ExecutiveAnalysisDataTable table = 
                am.GetAllData();
            StringWriter sw = new StringWriter();
            table.WriteXml(sw);
            string xml = sw.ToString();
            return xml;
        }
    }
}
