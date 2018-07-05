using System.Data.Entity.Migrations;

namespace MOE.Common.Migrations
{
    public partial class SPMWatchDog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.SPMWatchDogErrorEvents",
                    c => new
                    {
                        ID = c.Int(false, true),
                        TimeStamp = c.DateTime(false),
                        SignalID = c.String(false, 10),
                        DetectorID = c.String(),
                        Direction = c.String(false),
                        Phase = c.Int(false),
                        ErrorCode = c.Int(false),
                        Message = c.String(false)
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Signals", t => t.SignalID, true)
                .Index(t => t.SignalID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.SPMWatchDogErrorEvents", "SignalID", "dbo.Signals");
            DropIndex("dbo.SPMWatchDogErrorEvents", new[] {"SignalID"});
            DropTable("dbo.SPMWatchDogErrorEvents");
        }
    }
}