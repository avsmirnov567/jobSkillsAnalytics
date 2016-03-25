 namespace JobSkillsDb.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Vacancy
    {
        public Vacancy()
        {
            Skills = new HashSet<Skill>();
            MarkedZones = new HashSet<MarkedZone>();
        }

        public int Id { get; set; }

        public string InnerNumber { get; set; }

        [Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; }

        public int? SalaryFrom { get; set; }

        public int? SalaryTo { get; set; }

        public string Currency { get; set; }

        [Required(ErrorMessage = "Link is required")]
        [Index(IsUnique = true)]
        [MaxLength(255)]
        public string Link { get; set; }

        public string Employer { get; set; }

        public DateTime Date { get; set; }

        public int VacancyLabelId { get; set; }

        public virtual VacancyContent VacancyContent { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }

        public virtual ICollection<MarkedZone> MarkedZones { get; set; }

        public virtual VacancyLabelType VacancyLabelType { get; set; }
    }
}
