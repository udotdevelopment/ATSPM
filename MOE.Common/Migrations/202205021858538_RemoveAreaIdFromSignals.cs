namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAreaIdFromSignals : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Signals", "AreaId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Signals", "AreaId", c => c.Int(nullable: false));
        }
    }
}
