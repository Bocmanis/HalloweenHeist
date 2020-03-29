namespace HaloweenHeist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredAlias : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Player", "Alias", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Player", "Alias", c => c.String());
        }
    }
}
