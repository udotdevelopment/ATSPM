using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.WCFServiceLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;

namespace MOE.Common.Business.WCFServiceLibrary.Tests
{
    [TestClass()]
    public class SignalAggregationMetricOptionsTests
    {
        [TestMethod()]
        public void SetEventCountSeriesTest()
        {
            SignalPreemptionAggregationOptions options = new SignalPreemptionAggregationOptions();
            Series y = new Series();
            Series s = options.SetEventCountSeries(y);

            Assert.IsTrue(s.Color == Color.Black);
        }

        [TestMethod()]
        public void SetEventCountSeriesWithSplitFailTest()
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            Series y = new Series();
            Series s = options.SetEventCountSeries(y);

            Assert.IsTrue(s.Color == Color.Black);
        }
    }
}