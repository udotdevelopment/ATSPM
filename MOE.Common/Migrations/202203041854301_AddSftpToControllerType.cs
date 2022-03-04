namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSftpToControllerType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ControllerTypes", "SFTP", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ControllerTypes", "SFTP");
        }
    }
}
