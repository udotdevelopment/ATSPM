using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOE.Common
{
    class RouteConsistencyCheck
    {
        private DateTime startdate;
        public DateTime StartDate
        {
            get
            {
                return startdate;
            }
            set
            {
                startdate = value;
            }
        }

        private DateTime enddate;
        public DateTime EndDate
        {
            get
            {
                return enddate;
            }
            set
            {
                enddate = value;
            }
        }

        private String endhour;
        public String EndHour
        {
            get
            {
                return endhour;
            }
            set
            {
                endhour = value;
            }
        }

        private String starthour;
        public String StartHour
        {
            get
            {
                return starthour;
            }
            set
            {
                starthour = value;
            }
        }

        private String daytypes;
        public String DayTypes
        {
            get
            {
                return daytypes;
            }
            set
            {
                daytypes = value;
            }
        }

        private int routeid;
        public int RouteID
        {
            get
            {
                return routeid;
            }
            set
            {
                routeid = value;
            }
        }

        public RouteConsistencyCheck(DateTime startDate, DateTime endDate, String startHour, String endHour, string dayTypes, int routeID)
        {
            startdate = startDate;
            enddate = endDate;
            starthour = startHour;
            endhour = endHour;
            daytypes = dayTypes;
            routeid = routeID;


        }

        public SortedDictionary<int, int> CycleLengthCheck(DateTime startDate, DateTime endDate, int routeID)
        {
            SortedDictionary<int, int> CycleLengths = new SortedDictionary<int, int>();

            return CycleLengths;

        }

        public bool PatternCycleLenghtConsitencyCheck(DateTime startDate, DateTime endDate, String signalId, int patternNumber)
        {
            bool PatternIsConsitent = true;


                return PatternIsConsitent;
        }

        private void GetRouteMembers(int routeID)
        {

        }

        private void GetPAtternInfo(DateTime startDate, DateTime endDate, string signalId)
        {
        }

    }
}
