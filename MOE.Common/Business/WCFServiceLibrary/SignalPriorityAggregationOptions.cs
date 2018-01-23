using System.Collections.Generic;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.DataAggregation;

namespace MOE.Common.Business.WCFServiceLibrary
{
    public class SignalPriorityAggregationOptions : AggregationMetricOptions
    {
        

        public SignalPriorityAggregationOptions()
        {
            MetricTypeID = 24;
        }

        protected override void GetSignalByPhaseAggregateCharts(List<Models.Signal> signals, Chart chart)
        {
            MOE.Common.Models.Repositories.IPriorityAggregationDatasRepository repo =
                MOE.Common.Models.Repositories.PriorityAggregationDatasRepositoryFactory.Create();




            //int maxPhase = (from r in aggregations select r.PriorityNumber).Max();

            for (int seriesCounter = 1; seriesCounter < 16; seriesCounter++)
            {
                Series series = new Series();
                series.Color = GetSeriesColorByNumber(seriesCounter);
                series.Name = "Priority#" + seriesCounter.ToString();
                series.ChartArea = "ChartArea1";
                SetSeriestype(series);
                chart.Series.Add(series);
            }


            foreach (var signal in signals)
            {
                for (int seriesCounter = 1; seriesCounter < 16; seriesCounter++)
                {
                    PriorityAggregationBySignal priorityAggregationBySignal =
                        new PriorityAggregationBySignal(this, signal, BinsContainer);

                    priorityAggregationBySignal.GetPriorityTotalsBySignalByPriorityNumber(BinsContainer, seriesCounter);

                    foreach (var totals in priorityAggregationBySignal.PriorityTotals)
                    {
                        foreach (var bin in totals.BinsContainer.Bins)
                        {
                            chart.Series[seriesCounter - 1].Points.AddXY(signal.SignalID, bin.Value);
                        }
                    }
                }
            }





        }

        protected override void GetRouteAggregateChart(List<Models.Signal> signals, Chart chart)
        {
            MOE.Common.Models.Repositories.IPriorityAggregationDatasRepository repo =
                MOE.Common.Models.Repositories.PriorityAggregationDatasRepositoryFactory.Create();


            int i = 1;
            foreach (var signal in signals)
            {
                Series series = new Series();
                series.Color = GetSeriesColorByNumber(i);
                series.Name = signal.SignalID;
                series.ChartArea = "ChartArea1";
                SetSeriestype(series);
                chart.Series.Add(series);

                PriorityAggregationBySignal priorityAggregationBySignal =
                        new PriorityAggregationBySignal(this, signal, BinsContainer);

                foreach (var totals in priorityAggregationBySignal.PriorityTotals)
                {
                    foreach (var bin in totals.BinsContainer.Bins)
                    {
                        series.Points.AddXY(bin.Start, bin.Value);
                    }
                }

                i++;
            }
        }

        protected override void GetDirectionAggregateChart(Models.Signal signal, Chart chart)
        {

        }

        protected override void GetSignalAggregateChart(List<Models.Signal> signals, Chart chart)
        {
            throw new System.NotImplementedException();
        }

        protected override void GetSignalByDirectionAggregateChart(List<Models.Signal> signals, Chart chart)
        {
            MOE.Common.Models.Repositories.IPriorityAggregationDatasRepository repo =
                MOE.Common.Models.Repositories.PriorityAggregationDatasRepositoryFactory.Create();


            int i = 1;
            foreach (var signal in signals)
            {
                Series series = new Series();
                series.Color = GetSeriesColorByNumber(i);
                series.Name = signal.SignalID;
                series.ChartArea = "ChartArea1";
                SetSeriestype(series);
                chart.Series.Add(series);

                PriorityAggregationBySignal priorityAggregationBySignal =
                    new PriorityAggregationBySignal(this, signal, BinsContainer);

                foreach (var totals in priorityAggregationBySignal.PriorityTotals)
                {
                    foreach (var bin in totals.BinsContainer.Bins)
                    {
                        series.Points.AddXY(bin.Start, bin.Value);
                    }
                }

                i++;
            }
        }

        protected override void GetApproachAggregateChart(Models.Signal signal, Chart chart)
        {

        }

        protected override void GetTimeAggregateChart(Models.Signal signal, Chart chart)
        {



            int i = 1;


            Series series = new Series();
            series.Color = GetSeriesColorByNumber(i);
            series.Name = signal.SignalID;
            series.ChartArea = "ChartArea1";
            series.BorderWidth = 2;
            SetSeriestype(series);
            chart.Series.Add(series);
            i++;


            PriorityAggregationBySignal priorityAggregationBySignal =
                new PriorityAggregationBySignal(this, signal, BinsContainer);

            priorityAggregationBySignal.GetPriorityByBin(BinsContainer);

            foreach (var p in priorityAggregationBySignal.PriorityTotals)
            {


                if (AggregationOpperation == AggregationMetricOptions.AggregationOpperations.Sum)
                {
                    foreach (var bin in p.BinsContainer.Bins)
                    {
                        series.Points.AddXY(bin.Start, bin.Value);
                    }
                }


            }


        }
    }
}