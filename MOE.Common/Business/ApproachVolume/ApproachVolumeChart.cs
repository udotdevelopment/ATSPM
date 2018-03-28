using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.WCFServiceLibrary;

namespace MOE.Common.Business.ApproachVolume
{
    public class ApproachVolumeChart
    {
        public Chart Chart { get; private set; } = new Chart();
        public DataTable Table { get; private set; } = new DataTable();
        public int Direction1TotalVolume { get; private set; }
        public int Direction2TotalVolume { get; private set; }
        public ApproachVolumeOptions Options { get; private set; }
        public MetricInfo MetricInfo { get; private set; }


        public ApproachVolumeChart(DateTime startDate, DateTime endDate, string signalId,
            string location, string direction1, string direction2, ApproachVolumeOptions options,
            List<Approach> approachDirectioncollection, bool useAdvance)
        {
            MetricInfo = new MetricInfo();
            Options = options;
            GetNewVolumeChart(startDate, endDate, direction1, direction2, options);
            AddDataToChart(Chart, approachDirectioncollection, startDate, endDate,
                direction1, direction2, options, useAdvance);
        }

        public static double GetPeakHourKFactor(double d, int digits)
        {
            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
            return scale * Math.Round(d / scale, digits);
        }

        protected void GetNewVolumeChart(DateTime graphStartDate, DateTime graphEndDate, string direction1, string direction2, ApproachVolumeOptions options)
        {
            TimeSpan reportTimespan = graphEndDate - graphStartDate;
            ChartFactory.SetImageProperties(Chart);
            SetChartTitle(direction1, direction2);
            Legend chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            Chart.Legends.Add(chartLegend);
            AddChartArea(Chart, options, reportTimespan);
            AddSeries(Chart, graphStartDate, graphEndDate, direction1, direction2, options);
        }

        private void SetChartTitle(string direction1, string direction2)
        {
            Chart.Titles.Add(ChartTitleFactory.GetChartName(Options.MetricTypeID));
            Chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(
                Options.SignalID, Options.StartDate, Options.EndDate));
            Chart.Titles.Add(ChartTitleFactory.GetBoldTitle(direction1 + " and " + direction2 + " Approaches"));
        }

        private void AddSeries(Chart Chart, DateTime graphStartDate, DateTime graphEndDate, string direction1,
            string direction2, ApproachVolumeOptions options)
        {
            //Add Direction1 Directional Volume Series

            Series D1Series = new Series();
            D1Series.ChartType = SeriesChartType.Line;
            D1Series.Color = Color.Blue;
            D1Series.Name = direction1;
            D1Series.BorderWidth = 2;
            //NBSeries.Name = "NB, Ph " + key.ToString() + " Hourly Volume";
            D1Series.XValueType = ChartValueType.DateTime;

            Chart.Series.Add(D1Series);

            //Add Direction2 Directional Volume Series

            Series D2Series = new Series();
            D2Series.ChartType = SeriesChartType.Line;
            D2Series.Color = Color.Red;
            D2Series.Name = direction2;
            //SBSeries.Name = "SB, Ph " + key.ToString() + " Hourly Volume";
            D2Series.XValueType = ChartValueType.DateTime;
            D2Series.BorderWidth = 2;
            Chart.Series.Add(D2Series);

            //Add D-Factor Series

            if (options.ShowDirectionalSplits)
            {
                Series D1DfactorSeries = new Series();
                D1DfactorSeries.ChartType = SeriesChartType.Line;
                D1DfactorSeries.Name = direction1 + " D-Factor";
                D1DfactorSeries.XValueType = ChartValueType.DateTime;
                D1DfactorSeries.YValueType = ChartValueType.Double;
                D1DfactorSeries.YAxisType = AxisType.Secondary;
                D1DfactorSeries.BorderDashStyle = ChartDashStyle.Dash;
                D1DfactorSeries.Color = Color.Blue;
                Chart.Series.Add(D1DfactorSeries);

                Series D2DfactorSeries = new Series();
                D2DfactorSeries.ChartType = SeriesChartType.Line;
                D2DfactorSeries.Name = direction2 + " D-Factor";
                D2DfactorSeries.XValueType = ChartValueType.DateTime;
                D2DfactorSeries.YValueType = ChartValueType.Double;
                D2DfactorSeries.YAxisType = AxisType.Secondary;
                D2DfactorSeries.Color = Color.Red;
                D2DfactorSeries.BorderDashStyle = ChartDashStyle.Dash;
                Chart.Series.Add(D2DfactorSeries);
            }

            //Add the Posts series to ensure the chart is the size of the selected timespan
            Series testSeries = new Series();
            testSeries.IsVisibleInLegend = false;
            testSeries.ChartType = SeriesChartType.Point;
            testSeries.Color = Color.White;
            testSeries.Name = "Posts";
            testSeries.XValueType = ChartValueType.DateTime;
            testSeries.MarkerSize = 0;
            Chart.Series.Add(testSeries);

            //Add points at the start and and of the x axis to ensure
            //the graph covers the entire period selected by the user
            //whether there is data or not
            Chart.Series["Posts"].Points.AddXY(graphStartDate, 0);
            Chart.Series["Posts"].Points.AddXY(graphEndDate, 0);
            SetLegend();
        }

