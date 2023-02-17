namespace PassionProjectASP.NETNajibOsman.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class playercommunity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "CommunityID", c => c.Int(nullable: false));
            CreateIndex("dbo.Players", "CommunityID");
            AddForeignKey("dbo.Players", "CommunityID", "dbo.Communities", "CommunityID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Players", "CommunityID", "dbo.Communities");
            DropIndex("dbo.Players", new[] { "CommunityID" });
            DropColumn("dbo.Players", "CommunityID");
        }
    }
}
