using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business;
using MOE.Common.Models;
using System.Runtime.Serialization;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.IO;
using MOE.Common.Business.Bins;
using MOE.Common.Business.DataAggregation;

namespace MOE.Common.Business.WCFServiceLibrary
{
    public abstract class AggregationMetricOptions : MetricOptions
    {
        
        public enum ChartTypes
        {
            Column,
            StackedColumn,
            Line,
            StackedLine
        };

        public enum AggregationOpperations
        {
            Sum,
            Average
        };

        public enum XAxisAggregationSeriesOptions
        {
            Time,
            Direction,
            Approach,
            Signal,
            SignalByDirection,
            Route
        }

        //wouldn't be used because the AggMetrics are incorporated into [MetricTypes]
        public enum AggregationMetrics
        {
            LaneByLaneCounts,
            AdvancedCounts,
            ArrivalonGreen,
            PlatoonRatio,
            SplitFail,
            PedestrianActuations,
            Preemption,
            TSP,
            DataQuality
        }
        public enum AggregationGroups
        {
            Hour,
            Day,
            Month,
            Year,
            None,
            Signal
        }

        public AggregationMetricOptions()
        {
        }

        public Business.Bins.BinFactoryOptions TimeOptions { get; set; }
        public ChartTypes ChartType { get; set; }
        public AggregationOpperations AggregationOpperation { get; set; }
        public XAxisAggregationSeriesOptions XAxisAggregationSeriesOption { get; set; }
        public List<string> SignalIds { get; set; } = new List<string>();
        public List<Models.Signal> Signals { get; set; } = new List<Models.Signal>();
        public List<Models.Approach> Approaches { get; set; } = new List<Models.Approach>();
        public List<Models.Detector> Detectors { get; set; } = new List<Models.Detector>();
        public BinsContainer BinsContainer { get; set; } = new BinsContainer();

        public override List<string> CreateMetric()
        {
            SetMetricType();
            base.CreateMetric();
            GetSignalObjects();
            SetBins();
            GetChartByXAxisAggregation();
            return ReturnList;
        }

        private void SetBins()
        {
            BinsContainer = BinFactory.GetBins(TimeOptions);
        }

        protected void GetSignalObjects()
        {
            if (Signals.Count == 0)
            {
                var signalRepository = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
                foreach (string signalId in SignalIds)
                {
                    Signals.AddRange(signalRepository.GetSignalsBetweenDates(signalId, StartDate, EndDate));
                }
            }
        }

        protected void SetMetricType()
        {
            var metricTypeRepository = MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();
            MetricType = metricTypeRepository.GetMetricsByID(MetricTypeID);
        }

        protected void SaveChartImage(Chart chart)
        {
            string chartName = CreateFileName(MetricType.Abbreviation);
            MetricFileLocation = ConfigurationManager.AppSettings["ImageLocation"];
            MetricWebPath = ConfigurationManager.AppSettings["ImageWebLocation"];
            chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);
            ReturnList.Add(MetricWebPath + chartName);
        }

        protected string CreateFileName(string MetricAbbreviation)
        {
            string fileName = MetricAbbreviation +
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

        private void GetChartByXAxisAggregation()
        {
            switch (XAxisAggregationSeriesOption)
            {
                case XAxisAggregationSeriesOptions.Time:
                    GetTimeCharts();
                    break;
                case XAxisAggregationSeriesOptions.Approach:
                    GetApproachCharts();
                    break;
                case XAxisAggregationSeriesOptions.Direction:
                    GetDirectionCharts();
                    break;
                case XAxisAggregationSeriesOptions.Route:
                    GetRouteCharts();
                    break;
                case XAxisAggregationSeriesOptions.Signal:
                    GetSignalCharts();
                    break;
                case XAxisAggregationSeriesOptions.SignalByDirection:
                    GetSignalByDirectionCharts();
                    break;
                default:
                    GetTimeCharts();
                    break;
            }
        }

        private void GetSignalCharts()
        {
            Chart chart = ChartFactory.CreateStringXIntYChart(this);
            GetSignalAggregateChart(Signals, chart);
            SaveChartImage(chart);
        }

        private void GetSignalByDirectionCharts()
        {
            Chart chart = ChartFactory.CreateStringXIntYChart(this);
            GetSignalByDirectionAggregateChart(Signals, chart);
            SaveChartImage(chart);
        }


        private  void GetRouteCharts()
        {
            Chart chart = ChartFactory.CreateTimeXIntYChart(this);
            GetRouteAggregateChart(Signals, chart);
            SaveChartImage(chart);
        }


        private void GetDirectionCharts()
        {
            Chart chart;
            foreach (var signal in Signals)
            {
                chart = ChartFactory.CreateStringXIntYChart(this);
                GetDirectionAggregateChart(signal, chart);
                SaveChartImage(chart);
            }
        }


        private void GetApproachCharts()
        {
            Chart chart;
            foreach (var signal in Signals)
            {
                chart = ChartFactory.CreateStringXIntYChart(this);
                GetApproachAggregateChart(signal, chart);
                SaveChartImage(chart);
            }

        }

        private void GetTimeCharts()
        {
            Chart chart;
            foreach (var signal in Signals)
            {
                chart = ChartFactory.CreateTimeXIntYChart(this);
                GetTimeAggregateChart(signal, chart);
                SaveChartImage(chart);
            }
        }

        protected abstract void GetRouteAggregateChart(List<Models.Signal> signals, Chart chart);

        protected abstract void GetDirectionAggregateChart(Models.Signal signal, Chart chart);

        protected abstract void GetSignalAggregateChart(List<Models.Signal> signals, Chart chart);

        protected abstract void GetSignalByDirectionAggregateChart(List<Models.Signal> signals, Chart chart);

        protected abstract void GetApproachAggregateChart(Models.Signal signal, Chart chart);

        protected abstract void GetTimeAggregateChart(Models.Signal signal, Chart chart);

        protected Color GetSeriesColorByNumber(int colorNumber)
        {
            switch (colorNumber)
            {
                case 1:
                    return Color.FromArgb(178, 4, 0);

                case 2:
                    return Color.FromArgb(235, 126, 110);

                case 3:
                    return Color.FromArgb(239, 160, 43);

                case 4:
                    return Color.FromArgb(253, 208, 125);

                case 5:
                    return Color.FromArgb(185, 204, 18);

                case 6:
                    return Color.FromArgb(95, 147, 23);

                case 7:
                    return Color.FromArgb(44, 92, 18);

                case 8:
                    return Color.FromArgb(101, 114, 148);

                case 9:
                    return Color.FromArgb(58, 61, 115);

                case 10:
                    return Color.FromArgb(25, 17, 64);

                default:
                    return Color.Black;
            }
        }

        protected void SetSeriestype(Series series)
        {
            switch (ChartType)
            {
                case ChartTypes.StackedColumn:
                    series.ChartType = SeriesChartType.StackedColumn;
                    break;
                case ChartTypes.Line:
                    series.ChartType = SeriesChartType.Line;
                    break;
                case ChartTypes.Column:
                    series.ChartType = SeriesChartType.Column;
                    break;
                case ChartTypes.StackedLine:
                    series.ChartType = SeriesChartType.StackedArea;
                    break;
            }
        }

    }

    






    
}