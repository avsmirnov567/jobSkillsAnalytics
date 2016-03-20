using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobSkillsDb.Entities;
using System.Data.Entity;
using FPGMiner.Handler;

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
                    .ToList()
                    .Select(v => new Vacancy
                    {
                        Id = v.Id,
                        Skills = v.Skills
                    })
                    .ToList();                   
            }
            FPGrowthMiner miner = new FPGrowthMiner(3);
            miner.test(vacancies);
        }
    }
}
