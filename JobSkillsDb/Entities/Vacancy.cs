namespace JobSkillsDb.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Vacancies")]
    public partial class Vacancy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vacancy()
        {
            Skills = new HashSet<Skill>();
            MarkedZones = new HashSet<MarkedZone>();
        }

        [Key]
        [Column("vancancy_id")]
        public int Id { get; set; }

        [Column("vacancy_info_id")]
        public int InnerId { get; set; }

        [Required]
        [Column("vacancy_name")]
        public string Title { get; set; }

        [Column("vacancy_salary_from")]
        public int? SalaryFrom { get; set; }

        [Column("vacancy_salary_to")]
        public int? SalaryTo { get; set; }

        [Column("vacancy_content_text")]
        public string ContentText { get; set; }

        [Column("vacancy_content_html")]
        public string ContentHTML { get; set; }

        [Required]
        [Column("vacancy_link")]
        public string Link { get; set; }

        [Column("vacancy_publisher")]
        public string Employer { get; set; }

        public virtual Source Source { get; set; }        

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Skill> Skills { get; set; }

        public virtual ICollection<MarkedZone> MarkedZones { get; set; }
    }
}
