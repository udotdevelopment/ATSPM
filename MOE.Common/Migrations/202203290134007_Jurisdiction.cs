namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Jurisdiction : DbMigration
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
            
            AddColumn("dbo.Signals", "JurisdictionId", c => c.Int(nullable: false, defaultValue: 1));
            CreateIndex("dbo.Signals", "JurisdictionId");
            AddForeignKey("dbo.Signals", "JurisdictionId", "dbo.Jurisdictions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Signals", "JurisdictionId", "dbo.Jurisdictions");
            DropIndex("dbo.Signals", new[] { "JurisdictionId" });
            DropColumn("dbo.Signals", "JurisdictionId");
            DropTable("dbo.Jurisdictions");
        }
    }
}
