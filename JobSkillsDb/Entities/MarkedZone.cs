using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSkillsDb.Entities
{
    [Table("marked_zones")]
    public class MarkedZone
    {
        [Key]
        [Column("marked_zone_id")]
        public int Id { get; set; }
        
        [Column("index_start")]
        public int IndexStart { get; set; }

        [Column("index_end")]
        public int IndexEnd { get; set; }

        [Column("highlighted_text")]
        public string HighlightedText { get; set; }

        public virtual Vacancy Vacancy { get; set; }

        public virtual Skill Skill { get; set; }
    }
}
