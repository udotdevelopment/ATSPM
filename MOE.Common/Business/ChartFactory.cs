using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using System.Xml;
using System.Xml.Linq;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using Newtonsoft.Json;

namespace MOE.Common.Business
{
    public static class ChartFactory
    {
        private static Random _rnd = new Random();

        private static readonly List<Series> SeriesList = new List<Series>();
        private static List<Bin> Bins;

        public static List<Bin> GetBins(SignalAggregationMetricOptions options)
        {
            var bins = new List<Bin>();

            var timeX = new DateTime();

            timeX = options.StartDate;

            while (timeX <= options.EndDate)
            {
                var bin = new Bin();

                bin.Start = timeX;

                //timeX = timeX.AddMinutes(options.BinSize);

                bin.End = timeX;

                bins.Add(bin);
            }

            return bins;
        }

        public static void AddSeriesToSeriesList(Series series)
        {
            var checkSeries = (from r in SeriesList
                where series.ChartType != r.ChartType
                select r).ToList();

            if (checkSeries == null || checkSeries.Count == 0)
                SeriesList.Add(series);
        }


        public static Chart ChartInitialization(MetricOptions options)
        {
            var chart = new Chart();
            SetImageProperties(chart);
            chart.ChartAreas.Add(CreateChartArea(options));
            return chart;
        }

        public static Chart ChartInitialization(SignalAggregationMetricOptions options)
        {
            var chart = new Chart();
            SetImageProperties(chart);
            ChartArea chartAreaAgg = CreateTimeXIntYChartArea(options);
            chart.ChartAreas.Add(chartAreaAgg);
            chart.Titles.Add(options.ChartTitle);
            if (options.ShowEventCount)
            {
                SetUpY2Axis(chartAreaAgg, options);
            }
            return chart;
        }

        public static Chart CreateDefaultChartNoX2AxisNoY2Axis(MetricOptions options)
        {
            var chart = new Chart();
            SetImageProperties(chart);
            chart.ChartAreas.Add(CreateChartAreaNoX2AxisNoY2Axis(options));
            return chart;
        }

        public static Chart CreateDefaultChartNoX2Axis(MetricOptions options)
        {
            var chart = new Chart();
            SetImageProperties(chart);
            chart.ChartAreas.Add(CreateChartAreaNoX2Axis(options));
            return chart;
        }

        public static Chart CreateDefaultChart(MetricOptions options)
        {
            var chart = new Chart();
            SetImageProperties(chart);
            chart.ChartAreas.Add(CreateChartArea(options));
            return chart;
        }

        public static Chart CreateSplitFailureChart(SplitFailOptions options)
        {
            var chart = new Chart();
            SetImageProperties(chart);
            chart.ChartAreas.Add(CreateSplitFailChartArea(options));
            SetLegend(chart);
            return chart;
        }


        //public static Chart CreateSplitMonitorChart(SplitMonitorOptions options)
        //{
        //    var chart = new Chart();
        //    SetImageProperties(chart);
        //    chart.ChartAreas.Add(CreateSplitMonitorChartArea(options));
        //    SetLegend(chart);
        //    return chart;
        //}

        //private static ChartArea CreateSplitMonitorChartArea(SplitMonitorOptions options)
        //{
        //    var chartArea = new ChartArea();
        //    chartArea.Name = "ChartArea1";
        //    SetUpXAxis(chartArea, options);
        //    SetUpX2Axis(chartArea, options);
        //    return chartArea;
        //}

        public static Chart CreateTimeXIntYChart(SignalAggregationMetricOptions options, List<Models.Signal> signals)
        {
            var chart = new Chart();
            SetImageProperties(chart);
            chart.ChartAreas.Add(CreateTimeXIntYChartArea(options));
            SetLegend(chart);
            var signalDescriptions = string.Empty;
            foreach (var signal in signals)
                signalDescriptions += signal.SignalDescription + ",";
            signalDescriptions = signalDescriptions.TrimEnd(',');
            chart.Titles.Add(signalDescriptions + "\n" + options.ChartTitle);
            return chart;
        }

        public static Chart CreateLaneByLaneAggregationChart(SignalAggregationMetricOptions options)
        {
            options.MetricTypeID = 16;
            var chart = ChartInitialization(options);
            Bins = GetBins(options);
            return chart;
        }

