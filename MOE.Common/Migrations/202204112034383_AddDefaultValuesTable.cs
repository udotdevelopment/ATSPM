namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDefaultValuesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MetricTypesDefaultValues",
                c => new
                    {
                        Chart = c.String(nullable: false, maxLength: 128),
                        Option = c.String(nullable: false, maxLength: 128),
                        Value = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Chart, t.Option, t.Value });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MetricTypesDefaultValues");
        }
    }
}
