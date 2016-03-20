using JobSkillsDb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FPGMiner.Handler
{
    public class FPGrowthMiner
    {
        private double minSupport;

        public FPGrowthMiner(double minSupport)
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
            int threshold = (int)Math.Round(skillsCounter.Keys.Count() * minSupport);
            skillsCounter = skillsCounter.Where(x => x.Value >= threshold)
                .ToDictionary(x => x.Key, x => x.Value);
            foreach(SimplifiedVacancy v in vacancies)
            {
                v.SkillsIds = v.SkillsIds.Where(s => skillsCounter.ContainsKey(s))                    
                    .ToList();
                v.SkillsIds.Sort((l, r) =>
                {
                    int countL = skillsCounter[l];
                    int countR = skillsCounter[r];
                    return -countL.CompareTo(countR);
                });
            }
            return vacancies.Where(v => v.SkillsIds.Count > 0).ToList();
        }

        private FPGTree BuildTree(List<SimplifiedVacancy> transactions)
        {
            FPGTree tree = new FPGTree();
            foreach(var vacancy in transactions)
            {
                tree.Add(vacancy);
            }
            return tree;
        }

        private List<List<int>> GetAssociations(FPGTree tree, int skillId = -1)
        {
            tree.GetNodes
        }

        public void test(List<Vacancy> vacs)
        {
            List<SimplifiedVacancy> vacancies = vacs.Select(v => new SimplifiedVacancy(v)).ToList();
            vacancies = SortSkillsInVacancy(vacancies);
            //using (StreamWriter sw = new StreamWriter("vac-" + Guid.NewGuid().ToString() + ".txt"))
            //{
            //    foreach(var v in vacancies)
            //    {
            //        string line = v.VacancyId+":\t";
            //        foreach(var s in v.SkillsIds)
            //        {
            //            line += s + "\t";
            //        }
            //        sw.WriteLine(line);
            //    }
            //}
            FPGTree tree = BuildTree(vacancies);
            int cSharpId = 720;
        }
    }
}
