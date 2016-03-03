namespace JobSkillsDb.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Source")]
    public partial class Source
    {
        [Key]        
        [Column("source_id")]
        public int Id { get; set; }

        [Required]
        [Column("source_name")]
        public string Name { get; set; }

        [Column("source_link")]
        public string source_link { get; set; }

        public virtual Vacancy Vacancies { get; set; }
    }
}
