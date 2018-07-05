using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Tests
{
    [TestClass()]
    public class DetectorTests
    {
        [TestMethod()]
        public void CopyDetectorTest()
        {
           var newDet = MOE.Common.Models.Detector.CopyDetector(30, false);
            Assert.IsTrue(newDet.DetectionTypes.Count > 0);
        }
    }
}