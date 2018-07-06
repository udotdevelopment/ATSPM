using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.CommonTests.Models;

namespace MOE.CommonTests.Helpers
{
    [TestClass()]
    public class XmlHelperTests
    {
        [TestMethod()]
        public void LoadControllerEventLogFromXmlTest()
        {
            var db = new InMemoryMOEDatabase();

            XmlToListImporter.LoadControllerEventLog("7185Events02_01_2018.Xml", db);

            Assert.IsTrue(db.Controller_Event_Log.Count > 1000);
        }

        [TestMethod()]
        public void LoadSignalsFromXmlTest()
        {
            InMemoryMOEDatabase db = new InMemoryMOEDatabase();

            XmlToListImporter.LoadSignals("signals.Xml", db);

            Assert.IsTrue(db.Signals.Count > 1000);
        }
        [TestMethod()]
        public void LoadApproachesFromXmlTest()
        {
            InMemoryMOEDatabase db = new InMemoryMOEDatabase();

            XmlToListImporter.LoadApproaches("approachesfor7185.Xml", db);

            Assert.IsTrue(db.Approaches.Count > 7);
        }

        [TestMethod()]
        public void ApproachesAreAssignedTosignalsTest()
        {
            InMemoryMOEDatabase db = new InMemoryMOEDatabase();

            XmlToListImporter.LoadSignals("signals.Xml", db);

            XmlToListImporter.LoadApproaches("approachesfor7185.Xml", db);

            foreach (var app in db.Approaches)
            {
                Assert.IsNotNull(app.Signal);
            }
        }

        [TestMethod()]
        public void LoadDetectorsFromXmlTest()
        {
            InMemoryMOEDatabase db = new InMemoryMOEDatabase();

            XmlToListImporter.LoadDetectors("detectorsFor7185.Xml", db);

            Assert.IsTrue(db.Detectors.Count > 7);
        }

        [TestMethod()]
        public void AddDetectionTypesToDetectorsFromXmlTest()
        {
            InMemoryMOEDatabase db = new InMemoryMOEDatabase();

            XmlToListImporter.LoadDetectors("detectorsFor7185.Xml", db);

            XmlToListImporter.AddDetectionTypesToDetectors("DetectorTypesforDetectorsFor7185.Xml", db);

            Assert.IsTrue(db.Detectors.Count > 7);
            Assert.IsTrue(db.Detectors.FirstOrDefault().DetectionTypeIDs.Count > 0);

        }

        [TestMethod()]
        public void LoadSpeedEventsFromXmlTest()
        {
            InMemoryMOEDatabase db = new InMemoryMOEDatabase();

            XmlToListImporter.LoadSpeedEvents("7185Speed.xml", db);

            Assert.IsTrue(db.Speed_Events.Count > 100);
        }
    }
}
