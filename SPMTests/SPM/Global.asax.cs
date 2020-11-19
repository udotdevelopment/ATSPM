using System;
using System.Collections.Generic;
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

            using (var db = new MOE.Common.Models.SPM())
            {
                if (!db.Database.Exists())
                {
                    db.Database.Initialize(true);
                    var config = new MOE.Common.Migrations.Configuration();
                    var migrator = new System.Data.Entity.Migrations.DbMigrator(config);
                    migrator.Update(); 
                }
            }



        }
    }
}
