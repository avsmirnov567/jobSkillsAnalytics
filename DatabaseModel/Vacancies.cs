namespace DatabaseModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Vacancies
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vacancies()
        {
            Skills = new HashSet<Skills>();
        }

        [Key]
        public int vancancy_id { get; set; }

        public int vacancy_source_id { get; set; }

        public int vacancy_info_id { get; set; }

        [Required]
        [StringLength(50)]
        public string vacancy_name { get; set; }

        public int? vacancy_salary_from { get; set; }

        public int? vacancy_salary_to { get; set; }

        [StringLength(100)]
        public string vacancy_link { get; set; }

        [StringLength(50)]
        public string vacancy_publisher { get; set; }

        public virtual Vacancies_content Vacancies_content { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Skills> Skills { get; set; }
    }
}
