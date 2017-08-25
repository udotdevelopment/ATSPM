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
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    [KnownType(typeof(MOE.Common.Business.WCFServiceLibrary.PCDOptions))]
    [KnownType(typeof(MOE.Common.Business.WCFServiceLibrary.TMCOptions))]
    [KnownType(typeof(MOE.Common.Business.WCFServiceLibrary.AoROptions))]
    [KnownType(typeof(MOE.Common.Business.WCFServiceLibrary.ApproachDelayOptions))]
    [KnownType(typeof(MOE.Common.Business.WCFServiceLibrary.MetricOptions))]
    [KnownType(typeof(MOE.Common.Business.WCFServiceLibrary.PhaseTerminationOptions))]
    [KnownType(typeof(MOE.Common.Business.WCFServiceLibrary.PreemptDetailOptions))]
    [KnownType(typeof(MOE.Common.Business.WCFServiceLibrary.PreemptServiceMetricOptions))]
    [KnownType(typeof(MOE.Common.Business.WCFServiceLibrary.PreemptServiceRequestOptions))]
    [KnownType(typeof(MOE.Common.Business.WCFServiceLibrary.YellowAndRedOptions))]
    [KnownType(typeof(MOE.Common.Business.WCFServiceLibrary.ApproachSpeedOptions))]
    [KnownType(typeof(MOE.Common.Business.WCFServiceLibrary.SplitFailOptions))]
    [KnownType(typeof(MOE.Common.Business.WCFServiceLibrary.SplitMonitorOptions))]
    [KnownType(typeof(MOE.Common.Business.WCFServiceLibrary.PedDelayOptions))]
    [KnownType(typeof(MOE.Common.Business.WCFServiceLibrary.ApproachVolumeOptions))]
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
        
        public MOE.Common.Models.MetricType MetricType{ get; set; }
        

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
            MetricWebPath = ConfigurationManager.AppSettings["SPMImageLocation"];
            ReturnList = new List<string>(); 
        }
                

        public virtual List<string> CreateMetric()
        {
            

            MOE.Common.Models.Repositories.IMetricTypeRepository metricTypeRepository =
                MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();
            this.MetricType = metricTypeRepository.GetMetricsByID(this.MetricTypeID);
            LogMetricRun();            
            return new List<string>();
        }


        protected void LogMetricRun()
        {
            Models.Repositories.IApplicationEventRepository appEventRepository =
                Models.Repositories.ApplicationEventRepositoryFactory.Create();
            Models.ApplicationEvent applicationEvent = new ApplicationEvent();
            applicationEvent.ApplicationName = "SPM Website";
            applicationEvent.Description = MetricType.ChartName + " Executed";
            applicationEvent.SeverityLevel = ApplicationEvent.SeverityLevels.Low;
            applicationEvent.Timestamp = DateTime.Now;
            appEventRepository.Add(applicationEvent);
        }       

        public string GetSignalLocation()
        {
            MOE.Common.Models.Repositories.ISignalsRepository signalRepository =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();

            return signalRepository.GetSignalLocation(SignalID);
        }

        public string CreateFileName()
        {
            string fileName =  MetricType.Abbreviation +
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

                            return fileName;
        }

        public bool TestDriveAvailable()
        {
            DirectoryInfo di = new DirectoryInfo(MetricFileLocation);

            if(di.Exists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
