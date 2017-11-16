using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business;
using MOE.Common.Models;
using System.Runtime.Serialization;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class PreemptServiceRequestOptions: MetricOptions
    {
        public PreemptServiceRequestOptions(string signalId, DateTime startDate, DateTime endDate)
        {
            SignalID = signalId;
            StartDate = startDate;
            EndDate = endDate;

        }
        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            List<string> returnList = new List<string>();
            ControllerEventLogs eventsTable = new ControllerEventLogs();
            eventsTable.FillforPreempt(SignalID, StartDate, EndDate);
            Preempt.PreemptRequestChart prChart = new Preempt.PreemptRequestChart(this, eventsTable);
            Chart chart = prChart.Chart;
            string chartName = CreateFileName();
            chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);
            returnList.Add(MetricWebPath + chartName);
            return returnList;
        }
    }
}
