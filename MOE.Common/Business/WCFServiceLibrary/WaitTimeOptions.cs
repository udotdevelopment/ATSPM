using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.UI.DataVisualization.Charting;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class WaitTimeOptions : MetricOptions
    {
        public const int PHASE_BEGIN_GREEN = 1;
        public const int PHASE_END_RED_CLEARANCE = 11;
        public const int PHASE_CALL_REGISTERED = 43;
        public const int PHASE_CALL_DROPPED = 44;

        public WaitTimeOptions(bool showPlanStripes)
        {
            ShowPlanStripes = showPlanStripes;
        }

        public WaitTimeOptions()
        {
            Y2AxisMax = 2000;
            SetDefaults();
        }

        [DataMember]
        [Display(Name = "Show Plan Stripes")]
        public bool ShowPlanStripes { get; set; }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            var signalsRepository = SignalsRepositoryFactory.Create();
            var signal = signalsRepository.GetVersionOfSignalByDate(SignalID, StartDate);
            var analysisPhaseCollection = new AnalysisPhaseCollection(SignalID, StartDate, EndDate);
            foreach (var plan in analysisPhaseCollection.Plans)
            {
                plan.SetProgrammedSplits(SignalID);
                plan.SetHighCycleCount(analysisPhaseCollection);
            }

            var eventLogs = new ControllerEventLogs(SignalID, StartDate, EndDate,
                new List<int>
                    { PHASE_BEGIN_GREEN, PHASE_END_RED_CLEARANCE, PHASE_CALL_REGISTERED, PHASE_CALL_DROPPED });
            foreach (var approach in signal.Approaches.OrderBy(x => x.ProtectedPhaseNumber))
            {
                var phaseInfo =
                    analysisPhaseCollection.Items.FirstOrDefault(x => x.PhaseNumber == approach.ProtectedPhaseNumber);
                CreateChart(approach, eventLogs, signal, phaseInfo, analysisPhaseCollection.Plans);
            }

            return ReturnList;
        }

        public void CreateChart(Approach approach, ControllerEventLogs eventLogs, Signal signal,
            AnalysisPhase phaseInfo, List<PlanSplitMonitor> plans)
        {
            var phaseEvents = eventLogs.Events.Where(x => x.EventParam == approach.ProtectedPhaseNumber);

            if (phaseEvents.Any())
            {
                var waitTimeChart = new WaitTimeChart(this, signal, approach, phaseEvents, StartDate, EndDate,
                    phaseInfo, plans);
                var chart = waitTimeChart.Chart;
                var chartName = CreateFileName();
                chart.SaveImage(MetricFileLocation + chartName);
                ReturnList.Add(MetricWebPath + chartName);
            }

        }

        public Series CreateChartSeries(Color color, SeriesChartType seriesChartType, ChartValueType xValueType,
            AxisType yAxisType, string name)
        {
            return new Series
            {
                ChartType = seriesChartType,
                XValueType = xValueType,
                YAxisType = yAxisType,
                Color = color,
                IsVisibleInLegend = true,
                Name = name
            };
        }

        public void CreateVolumeSeries(Chart chart)
        {
            var volumeSeries = new Series();
            volumeSeries.ChartType = SeriesChartType.Line;
            volumeSeries.Color = Color.Blue;
            volumeSeries.Name = "Volume Per Hour";
            volumeSeries.XValueType = ChartValueType.DateTime;

            volumeSeries.YAxisType = AxisType.Secondary;
            SetSeriesLineWidth(volumeSeries);
            chart.Series.Add(volumeSeries);
        }

        void SetSeriesLineWidth(Series series)
        {
            series.BorderWidth = 1;
        }
    }
}