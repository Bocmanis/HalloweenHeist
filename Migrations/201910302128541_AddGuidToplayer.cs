namespace HaloweenHeist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGuidToplayer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Player", "UniqueId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Player", "UniqueId");
        }
    }
}
