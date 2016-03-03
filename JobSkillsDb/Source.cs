namespace JobSkillsDb
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int source_id { get; set; }

        [Required]
        [StringLength(50)]
        public string source_name { get; set; }

        [StringLength(100)]
        public string source_link { get; set; }

        public virtual Vacancies Vacancies { get; set; }
    }
}