        public static Chart CreateAdvancedCountsAggregationChart(SignalAggregationMetricOptions options)
        {
            options.MetricTypeID = 17;
            var chart = ChartInitialization(options);
            Bins = GetBins(options);

            return chart;
        }

        public static Chart CreateArrivalOnGreenAggregationChart(SignalAggregationMetricOptions options)
        {
            options.MetricTypeID = 18;
            var chart = ChartInitialization(options);
            Bins = GetBins(options);
            return chart;
        }

        public static Chart CreatePlatoonRatioAggregationChart(SignalAggregationMetricOptions options)
        {
            options.MetricTypeID = 19;
            var chart = ChartInitialization(options);
            Bins = GetBins(options);
            return chart;
        }
        
        private static void PopulateBinsWithSplitFailAggregateSums(List<ApproachSplitFailAggregation> records)
        {
            foreach (var bin in Bins)
            {
                var recordsForBins = from r in records
                    where r.BinStartTime >= bin.Start && r.BinStartTime < bin.End
                    select r;

                bin.Sum = recordsForBins.Sum(s => s.SplitFailures);
            }
        }

        private static void PopulateBinsWithSplitFailAggregateAverages(List<ApproachSplitFailAggregation> records)
        {
            foreach (var bin in Bins)
            {
                var recordsForBins = from r in records
                    where r.BinStartTime >= bin.Start && r.BinStartTime < bin.End
                    select r;

                bin.Sum = Convert.ToInt32(recordsForBins.Average(s => s.SplitFailures));
            }
        }

        public static List<ApproachSplitFailAggregation> GetApproachAggregationRecords(Approach approach,
            SignalAggregationMetricOptions options)
        {
            var Repo = ApproachSplitFailAggregationRepositoryFactory.Create();
            if (approach != null)
            {
                //List<ApproachSplitFailAggregation> aggregations =
                //    Repo.GetApproachSplitFailAggregationByApproachIdAndDateRange(
                //        approach.ApproachID, options.StartDate, options.EndDate);
                //return aggregations;
            }
            return null;
        }

        public static Chart CreatePedestrianActuationAggregationChart(SignalAggregationMetricOptions options)
        {
            options.MetricTypeID = 21;
            var chart = ChartInitialization(options);
            Bins = GetBins(options);
            return chart;
        }

        public static Chart PreemptionAggregationChart(SignalAggregationMetricOptions options)
        {
            options.MetricTypeID = 22;
            var chart = ChartInitialization(options);
            Bins = GetBins(options);
            return chart;
        }

        public static Chart CreateApproachDelayAggregationChart(SignalAggregationMetricOptions options)
        {
            options.MetricTypeID = 23;
            var chart = ChartInitialization(options);
            Bins = GetBins(options);
            return chart;
        }


        public static Chart TransitSignalPriorityAggregationChart(SignalAggregationMetricOptions options)
        {
            options.MetricTypeID = 24;
            var chart = ChartInitialization(options);
            return chart;
        }

        public static Series GetSeriesFromBins()
        {
            var s = new Series();
            foreach (var bin in Bins)
                s.Points.AddXY(bin.Start, bin.Sum);

            return s;
        }

        private static void SetLegend(Chart chart)
        {
            var chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            chart.Legends.Add(chartLegend);
        }

        private static ChartArea CreateSplitFailChartArea(SplitFailOptions options)
        {
            var chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            SetSplitFailYAxis(chartArea, options);
            SetUpXAxis(chartArea, options);
            SetSplitFailPassX2Axis(chartArea, options);
            return chartArea;
        }

        private static ChartArea CreateTimeXIntYChartArea(SignalAggregationMetricOptions options)
        {
            var chartArea = new ChartArea();
            SetDimension(options, chartArea);
            chartArea.Name = "ChartArea1";
            SetIntYAxis(chartArea, options);
            if (options.ShowEventCount)
            {
                SetIntY2Axis(chartArea, options);
            }
            SetTimeXAxis(chartArea, options);
            return chartArea;
        }


