namespace DatabaseContext
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DatabaseModel : DbContext
    {
        public DatabaseModel()
            : base("name=DatabaseModel")
        {
        }

        public virtual DbSet<Newskillset> Newskillset { get; set; }
        public virtual DbSet<Skills> Skills { get; set; }
        public virtual DbSet<Vacancies> Vacancies { get; set; }
        public virtual DbSet<Vacancies_content> Vacancies_content { get; set; }

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

            modelBuilder.Entity<Vacancies_content>()
                .HasMany(e => e.Vacancies)
                .WithRequired(e => e.Vacancies_content)
                .HasForeignKey(e => e.vacancy_info_id)
                .WillCascadeOnDelete(false);
        }
    }
}
