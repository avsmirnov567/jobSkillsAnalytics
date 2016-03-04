namespace JobSkillsDb.Entities
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
        }

        public virtual DbSet<AprioriSkillSet> AprioriSkillSet { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<Vacancy> Vacancies { get; set; }  

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AprioriSkillSet>()
                .Property(e => e.Support)
                .HasPrecision(18, 0);

            modelBuilder.Entity<AprioriSkillSet>()
                .Property(e => e.Confidence)
                .HasPrecision(18, 0);
        }
    }
}
