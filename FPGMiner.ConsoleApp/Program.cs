using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobSkillsDb.Entities;
using System.Data.Entity;
using FPGMiner.Handler;
using System.IO;

namespace FPGMiner.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Vacancy> vacancies;
            using(JobSkillsContext db = new JobSkillsContext())
            {
                vacancies = db.Vacancies
                    .Include(v => v.Skills)
                    .Select(v => new { v.Id , v.Skills})
                    .Where(v => v.Skills.Count>0)
                    .ToList()
                    .Select(v => new Vacancy
                    {
                        Id = v.Id,
                        Skills = v.Skills
                    })
                    .ToList();                   
            }
            FPGrowthMiner miner = new FPGrowthMiner(3);
            miner.BuildTree(vacancies);
            var associations = miner.GetAllAssociations();
            using (StreamWriter sw = new StreamWriter("ass-" + Guid.NewGuid().ToString() + ".txt"))
            {
                foreach (var set in associations)
                {
                    string line = "";
                    foreach (Skill skill in set)
                    {
                        line += "\"" + skill.Name + "\"\t";
                    }
                    sw.WriteLine(line);
                }
            }
        }
    }
}
