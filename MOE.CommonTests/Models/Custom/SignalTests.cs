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
    public class SignalTests
    {
        [TestMethod()]
        public void CopyVersionTest()
        {
            var signalRepository = Repositories.SignalsRepositoryFactory.Create();
            Signal signal = signalRepository.GetLatestVersionOfSignalBySignalID("7064");
            Signal signalCopy = Signal.CopyVersion(signal);
            Assert.IsFalse(signal.VersionID == signalCopy.VersionID);
            Assert.IsTrue(signal.VersionList.Count == signalCopy.VersionList.Count);
            Assert.IsFalse(signal.Start == signalCopy.Start);
            Assert.IsFalse(signal.SelectListName == signalCopy.SelectListName);
            Assert.IsNotNull(signalCopy);
        }
    }
}