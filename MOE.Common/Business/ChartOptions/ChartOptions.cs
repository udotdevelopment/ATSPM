using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOE.Common.Business
{
    public class ChartOptions
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int YAxisMax { get; set; }
        public bool UploadCurrent { get; set; }

    }
}
