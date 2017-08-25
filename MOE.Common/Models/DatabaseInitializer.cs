using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MOE.Common.Business.SiteSecurity;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace MOE.Common.Models
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<SPM>
    {
        protected override void Seed(MOE.Common.Models.SPM context)
        {
            base.Seed(context);
        }
    
    }
}
