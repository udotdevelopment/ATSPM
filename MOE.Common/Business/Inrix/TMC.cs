using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Business.Inrix
{
    public class TMC
    {
        protected double binAverageTravelTime;

        //private MOE.Common.Data.InrixTableAdapters.TravelTimesTableAdapter TravelTimesTA = new Data.InrixTableAdapters.TravelTimesTableAdapter();
        //private MOE.Common.Data.Inrix.TravelTimesDataTable rawTravelTimes = new Data.Inrix.TravelTimesDataTable();
        public List<TravelTimeRecord> BinnedTravelTimes = new List<TravelTimeRecord>();

        protected double confidenceAverage;

        protected string direction;

        protected double expectedRecords;

        protected bool hasRecords;

        protected int invalidBinCount;

        protected double length;
        public SortedDictionary<double, double> RankedTravelTimes = new SortedDictionary<double, double>();

        protected string start;

        protected string stop;

        protected string street;
        protected string tmcCode;

        /// <summary>
        ///     Default Constructor for TMC class
        /// </summary>
        /// <param name="tmccode"></param>
        /// <param name="direction"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="length"></param>
        /// <param name="street"></param>
        public TMC(string tmccode, string direction, string start, string stop, double length, string street)
        {
            TMCCode = tmccode;
            Direction = direction;
            Start = start;
            Stop = stop;
            Length = length;
            Street = street;
            invalidBinCount = 0;
            hasRecords = false;
        }

        public string TMCCode
        {
            get => tmcCode;
            set => tmcCode = value;
        }

        public string Direction
        {
            get => direction;
            set => direction = value;
        }

        public string Start
        {
            get => start;
            set => start = value;
        }

        public string Stop
        {
            get => stop;
            set => stop = value;
        }

        public double Length
        {
            get => length;
            set => length = value;
        }

        public string Street
        {
            get => street;
            set => street = value;
        }

        public int InvalidBinCount => invalidBinCount;

        public double ExpectedRecords => expectedRecords;

        public double ConfidenceAverage => confidenceAverage;

        public double BinAverageTravelTime => binAverageTravelTime;

        public bool HasRecords => hasRecords;

        public void IncreaseInvalidBinCount()
        {
            invalidBinCount++;
        }

        private void CalculateTravelTimePercentile()
        {
            RankedTravelTimes.Clear();
            var sortedTravelTimes = new List<double>();

            foreach (var ttr in BinnedTravelTimes)
                sortedTravelTimes.Add(ttr.TravelTime);
            sortedTravelTimes.Sort();

            if (sortedTravelTimes.Count > 0)
            {
                hasRecords = true;
                double rank = 0;
                for (double percentile = 1; percentile <= 100; percentile++)
                {
                    rank = percentile / 100 * (sortedTravelTimes.Count + 1);

                    var intPartofRank = Math.Floor(rank);
                    var decPartofRank = rank - intPartofRank;

                    double TT = 0;

                    if (intPartofRank > 0 && intPartofRank < sortedTravelTimes.Count)
                    {
                        var TT1 = sortedTravelTimes[Convert.ToInt16(Math.Floor(rank) - 1)];
                        var TT2 = sortedTravelTimes[Convert.ToInt16(Math.Floor(rank))];
                        TT = TT1 + (TT2 - TT1) * decPartofRank;
                    }
                    else if (intPartofRank <= 0)
                    {
                        TT = sortedTravelTimes.First();
                    }
                    else if (intPartofRank >= sortedTravelTimes.Count)
                    {
                        TT = sortedTravelTimes.Last();
                    }

                    RankedTravelTimes.Add(percentile, TT);
                }
            }
            else
            {
                hasRecords = false;
            }
        }


        public void GetTravelTimes(DateTime startday, DateTime endday, string starthour, string endhour, int confidence,
            List<DayOfWeek> validdays, int binsize)
        {
            BinnedTravelTimes.Clear();
            double binFactor = 60 / binsize;
            expectedRecords = 0;
            var x = 0;
            while (startday.AddDays(x) <= endday)
            {
                var dayPeriodStart = startday.AddDays(x).ToShortDateString() + " " + starthour;
                var dayPeriodEnd = startday.AddDays(x).ToShortDateString() + " " + endhour;
                var start = Convert.ToDateTime(dayPeriodStart);
                var end = Convert.ToDateTime(dayPeriodEnd);

                var dr = DataRepositoryFactory.CreatedataRepository();

                var travelTimes = dr.GetTravelTimes(TMCCode, confidence, validdays, start, end);
                //MOE.Common.Data.Inrix.TravelTimesDataTable travelTimes = TravelTimesTA.GetData(this.TMCCode, confidence, validdays, start, end);
                //what happens when the recordset is empty?
                //average records into bins
                if (binsize > 1)
                {
                    //   expectedRecords += (end.Subtract(start).TotalMinutes / binsize);
                    //    while (start < end)
                    //    {
                    //        var TTs = from MOE.Common.Data.Inrix.TravelTimesRow row in travelTimes.Rows
                    //                  where row.measurement_tstamp >= start && row.measurement_tstamp <= start.AddMinutes(binsize)
                    //                  select row;

                    //        double binTravelTime = 0;
                    //        double binConfidence = 0;
                    //        confidenceAverage = 0;
                    //        binAverageTravelTime = 0;

                    //        foreach (MOE.Common.Data.Inrix.TravelTimesRow row in TTs)
                    //        {
                    //            binTravelTime = binTravelTime + row.travel_time_minutes;
                    //            binConfidence = binConfidence + row.confidence_score;
                    //        }


                    //        if (TTs.Count() > 0)
                    //        {
                    //            confidenceAverage = binConfidence / TTs.Count();
                    //            binAverageTravelTime = (binTravelTime / TTs.Count());
                    //        }

                    //        MOE.Common.Business.Inrix.TravelTimeRecord TTR = new TravelTimeRecord(start, binAverageTravelTime, Convert.ToInt16(Math.Round(confidenceAverage)));


                    //        BinnedTravelTimes.Add(TTR);

                    //        start = start.AddMinutes(binsize);
                    //    }
                }
                else if (binsize == 1)
                {
                    double binTravelTime = 0;
                    double binConfidence = 0;
                    confidenceAverage = 0;
                    binAverageTravelTime = 0;
                    expectedRecords += end.Subtract(start).TotalMinutes;

                    foreach (var row in travelTimes)
                    {
                        var TTR = new TravelTimeRecord(row.measurement_tstamp.Value, row.travel_time_minutes.Value,
                            row.confidence_score.Value);
                        BinnedTravelTimes.Add(TTR);
                        binConfidence = binConfidence + row.confidence_score.Value;
                        binTravelTime = binTravelTime + row.travel_time_minutes.Value;
                    }

                    if (travelTimes.Count > 0)
                    {
                        confidenceAverage = binConfidence / travelTimes.Count;
                        binAverageTravelTime = binTravelTime / travelTimes.Count;
                    }
                }


                //foreach (MOE.Common.Data.Inrix.TravelTimesRow row in travelTimes)
                //{

                //    //rawTravelTimes.AddTravelTimesRow(row);


                //}

                x++;
            }


            CalculateTravelTimePercentile();
        }
    }
}