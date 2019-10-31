namespace HaloweenHeist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTimeStampsWNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Player", "EndTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Player", "EndTime", c => c.DateTime(nullable: false));
        }
    }
}
