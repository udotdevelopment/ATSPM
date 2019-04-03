using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.TimingAndActuations;
using MOE.Common.Migrations;
using MOE.Common.Models;
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

        [Display(Name = "Show All Lanes For Each Phase")]
        [Required]
        [DataMember]
        public bool ShowAllLanes { get; set; }

        [Display(Name = "Make an Extra Strip at the End")]
        [Required]
        [DataMember]
        public bool MakeExtraStrip { get; set; }

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

        [Display(Name = "Phase Filter")]
        [DataMember]
        public string PhaseFilter { get; set; }

        public TimingAndActuationsOptions(string signalID, DateTime startDate, DateTime endDate, double yAxisMax,
            double y2AxisMax)
        {
            SignalID = signalID;
            YAxisMax = yAxisMax;
            Y2AxisMax = y2AxisMax;
            MetricTypeID = 17;
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
            DotAndBarSize = 6;
            MetricTypeID = 17;
            ShowAdvancedCount = true;
            ShowAdvancedDilemmaZone = true;
            ShowLaneByLaneCount = true;
            ShowPedestrianActuation = true;
            ShowPedestrianIntervals = true;
            ShowStopBarPresence = true;
            ShowVehicleSignalDisplay = true;
            ShowAllLanes = false;
            MakeExtraStrip = false;

        }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
           List<string> phaseFilters = ExtractPhaseFilters();
            var signalRepository = SignalsRepositoryFactory.Create();
            var signal = signalRepository.GetVersionOfSignalByDateWithDetectionTypes(SignalID, StartDate);
            if (signal.Approaches.Count > 0)
            {
                List<TimingAndActuationsForPhase>
                    timingAndActuationsForPhases = new List<TimingAndActuationsForPhase>();
                foreach (var approach in signal.Approaches)
                {
                    bool permissivePhase;
                    if (approach.PermissivePhaseNumber != null && approach.PermissivePhaseNumber > 0)
                    {
                        if (phaseFilters.Any() && phaseFilters.Contains(approach.PermissivePhaseNumber.ToString())
                           || phaseFilters.Count == 0)
                        {
                            permissivePhase = true;
                            timingAndActuationsForPhases.Add(new TimingAndActuationsForPhase(approach, this,  permissivePhase));
                        }
                    }
                    if (approach.ProtectedPhaseNumber > 0)
                    {
                        if (phaseFilters.Any() && phaseFilters.Contains(approach.ProtectedPhaseNumber.ToString())
                            || phaseFilters.Count == 0)
                        {
                            permissivePhase = false;
                            timingAndActuationsForPhases.Add(new TimingAndActuationsForPhase(approach, this, permissivePhase));
                        }
                    }
                }
                timingAndActuationsForPhases = timingAndActuationsForPhases.OrderBy(t => t.PhaseNumberSort).ToList();
                foreach (var timingAndActutionsForPhase in timingAndActuationsForPhases)
                {
                    GetChart(timingAndActutionsForPhase);
                }
            }
            var timingAndActuationsGlobalGetData = new TimingAndActuationsGlobalGetData(SignalID, this);
            if (!GlobalCustomCode1.HasValue && !GlobalCustomCode2.HasValue) return ReturnList;
            GetGlobalChart(timingAndActuationsGlobalGetData);
            return ReturnList;
        }

        private void GetGlobalChart(TimingAndActuationsGlobalGetData timingAndActuationsGlobalGetData)
        {
            if (ReturnList == null) ReturnList = new List<string>();
            var taaGlobalChart = new TimingAndActuationsChartForGlobalEvents(SignalID, timingAndActuationsGlobalGetData);
            SetDefaults();
            taaGlobalChart.Chart.BackColor = Color.LightSkyBlue;
            var chartName = CreateFileName();
            taaGlobalChart.Chart.ImageLocation = MetricFileLocation + chartName;
            taaGlobalChart.Chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);
            ReturnList.Add(MetricWebPath + chartName);
        }


        private List<string> ExtractPhaseFilters()
        {
            var phases = new List<string>();
            if (String.IsNullOrEmpty(PhaseFilter))
            {
                return phases;
            }
            else
            {
                phases = PhaseFilter.Split(',').ToList();
                return phases;
            }
        }

        private void GetChart(TimingAndActuationsForPhase timingAndActutionsForPhase)
        {
            if (ReturnList == null)
                ReturnList = new List<string>();
            var taaChart = new TimingAndActuationsChartForPhase(timingAndActutionsForPhase);
            SetDefaults();
            if (timingAndActutionsForPhase.GetPermissivePhase)
            {
                taaChart.Chart.BackColor = Color.LightGray;
            }

            var chartName = CreateFileName();
            taaChart.Chart.ImageLocation = MetricFileLocation + chartName;
            taaChart.Chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);
            ReturnList.Add(MetricWebPath + chartName);
        }
    }
}

