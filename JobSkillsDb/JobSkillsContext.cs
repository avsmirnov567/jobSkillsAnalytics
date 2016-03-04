namespace JobSkillsDb
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using JobSkillsDb.Entities;

    public partial class JobSkillsContext : DbContext
    {
        public JobSkillsContext()
            : base("name=JobSkillsContext")
        {
            Database.SetInitializer<JobSkillsContext>(new DropCreateDatabaseAlways<JobSkillsContext>());
        }

        public virtual DbSet<AprioriSkillSet> AprioriSkillSet { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<Source> Source { get; set; }
        public virtual DbSet<Vacancy> Vacancies { get; set; }  

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AprioriSkillSet>()
                .Property(e => e.Support)
                .HasPrecision(18, 0);

            modelBuilder.Entity<AprioriSkillSet>()
                .Property(e => e.Confidence)
                .HasPrecision(18, 0);

            //modelBuilder.Entity<AprioriSkillSet>()
            //    .HasMany(e => e.Skills)
            //    .WithMany(e => e.AprioriSkillSet)
            //    .Map(m => m.ToTable("Newskillset_skill").MapLeftKey("newskillset_id").MapRightKey("skill_id"));

            //modelBuilder.Entity<Skill>()
            //    .HasMany(e => e.Vacancies)
            //    .WithMany(e => e.Skills)
            //    .Map(m => m.ToTable("Skillset").MapLeftKey("skill_id").MapRightKey("skillset_id"));
        }
    }
}
