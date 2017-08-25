using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;

namespace MOEWcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IExecutiveReportService" in both code and config file together.
    [ServiceContract]
    public interface IExecutiveReportService
    {
        [OperationContract]
        string FullReport(DateTime startDate, DateTime endDate, int startHour, int endHour, List<DayOfWeek> dayTypes);
    }
}
