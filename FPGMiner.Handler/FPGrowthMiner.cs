using JobSkillsDb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPGMiner.Handler
{
    public class FPGrowthMiner
    {
        private int minSupport = 3;

        public FPGrowthMiner(int minSupport)
        {
            this.minSupport = minSupport;
        }

        private List<SimplifiedVacancy> SortSkillsInVacancy(List<SimplifiedVacancy> vacancies)
        {
            Dictionary<int, int> skillsCounter = new Dictionary<int, int>();
            foreach(SimplifiedVacancy v in vacancies)
            {
                foreach(int skillId in v.SkillsIds)
                {
                    if (skillsCounter.ContainsKey(skillId))
                    {
                        skillsCounter[skillId]++;
                    }
                    else
                    {
                        skillsCounter.Add(skillId, 1);
                    }
                }
            }
            skillsCounter = skillsCounter.Where(x => x.Value >= minSupport)
                .OrderByDescending(x => x.Value)
                .ToDictionary(x => x.Key, x => x.Value);
            foreach(SimplifiedVacancy v in vacancies)
            {
                v.SkillsIds = v.SkillsIds.Where(s => skillsCounter.ContainsKey(s))
                    .OrderByDescending(s => s)
                    .ToList();
            }
            return vacancies;
        }

        public void test(List<Vacancy> vacs)
        {
            List<SimplifiedVacancy> sv = vacs.Select(v => new SimplifiedVacancy(v)).ToList();
            var sortSkills = SortSkillsInVacancy(sv);
        }
    }
}
