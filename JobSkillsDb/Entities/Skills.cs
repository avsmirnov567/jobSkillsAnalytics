namespace JobSkillsDb.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Skill
    {
        public Skill()
        {
            AprioriSkillSet = new HashSet<AprioriSkillSet>();
            Vacancies = new HashSet<Vacancy>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(255)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public virtual ICollection<AprioriSkillSet> AprioriSkillSet { get; set; }

        public virtual ICollection<Vacancy> Vacancies { get; set; }
    }
}
