namespace JobSkillsDb.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Newskillset")]
    public partial class AprioriSkillSet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AprioriSkillSet()
        {
            Skills = new HashSet<Skill>();
        }

        [Key]
        [Column("newskillset_id")]
        public int Id { get; set; }

        [Column("skillset_number")]
        public int Number { get; set; }

        [Column("skillset_support")]
        public decimal? Support { get; set; }

        [Column("skillset_confidence")]
        public decimal? Confidence { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Skill> Skills { get; set; }
    }
}
