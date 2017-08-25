using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOE.Common.Business.Inrix
{
    public class TMC
    {
        protected string tmcCode;
        public string TMCCode
        {
            get
            {
                return tmcCode;
            }
            set
            {
                tmcCode = value;
            }
        }

        protected string direction;
        public string Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
            }
        }

        protected string start;
        public string Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
            }
        }

        protected string stop;
        public string Stop
        {
            get
            {
                return stop;
            }
            set
            {
                stop = value;
            }
        }

        protected double length;
        public double Length
        {
            get
            {
                return length;
            }
            set
            {
                length = value;
            }
        }

        protected string street;
        public string Street
        {
            get
            {
                return street;
            }
            set
            {
                street = value;
            }
        }

        protected int invalidBinCount;
        public int InvalidBinCount
        {
            get
            {
                return invalidBinCount;
            }

        }

        protected double expectedRecords;
        public double ExpectedRecords
        {
            get
            {
                return expectedRecords;
            }
        }

        protected double confidenceAverage;
        public double ConfidenceAverage
        {
            get
            {
                return confidenceAverage;
            }
        }

        protected double binAverageTravelTime;
        public double BinAverageTravelTime
        {
            get
            {
                return binAverageTravelTime;
            }
        }

        protected bool hasRecords;
        public bool HasRecords
        {
            get
            {
                return hasRecords;
            }
        }

        //private MOE.Common.Data.InrixTableAdapters.TravelTimesTableAdapter TravelTimesTA = new Data.InrixTableAdapters.TravelTimesTableAdapter();
        //private MOE.Common.Data.Inrix.TravelTimesDataTable rawTravelTimes = new Data.Inrix.TravelTimesDataTable();
        public List<MOE.Common.Business.Inrix.TravelTimeRecord> BinnedTravelTimes = new List<TravelTimeRecord>();
        public SortedDictionary<double, double> RankedTravelTimes = new SortedDictionary<double, double>();
        /// <summary>
        /// Default Constructor for TMC class
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

        public void IncreaseInvalidBinCount()
        {
            invalidBinCount++;
        }

        private void CalculateTravelTimePercentile()
        {
            RankedTravelTimes.Clear();
            List<Double> sortedTravelTimes = new List<double>();

            foreach (TravelTimeRecord ttr in this.BinnedTravelTimes)
            {
                sortedTravelTimes.Add(ttr.TravelTime);
            }
            sortedTravelTimes.Sort();

            if (sortedTravelTimes.Count > 0)
            {
                this.hasRecords = true;
                double rank = 0;
                for (double percentile = 1; percentile <= 100; percentile++)
                {

                    rank = (percentile / 100) * (sortedTravelTimes.Count + 1);

                    double intPartofRank = Math.Floor(rank);
                    double decPartofRank = rank - intPartofRank;

                    double TT = 0;

                    if (intPartofRank > 0 && intPartofRank < sortedTravelTimes.Count)
                    {
                        double TT1 = sortedTravelTimes[Convert.ToInt16(Math.Floor(rank) - 1)];
                        double TT2 = sortedTravelTimes[Convert.ToInt16(Math.Floor(rank))];
                        TT = TT1 + ((TT2 - TT1) * decPartofRank);
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
                this.hasRecords = false;
            }


        }


        public void GetTravelTimes(DateTime startday, DateTime endday, string starthour, string endhour, int confidence, List<DayOfWeek> validdays, int binsize)
        {

            BinnedTravelTimes.Clear();
            double binFactor = (60 / binsize);
            expectedRecords = 0;
            int x = 0;
            while (startday.AddDays(x) <= endday)
            {
                string dayPeriodStart = startday.AddDays(x).ToShortDateString() + " " + starthour;
                string dayPeriodEnd = startday.AddDays(x).ToShortDateString() + " " + endhour;
                DateTime start = Convert.ToDateTime(dayPeriodStart);
                DateTime end = Convert.ToDateTime(dayPeriodEnd);

                MOE.Common.Models.Inrix.Repositories.IDataRepository dr = MOE.Common.Models.Inrix.Repositories.DataRepositoryFactory.CreatedataRepository();

                List<MOE.Common.Models.Inrix.Datum> travelTimes = dr.GetTravelTimes(this.TMCCode, confidence, validdays, start, end);
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

                    foreach (MOE.Common.Models.Inrix.Datum row in travelTimes)
                    {

                        MOE.Common.Business.Inrix.TravelTimeRecord TTR = new TravelTimeRecord(row.measurement_tstamp.Value, row.travel_time_minutes.Value, row.confidence_score.Value);
                        BinnedTravelTimes.Add(TTR);
                        binConfidence = binConfidence + row.confidence_score.Value;
                        binTravelTime = binTravelTime + row.travel_time_minutes.Value;
                    }

                    if (travelTimes.Count > 0)
                    {
                        confidenceAverage = (binConfidence / travelTimes.Count);
                        binAverageTravelTime = (binTravelTime / travelTimes.Count);
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
