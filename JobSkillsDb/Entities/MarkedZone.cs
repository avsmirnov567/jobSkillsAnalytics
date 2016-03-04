using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSkillsDb.Entities
{
    public class MarkedZone
    {
        public int Id { get; set; }
        
        public int IndexStart { get; set; }

        public int IndexEnd { get; set; }

        public string HighlightedText { get; set; }

        public virtual Vacancy Vacancy { get; set; }

        public virtual Skill Skill { get; set; }
    }
}
