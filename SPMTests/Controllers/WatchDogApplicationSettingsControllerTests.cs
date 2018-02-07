using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPM.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MOE.Common.Migrations;
using MOE.CommonTests.Models;
using System.Data.Entity.Migrations;

namespace SPM.Controllers.Tests
{
    [TestClass()]
    public class WatchDogApplicationSettingsControllerTests
    {
        [TestInitialize]
        public void Initialize()
        {

            
        }
        [TestMethod()]
        public void EditTest()
        {
            //InMemoryApplicationSettingsRepository appRep = new InMemoryApplicationSettingsRepository();
            //MOE.Common.Models.Repositories.ApplicationSettingsRepositoryFactory.SetMetricsRepository(appRep);

            WatchDogApplicationSettingsController controller = new WatchDogApplicationSettingsController();
            ActionResult ar = controller.Edit();

            Assert.IsNotNull(ar);
        }

        //[TestMethod()]
        //public void SeedMethodTest()
        //{
        //    var configuration = new Configuration();
        //    var migrator = new System.Data.Entity.Migrations.DbMigrator(configuration);
        //    migrator.Update();

        //    Assert.IsTrue(1 == 1);
        //}
    }
}