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
    public class PreemptDetailOptions: MetricOptions
    {
        public PreemptDetailOptions( string signalID, DateTime startDate, DateTime endDate)
        {
            SignalID = signalID;
            StartDate = startDate;
            EndDate = endDate;

        }
        public override List<string> CreateMetric()
        {
            base.CreateMetric();


            List<string> returnList = new List<string>();
            List<MOE.Common.Business.ControllerEventLogs> tables = new List<MOE.Common.Business.ControllerEventLogs>();
            List<MOE.Common.Business.ControllerEventLogs> preTestTables = new List<MOE.Common.Business.ControllerEventLogs>();
            MOE.Common.Business.ControllerEventLogs eventsTable = new MOE.Common.Business.ControllerEventLogs();
            eventsTable.FillforPreempt(SignalID, StartDate, EndDate);

                MOE.Common.Business.ControllerEventLogs t1 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t2 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t3 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t4 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t5 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t6 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t7 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t8 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t9 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t10 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t11 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t12 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t13 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t14 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t15 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t16 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t17 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t18 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t19 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t20 = new MOE.Common.Business.ControllerEventLogs();


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



                foreach (MOE.Common.Models.Controller_Event_Log row in eventsTable.Events)
                {
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
                }

                foreach (MOE.Common.Business.ControllerEventLogs t in preTestTables)
                {
                    TestForValidRecords(t, tables);
                }
                
                foreach (MOE.Common.Business.ControllerEventLogs t in tables)
                {
                    t.Add105Events(SignalID, StartDate, EndDate);
                    
                    MOE.Common.Business.Preempt.PreemptDetailChart detailchart = 
                        new MOE.Common.Business.Preempt.PreemptDetailChart(this, t);
                    Chart chart = detailchart.chart;
                    string chartName = CreateFileName();
                    chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);
                    returnList.Add(MetricWebPath + chartName);
                }
                return returnList;
            }

        private void TestForValidRecords(ControllerEventLogs t, List<ControllerEventLogs> tables)
        {
            bool AddToTables = false;

            if(t.Events.Count > 0)
            {
                var hasStart = from r in t.Events
                               where r.EventCode == 105 ||
                               r.EventCode == 111 ||
                               r.EventCode == 107
                               select r;

                if(hasStart.Count() > 0 )
                {
                    AddToTables = true;
                }

            }

            if(AddToTables)
            {
                tables.Add(t);
            }
        }

        }

 
    }
