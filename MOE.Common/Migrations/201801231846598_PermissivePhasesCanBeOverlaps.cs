namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PermissivePhasesCanBeOverlaps : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Approaches", "IsPermissivePhaseOverlap", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Approaches", "IsPermissivePhaseOverlap");
        }
    }
}
