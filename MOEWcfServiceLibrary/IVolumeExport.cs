using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MOEWcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IVolumeExport" in both code and config file together.
    [ServiceContract]
    public interface IVolumeExport
    {
        [OperationContract]
        DataSet GetExportTable(string signalId, DateTime startDate, DateTime endDate, int binSize,
           List<String> dayOfWeek);
        [OperationContract]
        List<SpeedBin> GetSpeedData(string signalId, string location, DateTime startDate, DateTime endDate, 
            int binSize, List<string> DayOfweek);
        [OperationContract]
        int return1();
    }
}
