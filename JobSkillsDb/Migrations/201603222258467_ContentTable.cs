namespace JobSkillsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContentTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VacancyContents",
                c => new
                    {
                        VacancyId = c.Int(nullable: false),
                        Text = c.String(),
                        Html = c.String(),
                    })
                .PrimaryKey(t => t.VacancyId)
                .ForeignKey("dbo.Vacancies", t => t.VacancyId)
                .Index(t => t.VacancyId);            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VacancyContents", "VacancyId", "dbo.Vacancies");
            DropIndex("dbo.VacancyContents", new[] { "VacancyId" });
            DropTable("dbo.VacancyContents");
        }
    }
}
