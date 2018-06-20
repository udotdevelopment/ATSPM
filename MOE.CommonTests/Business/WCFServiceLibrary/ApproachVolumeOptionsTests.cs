using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.WCFServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.WCFServiceLibrary.Tests
{
    [TestClass()]
    public class ApproachVolumeOptionsTests
    {
        [TestMethod()]
        public void CreateMetricTest()
        {
            ApproachVolumeOptions options = new ApproachVolumeOptions("7185", new DateTime(2018, 2, 1, 0, 0, 0), new DateTime(2018, 2, 1, 23, 59, 0), null, 15, true, true, true, true, true, true);
            options.CreateMetric();
        }
    }
}