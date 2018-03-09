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
        public Chart Chart = new Chart();
        public DataTable Table = new DataTable();


        public ApproachVolumeChart(DateTime startDate, DateTime endDate, string signalId,
            string location, string direction1, string direction2, ApproachVolumeOptions options,
            List<Approach> approachDirectioncollection, bool useAdvance)
        {
            info = new MetricInfo();
            Options = options;
            GetNewVolumeChart(startDate, endDate, signalId, location, direction1, direction2, options, useAdvance);
            AddDataToChart(Chart, approachDirectioncollection, startDate, endDate, signalId,
                direction1, direction2, options, useAdvance);
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SignalId { get; set; }
        public string Location { get; set; }
        public string Direction1 { get; set; }
        public string Direction2 { get; set; }
        public int D1TotalVolume { get; set; }
        public int D2TotalVolume { get; set; }
        public ApproachVolumeOptions Options { get; set; }
        public MetricInfo info { get; set; }

        public static double SetSigFigs(double d, int digits)
        {
            var scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
            return scale * Math.Round(d / scale, digits);
        }

        protected void GetNewVolumeChart(DateTime graphStartDate, DateTime graphEndDate, string signalId,
            string location, string direction1, string direction2, ApproachVolumeOptions options, bool useAdvance)
        {
            var reportTimespan = graphEndDate - graphStartDate;
            var extendedDirection = string.Empty;
            //Set the chart properties
           ChartFactory.SetImageProperties(Chart);

            SetChartTitle(direction1, direction2);
            //Create the chart legend
            var chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            Chart.Legends.Add(chartLegend);

            AddChartArea(Chart, options, reportTimespan);


            AddSeries(Chart, graphStartDate, graphEndDate, direction1, direction2, options);


            //return Chart;
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


            var D1Series = new Series();
            D1Series.ChartType = SeriesChartType.Line;
            D1Series.Color = Color.Blue;
            D1Series.Name = direction1;
            D1Series.BorderWidth = 2;
            //NBSeries.Name = "NB, Ph " + key.ToString() + " Hourly Volume";
            D1Series.XValueType = ChartValueType.DateTime;

            Chart.Series.Add(D1Series);


            //Add Direction2 Directional Volume Series


            var D2Series = new Series();
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
                var D1DfactorSeries = new Series();
                D1DfactorSeries.ChartType = SeriesChartType.Line;
                D1DfactorSeries.Name = direction1 + " D-Factor";
                D1DfactorSeries.XValueType = ChartValueType.DateTime;
                D1DfactorSeries.YValueType = ChartValueType.Double;
                D1DfactorSeries.YAxisType = AxisType.Secondary;
                D1DfactorSeries.BorderDashStyle = ChartDashStyle.Dash;
                D1DfactorSeries.Color = Color.Blue;
                Chart.Series.Add(D1DfactorSeries);

                var D2DfactorSeries = new Series();
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
            var testSeries = new Series();
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

            var chartArea = new ChartArea();
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
            DateTime endDate, string signalId, string direction1, string direction2, ApproachVolumeOptions options,
            bool useAdvance)
        {
            var D1vol = 0;
            var D2vol = 0;
            var D1time = new DateTime();
            var D2time = new DateTime();
            var D1volumes = new SortedDictionary<DateTime, int>();
            var D2volumes = new SortedDictionary<DateTime, int>();


            var FilteredApproaches = (from r in approachDirectionCollection
                where
                    r.Direction == direction1 || r.Direction == direction2
                select r).ToList();


            foreach (var approachDirection in FilteredApproaches)
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
                var d = FilteredApproaches[0].Detectors.ApproachCountDetectors[0];


                if (d.DistanceFromStopBar != null && d.DistanceFromStopBar > 0)
                    Chart.Titles.Add(ChartTitleFactory.GetTitle(
                        d.DetectionHardware.Name + " located " + d.DistanceFromStopBar +
                        "ft. upstream of the stop bar"));
                else
                    Chart.Titles.Add(ChartTitleFactory.GetTitle(d.DetectionHardware.Name + " at stop bar"));
            }


            foreach (var approachDirection in FilteredApproaches)
            {
                if (approachDirection.Volume.Items.Count > 0)
                    foreach (var v in approachDirection.Volume.Items)
                    {
                        //add Direction1 volumes
                        if (approachDirection.Direction == direction1)
                            if (!D1volumes.ContainsKey(v.XAxis))
                            {
                                D1volumes.Add(v.XAxis, v.YAxis);
                                D1TotalVolume = D1TotalVolume + v.DetectorCount;
                            }


                        //add Direction2 volumes
                        if (approachDirection.Direction == direction2)
                            if (!D2volumes.ContainsKey(v.XAxis))
                            {
                                D2volumes.Add(v.XAxis, v.YAxis);
                                D2TotalVolume = D2TotalVolume + v.DetectorCount;
                            }
                    }

                if (options.ShowNbEbVolume)
                    foreach (var vol in D1volumes)
                        //This is the Thicker Solid Red line
                        chart.Series[0].Points.AddXY(vol.Key, vol.Value);


                if (options.ShowSbWbVolume)
                    foreach (var vol in D2volumes)
                        //This is the Thicker Solid Blue line
                        chart.Series[1].Points.AddXY(vol.Key, vol.Value);


                //Match the times in the dir1 colleciton to the dir2 collection so we can get a ratio
                //of the values collected at the same point in time.
                foreach (var volRow in D1volumes)
                {
                    D2vol = (from k in D2volumes
                        where DateTime.Compare(k.Key, volRow.Key) == 0
                        select k.Value).FirstOrDefault();

                    D1vol = volRow.Value;
                    D1time = volRow.Key;

                    if (D1vol > 0 && D2vol > 0)
                    {
                        //ratio the values
                        var D1DFactor = Convert.ToDouble(D1vol) / (Convert.ToDouble(D2vol) + Convert.ToDouble(D1vol));

                        if (options.ShowDirectionalSplits)
                            chart.Series[2].Points.AddXY(D1time, D1DFactor);
                    }
                }


                //Match the times in the dir2 colleciton to the dir1 collection so we can get a ratio
                //of the values collected at the same point in time.
                foreach (var volRow in D2volumes)
                {
                    D1vol = (from k in D1volumes
                        where DateTime.Compare(k.Key, volRow.Key) == 0
                        select k.Value).FirstOrDefault();

                    D2vol = volRow.Value;
                    D2time = volRow.Key;

                    if (D2vol > 0 && D1vol > 0)
                    {
                        //ratio the values
                        var D2DFactor = Convert.ToDouble(D2vol) / (Convert.ToDouble(D2vol) + Convert.ToDouble(D1vol));

                        //plot the ratio and time on the secondary Y axis
                        if (options.ShowDirectionalSplits)
                            chart.Series[3].Points.AddXY(D2time, D2DFactor);
                    }
                }
            }

            foreach (var s in chart.Series)
            {
                var temppoints = CheckAndCorrectConsecutiveXValues(s.Points);

                s.Points.Clear();

                foreach (var d in temppoints)
                    s.Points.Add(d);
            }


            if (D1volumes.Count > 0 && D2volumes.Count > 0)
                Table = CreateVolumeMetricsTable(direction1, direction2, D1TotalVolume, D2TotalVolume, D1volumes,
                    D2volumes, options);

            if (options.ShowTotalVolume)
                AddTotalVolumeSeries(chart);
        }

        private void AddTotalVolumeSeries(Chart chart)
        {
            var totals = new Series();
            totals.ChartType = SeriesChartType.Line;
            totals.Color = Color.Black;
            totals.Name = "Total Volume";
            totals.BorderWidth = 2;
            //NBSeries.Name = "NB, Ph " + key.ToString() + " Hourly Volume";
            totals.XValueType = ChartValueType.DateTime;

            Chart.Series.Add(totals);

            AddTotalValuestoSeries(chart);
        }

        private void AddTotalValuestoSeries(Chart chart)
        {
            foreach (var dp in chart.Series[0].Points)
            {
                var dp2 = (from p in chart.Series[1].Points
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

        public DataTable CreateVolumeMetricsTable(string direction1, string direction2, int D1TV, int D2TV,
            SortedDictionary<DateTime, int> D1Volumes, SortedDictionary<DateTime, int> D2Volumes,
            ApproachVolumeOptions options)
        {
            var startTime = new DateTime();
            var endTime = new DateTime();
            var missingD1 = false;
            var missingD2 = false;
            //Create the Volume Metrics table
            if (D1Volumes.Count > 0)
            {
                startTime = D1Volumes.First().Key;
                endTime = D1Volumes.Last().Key;
                missingD1 = true;
            }
            else if (D1Volumes.Count > 0)
            {
                startTime = D2Volumes.First().Key;
                endTime = D2Volumes.Last().Key;
                missingD2 = true;
            }
            else if (missingD1 || missingD2)
            {
                var emptytable = new DataTable();
                return emptytable;
            }

            var timeDiff = endTime.Subtract(startTime);


            var volMetrics = new DataTable();
            var binSizeMultiplier = 60 / options.SelectedBinSize;
            var volMetName = new DataColumn();
            var volMetValue = new DataColumn();
            volMetName.ColumnName = "Metric";
            volMetValue.ColumnName = "Values";


            var validKfactors = false;
            if (timeDiff.TotalHours >= 23 && timeDiff.TotalHours < 25)
                validKfactors = true;
            var biDirVolumes = new SortedDictionary<DateTime, int>();
            var D1HourlyVolumes = new SortedDictionary<int, int>();
            var D2HourlyVolumes = new SortedDictionary<int, int>();

            // if (!missingD1 && !missingD2)
            // {
            //add the two volume dictionaries to get a total dictionary
            foreach (var current in D1Volumes)
                if (D2Volumes.ContainsKey(current.Key))
                    biDirVolumes.Add(current.Key, D2Volumes[current.Key] + current.Value);

            var biDirPeak = findPeakHour(biDirVolumes, binSizeMultiplier);
            var biDirPeakHour = biDirPeak.Key;
            var biDirPeakVolume = biDirPeak.Value;
            var biDirPHvol = findPeakValueinHour(biDirPeakHour, biDirVolumes, binSizeMultiplier);
            // Find Total PHF
            double biDirPHF = 0;
            if (biDirPHvol > 0)
            {
                biDirPHF = Convert.ToDouble(biDirPeakVolume) / Convert.ToDouble(biDirPHvol);
                biDirPHF = SetSigFigs(biDirPHF, 3);
            }

            var biDirPeakHourString =
                biDirPeakHour.ToShortTimeString() + " - " + biDirPeakHour.AddHours(1).ToShortTimeString();

            //   }

            //  if (!missingD1)
            //  {
            var D1Peak = findPeakHour(D1Volumes, binSizeMultiplier);
            var D1PeakHour = D1Peak.Key;
            var D1PeakHourVolume = D1Peak.Value;
            var D1PHvol = findPeakValueinHour(D1PeakHour, D1Volumes, binSizeMultiplier);
            // Find the Peak hour factor for Direciton1
            double D1PHF = 0;
            if (D1PHvol > 0)
            {
                D1PHF = Convert.ToDouble(D1PeakHourVolume) / Convert.ToDouble(D1PHvol);
                D1PHF = SetSigFigs(D1PHF, 3);
            }

            // }

            // if (!missingD2)
            // {
            var D2Peak = findPeakHour(D2Volumes, binSizeMultiplier);

            var D2PeakHour = D2Peak.Key;
            var D2PeakHourVolume = D2Peak.Value;
            var D2PHvol = findPeakValueinHour(D2PeakHour, D2Volumes, binSizeMultiplier);
            // Find the Peak hour factor for Direciton2
            double D2PHF = 0;
            if (D2PHvol > 0)
            {
                D2PHF = Convert.ToDouble(D2PeakHourVolume) / Convert.ToDouble(D2PHvol);
                D2PHF = SetSigFigs(D2PHF, 3);
            }
            // }

            var D1PeakHourString = D1PeakHour.ToShortTimeString() + " - " + D1PeakHour.AddHours(1).ToShortTimeString();
            var D2PeakHourString = D2PeakHour.ToShortTimeString() + " - " + D2PeakHour.AddHours(1).ToShortTimeString();


            var totalVolume = D1TV + D2TV;
            var PHKF = SetSigFigs(Convert.ToDouble(biDirPeakVolume * binSizeMultiplier) / Convert.ToDouble(totalVolume), 3).ToString();
            var D1PHKF = SetSigFigs(Convert.ToDouble(D1PeakHourVolume) / Convert.ToDouble(D1TV), 3).ToString();
            var D1PHDF = findPHDF(D1PeakHour, D1PeakHourVolume, D2Volumes, binSizeMultiplier).ToString();
            var D2PHKF = SetSigFigs(Convert.ToDouble(D2PeakHourVolume) / Convert.ToDouble(D2TV), 3).ToString();
            var D2PHDF = findPHDF(D2PeakHour, D2PeakHourVolume, D1Volumes, binSizeMultiplier).ToString();

            volMetrics.Columns.Add(volMetName);
            volMetrics.Columns.Add(volMetValue);


            volMetrics.Rows.Add("Total Volume", totalVolume.ToString("N0"));
            volMetrics.Rows.Add("Peak Hour", biDirPeakHourString);
            volMetrics.Rows.Add("Peak Hour Volume", string.Format("{0:#,0}", biDirPeakVolume));
            volMetrics.Rows.Add("PHF", biDirPHF.ToString());

            if (validKfactors)
                volMetrics.Rows.Add("Peak-Hour K-factor", PHKF);
            else
                volMetrics.Rows.Add("Peak-Hour K-factor", "NA");

            volMetrics.Rows.Add("", "");
            volMetrics.Rows.Add(direction1 + " Total Volume", D1TV.ToString("N0"));
            volMetrics.Rows.Add(direction1 + " Peak Hour", D1PeakHourString);
            volMetrics.Rows.Add(direction1 + " Peak Hour Volume", string.Format("{0:#,0}", D1PeakHourVolume));
            volMetrics.Rows.Add(direction1 + " PHF", D1PHF.ToString());

            if (validKfactors)
                volMetrics.Rows.Add(direction1 + " Peak-Hour K-factor", D1PHKF);
            else
                volMetrics.Rows.Add(direction1 + " Peak-Hour K-factor", "NA");

            volMetrics.Rows.Add(direction1 + " Peak-Hour D-factor", D1PHDF);
            volMetrics.Rows.Add("", "");
            volMetrics.Rows.Add(direction2 + " Total Volume", D2TV.ToString("N0"));
            volMetrics.Rows.Add(direction2 + " Peak Hour", D2PeakHourString);
            volMetrics.Rows.Add(direction2 + " Peak Hour Volume", string.Format("{0:#,0}", D2PeakHourVolume));
            volMetrics.Rows.Add(direction2 + " PHF", D2PHF.ToString("N0"));

            if (validKfactors)
                volMetrics.Rows.Add(direction2 + " Peak-Hour K-factor", D2PHKF);
            else
                volMetrics.Rows.Add(direction2 + " Peak-Hour K-factor", "NA");

            volMetrics.Rows.Add(direction2 + " Peak-Hour D-factor", D2PHDF);


            info.Direction1 = direction1;
            info.Direction2 = direction2;
            info.D1PeakHour = D1PeakHourString;
            info.D2PeakHour = D2PeakHourString;
            info.D1PeakHourVolume = D1PeakHourVolume.ToString();
            info.D1PeakHourKValue = D1PHKF;
            info.D1PeakHourDValue = D1PHDF;
            info.D1PHF = D1PHF.ToString();
            info.D2PeakHourVolume = D2PeakHourVolume.ToString();
            info.D2PeakHourKValue = D2PHKF;
            info.D2PeakHourDValue = D2PHDF;
            info.D2PHF = D2PHF.ToString();
            info.TotalVolume = totalVolume.ToString();
            info.PeakHour = biDirPeakHour.ToString();
            info.PeakHourVolume = biDirPeakVolume.ToString();
            info.PHF = biDirPHF.ToString();
            info.PeakHourKFactor = PHKF;
            info.D1TotalVolume = D1TotalVolume.ToString();
            info.D2TotalVolume = D2TotalVolume.ToString();


            return volMetrics;
        }

        protected KeyValuePair<DateTime, int> findPeakHour(SortedDictionary<DateTime, int> dirVolumes,
            int binMultiplier)
        {
            var subTotal = 0;
            var peakHourValue = new KeyValuePair<DateTime, int>();

            var startTime = new DateTime();
            var iteratedVolumes = new SortedDictionary<DateTime, int>();

            for (var i = 0; i < dirVolumes.Count - (binMultiplier - 1); i++)
            {
                startTime = dirVolumes.ElementAt(i).Key;
                subTotal = 0;
                for (var x = 0; x < binMultiplier; x++)
                    subTotal = subTotal + dirVolumes.ElementAt(i + x).Value;
                subTotal = (int) (subTotal / binMultiplier);
                iteratedVolumes.Add(startTime, subTotal);
            }

            //Find the highest value in the iterated Volumes dictionary.
            //This should bee the peak hour.
            foreach (var kvp in iteratedVolumes)
                if (kvp.Value > peakHourValue.Value)
                    peakHourValue = kvp;

            return peakHourValue;
        }

        protected int findPeakValueinHour(DateTime StartofHour, SortedDictionary<DateTime, int> volDic,
            int binMultiplier)
        {
            var maxVolume = 0;

            for (var i = 0; i < binMultiplier; i++)
            {
                if (volDic.ContainsKey(StartofHour))
                    if (maxVolume < volDic[StartofHour])
                        maxVolume = volDic[StartofHour];

                StartofHour = StartofHour.AddMinutes(60 / binMultiplier);
            }
            return maxVolume;
        }

        protected double findPHDF(DateTime StartofHour, int Peakhourvolume, SortedDictionary<DateTime, int> volDic,
            int binMultiplier)
        {
            var totalVolume = 0;
            double PHDF = 0;

            for (var i = 0; i < binMultiplier; i++)
            {
                if (volDic.ContainsKey(StartofHour))
                    totalVolume = totalVolume + volDic[StartofHour];

                StartofHour = StartofHour.AddMinutes(60 / binMultiplier);
            }
            totalVolume /= binMultiplier;
            totalVolume += Peakhourvolume;
            if (totalVolume > 0)
                PHDF = SetSigFigs(Convert.ToDouble(Peakhourvolume) / Convert.ToDouble(totalVolume), 3);
            else
                PHDF = 0;
            return PHDF;
        }

        public List<DataPoint> CheckAndCorrectConsecutiveXValues(DataPointCollection points)
        {
            var dcp = new List<DataPoint>();

            var i = 0;
            double currentmax = 0;
            var badPoints = new List<int>();


            foreach (var dp in points)
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