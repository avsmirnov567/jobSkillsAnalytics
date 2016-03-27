namespace JobSkillsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVacancyLabelTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VacancyLabelTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tag = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Vacancies", "VacancyLabelId", c => c.Int(nullable: false));
            AddColumn("dbo.Vacancies", "VacancyLabelType_Id", c => c.Int());
            CreateIndex("dbo.Vacancies", "VacancyLabelType_Id");
            AddForeignKey("dbo.Vacancies", "VacancyLabelType_Id", "dbo.VacancyLabelTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vacancies", "VacancyLabelType_Id", "dbo.VacancyLabelTypes");
            DropIndex("dbo.Vacancies", new[] { "VacancyLabelType_Id" });
            DropColumn("dbo.Vacancies", "VacancyLabelType_Id");
            DropColumn("dbo.Vacancies", "VacancyLabelId");
            DropTable("dbo.VacancyLabelTypes");
        }
    }
}
