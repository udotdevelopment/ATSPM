namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4_3 : DbMigration
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
                "dbo.MeasuresDefaults",
                c => new
                    {
                        Measure = c.String(nullable: false, maxLength: 128),
                        OptionName = c.String(nullable: false, maxLength: 128),
                        Value = c.String(),
                    })
                .PrimaryKey(t => new { t.Measure, t.OptionName });
            
            AddColumn("dbo.Signals", "JurisdictionId", c => c.Int(nullable: false, defaultValue:1));
            AddColumn("dbo.Signals", "Pedsare1to1", c => c.Boolean(nullable: false));
            AddColumn("dbo.Approaches", "PedestrianPhaseNumber", c => c.Int());
            AddColumn("dbo.Approaches", "IsPedestrianPhaseOverlap", c => c.Boolean(nullable: false));
            AddColumn("dbo.Approaches", "PedestrianDetectors", c => c.String());
            Sql("insert into Jurisdictions(JurisdictionName) values ('Default')");
            CreateIndex("dbo.Signals", "JurisdictionId");
            AddForeignKey("dbo.Signals", "JurisdictionId", "dbo.Jurisdictions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Signals", "JurisdictionId", "dbo.Jurisdictions");
            DropIndex("dbo.Signals", new[] { "JurisdictionId" });
            DropColumn("dbo.Approaches", "PedestrianDetectors");
            DropColumn("dbo.Approaches", "IsPedestrianPhaseOverlap");
            DropColumn("dbo.Approaches", "PedestrianPhaseNumber");
            DropColumn("dbo.Signals", "Pedsare1to1");
            DropColumn("dbo.Signals", "JurisdictionId");
            DropTable("dbo.MeasuresDefaults");
            DropTable("dbo.Jurisdictions");
        }
    }
}
