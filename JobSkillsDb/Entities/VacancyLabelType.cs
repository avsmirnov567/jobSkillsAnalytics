using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSkillsDb.Entities
{
    public partial class VacancyLabelType
    {
        public VacancyLabelType()
        {
            Vacancies = new HashSet<Vacancy>();
        }

        public int Id { get; set; }
        public string Tag { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Vacancy> Vacancies { get; set; }
    }
}
