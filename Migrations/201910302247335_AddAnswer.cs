namespace HaloweenHeist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnswer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Player", "Stage3Answer", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Player", "Stage3Answer");
        }
    }
}
