using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Models;
using MOE.CommonTests.Models;
using MOE.CommonTests;
using SPM.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using System.Web.Mvc;
using MOE.Common.Models.Repositories;

namespace SPM.Controllers.Tests
{
    [TestClass()]
    public class SignalsControllerTests
    {
        public static InMemoryMOEDatabase Db = new InMemoryMOEDatabase();

        private MOE.Common.Models.Repositories.IControllerTypeRepository _controllerTypeRepository =
            new InMemoryControllerTypeRepository(Db);

        private MOE.Common.Models.Repositories.IRegionsRepository _regionRepository = new InMemoryRegionsRepository(Db);

        private MOE.Common.Models.Repositories.IDirectionTypeRepository _directionTypeRepository =
            new InMemoryDirectionTypeRepository(Db);

        private MOE.Common.Models.Repositories.IMovementTypeRepository _movementTypeRepository =
            new InMemoryMovementTypeRepository(Db);

        private MOE.Common.Models.Repositories.ILaneTypeRepository _laneTypeRepository =
            new InMemoryLaneTypeRepository(Db);

        private MOE.Common.Models.Repositories.IDetectionHardwareRepository _detectionHardwareRepository =
            new InMemoryDetectionHardwareRepository(Db);

        private MOE.Common.Models.Repositories.ISignalsRepository _signalsRepository =
            new InMemorySignalsRepository(Db);

        private MOE.Common.Models.Repositories.IDetectorRepository _detectorRepository =
            new InMemoryDetectorRepository(Db);

        private MOE.Common.Models.Repositories.IDetectionTypeRepository _detectionTypeRepository =
            new InMemoryDetectionTypeRepository(Db);

        private MOE.Common.Models.Repositories.IApproachRepository _approachRepository =
            new InMemoryApproachRepository(Db);

        private MOE.Common.Models.Repositories.IMetricTypeRepository _metricTypeRepository =
            new InMemoryMetricTypeRepository(Db);


        [TestInitialize]
        public void Initialize()
        {
            MOE.Common.Models.Repositories.DetectorRepositoryFactory.SetDetectorRepository(_detectorRepository);
            MOE.Common.Models.Repositories.DirectionTypeRepositoryFactory.SetDirectionsRepository(_directionTypeRepository);
            MOE.Common.Models.Repositories.DetectionTypeRepositoryFactory.SetDetectionTypeRepository(_detectionTypeRepository);
            MOE.Common.Models.Repositories.SignalsRepositoryFactory.SetSignalsRepository(_signalsRepository);
        }

        [TestMethod()]
        public void IndexTest()
        {
            Db.ClearTables();

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
            Db.ClearTables();

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
            Db.ClearTables();

            var sc = new SignalsController(_controllerTypeRepository, _regionRepository, _directionTypeRepository,
                _movementTypeRepository, _laneTypeRepository, _detectionHardwareRepository, _signalsRepository,
                _detectorRepository, _detectionTypeRepository, _approachRepository, _metricTypeRepository);

            sc.Create("1001");
            var version = _signalsRepository.GetLatestVersionOfSignalBySignalID("1001");

            
                           
            var result = sc.AddApproach(version.VersionID.ToString()) as PartialViewResult;
            if (result != null)
            {
                var appr = (Approach)result.ViewData.Model;

                Assert.AreEqual(appr.Description, "New Phase/Direction");

            }
            else
            {
                Assert.Fail();
            }


        }

