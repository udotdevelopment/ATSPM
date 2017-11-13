using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business
{
    public class PlanBase
    {
        public DateTime PlanStart { get; set; }
        public DateTime PlanEnd { get; set; }
        public int PlanNumber { get; set; }
    }
}
