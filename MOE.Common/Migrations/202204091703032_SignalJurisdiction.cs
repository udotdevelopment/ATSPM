namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SignalJurisdiction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Signals", "JurisdictionId", c => c.Int(nullable: false, defaultValue: 1));
            CreateIndex("dbo.Signals", "JurisdictionId");
            AddForeignKey("dbo.Signals", "JurisdictionId", "dbo.Jurisdictions", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Signals", "JurisdictionId", "dbo.Jurisdictions");
            DropIndex("dbo.Signals", new[] { "JurisdictionId" });
            DropColumn("dbo.Signals", "JurisdictionId");
        }
    }
}
