namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JurisdictionTest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Jurisdictions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JurisdictionName = c.String(maxLength: 50),
                        MPO = c.String(maxLength: 50),
                        CountyParish = c.String(maxLength: 50),
                        OtherPartners = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JurisdictionSignals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JurisdictionId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        SignalId = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jurisdictions", t => t.JurisdictionId, cascadeDelete: true)
                .Index(t => t.JurisdictionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JurisdictionSignals", "JurisdictionId", "dbo.Jurisdictions");
            DropIndex("dbo.JurisdictionSignals", new[] { "JurisdictionId" });
            DropTable("dbo.JurisdictionSignals");
            DropTable("dbo.Jurisdictions");
        }
    }
}
