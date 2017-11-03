namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Signals", name: "VersionAction_ID", newName: "VersionActionId");
            RenameIndex(table: "dbo.Signals", name: "IX_VersionAction_ID", newName: "IX_VersionActionId");
            AddColumn("dbo.Signals", "Start", c => c.DateTime(nullable: false));
            DropColumn("dbo.Signals", "End");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Signals", "End", c => c.DateTime(nullable: false));
            DropColumn("dbo.Signals", "Start");
            RenameIndex(table: "dbo.Signals", name: "IX_VersionActionId", newName: "IX_VersionAction_ID");
            RenameColumn(table: "dbo.Signals", name: "VersionActionId", newName: "VersionAction_ID");
        }
    }
}
