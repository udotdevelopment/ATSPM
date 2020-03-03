using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SPM
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //using (var db = new MOE.Common.Models.SPM())
            //{
                //if (!db.Database.Exists())
                //{
                //    var config = new MOE.Common.Migrations.Configuration();
                //    config.AutomaticMigrationsEnabled = true;
                //    config.AutomaticMigrationDataLossAllowed = true;

                //    db.Database.Initialize(true);
                //    var migrator = new System.Data.Entity.Migrations.DbMigrator(config);
                //    migrator.Update();

                //}
           // }



        }
    }
}
