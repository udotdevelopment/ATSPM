namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReCaptchaKeys : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSettings", "ReCaptchaPublicKey", c => c.String());
            AddColumn("dbo.ApplicationSettings", "ReCaptchaSecretKey", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationSettings", "ReCaptchaSecretKey");
            DropColumn("dbo.ApplicationSettings", "ReCaptchaPublicKey");
        }
    }
}
