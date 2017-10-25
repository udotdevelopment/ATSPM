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
    public class ApproachDelayOptions: MetricOptions
    {
        [Required]
        [DataMember]
        [Display(Name = "Volume Bin Size")]
        public int SelectedBinSize { get; set; }
        [DataMember]
        public List<int> BinSizeList { get; set; }

        [DataMember]
        [Display(Name = "Show Plans")]
        public bool ShowPlanStatistics { get; set; }
        [DataMember]
        [Display(Name = "Show Total Delay PerHour")]
        public bool ShowTotalDelayPerHour { get; set; }
        [DataMember]
        [Display(Name = "Show Delay Per Vehicle")]
        public bool ShowDelayPerVehicle { get; set; }
        
        public ApproachDelayOptions(string signalID, DateTime startDate, DateTime endDate, double yAxisMax, double y2AxisMax,
            int binSize, bool showPlanStatistics, int metricTypeID, bool showDelayPerVehicle, bool showTotalDelayPerHour)
        {
            SignalID = signalID;
            //StartDate = startDate;
            //EndDate = endDate;
            YAxisMax = yAxisMax;
            Y2AxisMax = y2AxisMax;
            SelectedBinSize = binSize;
            ShowPlanStatistics = showPlanStatistics;
            MetricTypeID = metricTypeID;
            ShowTotalDelayPerHour = ShowTotalDelayPerHour;
            ShowDelayPerVehicle = showDelayPerVehicle;            
        }
        public  ApproachDelayOptions()
        {
            BinSizeList = new List<int>();
            BinSizeList.Add(15);
            BinSizeList.Add(5);
            MetricTypeID = 8;
            SetDefaults();
        }
        public void SetDefaults()
        {
            YAxisMax = 15;
            Y2AxisMax = 20000;
            ShowPlanStatistics = true;
            ShowTotalDelayPerHour = true;
            ShowDelayPerVehicle = true;
        }
        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            string location = GetSignalLocation();

             MOE.Common.Business.SignalPhaseCollection signalphasecollection =
               new MOE.Common.Business.SignalPhaseCollection(StartDate,
                   EndDate, SignalID,
                   ShowPlanStatistics, SelectedBinSize, 8);
            

          
                foreach (MOE.Common.Business.SignalPhase signalPhase in signalphasecollection.SignalPhaseList)
                {

                    MOE.Common.Business.DelayChart delayChart = new MOE.Common.Business.DelayChart(this, signalPhase);

                    Chart chart = delayChart.chart;

                            string chartName = CreateFileName();
                            
                            var removethese = new List<Title>();

                            foreach (Title t in chart.Titles)
                            {
                                if (t.Text == "" || t.Text == null)
                                {
                                    removethese.Add(t);
                                }
                            }
                            foreach (Title t in removethese)
                            {
                                chart.Titles.Remove(t);
                            }

                            //Save an image of the chart
                            chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);

                            ReturnList.Add(MetricWebPath + chartName);

            }
                return ReturnList;

        }
    }
}
