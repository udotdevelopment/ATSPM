using System;
using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MOE.Common.Business.SiteSecurity;
using MOE.Common.Models;
using Action = MOE.Common.Models.Action;

namespace MOE.Common.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<SPM>
    {
        

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
            CommandTimeout = int.MaxValue;
        }

        protected override void Seed(SPM context)
        {
            Models.Custom.Seeder.Seed(context);

        }
    }
}

