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
    public class YellowAndRedOptions : MetricOptions
    {
        [DataMember]
        [Display(Name = "Severe Red Light Violations")]
        public double SeverLevel { get; set; }
        [DataMember]
        public int BinSize { get; set; }
        [DataMember]
        [Display(Name = "Red Light Violations")]
        public bool ShowRedLightViolations { get; set; }
        [DataMember]
        [Display(Name = "Severe Red Light Violations")]
        public bool ShowSevereRedLightViolations { get; set; }
        [DataMember]
        [Display(Name = "Percent Red Light Violations")]
        public bool ShowPercentRedLightViolations { get; set; }
        [DataMember]
        [Display(Name = "Percent Severe Red Light Violations")]
        public bool ShowPercentSevereRedLightViolations { get; set; }
        [DataMember]
        [Display(Name = "Average Time Red Light Violations")]
        public bool ShowAverageTimeRedLightViolations { get; set; }
        [DataMember]
        [Display(Name = "Yellow Light Occurrences")]
        public bool ShowYellowLightOccurrences { get; set; }
        [DataMember]
        [Display(Name = "Percent Yellow Light Occurrences")]
        public bool ShowPercentYellowLightOccurrences { get; set; }
        [DataMember]
        [Display(Name = "Average Time Yellow Occurences")]
        public bool ShowAverageTimeYellowOccurences { get; set; }

        public YellowAndRedOptions(string signalID, DateTime startDate, DateTime endDate, double yAxisMax, double y2AxisMax,
            int binSize, int metricTypeID, Double severLevel, bool showRedLightViolations, bool showSevereRedLightViolations,
            bool showPercentRedLightViolations, bool showPercentSevereRedLightViolations, bool showAverageTimeRedLightViolations, 
            bool showYellowLightOccurrences, bool showPercentYellowLightOccurrences, bool showAverageTimeYellowOccurences)
        {
            SignalID = signalID;
            //StartDate = startDate;
            //EndDate = endDate;
            YAxisMax = yAxisMax;
            Y2AxisMax = y2AxisMax;
            MetricTypeID = metricTypeID;
            SeverLevel = severLevel;
            ShowRedLightViolations = showRedLightViolations;
            ShowSevereRedLightViolations = showSevereRedLightViolations;
            ShowPercentRedLightViolations = showPercentRedLightViolations;
            ShowPercentSevereRedLightViolations = showPercentSevereRedLightViolations;
            ShowAverageTimeRedLightViolations = showAverageTimeRedLightViolations;
            ShowYellowLightOccurrences = showYellowLightOccurrences;
            ShowPercentYellowLightOccurrences = showPercentYellowLightOccurrences;
            ShowAverageTimeYellowOccurences = showAverageTimeYellowOccurences;
            BinSize = binSize;
        }

        public YellowAndRedOptions()
        {
            MetricTypeID = 11;
            BinSize = 15; //TODO: this is not even an option on the front end!
            SetDefaults();
        }
        public void SetDefaults()
        {
            YAxisMax = 15;
            SeverLevel = 4.0;
            ShowRedLightViolations = true;
            ShowSevereRedLightViolations = true;
            ShowPercentRedLightViolations = true;
            ShowPercentSevereRedLightViolations = true;
            ShowAverageTimeRedLightViolations = true;
            ShowYellowLightOccurrences = true;
            ShowPercentYellowLightOccurrences = true;
            ShowAverageTimeYellowOccurences = true;
        }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            List<string> returnList = new List<string>();
            //string MetricLocation = ConfigurationManager.AppSettings["ImageLocation"];

            MOE.Common.Business.RLMSignalPhaseCollection signalphasecollection =
              new MOE.Common.Business.RLMSignalPhaseCollection(StartDate, EndDate, SignalID, BinSize, SeverLevel);

            if (signalphasecollection.SignalPhaseList.Count > 0)
            {
                foreach (MOE.Common.Business.RLMSignalPhase signalPhase in signalphasecollection.SignalPhaseList)
                {
                    string location = GetSignalLocation();
                    MOE.Common.Business.RLMChart rlmChart = new RLMChart();
                    Chart chart = rlmChart.GetChart(signalPhase, this);

                    if (signalPhase.IsPermissive)
                    {
                        chart.BackColor = Color.LightGray;
                        chart.Titles[2].Text = "Permissive " + chart.Titles[2].Text + " " + signalPhase.Approach.Detectors.FirstOrDefault().MovementType.Description;

                    }
                    else
                    {
                        chart.Titles[2].Text = "Protected " + chart.Titles[2].Text + " " + signalPhase.Approach.Detectors.FirstOrDefault().MovementType.Description;
                    }


                   

                    string chartName = CreateFileName();

                    chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);
                    returnList.Add(MetricWebPath + chartName);
                }
            }

            return returnList;
        }
    }
}
