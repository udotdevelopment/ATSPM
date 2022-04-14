namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CycleCompletionSeconds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSettings", "CycleCompletionSeconds", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationSettings", "CycleCompletionSeconds");
        }
    }
}
