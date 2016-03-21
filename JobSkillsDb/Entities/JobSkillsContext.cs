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
            Database.SetInitializer<JobSkillsContext>(null);
        }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<Vacancy> Vacancies { get; set; }
        public virtual DbSet<MarkedZone> MarkedZones { get; set; }

        public virtual DbSet<SkillToGroupConnection> SkillToGroupConnections { get; set; }

        public virtual DbSet<SkillsGroup> SkillsGroup { get; set; }
        public virtual DbSet<AprioriRule> AprioriRules { get; set; }
        public virtual DbSet<EclatSet> EclatSets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
