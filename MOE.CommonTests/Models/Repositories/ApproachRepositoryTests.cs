using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.CommonTests.Models;

namespace MOE.Common.Models.Repositories.Tests
{
    [TestClass()]
    public class ApproachRepositoryTests
    {
        private InMemoryApproachRepository InitializeContext()
        {
            InMemoryMOEDatabase db = new InMemoryMOEDatabase();
            db.PopulateSignal();
            db.PopulateSignalsWithApproaches();
            db.PopulateApproachesWithDetectors();

            InMemoryApproachRepository Ar = new InMemoryApproachRepository(db);
            return Ar;

        }

        [TestMethod()]
        public void GetAllApproachesTest()
        {
        

         InMemoryApproachRepository Ar = InitializeContext();



            var approaches = Ar.GetAllApproaches();

        
                Assert.IsTrue(approaches.Count > 19);
            




        }


        [TestMethod()]
        public void GetApproachByApproachIDTest()
        {
            InMemoryApproachRepository Ar = InitializeContext(); ;

            

            var app = Ar.GetApproachByApproachID(1021);

            Assert.IsTrue(app.SignalID == "101");
        }

        [TestMethod()]
        public void AddOrUpdateTest()
        {
            InMemoryApproachRepository Ar = InitializeContext();



            Common.Models.Approach app = new Approach
            {
                ApproachID = 99999,
                Description = "Add Approach for Signal 101",
                DirectionTypeID = 8,
                ProtectedPhaseNumber = 2,
                SignalID = "101",
                VersionID = 1,
              
            };

            Ar.AddOrUpdate(app);

            var app2 = Ar.GetApproachByApproachID(99999);

            Assert.IsTrue(app2.SignalID == "101");

            app.Description = "Description After Update";

            Ar.AddOrUpdate(app);

            app2 = Ar.GetApproachByApproachID(99999);

            Assert.IsTrue(app2.Description == "Description After Update");


        }

        [TestMethod()]
        public void RemoveTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RemoveTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddTest()
        {
            Assert.Fail();
        }
    }
}