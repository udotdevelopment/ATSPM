using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.Preempt;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class PreemptServiceRequestOptions : MetricOptions
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
            var returnList = new List<string>();
            var eventsTable = new ControllerEventLogs();
            eventsTable.FillforPreempt(SignalID, StartDate, EndDate);
            var prChart = new PreemptRequestChart(this, eventsTable);
            var chart = prChart.Chart;
            var chartName = CreateFileName();
            chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);
            returnList.Add(MetricWebPath + chartName);
            return returnList;
        }
    }
}