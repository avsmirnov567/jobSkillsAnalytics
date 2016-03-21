using JobSkillsDb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace FPGMiner.Handler
{
    public class FPGrowthMiner
    {
        private int minSupport;
        private List<Vacancy> originalVacancies;
        private FPGTree tree;

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

        private List<List<int>> GetAssociationsIds(FPGTree tree, int skillId = -1)
        {
            return tree.GetAssociations(minSupport);
        }

        private List<SimplifiedVacancy> SimplifyVacancies(List<Vacancy> vacancies)
        {
            List<SimplifiedVacancy> vacancyVies = vacancies.Select(v => new SimplifiedVacancy(v)).ToList();
            vacancyVies = SortSkillsInVacancy(vacancyVies);
            return vacancyVies;
        }

        private List<Skill> GetAllSkillsFromVacancies(List<Vacancy> vacancies)
        {
            List<Skill> skills = new List<Skill>();
            foreach(Vacancy v in vacancies)
            {
                skills.AddRange(v.Skills.Distinct().Where(s=>!skills.Contains(s)));
            }
            return skills;
        }

        public void BuildTree(List<Vacancy> vacancies)
        {
            originalVacancies = vacancies;
            tree = BuildTree(SimplifyVacancies(originalVacancies));
        }

        public List<List<Skill>> GetAllAssociations()
        {
            List<Skill> allSkills = GetAllSkillsFromVacancies(originalVacancies);
            List<List<int>> associationsIds = GetAssociationsIds(tree);
            List<List<Skill>> associations = new List<List<Skill>>();
            foreach (List<int> frequentSet in associationsIds)
            {
                List<Skill> set = frequentSet.Select(s => allSkills.Single(x => x.Id == s)).ToList();
                if (set.Count > 0)
                {
                    associations.Add(set);
                }
            }
            return associations;
        }
    }
}
