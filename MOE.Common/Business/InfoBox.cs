using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
   
    public class InfoBox
    {

        protected string id;
        public string ID
        {
            get
            {
                return id;
            }
        }

        protected string location;
        public string Location
        {
            get
            {
                return location;
            }
        }


        protected SortedDictionary<int, bool> availReports;
        public SortedDictionary<int, bool> AvailReports
        {
        get
        {
            return availReports;
        }
        }
  

        /// <summary>
        /// Default constructor for the infobox class.
        /// Usually there will be one of these per pin.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="loc"></param>
        /// <param name="reports"></param>
        public InfoBox(string signalId, string loc, SortedDictionary<int,bool> reports)
        {

            id = signalId;
            location = loc;
            availReports = reports;
  

        }
    }
}