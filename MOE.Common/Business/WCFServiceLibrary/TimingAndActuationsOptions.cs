using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class TimingAndActuationsOptions : MetricOptions
    {
        [Display(Name = "Show Plans")]
        [Required]
        [DataMember]
        public bool ShowPlans { get; set; }
        [Display(Name = "Show Vehicle Signal Display")]
        [Required]
        [DataMember]
        public bool ShowVehicleSignalDisplay { get; set; }
        [Display(Name = "Show Pedestrian Intervals")]
        [Required]
        [DataMember]
        public bool ShowPedestrianIntervals { get; set; }
        [Display(Name = "Show Begin Of MaxGreen")]
        [Required]
        [DataMember]
        public bool ShowBeginOfMaxGreen { get; set; }
        [Display(Name = "Show Stop Bar Presence")]
        [Required]
        [DataMember]
        public bool ShowStopBarPresence { get; set; }
        [Display(Name = "Show Lane By Lane Count")]
        [Required]
        [DataMember]
        public bool ShowLaneByLaneCount { get; set; }
        [Display(Name = "Show Advanced Count")]
        [Required]
        [DataMember]
        public bool ShowAdvancedCount { get; set; }
        [Display(Name = "Show Advanced Dilemma Zone")]
        [Required]
        [DataMember]
        public bool ShowAdvancedDilemmaZone { get; set; }
        [Display(Name = "Show Pedestrian Actuation")]
        [Required]
        [DataMember]
        public bool ShowPedestrianActuation { get; set; }
        [Display(Name = "Combine Lanes For Each Group")]
        [Required]
        [DataMember]
        public bool CombineLanesForEachGroup { get; set; }
        [Display(Name = "Show Phase Custom")]
        [Required]
        [DataMember]
        public bool ShowPhaseCustom { get; set; }
        [Display(Name = "Show Global Custom")]
        [Required]
        [DataMember]
        public bool ShowGlobalCustom { get; set; }
        [Display(Name = "Dot And Bar Size")]
        [Required]
        [DataMember]
        public int DotAndBarSize { get; set; }
        [Display(Name = "Phase Custom Code 1")]
        [DataMember]
        public int? PhaseCustomCode1 { get; set; }
        [Display(Name = "Phase Custom Code 2")]
        [DataMember]
        public int? PhaseCustomCode2 { get; set; }
        [Display(Name = "Global Custom Code 1")]
        [DataMember]
        public int? GlobalCustomCode1 { get; set; }
        [Display(Name = "Global Custom Code 2")]
        [DataMember]
        public int? GlobalCustomCode2 { get; set; }


        public TimingAndActuationsOptions(string signalID, DateTime startDate, DateTime endDate, double yAxisMax, double y2AxisMax)
        {
            SignalID = signalID;
            YAxisMax = yAxisMax;
            Y2AxisMax = y2AxisMax;
            MetricTypeID = 6;
            StartDate = startDate;
            EndDate = endDate;
        }

        public TimingAndActuationsOptions()
        {
            MetricTypeID = 17;
            SetDefaults();
        }

        public void SetDefaults()
        {
            YAxisMax = 150;
            Y2AxisMax = 2000;
            MetricTypeID = 6;
        }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            var signalRepository = SignalsRepositoryFactory.Create();
           
            return ReturnList;
        }

        void AddDataToChart(Chart chart, SignalPhase signalPhase)
        {
           
        }
    }
}
