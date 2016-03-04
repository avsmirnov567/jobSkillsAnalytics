namespace JobSkillsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NameRefacroring : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Newskillset", newName: "AprioriSkillSets");
            RenameTable(name: "dbo.marked_zones", newName: "MarkedZones");
            RenameTable(name: "dbo.Source", newName: "Sources");
            RenameTable(name: "dbo.Newskillset_skill", newName: "SkillAprioriSkillSets");
            RenameTable(name: "dbo.Skillset", newName: "VacancySkills");
            DropIndex("dbo.Sources", new[] { "source_id" });
            DropIndex("dbo.VacancySkills", new[] { "skill_id" });
            DropIndex("dbo.SkillAprioriSkillSets", new[] { "skill_id" });
            RenameColumn(table: "dbo.AprioriSkillSets", name: "newskillset_id", newName: "Id");
            RenameColumn(table: "dbo.AprioriSkillSets", name: "skillset_number", newName: "Number");
            RenameColumn(table: "dbo.AprioriSkillSets", name: "skillset_support", newName: "Support");
            RenameColumn(table: "dbo.AprioriSkillSets", name: "skillset_confidence", newName: "Confidence");
            RenameColumn(table: "dbo.Skills", name: "skill_id", newName: "Id");
            RenameColumn(table: "dbo.Skills", name: "skill_name", newName: "Name");
            RenameColumn(table: "dbo.Vacancies", name: "vancancy_id", newName: "Id");
            RenameColumn(table: "dbo.Vacancies", name: "vacancy_info_id", newName: "InnerId");
            RenameColumn(table: "dbo.Vacancies", name: "vacancy_name", newName: "Title");
            RenameColumn(table: "dbo.Vacancies", name: "vacancy_salary_from", newName: "SalaryFrom");
            RenameColumn(table: "dbo.Vacancies", name: "vacancy_salary_to", newName: "SalaryTo");
            RenameColumn(table: "dbo.Vacancies", name: "vacancy_content_text", newName: "ContentText");
            RenameColumn(table: "dbo.Vacancies", name: "vacancy_link", newName: "Link");
            RenameColumn(table: "dbo.Vacancies", name: "vacancy_publisher", newName: "Employer");
            RenameColumn(table: "dbo.MarkedZones", name: "marked_zone_id", newName: "Id");
            RenameColumn(table: "dbo.MarkedZones", name: "index_start", newName: "IndexStart");
            RenameColumn(table: "dbo.MarkedZones", name: "index_end", newName: "IndexEnd");
            RenameColumn(table: "dbo.MarkedZones", name: "highlighted_text", newName: "HighlightedText");
            RenameColumn(table: "dbo.Sources", name: "source_id", newName: "Id");
            RenameColumn(table: "dbo.Sources", name: "source_name", newName: "Name");
            RenameColumn(table: "dbo.SkillAprioriSkillSets", name: "newskillset_id", newName: "AprioriSkillSet_Id");
            RenameColumn(table: "dbo.VacancySkills", name: "skillset_id", newName: "Vacancy_Id");
            RenameIndex(table: "dbo.SkillAprioriSkillSets", name: "IX_newskillset_id", newName: "IX_AprioriSkillSet_Id");
            RenameIndex(table: "dbo.VacancySkills", name: "IX_skillset_id", newName: "IX_Vacancy_Id");
            DropPrimaryKey("dbo.Sources");
            DropPrimaryKey("dbo.SkillAprioriSkillSets");
            DropPrimaryKey("dbo.VacancySkills");
            AddColumn("dbo.Vacancies", "ContentHtml", c => c.String());
            AddColumn("dbo.Vacancies", "Source_Id", c => c.Int());
            AddColumn("dbo.Sources", "SourceLink", c => c.String());
            AlterColumn("dbo.Sources", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Sources", "Id");
            AddPrimaryKey("dbo.SkillAprioriSkillSets", new[] { "Skill_Id", "AprioriSkillSet_Id" });
            AddPrimaryKey("dbo.VacancySkills", new[] { "Vacancy_Id", "Skill_Id" });
            CreateIndex("dbo.Vacancies", "Source_Id");
            CreateIndex("dbo.SkillAprioriSkillSets", "Skill_Id");
            CreateIndex("dbo.VacancySkills", "Skill_Id");
            AddForeignKey("dbo.Vacancies", "Source_Id", "dbo.Sources", "Id");
            DropColumn("dbo.Vacancies", "vacancy_content_html");
            DropColumn("dbo.Sources", "source_link");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sources", "source_link", c => c.String());
            AddColumn("dbo.Vacancies", "vacancy_content_html", c => c.String());
            DropForeignKey("dbo.Vacancies", "Source_Id", "dbo.Sources");
            DropIndex("dbo.VacancySkills", new[] { "Skill_Id" });
            DropIndex("dbo.SkillAprioriSkillSets", new[] { "Skill_Id" });
            DropIndex("dbo.Vacancies", new[] { "Source_Id" });
            DropPrimaryKey("dbo.VacancySkills");
            DropPrimaryKey("dbo.SkillAprioriSkillSets");
            DropPrimaryKey("dbo.Sources");
            AlterColumn("dbo.Sources", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.Sources", "SourceLink");
            DropColumn("dbo.Vacancies", "Source_Id");
            DropColumn("dbo.Vacancies", "ContentHtml");
            AddPrimaryKey("dbo.VacancySkills", new[] { "skill_id", "skillset_id" });
            AddPrimaryKey("dbo.SkillAprioriSkillSets", new[] { "newskillset_id", "skill_id" });
            AddPrimaryKey("dbo.Sources", "source_id");
            RenameIndex(table: "dbo.VacancySkills", name: "IX_Vacancy_Id", newName: "IX_skillset_id");
            RenameIndex(table: "dbo.SkillAprioriSkillSets", name: "IX_AprioriSkillSet_Id", newName: "IX_newskillset_id");
            RenameColumn(table: "dbo.VacancySkills", name: "Vacancy_Id", newName: "skillset_id");
            RenameColumn(table: "dbo.SkillAprioriSkillSets", name: "AprioriSkillSet_Id", newName: "newskillset_id");
            RenameColumn(table: "dbo.Sources", name: "Name", newName: "source_name");
            RenameColumn(table: "dbo.Sources", name: "Id", newName: "source_id");
            RenameColumn(table: "dbo.MarkedZones", name: "HighlightedText", newName: "highlighted_text");
            RenameColumn(table: "dbo.MarkedZones", name: "IndexEnd", newName: "index_end");
            RenameColumn(table: "dbo.MarkedZones", name: "IndexStart", newName: "index_start");
            RenameColumn(table: "dbo.MarkedZones", name: "Id", newName: "marked_zone_id");
            RenameColumn(table: "dbo.Vacancies", name: "Employer", newName: "vacancy_publisher");
            RenameColumn(table: "dbo.Vacancies", name: "Link", newName: "vacancy_link");
            RenameColumn(table: "dbo.Vacancies", name: "ContentText", newName: "vacancy_content_text");
            RenameColumn(table: "dbo.Vacancies", name: "SalaryTo", newName: "vacancy_salary_to");
            RenameColumn(table: "dbo.Vacancies", name: "SalaryFrom", newName: "vacancy_salary_from");
            RenameColumn(table: "dbo.Vacancies", name: "Title", newName: "vacancy_name");
            RenameColumn(table: "dbo.Vacancies", name: "InnerId", newName: "vacancy_info_id");
            RenameColumn(table: "dbo.Vacancies", name: "Id", newName: "vancancy_id");
            RenameColumn(table: "dbo.Skills", name: "Name", newName: "skill_name");
            RenameColumn(table: "dbo.Skills", name: "Id", newName: "skill_id");
            RenameColumn(table: "dbo.AprioriSkillSets", name: "Confidence", newName: "skillset_confidence");
            RenameColumn(table: "dbo.AprioriSkillSets", name: "Support", newName: "skillset_support");
            RenameColumn(table: "dbo.AprioriSkillSets", name: "Number", newName: "skillset_number");
            RenameColumn(table: "dbo.AprioriSkillSets", name: "Id", newName: "newskillset_id");
            CreateIndex("dbo.SkillAprioriSkillSets", "skill_id");
            CreateIndex("dbo.VacancySkills", "skill_id");
            CreateIndex("dbo.Sources", "source_id");
            RenameTable(name: "dbo.VacancySkills", newName: "Skillset");
            RenameTable(name: "dbo.SkillAprioriSkillSets", newName: "Newskillset_skill");
            RenameTable(name: "dbo.Sources", newName: "Source");
            RenameTable(name: "dbo.MarkedZones", newName: "marked_zones");
            RenameTable(name: "dbo.AprioriSkillSets", newName: "Newskillset");
        }
    }
}
