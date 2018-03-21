using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPM.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPM.Controllers.Tests
{
    [TestClass()]
    public class SignalsControllerTests
    {
        [TestMethod()]
        public void CopyVersionTest()
        {
            SignalsController signalsController = new SignalsController();
            var actionResult = signalsController.CopyVersion(1);
            Assert.IsNotNull(actionResult);
        }
    }
}