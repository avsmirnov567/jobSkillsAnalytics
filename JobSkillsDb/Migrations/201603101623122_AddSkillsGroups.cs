namespace JobSkillsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSkillsGroups : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SkillToGroupConnections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SkillId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                        IsStandard = c.Boolean(nullable: false),
                        SkillGroup_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Skills", t => t.SkillId, cascadeDelete: true)
                .ForeignKey("dbo.SkillsGroups", t => t.SkillGroup_Id)
                .Index(t => t.SkillId)
                .Index(t => t.SkillGroup_Id);
            
            CreateTable(
                "dbo.SkillsGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SkillToGroupConnections", "SkillGroup_Id", "dbo.SkillsGroups");
            DropForeignKey("dbo.SkillToGroupConnections", "SkillId", "dbo.Skills");
            DropIndex("dbo.SkillToGroupConnections", new[] { "SkillGroup_Id" });
            DropIndex("dbo.SkillToGroupConnections", new[] { "SkillId" });
            DropTable("dbo.SkillsGroups");
            DropTable("dbo.SkillToGroupConnections");
        }
    }
}
