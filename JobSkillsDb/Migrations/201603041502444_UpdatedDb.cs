namespace JobSkillsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedDb : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vacancies", "Source_Id", "dbo.Sources");
            DropIndex("dbo.Vacancies", new[] { "Source_Id" });
            AddColumn("dbo.Vacancies", "Currency", c => c.String());
            AddColumn("dbo.Vacancies", "Date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Vacancies", "InnerId", c => c.String());
            CreateIndex("dbo.Skills", "Name", unique: true);
            CreateIndex("dbo.Vacancies", "Link", unique: true);
            DropColumn("dbo.Vacancies", "Source_Id");
            DropTable("dbo.Sources");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Sources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        SourceLink = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Vacancies", "Source_Id", c => c.Int());
            DropIndex("dbo.Vacancies", new[] { "Link" });
            DropIndex("dbo.Skills", new[] { "Name" });
            AlterColumn("dbo.Vacancies", "InnerId", c => c.Int(nullable: false));
            DropColumn("dbo.Vacancies", "Date");
            DropColumn("dbo.Vacancies", "Currency");
            CreateIndex("dbo.Vacancies", "Source_Id");
            AddForeignKey("dbo.Vacancies", "Source_Id", "dbo.Sources", "Id");
        }
    }
}
