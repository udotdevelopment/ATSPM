using System.Data.Entity;

namespace MOE.Common.Models
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<SPM>
    {
        protected override void Seed(SPM context)
        {
            base.Seed(context);
        }
    }
}