        private static void SetSplitFailX2Axis(ChartArea chartArea, SplitFailOptions options)
        {
            var reportTimespan = options.EndDate - options.StartDate;
            chartArea.AxisX2.LabelStyle.Format = "HH";
            if (reportTimespan.Days < 1)
                if (reportTimespan.Hours > 1)
                    chartArea.AxisX2.Interval = 1;
                else
                    chartArea.AxisX2.LabelStyle.Format = "HH:mm";
            chartArea.AxisX2.Minimum = options.StartDate.ToOADate();
            chartArea.AxisX2.Maximum = options.EndDate.ToOADate();
            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisX2.MajorTickMark.Enabled = true;
            chartArea.AxisX2.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX2.LabelAutoFitStyle = LabelAutoFitStyles.None;
        }

        private static void SetSplitFailXAxis(ChartArea chartArea, SplitFailOptions options)
        {
            var reportTimespan = options.EndDate - options.StartDate;
            chartArea.AxisX.Title = "Time (Hour of Day)";
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.LabelStyle.Format = "HH";
            chartArea.AxisX.Minimum = options.StartDate.ToOADate();
            chartArea.AxisX.Maximum = options.EndDate.ToOADate();
            if (reportTimespan.Days < 1)
                if (reportTimespan.Hours > 1)
                    chartArea.AxisX.Interval = 1;
                else
                    chartArea.AxisX.LabelStyle.Format = "HH:mm";
        }

        private static void SetSplitFailYAxis(ChartArea chartArea, SplitFailOptions options)
        {
            if (options.YAxisMax != null)
                chartArea.AxisY.Maximum = options.YAxisMax.Value;
            else
                chartArea.AxisY.Maximum = 100;
            chartArea.AxisY.Title = "Occupancy Ratio (percent)";
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Interval = 10;
        }

        private static void SetIntYAxis(ChartArea chartArea, SignalAggregationMetricOptions options)
        {
            if (options.YAxisMax != null)
                chartArea.AxisY.Maximum = options.YAxisMax.Value;
            else
            chartArea.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartArea.AxisY.Title = options.YAxisTitle;
            chartArea.AxisY.Minimum = 0;
        }

        private static void SetIntY2Axis(ChartArea chartArea, SignalAggregationMetricOptions options)
        {
            chartArea.AxisY2.Enabled = AxisEnabled.True;
            chartArea.AxisY2.MajorGrid.Enabled = false;
            chartArea.AxisY2.MinorGrid.Enabled = false;
            chartArea.AxisY2.Title = options.Y2AxisTitle;
            chartArea.AxisY2.Minimum = 0;
        }

