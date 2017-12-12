using System;
using System.CodeDom;
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
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    [KnownType(typeof(PCDOptions))]
    [KnownType(typeof(TMCOptions))]
    [KnownType(typeof(AoROptions))]
    [KnownType(typeof(ApproachDelayOptions))]
    [KnownType(typeof(MetricOptions))]
    [KnownType(typeof(PhaseTerminationOptions))]
    [KnownType(typeof(PreemptDetailOptions))]
    [KnownType(typeof(PreemptServiceMetricOptions))]
    [KnownType(typeof(PreemptServiceRequestOptions))]
    [KnownType(typeof(YellowAndRedOptions))]
    [KnownType(typeof(ApproachSpeedOptions))]
    [KnownType(typeof(SplitFailOptions))]
    [KnownType(typeof(SplitMonitorOptions))]
    [KnownType(typeof(PedDelayOptions))]
    [KnownType(typeof(ApproachVolumeOptions))]
    [KnownType(typeof(AggregationMetricOptions))]
    [KnownType(typeof(string[]))]
    public class MetricOptions
    {
        [Key]
        [Required]
        [DataMember]
        public string SignalID { get; set; }
        [DataMember]
        public int MetricTypeID { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }
        [DataMember]
        [Display(Name="Y-axis Max")]
        public double? YAxisMax { get; set; }
        [DataMember]
        [Display(Name = "Y-axis Min")]
        public double YAxisMin { get; set; }
        [DataMember]
        [Display(Name = "Secondary Y-axis Max")]
        public double? Y2AxisMax { get; set; }
        [DataMember]
        [Display(Name = "Secondary Y-axis Min")]
        public double Y2AxisMin { get; set; }
        [DataMember]
        public string MetricFileLocation { get; set; }
        [DataMember]
        public string MetricWebPath { get; set; }
        
        public MetricType MetricType{ get; set; }

        



        [DataMember]
        public List<string> ReturnList{ get; set;}

        public MetricOptions()
        {
            SignalID = string.Empty;
            YAxisMin = 0;
            Y2AxisMax = 0;
            Y2AxisMin = 0;
            MetricTypeID = 0;
            MetricFileLocation = ConfigurationManager.AppSettings["ImageLocation"];
            MetricWebPath = ConfigurationManager.AppSettings["ImageWebLocation"];
            ReturnList = new List<string>(); 
        }

        public virtual List<string> CreateMetric()
        {
            Models.Repositories.IMetricTypeRepository metricTypeRepository = Models.Repositories.MetricTypeRepositoryFactory.Create();
            MetricType = metricTypeRepository.GetMetricsByID(MetricTypeID);
            LogMetricRun();  
            return new List<string>();
        }


        protected void LogMetricRun()
        {
            Models.Repositories.IApplicationEventRepository appEventRepository =
                Models.Repositories.ApplicationEventRepositoryFactory.Create();
            ApplicationEvent applicationEvent = new ApplicationEvent();
            applicationEvent.ApplicationName = "SPM Website";
            applicationEvent.Description = MetricType.ChartName + " Executed";
            applicationEvent.SeverityLevel = ApplicationEvent.SeverityLevels.Low;
            applicationEvent.Timestamp = DateTime.Now;
            appEventRepository.Add(applicationEvent);
        }       

        public string GetSignalLocation()
        {
            Models.Repositories.ISignalsRepository signalRepository =
                Models.Repositories.SignalsRepositoryFactory.Create();
            return signalRepository.GetSignalLocation(SignalID);
        }

        public string CreateFileName()
        {
            if (MetricType == null)
            {
                Models.Repositories.IMetricTypeRepository metricTypeRepository = Models.Repositories.MetricTypeRepositoryFactory.Create();
                MetricType = metricTypeRepository.GetMetricsByID(MetricTypeID);
            }
            
                string fileName = MetricType.Abbreviation +
                                  SignalID +
                                  "-" +
                                  StartDate.Year.ToString() +
                                  StartDate.ToString("MM") +
                                  StartDate.ToString("dd") +
                                  StartDate.ToString("HH") +
                                  StartDate.ToString("mm") +
                                  "-" +
                                  EndDate.Year.ToString() +
                                  EndDate.ToString("MM") +
                                  EndDate.ToString("dd") +
                                  EndDate.ToString("HH") +
                                  EndDate.ToString("mm-");
                Random r = new Random();
                fileName += r.Next().ToString();
                fileName += ".jpg";
            try
            {
                if (DriveAvailable())
                {
                    return fileName;
                }
                return null;
            }
            catch
            {
                throw new Exception("Path not found");
                
            }
        }

        public bool DriveAvailable()
        {
            DirectoryInfo di = new DirectoryInfo(MetricFileLocation);
            di.Refresh();
            if(di.Exists)
            {
                return true;
            }
            Directory.CreateDirectory(MetricFileLocation);
            di.Refresh();
            if (di.Exists)
            {
                return true;
            }
            return false;
        }


    }
}
