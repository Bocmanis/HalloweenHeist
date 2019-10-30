namespace HaloweenHeist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGuidNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Player", "UniqueId", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Player", "UniqueId", c => c.Guid(nullable: false));
        }
    }
}
