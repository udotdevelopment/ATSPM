namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SPMWatchDog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SPMWatchDogErrorEvents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        SignalID = c.String(nullable: false, maxLength: 10),
                        DetectorID = c.String(),
                        Direction = c.String(nullable: false),
                        Phase = c.Int(nullable: false),
                        ErrorCode = c.Int(nullable: false),
                        Message = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Signals", t => t.SignalID, cascadeDelete: true)
                .Index(t => t.SignalID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SPMWatchDogErrorEvents", "SignalID", "dbo.Signals");
            DropIndex("dbo.SPMWatchDogErrorEvents", new[] { "SignalID" });
            DropTable("dbo.SPMWatchDogErrorEvents");
        }
    }
}
