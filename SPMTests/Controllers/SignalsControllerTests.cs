using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Models;
using MOE.CommonTests.Models;
using SPM.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MOE.Common.Models.Repositories;

namespace SPM.Controllers.Tests
{
    [TestClass()]
    public class SignalsControllerTests
    {
       
        private MOE.Common.Models.Repositories.IControllerTypeRepository _controllerTypeRepository = new InMemoryControllerTypeRepository();
        private MOE.Common.Models.Repositories.IRegionsRepository _regionRepository = new InMemoryRegionsRepository();
        private MOE.Common.Models.Repositories.IDirectionTypeRepository _directionTypeRepository = new InMemoryDirectionTypeRepository();
        private MOE.Common.Models.Repositories.IMovementTypeRepository _movementTypeRepository = new InMemoryMovementTypeRepository();
        private MOE.Common.Models.Repositories.ILaneTypeRepository _laneTypeRepository = new InMemoryLaneTypeRepository();
        private MOE.Common.Models.Repositories.IDetectionHardwareRepository _detectionHardwareRepository = new InMemoryDetectionHardwareRepository();
        private MOE.Common.Models.Repositories.ISignalsRepository _signalsRepository = new InMemorySignalsRepository();
        private MOE.Common.Models.Repositories.IDetectorRepository _detectorRepository = new InMemoryDetectorRepository();
        private MOE.Common.Models.Repositories.IDetectionTypeRepository _detectionTypeRepository = new InMemoryDetectionTypeRepository();
        private MOE.Common.Models.Repositories.IApproachRepository _approachRepository = new InMemoryApproachRepository();
        private MOE.Common.Models.Repositories.IMetricTypeRepository _metricTypeRepository = new InMemoryMetricTypeRepository();