        private void AddChartArea(Chart Chart, ApproachVolumeOptions options, TimeSpan reportTimespan)
        {
            //Create the chart area

            ChartArea chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;

            chartArea.AxisY.Minimum = options.YAxisMin;
            if (options.YAxisMax != null)
                chartArea.AxisY.Maximum = options.YAxisMax ?? 0;

            chartArea.AxisY.Title = "Volume (Vehicles Per Hour)";

            chartArea.AxisY.MajorTickMark.Enabled = true;
            chartArea.AxisY.MajorGrid.Enabled = true;
            chartArea.AxisY.IntervalType = DateTimeIntervalType.Number;
            chartArea.AxisY.Interval = 200;

            if (options.ShowDirectionalSplits)
            {
                chartArea.AxisY2.Minimum = 0.0;
                chartArea.AxisY2.Maximum = 1.0;
                chartArea.AxisY2.Title = "Directional Split";

                chartArea.AxisY2.IntervalType = DateTimeIntervalType.Number;
                chartArea.AxisY2.Interval = .1;
                chartArea.AxisY2.Enabled = AxisEnabled.True;
                chartArea.AxisY2.IsStartedFromZero = chartArea.AxisY.IsStartedFromZero;
                chartArea.AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                chartArea.AxisY2.MajorGrid.Enabled = false;
            }

            chartArea.AxisX.Title = "Time (Hour of Day)";
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.LabelStyle.Format = "HH";
            if (reportTimespan.Days < 1)
                if (reportTimespan.Hours > 1)
                    chartArea.AxisX.Interval = 1;
                else
                    chartArea.AxisX.LabelStyle.Format = "HH:mm";

            chartArea.AxisX2.Enabled = AxisEnabled.True;
            Chart.ChartAreas.Add(chartArea);
        }

        public void SetLegend()
        {
            if (!Options.ShowNbEbVolume)
                Chart.Series[0].IsVisibleInLegend = false;
            if (!Options.ShowSbWbVolume)
                Chart.Series[1].IsVisibleInLegend = false;
        }

