using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
    public class Pin : object
    {

        protected string signalID;
        public string SignalID
        {
            get
            {
                return signalID;
            }
        }

        protected string region;
        public string Region
        {
            get
            {
                return region;
            }
        }

        protected string latitude;
        public string Latitude
        {
            get
            {
                return latitude;
            }
        }

        protected string longitude;
        public string Longitude
        {
            get
            {
                return longitude;
            }
        }

        protected string description;
        public string Description
        {
            get
            {
                return description;
            }
        }

        protected Business.InfoBox box;
        public Business.InfoBox Box
        {
            get
            {
                return box;
            }
        }

        public string MetricTypes { get; set; }


        /// <summary>
        /// Default Constructor Used for Map Pins
        /// </summary>
        /// <param name="number"></param>
        /// <param name="lat"></param>
        /// <param name="Long"></param>
        /// <param name="desc"></param>
        /// <param name="hasPCD"></param>
        /// <param name="hasSpeed"></param>
        /// <param name="hasPhase"></param>
        /// <param name="hasTMC"></param>
        /// <param name="Region"></param>

        
        //public Pin(string signalId, string lat, string Long, string desc, bool hasPCD, bool hasSpeed, bool hasPhase, bool hasTMC, bool hasRLM, bool hasSplitFail, string Region)
        //{

        //    signalID = signalId;
        //    latitude = lat;
        //    longitude = Long;
        //    description = desc;
        //    region = Region;
            


        //    SortedDictionary<int, bool> reports = FindReports(hasPCD,hasSpeed, hasPhase, hasTMC, hasRLM, hasSplitFail);
        //    box = new InfoBox(signalId, description, reports);
        //}

        public Pin(string signalId, string lat, string Long, string desc, string Region)
        {

            signalID = signalId;
            latitude = lat;
            longitude = Long;
            description = desc;
            region = Region;
            
           
            //box = new InfoBox(signalId, description, reports);
        }


        /// <summary>
        /// Find the avialable report for the pin.
        /// </summary>
        /// <param name="hasPCD"></param>
        /// <param name="hasSpeed"></param>
        /// <param name="hasPhase"></param>
        /// <param name="hasTMC"></param>
        /// <returns>SortedDictionary<int, Boolean></returns>
        private SortedDictionary<int, Boolean> FindReports(bool hasPCD, bool hasSpeed, bool hasPhase, bool hasTMC, bool hasRLM, bool hasSplitFail)
    {
            SortedDictionary<int,Boolean> reports = new SortedDictionary<int,bool>();

            if(hasPCD)
            {
                reports.Add(1,true);
            }
            else
            {
                reports.Add(1,false);
            }

            if(hasSpeed)
            {
                reports.Add(2,true);
            }
            else
            {
                reports.Add(2,false);
            }

            if(hasPhase)
            {
                reports.Add(3,true);
            }
            else
            {
                reports.Add(3,false);
            }

            if (hasTMC)
            {
                reports.Add(4, true);
            }
            else
            {
                reports.Add(4, false);
            }
            if (hasRLM)
            {
                reports.Add(5, true);
            }
            else
            {
                reports.Add(5, false);
            }
            if (hasSplitFail)
            {
                reports.Add(6, true);
            }
            else
            {
                reports.Add(6, false);
            }
            //PED Delay on all pins
            reports.Add(7, true);

            return reports;
    }
        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            Pin y = (Pin)obj;
            return this != null && y != null && this.signalID == y.signalID 
                ;
        }



        public override int GetHashCode()
        {
            return this == null ? 0 : (this.SignalID.GetHashCode() ^ this.Region.GetHashCode());
        }
    }

}