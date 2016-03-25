namespace JobSkillsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropOldForeignKey : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Vacancies", "VacancyLabelId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vacancies", "VacancyLabelId", c => c.Int(nullable: false));
        }
    }
}
