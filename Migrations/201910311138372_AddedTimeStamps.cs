namespace HaloweenHeist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTimeStamps : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Player", "StartTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Player", "EndTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Player", "EndTime");
            DropColumn("dbo.Player", "StartTime");
        }
    }
}