        private static void SetTimeXAxis(ChartArea chartArea, SignalAggregationMetricOptions options)
        {
            //var reportTimespan = options.EndDate - options.StartDate;
            chartArea.AxisX.Title = "Time (Hour of Day)";
            chartArea.AxisX.LabelStyle.IsEndLabelVisible = false;
            chartArea.AxisX.Interval = 1;
            if (options.SelectedXAxisType == XAxisType.TimeOfDay)
            {
                switch (options.TimeOptions.SelectedBinSize)
                {
                    case BinFactoryOptions.BinSize.Year:
                    {
                        chartArea.AxisX.IntervalType = DateTimeIntervalType.Years;
                        chartArea.AxisX.LabelStyle.Format = "yyyy";
                        chartArea.AxisX.Title = "Day of Month";
                        chartArea.AxisX.Minimum = options.TimeOptions.Start.AddYears(-1).ToOADate();
                        chartArea.AxisX.Maximum = options.TimeOptions.End.AddYears(1).ToOADate();
                        break;
                    }
                    case BinFactoryOptions.BinSize.Month:
                    {
                        chartArea.AxisX.IntervalType = DateTimeIntervalType.Months;
                        chartArea.AxisX.LabelStyle.Format = "MM/yy";
                        chartArea.AxisX.Title = "Day of Month";
                        chartArea.AxisX.Minimum = options.TimeOptions.Start.AddMonths(-1).ToOADate();
                        chartArea.AxisX.Maximum = options.TimeOptions.End.AddMonths(1).ToOADate();
                        break;
                    }
                    case BinFactoryOptions.BinSize.Day:
                    {
                        chartArea.AxisX.IntervalType = DateTimeIntervalType.Days;
                        chartArea.AxisX.LabelStyle.Format = "MM/dd/yy";
                        chartArea.AxisX.Title = "Day of Month";
                        chartArea.AxisX.Minimum = options.TimeOptions.Start.AddDays(-1).ToOADate();
                        chartArea.AxisX.Maximum = options.TimeOptions.End.AddDays(1).ToOADate();
                        break;
                    }
                    default:
                    {
                        chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
                        chartArea.AxisX.LabelStyle.Format = "HH";
                        chartArea.AxisX.Minimum = options.TimeOptions.Start.AddHours(-1).ToOADate();
                        chartArea.AxisX.Maximum = new DateTime(options.TimeOptions.Start.Year,
                                options.TimeOptions.Start.Month, options.TimeOptions.Start.Day,
                                options.TimeOptions.TimeOfDayEndHour.Value,
                                options.TimeOptions.TimeOfDayEndMinute.Value, 0)
                            .AddHours(1).ToOADate();
                            break;
                    }
                }
            }
            else
            {
                switch (options.TimeOptions.SelectedBinSize)
                {
                    case BinFactoryOptions.BinSize.FifteenMinute:
                        chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
                        chartArea.AxisX.LabelStyle.Format = "HH:mm";
                        chartArea.AxisX.Minimum = options.TimeOptions.Start.AddMinutes(-15).ToOADate();
                        chartArea.AxisX.Maximum = options.TimeOptions.End.AddMinutes(15).ToOADate();
                        break;
                    case BinFactoryOptions.BinSize.ThirtyMinute:
                        chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
                        chartArea.AxisX.LabelStyle.Format = "HH:mm";
                        chartArea.AxisX.Minimum = options.TimeOptions.Start.AddMinutes(-30).ToOADate();
                        chartArea.AxisX.Maximum = options.TimeOptions.End.AddMinutes(30).ToOADate();
                        break;
                    case BinFactoryOptions.BinSize.Hour:
                        chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
                        chartArea.AxisX.LabelStyle.Format = "HH";
                        chartArea.AxisX.Minimum = options.TimeOptions.Start.AddHours(-1).ToOADate();
                        chartArea.AxisX.Maximum = options.TimeOptions.End.AddHours(1).ToOADate();
                        break;
                    case BinFactoryOptions.BinSize.Day:
                        chartArea.AxisX.IntervalType = DateTimeIntervalType.Days;
                        chartArea.AxisX.LabelStyle.Format = "MM/dd/yy";
                        chartArea.AxisX.Title = "Day of Month";
                        chartArea.AxisX.Minimum = options.TimeOptions.Start.AddDays(-1).ToOADate();
                        chartArea.AxisX.Maximum = options.TimeOptions.End.AddDays(1).ToOADate();
                        break;
                    //case BinFactoryOptions.BinSize.Week:
                    //    chartArea.AxisX.IntervalType = DateTimeIntervalType.Weeks;
                    //    chartArea.AxisX.LabelStyle.Format = "";
                    //    chartArea.AxisX.Title = "Start of Week";
                    //    chartArea.AxisX.Minimum = options.StartDate.AddDays(-7).ToOADate();
                    //    break;
                    case BinFactoryOptions.BinSize.Month:
                        chartArea.AxisX.MinorTickMark.Enabled = true;
                        chartArea.AxisX.MajorTickMark.Enabled = true;
                        chartArea.AxisX.IntervalType = DateTimeIntervalType.Months;
                        chartArea.AxisX.Interval = 1;
                        chartArea.AxisX.LabelStyle.Enabled = true;
                        chartArea.AxisX.LabelStyle.Angle = 45;
                        chartArea.AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Months;
                        chartArea.AxisX.LabelStyle.Interval = 1;
                        chartArea.AxisX.Title = "Month and Year";
                        chartArea.AxisX.Minimum = options.TimeOptions.Start.AddMonths(-1).ToOADate();
                        chartArea.AxisX.Maximum = options.TimeOptions.End.AddMonths(1).ToOADate();
                        chartArea.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.DecreaseFont;
                        chartArea.AxisX.LabelStyle.Enabled = true;
                        break;
                    case BinFactoryOptions.BinSize.Year:
                        chartArea.AxisX.IntervalType = DateTimeIntervalType.Years;
                        chartArea.AxisX.LabelStyle.Format = "yyyy";
                        chartArea.AxisX.Title = "Year";
                        chartArea.AxisX.Minimum = options.TimeOptions.Start.AddYears(-1).ToOADate();
                        chartArea.AxisX.Maximum = options.TimeOptions.End.AddYears(1).ToOADate();
                        break;
                    default:
                        chartArea.AxisX.IntervalType = DateTimeIntervalType.Days;
                        chartArea.AxisX.LabelStyle.Format = "MM/dd/yy";
                        break;
                }

                if (options.TimeOptions.TimeOption == BinFactoryOptions.TimeOptions.TimePeriod &&
                    (options.TimeOptions.SelectedBinSize == BinFactoryOptions.BinSize.FifteenMinute ||
                     options.TimeOptions.SelectedBinSize == BinFactoryOptions.BinSize.ThirtyMinute ||
                     options.TimeOptions.SelectedBinSize == BinFactoryOptions.BinSize.Hour))
                {
                    var tempStart = new DateTime(options.TimeOptions.Start.Year, options.TimeOptions.Start.Month,
                        options.TimeOptions.Start.Day, options.TimeOptions.TimeOfDayStartHour ?? 0,
                        options.TimeOptions.TimeOfDayStartMinute ?? 0, 0);
                    var tempEnd = new DateTime(options.TimeOptions.Start.Year, options.TimeOptions.Start.Month,
                        options.TimeOptions.Start.Day, options.TimeOptions.TimeOfDayEndHour ?? 0,
                        options.TimeOptions.TimeOfDayEndMinute ?? 0, 0);
                    chartArea.AxisX.Minimum = tempStart.AddMinutes(-15).ToOADate();
                    chartArea.AxisX.Maximum = tempEnd.ToOADate();
                }
            }
        }

