using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common;

namespace MOE.Common.Business
{
    public class SPMChartOptions
    {
        public bool ShowSBEBVolume { get; set; }
        public bool ShowNBWBVolume { get; set; }
        public bool ShowDirectionalSplits { get; set; }
        public int VolumeBinSize { get; set; }
        public double yAxisMaximum { get; set; }
        public bool ShowTMCDetection { get; set; }
        public bool ShowAdvanceDetection { get; set; }

        public SPMChartOptions()
        { }
    }
}
