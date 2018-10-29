using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.SplitFail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Runtime.Serialization;
using System.Threading;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using MOE.Common.Business.WCFServiceLibrary;
using System.Web.UI.DataVisualization.Charting;

namespace MOE.Common.Business.SplitFail.Tests
{
    [TestClass()]
    public class SplitFailChartChartTests
    {
        public Chart Chart;
        [global::Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]

        public void SplitFailChartSplitFailChartTest()
        {
            string signalId = "6311".ToString();
            DateTime startTime = global::System.Convert.ToDateTime("01/01/2014 2:00 AM");
            DateTime endTime = global::System.Convert.ToDateTime("1/1/2014 2:00 PM");
            int metricId = 12;
            int firstSecondOfRed = 23;
           
           SplitFailOptions options = new SplitFailOptions(signalId, startTime, endTime, metricId, firstSecondOfRed, false, false, false);


            global::Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail();
        }
    }
}