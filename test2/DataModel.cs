namespace test2
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataModel : DbContext
    {
        public DataModel()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Newskillset> Newskillset { get; set; }
        public virtual DbSet<Skills> Skills { get; set; }
        public virtual DbSet<Source> Source { get; set; }
        public virtual DbSet<Vacancies> Vacancies { get; set; }
        public virtual DbSet<Vacancies_content> Vacancies_content { get; set; }
        public virtual DbSet<database_firewall_rules> database_firewall_rules { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Newskillset>()
                .Property(e => e.skillset_support)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Newskillset>()
                .Property(e => e.skillset_confidence)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Newskillset>()
                .HasMany(e => e.Skills)
                .WithMany(e => e.Newskillset)
                .Map(m => m.ToTable("Newskillset_skill").MapLeftKey("newskillset_id").MapRightKey("skill_id"));

            modelBuilder.Entity<Skills>()
                .HasMany(e => e.Vacancies)
                .WithMany(e => e.Skills)
                .Map(m => m.ToTable("Skillset").MapLeftKey("skill_id").MapRightKey("skillset_id"));

            modelBuilder.Entity<Vacancies>()
                .HasOptional(e => e.Source)
                .WithRequired(e => e.Vacancies);

            modelBuilder.Entity<Vacancies_content>()
                .HasMany(e => e.Vacancies)
                .WithRequired(e => e.Vacancies_content)
                .HasForeignKey(e => e.vacancy_info_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<database_firewall_rules>()
                .Property(e => e.start_ip_address)
                .IsUnicode(false);

            modelBuilder.Entity<database_firewall_rules>()
                .Property(e => e.end_ip_address)
                .IsUnicode(false);
        }
    }
}
