using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
    public class AvgSpeedBucket
    {
        //Xaxis is time
        private DateTime xAxis;
        public DateTime XAxis
        {
            get
            {
                return xAxis; 
            }
            set
            {
                xAxis = value;
            }
        }

        private DateTime totalHits;
        public DateTime TotalHits
        {
            get
            {
                return totalHits;
            }
            set
            {
                totalHits = value;
            }
        }

        private DateTime totalMPH;
        public DateTime TotalMPH
        {
            get
            {
                return totalMPH;
            }
            set
            {
                totalMPH = value;
            }
        }

        private DateTime startTime;
        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
            
        }

        private DateTime endTime;
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
            
        }

        private int avgSpeed;
        public int AvgSpeed
        {
            get
            {
                return avgSpeed;
            }
        }

        private int eightyFifth;
        public int EightyFifth
        {
            get
            {
                return eightyFifth;
            }
        }

        private int minSpeedFilter;
        public int MinSpeedFilter
        {
            get
            {
                return minSpeedFilter;
            }
        }

        private int movementDelay;
        public int MovementDelay
        {
            get
            {
                return movementDelay;
            }
        }



        private int binSizeMultiplier;

        public AvgSpeedBucket(DateTime startTime, DateTime endTime, List<Cycle> CycleCollection, int binSize, int minspeedfilter, int movementdelay)
        {
            this.startTime = startTime;
            this.endTime = endTime;
            this.xAxis = endTime;
            this.binSizeMultiplier = 60 / binSize;
            this.avgSpeed = AddspeedHitToAverage(CycleCollection).Item1;
            this.eightyFifth = AddspeedHitToAverage(CycleCollection).Item2;
            this.minSpeedFilter = minspeedfilter;
            this.movementDelay = movementdelay;
        }

        public Tuple<int, int> AddspeedHitToAverage(List<Cycle> CycleCollection)
        {
            int AverageSpeed;
            int TotalSpeed = 0;
            int TotalHits = 0;
            int Eightyfifth = 0;
            int EighyFiveIndexInt = 0;
            List<int> Speeds = new List<int>();
            
            

                

                var query = from cycle in CycleCollection
                            where (cycle.StartTime > this.startTime && cycle.EndTime < this.endTime)
                            select cycle;

            foreach (var Cy in query)
            {
                
                foreach (Models.Speed_Events SH in Cy.SpeedsForCycle)
                {

                        TotalSpeed = TotalSpeed + SH.MPH;
                        Speeds.Add(SH.MPH);
                        TotalHits++;
                        //TotalHits = TotalHits + Cy.SpeedCollection.Items.Count;
                    
                }
            }

            if (TotalHits > 0)
            {
            double RawAverageSpeed = (TotalSpeed / TotalHits);
            AverageSpeed = Convert.ToInt32( Math.Round(RawAverageSpeed));
            double EighyFiveIndex =  ((TotalHits * .85) + .5);
            Speeds.Sort();
            if (Speeds.Count > 3)
            {
                if ((EighyFiveIndex % 1) == 0)
                {
                    EighyFiveIndexInt = Convert.ToInt16(EighyFiveIndex);
                    Eightyfifth = Speeds.ElementAt(EighyFiveIndexInt - 2);
                }
                else
                {
                    double IndexMod = (EighyFiveIndex % 1);
                    EighyFiveIndexInt = Convert.ToInt16(EighyFiveIndex);
                    int Speed1 = Speeds.ElementAt(EighyFiveIndexInt - 2);
                    int Speed2 = Speeds.ElementAt(EighyFiveIndexInt - 1);
                    double RawEightyfifth = (1 - IndexMod) * Speed1 + IndexMod * Speed2;
                    Eightyfifth = Convert.ToInt32(Math.Round(RawEightyfifth));
                }
            }
            
               
                

 
            }
            else
            {
                AverageSpeed = 0;
                Eightyfifth = 0;
            }

            return new Tuple<int, int>(AverageSpeed, Eightyfifth);

                }

        

            }


        }
