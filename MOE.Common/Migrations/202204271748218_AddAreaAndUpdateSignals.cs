namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAreaAndUpdateSignals : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AreaName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AreaSignals",
                c => new
                    {
                        Area_Id = c.Int(nullable: false),
                        Signal_VersionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Area_Id, t.Signal_VersionID })
                .ForeignKey("dbo.Areas", t => t.Area_Id, cascadeDelete: true)
                .ForeignKey("dbo.Signals", t => t.Signal_VersionID, cascadeDelete: true)
                .Index(t => t.Area_Id)
                .Index(t => t.Signal_VersionID);
            
            AddColumn("dbo.Signals", "AreaId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {  
            DropForeignKey("dbo.AreaSignals", "Signal_VersionID", "dbo.Signals");
            DropForeignKey("dbo.AreaSignals", "Area_Id", "dbo.Areas");
            DropIndex("dbo.AreaSignals", new[] { "Signal_VersionID" });
            DropIndex("dbo.AreaSignals", new[] { "Area_Id" });
            DropColumn("dbo.Signals", "AreaId");
            DropTable("dbo.AreaSignals");
            DropTable("dbo.Areas");
        }
    }
}
