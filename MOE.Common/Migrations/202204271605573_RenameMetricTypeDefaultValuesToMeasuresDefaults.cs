namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameMetricTypeDefaultValuesToMeasuresDefaults : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MeasuresDefaults",
                c => new
                    {
                        Measure = c.String(nullable: false, maxLength: 128),
                        OptionName = c.String(nullable: false, maxLength: 128),
                        Value = c.String(),
                    })
                .PrimaryKey(t => new { t.Measure, t.OptionName });
            
            DropTable("dbo.MetricTypesDefaultValues");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MetricTypesDefaultValues",
                c => new
                    {
                        Chart = c.String(nullable: false, maxLength: 128),
                        Option = c.String(nullable: false, maxLength: 128),
                        Value = c.String(),
                    })
                .PrimaryKey(t => new { t.Chart, t.Option });
            
            DropTable("dbo.MeasuresDefaults");
        }
    }
}
