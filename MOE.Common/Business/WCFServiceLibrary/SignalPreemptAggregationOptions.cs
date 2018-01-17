using System.Collections.Generic;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Models;

namespace MOE.Common.Business.WCFServiceLibrary
{
    public class SignalPreemptAggregationOptions: AggregationMetricOptions


    {

        public SignalPreemptAggregationOptions()
        {
            MetricTypeID = 22;
        }

        protected override void GetSignalByPhaseAggregateCharts(List<Models.Signal> signals, Chart chart)
        {
            MOE.Common.Models.Repositories.IPreemptAggregationDatasRepository repo =
                MOE.Common.Models.Repositories.PreemptAggregationDatasRepositoryFactory.Create();
            
            foreach (var sig in signals)
            {
                List<PreemptionAggregation> aggregations =
                    repo.GetPreemptAggregationByVersionIdAndDateRange(sig.VersionID, this.StartDate, this.EndDate);

                int maxPhase = (from r in aggregations select r.PreemptNumber).Max();

                for (int seriesCounter = 0; seriesCounter < maxPhase; seriesCounter++)
                {
                    Series series = new Series();
                    series.Color = GetSeriesColorByNumber(seriesCounter);
                    series.Name = "Preempt#" + seriesCounter.ToString();
                    series.ChartArea = "ChartArea1";
                    SetSeriestype(series);
                    spliFailAggregationBySignal.GetSplitFailuresByBin(BinsContainer);
                }
            }


        }

        protected override void GetRouteAggregateChart(List<Models.Signal> signals, Chart chart)
        {
            throw new System.NotImplementedException();
        }

        protected override void GetDirectionAggregateChart(Models.Signal signal, Chart chart)
        {
            throw new System.NotImplementedException();
        }

        protected override void GetSignalAggregateChart(List<Models.Signal> signals, Chart chart)
        {
            throw new System.NotImplementedException();
        }

        protected override void GetSignalByDirectionAggregateChart(List<Models.Signal> signals, Chart chart)
        {
            throw new System.NotImplementedException();
        }

        protected override void GetApproachAggregateChart(Models.Signal signal, Chart chart)
        {
            throw new System.NotImplementedException();
        }

        protected override void GetTimeAggregateChart(Models.Signal signal, Chart chart)
        {
            throw new System.NotImplementedException();
        }


    }
}