        protected void AddDataToChart(Chart chart, List<Approach> approachDirectionCollection, DateTime startDate,
            DateTime endDate, string direction1, string direction2, ApproachVolumeOptions options,
            bool useAdvance)
        {
            int D1vol = 0;
            int D2vol = 0;
            DateTime D1time = new DateTime();
            DateTime D2time = new DateTime();
            SortedDictionary<DateTime, int> D1volumes = new SortedDictionary<DateTime, int>();
            SortedDictionary<DateTime, int> D2volumes = new SortedDictionary<DateTime, int>();

            List<Approach> FilteredApproaches = (from r in approachDirectionCollection
                where
                    r.Direction == direction1 || r.Direction == direction2
                select r).ToList();

            foreach (Approach approachDirection in FilteredApproaches)
            {
                if (useAdvance)
                    approachDirection.SetDetectorEvents(approachDirection.ApproachModel, startDate, endDate, true,
                        false);
                else
                    approachDirection.SetDetectorEvents(approachDirection.ApproachModel, startDate, endDate, false,
                        true);

                approachDirection.SetVolume(startDate, endDate, options.SelectedBinSize);
            }

            if (FilteredApproaches.Count > 0)
            {
                Models.Detector d = FilteredApproaches[0].Detectors.ApproachCountDetectors[0];

                if (d.DistanceFromStopBar != null && d.DistanceFromStopBar > 0)
                    Chart.Titles.Add(ChartTitleFactory.GetTitle(
                        d.DetectionHardware.Name + " located " + d.DistanceFromStopBar +
                        "ft. upstream of the stop bar"));
                else
                    Chart.Titles.Add(ChartTitleFactory.GetTitle(d.DetectionHardware.Name + " at stop bar"));
            }

            foreach (Approach approachDirection in FilteredApproaches)
            {
                if (approachDirection.Volume.Items.Count > 0)
                    foreach (Volume v in approachDirection.Volume.Items)
                    {
                        //add Direction1 volumes
                        if (approachDirection.Direction == direction1)
                            if (!D1volumes.ContainsKey(v.XAxis))
                            {
                                D1volumes.Add(v.XAxis, v.YAxis);
                                Direction1TotalVolume = Direction1TotalVolume + v.DetectorCount;
                            }

                        //add Direction2 volumes
                        if (approachDirection.Direction == direction2)
                            if (!D2volumes.ContainsKey(v.XAxis))
                            {
                                D2volumes.Add(v.XAxis, v.YAxis);
                                Direction2TotalVolume = Direction2TotalVolume + v.DetectorCount;
                            }
                    }

                if (options.ShowNbEbVolume)
                    foreach (KeyValuePair<DateTime, int> vol in D1volumes)
                        //This is the Thicker Solid Red line
                        chart.Series[0].Points.AddXY(vol.Key, vol.Value);

                if (options.ShowSbWbVolume)
                    foreach (KeyValuePair<DateTime, int> vol in D2volumes)
                        //This is the Thicker Solid Blue line
                        chart.Series[1].Points.AddXY(vol.Key, vol.Value);

                //Match the times in the dir1 colleciton to the dir2 collection so we can get a ratio
                //of the values collected at the same point in time.
                foreach (KeyValuePair<DateTime, int> volRow in D1volumes)
                {
                    D2vol = (from k in D2volumes
                        where DateTime.Compare(k.Key, volRow.Key) == 0
                        select k.Value).FirstOrDefault();

                    D1vol = volRow.Value;
                    D1time = volRow.Key;

                    if (D1vol > 0 && D2vol > 0)
                    {
                        //ratio the values
                        double D1DFactor = Convert.ToDouble(D1vol) / (Convert.ToDouble(D2vol) + Convert.ToDouble(D1vol));

                        if (options.ShowDirectionalSplits)
                            chart.Series[2].Points.AddXY(D1time, D1DFactor);
                    }
                }

                //Match the times in the dir2 colleciton to the dir1 collection so we can get a ratio
                //of the values collected at the same point in time.
                foreach (KeyValuePair<DateTime, int> volRow in D2volumes)
                {
                    D1vol = (from k in D1volumes
                        where DateTime.Compare(k.Key, volRow.Key) == 0
                        select k.Value).FirstOrDefault();

                    D2vol = volRow.Value;
                    D2time = volRow.Key;

                    if (D2vol > 0 && D1vol > 0)
                    {
                        //ratio the values
                        double D2DFactor = Convert.ToDouble(D2vol) / (Convert.ToDouble(D2vol) + Convert.ToDouble(D1vol));

                        //plot the ratio and time on the secondary Y axis
                        if (options.ShowDirectionalSplits)
                            chart.Series[3].Points.AddXY(D2time, D2DFactor);
                    }
                }
            }

            foreach (Series s in chart.Series)
            {
                List<DataPoint> temppoints = CheckAndCorrectConsecutiveXValues(s.Points);

                s.Points.Clear();

                foreach (DataPoint d in temppoints)
                    s.Points.Add(d);
            }

            if (D1volumes.Count > 0 && D2volumes.Count > 0)
                Table = CreateVolumeMetricsTable(direction1, direction2, Direction1TotalVolume, Direction2TotalVolume, D1volumes,
                    D2volumes, options);

            if (options.ShowTotalVolume)
                AddTotalVolumeSeries(chart);
        }

        private void AddTotalVolumeSeries(Chart chart)
        {
            Series totals = new Series();
            totals.ChartType = SeriesChartType.Line;
            totals.Color = Color.Black;
            totals.Name = "Total Volume";
            totals.BorderWidth = 2;
            totals.XValueType = ChartValueType.DateTime;
            Chart.Series.Add(totals);
            AddTotalValuestoSeries(chart);
        }