        public static void SetImageProperties(Chart chart)
        {
            chart.ImageType = ChartImageType.Jpeg;
            chart.Height = 720;
            chart.Width = 1400;
            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
        }

        public static Chart CreateTAALegendChart()
        {
            var chart = new Chart();
            SetImageProperties(chart, 720, 700);
            var chartArea = new ChartArea
            {
                Name = "ChartArea1"
            };
            chart.ChartAreas.Add(chartArea);
            chartArea.AxisX.Minimum = 0;
            chartArea.AxisX.Maximum = 1;
            chartArea.AxisX.LabelStyle.Interval = 1;
            chartArea.AxisY.LabelStyle.Interval = 1;
            return chart;
        }

        public static void SetImageProperties(Chart chart, int width, int height)
        {
            chart.ImageType = ChartImageType.Jpeg;
            chart.Height = height;
            chart.Width = width;
            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
        }

        private static ChartArea CreateChartAreaNoX2AxisNoY2Axis(MetricOptions options)
        {
            var chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            SetUpYAxis(chartArea, options);
            SetUpXAxis(chartArea, options);
            SetUpX2AxisNoLabels(chartArea, options);
            return chartArea;
        }

        private static ChartArea CreateChartAreaNoX2Axis(MetricOptions options)
        {
            var chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            SetUpYAxis(chartArea, options);
            SetUpY2Axis(chartArea, options);
            SetUpXAxis(chartArea, options);
            SetUpX2AxisNoLabels(chartArea, options);
            return chartArea;
        }

        private static ChartArea CreateChartArea(MetricOptions options)
        {
            var chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            SetUpYAxis(chartArea, options);
            SetUpY2Axis(chartArea, options);
            SetUpXAxis(chartArea, options);
            SetUpX2Axis(chartArea, options);
            return chartArea;
        }

        private static void SetUpX2AxisNoLabels(ChartArea chartArea, MetricOptions options)
        {
            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisX2.MajorGrid.Enabled = false;
            chartArea.AxisX2.MajorTickMark.Enabled = false;
            chartArea.AxisX2.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX2.Minimum = options.StartDate.ToOADate();
            chartArea.AxisX2.Maximum = options.EndDate.ToOADate();
            chartArea.AxisX2.CustomLabels.Add(options.StartDate.ToOADate(), options.EndDate.ToOADate(), "");
        }

