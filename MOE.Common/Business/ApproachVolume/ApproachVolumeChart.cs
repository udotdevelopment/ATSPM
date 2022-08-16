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

        

        public ApproachVolumeChart(ApproachVolumeOptions options, ApproachVolume approachVolume, Models.DirectionType direction1, Models.DirectionType direction2)
        {
            MetricInfo = new MetricInfo();
            Options = options;
            Chart = ChartFactory.CreateApproachVolumeChart(options, approachVolume);
            AddDataToChart(approachVolume, options);
        }

        public static double GetPeakHourKFactor(double d, int digits)
        {
            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
            return scale * Math.Round(d / scale, digits);
        }

        public void SetLegend()
        {
            if (!Options.ShowNbEbVolume)
                Chart.Series[0].IsVisibleInLegend = false;
            if (!Options.ShowSbWbVolume)
                Chart.Series[1].IsVisibleInLegend = false;
        }

        protected void AddDataToChart(ApproachVolume approachVolume, ApproachVolumeOptions options)
        {
            AddDetectionTypeTitle(approachVolume);
            AddPrimaryDirectionSeries(approachVolume);
            AddOpposingDirectionSeries(approachVolume);
            AddCombindedDirectionSeries(approachVolume);
            AddDFactorSeries(approachVolume, options);
        }

        private void AddDFactorSeries(ApproachVolume approachVolume, ApproachVolumeOptions options)
        {
            if (options.ShowDirectionalSplits)
            {
                Series d1DfactorSeries = new Series();
                d1DfactorSeries.ChartType = SeriesChartType.Line;
                d1DfactorSeries.Name = approachVolume.PrimaryDirection.Description + " D-Factor";
                d1DfactorSeries.XValueType = ChartValueType.DateTime;
                d1DfactorSeries.YValueType = ChartValueType.Double;
                d1DfactorSeries.YAxisType = AxisType.Secondary;
                d1DfactorSeries.BorderDashStyle = ChartDashStyle.Dash;
                d1DfactorSeries.Color = Color.Blue;

                Series d2DfactorSeries = new Series();
                d2DfactorSeries.ChartType = SeriesChartType.Line;
                d2DfactorSeries.Name = approachVolume.OpposingDirection.Description + " D-Factor";
                d2DfactorSeries.XValueType = ChartValueType.DateTime;
                d2DfactorSeries.YValueType = ChartValueType.Double;
                d2DfactorSeries.YAxisType = AxisType.Secondary;
                d2DfactorSeries.Color = Color.Red;
                d2DfactorSeries.BorderDashStyle = ChartDashStyle.Dash;

                for (int i = 0; i < approachVolume.PrimaryDirectionVolume.Items.Count; i++)
                {
                    var primaryBin = approachVolume.PrimaryDirectionVolume.Items[i];
                    var opposingBin = approachVolume.OpposingDirectionVolume.Items[i];
                    var combinedBin = approachVolume.CombinedDirectionsVolumes.Items[i];
                    double direction1DFactor = Convert.ToDouble(primaryBin.YAxis) / Convert.ToDouble(combinedBin.YAxis);
                    d1DfactorSeries.Points.AddXY(primaryBin.StartTime.ToOADate(), direction1DFactor);
                    d2DfactorSeries.Points.AddXY(primaryBin.StartTime.ToOADate(), Convert.ToDouble(opposingBin.YAxis) / Convert.ToDouble(combinedBin.YAxis));
                }
                Chart.Series.Add(d1DfactorSeries);
                Chart.Series.Add(d2DfactorSeries);
            }
        }

        private void AddCombindedDirectionSeries(ApproachVolume approachVolume)
        {
            if (approachVolume.CombinedDirectionsVolumes.Items.Count > 0 && Options.ShowTotalVolume)
            {
                Series CombinedVolumeSeries = new Series();
                CombinedVolumeSeries.ChartType = SeriesChartType.Line;
                CombinedVolumeSeries.Color = Color.Black;
                CombinedVolumeSeries.Name = "Combined Volume";
                CombinedVolumeSeries.XValueType = ChartValueType.DateTime;
                CombinedVolumeSeries.BorderWidth = 2;
                foreach (Volume v in approachVolume.CombinedDirectionsVolumes.Items)
                {
                    CombinedVolumeSeries.Points.AddXY(v.XAxis.ToOADate(), v.YAxis);
                }
                Chart.Series.Add(CombinedVolumeSeries);
            }
        }

        private void AddOpposingDirectionSeries(ApproachVolume approachVolume)
        {
            if (approachVolume.OpposingDirectionVolume.Items.Count > 0)
            {
                if ((Options.ShowNbEbVolume && (approachVolume.OpposingDirection.Description == "Northbound" || approachVolume.OpposingDirection.Description == "Eastbound")) ||
                    (Options.ShowSbWbVolume && (approachVolume.OpposingDirection.Description == "Southbound" || approachVolume.OpposingDirection.Description == "Westbound")))
                {
                    Series D2Series = new Series();
                    D2Series.ChartType = SeriesChartType.Line;
                    D2Series.Color = Color.Red;
                    D2Series.Name = approachVolume.OpposingDirection.Description;
                    D2Series.XValueType = ChartValueType.DateTime;
                    D2Series.BorderWidth = 2;
                    foreach (Volume v in approachVolume.OpposingDirectionVolume.Items)
                    {
                        D2Series.Points.AddXY(v.XAxis.ToOADate(), v.YAxis);
                    }
                    Chart.Series.Add(D2Series);
                }
            }
        }

        private void AddPrimaryDirectionSeries(ApproachVolume approachVolume)
        {
            if (approachVolume.PrimaryDirectionVolume.Items.Count > 0)
            {
                if ((Options.ShowNbEbVolume && (approachVolume.PrimaryDirection.Description == "Northbound" || approachVolume.PrimaryDirection.Description == "Eastbound")) ||
                    (Options.ShowSbWbVolume && (approachVolume.PrimaryDirection.Description == "Southbound" || approachVolume.PrimaryDirection.Description == "Westbound")))
                {
                    Series D1Series = new Series();
                    D1Series.ChartType = SeriesChartType.Line;
                    D1Series.Color = Color.Blue;
                    D1Series.Name = approachVolume.PrimaryDirection.Description;
                    D1Series.BorderWidth = 2;
                    D1Series.XValueType = ChartValueType.DateTime;
                    foreach (Volume v in approachVolume.PrimaryDirectionVolume.Items)
                    {
                        D1Series.Points.AddXY(v.XAxis.ToOADate(), v.YAxis);
                    }
                    Chart.Series.Add(D1Series);
                }
            }
        }

        private void AddDetectionTypeTitle(ApproachVolume approachVolume)
        {
            if (approachVolume.Detectors.Count > 0)
            {
                Models.Detector d = approachVolume.Detectors.FirstOrDefault();
                if (d.DistanceFromStopBar != null && d.DistanceFromStopBar > 0)
                    Chart.Titles.Add(ChartTitleFactory.GetTitle(d.DetectionHardware.Name + " located " + d.DistanceFromStopBar +
                        "ft. upstream of the stop bar"));
                else
                    Chart.Titles.Add(ChartTitleFactory.GetTitle(d.DetectionHardware.Name + " at stop bar"));
            }
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

        //public DataTable CreateVolumeMetricsTable(string direction1, string direction2, int direction1TotalVolume, int direction2TotalVolume,
        //    SortedDictionary<DateTime, int> direction1Volumes, SortedDictionary<DateTime, int> direction2Volumes,
        //    ApproachVolumeOptions options)
        //{
        //    DateTime startTime, endTime;
        //    SetStartTimeAndEndTime(direction1Volumes, direction2Volumes, out startTime, out endTime);
        //    int binSizeMultiplier = 60 / options.SelectedBinSize;
        //    SortedDictionary<DateTime, int> combinedDirectionVolumes = new SortedDictionary<DateTime, int>();
        //    CombineDirectionVolumes(direction1Volumes, direction2Volumes, combinedDirectionVolumes);
        //    KeyValuePair<DateTime, int> combinedPeakHourItem = GetPeakHourVolumeItem(combinedDirectionVolumes, binSizeMultiplier);
        //    int combinedPeakHourValue = FindPeakValueinHour(combinedPeakHourItem.Key, combinedDirectionVolumes, binSizeMultiplier);
        //    double combinedPeakHourFactor = GetPeakHourFactor(combinedPeakHourItem.Value, combinedPeakHourValue, binSizeMultiplier);
        //    double combinedPeakHourKFactor = GetPeakHourKFactor(Convert.ToDouble(combinedPeakHourItem.Value) / Convert.ToDouble(direction1TotalVolume), 3);
        //    string combinedPeakHourString = combinedPeakHourItem.Key.ToShortTimeString() + " - " + combinedPeakHourItem.Key.AddHours(1).ToShortTimeString();
        //    int combinedVolume = combinedDirectionVolumes.Sum(c => c.Value)/binSizeMultiplier;
        //    KeyValuePair<DateTime, int> direction1PeakHourItem = GetPeakHourVolumeItem(direction1Volumes, binSizeMultiplier);
        //    int direction1PeakHourValue = FindPeakValueinHour(direction1PeakHourItem.Key, direction1Volumes, binSizeMultiplier);
        //    double direction1PeakHourFactor = GetPeakHourFactor(direction1PeakHourItem.Value, direction1PeakHourValue,binSizeMultiplier);
        //    double direction1PeakHourKFactor = GetPeakHourKFactor(Convert.ToDouble(direction1PeakHourItem.Value) / Convert.ToDouble(direction1TotalVolume), 3);
        //    double direction1PeakHourDFactor = GetPeakHourDFactor(direction1PeakHourItem.Key, direction1PeakHourItem.Value, direction2Volumes, binSizeMultiplier);
        //    string direction1PeakHourString = direction1PeakHourItem.Key.ToShortTimeString() + " - " + direction1PeakHourItem.Key.AddHours(1).ToShortTimeString();
        //    KeyValuePair<DateTime, int> direction2PeakHourItem = GetPeakHourVolumeItem(direction2Volumes, binSizeMultiplier);
        //    int direction2PeakValueInHour = FindPeakValueinHour(direction2PeakHourItem.Key, direction2Volumes, binSizeMultiplier);
        //    double direction2PeakHourFactor = GetPeakHourFactor(direction2PeakHourItem.Value, direction2PeakValueInHour, binSizeMultiplier);
        //    double direction2PeakHourKFactor = GetPeakHourKFactor(Convert.ToDouble(direction2PeakHourItem.Value) / Convert.ToDouble(direction2TotalVolume), 3);
        //    double direction2PeakHourDFactor = GetPeakHourDFactor(direction2PeakHourItem.Key, direction2PeakHourItem.Value, direction1Volumes, binSizeMultiplier);
        //    string direction2PeakHourString = direction2PeakHourItem.Key.ToShortTimeString() + " - " + direction2PeakHourItem.Key.AddHours(1).ToShortTimeString();
        //    DataTable volumeMetricsTable = CreateAndSetVolumeMetricsTable(direction1, direction2, direction1TotalVolume, direction2TotalVolume, startTime,
        //        endTime, combinedPeakHourItem.Value, combinedPeakHourFactor, combinedPeakHourString, direction1PeakHourItem.Value, direction1PeakHourFactor,
        //        direction2PeakHourItem.Value, direction2PeakHourFactor, direction1PeakHourString, direction2PeakHourString, combinedVolume,
        //        combinedPeakHourKFactor, direction1PeakHourKFactor, direction1PeakHourDFactor, direction2PeakHourKFactor, direction2PeakHourDFactor);
        //    SetMetricInfo(direction1, direction2, combinedPeakHourString, combinedPeakHourItem.Value, combinedPeakHourFactor, direction1PeakHourItem, direction1PeakHourFactor, 
        //        direction2PeakHourItem, direction2PeakHourFactor, direction1PeakHourString, direction2PeakHourString, combinedVolume, combinedPeakHourKFactor, 
        //        direction1PeakHourKFactor, direction1PeakHourDFactor, direction2PeakHourKFactor, direction2PeakHourDFactor);
        //    return volumeMetricsTable;
        //}

        //private void SetMetricInfo(string direction1, string direction2, string combinedPeakHourString, int combinedPeakVolume, double combinedPeakHourFactor, 
        //    KeyValuePair<DateTime, int> direction1PeakHourItem, double direction1PeakHourFactor, KeyValuePair<DateTime, int> direction2PeakHourItem, 
        //    double direction2PeakHourFactor, string direction1PeakHourString, string direction2PeakHourString, int totalVolume, double peakHourKFactor, 
        //    double direction1PeakHourKFactor, double direction1PeakHourDFactor, double direction2PeakHourKFactor, double direction2PeakHourDFactor)
        //{
        //    MetricInfo.Direction1 = direction1;
        //    MetricInfo.Direction2 = direction2;
        //    MetricInfo.D1PeakHour = direction1PeakHourString;
        //    MetricInfo.D2PeakHour = direction2PeakHourString;
        //    MetricInfo.D1PeakHourVolume = direction1PeakHourItem.Value.ToString();
        //    MetricInfo.D1PeakHourKValue = direction1PeakHourKFactor.ToString();
        //    MetricInfo.D1PeakHourDValue = direction1PeakHourDFactor.ToString();
        //    MetricInfo.D1PHF = direction1PeakHourFactor.ToString();
        //    MetricInfo.D2PeakHourVolume = direction2PeakHourItem.Value.ToString();
        //    MetricInfo.D2PeakHourKValue = direction2PeakHourKFactor.ToString();
        //    MetricInfo.D2PeakHourDValue = direction2PeakHourDFactor.ToString();
        //    MetricInfo.D2PHF = direction2PeakHourFactor.ToString();
        //    MetricInfo.TotalVolume = totalVolume.ToString();
        //    MetricInfo.PeakHour = combinedPeakHourString.ToString();
        //    MetricInfo.PeakHourVolume = combinedPeakVolume.ToString();
        //    MetricInfo.PHF = combinedPeakHourFactor.ToString();
        //    MetricInfo.PeakHourKFactor = peakHourKFactor.ToString();
        //    MetricInfo.D1TotalVolume = Direction1TotalVolume.ToString();
        //    MetricInfo.D2TotalVolume = Direction2TotalVolume.ToString();
        //}

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