        private void AddTotalValuestoSeries(Chart chart)
        {
            foreach (DataPoint dp in chart.Series[0].Points)
            {
                DataPoint dp2 = (from p in chart.Series[1].Points
                    where p.XValue == dp.XValue
                    select p).FirstOrDefault();

                double totalVolforBin = 0;
                if (dp2 != null)
                    totalVolforBin = dp.YValues[0] + dp2.YValues[0];
                else
                    totalVolforBin = dp.YValues[0];

                chart.Series["Total Volume"].Points.AddXY(dp.XValue, totalVolforBin);
            }
        }

        public DataTable CreateVolumeMetricsTable(string direction1, string direction2, int direction1TotalVolume, int direction2TotalVolume,
            SortedDictionary<DateTime, int> direction1Volumes, SortedDictionary<DateTime, int> direction2Volumes,
            ApproachVolumeOptions options)
        {
            DateTime startTime, endTime;
            SetStartTimeAndEndTime(direction1Volumes, direction2Volumes, out startTime, out endTime);
            int binSizeMultiplier = 60 / options.SelectedBinSize;
            SortedDictionary<DateTime, int> combinedDirectionVolumes = new SortedDictionary<DateTime, int>();
            CombineDirectionVolumes(direction1Volumes, direction2Volumes, combinedDirectionVolumes);
            KeyValuePair<DateTime, int> combinedPeakHourItem = GetPeakHourVolumeItem(combinedDirectionVolumes, binSizeMultiplier);
            int combinedPeakHourValue = FindPeakValueinHour(combinedPeakHourItem.Key, combinedDirectionVolumes, binSizeMultiplier);
            double combinedPeakHourFactor = GetPeakHourFactor(combinedPeakHourItem.Value, combinedPeakHourValue, binSizeMultiplier);
            double combinedPeakHourKFactor = GetPeakHourKFactor(Convert.ToDouble(combinedPeakHourItem.Value) / Convert.ToDouble(direction1TotalVolume), 3);
            string combinedPeakHourString = combinedPeakHourItem.Key.ToShortTimeString() + " - " + combinedPeakHourItem.Key.AddHours(1).ToShortTimeString();
            int combinedVolume = combinedDirectionVolumes.Sum(c => c.Value)/binSizeMultiplier;
            KeyValuePair<DateTime, int> direction1PeakHourItem = GetPeakHourVolumeItem(direction1Volumes, binSizeMultiplier);
            int direction1PeakHourValue = FindPeakValueinHour(direction1PeakHourItem.Key, direction1Volumes, binSizeMultiplier);
            double direction1PeakHourFactor = GetPeakHourFactor(direction1PeakHourItem.Value, direction1PeakHourValue,binSizeMultiplier);
            double direction1PeakHourKFactor = GetPeakHourKFactor(Convert.ToDouble(direction1PeakHourItem.Value) / Convert.ToDouble(direction1TotalVolume), 3);
            double direction1PeakHourDFactor = GetPeakHourDFactor(direction1PeakHourItem.Key, direction1PeakHourItem.Value, direction2Volumes, binSizeMultiplier);
            string direction1PeakHourString = direction1PeakHourItem.Key.ToShortTimeString() + " - " + direction1PeakHourItem.Key.AddHours(1).ToShortTimeString();
            KeyValuePair<DateTime, int> direction2PeakHourItem = GetPeakHourVolumeItem(direction2Volumes, binSizeMultiplier);
            int direction2PeakValueInHour = FindPeakValueinHour(direction2PeakHourItem.Key, direction2Volumes, binSizeMultiplier);
            double direction2PeakHourFactor = GetPeakHourFactor(direction2PeakHourItem.Value, direction2PeakValueInHour, binSizeMultiplier);
            double direction2PeakHourKFactor = GetPeakHourKFactor(Convert.ToDouble(direction2PeakHourItem.Value) / Convert.ToDouble(direction2TotalVolume), 3);
            double direction2PeakHourDFactor = GetPeakHourDFactor(direction2PeakHourItem.Key, direction2PeakHourItem.Value, direction1Volumes, binSizeMultiplier);
            string direction2PeakHourString = direction2PeakHourItem.Key.ToShortTimeString() + " - " + direction2PeakHourItem.Key.AddHours(1).ToShortTimeString();
            DataTable volumeMetricsTable = CreateAndSetVolumeMetricsTable(direction1, direction2, direction1TotalVolume, direction2TotalVolume, startTime,
                endTime, combinedPeakHourItem.Value, combinedPeakHourFactor, combinedPeakHourString, direction1PeakHourItem.Value, direction1PeakHourFactor,
                direction2PeakHourItem.Value, direction2PeakHourFactor, direction1PeakHourString, direction2PeakHourString, combinedVolume,
                combinedPeakHourKFactor, direction1PeakHourKFactor, direction1PeakHourDFactor, direction2PeakHourKFactor, direction2PeakHourDFactor);
            SetMetricInfo(direction1, direction2, combinedPeakHourString, combinedPeakHourItem.Value, combinedPeakHourFactor, direction1PeakHourItem, direction1PeakHourFactor, 
                direction2PeakHourItem, direction2PeakHourFactor, direction1PeakHourString, direction2PeakHourString, combinedVolume, combinedPeakHourKFactor, 
                direction1PeakHourKFactor, direction1PeakHourDFactor, direction2PeakHourKFactor, direction2PeakHourDFactor);
            return volumeMetricsTable;
        }

