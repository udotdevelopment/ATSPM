namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelforSep2021 : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Menu(MenuId,MenuName,ParentId,Application,DisplayOrder,Controller,Action) Values('99','Enhanced Configuration','11','SignalPerformanceMetrics','10','EnhancedConfiguration','Index')");
        }                                                                                                                                                                               

        public override void Down()
        {
            Sql("DELETE FROM Menu where MenuId = 99");        
        }
    }
}