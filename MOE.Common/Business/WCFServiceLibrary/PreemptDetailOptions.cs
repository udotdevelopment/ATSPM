using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.Preempt;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class PreemptDetailOptions : MetricOptions
    {
        public PreemptDetailOptions(string signalID, DateTime startDate, DateTime endDate)
        {
            SignalID = signalID;
            StartDate = startDate;
            EndDate = endDate;
        }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();


            var returnList = new List<string>();
            var tables = new List<ControllerEventLogs>();
            var preTestTables = new List<ControllerEventLogs>();
            var eventsTable = new ControllerEventLogs();
            eventsTable.FillforPreempt(SignalID, StartDate, EndDate);

            var t1 = new ControllerEventLogs();
            var t2 = new ControllerEventLogs();
            var t3 = new ControllerEventLogs();
            var t4 = new ControllerEventLogs();
            var t5 = new ControllerEventLogs();
            var t6 = new ControllerEventLogs();
            var t7 = new ControllerEventLogs();
            var t8 = new ControllerEventLogs();
            var t9 = new ControllerEventLogs();
            var t10 = new ControllerEventLogs();
            var t11 = new ControllerEventLogs();
            var t12 = new ControllerEventLogs();
            var t13 = new ControllerEventLogs();
            var t14 = new ControllerEventLogs();
            var t15 = new ControllerEventLogs();
            var t16 = new ControllerEventLogs();
            var t17 = new ControllerEventLogs();
            var t18 = new ControllerEventLogs();
            var t19 = new ControllerEventLogs();
            var t20 = new ControllerEventLogs();


            preTestTables.Add(t1);
            preTestTables.Add(t2);
            preTestTables.Add(t3);
            preTestTables.Add(t4);
            preTestTables.Add(t5);
            preTestTables.Add(t6);
            preTestTables.Add(t7);
            preTestTables.Add(t8);
            preTestTables.Add(t9);
            preTestTables.Add(t10);
            preTestTables.Add(t11);
            preTestTables.Add(t12);
            preTestTables.Add(t13);
            preTestTables.Add(t14);
            preTestTables.Add(t15);
            preTestTables.Add(t16);
            preTestTables.Add(t17);
            preTestTables.Add(t18);
            preTestTables.Add(t19);
            preTestTables.Add(t20);


            foreach (var row in eventsTable.Events)
                switch (row.EventParam)
                {
                    case 1:
                        t1.Events.Add(row);
                        break;
                    case 2:
                        t2.Events.Add(row);
                        break;
                    case 3:
                        t3.Events.Add(row);
                        break;
                    case 4:
                        t4.Events.Add(row);
                        break;
                    case 5:
                        t5.Events.Add(row);
                        break;
                    case 6:
                        t6.Events.Add(row);
                        break;
                    case 7:
                        t7.Events.Add(row);
                        break;
                    case 8:
                        t8.Events.Add(row);
                        break;
                    case 9:
                        t9.Events.Add(row);
                        break;
                    case 10:
                        t10.Events.Add(row);
                        break;
                    case 11:
                        t11.Events.Add(row);
                        break;
                    case 12:
                        t12.Events.Add(row);
                        break;
                    case 13:
                        t13.Events.Add(row);
                        break;
                    case 14:
                        t14.Events.Add(row);
                        break;
                    case 15:
                        t15.Events.Add(row);
                        break;
                    case 16:
                        t16.Events.Add(row);
                        break;
                    case 17:
                        t17.Events.Add(row);
                        break;
                    case 18:
                        t18.Events.Add(row);
                        break;
                    case 19:
                        t19.Events.Add(row);
                        break;
                    case 20:
                        t20.Events.Add(row);
                        break;
                }

            foreach (var t in preTestTables)
                TestForValidRecords(t, tables);

            foreach (var t in tables)
            {
                t.Add105Events(SignalID, StartDate, EndDate);
                var detailchart = new PreemptDetailChart(this, t);
                var chart = detailchart.Chart;
                var chartName = CreateFileName();
                chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);
                returnList.Add(MetricWebPath + chartName);
            }
            return returnList;
        }

        private void TestForValidRecords(ControllerEventLogs t, List<ControllerEventLogs> tables)
        {
            var AddToTables = false;

            if (t.Events.Count > 0)
            {
                var hasStart = from r in t.Events
                    where r.EventCode == 105 ||
                          r.EventCode == 111 ||
                          r.EventCode == 107
                    select r;

                if (hasStart.Any())
                    AddToTables = true;
            }

            if (AddToTables)
                tables.Add(t);
        }
    }
}