        private void SetMetricInfo(string direction1, string direction2, string combinedPeakHourString, int combinedPeakVolume, double combinedPeakHourFactor, 
            KeyValuePair<DateTime, int> direction1PeakHourItem, double direction1PeakHourFactor, KeyValuePair<DateTime, int> direction2PeakHourItem, 
            double direction2PeakHourFactor, string direction1PeakHourString, string direction2PeakHourString, int totalVolume, double peakHourKFactor, 
            double direction1PeakHourKFactor, double direction1PeakHourDFactor, double direction2PeakHourKFactor, double direction2PeakHourDFactor)
        {
            MetricInfo.Direction1 = direction1;
            MetricInfo.Direction2 = direction2;
            MetricInfo.D1PeakHour = direction1PeakHourString;
            MetricInfo.D2PeakHour = direction2PeakHourString;
            MetricInfo.D1PeakHourVolume = direction1PeakHourItem.Value.ToString();
            MetricInfo.D1PeakHourKValue = direction1PeakHourKFactor.ToString();
            MetricInfo.D1PeakHourDValue = direction1PeakHourDFactor.ToString();
            MetricInfo.D1PHF = direction1PeakHourFactor.ToString();
            MetricInfo.D2PeakHourVolume = direction2PeakHourItem.Value.ToString();
            MetricInfo.D2PeakHourKValue = direction2PeakHourKFactor.ToString();
            MetricInfo.D2PeakHourDValue = direction2PeakHourDFactor.ToString();
            MetricInfo.D2PHF = direction2PeakHourFactor.ToString();
            MetricInfo.TotalVolume = totalVolume.ToString();
            MetricInfo.PeakHour = combinedPeakHourString.ToString();
            MetricInfo.PeakHourVolume = combinedPeakVolume.ToString();
            MetricInfo.PHF = combinedPeakHourFactor.ToString();
            MetricInfo.PeakHourKFactor = peakHourKFactor.ToString();
            MetricInfo.D1TotalVolume = Direction1TotalVolume.ToString();
            MetricInfo.D2TotalVolume = Direction2TotalVolume.ToString();
        }

