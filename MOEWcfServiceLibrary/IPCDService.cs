using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;

namespace MOEWcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPCDService" in both code and config file together.
    [ServiceContract]
    public interface IPCDService
    {
        [OperationContract]
        List<string> CreateChart(DateTime startDate, DateTime endDate, string signalId, bool showVolume,
            int volumeBinSize, string location, double y1AxisMaximum,
            double y2AxisMaximum, int dotSize, bool showArrivalOnGreen);
    }
}
