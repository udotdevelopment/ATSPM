using Microsoft.VisualStudio.TestTools.UnitTesting;
using FTPfromAllControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Data;
using MOE.Common.Business;

namespace FTPfromAllControllers.Tests
{
    [TestClass()]
    public class FTPfromAllControllersTests
    {
        private MOE.Common.Business.Signal _signal;

        [TestInitialize()]
        public void Initialize()
        {
            _signal = new Signal();
            _signal.SignalId = "9797";
        }

        [TestMethod()]
        public void ZeroIPRetunsFalse()
        {
            _signal.IpAddress = "0";
            Assert.IsFalse(FTPfromAllControllers.CheckIfIPAddressIsValid(_signal));
        }

        [TestMethod()]
        public void ZeroDotZeroIPRetunsFalse()
        {
            _signal.IpAddress = "0.0.0.0";
            Assert.IsFalse(FTPfromAllControllers.CheckIfIPAddressIsValid(_signal));
        }
        [TestMethod()]
        public void BlankIPRetunsFalse()
        {
            Assert.IsFalse(FTPfromAllControllers.CheckIfIPAddressIsValid(_signal));
        }

        [TestMethod()]
        public void NonsenseIPRetunsFalse()
        {
            _signal.IpAddress = "Nonsense";
            Assert.IsFalse(FTPfromAllControllers.CheckIfIPAddressIsValid(_signal));
        }
        [TestMethod()]
        public void UnreachableIPRetunsFalse()
        {
            _signal.IpAddress = "121.121.123.123";
            Assert.IsFalse(FTPfromAllControllers.CheckIfIPAddressIsValid(_signal));
        }

        [TestMethod()]
        public void GoodIPRetunsTrue()
        {
            _signal.IpAddress = "127.0.0.1";
            Assert.IsTrue(FTPfromAllControllers.CheckIfIPAddressIsValid(_signal));
        }

    }
}