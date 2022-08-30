using System.Data.Entity.Migrations;

namespace MOE.Common.Migrations
{
    public partial class GeneralSettings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSettings", "ImageUrl", c => c.String());
            AddColumn("dbo.ApplicationSettings", "ImagePath", c => c.String());
            AddColumn("dbo.ApplicationSettings", "RawDataCountLimit", c => c.Int());
        }

        public override void Down()
        {
            DropColumn("dbo.ApplicationSettings", "RawDataCountLimit");
            DropColumn("dbo.ApplicationSettings", "ImagePath");
            DropColumn("dbo.ApplicationSettings", "ImageUrl");
        }
    }
}