namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SPMWatchdogExclusionsInitialModelChanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SPMWatchdogExclusions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SignalID = c.String(nullable: false),
                        PhaseID = c.Int(),
                        TypeOfAlert = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SPMWatchdogExclusions");
        }
    }
}