        private static void SetUpX2Axis(ChartArea chartArea, MetricOptions options)
        {
            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisX2.Minimum = options.StartDate.ToOADate();
            chartArea.AxisX2.Maximum = options.EndDate.ToOADate();
            var reportTimespan = options.EndDate - options.StartDate;
            double totalMinutesRounded = Math.Round(reportTimespan.TotalMinutes);
            if (totalMinutesRounded <= 1)
            {
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Seconds;
                chartArea.AxisX.Interval = 2;
                chartArea.AxisX.LabelStyle.Format = "HH:mm:ss";
                chartArea.AxisX.Title = "Time (Hours:Minutes:Seconds)";
            }
            else if (totalMinutesRounded > 1.0 && totalMinutesRounded <= 3.0)
            {
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Seconds;
                chartArea.AxisX.Interval = 3;
                chartArea.AxisX.LabelStyle.Format = "HH:mm:ss";
                chartArea.AxisX.Title = "Time (Hours:Minutes:Seconds)";
            }
            else if (totalMinutesRounded > 3.0 && totalMinutesRounded <= 6.0)
            {
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Seconds;
                chartArea.AxisX.Interval = 15;
                chartArea.AxisX.LabelStyle.Format = "HH:mm:ss";
                chartArea.AxisX.Title = "Time (Hours:Minutes:Seconds)";
            }
            else if (totalMinutesRounded > 6.0 && totalMinutesRounded <= 10.0)
            {
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Seconds;
                chartArea.AxisX.Interval = 30;
                chartArea.AxisX.LabelStyle.Format = "HH:mm:ss";
                chartArea.AxisX.Title = "Time (Hours:Minutes:Seconds)";
            }
            else if (totalMinutesRounded > 10.0 && totalMinutesRounded <= 1.0 * 60.0)
            {
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Minutes;
                chartArea.AxisX.Interval = 2;
                chartArea.AxisX.LabelStyle.Format = "HH:mm";
                chartArea.AxisX.Title = "Time (Hours:Minutes)";
            }
            else if (totalMinutesRounded > 1.0 * 60.0 && totalMinutesRounded <= 2.0 * 60.0)
            {
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Minutes;
                chartArea.AxisX.Interval = 5;
                chartArea.AxisX.LabelStyle.Format = "HH:mm";
                chartArea.AxisX.Title = "Time (Hours:Minutes)";
            }
            else if (totalMinutesRounded > 2.0 * 60.0 && totalMinutesRounded <= 24.0 * 60.0)
            {
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
                chartArea.AxisX.Interval = 1;
                chartArea.AxisX.LabelStyle.Format = "HH:mm";
                chartArea.AxisX.Title = "Time (Hours:Minutes)";
            }
            else if (totalMinutesRounded > 24.0 * 60.0 && totalMinutesRounded <= 48.0 * 60.0)
            {
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
                chartArea.AxisX.Interval = 12;
                chartArea.AxisX.LabelStyle.Format = "HH";
                chartArea.AxisX.Title = "Time (Hours of Day)";
            }
            else if (totalMinutesRounded > 48.0 * 60.0)
            {
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
                chartArea.AxisX.Interval = 24;
                chartArea.AxisX.LabelStyle.Format = "MM/dd/yyyy";
                chartArea.AxisX.Title = "Time (Days)";
            }
        }
        private static void SetSplitFailPassX2Axis(ChartArea chartArea, MetricOptions options)
        {
            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisX2.IntervalType = DateTimeIntervalType.Days;
            chartArea.AxisX2.LabelStyle.Format = "  ";
            chartArea.AxisX2.Minimum = options.StartDate.ToOADate();
            chartArea.AxisX2.Maximum = options.EndDate.ToOADate();
            chartArea.AxisX2.Interval = 30;
            }

