namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApproachEventCountAggregation : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PhaseEventCountAggregations", newName: "ApproachEventCountAggregations");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ApproachEventCountAggregations", newName: "PhaseEventCountAggregations");
        }
    }
}
