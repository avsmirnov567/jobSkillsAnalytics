using JobSkillsDb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPGMiner.Handler
{
    internal class SimplifiedVacancy
    {
        public int VacancyId { get; set; }
        public List<int> SkillsIds { get; set; }

        public SimplifiedVacancy(Vacancy vacancy)
        {
            VacancyId = vacancy.Id;
            SkillsIds = vacancy.Skills.Select(s => s.Id).ToList();
        }
    }
}
