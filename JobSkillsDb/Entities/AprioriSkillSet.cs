namespace JobSkillsDb.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AprioriSkillSet
    {
        public AprioriSkillSet()
        {
            Skills = new HashSet<Skill>();
        }

        public int Id { get; set; }

        public int Number { get; set; }

        public decimal? Support { get; set; }

        public decimal? Confidence { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }
    }
}
