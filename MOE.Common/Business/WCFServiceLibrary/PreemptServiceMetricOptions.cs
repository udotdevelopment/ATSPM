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
    public class PreemptServiceMetricOptions : MetricOptions
    {
        public PreemptServiceMetricOptions(string signalID, DateTime startDate, DateTime endDate, double yAxisMax)
        {
            SignalID = signalID;
            StartDate = startDate;
            EndDate = endDate;
            YAxisMax = yAxisMax;        
            //MetricTypeID = metricTypeID;
        }
        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            List<string> returnString = new List<string>();

            ControllerEventLogs eventsTable = new ControllerEventLogs();

            eventsTable.FillforPreempt(SignalID, StartDate, EndDate);
            if (eventsTable.Events.Count > 0)
            {
                Preempt.PreemptServiceMetric psChart = new Preempt.PreemptServiceMetric(this, eventsTable);
                Chart chart = psChart.chart;
                string chartName = CreateFileName();
                chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);
                returnString.Add(MetricWebPath + chartName);
            }
            return returnString;
        }
    }
}
