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
using System.ComponentModel.DataAnnotations;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class SplitFailOptions : MetricOptions
    {
        [Required]
        [DataMember]
        [Display(Name = "First Seconds Of Red")]
        public int FirstSecondsOfRed { get; set; }
        [DataMember]
        [Display(Name = "Show Fail Lines")]
        public bool ShowFailLines { get; set; }
        [DataMember]
        [Display(Name = "Show Average Lines")]
        public bool ShowAvgLines { get; set; }
        [DataMember]
        [Display(Name = "Show Percent Fail Lines")]
        public bool ShowPercentFailLines { get; set; }


        public SplitFailOptions(string signalID, DateTime startDate, DateTime endDate,
            int metricTypeID, int firstSecondsOfRed, bool showFailLines, bool showAvgLines, bool showPercentFailLine)
        {
            SignalID = signalID;
            StartDate = startDate;
            EndDate = endDate;
            MetricTypeID = metricTypeID;
            FirstSecondsOfRed = firstSecondsOfRed;
            ShowFailLines = showFailLines;
            ShowAvgLines = showAvgLines;
            ShowPercentFailLines = showPercentFailLine;
        }
        public SplitFailOptions()
        {
            MetricTypeID = 12;
            SetDefaults();
        }
        public void SetDefaults()
        {
            FirstSecondsOfRed = 5;
            ShowFailLines = true;
            ShowAvgLines = true;
            ShowPercentFailLines = false;
        }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            List<string> returnString = new List<string>();



            MOE.Common.Models.Repositories.ISignalsRepository sr = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            MOE.Common.Models.Signal signal = sr.GetSignalBySignalID(SignalID);



            //for (int x = 1; x < maxPhase + 1; x++)
            List<Approach> metricApproaches = signal.GetApproachesForSignalThatSupportMetric(this.MetricTypeID);

            if (metricApproaches.Count > 0)
            {
                foreach (Approach approach in metricApproaches)
                {


                
                        //MOE.Common.Business.CustomReport.Phase phase = new MOE.Common.Business.CustomReport.Phase(detectors, x, SignalID, StartDate, EndDate, new List<int> { 1, 4, 5, 6, 7, 8, 9, 10, 61, 63, 64 }, 1);
                        MOE.Common.Business.CustomReport.Phase phase = new MOE.Common.Business.CustomReport.Phase(approach, StartDate, EndDate, new List<int> { 1, 4, 5, 6, 7, 8, 9, 10, 61, 63, 64 }, 1, false);

                        phase.ApproachDirection = approach.DirectionType.Description;

                        string location = GetSignalLocation();
                        string chartName = CreateFileName();


                        if (phase.PhaseNumber > 0)
                        {
                            GetChart(StartDate, EndDate, phase, location, FirstSecondsOfRed,
                                    ShowFailLines, ShowAvgLines, ShowPercentFailLines, YAxisMax, chartName, returnString, false);
                        }

                    if(approach.PermissivePhaseNumber != null && approach.PermissivePhaseNumber > 0)
                    {
                        string permChartName = CreateFileName();

                        MOE.Common.Business.CustomReport.Phase permPhase = new MOE.Common.Business.CustomReport.Phase(approach, StartDate, 
                            EndDate, new List<int> { 1, 4, 5, 6, 7, 8, 9, 10, 61, 63, 64 }, 1, true);

                        permPhase.ApproachDirection = approach.DirectionType.Description;

                         GetChart(StartDate, EndDate, permPhase, location, FirstSecondsOfRed,
                                ShowFailLines, ShowAvgLines, ShowPercentFailLines, YAxisMax, permChartName, returnString, true);
                    }

                      
                    
                }
            }
            return returnString;
        }

        private void GetChart(DateTime StartDate, DateTime EndDate, CustomReport.Phase phase, string location, int FirstSecondsOfRed, bool ShowFailLines, bool ShowAvgLines, bool ShowPercentFailLines, double? YAxisMax, string chartName, List<string> returnString, bool isPermissivePhase)
        {
            MOE.Common.Business.SplitFail.SplitFailChart sfChart = 
                new MOE.Common.Business.SplitFail.SplitFailChart(this, phase);

            if (isPermissivePhase)
            {
                sfChart.chart.BackColor = Color.LightGray;
                sfChart.chart.Titles[2].Text = "Permissive " + sfChart.chart.Titles[2].Text + " " + phase.Approach.GetDetectorsForMetricType(12).FirstOrDefault().MovementType.Description;
                
            }
            else
            {
                sfChart.chart.Titles[2].Text = "Protected " + sfChart.chart.Titles[2].Text + " " + phase.Approach.GetDetectorsForMetricType(12).FirstOrDefault().MovementType.Description;
            }

            System.Threading.Thread.Sleep(300);

            chartName = chartName.Replace(".", (phase.ApproachDirection + "."));

            try
            {
                sfChart.chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);
            }
            catch
            {
                try
                {
                    sfChart.chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);
                }
                catch
                {
                    Models.Repositories.IApplicationEventRepository appEventRepository =
                     Models.Repositories.ApplicationEventRepositoryFactory.Create();
                    Models.ApplicationEvent applicationEvent = new ApplicationEvent();
                    applicationEvent.ApplicationName = "SPM Website";
                    applicationEvent.Description = MetricType.ChartName + " Failed While Saving File";
                    applicationEvent.SeverityLevel = ApplicationEvent.SeverityLevels.Medium;
                    applicationEvent.Timestamp = DateTime.Now;
                    appEventRepository.Add(applicationEvent);
                }
            }



            returnString.Add(MetricWebPath + chartName);
        }
    }
}

