using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace MOE.Common.Business.Inrix
{
    public class TMCCollection
    {
 
        public int Count
        {
            get
            {
                return Items.Count;
            }
        }



        

        protected List<MOE.Common.Business.Inrix.TMC> Items = new List<TMC>();
        public List<MOE.Common.Business.Inrix.TMC> BadTMCs = new List<TMC>();
        public SortedDictionary<double, double> RankedTravelTimes = new SortedDictionary<double, double>();

        public void AddItem(MOE.Common.Business.Inrix.TMC tmc)
        {
            if (!Items.Contains(tmc))
            {
                Items.Add(tmc);
            }
        }

        public void RemoveItem(MOE.Common.Business.Inrix.TMC tmc)
        {
            if (Items.Contains(tmc))
            {
                Items.Remove(tmc);
            }
        }


        public void ClearItems()
        {
            Items.Clear();
        }

        public TMCCollection()
        {
        }

        public void CalculateTravelTimes(DateTime startday, DateTime endday, string starthour, string endhour, int confidence, List<DayOfWeek> validdays, int binsize, ref DataTable InrixReliabilityDT, int rangeID)
        {
            //double avgConfidence = 0;
            //double averageTT = 0;
            //double confidenceTotal = 0;
            //double perGoodBins = 0;

           SortedDictionary<DateTime, double> summedTravelTimes = new SortedDictionary<DateTime, double>();
           
           List<DateTime> DeadBins = new List<DateTime>();
           
            foreach (TMC tmc in Items)
            {
                //SortedDictionary<DateTime, double> summedTMCTravelTimes = new SortedDictionary<DateTime, double>();
                //List<DateTime> deadTMCBins = new List<DateTime>();
               // deadTMCBins.Clear();
                //summedTMCTravelTimes.Clear();
                double avgConfidence = 0;
                double averageTT = 0;
                double confidenceTotal = 0;
                double perGoodBins = 0;

                tmc.GetTravelTimes(startday, endday, starthour, endhour, confidence, validdays, binsize);

                if (tmc.HasRecords)
                {
                    

                    foreach (KeyValuePair<double, double> record in tmc.RankedTravelTimes)
                    {

                        if (RankedTravelTimes.ContainsKey(record.Key))
                        {

                            RankedTravelTimes[record.Key] = RankedTravelTimes[record.Key] + record.Value;
                        }
                        else
                        {
                            RankedTravelTimes.Add(record.Key, record.Value);
                        }
                    }
                }
                else
                {
                    BadTMCs.Add(tmc);
                }

                double travelTimes = 0;

                foreach (TravelTimeRecord record in tmc.BinnedTravelTimes)
                {
                   travelTimes += record.TravelTime;
                   confidenceTotal += record.Confidence;
                }
                //{

                //   //

                //   // if (record.TravelTime <= 0)
                //   // {
                //   //     if (!DeadBins.Contains(record.TimeStamp))
                //   //     {
                //   //         DeadBins.Add(record.TimeStamp);
                //   //     }
                //   //     tmc.IncreaseInvalidBinCount();
                        
                //   // }

                //   //if (summedTravelTimes.ContainsKey(record.TimeStamp))
                //   //     {
                //   //         summedTravelTimes[record.TimeStamp] += record.TravelTime;
                           
                //   //     }
                //   //     else
                //   //     {
                //   //         summedTravelTimes.Add(record.TimeStamp, record.TravelTime);
                //   //     }

                //   //if (summedTMCTravelTimes.ContainsKey(record.TimeStamp))
                //   //{
                //   //    summedTMCTravelTimes[record.TimeStamp] += record.TravelTime;

                //   //}
                //   //else
                //   //{
                //   //    summedTMCTravelTimes.Add(record.TimeStamp, record.TravelTime);
                //   //}
                //}

                //foreach (DateTime badBin in deadTMCBins)
                //{
                //    if (summedTMCTravelTimes.ContainsKey(badBin))
                //    {
                //        summedTMCTravelTimes.Remove(badBin);
                //    }
                //}


                //double goodBins = tmc.ExpectedRecords - summedTMCTravelTimes.Count;


                //if (goodBins > 0)
                //{
                //    //double sums = summedTravelTimes.Values.Sum();
                //    averageTT = summedTMCTravelTimes.Values.Sum() / goodBins;

                //    //perGoodBins = (goodBins / Convert.ToDouble(tmc.BinnedTravelTimes.Count)) * 100;
                //    perGoodBins = (goodBins / tmc.ExpectedRecords) * 100;
                //}


                averageTT = travelTimes / tmc.BinnedTravelTimes.Count;
               avgConfidence = confidenceTotal / tmc.BinnedTravelTimes.Count;
               if (tmc.BinnedTravelTimes.Count < tmc.ExpectedRecords)
               {
                   perGoodBins = (tmc.BinnedTravelTimes.Count / tmc.ExpectedRecords) * 100;
               }
               else
               {
                   perGoodBins = 100;
               }
                string range = startday.ToShortDateString() + " - " + endday.ToShortDateString() + "\n From: " + starthour + " To: " + endhour;
                double sumOfSquaresOfDifferences = tmc.RankedTravelTimes.Values.Select(val => (val - averageTT) * (val - averageTT)).Sum();
                double StdDev = Math.Sqrt(sumOfSquaresOfDifferences / tmc.RankedTravelTimes.Count);
                
                string TMCName = tmc.Street + " From: " + tmc.Start + " To: " + tmc.Stop;

                


                AddRowsToTable(ref InrixReliabilityDT, tmc.TMCCode, range, averageTT, StdDev, perGoodBins, avgConfidence, TMCName, tmc.Length.ToString(), rangeID);
            }

            //foreach (DateTime badBin in DeadBins)
            //{
            //    if (summedTravelTimes.ContainsKey(badBin))
            //    {
            //        summedTravelTimes.Remove(badBin);
            //    }
            //}

            //var items = from pair in summedTravelTimes
            //        orderby pair.Value
            //        select pair;

            //double itemCount = items.Count();
            //double x = 1;
            //foreach (KeyValuePair<DateTime, double> item in items)
            //{
            //    if (itemCount > 0)
            //    {
                   
            //        double percentile = Math.Round(((x / itemCount)*100),2);
            //        if (!RankedTravelTimes.ContainsKey(percentile))
            //        {
            //        RankedTravelTimes.Add(percentile, item.Value);
            //        }
            //    }
            //    x++;

            //}



        }


        public void AddRowsToTable(ref DataTable InrixReliabilityDT, string tmc_code, string range, double avg_tt, double std_dev, double perc_good_bins, double avg_confidence, string tmc_name, string tmc_length, int date_rangeID)
        {


            InrixReliabilityDT.Rows.Add(new Object[] { tmc_code,
                                                        tmc_name,
                                                        date_rangeID.ToString(),
                                                       range, 
                                                       String.Format("{0:0.##}", Convert.ToDouble(tmc_length)),
                                                       avg_tt.ToString("N2"), 
                                                       std_dev.ToString("N2"), 
                                                       perc_good_bins.ToString("N0") + "%",
                                                       avg_confidence.ToString("N0"),
                                                       });

            

        }

    }
}
