namespace JobSkillsDb.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Source
    {
        public Source()
        {
            Vacancies = new HashSet<Vacancy>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string SourceLink { get; set; }

        public virtual ICollection<Vacancy> Vacancies { get; set; }
    }
}
