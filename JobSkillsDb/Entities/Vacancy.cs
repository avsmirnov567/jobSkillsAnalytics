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

        [Key]
        public int Id { get; set; }

        public int InnerId { get; set; }

        [Required]
        public string Title { get; set; }

        public int? SalaryFrom { get; set; }

        public int? SalaryTo { get; set; }

        public string ContentText { get; set; }

        public string ContentHtml { get; set; }

        [Required]
        public string Link { get; set; }

        public string Employer { get; set; }

        public virtual Source Source { get; set; }        

        public virtual ICollection<Skill> Skills { get; set; }

        public virtual ICollection<MarkedZone> MarkedZones { get; set; }
    }
}
