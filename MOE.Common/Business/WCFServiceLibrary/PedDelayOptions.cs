using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MOE.Common.Business.PEDDelay;
using System.ComponentModel.DataAnnotations;
using MOE.Common.Models;
using System.Linq;
using System.Data;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class PedDelayOptions : MetricOptions
    {
        public PedDelayOptions(string signalId, DateTime startDate, DateTime endDate, int timeBuffer, bool showPedBeginWalk, bool showCycleLength, bool showPercentDelay, double? yAxisMax)
        {
            SignalID = signalId;
            StartDate = startDate;
            EndDate = endDate;
            TimeBuffer = timeBuffer;
            ShowPedBeginWalk = showPedBeginWalk;
            ShowCycleLength = showCycleLength;
            ShowPercentDelay = showPercentDelay;
            YAxisMax = yAxisMax;
        }

        public PedDelayOptions()
        {
            Y2AxisMax = 10;
            SetDefaults();
        }

        [DataMember]
        [Display(Name = "Time Buffer Between Unique Ped Detections")]
        public int TimeBuffer { get; set; }

        [DataMember]
        [Display(Name = "Show Ped Begin Walk")]
        public bool ShowPedBeginWalk { get; set; }

        [DataMember]
        [Display(Name = "Show Cycle Length")]
        public bool ShowCycleLength { get; set; }

        [DataMember]
        [Display(Name = "Show Percent Delay")]
        public bool ShowPercentDelay { get; set; }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            var signalRepository = SignalsRepositoryFactory.Create();
            Signal signal = signalRepository.GetVersionOfSignalByDate(SignalID, StartDate);

            var pedDelaySignal = new PedDelaySignal(signal, TimeBuffer, StartDate, EndDate);

            SPM db = new SPM();
            var cel = ControllerEventLogRepositoryFactory.Create(db);

            foreach (var pedPhase in pedDelaySignal.PedPhases)
                if (pedPhase.Cycles.Count > 0)
                {
                    var approach = signal.Approaches.Where(a => a.ProtectedPhaseNumber == pedPhase.PhaseNumber).FirstOrDefault();
                    var cycleEventNumbers = approach.IsPermissivePhaseOverlap
                        ? new List<int> { 61, 63, 64 }
                        : new List<int> { 1, 8, 9 };
                    var cycleEvents = cel.GetEventsByEventCodesParam(approach.SignalID, StartDate, EndDate.AddSeconds(900),
                        cycleEventNumbers,
                        approach.ProtectedPhaseNumber);
                    var cycleLength = CycleFactory.GetRedToRedCycles(approach, StartDate, EndDate, false, cycleEvents);
                    var pdc = new PEDDelayChart(this, pedPhase, cycleLength);
                    var chart = pdc.Chart;
                    var chartName = CreateFileName();
                    chart.SaveImage(MetricFileLocation + chartName);
                    ReturnList.Add(MetricWebPath + chartName);
                }
            return ReturnList;
        }
    }
}