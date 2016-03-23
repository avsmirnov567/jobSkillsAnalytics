namespace JobSkillsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveContentFromVacancyTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Vacancies", "ContentText");
            DropColumn("dbo.Vacancies", "ContentHtml");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vacancies", "ContentHtml", c => c.String());
            AddColumn("dbo.Vacancies", "ContentText", c => c.String());
        }
    }
}
