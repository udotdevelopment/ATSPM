using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.WCFServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.CommonTests.Helpers;
using MOE.CommonTests.Models;

namespace MOE.Common.Business.WCFServiceLibrary.Tests
{
    [TestClass()]
    public class SplitFailOptionsTests
    {
        [TestMethod()]
        public void CreateMetricTest()
        {
            string signalId = "7220";
            DateTime start = Convert.ToDateTime("10/17/2017 17:00:00");
            DateTime end = Convert.ToDateTime("10/17/2017 18:00:00");

            SplitFailOptions sfo = new SplitFailOptions(signalId, start, end,12,5,true,true,false);
            List<string> path = sfo.CreateMetric();

            Assert.IsTrue(path.Count > 0);
        }

        [TestMethod()]
        public void GetRecordsFromXMLTest()
        {
            InMemoryMOEDatabase _db = new InMemoryMOEDatabase();

            //XMLToListImporter.LoadConterollerEventLog("7185_10_17_2017.xml", _db);

            Assert.IsTrue(_db.Controller_Event_Log.Count > 1000);
        }

        [TestMethod()]
        public void SomeSpliTFailTest()
        {
            InMemoryMOEDatabase _db = new InMemoryMOEDatabase();

            //XMLToListImporter.LoadConterollerEventLog("7185_10_17_2017.xml", _db);




        }





    }
}