namespace JobSkillsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSkills : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Skills", "Support", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Skills", "Support");
        }
    }
}