        [TestMethod()]
        public void CopyApproachTest()
        {
            Db.ClearTables();

            var sc = new SignalsController(_controllerTypeRepository, _regionRepository, _directionTypeRepository,
                _movementTypeRepository, _laneTypeRepository, _detectionHardwareRepository, _signalsRepository,
                _detectorRepository, _detectionTypeRepository, _approachRepository, _metricTypeRepository);

            var sigresult = sc.Create("1001") as PartialViewResult;

            var sig = (Signal)sigresult.ViewData.Model;

            var apprResult =  sc.AddApproach(sig.VersionID.ToString()) as PartialViewResult;
            var appr = (Approach)apprResult.ViewData.Model;

            MOE.Common.Models.Repositories.ApproachRepositoryFactory.SetApproachRepository(_approachRepository);

            var result = sc.CopyApproach(sig.VersionID,appr.ApproachID) as ContentResult;
            
            if (result != null)
            {
                

                Assert.AreEqual(result.Content, "<h1>Copy Successful!</h1>");

            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void AddDetectorTest()
        {
            Db.ClearTables();

            var sc = new SignalsController(_controllerTypeRepository, _regionRepository, _directionTypeRepository,
             _movementTypeRepository, _laneTypeRepository, _detectionHardwareRepository, _signalsRepository,
             _detectorRepository, _detectionTypeRepository, _approachRepository, _metricTypeRepository);

            var sigresult = sc.Create("1001") as PartialViewResult;

            var sig = (Signal)sigresult.ViewData.Model;
            sig.Approaches = new List<Approach>();
            MOE.Common.Models.Repositories.ApproachRepositoryFactory.SetApproachRepository(_approachRepository);
            var apprResult = sc.AddApproach(sig.VersionID.ToString()) as PartialViewResult;

            var appr = (Approach)apprResult.ViewData.Model;

            var result = sc.AddDetector(sig.VersionID, appr.ApproachID, appr.Index) as PartialViewResult;

            var det = (Detector)result.ViewData.Model;

            Assert.IsNotNull(det);
        }

        [TestMethod()]
        public void CopyDetectorTest()
        {
            Db.ClearTables();

            var sc = new SignalsController(_controllerTypeRepository, _regionRepository, _directionTypeRepository,
                _movementTypeRepository, _laneTypeRepository, _detectionHardwareRepository, _signalsRepository,
                _detectorRepository, _detectionTypeRepository, _approachRepository, _metricTypeRepository);

            MOE.Common.Models.Repositories.DetectorRepositoryFactory.SetDetectorRepository(_detectorRepository);
            MOE.Common.Models.Repositories.DirectionTypeRepositoryFactory.SetDirectionsRepository(_directionTypeRepository);
            MOE.Common.Models.Repositories.DetectionTypeRepositoryFactory.SetDetectionTypeRepository(_detectionTypeRepository);

           var sigresult = sc.Create("1001") as PartialViewResult;

            var sig = (Signal)sigresult.ViewData.Model;
            sig.Approaches = new List<Approach>();
            MOE.Common.Models.Repositories.ApproachRepositoryFactory.SetApproachRepository(_approachRepository);
            var apprResult = sc.AddApproach(sig.VersionID.ToString()) as PartialViewResult;

            var appr = (Approach)apprResult.ViewData.Model;

            var detResult = sc.AddDetector(sig.VersionID, appr.ApproachID, appr.Index) as PartialViewResult;

            var detOrig = (Detector)detResult.ViewData.Model;

            var copyResult = sc.CopyDetector(detOrig.ID, sig.VersionID, appr.ApproachID, appr.Index) as PartialViewResult;

            var detCopy = (Detector)copyResult.ViewData.Model;

            Assert.IsTrue(detCopy.DetectorID== detOrig.DetectorID);
        }

        [TestMethod()]
        public void CreateTest()
        {
            Db.ClearTables();

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
            Db.ClearTables();

            var sc = new SignalsController(_controllerTypeRepository, _regionRepository, _directionTypeRepository,
                _movementTypeRepository, _laneTypeRepository, _detectionHardwareRepository, _signalsRepository,
                _detectorRepository, _detectionTypeRepository, _approachRepository, _metricTypeRepository);

            sc.Create("1001");

            var results = _signalsRepository.GetAllVersionsOfSignalBySignalID("1001");
            int vId = 0;
            if (results != null)
            {
                Assert.IsTrue (results.FirstOrDefault().SignalID == "1001");

                vId = results.FirstOrDefault().VersionID;
            }

            sc.DeleteVersion(vId.ToString());

            var results1 = _signalsRepository.GetAllVersionsOfSignalBySignalID("1001");

            if (results1 != null)
            {
                Assert.IsTrue(results1.Count == 0);
               
            }




        }

        [TestMethod]
        public void DeleteAllVersionsOfASignal()
        {
            Db.ClearTables();
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
        public void CreateExistingSignalIdShouldReturnErrorMessage()
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
            Db.ClearTables();
            var sc = new SignalsController(_controllerTypeRepository, _regionRepository, _directionTypeRepository,
                _movementTypeRepository, _laneTypeRepository, _detectionHardwareRepository, _signalsRepository,
                _detectorRepository, _detectionTypeRepository, _approachRepository, _metricTypeRepository);

            var createResults = sc.Create("1001");

            var origversion = createResults;

            var newVersionResults = sc.AddNewVersion("1001"); 

            

            //if (results != null)
            //{
            //        Assert.IsTrue(results.Count == 2);
            //        Assert.AreEqual(results.First().SignalID, results.Last().SignalID);
            //    Assert.AreNotEqual(results.First().VersionID, results.Last().VersionID);
            //}
            //else
            //{

            //    Assert.Fail();
            //}


        }

        [TestMethod()]
        public void AddNewVersionOfExistingSignalShouldCopyDetectionTypes()
        {
            Db.ClearTables();
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

                Assert.AreEqual(results.First().Approaches.First().Detectors.First().DetectionTypes, results.Last().Approaches.Last().Detectors.Last().DetectionTypes);
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
            var sc = new SignalsController(_controllerTypeRepository, _regionRepository, _directionTypeRepository,
                _movementTypeRepository, _laneTypeRepository, _detectionHardwareRepository, _signalsRepository,
                _detectorRepository, _detectionTypeRepository, _approachRepository, _metricTypeRepository);

            sc.Create("1001");

            var result = sc.Copy("1001", "2002") as PartialViewResult;

            var sigCopy = result.Model as Signal;

            Assert.IsTrue(sigCopy.SignalID == "2002");
        }

        [TestMethod()]
        public void EditTest()
        {
            Db.ClearTables();

            var sc = new SignalsController(_controllerTypeRepository, _regionRepository, _directionTypeRepository,
                _movementTypeRepository, _laneTypeRepository, _detectionHardwareRepository, _signalsRepository,
                _detectorRepository, _detectionTypeRepository, _approachRepository, _metricTypeRepository);

         
            sc.ControllerContext = new ControllerContext();

            var signal = sc.Create("1001") as PartialViewResult;


            if (signal != null)
            {
                var result = sc.Edit(signal.Model as Signal) as ContentResult;


                Assert.IsTrue(result != null && result.Content.Contains("Save Successful!"));
            }
            else
            {
                Assert.Fail();
            }
        }







        [TestMethod()]
        public void DeleteTest()
        {
            Db.ClearTables();

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




    }

   
}