namespace JobSkillsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVacancyIdForLabelTypes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vacancies", "VacancyLabelType_Id", "dbo.VacancyLabelTypes");
            DropIndex("dbo.Vacancies", new[] { "VacancyLabelType_Id" });
            RenameColumn(table: "dbo.Vacancies", name: "VacancyLabelType_Id", newName: "VacancyLabelTypeId");
            AlterColumn("dbo.Vacancies", "VacancyLabelTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Vacancies", "VacancyLabelTypeId");
            AddForeignKey("dbo.Vacancies", "VacancyLabelTypeId", "dbo.VacancyLabelTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vacancies", "VacancyLabelTypeId", "dbo.VacancyLabelTypes");
            DropIndex("dbo.Vacancies", new[] { "VacancyLabelTypeId" });
            AlterColumn("dbo.Vacancies", "VacancyLabelTypeId", c => c.Int());
            RenameColumn(table: "dbo.Vacancies", name: "VacancyLabelTypeId", newName: "VacancyLabelType_Id");
            CreateIndex("dbo.Vacancies", "VacancyLabelType_Id");
            AddForeignKey("dbo.Vacancies", "VacancyLabelType_Id", "dbo.VacancyLabelTypes", "Id");
        }
    }
}
