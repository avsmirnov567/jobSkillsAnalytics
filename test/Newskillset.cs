namespace test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Newskillset")]
    public partial class Newskillset
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Newskillset()
        {
            Skills = new HashSet<Skills>();
        }

        [Key]
        public int newskillset_id { get; set; }

        public int skillset_number { get; set; }

        public decimal? skillset_support { get; set; }

        public decimal? skillset_confidence { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Skills> Skills { get; set; }
    }
}