        private static DataTable CreateAndSetVolumeMetricsTable(string direction1, string direction2, int direction1TotalVolume, int direction2TotalVolume, 
            DateTime startTime, DateTime endTime, int combinedPeakVolume, double combinedPeakHourFactor, string combinedPeakHourString, 
            int direction1PeakHourVolume, double direction1PeakHourFactor, int direction2PeakHourVolume, double direction2PeakHourFactor, 
            string direction1PeakHourString, string direction2PeakHourString, int totalVolume, double peakHourKFactor, double direction1PeakHourKFactor, 
            double direction1PeakHourDFactor, double direction2PeakHourKFactor, double direction2PeakHourDFactor)
        {
            DataTable volumeMetricsTable = new DataTable();
            DataColumn descriptionColumn = new DataColumn();
            DataColumn valueColumn = new DataColumn();
            descriptionColumn.ColumnName = "Metric";
            valueColumn.ColumnName = "Values";
            volumeMetricsTable.Columns.Add(descriptionColumn);
            volumeMetricsTable.Columns.Add(valueColumn);


            volumeMetricsTable.Rows.Add("Total Volume", totalVolume.ToString("N0"));
            volumeMetricsTable.Rows.Add("Peak Hour", combinedPeakHourString);
            volumeMetricsTable.Rows.Add("Peak Hour Volume", string.Format("{0:#,0}", combinedPeakVolume));
            volumeMetricsTable.Rows.Add("PHF", combinedPeakHourFactor.ToString());

            if (IsValidTimePeriodForKFactors(startTime, endTime))
            {
                volumeMetricsTable.Rows.Add("Peak-Hour K-factor", peakHourKFactor);
                volumeMetricsTable.Rows.Add(direction1 + " Peak-Hour K-factor", direction1PeakHourKFactor);
                volumeMetricsTable.Rows.Add(direction2 + " Peak-Hour K-factor", direction2PeakHourKFactor);
            }
            else
            {
                volumeMetricsTable.Rows.Add("Peak-Hour K-factor", "NA");
                volumeMetricsTable.Rows.Add(direction1 + " Peak-Hour K-factor", "NA");
                volumeMetricsTable.Rows.Add(direction2 + " Peak-Hour K-factor", "NA");
            }

            volumeMetricsTable.Rows.Add("", "");
            volumeMetricsTable.Rows.Add(direction1 + " Total Volume", direction1TotalVolume.ToString("N0"));
            volumeMetricsTable.Rows.Add(direction1 + " Peak Hour", direction1PeakHourString);
            volumeMetricsTable.Rows.Add(direction1 + " Peak Hour Volume", string.Format("{0:#,0}", direction1PeakHourVolume));
            volumeMetricsTable.Rows.Add(direction1 + " PHF", direction1PeakHourFactor.ToString());


            volumeMetricsTable.Rows.Add(direction1 + " Peak-Hour D-factor", direction1PeakHourDFactor);
            volumeMetricsTable.Rows.Add("", "");
            volumeMetricsTable.Rows.Add(direction2 + " Total Volume", direction2TotalVolume.ToString("N0"));
            volumeMetricsTable.Rows.Add(direction2 + " Peak Hour", direction2PeakHourString);
            volumeMetricsTable.Rows.Add(direction2 + " Peak Hour Volume", string.Format("{0:#,0}", direction2PeakHourVolume));
            volumeMetricsTable.Rows.Add(direction2 + " PHF", direction2PeakHourFactor.ToString("N0"));


            volumeMetricsTable.Rows.Add(direction2 + " Peak-Hour D-factor", direction2PeakHourDFactor);
            return volumeMetricsTable;
        }

        private static double GetPeakHourFactor(int direction1PeakHourVolume, int D1PHvol, int binSizeMultiplier)
        {
            double D1PHF = 0;
            if (D1PHvol > 0)
            {
                D1PHF = Convert.ToDouble(direction1PeakHourVolume) / Convert.ToDouble(D1PHvol);
                D1PHF = GetPeakHourKFactor(D1PHF, 3);
            }

            return D1PHF/binSizeMultiplier;
        }

        private void GetCombinedPeakHourValues(int binSizeMultiplier, SortedDictionary<DateTime, int> combinedDirectionVolumes, out DateTime combinedPeakHour, out int combinedPeakVolume, out double combinedPeakHourFactor, out string combinedPeakHourString)
        {
            KeyValuePair<DateTime, int> combinedPeakHourVolumeItem = GetPeakHourVolumeItem(combinedDirectionVolumes, binSizeMultiplier);
            combinedPeakHour = combinedPeakHourVolumeItem.Key;
            combinedPeakVolume = combinedPeakHourVolumeItem.Value / 4;
            int combinedPeakValueInHour = FindPeakValueinHour(combinedPeakHour, combinedDirectionVolumes, binSizeMultiplier);
            // Find Total PHF
            combinedPeakHourFactor = 0;
            if (combinedPeakValueInHour > 0)
            {
                combinedPeakHourFactor = Convert.ToDouble(combinedPeakVolume) / Convert.ToDouble(combinedPeakValueInHour);
                combinedPeakHourFactor = GetPeakHourKFactor(combinedPeakHourFactor, 3);
            }

            combinedPeakHourString = combinedPeakHour.ToShortTimeString() + " - " + combinedPeakHour.AddHours(1).ToShortTimeString();
        }

