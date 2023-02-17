namespace PassionProjectASP.NETNajibOsman.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gamesplayers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        GameID = c.Int(nullable: false, identity: true),
                        GameName = c.String(),
                        GameDescription = c.String(),
                    })
                .PrimaryKey(t => t.GameID);
            
            CreateTable(
                "dbo.GamePlayers",
                c => new
                    {
                        Game_GameID = c.Int(nullable: false),
                        Player_PlayerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Game_GameID, t.Player_PlayerID })
                .ForeignKey("dbo.Games", t => t.Game_GameID, cascadeDelete: true)
                .ForeignKey("dbo.Players", t => t.Player_PlayerID, cascadeDelete: true)
                .Index(t => t.Game_GameID)
                .Index(t => t.Player_PlayerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GamePlayers", "Player_PlayerID", "dbo.Players");
            DropForeignKey("dbo.GamePlayers", "Game_GameID", "dbo.Games");
            DropIndex("dbo.GamePlayers", new[] { "Player_PlayerID" });
            DropIndex("dbo.GamePlayers", new[] { "Game_GameID" });
            DropTable("dbo.GamePlayers");
            DropTable("dbo.Games");
        }
    }
}
