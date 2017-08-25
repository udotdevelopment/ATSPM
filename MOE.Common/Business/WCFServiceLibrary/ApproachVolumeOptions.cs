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
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class ApproachVolumeOptions: MetricOptions
    {
        [Required]
        [DataMember]
        [Display(Name = "Volume Bin Size")]
        public int SelectedBinSize { get; set; }
        [DataMember]
        public List<int> BinSizeList { get; set; }
        [DataMember]
        [Display(Name = "Show Directional Splits")]
        public bool ShowDirectionalSplits { get; set; }
        [DataMember]
        [Display(Name = "Show Total Volume")]
        public bool ShowTotalVolume { get; set; }
        [DataMember]
        [Display(Name = "Show NB/WB Volume")]
        public bool ShowNBWBVolume { get; set; }
        [DataMember]
        [Display(Name = "Show SB/EB Volume")]
        public bool ShowSBEBVolume { get; set; }
        [DataMember]
        [Display(Name = "Show TMC Detection")]
        public bool ShowTMCDetection { get; set; }
        [DataMember]
        [Display(Name = "Show Advance Detection")]
        public bool ShowAdvanceDetection { get; set; }

        public List<MOE.Common.Business.ApproachVolume.MetricInfo> MetricInfoList;

        

        public ApproachVolumeOptions(string signalID, DateTime startDate, DateTime endDate, double? yAxisMax, int binSize, bool showDirectionalSplits,
            bool showTotalVolume, bool showNBWBVolume, bool showSBEBVolume, bool showTMCDetection, bool showAdvanceDetection)
        {
            SignalID = signalID;
            //StartDate = startDate;
            //EndDate = endDate;
            YAxisMax = yAxisMax;

            SelectedBinSize = binSize;
            ShowTotalVolume = showTotalVolume;
            ShowDirectionalSplits = showDirectionalSplits;
            ShowNBWBVolume = showNBWBVolume;
            ShowSBEBVolume = showSBEBVolume;
            ShowTMCDetection = showTMCDetection;
            ShowAdvanceDetection = showAdvanceDetection;
        }

        public ApproachVolumeOptions()
        {
            
            BinSizeList = new List<int>();
            BinSizeList.Add(15);
            BinSizeList.Add(5);
            MetricTypeID = 7;
            SetDefaults();
        }

        public void SetDefaults()
        {
            YAxisMin = 0;
            YAxisMax = null;
            ShowDirectionalSplits = false;
            ShowTotalVolume = false;
            ShowNBWBVolume = true;
            ShowSBEBVolume = true;
            ShowTMCDetection = true;
            ShowAdvanceDetection = true;
        }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            List<string> returnList = new List<string>();
            MetricInfoList = new List<ApproachVolume.MetricInfo>();          
            MOE.Common.Models.Repositories.ISignalsRepository signalsRepository = 
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            var signal = signalsRepository.GetSignalBySignalID(this.SignalID);    
            var NSAdvanceVolumeApproaches = new List<MOE.Common.Business.ApproachVolume.Approach>();
            var NSTMCVolumeApproaches = new List<MOE.Common.Business.ApproachVolume.Approach>();
            var EWAdvanceVolumeApproaches = new List<MOE.Common.Business.ApproachVolume.Approach>();
            var EWTMCVolumeApproaches = new List<MOE.Common.Business.ApproachVolume.Approach>(); 
            
            //Sort the approaches by metric type and direction

            foreach(MOE.Common.Models.Approach a in signal.Approaches)
            {
                if (a.DirectionType.Description == "Northbound" && a.GetDetectorsForMetricType(6).Count > 0)
               {
                   MOE.Common.Business.ApproachVolume.Approach av = new MOE.Common.Business.ApproachVolume.Approach(a);
                   NSAdvanceVolumeApproaches.Add(av);
               }

               if (a.DirectionType.Description == "Northbound" && a.GetDetectorsForMetricType(5).Count > 0)
               {
                   MOE.Common.Business.ApproachVolume.Approach av = new MOE.Common.Business.ApproachVolume.Approach(a);
                   NSTMCVolumeApproaches.Add(av);
               }
               if (a.DirectionType.Description == "Southbound" && a.GetDetectorsForMetricType(6).Count > 0)
               {
                   MOE.Common.Business.ApproachVolume.Approach av = new MOE.Common.Business.ApproachVolume.Approach(a);
                   NSAdvanceVolumeApproaches.Add(av);
               }

               if (a.DirectionType.Description == "Southbound" && a.GetDetectorsForMetricType(5).Count > 0)
               {
                   MOE.Common.Business.ApproachVolume.Approach av = new MOE.Common.Business.ApproachVolume.Approach(a);
                   NSTMCVolumeApproaches.Add(av);
               }
               if (a.DirectionType.Description == "Eastbound" && a.GetDetectorsForMetricType(6).Count > 0)
               {
                   MOE.Common.Business.ApproachVolume.Approach av = new MOE.Common.Business.ApproachVolume.Approach(a);
                   EWAdvanceVolumeApproaches.Add(av);
               }

               if (a.DirectionType.Description == "Eastbound" && a.GetDetectorsForMetricType(5).Count > 0)
               {
                   MOE.Common.Business.ApproachVolume.Approach av = new MOE.Common.Business.ApproachVolume.Approach(a);
                   EWTMCVolumeApproaches.Add(av);
               }
                if (a.DirectionType.Description == "Westbound" && a.GetDetectorsForMetricType(6).Count > 0)
               {
                   MOE.Common.Business.ApproachVolume.Approach av = new MOE.Common.Business.ApproachVolume.Approach(a);
                   EWAdvanceVolumeApproaches.Add(av);
               }

               if (a.DirectionType.Description == "Westbound" && a.GetDetectorsForMetricType(5).Count > 0)
               {
                   MOE.Common.Business.ApproachVolume.Approach av = new MOE.Common.Business.ApproachVolume.Approach(a);
                   EWTMCVolumeApproaches.Add(av);
               }
            }



                string location = GetSignalLocation();

            //create the charts for each metric type and direction

                if (ShowAdvanceDetection && NSAdvanceVolumeApproaches.Count > 0)
                {

                        MOE.Common.Business.ApproachVolume.ApproachVolumeChart AVC = 
                            new MOE.Common.Business.ApproachVolume.ApproachVolumeChart(
                                StartDate, EndDate, SignalID, location, "Northbound", "Southbound", this,
                                NSAdvanceVolumeApproaches, true);
                        
                        string chartName = CreateFileName();


                        //Save an image of the chart
                      AVC.Chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);

                      AVC.info.ImageLocation = (MetricWebPath + chartName);
                      MetricInfoList.Add(AVC.info);


                       
                    }

                        if (ShowTMCDetection && NSTMCVolumeApproaches.Count > 0)
                    {
                        MOE.Common.Business.ApproachVolume.ApproachVolumeChart AVC = 
                            new MOE.Common.Business.ApproachVolume.ApproachVolumeChart(StartDate, EndDate, SignalID,
                                location, "Northbound", "Southbound", this, NSTMCVolumeApproaches, false);

                        string chartName = CreateFileName();


                        //Save an image of the chart
                        AVC.Chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);

                        AVC.info.ImageLocation = (MetricWebPath + chartName);
                        MetricInfoList.Add(AVC.info);

                }

                        if (ShowAdvanceDetection && EWAdvanceVolumeApproaches.Count > 0)
                {
                   
                    
                        MOE.Common.Business.ApproachVolume.ApproachVolumeChart AVC = 
                            new MOE.Common.Business.ApproachVolume.ApproachVolumeChart(StartDate, EndDate,
                                SignalID, location, "Eastbound", "Westbound", this, EWAdvanceVolumeApproaches, true);

                        string chartName = CreateFileName();


                        //Save an image of the chart
                        AVC.Chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);

                        AVC.info.ImageLocation = (MetricWebPath + chartName);
                        MetricInfoList.Add(AVC.info);

                       
                    }

                    if (ShowTMCDetection && EWTMCVolumeApproaches.Count > 0)
                    {
                        MOE.Common.Business.ApproachVolume.ApproachVolumeChart AVC = 
                            new MOE.Common.Business.ApproachVolume.ApproachVolumeChart(StartDate, EndDate,
                                SignalID, location, "Eastbound", "Westbound", this, EWTMCVolumeApproaches, false);

                        string chartName = CreateFileName();


                        //Save an image of the chart
                        AVC.Chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);


                        AVC.info.ImageLocation = (MetricWebPath + chartName);
                        MetricInfoList.Add(AVC.info);

                     
                    }
                

            
            return returnList;
        }



       
    }
}
