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
        public PreemptServiceRequestOptions(string signalID, DateTime startDate, DateTime endDate)
        {
            SignalID = signalID;
            StartDate = startDate;
            EndDate = endDate;

        }
        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            List<string> returnList = new List<string>();

            MOE.Common.Business.ControllerEventLogs eventsTable = new MOE.Common.Business.ControllerEventLogs();

            eventsTable.FillforPreempt(SignalID, StartDate, EndDate);

            string location = GetSignalLocation();

            MOE.Common.Business.Preempt.PreemptRequestChart prChart = 
                new MOE.Common.Business.Preempt.PreemptRequestChart(this, eventsTable);
                Chart chart = prChart.chart;
                string chartName = CreateFileName();


                //Save an image of the chart
                chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);

                returnList.Add(MetricWebPath + chartName);

                return returnList;
            
        }
    }
}
