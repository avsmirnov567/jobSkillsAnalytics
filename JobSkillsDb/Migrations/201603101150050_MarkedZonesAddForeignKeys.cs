namespace JobSkillsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MarkedZonesAddForeignKeys : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MarkedZones", "Vacancy_Id", "dbo.Vacancies");
            DropForeignKey("dbo.MarkedZones", "Skill_Id", "dbo.Skills");
            DropIndex("dbo.MarkedZones", new[] { "Skill_Id" });
            DropIndex("dbo.MarkedZones", new[] { "Vacancy_Id" });
            RenameColumn(table: "dbo.MarkedZones", name: "Vacancy_Id", newName: "VacancyId");
            RenameColumn(table: "dbo.MarkedZones", name: "Skill_Id", newName: "SkillId");
            AlterColumn("dbo.MarkedZones", "SkillId", c => c.Int(nullable: false));
            AlterColumn("dbo.MarkedZones", "VacancyId", c => c.Int(nullable: false));
            CreateIndex("dbo.MarkedZones", "VacancyId");
            CreateIndex("dbo.MarkedZones", "SkillId");
            AddForeignKey("dbo.MarkedZones", "VacancyId", "dbo.Vacancies", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MarkedZones", "SkillId", "dbo.Skills", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MarkedZones", "SkillId", "dbo.Skills");
            DropForeignKey("dbo.MarkedZones", "VacancyId", "dbo.Vacancies");
            DropIndex("dbo.MarkedZones", new[] { "SkillId" });
            DropIndex("dbo.MarkedZones", new[] { "VacancyId" });
            AlterColumn("dbo.MarkedZones", "VacancyId", c => c.Int());
            AlterColumn("dbo.MarkedZones", "SkillId", c => c.Int());
            RenameColumn(table: "dbo.MarkedZones", name: "SkillId", newName: "Skill_Id");
            RenameColumn(table: "dbo.MarkedZones", name: "VacancyId", newName: "Vacancy_Id");
            CreateIndex("dbo.MarkedZones", "Vacancy_Id");
            CreateIndex("dbo.MarkedZones", "Skill_Id");
            AddForeignKey("dbo.MarkedZones", "Skill_Id", "dbo.Skills", "Id");
            AddForeignKey("dbo.MarkedZones", "Vacancy_Id", "dbo.Vacancies", "Id");
        }
    }
}
