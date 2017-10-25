using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Models;

namespace MOE.Common.Models.Tests
{
    [TestClass()]
    public class ApproachTests
    {
        [TestMethod()]
        public void CopyApproachTest()
        {
            InMemoryApproachRepository ar = new InMemoryApproachRepository();
            Approach a = new Approach();

            a.SignalID = "101";
            a.ApproachID = 1;
            a.Description = "TestAppr";



            ar.AddOrUpdate(a);

            MOE.Common.Models.Repositories.ApproachRepositoryFactory.SetApproachRepository(ar);
            


            Approach b = Approach.CopyApproach(1);

            Assert.IsTrue(b.Description == "TestAppr Copy");

           
        }

        [TestMethod()]
        public void GetApproachByApproachID()
        {
         



        }
    }

    
}