using System;
using System.Collections.Generic;

namespace MOE.Common
{
    internal class RouteConsistencyCheck
    {
        public RouteConsistencyCheck(DateTime startDate, DateTime endDate, string startHour, string endHour,
            string dayTypes, int routeID)
        {
            StartDate = startDate;
            EndDate = endDate;
            StartHour = startHour;
            EndHour = endHour;
            DayTypes = dayTypes;
            RouteID = routeID;
        }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string EndHour { get; set; }

        public string StartHour { get; set; }

        public string DayTypes { get; set; }

        public int RouteID { get; set; }

        public SortedDictionary<int, int> CycleLengthCheck(DateTime startDate, DateTime endDate, int routeID)
        {
            var CycleLengths = new SortedDictionary<int, int>();

            return CycleLengths;
        }

        public bool PatternCycleLenghtConsitencyCheck(DateTime startDate, DateTime endDate, string signalId,
            int patternNumber)
        {
            var PatternIsConsitent = true;


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