        [TestMethod()]
        public void IndexTest()
        {
            var sc = new SignalsController(_controllerTypeRepository, _regionRepository, _directionTypeRepository,
                _movementTypeRepository, _laneTypeRepository, _detectionHardwareRepository, _signalsRepository,
                _detectorRepository, _detectionTypeRepository, _approachRepository, _metricTypeRepository);

            // Act
            var result = sc.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void SignalDetailTest()
        {
            var sc = new SignalsController(_controllerTypeRepository, _regionRepository, _directionTypeRepository,
                _movementTypeRepository, _laneTypeRepository, _detectionHardwareRepository, _signalsRepository,
                _detectorRepository, _detectionTypeRepository, _approachRepository, _metricTypeRepository);

            // Act
            var result = sc.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void AddApproachTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CopyApproachTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddDetectorTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CopyDetectorTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateTest()
        {

           var sc = new SignalsController(_controllerTypeRepository, _regionRepository, _directionTypeRepository,
               _movementTypeRepository, _laneTypeRepository, _detectionHardwareRepository, _signalsRepository,
               _detectorRepository, _detectionTypeRepository, _approachRepository, _metricTypeRepository);


             var result = sc.Create("1001") as PartialViewResult;
            if (result != null)
            {
                var signal = (Signal)result.ViewData.Model;

                Assert.AreEqual(signal.SignalID, "1001");
                
            }
            else
            {
                Assert.Fail();
            }


        }

        [TestMethod]
        public void DeleteSignalVersion()
        {
            var sc = new SignalsController(_controllerTypeRepository, _regionRepository, _directionTypeRepository,
                _movementTypeRepository, _laneTypeRepository, _detectionHardwareRepository, _signalsRepository,
                _detectorRepository, _detectionTypeRepository, _approachRepository, _metricTypeRepository);

            sc.Create("1001");

            var results = _signalsRepository.GetAllVersionsOfSignalBySignalID("1001");

            if (results != null)
            {
                Assert.IsTrue(results.FirstOrDefault().VersionID == 0 && results.FirstOrDefault().SignalID == "1001");
            }

            sc.DeleteVersion(0);

            var results1 = _signalsRepository.GetAllVersionsOfSignalBySignalID("1001");

            if (results1 != null)
            {
                Assert.IsTrue(results1.Count == 0);
               
            }




        }

        [TestMethod]
        public void DeleteAllVersionsOfASignal()
        {
            var sc = new SignalsController(_controllerTypeRepository, _regionRepository, _directionTypeRepository,
                _movementTypeRepository, _laneTypeRepository, _detectionHardwareRepository, _signalsRepository,
                _detectorRepository, _detectionTypeRepository, _approachRepository, _metricTypeRepository);

            sc.Create("1001");

            sc.AddNewVersion("1001");

            var results = _signalsRepository.GetAllVersionsOfSignalBySignalID("1001");

            if (results != null)
            {
                Assert.IsTrue(results.Count > 1);
            }

            sc.Delete("1001");

            var results1 = _signalsRepository.GetAllVersionsOfSignalBySignalID("1001");

            Assert.IsTrue(results1.Count == 0);
        }


        [TestMethod()]
        public void CreateExistingSgnalIdShouldReturnErrorMessage()
        {

            var sc = new SignalsController(_controllerTypeRepository, _regionRepository, _directionTypeRepository,
                _movementTypeRepository, _laneTypeRepository, _detectionHardwareRepository, _signalsRepository,
                _detectorRepository, _detectionTypeRepository, _approachRepository, _metricTypeRepository);

            sc.Create("1001");

            var result = sc.Create("1001") as ContentResult;

            if (result != null)
            {
                var content = (string) result.Content;

                Assert.AreEqual(content, "<h1>Signal Already Exists</h1>");
            }
            else
            {
                Assert.Fail();
            }


        }


        [TestMethod()]
        public void AddNewVersionOfExistingSignal()
        {
            var sc = new SignalsController(_controllerTypeRepository, _regionRepository, _directionTypeRepository,
                _movementTypeRepository, _laneTypeRepository, _detectionHardwareRepository, _signalsRepository,
                _detectorRepository, _detectionTypeRepository, _approachRepository, _metricTypeRepository);

            sc.Create("1001"); 

            sc.AddNewVersion("1001");

            var results = _signalsRepository.GetAllVersionsOfSignalBySignalID("1001");

            if (results != null)
            {
                    Assert.IsTrue(results.Count == 2);
                    Assert.AreEqual(results.First().SignalID, results.Last().SignalID);
                Assert.AreNotEqual(results.First().VersionID, results.Last().VersionID);
            }
            else
            {

                Assert.Fail();
            }


        }

        [TestMethod]
        public void GetSelectListOfAvailableVersions()
        {
            var sc = new SignalsController(_controllerTypeRepository, _regionRepository, _directionTypeRepository,
                _movementTypeRepository, _laneTypeRepository, _detectionHardwareRepository, _signalsRepository,
                _detectorRepository, _detectionTypeRepository, _approachRepository, _metricTypeRepository);

            sc.Create("1001");

            sc.AddNewVersion("1001");


        }



        [TestMethod()]
        public void CopyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void _SignalPartialTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SignalDetailResultTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteTest()
        {
            var sc = new SignalsController(_controllerTypeRepository, _regionRepository, _directionTypeRepository,
                _movementTypeRepository, _laneTypeRepository, _detectionHardwareRepository, _signalsRepository,
                _detectorRepository, _detectionTypeRepository, _approachRepository, _metricTypeRepository);

            sc.Create("1001");

            sc.AddNewVersion("1001");

            var results = _signalsRepository.GetAllVersionsOfSignalBySignalID("1001");

            if (results != null)
            {
                Assert.IsTrue(results.Count > 1);
            }

            sc.Delete("1001");

            var results1 = _signalsRepository.GetAllVersionsOfSignalBySignalID("1001");

            Assert.IsTrue(results1.Count == 0);
        }

        [TestMethod()]
        public void DeleteConfirmedTest()
        {
            Assert.Fail();
        }
    }

   
}