        private static void SetUpXAxis(ChartArea chartArea, MetricOptions options)
        {
            chartArea.AxisX.Minimum = options.StartDate.ToOADate();
            chartArea.AxisX.Maximum = options.EndDate.ToOADate();
            var reportTimespan = options.EndDate - options.StartDate;
            double totalMinutesRounded = Math.Round(reportTimespan.TotalMinutes);
            if (totalMinutesRounded <= 1)
            {
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Seconds;
                chartArea.AxisX.Interval = 1;
                chartArea.AxisX.LabelStyle.Format = "HH:mm:ss";
                chartArea.AxisX.Title = "Time (Hours:Minutes:Seconds of Day)";
            }
            else if(totalMinutesRounded > 1.0 && totalMinutesRounded <= 3.0)
            {
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Seconds;
                chartArea.AxisX.Interval = 3;
                chartArea.AxisX.LabelStyle.Format = "HH:mm:ss";
                chartArea.AxisX.Title = "Time (Hours:Minutes:Seconds of Day)";
            }
            else if (totalMinutesRounded > 3.0 && totalMinutesRounded <= 6.0)
            {
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Seconds;
                chartArea.AxisX.Interval =15;
                chartArea.AxisX.LabelStyle.Format = "HH:mm:ss";
                chartArea.AxisX.Title = "Time (Hours:Minutes:Seconds of Day)";
            }
            else if (totalMinutesRounded > 6.0 && totalMinutesRounded <= 10.0)
            {
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Seconds;
                chartArea.AxisX.Interval = 30;
                chartArea.AxisX.LabelStyle.Format = "HH:mm:ss";
                chartArea.AxisX.Title = "Time (Hours:Minutes:Seconds of Day)";
            }
            else if (totalMinutesRounded > 10.0 && totalMinutesRounded <= 1.0 * 60.0)
            {
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Minutes;
                chartArea.AxisX.Interval = 5;
                chartArea.AxisX.LabelStyle.Format = "HH:mm";
                chartArea.AxisX.Title = "Time (Hours:Minutes of Day)";
            }
            else if (totalMinutesRounded > 1.0 * 60.0 && totalMinutesRounded <= 2.0 * 60.0)
            {
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Minutes;
                chartArea.AxisX.Interval = 15;
                chartArea.AxisX.LabelStyle.Format = "HH:mm";
                chartArea.AxisX.Title = "Time (Hours:Minutes of Day)";
            }
            else if (totalMinutesRounded > 120 && totalMinutesRounded <= 1441)
            {
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
                chartArea.AxisX.Interval = 1;
                chartArea.AxisX.LabelStyle.Format = "HH:mm";
                chartArea.AxisX.Title = "Time (Hours:Minutes of Day)";
            }
            else if (totalMinutesRounded > 24.0 * 60.0 && totalMinutesRounded <= 48.0 * 60.0)
            {
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
                chartArea.AxisX.Interval = 12;
                chartArea.AxisX.LabelStyle.Format = "HH";
                chartArea.AxisX.Title = "Time (Hours of Day)";
            }
            else if (totalMinutesRounded > 48.0 * 60.0)
            {
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
                chartArea.AxisX.Interval = 24;
                chartArea.AxisX.LabelStyle.Format = "MM/dd/yyyy";
                chartArea.AxisX.Title = "Time (Days)";
            }
        }

        private static void SetUpY2Axis(ChartArea chartArea, MetricOptions options)
        {
            if (options.Y2AxisMax != null)
                chartArea.AxisY2.Maximum = options.Y2AxisMax.Value;
            chartArea.AxisY2.Enabled = AxisEnabled.True;
            chartArea.AxisY2.MajorTickMark.Enabled = false;
            chartArea.AxisY2.MajorGrid.Enabled = false;
            chartArea.AxisY2.IntervalType = DateTimeIntervalType.Number;
            chartArea.AxisY2.TitleFont = new Font("Microsoft Sans Serif", 9);
            chartArea.AxisY2.Title = options.Y2AxisTitle??"";
        }
        private static void SetUpY2Axis(ChartArea chartArea, SignalAggregationMetricOptions options)
        {
            chartArea.AxisY2.MaximumAutoSize = 50;
            chartArea.AxisY2.Enabled = AxisEnabled.True;
            chartArea.AxisY2.MajorTickMark.Enabled = false;
            chartArea.AxisY2.MajorGrid.Enabled = false;
            chartArea.AxisY2.IntervalType = DateTimeIntervalType.Number;
            chartArea.AxisY2.TitleFont = new Font("Microsoft Sans Serif", 9);
            chartArea.AxisY2.Title = options.Y2AxisTitle ?? "";
        }

