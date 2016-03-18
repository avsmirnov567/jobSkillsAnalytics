using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using JobSkillsDb;
using JobSkillsDb.Entities;

//using Parser;

namespace Apriori
{

    class Program
    {
        static void Main(string[] args)
        {
            var minsupport = (decimal) 0.05;
            var minconfidence = (decimal) 0.05;

            var context = new JobSkillsContext();
            //var givenSkills = context.Skills.ToList();
            //var givenVacancies = context.Vacancies.ToList();

            var givenVacancies = context.Vacancies.Take(5).ToList();
            foreach (var vac in givenVacancies)
            {
                Console.WriteLine("name -> {0} / id -> {1}", vac.Title, vac.Id);
                foreach (var skill in vac.Skills)
                {
                    Console.WriteLine("           -> {0}", skill.Name);
                }
            }

            var givenSkills = new List<Skill>();

            givenSkills.AddRange(givenVacancies.ElementAt(0).Skills);
            givenSkills.AddRange(givenVacancies.ElementAt(1).Skills);

            AprioriProcessTransactions process = new AprioriProcessTransactions(minsupport, minconfidence, givenSkills,
                givenVacancies);
        }
        
    }
}
