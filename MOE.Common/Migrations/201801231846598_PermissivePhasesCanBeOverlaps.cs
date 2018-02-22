using System.Data.Entity.Migrations;

namespace MOE.Common.Migrations
{
    public partial class PermissivePhasesCanBeOverlaps : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Approaches", "IsPermissivePhaseOverlap", c => c.Boolean(false));
        }

        public override void Down()
        {
            DropColumn("dbo.Approaches", "IsPermissivePhaseOverlap");
        }
    }
}