        private static void SetUpYAxis(ChartArea chartArea, MetricOptions options)
        {
            if (options.YAxisMax != null)
                chartArea.AxisY.Maximum = options.YAxisMax.Value;
            chartArea.AxisY.Title = "Cycle Time (Seconds) ";
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.IntervalType = DateTimeIntervalType.Number;
        }

        public static Series CreateLineSeries(string seriesName, Color seriesColor)
        {
            var s = new Series();
            s.ChartType = SeriesChartType.Line;
            s.Color = seriesColor;
            return s;
        }

        public static Series CreateStackedAreaSeries(string seriesName, Color seriesColor)
        {
            var s = new Series();
            s.ChartType = SeriesChartType.StackedArea;
            s.Color = seriesColor;
            return s;
        }

        public static Series CreateColumnSeries(string seriesName, Color seriesColor)
        {
            var s = new Series();
            s.ChartType = SeriesChartType.Column;
            s.Color = seriesColor;
            return s;
        }

        public static Series CreateStackedColumnSeries(string seriesName, Color seriesColor)
        {
            var s = new Series();
            s.ChartType = SeriesChartType.StackedColumn;
            s.Color = seriesColor;
            return s;
        }


        public static Chart CreateStringXIntYChart(SignalAggregationMetricOptions options)
        {
            var chart = new Chart();
            SetImageProperties(chart);
            chart.ChartAreas.Add(CreateStringXIntYChartArea(options));
            SetLegend(chart);
            chart.Titles.Add(options.ChartTitle);
            return chart;
        }

        private static ChartArea CreateStringXIntYChartArea(SignalAggregationMetricOptions options)
        {
            var chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            SetDimension(options, chartArea);
            SetIntYAxis(chartArea, options);
            SetStringXAxis(chartArea, options);
            return chartArea;
        }

        private static void SetDimension(SignalAggregationMetricOptions options, ChartArea chartArea)
        {
            if (options.SelectedDimension == Dimension.ThreeDimensional)
                chartArea.Area3DStyle = new ChartArea3DStyle {Enable3D = true, WallWidth = 0};
        }

        private static void SetStringXAxis(ChartArea chartArea, SignalAggregationMetricOptions options)
        {
            chartArea.AxisX.Title = "Signals";
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.None;
            chartArea.AxisX.LabelStyle.Font = new Font("Microsoft Sans Serif", 12);
            chartArea.AxisX.LabelStyle.Angle = 45;
        }


        public static Chart CreateApproachVolumeChart(ApproachVolumeOptions options, ApproachVolume.ApproachVolume approachVolume)
        {
            Chart chart = new Chart();
            SetImageProperties(chart);
            chart.Titles.Add(ChartTitleFactory.GetChartName(options.MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(options.SignalID, options.StartDate, options.EndDate));
            chart.Titles.Add(ChartTitleFactory.GetBoldTitle(approachVolume.PrimaryDirection.Description + " and " + approachVolume.OpposingDirection.Description + " Approaches"));
            Legend chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            chart.Legends.Add(chartLegend);
            chart.ChartAreas.Add(CreateChartArea(options));
            CustomizeChartAreaForApproachVolume(chart, options);
            return chart;
        }

        private static void CustomizeChartAreaForApproachVolume(Chart chart, ApproachVolumeOptions options)
        {
            chart.ChartAreas[0].AxisY.Minimum = options.YAxisMin;
            if (options.YAxisMax != null)
                chart.ChartAreas[0].AxisY.Maximum = options.YAxisMax ?? 0;
            chart.ChartAreas[0].AxisY.Title = "Volume (Vehicles Per Hour)";
            chart.ChartAreas[0].AxisY.Interval = 200;

            if (options.ShowDirectionalSplits)
            {
                chart.ChartAreas[0].AxisY2.Minimum = 0.0;
                chart.ChartAreas[0].AxisY2.Maximum = 1.0;
                chart.ChartAreas[0].AxisY2.Title = "Directional Split";
                chart.ChartAreas[0].AxisY2.IntervalType = DateTimeIntervalType.Number;
                chart.ChartAreas[0].AxisY2.Interval = .1;
                chart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                chart.ChartAreas[0].AxisY2.IsStartedFromZero = chart.ChartAreas[0].AxisY.IsStartedFromZero;
                chart.ChartAreas[0].AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                chart.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
            }
        }

    }
}