        private static void CombineDirectionVolumes(SortedDictionary<DateTime, int> direction1Volumes, SortedDictionary<DateTime, int> direction2Volumes, SortedDictionary<DateTime, int> combinedDirectionVolumes)
        {
            foreach (KeyValuePair<DateTime, int> current in direction1Volumes)
                if (direction2Volumes.ContainsKey(current.Key))
                    combinedDirectionVolumes.Add(current.Key, direction2Volumes[current.Key] + current.Value);
        }

        private static bool IsValidTimePeriodForKFactors(DateTime startTime, DateTime endTime)
        {
            TimeSpan timeDiff = endTime.Subtract(startTime);
            bool validKfactors = timeDiff.TotalHours >= 23 && timeDiff.TotalHours < 25;
            return validKfactors;
        }

        private static void SetStartTimeAndEndTime(SortedDictionary<DateTime, int> direction1Volumes, SortedDictionary<DateTime, int> direction2Volumes, out DateTime startTime, out DateTime endTime)
        {
            startTime = new DateTime();
            endTime = new DateTime();
            //Create the Volume Metrics table
            if (direction1Volumes.Count > 0)
            {
                startTime = direction1Volumes.First().Key;
                endTime = direction1Volumes.Last().Key;
            }
            else if (direction1Volumes.Count > 0)
            {
                startTime = direction2Volumes.First().Key;
                endTime = direction2Volumes.Last().Key;
            }
        }

        protected KeyValuePair<DateTime, int> GetPeakHourVolumeItem(SortedDictionary<DateTime, int> volumes,
            int binMultiplier)
        {
            KeyValuePair<DateTime, int> peakHourValue = new KeyValuePair<DateTime, int>();
            SortedDictionary<DateTime, int> iteratedVolumes = new SortedDictionary<DateTime, int>();
            foreach (var volume in volumes)
            {
                iteratedVolumes.Add(volume.Key, volumes.Where(v => v.Key >= volume.Key && v.Key < volume.Key.AddHours(1)).Sum(v => v.Value));
            }
            peakHourValue = iteratedVolumes.OrderByDescending(i => i.Value).FirstOrDefault();
            //Find the highest value in the iterated Volumes dictionary.
            //This should bee the peak hour.
            //foreach (KeyValuePair<DateTime, int> kvp in iteratedVolumes)
            //    if (kvp.Value > peakHourValue.Value)
            //        peakHourValue = kvp;

            return peakHourValue;
        }

        protected int FindPeakValueinHour(DateTime StartofHour, SortedDictionary<DateTime, int> volDic,
            int binMultiplier)
        {
            int maxVolume = 0;

            for (int i = 0; i < binMultiplier; i++)
            {
                if (volDic.ContainsKey(StartofHour))
                    if (maxVolume < volDic[StartofHour])
                        maxVolume = volDic[StartofHour];

                StartofHour = StartofHour.AddMinutes(60 / binMultiplier);
            }
            return maxVolume;
        }

        protected double GetPeakHourDFactor(DateTime StartofHour, int Peakhourvolume, SortedDictionary<DateTime, int> volDic,
            int binMultiplier)
        {
            int totalVolume = 0;
            double PHDF = 0;

            for (int i = 0; i < binMultiplier; i++)
            {
                if (volDic.ContainsKey(StartofHour))
                    totalVolume = totalVolume + volDic[StartofHour];

                StartofHour = StartofHour.AddMinutes(60 / binMultiplier);
            }
            totalVolume /= binMultiplier;
            totalVolume += Peakhourvolume;
            if (totalVolume > 0)
                PHDF = GetPeakHourKFactor(Convert.ToDouble(Peakhourvolume) / Convert.ToDouble(totalVolume), 3);
            else
                PHDF = 0;
            return PHDF;
        }

        public List<DataPoint> CheckAndCorrectConsecutiveXValues(DataPointCollection points)
        {
            List<DataPoint> dcp = new List<DataPoint>();

            int i = 0;
            double currentmax = 0;
            List<int> badPoints = new List<int>();

            foreach (DataPoint dp in points)
            {
                if (dp.XValue > currentmax)
                {
                    currentmax = dp.XValue;
                    dcp.Add(dp);
                }
                else
                {
                    badPoints.Add(i);

                    //for (int x = 0; x < points.Count; x++)
                    //{
                    //    if(points[x].XValue < )
                    //}
                }
                i++;
            }

            return dcp;

            //foreach (int I in badPoints)
            //{
            //    points.RemoveAt(I);
            //}
        }
    }
}