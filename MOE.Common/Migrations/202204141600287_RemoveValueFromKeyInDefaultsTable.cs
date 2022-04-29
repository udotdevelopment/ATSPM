namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveValueFromKeyInDefaultsTable : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.MetricTypesDefaultValues");
            AlterColumn("dbo.MetricTypesDefaultValues", "Value", c => c.String());
            AddPrimaryKey("dbo.MetricTypesDefaultValues", new[] { "Chart", "Option" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.MetricTypesDefaultValues");
            AlterColumn("dbo.MetricTypesDefaultValues", "Value", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.MetricTypesDefaultValues", new[] { "Chart", "Option", "Value" });
        }
    }
}
