using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

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
    [KnownType(typeof(SignalAggregationMetricOptions))]
    [KnownType(typeof(ApproachAggregationMetricOptions))]
    [KnownType(typeof(ApproachSplitFailAggregationOptions))]
    [KnownType(typeof(SignalPreemptionAggregationOptions))]
    [KnownType(typeof(SignalPriorityAggregationOptions))]
    [KnownType(typeof(string[]))]
    public class MetricOptions
    {
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
        [Display(Name = "Y-axis Max")]
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

        public MetricType MetricType { get; set; }


        [DataMember]
        public List<string> ReturnList { get; set; }

        public virtual List<string> CreateMetric()
        {
            var metricTypeRepository = MetricTypeRepositoryFactory.Create();
            MetricType = metricTypeRepository.GetMetricsByID(MetricTypeID);
            LogMetricRun();
            return new List<string>();
        }


        protected void LogMetricRun()
        {
            var appEventRepository =
                ApplicationEventRepositoryFactory.Create();
            var applicationEvent = new ApplicationEvent();
            applicationEvent.ApplicationName = "SPM Website";
            applicationEvent.Description = MetricType.ChartName + " Executed";
            applicationEvent.SeverityLevel = ApplicationEvent.SeverityLevels.Low;
            applicationEvent.Timestamp = DateTime.Now;
            appEventRepository.Add(applicationEvent);
        }

        public string GetSignalLocation()
        {
            var signalRepository =
                SignalsRepositoryFactory.Create();
            return signalRepository.GetSignalLocation(SignalID);
        }

        public string CreateFileName()
        {
            if (MetricType == null)
            {
                var metricTypeRepository = MetricTypeRepositoryFactory.Create();
                MetricType = metricTypeRepository.GetMetricsByID(MetricTypeID);
            }

            var fileName = MetricType.Abbreviation +
                           SignalID +
                           "-" +
                           StartDate.Year +
                           StartDate.ToString("MM") +
                           StartDate.ToString("dd") +
                           StartDate.ToString("HH") +
                           StartDate.ToString("mm") +
                           "-" +
                           EndDate.Year +
                           EndDate.ToString("MM") +
                           EndDate.ToString("dd") +
                           EndDate.ToString("HH") +
                           EndDate.ToString("mm-");
            var r = new Random();
            fileName += r.Next().ToString();
            fileName += ".jpg";
            try
            {
                if (DriveAvailable())
                    return fileName;
                return null;
            }
            catch
            {
                throw new Exception("Path not found");
            }
        }

        public bool DriveAvailable()
        {
            var di = new DirectoryInfo(MetricFileLocation);
            di.Refresh();
            if (di.Exists)
                return true;
            Directory.CreateDirectory(MetricFileLocation);
            di.Refresh();
            if (di.Exists)
                return true;
            return false;
        }
    }
}