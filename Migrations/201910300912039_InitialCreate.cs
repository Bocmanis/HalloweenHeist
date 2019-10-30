namespace HaloweenHeist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EinteinsPuzzle",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PuzzleId = c.Guid(nullable: false),
                        Drink = c.Int(nullable: false),
                        ShirtColor = c.Int(nullable: false),
                        Nationality = c.Int(nullable: false),
                        Name = c.Int(nullable: false),
                        Hobby = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Player",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        Alias = c.String(),
                        GameStage = c.Int(nullable: false),
                        RicketyBridgeId = c.Int(nullable: false),
                        EinsteinsPuzzleId = c.Int(nullable: false),
                        EinteinsPuzzle_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EinteinsPuzzle", t => t.EinteinsPuzzle_Id)
                .ForeignKey("dbo.RicketyBridge", t => t.RicketyBridgeId, cascadeDelete: true)
                .Index(t => t.RicketyBridgeId)
                .Index(t => t.EinteinsPuzzle_Id);
            
            CreateTable(
                "dbo.RicketyBridge",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WrongAnswer = c.Int(nullable: false),
                        CorrectAnswer = c.Int(nullable: false),
                        Speedster1 = c.Int(nullable: false),
                        Speedster2 = c.Int(nullable: false),
                        SlowPoke1 = c.Int(nullable: false),
                        SlowPoke2 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Player", "RicketyBridgeId", "dbo.RicketyBridge");
            DropForeignKey("dbo.Player", "EinteinsPuzzle_Id", "dbo.EinteinsPuzzle");
            DropIndex("dbo.Player", new[] { "EinteinsPuzzle_Id" });
            DropIndex("dbo.Player", new[] { "RicketyBridgeId" });
            DropTable("dbo.RicketyBridge");
            DropTable("dbo.Player");
            DropTable("dbo.EinteinsPuzzle");
        }
    }
}
