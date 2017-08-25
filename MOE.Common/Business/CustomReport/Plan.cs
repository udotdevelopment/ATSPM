using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.CustomReport
{
    public class Plan
    {
        private DateTime _StartDate;

        public DateTime StartDate
        {
            get { return _StartDate; }
        }

        private DateTime _EndDate;

        public DateTime EndDate
        {
            get { return _EndDate; }
        }

        private int _PlanNumber;

        public int PlanNumber
        {
            get { return _PlanNumber; }
        }

        
        
        public Plan(string signalID, DateTime startDate, DateTime endDate, int planNumber)
        {
            _StartDate = startDate;
            _EndDate = endDate;
            _PlanNumber = planNumber;
        }

        

    }
}
