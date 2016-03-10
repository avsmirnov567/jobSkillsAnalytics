using System;
using System.Linq;
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
            var minsupport = (decimal) 0.5;
            var minconfidence = (decimal) 0.5;

            var context = new JobSkillsContext();
            var givenSkills = context.Skills.ToList();
            var givenVacancies = context.Vacancies.ToList();

            AprioriProcessTransactions process = new AprioriProcessTransactions(minsupport, minconfidence, givenSkills,
                givenVacancies);
        }
    }
}
