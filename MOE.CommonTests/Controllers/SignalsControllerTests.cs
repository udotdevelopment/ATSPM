using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPM.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace SPM.Controllers.Tests
{
    [TestClass()]
    public class SignalsControllerTests
    {
        [TestMethod()]
        public void CopyVersionTest()
        {
            var signalsRepository = SignalsRepositoryFactory.Create();
            Signal signal = signalsRepository.GetLatestVersionOfSignalBySignalID("7185");
            SignalsController signalsController = new SignalsController();
            var actionResult = signalsController.CopyVersion(signal);
            Assert.IsNotNull(actionResult);
        }
    }
}