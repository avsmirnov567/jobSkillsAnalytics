using JobSkillsDb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

namespace TextMining.Handler
{
    public class SkillsAnalytics
    {
        public static List<Skill> GetFrequentSkills(double minSupport)
        {
            using (JobSkillsContext db = new JobSkillsContext())
            {                
                List<List<Skill>> skillSets = db.Vacancies.Include(v => v.Skills)
                    .Where(v => v.Skills.Count>0)
                    .Select(v => v.Skills.ToList())                    
                    .ToList();
                int minSupportCount = (int)Math.Round(skillSets.Count * minSupport);
                Dictionary<Skill, int> skillTable = new Dictionary<Skill, int>();
                foreach (List<Skill> set in skillSets)
                {
                    foreach (Skill s in set)
                    {
                        if (!skillTable.ContainsKey(s))
                        {
                            skillTable.Add(s, 1);
                        }
                        else
                        {
                            skillTable[s]++;
                        }
                    }
                }
                return skillTable.Where(s => s.Value >= minSupportCount)
                    .Select(s => s.Key)
                    .ToList();
            }
        }
    }
}
