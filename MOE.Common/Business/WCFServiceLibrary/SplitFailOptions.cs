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
using MOE.Common.Business.SplitFail;

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
            EndDate = EndDate.AddSeconds(59);
            List<string> returnString = new List<string>();
            Models.Repositories.ISignalsRepository sr = Models.Repositories.SignalsRepositoryFactory.Create();
            Models.Signal signal = sr.GetVersionOfSignalByDate(SignalID, StartDate);
            List<Approach> metricApproaches = signal.GetApproachesForSignalThatSupportMetric(MetricTypeID);
            if (metricApproaches.Count > 0)
            {
                Parallel.ForEach(metricApproaches, approach =>
                //foreach (Approach approach in metricApproaches)
                {
                    if (approach.ProtectedPhaseNumber > 0)
                    {
                        SplitFailPhase splitFailPhase = new SplitFailPhase(approach, this, false);
                        string chartName = CreateFileName();
                        GetChart(splitFailPhase, chartName, returnString, false, approach);
                    }
                    if(approach.PermissivePhaseNumber != null && approach.PermissivePhaseNumber > 0)
                    {
                        string permChartName = CreateFileName();
                        SplitFailPhase splitFailPermissivePhase = new SplitFailPhase(approach, this, true);
                        GetChart(splitFailPermissivePhase, permChartName, returnString, true, approach);
                    }
                });
            }
            return returnString;
        }

        private void GetChart(SplitFailPhase splitFailPhase, string chartName, List<string> returnString, bool getPermissivePhase, Approach approach)
        {
            SplitFailChart sfChart = new SplitFailChart(this, splitFailPhase, getPermissivePhase);
            var detector = approach.GetDetectorsForMetricType(12).FirstOrDefault();
            if (detector != null)
            {
                string direction = detector.MovementType.Description;
                if (getPermissivePhase)
                {
                    sfChart.Chart.BackColor = Color.LightGray;
                    sfChart.Chart.Titles[2].Text = "Permissive " + sfChart.Chart.Titles[2].Text + " " + direction;
                }
                else
                {
                    sfChart.Chart.Titles[2].Text = "Protected " + sfChart.Chart.Titles[2].Text + " " + direction;
                }
            }
            System.Threading.Thread.Sleep(300);
            chartName = chartName.Replace(".", (approach.DirectionType.Description + "."));
            try
            {
                sfChart.Chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);
            }
            catch(Exception ex)
            {
                try
                {
                    sfChart.Chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);
                }
                catch
                {
                    Models.Repositories.IApplicationEventRepository appEventRepository =
                    Models.Repositories.ApplicationEventRepositoryFactory.Create();
                    ApplicationEvent applicationEvent = new ApplicationEvent();
                    applicationEvent.ApplicationName = "SPM Website";
                    applicationEvent.Description = MetricType.ChartName + ex.Message + " Failed While Saving File";
                    applicationEvent.SeverityLevel = ApplicationEvent.SeverityLevels.Medium;
                    applicationEvent.Timestamp = DateTime.Now;
                    appEventRepository.Add(applicationEvent);
                }
            }
            returnString.Add(MetricWebPath + chartName);
        }
    }
}

