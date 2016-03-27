using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSkillsDb.Entities
{
    public partial class VacancyContent
    {
        [Key, ForeignKey("Vacancy")]
        public int VacancyId { get; set; }
        public string Text { get; set; }
        public string Html { get; set; }

        public virtual Vacancy Vacancy { get; set; }
    }
}
