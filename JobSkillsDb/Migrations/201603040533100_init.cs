namespace JobSkillsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Newskillset",
                c => new
                    {
                        newskillset_id = c.Int(nullable: false, identity: true),
                        skillset_number = c.Int(nullable: false),
                        skillset_support = c.Decimal(precision: 18, scale: 0),
                        skillset_confidence = c.Decimal(precision: 18, scale: 0),
                    })
                .PrimaryKey(t => t.newskillset_id);
            
            CreateTable(
                "dbo.Skills",
                c => new
                    {
                        skill_id = c.Int(nullable: false, identity: true),
                        skill_name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.skill_id);
            
            CreateTable(
                "dbo.Vacancy",
                c => new
                    {
                        vancancy_id = c.Int(nullable: false, identity: true),
                        vacancy_info_id = c.Int(nullable: false),
                        vacancy_name = c.String(nullable: false),
                        vacancy_salary_from = c.Int(),
                        vacancy_salary_to = c.Int(),
                        vacancy_content_text = c.String(),
                        vacancy_content_html = c.String(),
                        vacancy_link = c.String(nullable: false),
                        vacancy_publisher = c.String(),
                    })
                .PrimaryKey(t => t.vancancy_id);
            
            CreateTable(
                "dbo.marked_zones",
                c => new
                    {
                        marked_zone_id = c.Int(nullable: false, identity: true),
                        index_start = c.Int(nullable: false),
                        index_end = c.Int(nullable: false),
                        highlighted_text = c.String(),
                        Skill_Id = c.Int(),
                        Vacancy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.marked_zone_id)
                .ForeignKey("dbo.Skills", t => t.Skill_Id)
                .ForeignKey("dbo.Vacancy", t => t.Vacancy_Id)
                .Index(t => t.Skill_Id)
                .Index(t => t.Vacancy_Id);
            
            CreateTable(
                "dbo.Source",
                c => new
                    {
                        source_id = c.Int(nullable: false),
                        source_name = c.String(nullable: false),
                        source_link = c.String(),
                    })
                .PrimaryKey(t => t.source_id)
                .ForeignKey("dbo.Vacancy", t => t.source_id)
                .Index(t => t.source_id);
            
            CreateTable(
                "dbo.Skillset",
                c => new
                    {
                        skill_id = c.Int(nullable: false),
                        skillset_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.skill_id, t.skillset_id })
                .ForeignKey("dbo.Skills", t => t.skill_id, cascadeDelete: true)
                .ForeignKey("dbo.Vacancy", t => t.skillset_id, cascadeDelete: true)
                .Index(t => t.skill_id)
                .Index(t => t.skillset_id);
            
            CreateTable(
                "dbo.Newskillset_skill",
                c => new
                    {
                        newskillset_id = c.Int(nullable: false),
                        skill_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.newskillset_id, t.skill_id })
                .ForeignKey("dbo.Newskillset", t => t.newskillset_id, cascadeDelete: true)
                .ForeignKey("dbo.Skills", t => t.skill_id, cascadeDelete: true)
                .Index(t => t.newskillset_id)
                .Index(t => t.skill_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Newskillset_skill", "skill_id", "dbo.Skills");
            DropForeignKey("dbo.Newskillset_skill", "newskillset_id", "dbo.Newskillset");
            DropForeignKey("dbo.Skillset", "skillset_id", "dbo.Vacancy");
            DropForeignKey("dbo.Skillset", "skill_id", "dbo.Skills");
            DropForeignKey("dbo.Source", "source_id", "dbo.Vacancy");
            DropForeignKey("dbo.marked_zones", "Vacancy_Id", "dbo.Vacancy");
            DropForeignKey("dbo.marked_zones", "Skill_Id", "dbo.Skills");
            DropIndex("dbo.Newskillset_skill", new[] { "skill_id" });
            DropIndex("dbo.Newskillset_skill", new[] { "newskillset_id" });
            DropIndex("dbo.Skillset", new[] { "skillset_id" });
            DropIndex("dbo.Skillset", new[] { "skill_id" });
            DropIndex("dbo.Source", new[] { "source_id" });
            DropIndex("dbo.marked_zones", new[] { "Vacancy_Id" });
            DropIndex("dbo.marked_zones", new[] { "Skill_Id" });
            DropTable("dbo.Newskillset_skill");
            DropTable("dbo.Skillset");
            DropTable("dbo.Source");
            DropTable("dbo.marked_zones");
            DropTable("dbo.Vacancy");
            DropTable("dbo.Skills");
            DropTable("dbo.Newskillset");
        }
    }
}
