namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WatchDogEmailAllErrors : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSettings", "EmailAllErrors", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationSettings", "EmailAllErrors");
        }
    }
}
