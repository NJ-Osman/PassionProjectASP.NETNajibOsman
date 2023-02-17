namespace PassionProjectASP.NETNajibOsman.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class communities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Communities",
                c => new
                    {
                        CommunityID = c.Int(nullable: false, identity: true),
                        CommunityName = c.String(),
                        CommunityBio = c.String(),
                    })
                .PrimaryKey(t => t.CommunityID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Communities");
        }
    }
}
