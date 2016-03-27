namespace JobSkillsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTablesForAssociations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SkillAprioriSkillSets", "Skill_Id", "dbo.Skills");
            DropForeignKey("dbo.SkillAprioriSkillSets", "AprioriSkillSet_Id", "dbo.AprioriSkillSets");
            DropIndex("dbo.SkillAprioriSkillSets", new[] { "Skill_Id" });
            DropIndex("dbo.SkillAprioriSkillSets", new[] { "AprioriSkillSet_Id" });
            CreateTable(
                "dbo.AprioriRules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LeftHandSide = c.String(),
                        RightHandSide = c.String(),
                        Rules = c.String(),
                        Support = c.Double(nullable: false),
                        Confidence = c.Double(nullable: false),
                        Lift = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EclatSets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemSet = c.String(),
                        Support = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Skills", "Support");
            DropTable("dbo.AprioriSkillSets");
            DropTable("dbo.SkillAprioriSkillSets");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SkillAprioriSkillSets",
                c => new
                    {
                        Skill_Id = c.Int(nullable: false),
                        AprioriSkillSet_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Skill_Id, t.AprioriSkillSet_Id });
            
            CreateTable(
                "dbo.AprioriSkillSets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Support = c.Decimal(precision: 18, scale: 0),
                        Confidence = c.Decimal(precision: 18, scale: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Skills", "Support", c => c.Decimal(precision: 18, scale: 2));
            DropTable("dbo.EclatSets");
            DropTable("dbo.AprioriRules");
            CreateIndex("dbo.SkillAprioriSkillSets", "AprioriSkillSet_Id");
            CreateIndex("dbo.SkillAprioriSkillSets", "Skill_Id");
            AddForeignKey("dbo.SkillAprioriSkillSets", "AprioriSkillSet_Id", "dbo.AprioriSkillSets", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SkillAprioriSkillSets", "Skill_Id", "dbo.Skills", "Id", cascadeDelete: true);
        }
    }
}
